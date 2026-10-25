using CinemaBookingLib;

Check("Under-age customer cannot book an 18+ movie", threwForAge);

// Payment through a booking
Console.WriteLine();
Check("Payment is rejected for a bad card", booking.Confirm(new CardPayment("Roy", "abcd")) == false);
Check("Booking stays pending after a failed payment", booking.Status == BookingStatus.Pending);
Check("Null payment is rejected", booking.Confirm(null) == false);

CardPayment card = new CardPayment("Roy", "4111111111111234");
Check("Booking is confirmed with a valid card", booking.Confirm(card) == true);
Check("Booking status becomes Confirmed", booking.Status == BookingStatus.Confirmed);
Check("Booking remembers its payment method", booking.Payment == card);
Check("A confirmed booking cannot be paid twice", booking.Confirm(new EWalletPayment("GrabPay", "012")) == false);

// Cancel and refund
Console.WriteLine();
Check("Cancelling a confirmed booking works", system.CancelBooking(booking.BookingId) == true);
Check("Cancelled booking has Cancelled status", booking.Status == BookingStatus.Cancelled);
Check("Cancelled seats are available again", show.IsSeatAvailable("A1") && show.IsSeatAvailable("B1"));
Check("The full amount was refunded to the card", card.Refund(1m) == false);
Check("Cancelling twice returns false", system.CancelBooking(booking.BookingId) == false);
Check("Cancelling an unknown booking returns false", system.CancelBooking("B999") == false);

Booking rebooked = system.CreateBooking(customer, show, new List<string> { "A1" });
Check("A cancelled seat can be booked again", rebooked.Status == BookingStatus.Pending);

Booking pending = system.CreateBooking(customer, show, new List<string> { "C1" });
Check("A pending booking can be cancelled", system.CancelBooking(pending.BookingId) == true);
Check("Booking IDs are unique", booking.BookingId != rebooked.BookingId);

Console.WriteLine();
    }

    // ------------------------------------------------------------------
    // PART 3: Checks for the payment classes (Member 5).
    // ------------------------------------------------------------------
    static void RunPaymentChecks()
{
    Console.WriteLine("=== TGV Payment Class Checks ===");
    Console.WriteLine();

    CardPayment card = new CardPayment("Roy", "4111111111111234");
    Check("Card shows only the last four digits", card.MethodName == "Card (**** 1234)");
    Check("Card accepts a positive payment", card.ProcessPayment(40.00m) == true);
    Check("Card rejects RM0 payment", card.ProcessPayment(0m) == false);
    Check("Card rejects a negative payment", card.ProcessPayment(-5.00m) == false);
    Check("Card accepts a partial refund", card.Refund(10.00m) == true);
    Check("Card rejects a refund above the remaining amount", card.Refund(31.00m) == false);
    Check("Card can refund the remaining RM30", card.Refund(30.00m) == true);
    Check("Card rejects a refund after the amount is fully refunded", card.Refund(1.00m) == false);

    // Missing or invalid card details should be rejected without crashing.
    Console.WriteLine();
    CardPayment missingCard = new CardPayment("Roy", null);
    Check("Missing card number shows N/A", missingCard.MethodName == "Card (**** N/A)");
    Check("Missing card number cannot make a payment", missingCard.ProcessPayment(10.00m) == false);
    Check("Card refund is rejected before any payment", missingCard.Refund(1.00m) == false);

    CardPayment letterCard = new CardPayment("Roy", "abcd");
    Check("Card with letters is rejected", letterCard.ProcessPayment(10.00m) == false);

    CardPayment mixedCard = new CardPayment("Roy", "abcd111111111234");
    Check("Card with letters at the start is rejected", mixedCard.ProcessPayment(10.00m) == false);

    CardPayment shortCard = new CardPayment("Roy", "1234");
    Check("Card that is too short is rejected", shortCard.ProcessPayment(10.00m) == false);

    CardPayment spacedCard = new CardPayment("Roy", "4111 1111 1111 1234");
    Check("Card with spaces is accepted", spacedCard.MethodName == "Card (**** 1234)");

    CardPayment noName = new CardPayment(null, "4111111111111234");
    Check("Missing card holder name becomes Unknown", noName.CardHolderName == "Unknown");

    Console.WriteLine();

    EWalletPayment wallet = new EWalletPayment("GrabPay", "012-3456789");
    Check("E-wallet shows its provider", wallet.MethodName == "E-Wallet (GrabPay)");
    Check("E-wallet accepts a positive payment", wallet.ProcessPayment(20.00m) == true);
    Check("E-wallet rejects RM0 payment", wallet.ProcessPayment(0m) == false);
    Check("E-wallet rejects a negative payment", wallet.ProcessPayment(-5.00m) == false);
    Check("E-wallet accepts a partial refund", wallet.Refund(5.00m) == true);
    Check("E-wallet rejects a refund above the remaining amount", wallet.Refund(16.00m) == false);
    Check("E-wallet can refund the remaining RM15", wallet.Refund(15.00m) == true);
    Check("E-wallet rejects a refund after the amount is fully refunded", wallet.Refund(1.00m) == false);

    // Check that missing wallet details are not accepted.
    Console.WriteLine();
    EWalletPayment missingWallet = new EWalletPayment("", "");
    Check("E-wallet rejects missing details", missingWallet.ProcessPayment(10.00m) == false);
    EWalletPayment nullWallet = new EWalletPayment(null, null);
    Check("E-wallet rejects null details", nullWallet.ProcessPayment(10.00m) == false);
}

static void Check(string checkName, bool condition)
{
    if (condition == true)
    {
        Console.WriteLine("PASS: " + checkName);
        passedTests++;
    }
    else
    {
        Console.WriteLine("FAIL: " + checkName);
        failedTests++;
    }
}
}

