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
                // TODO: add up the Price of every ticket
                return 0m;
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
            // TODO: create one Ticket for each seat using seat.CalculatePrice(showtime.BasePrice)
        }

        // Pays for the booking. Returns true if the payment worked.
        public bool Confirm(IPaymentMethod payment)
        {
            // TODO: call payment.ProcessPayment(TotalAmount), then set Payment and Status = Confirmed
            return false;
        }

        // Cancels the booking, frees the seats and refunds the money
        public void Cancel()
        {
            // TODO: refund if Confirmed, call Showtime.ReleaseSeats, set Status = Cancelled
        }
    }
}
