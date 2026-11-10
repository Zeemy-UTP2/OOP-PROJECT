using System;
using System.Collections.Generic;

namespace CinemaBookingLib
{
    // A customer's booking of seats for one showtime. The booking owns its tickets (composition).
    public class Booking
    {
        private readonly List<Ticket> tickets;

        public string BookingId { get; private set; }
        public Customer Customer { get; private set; }
        public Showtime Showtime { get; private set; }
        public BookingStatus Status { get; private set; }
        public DateTime BookingTime { get; private set; }
        public IPaymentMethod Payment { get; private set; }

        // One ticket per seat (read-only to apps)
        public IReadOnlyList<Ticket> Tickets
        {
            get { return tickets; }
        }

        // Total price of all tickets
        public decimal TotalAmount
        {
            get
            {
                decimal total = 0m;
                foreach (Ticket ticket in tickets)
                {
                    total = total + ticket.Price;
                }
                return total;
            }
        }

        // Internal: only BookingSystem can create bookings
        internal Booking(string bookingId, Customer customer, Showtime showtime, List<Seat> seats)
        {
            BookingId = bookingId;
            Customer = customer;
            Showtime = showtime;
            Status = BookingStatus.Pending;
            BookingTime = DateTime.Now;
            tickets = new List<Ticket>();

            // One ticket for each seat. Each seat type works out its own price (polymorphism).
            foreach (Seat seat in seats)
            {
                string code = bookingId + "-" + seat.SeatId;
                decimal price = seat.CalculatePrice(showtime.BasePrice);
                tickets.Add(new Ticket(code, seat, price));
            }
        }

        // Pays for the booking. Returns true if the payment worked.
        public bool Confirm(IPaymentMethod payment)
        {
            if (Status != BookingStatus.Pending)
            {
                throw new InvalidOperationException("Booking " + BookingId + " is already " + Status + ".");
            }

            bool paid = payment.ProcessPayment(TotalAmount);
            if (paid)
            {
                Payment = payment;
                Status = BookingStatus.Confirmed;
            }
            return paid;
        }

        // Cancels the booking, frees the seats and refunds the money
        public void Cancel()
        {
            if (Status == BookingStatus.Cancelled)
            {
                throw new InvalidOperationException("Booking " + BookingId + " is already cancelled.");
            }

            if (Status == BookingStatus.Confirmed && Payment != null)
            {
                Payment.Refund(TotalAmount);
            }

            List<Seat> seats = new List<Seat>();
            foreach (Ticket ticket in tickets)
            {
                seats.Add(ticket.Seat);
            }
            Showtime.ReleaseSeats(seats);

            Status = BookingStatus.Cancelled;
        }
    }
}
