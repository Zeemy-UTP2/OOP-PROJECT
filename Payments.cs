namespace CinemaBookingLib
{
    // Any class that can take a payment must have these members
    public interface IPaymentMethod
    {
        string MethodName { get; }
        bool ProcessPayment(decimal amount);
        bool Refund(decimal amount);
    }

    // Credit or debit card. Only the last 4 digits are kept for safety.
    public class CardPayment : IPaymentMethod
    {
        private readonly string lastFourDigits;

        public string CardHolderName { get; private set; }

        public string MethodName
        {
            get { return "Card (**** " + lastFourDigits + ")"; }
        }

        public CardPayment(string cardHolderName, string cardNumber)
        {
            CardHolderName = cardHolderName;
            // TODO: keep only the last 4 digits of cardNumber
            lastFourDigits = "0000";
        }

        public bool ProcessPayment(decimal amount)
        {
            // TODO: return true if amount is more than 0
            return false;
        }

        public bool Refund(decimal amount)
        {
            // TODO: return true
            return false;
        }
    }

    // E-wallet such as Touch 'n Go or GrabPay
    public class EWalletPayment : IPaymentMethod
    {
        public string Provider { get; private set; }
        public string PhoneNumber { get; private set; }

        public string MethodName
        {
            get { return Provider + " eWallet"; }
        }

        public EWalletPayment(string provider, string phoneNumber)
        {
            Provider = provider;
            PhoneNumber = phoneNumber;
        }

        public bool ProcessPayment(decimal amount)
        {
            // TODO: return true if amount is more than 0
            return false;
        }

        public bool Refund(decimal amount)
        {
            // TODO: return true
            return false;
        }
    }
}
