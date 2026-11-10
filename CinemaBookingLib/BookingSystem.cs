using System;
using System.Collections.Generic;

namespace CinemaBookingLib
{
    // The main class of the library. Apps use this to add cinemas and showtimes,
    // and to create or cancel bookings.
    public class BookingSystem
    {
        private readonly List<Cinema> cinemas;
        private readonly List<Showtime> showtimes;
        private readonly List<Booking> bookings;
        private int bookingCounter;

        // All cinemas (read-only to apps)
        public IReadOnlyList<Cinema> Cinemas
        {
            get { return cinemas; }
        }

        public BookingSystem()
        {
            cinemas = new List<Cinema>();
            showtimes = new List<Showtime>();
            bookings = new List<Booking>();
            bookingCounter = 0;
        }

        public void AddCinema(Cinema cinema)
        {
            cinemas.Add(cinema);
        }

        // Adds a showtime. Two showtimes cannot use the same hall at the same time.
        public void AddShowtime(Showtime showtime)
        {
            foreach (Showtime other in showtimes)
            {
                bool sameHall = other.Hall == showtime.Hall;
                bool overlaps = showtime.StartTime < other.EndTime && other.StartTime < showtime.EndTime;
                if (sameHall && overlaps)
                {
                    throw new InvalidOperationException(showtime.Hall.Name + " is already used by "
                        + other.Movie.Title + " at " + other.StartTime.ToString("HH:mm") + ".");
                }
            }
            showtimes.Add(showtime);
        }

        // Finds all showtimes of a movie on a certain date
        public List<Showtime> FindShowtimes(string movieTitle, DateTime date)
        {
            List<Showtime> result = new List<Showtime>();
            foreach (Showtime showtime in showtimes)
            {
                bool sameTitle = string.Equals(showtime.Movie.Title, movieTitle, StringComparison.OrdinalIgnoreCase);
                bool sameDate = showtime.StartTime.Date == date.Date;
                if (sameTitle && sameDate)
                {
                    result.Add(showtime);
                }
            }
            return result;
        }

        // Books seats for a customer.
        // Throws SeatUnavailableException if a seat is taken,
        // and InvalidOperationException if the customer is too young for the movie.
        public Booking CreateBooking(Customer customer, Showtime showtime, List<string> seatIds)
        {
            if (seatIds == null || seatIds.Count == 0)
            {
                throw new ArgumentException("Please choose at least one seat.");
            }

            if (!showtime.Movie.IsSuitableForAge(customer.Age))
            {
                throw new InvalidOperationException(showtime.Movie.Title + " is rated 18. "
                    + customer.Name + " is only " + customer.Age + ".");
            }

            // Find each seat in the hall (skip the same seat chosen twice)
            List<Seat> seats = new List<Seat>();
            foreach (string seatId in seatIds)
            {
                Seat seat = showtime.Hall.GetSeat(seatId);
                if (seat == null)
                {
                    throw new SeatUnavailableException(seatId);
                }
                if (!seats.Contains(seat))
                {
                    seats.Add(seat);
                }
            }

            // Throws SeatUnavailableException if any seat is already booked
            showtime.ReserveSeats(seats);

            Booking booking = new Booking(GenerateBookingId(), customer, showtime, seats);
            bookings.Add(booking);
            customer.AddBooking(booking);
            return booking;
        }

        // Cancels a booking. Returns false if the booking id does not exist
        // or the booking is already cancelled.
        public bool CancelBooking(string bookingId)
        {
            Booking booking = GetBooking(bookingId);
            if (booking == null || booking.Status == BookingStatus.Cancelled)
            {
                return false;
            }
            booking.Cancel();
            return true;
        }

        // Finds a booking by id. Returns null if not found.
        public Booking GetBooking(string bookingId)
        {
            foreach (Booking booking in bookings)
            {
                if (booking.BookingId == bookingId)
                {
                    return booking;
                }
            }
            return null;
        }

        // Makes ids like "B1", "B2", "B3"
        private string GenerateBookingId()
        {
            bookingCounter++;
            return "B" + bookingCounter;
        }
    }
}
