namespace CinemaBookingLib
{

    // Base class for all seats. 
 public abstract class Seat
    {
        // Row letter "F"
        public char Row {get; private set; }

        // Seat number within the row (12)
        public int Number {get; private set; }

        // Combined ID e.gF12
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

        // price for a showtume with the given base price
        public abstract decimal CalculatePrice(decimal basePrice);

        // Seat type name "Premium"
        public abstract string GetSeatTypeName();
    }  

    // Regular single seat charged at base price
    public class StandardSeat : Seat
    {
        // creates a standard seat
        public StandardSeat(char row, int number) : base(row, number) {}

        // inheritdoc
        public override decimal CalculatePrice(decimal basePrice)
        {
            // return base price
            return 0m;
        }

        // inheritdoc
        public override string GetSeatTypeName()
        {
            return "Standard";
        }
    } 

    // Recliner seat charged at a premium over base price
    public class PremiumSeat : Seat
    {
        // Price multiplier (1.5 = 50% more)
        public decimal PriceMultiplier {get; private set; }

        // creates a premium seat
        public PremiumSeat(char row, int number, decimal priceMultiplier) : base(row, number)
        {
            PriceMultiplier = priceMultiplier;
        }

        // inheritdoc
        public override decimal CalculatePrice(decimal basePrice)
        {
            // return basePrice * PriceMultiplier
            return 0m;
        }

        // inheritdoc
        public override string GetSeatTypeName()
        {
            return "Premium";
        }
    }

    // Twin seat for two people, sold as one unit at a discount
    public class CoupleSeat : Seat
    {
        // Create a couple seat
        public CoupleSeat(char row, int number) : base(row, number) {}

        // Holds two people
        public override int Capacity
        {
            get { return 2; }

        }

        // inheritdoc
        public override decimal CalculatePrice( decimal basePrice)
        {
            // return basePrice * 2 * 0.9m (10% off)
            return 0m;
        }

         // inheritdoc
        public override string GetSeatTypeName()
        {
            return "Couple";
        }
    }
}
