using System;
using System.Collections.Generic;

namespace CinemaBookingLib
{
    // Represents a cinema branch that contains multiple screening halls.
    public class Cinema
    {
        private readonly List<Hall> halls;

        // Gets the name of the cinema branch.
        public string Name { get; private set; }

        // Gets the city or address location.
        public string Location { get; private set; }

        // Gets a read-only list of all halls in this cinema.
        public IReadOnlyList<Hall> Halls
        {
            get { return halls; }
        }

        // Initializes a new cinema with no halls.
        public Cinema(string name, string location)
        {
            Name = name;
            Location = location;
            halls = new List<Hall>();
        }

        // Adds a new hall to the cinema. Two halls cannot have the same number.
        public void AddHall(Hall hall)
        {
            if (GetHall(hall.HallNumber) != null)
            {
                throw new ArgumentException("Hall " + hall.HallNumber + " already exists in " + Name + ".");
            }
            halls.Add(hall);
        }

        // Finds and returns a hall by its number. Returns null if not found.
        public Hall GetHall(int hallNumber)
        {
            foreach (Hall hall in halls)
            {
                if (hall.HallNumber == hallNumber)
                {
                    return hall;
                }
            }
            return null;
        }
    }
}
