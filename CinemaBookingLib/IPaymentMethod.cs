namespace CinemaBookingLib
{
    // Both payment types use these same methods.
    public interface IPaymentMethod
    {
        string MethodName { get; }

        bool ProcessPayment(decimal amount);

        bool Refund(decimal amount);
    }
}
