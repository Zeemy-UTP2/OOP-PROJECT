namespace CinemaBookingLib
{
    // One ticket for one seat
    public class Ticket
    {
        public string TicketCode { get; private set; }
        public Seat Seat { get; private set; }
        public decimal Price { get; private set; }

        public Ticket(string ticketCode, Seat seat, decimal price)
        {
            TicketCode = ticketCode;
            Seat = seat;
            Price = price;
        }

        // Example output: "B1-E3 | E3 (Premium) | RM 22.50"
        public override string ToString()
        {
            return TicketCode + " | " + Seat.SeatId + " (" + Seat.GetSeatTypeName() + ") | RM " + Price.ToString("F2");
        }
    }
}
