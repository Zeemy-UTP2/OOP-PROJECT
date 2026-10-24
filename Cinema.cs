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

        // Adds a new hall to the cinema.
        public void AddHall(Hall hall)
        {
            // Reject duplicate HallNumber before adding.
            halls.Add(hall);
        }

        // Finds and returns a hall by its number.
        public Hall GetHall(int hallNumber)
        {
            return null;
        }
    }
}