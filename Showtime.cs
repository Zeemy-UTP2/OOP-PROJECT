using System;
using System.Collections.Generic;

namespace CinemaBookingLib
{
    // Represents a single screening of a movie in a hall, tracking booked seats.
    public class Showtime
    {
        private readonly HashSet<string> bookedSeatIds;

        // Gets the unique identifier for this showtime.
        public string ShowtimeId { get; private set; }
        
        // Gets the movie being screened.
        public Movie Movie { get; private set; }

        // Gets the hall where the screening takes place.
        public Hall Hall { get; private set; }

        // Gets the date and time when the screening starts.
        public DateTime StartTime { get; private set; }

        // Gets the base price for a standard seat in Ringgit Malaysia (RM).
        public decimal BasePrice { get; private set; }

        // Gets the calculated end time based on the movie's duration.
        public DateTime EndTime
        {
            get
            {
                // Return StartTime.AddMinutes(Movie.DurationInMinutes);
                return StartTime;
            }
        }

        // Initializes a new showtime with an empty list of booked seats.
        public Showtime(string showtimeId, Movie movie, Hall hall, DateTime startTime, decimal basePrice)
        {
            ShowtimeId = showtimeId;
            Movie = movie;
            Hall = hall;
            StartTime = startTime;
            BasePrice = basePrice;
            bookedSeatIds = new HashSet<string>();
        }

        // Checks if a specific seat exists in the hall and is currently unbooked.
        public bool IsSeatAvailable(string seatId)
        {
            // Check if the seat exists in the Hall and is not in the bookedSeatIds.
            return true;
        }

        // Retrieves a list of all seats in the hall that are still available.
        public List<Seat> GetAvailableSeats()
        {
            // Filter Hall.Seats using the IsSeatAvailable method.
            return new List<Seat>();
        }

        // Reserves the selected seats by adding their IDs to the booked list.
        internal void ReserveSeats(List<Seat> seats)
        {
            // Throw SeatUnavailableException if taken, otherwise add IDs to bookedSeatIds.
        }

        // Releases previously booked seats back to the available pool.
        internal void ReleaseSeats(List<Seat> seats)
        {
            // Remove seat IDs from bookedSeatIds.
        }
    }
}