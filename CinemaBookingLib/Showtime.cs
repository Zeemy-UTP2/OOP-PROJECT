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
            get { return StartTime.AddMinutes(Movie.DurationMinutes); }
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
            if (Hall.GetSeat(seatId) == null)
            {
                return false;
            }
            return !bookedSeatIds.Contains(seatId);
        }

        // Retrieves a list of all seats in the hall that are still available.
        public List<Seat> GetAvailableSeats()
        {
            List<Seat> available = new List<Seat>();
            foreach (Seat seat in Hall.Seats)
            {
                if (IsSeatAvailable(seat.SeatId))
                {
                    available.Add(seat);
                }
            }
            return available;
        }

        // Reserves the selected seats by adding their IDs to the booked list.
        // Checks every seat first, so nothing is reserved if one seat is taken.
        internal void ReserveSeats(List<Seat> seats)
        {
            foreach (Seat seat in seats)
            {
                if (!IsSeatAvailable(seat.SeatId))
                {
                    throw new SeatUnavailableException(seat.SeatId);
                }
            }
            foreach (Seat seat in seats)
            {
                bookedSeatIds.Add(seat.SeatId);
            }
        }

        // Releases previously booked seats back to the available pool.
        internal void ReleaseSeats(List<Seat> seats)
        {
            foreach (Seat seat in seats)
            {
                bookedSeatIds.Remove(seat.SeatId);
            }
        }
    }
}
