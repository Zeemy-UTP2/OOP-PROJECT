using System;
using System.Collections.Generic;

namespace CinemaBookingLib
{
    // The main class of the library

    public class BookingSystem
    {
        private List<Showtime> showtimes;
        private List<Booking> bookings;
        private int bookingCounter;

        public List<Cinema> Cinemas { get; private set; }

        public BookingSystem()
        {
            Cinemas = new List<Cinema>();
            showtimes = new List<Showtime>();
            bookings = new List<Booking>();
            bookingCounter = 0;
        }

        public void AddCinema(Cinema cinema)
        {
            Cinemas.Add(cinema);
        }

        public void AddShowtime(Showtime showtime)
        {
           
            showtimes.Add(showtime);
        }

        // Finds all showtimes of a movie on a certain date
        public List<Showtime> FindShowtimes(string movieTitle, DateTime date)
        {
            // loop showtimes
            return new List<Showtime>();
        }

      
        // Create the booking for the customer
        public Booking CreateBooking(Customer customer, Showtime showtime, List<string> seatIds)
        {
            
            Booking booking = new Booking(GenerateBookingId(), customer, showtime, new List<Seat>());
            return booking;
        }

        // Cancel booking
        public bool CancelBooking(string bookingId)
        {
            return false;
        }

        // find booking by id
        public Booking GetBooking(string bookingId)
        {
            return null;
        }

        // create id, eg: B1, B2
        private string GenerateBookingId()
        {
            bookingCounter++;
            return "B" + bookingCounter;
        }
    }
}
