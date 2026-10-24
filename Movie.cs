namespace CinemaBookingLib
{
    // Represents a film with details like title, duration, and age rating.
    public class Movie
    {
        // Gets the display title of the movie.
        public string Title { get; private set; }

        // Gets the total running time in minutes.
        public int DurationMinutes { get; private set; }

        // Gets the movie's genre category.
        public string Genre { get; private set; }

        // Gets the official age classification rating.
        public AgeRating Rating { get; private set; }

        // Initializes a new movie with the specified details.
        public Movie(string title, int durationMinutes, string genre, AgeRating rating)
        {
            Title = title;
            DurationMinutes = durationMinutes;
            Genre = genre;
            Rating = rating;
        }

        // Checks if a viewer meets the age requirement for this movie.
        public bool IsSuitableForAge(int age)
        {
            // Implement logic (Plus18 requires age >= 18, U and P13 allow all).
            return true;
        }
    }
}