using System.Collections.Generic;

namespace CinemaBookingLib
{
    // A person who books tickets
    public class Customer
    {
        private readonly List<Booking> bookings;

        public string CustomerId { get; private set; }
        public string Name { get; private set; }
        public string Email { get; private set; }
        public int Age { get; private set; }

        // Booking history (read-only to apps)
        public IReadOnlyList<Booking> Bookings
        {
            get { return bookings; }
        }

        public Customer(string customerId, string name, string email, int age)
        {
            CustomerId = customerId;
            Name = name;
            Email = email;
            Age = age;
            bookings = new List<Booking>();
        }

        // Saves a booking into the history. Internal: only BookingSystem may call this.
        internal void AddBooking(Booking booking)
        {
            bookings.Add(booking);
        }
    }
}
