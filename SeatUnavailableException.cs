using System;

namespace CinemaBookingLib
{
    // when someone is trying to select a seat that's already taken
    public class SeatUnavailableException : Exception
    {
        public string SeatId { get; private set; }

        public SeatUnavailableException(string seatId) : base("Seat " + seatId + " is not available.")
        {
            SeatId = seatId;
        }
    }
}