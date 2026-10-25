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

        public void AddShowtime(Showtime showtime)
        {
            // TODO: do not allow two showtimes in the same hall at the same time
            showtimes.Add(showtime);
        }

        // Finds all showtimes of a movie on a certain date
        public List<Showtime> FindShowtimes(string movieTitle, DateTime date)
        {
            // TODO: loop through showtimes and match the title and date
            return new List<Showtime>();
        }

        // Books seats for a customer.
        // Throws SeatUnavailableException if a seat is taken,
        // and InvalidOperationException if the customer is too young for the movie.
        public Booking CreateBooking(Customer customer, Showtime showtime, List<string> seatIds)
        {
            // TODO: check age, find the seats, reserve them,
            // then save the booking in bookings and in the customer's history
            return new Booking(GenerateBookingId(), customer, showtime, new List<Seat>());
        }

        // Cancels a booking. Returns false if the booking id does not exist.
        public bool CancelBooking(string bookingId)
        {
            // TODO: find the booking with GetBooking, then call Cancel()
            return false;
        }

        // Finds a booking by id. Returns null if not found.
        public Booking GetBooking(string bookingId)
        {
            // TODO: loop through bookings and return the matching one
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
