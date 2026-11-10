namespace CinemaBookingLib
{
    // Base class for all seats.
    public abstract class Seat
    {
        // Row letter "F"
        public char Row { get; private set; }

        // Seat number within the row (12)
        public int Number { get; private set; }

        // Combined ID e.g. F12
        public string SeatId
        {
            get { return Row.ToString() + Number; }
        }

        // People this seat holds
        public virtual int Capacity
        {
            get { return 1; }
        }

        // Sets the row and number
        protected Seat(char row, int number)
        {
            Row = row;
            Number = number;
        }

        // Price for a showtime with the given base price
        public abstract decimal CalculatePrice(decimal basePrice);

        // Seat type name "Premium"
        public abstract string GetSeatTypeName();
    }

    // Regular single seat charged at base price
    public class StandardSeat : Seat
    {
        // Creates a standard seat
        public StandardSeat(char row, int number) : base(row, number) { }

        // A standard seat costs the base price
        public override decimal CalculatePrice(decimal basePrice)
        {
            return basePrice;
        }

        public override string GetSeatTypeName()
        {
            return "Standard";
        }
    }

    // Recliner seat charged at a premium over base price
    public class PremiumSeat : Seat
    {
        // Price multiplier (1.5 = 50% more)
        public decimal PriceMultiplier { get; private set; }

        // Creates a premium seat
        public PremiumSeat(char row, int number, decimal priceMultiplier) : base(row, number)
        {
            PriceMultiplier = priceMultiplier;
        }

        // A premium seat costs the base price times the multiplier
        public override decimal CalculatePrice(decimal basePrice)
        {
            return basePrice * PriceMultiplier;
        }

        public override string GetSeatTypeName()
        {
            return "Premium";
        }
    }

    // Twin seat for two people, sold as one unit at a discount
    public class CoupleSeat : Seat
    {
        // Creates a couple seat
        public CoupleSeat(char row, int number) : base(row, number) { }

        // Holds two people
        public override int Capacity
        {
            get { return 2; }
        }

        // Two seats with 10% off
        public override decimal CalculatePrice(decimal basePrice)
        {
            return basePrice * 2 * 0.9m;
        }

        public override string GetSeatTypeName()
        {
            return "Couple";
        }
    }
}
