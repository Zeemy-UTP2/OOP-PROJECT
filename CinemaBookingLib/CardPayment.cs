using System;
using System.Globalization;

namespace CinemaBookingLib
{
    // This is a simple card-payment simulation 
    
    public class CardPayment : IPaymentMethod
    {
        private string lastFourDigits;
        private bool hasCardNumber;
        private decimal availableRefundAmount = 0m;

        public string CardHolderName { get; private set; }

        public string MethodName
        {
            get
            {
                return "Card (**** " + lastFourDigits + ")";
            }
        }

        public CardPayment(string cardHolderName, string cardNumber)
        {
            // Use a default name if the cardholder name was not provided.
            if (cardHolderName == null || cardHolderName.Trim() == "")
            {
                this.CardHolderName = "Unknown";
            }
            else
            {
                this.CardHolderName = cardHolderName.Trim();
            }

            // Allow spaces and hyphens in the sample number, then check the digits.
            string cleaned = "";
            if (cardNumber != null)
            {
                cleaned = cardNumber.Trim().Replace(" ", "").Replace("-", "");
            }

            if (cleaned.Length >= 13 && cleaned.Length <= 19 && AllDigits(cleaned))
            {
                this.lastFourDigits = cleaned.Substring(cleaned.Length - 4);
                this.hasCardNumber = true;
            }
            else
            {
                this.lastFourDigits = "N/A";
                this.hasCardNumber = false;
            }
        }

        public bool ProcessPayment(decimal amount)
        {
            // A payment needs a positive amount and a usable sample card number.
            if (amount <= 0 || hasCardNumber == false)
            {
                Console.WriteLine("  [Card] Payment failed. Check the card number and amount.");
                return false;
            }
            else
            {
                // Keep track of the amount paid so a later refund can be checked.
                availableRefundAmount = availableRefundAmount + amount;
                Console.WriteLine("  [Card] Charged RM" + FormatMoney(amount) + " to " + CardHolderName);
                return true;
            }
        }

        public bool Refund(decimal amount)
        {
            // Do not allow a zero, negative, or too-large refund.
            if (amount <= 0 || amount > availableRefundAmount)
            {
                Console.WriteLine("  [Card] Refund failed. Check the amount paid and refund amount.");
                return false;
            }
            else
            {
                availableRefundAmount = availableRefundAmount - amount;
                Console.WriteLine("  [Card] Refunded RM" + FormatMoney(amount) + " to " + CardHolderName);
                return true;
            }
        }

        private static bool AllDigits(string text)
        {
            foreach (char c in text)
            {
                if (!char.IsDigit(c))
                {
                    return false;
                }
            }

            return true;
        }

        private static string FormatMoney(decimal amount)
        {
            return amount.ToString("F2", CultureInfo.InvariantCulture);
        }
    }
}
