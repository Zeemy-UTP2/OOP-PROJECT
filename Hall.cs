using System.Collections.Generic;

namespace CinemaBookingLib
{
    public class Hall
    {
        private readonly List<Seat> seats;

        public int HallNumber {get; private set;}

        public string Name {get; private set;}

        public IReadOnlyLists<Seat> Seats
        {
            get { return seats; }
        }

        public int TotalCapacity
        {
            get
            {
                return 0;
            }
        }
        public Hall(int hallNumber, string name)
        {
            HallNumber = hallNumber;
            Name = name;
            seats = new Lists<Seat>();
        }

        public void AddSeat(Seat seat)
        {
            seats.Add(seat);
        }
        public Seat GetSeat(string SeatID)
        {
            return null;
        }
    }
}