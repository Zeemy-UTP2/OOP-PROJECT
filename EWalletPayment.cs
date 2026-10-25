using System;
using System.Globalization;

namespace CinemaBookingLib
{
    // This class simulates an e-wallet payment 

    public class EWalletPayment : IPaymentMethod
    {
        private bool hasWalletDetails;
        private decimal availableRefundAmount = 0m;

        public string Provider { get; private set; }
        public string PhoneNumber { get; private set; }

        public string MethodName
        {
            get
            {
                return "E-Wallet (" + Provider + ")";
            }
        }

        public EWalletPayment(string provider, string phoneNumber)
        {
            // Check that both details have been entered.
            if (provider == null || provider.Trim() == "" ||
                phoneNumber == null || phoneNumber.Trim() == "")
            {
                this.Provider = "N/A";
                this.PhoneNumber = "";
                this.hasWalletDetails = false;
            }
            else
            {
                this.Provider = provider.Trim();
                this.PhoneNumber = phoneNumber.Trim();
                this.hasWalletDetails = true;
            }
        }

        public bool ProcessPayment(decimal amount)
        {
            if (amount <= 0 || hasWalletDetails == false)
            {
                Console.WriteLine("  [E-Wallet] Payment failed. Check the wallet details and amount.");
                return false;
            }
            else
            {
                // Save the amount so the refund method can check it later.
                availableRefundAmount = availableRefundAmount + amount;
                Console.WriteLine("  [E-Wallet] Charged RM" + FormatMoney(amount) + " via " + Provider);
                return true;
            }
        }

        public bool Refund(decimal amount)
        {
            if (amount <= 0 || amount > availableRefundAmount)
            {
                Console.WriteLine("  [E-Wallet] Refund failed. Check the amount paid and refund amount.");
                return false;
            }
            else
            {
                availableRefundAmount = availableRefundAmount - amount;
                Console.WriteLine("  [E-Wallet] Refunded RM" + FormatMoney(amount) + " to " + Provider);
                return true;
            }
        }

        private static string FormatMoney(decimal amount)
        {
            return amount.ToString("F2", CultureInfo.InvariantCulture);
        }
    }
}
