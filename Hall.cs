using System.Collections.Generic;

namespace CinemaBookingLib
{
    // A cinema hall. The hall owns its seats (composition).
    public class Hall
    {
        private readonly List<Seat> seats;

        public int HallNumber { get; private set; }
        public string Name { get; private set; }

        // All seats in the hall (read-only to apps)
        public IReadOnlyList<Seat> Seats
        {
            get { return seats; }
        }

        // Total number of people the hall can hold
        public int TotalCapacity
        {
            get
            {
                // TODO: add up Capacity of every seat
                return 0;
            }
        }

        public Hall(int hallNumber, string name)
        {
            HallNumber = hallNumber;
            Name = name;
            seats = new List<Seat>();
        }

        public void AddSeat(Seat seat)
        {
            // TODO: do not allow two seats with the same SeatId
            seats.Add(seat);
        }

        // Finds a seat by its id, for example "F12". Returns null if not found.
        public Seat GetSeat(string seatId)
        {
            // TODO: loop through seats and return the matching one
            return null;
        }
    }
}
