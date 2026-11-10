using System;
using System.Collections.Generic;
using CinemaBookingLib;

namespace CinemaBookingLib.Demo
{
    // Demo app that shows how to use CinemaBookingLib.
    // The library methods are still dummies, so prices show RM 0 and payments fail for now.
    class Program
    {
        static void Main(string[] args)
        {
            // 1. Create a cinema with one hall and 3 types of seats
            Cinema cinema = new Cinema("Cineplex Ipoh Parade", "Ipoh, Perak");
            Hall hall = new Hall(3, "Hall 3");

            for (int i = 1; i <= 10; i++)
            {
                hall.AddSeat(new StandardSeat('A', i));
            }
            for (int i = 1; i <= 6; i++)
            {
                hall.AddSeat(new PremiumSeat('E', i, 1.5m));
            }
            for (int i = 1; i <= 4; i++)
            {
                hall.AddSeat(new CoupleSeat('F', i));
            }
            cinema.AddHall(hall);

            BookingSystem system = new BookingSystem();
            system.AddCinema(cinema);

            // 2. Add a movie and a showtime
            Movie movie = new Movie("Galactic Drift", 128, "Sci-Fi", AgeRating.P13);
            Showtime show = new Showtime("S001", movie, hall, DateTime.Today.AddHours(21), 15.00m);
            system.AddShowtime(show);

            Console.WriteLine("=== " + cinema.Name + " ===");
            Console.WriteLine(movie.Title + " (" + movie.Rating + ") in " + hall.Name);
            Console.WriteLine("Total seats: " + hall.Seats.Count);

            // 3. Polymorphism: same method, different price for each seat type
            Console.WriteLine();
            Console.WriteLine("Seat prices (base price RM " + show.BasePrice + "):");
            List<Seat> sampleSeats = new List<Seat>();
            sampleSeats.Add(new StandardSeat('A', 1));
            sampleSeats.Add(new PremiumSeat('E', 1, 1.5m));
            sampleSeats.Add(new CoupleSeat('F', 1));

            foreach (Seat seat in sampleSeats)
            {
                Console.WriteLine(seat.GetSeatTypeName() + " seat for " + seat.Capacity +
                                  " person(s): RM " + seat.CalculatePrice(show.BasePrice));
            }

            // 4. A customer books 2 seats
            Customer customer = new Customer("C001", "Aisyah", "aisyah@example.com", 20);
            Console.WriteLine();
            Console.WriteLine("Is seat E3 available? " + show.IsSeatAvailable("E3"));

            try
            {
                List<string> seatIds = new List<string>();
                seatIds.Add("E3");
                seatIds.Add("E4");

                Booking booking = system.CreateBooking(customer, show, seatIds);
                Console.WriteLine("Booking " + booking.BookingId + " created. Status: " + booking.Status);

                // 5. Pay with an e-wallet (interface: any IPaymentMethod can be used)
                IPaymentMethod payment = new EWalletPayment("Touch 'n Go", "012-3456789");
                bool paid = booking.Confirm(payment);
                Console.WriteLine("Pay RM " + booking.TotalAmount + " with " +
                                  payment.MethodName + ": " + paid);

                foreach (Ticket ticket in booking.Tickets)
                {
                    Console.WriteLine("Ticket: " + ticket);
                }

                // 6. Cancel the booking
                bool cancelled = system.CancelBooking(booking.BookingId);
                Console.WriteLine("Cancelled: " + cancelled);
            }
            catch (SeatUnavailableException ex)
            {
                Console.WriteLine("Seat " + ex.SeatId + " is taken.");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine("Booking refused: " + ex.Message);
            }

            // 7. The app can make its own payment class because IPaymentMethod is an interface
            IPaymentMethod fpx = new FpxPayment("Maybank2u");
            Console.WriteLine();
            Console.WriteLine("Our own payment class also works: " + fpx.MethodName);
        }
    }

    // A payment class written by the app, not by the library
    class FpxPayment : IPaymentMethod
    {
        private string bank;

        public FpxPayment(string bank)
        {
            this.bank = bank;
        }

        public string MethodName
        {
            get { return "FPX (" + bank + ")"; }
        }

        public bool ProcessPayment(decimal amount)
        {
            return amount > 0;
        }

        public bool Refund(decimal amount)
        {
            return true;
        }
    }
}
