using System;
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
                int total = 0;
                foreach (Seat seat in seats)
                {
                    total = total + seat.Capacity;
                }
                return total;
            }
        }

        public Hall(int hallNumber, string name)
        {
            HallNumber = hallNumber;
            Name = name;
            seats = new List<Seat>();
        }

        // Adds a seat. Two seats cannot have the same SeatId.
        public void AddSeat(Seat seat)
        {
            if (GetSeat(seat.SeatId) != null)
            {
                throw new ArgumentException("Seat " + seat.SeatId + " already exists in " + Name + ".");
            }
            seats.Add(seat);
        }

        // Finds a seat by its id, for example "F12". Returns null if not found.
        public Seat GetSeat(string seatId)
        {
            foreach (Seat seat in seats)
            {
                if (seat.SeatId == seatId)
                {
                    return seat;
                }
            }
            return null;
        }
    }
}
