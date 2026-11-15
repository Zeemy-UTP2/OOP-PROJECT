using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using CinemaBookingLib;

namespace CinemaBookingGUI
{
    // The main window of the app. It uses the classes from CinemaBookingLib
    // to show showtimes, let the user pick seats, pay, and cancel bookings.
    // The whole screen is built in code, so Visual Studio opens this file as code, not in the designer.
    [System.ComponentModel.DesignerCategory("")]
    public class MainForm : Form
    {
        // ---------- Data from our library ----------
        private BookingSystem system;
        private Cinema cinema;
        private List<Showtime> showtimes = new List<Showtime>();
        private List<Booking> myBookings = new List<Booking>();
        private List<string> selectedSeatIds = new List<string>();
        private Showtime currentShowtime;
        private int customerCounter = 0;

        // ---------- Seat colours ----------
        private Color standardColour = Color.Gainsboro;
        private Color premiumColour = Color.Gold;
        private Color coupleColour = Color.LightPink;
        private Color selectedColour = Color.LimeGreen;
        private Color bookedColour = Color.DimGray;

        // ---------- Controls ----------
        private ListBox lstShowtimes;
        private Label lblMovieInfo;
        private ListBox lstBookings;
        private Button btnCancelBooking;

        private GroupBox grpSeats;
        private Panel pnlSeats;

        private TextBox txtName;
        private TextBox txtEmail;
        private NumericUpDown numAge;

        private RadioButton rdoCard;
        private RadioButton rdoWallet;
        private Panel pnlCard;
        private Panel pnlWallet;
        private TextBox txtCardName;
        private TextBox txtCardNumber;
        private ComboBox cboProvider;
        private TextBox txtPhone;

        private Label lblSelected;
        private Label lblTotal;
        private Button btnBook;

        public MainForm()
        {
            Text = "CinemaBookingLib - Ticket Booking";
            Size = new Size(1190, 760);
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Font = new Font("Segoe UI", 9.75f);
            BackColor = Color.WhiteSmoke;

            LoadCinemaData();
            BuildHeader();
            BuildShowtimeSection();
            BuildBookingsSection();
            BuildSeatSection();
            BuildCustomerSection();
            BuildPaymentSection();
            BuildSummarySection();

            // Show the first showtime when the app opens
            lstShowtimes.SelectedIndex = 0;
        }

        // =====================================================================
        //  Sample data: one cinema, two halls, three movies, five showtimes
        // =====================================================================
        private void LoadCinemaData()
        {
            system = new BookingSystem();
            cinema = new Cinema("Cineplex Ipoh Parade", "Ipoh, Perak");

            // Hall 1: rows A-D standard, row E premium recliners, row F couple seats
            Hall hall1 = new Hall(1, "Hall 1");
            for (char row = 'A'; row <= 'D'; row++)
            {
                for (int i = 1; i <= 10; i++)
                {
                    hall1.AddSeat(new StandardSeat(row, i));
                }
            }
            for (int i = 1; i <= 8; i++)
            {
                hall1.AddSeat(new PremiumSeat('E', i, 1.5m));
            }
            for (int i = 1; i <= 5; i++)
            {
                hall1.AddSeat(new CoupleSeat('F', i));
            }

            // Hall 2: smaller hall, rows A-C standard, row D couple seats
            Hall hall2 = new Hall(2, "Hall 2");
            for (char row = 'A'; row <= 'C'; row++)
            {
                for (int i = 1; i <= 8; i++)
                {
                    hall2.AddSeat(new StandardSeat(row, i));
                }
            }
            for (int i = 1; i <= 4; i++)
            {
                hall2.AddSeat(new CoupleSeat('D', i));
            }

            cinema.AddHall(hall1);
            cinema.AddHall(hall2);
            system.AddCinema(cinema);

            Movie galactic = new Movie("Galactic Drift", 128, "Sci-Fi", AgeRating.P13);
            Movie monsoon = new Movie("Monsoon Hearts", 112, "Romance", AgeRating.U);
            Movie nightShift = new Movie("Night Shift", 105, "Horror", AgeRating.Plus18);

            DateTime today = DateTime.Today;
            AddShowtime(new Showtime("S1", galactic, hall1, today.AddHours(14), 15.00m));
            AddShowtime(new Showtime("S2", monsoon, hall2, today.AddHours(15.5), 13.00m));
            AddShowtime(new Showtime("S3", nightShift, hall1, today.AddHours(17), 16.00m));
            AddShowtime(new Showtime("S4", galactic, hall2, today.AddHours(18), 15.00m));
            AddShowtime(new Showtime("S5", nightShift, hall1, today.AddHours(21), 16.00m));

            // Some seats are already sold, so the seat map looks realistic
            Customer earlyBird = new Customer("C0", "Walk-in Customer", "counter@cineplex.my", 30);
            Booking sold1 = system.CreateBooking(earlyBird, showtimes[0], new List<string> { "C4", "C5", "C6", "E2", "F3" });
            sold1.Confirm(new CardPayment("Walk-in Customer", "4111111111111111"));
            Booking sold2 = system.CreateBooking(earlyBird, showtimes[1], new List<string> { "B3", "B4", "D2" });
            sold2.Confirm(new EWalletPayment("GrabPay", "011-1111111"));
        }

        // Adds the showtime to the system and keeps our own list for the ListBox
        private void AddShowtime(Showtime showtime)
        {
            system.AddShowtime(showtime);
            showtimes.Add(showtime);
        }

        // =====================================================================
        //  Building the screen
        // =====================================================================
        private void BuildHeader()
        {
            Panel header = new Panel();
            header.Location = new Point(0, 0);
            header.Size = new Size(1190, 58);
            header.BackColor = Color.FromArgb(32, 33, 60);
            Controls.Add(header);

            Label title = new Label();
            title.Text = cinema.Name.ToUpper();
            title.Font = new Font("Segoe UI", 16f, FontStyle.Bold);
            title.ForeColor = Color.White;
            title.Location = new Point(14, 6);
            title.AutoSize = true;
            header.Controls.Add(title);

            Label subtitle = new Label();
            subtitle.Text = "Ticket Booking  |  " + cinema.Location + "  |  " + DateTime.Today.ToString("dddd, d MMMM yyyy");
            subtitle.ForeColor = Color.FromArgb(200, 200, 220);
            subtitle.Location = new Point(16, 34);
            subtitle.AutoSize = true;
            header.Controls.Add(subtitle);
        }

        private void BuildShowtimeSection()
        {
            GroupBox grp = MakeGroup("1. Choose a showtime", 12, 68, 340, 300);

            lstShowtimes = new ListBox();
            lstShowtimes.Location = new Point(12, 26);
            lstShowtimes.Size = new Size(316, 172);
            lstShowtimes.Font = new Font("Consolas", 10f);
            foreach (Showtime s in showtimes)
            {
                lstShowtimes.Items.Add(s.StartTime.ToString("HH:mm") + "  " + s.Movie.Title.PadRight(15)
                    + " " + RatingText(s.Movie.Rating).PadRight(4) + s.Hall.Name);
            }
            lstShowtimes.SelectedIndexChanged += LstShowtimes_SelectedIndexChanged;
            grp.Controls.Add(lstShowtimes);

            lblMovieInfo = new Label();
            lblMovieInfo.Location = new Point(12, 204);
            lblMovieInfo.Size = new Size(316, 88);
            lblMovieInfo.Font = RegularFont();
            grp.Controls.Add(lblMovieInfo);
        }

        private void BuildBookingsSection()
        {
            GroupBox grp = MakeGroup("My bookings", 12, 376, 340, 330);

            lstBookings = new ListBox();
            lstBookings.Location = new Point(12, 26);
            lstBookings.Size = new Size(316, 244);
            lstBookings.HorizontalScrollbar = true;
            lstBookings.Font = new Font("Segoe UI", 9f);
            grp.Controls.Add(lstBookings);

            btnCancelBooking = new Button();
            btnCancelBooking.Text = "Cancel selected booking";
            btnCancelBooking.Location = new Point(12, 280);
            btnCancelBooking.Size = new Size(316, 36);
            btnCancelBooking.FlatStyle = FlatStyle.Flat;
            btnCancelBooking.BackColor = Color.MistyRose;
            btnCancelBooking.Click += BtnCancelBooking_Click;
            grp.Controls.Add(btnCancelBooking);
        }

        private void BuildSeatSection()
        {
            grpSeats = MakeGroup("2. Choose your seats", 362, 68, 490, 638);

            Label screen = new Label();
            screen.Text = "S C R E E N";
            screen.TextAlign = ContentAlignment.MiddleCenter;
            screen.BackColor = Color.FromArgb(32, 33, 60);
            screen.ForeColor = Color.White;
            screen.Location = new Point(40, 30);
            screen.Size = new Size(410, 24);
            grpSeats.Controls.Add(screen);

            pnlSeats = new Panel();
            pnlSeats.Location = new Point(10, 66);
            pnlSeats.Size = new Size(470, 420);
            grpSeats.Controls.Add(pnlSeats);

            // Legend, with the price of each seat type for the chosen showtime
            AddLegend("Standard", standardColour, 20, 500);
            AddLegend("Premium (recliner)", premiumColour, 20, 530);
            AddLegend("Couple (2 people)", coupleColour, 20, 560);
            AddLegend("Selected", selectedColour, 260, 500);
            AddLegend("Booked", bookedColour, 260, 530);
        }

        private void AddLegend(string text, Color colour, int x, int y)
        {
            Panel box = new Panel();
            box.BackColor = colour;
            box.BorderStyle = BorderStyle.FixedSingle;
            box.Location = new Point(x, y + 2);
            box.Size = new Size(22, 18);
            grpSeats.Controls.Add(box);

            Label label = new Label();
            label.Text = text;
            label.Font = RegularFont();
            label.Location = new Point(x + 30, y);
            label.AutoSize = true;
            grpSeats.Controls.Add(label);
        }

        private void BuildCustomerSection()
        {
            GroupBox grp = MakeGroup("3. Your details", 862, 68, 300, 150);

            AddLabel(grp, "Name", 12, 30);
            txtName = AddTextBox(grp, 80, 27, 205);

            AddLabel(grp, "Email", 12, 66);
            txtEmail = AddTextBox(grp, 80, 63, 205);

            AddLabel(grp, "Age", 12, 102);
            numAge = new NumericUpDown();
            numAge.Font = RegularFont();
            numAge.Location = new Point(80, 99);
            numAge.Size = new Size(70, 25);
            numAge.Minimum = 1;
            numAge.Maximum = 100;
            numAge.Value = 20;
            grp.Controls.Add(numAge);
        }

        private void BuildPaymentSection()
        {
            GroupBox grp = MakeGroup("4. Payment", 862, 226, 300, 200);

            rdoCard = new RadioButton();
            rdoCard.Text = "Card";
            rdoCard.Font = RegularFont();
            rdoCard.Location = new Point(14, 26);
            rdoCard.AutoSize = true;
            rdoCard.Checked = true;
            rdoCard.CheckedChanged += PaymentType_Changed;
            grp.Controls.Add(rdoCard);

            rdoWallet = new RadioButton();
            rdoWallet.Text = "E-Wallet";
            rdoWallet.Font = RegularFont();
            rdoWallet.Location = new Point(110, 26);
            rdoWallet.AutoSize = true;
            rdoWallet.CheckedChanged += PaymentType_Changed;
            grp.Controls.Add(rdoWallet);

            // Card details
            pnlCard = new Panel();
            pnlCard.Location = new Point(4, 58);
            pnlCard.Size = new Size(290, 130);
            grp.Controls.Add(pnlCard);
            AddLabel(pnlCard, "Card holder", 8, 6);
            txtCardName = AddTextBox(pnlCard, 8, 28, 270);
            AddLabel(pnlCard, "Card number", 8, 62);
            txtCardNumber = AddTextBox(pnlCard, 8, 84, 270);

            // E-wallet details
            pnlWallet = new Panel();
            pnlWallet.Location = new Point(4, 58);
            pnlWallet.Size = new Size(290, 130);
            pnlWallet.Visible = false;
            grp.Controls.Add(pnlWallet);
            AddLabel(pnlWallet, "Provider", 8, 6);
            cboProvider = new ComboBox();
            cboProvider.DropDownStyle = ComboBoxStyle.DropDownList;
            cboProvider.Font = RegularFont();
            cboProvider.Items.Add("Touch 'n Go");
            cboProvider.Items.Add("GrabPay");
            cboProvider.Items.Add("Boost");
            cboProvider.SelectedIndex = 0;
            cboProvider.Location = new Point(8, 28);
            cboProvider.Size = new Size(270, 25);
            pnlWallet.Controls.Add(cboProvider);
            AddLabel(pnlWallet, "Phone number", 8, 62);
            txtPhone = AddTextBox(pnlWallet, 8, 84, 270);
        }

        private void BuildSummarySection()
        {
            GroupBox grp = MakeGroup("5. Summary", 862, 434, 300, 272);

            lblSelected = new Label();
            lblSelected.Location = new Point(14, 28);
            lblSelected.Size = new Size(272, 110);
            lblSelected.Font = RegularFont();
            grp.Controls.Add(lblSelected);

            lblTotal = new Label();
            lblTotal.Font = new Font("Segoe UI", 16f, FontStyle.Bold);
            lblTotal.Location = new Point(12, 144);
            lblTotal.Size = new Size(276, 36);
            grp.Controls.Add(lblTotal);

            btnBook = new Button();
            btnBook.Text = "Book && Pay";
            btnBook.Font = new Font("Segoe UI", 11f, FontStyle.Bold);
            btnBook.Location = new Point(14, 196);
            btnBook.Size = new Size(272, 56);
            btnBook.FlatStyle = FlatStyle.Flat;
            btnBook.BackColor = Color.FromArgb(46, 125, 50);
            btnBook.ForeColor = Color.White;
            btnBook.Click += BtnBook_Click;
            grp.Controls.Add(btnBook);
        }

        // =====================================================================
        //  Seat map
        // =====================================================================

        // Creates one button for every seat in the hall of the chosen showtime
        private void DrawSeats()
        {
            pnlSeats.Controls.Clear();

            int seatWidth = 36;
            int seatHeight = 32;
            int gap = 5;
            int top = 4;
            char currentRow = ' ';
            int x = 0;
            int y = top - (seatHeight + 12);

            foreach (Seat seat in currentShowtime.Hall.Seats)
            {
                // A new row starts: move down and work out where the row begins
                if (seat.Row != currentRow)
                {
                    currentRow = seat.Row;
                    y = y + seatHeight + 12;
                    int rowWidth = RowWidth(seat.Row, seatWidth, gap);
                    x = 30 + (420 - rowWidth) / 2;

                    Label rowLabel = new Label();
                    rowLabel.Text = seat.Row.ToString();
                    rowLabel.Font = new Font("Segoe UI", 10f, FontStyle.Bold);
                    rowLabel.Location = new Point(4, y + 6);
                    rowLabel.Size = new Size(22, 20);
                    pnlSeats.Controls.Add(rowLabel);
                }

                // A couple seat is twice as wide because it holds two people
                int width = seatWidth * seat.Capacity + gap * (seat.Capacity - 1);

                Button btn = new Button();
                btn.Text = seat.SeatId;
                btn.Font = new Font("Segoe UI", 7.5f);
                btn.Location = new Point(x, y);
                btn.Size = new Size(width, seatHeight);
                btn.FlatStyle = FlatStyle.Flat;
                btn.Tag = seat;
                btn.Click += SeatButton_Click;
                pnlSeats.Controls.Add(btn);

                x = x + width + gap;
            }

            UpdateSeatColours();
        }

        // Total width of one row of seats, so the row can be centred
        private int RowWidth(char row, int seatWidth, int gap)
        {
            int width = 0;
            foreach (Seat seat in currentShowtime.Hall.Seats)
            {
                if (seat.Row == row)
                {
                    width = width + seatWidth * seat.Capacity + gap * (seat.Capacity - 1) + gap;
                }
            }
            return width - gap;
        }

        // Colours each seat button: booked, selected, or by seat type
        private void UpdateSeatColours()
        {
            foreach (Control control in pnlSeats.Controls)
            {
                Button btn = control as Button;
                if (btn == null)
                {
                    continue;
                }

                Seat seat = (Seat)btn.Tag;
                if (!currentShowtime.IsSeatAvailable(seat.SeatId))
                {
                    btn.BackColor = bookedColour;
                    btn.ForeColor = Color.White;
                    btn.Enabled = false;
                }
                else if (selectedSeatIds.Contains(seat.SeatId))
                {
                    btn.BackColor = selectedColour;
                    btn.ForeColor = Color.Black;
                    btn.Enabled = true;
                }
                else
                {
                    btn.BackColor = ColourForSeat(seat);
                    btn.ForeColor = Color.Black;
                    btn.Enabled = true;
                }
            }
        }

        private Color ColourForSeat(Seat seat)
        {
            if (seat is PremiumSeat)
            {
                return premiumColour;
            }
            if (seat is CoupleSeat)
            {
                return coupleColour;
            }
            return standardColour;
        }

        // =====================================================================
        //  Event handlers
        // =====================================================================
        private void LstShowtimes_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstShowtimes.SelectedIndex < 0)
            {
                return;
            }

            currentShowtime = showtimes[lstShowtimes.SelectedIndex];
            selectedSeatIds.Clear();
            DrawSeats();
            UpdateMovieInfo();
            UpdateSummary();
        }

        private void SeatButton_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            Seat seat = (Seat)btn.Tag;

            // Click once to select, click again to unselect
            if (selectedSeatIds.Contains(seat.SeatId))
            {
                selectedSeatIds.Remove(seat.SeatId);
            }
            else
            {
                selectedSeatIds.Add(seat.SeatId);
            }

            UpdateSeatColours();
            UpdateSummary();
        }

        private void PaymentType_Changed(object sender, EventArgs e)
        {
            pnlCard.Visible = rdoCard.Checked;
            pnlWallet.Visible = rdoWallet.Checked;
        }

        private void BtnBook_Click(object sender, EventArgs e)
        {
            if (selectedSeatIds.Count == 0)
            {
                ShowWarning("Please choose at least one seat.");
                return;
            }
            if (txtName.Text.Trim() == "" || txtEmail.Text.Trim() == "")
            {
                ShowWarning("Please enter your name and email.");
                return;
            }

            if (rdoCard.Checked)
            {
                if (txtCardName.Text.Trim() == "")
                {
                    ShowWarning("Please enter the card holder name.");
                    return;
                }
                if (HasNumber(txtCardName.Text))
                {
                ShowWarning("Card holder name cannot contain numbers.");
                    return;
                }
        }

            customerCounter++;
            Customer customer = new Customer("C" + customerCounter, txtName.Text.Trim(),
                                             txtEmail.Text.Trim(), (int)numAge.Value);

            // Step 1: create the booking. The library checks age and seat availability.
            Booking booking;
            try
            {
                booking = system.CreateBooking(customer, currentShowtime, selectedSeatIds);
            }
            catch (SeatUnavailableException ex)
            {
                ShowError("Sorry, seat " + ex.SeatId + " has already been booked.");
                selectedSeatIds.Remove(ex.SeatId);
                UpdateSeatColours();
                UpdateSummary();
                return;
            }
            catch (InvalidOperationException ex)
            {
                ShowError("Booking refused: " + ex.Message);
                return;
            }

            // Step 2: pay. Card or e-wallet both work because they share the IPaymentMethod interface.
            IPaymentMethod payment = CreatePaymentMethod();
            bool paid = booking.Confirm(payment);

            if (!paid)
            {
                // Payment failed, so cancel the booking to free the seats again
                system.CancelBooking(booking.BookingId);
                if (rdoCard.Checked)
                {
                    ShowError("Payment failed. Please check the card number (13 to 19 digits).");
                }
                else
                {
                    ShowError("Payment failed. Please enter your e-wallet phone number.");
                }
                UpdateSeatColours();
                return;
            }

            // Step 3: success, show the tickets
            myBookings.Add(booking);
            RefreshBookingList();
            MessageBox.Show(ReceiptText(booking), "Booking confirmed",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);

            selectedSeatIds.Clear();
            UpdateSeatColours();
            UpdateMovieInfo();
            UpdateSummary();
        }

        private void BtnCancelBooking_Click(object sender, EventArgs e)
        {
            if (lstBookings.SelectedIndex < 0)
            {
                ShowWarning("Please select a booking to cancel.");
                return;
            }

            Booking booking = myBookings[lstBookings.SelectedIndex];
            if (booking.Status == BookingStatus.Cancelled)
            {
                ShowWarning("Booking " + booking.BookingId + " is already cancelled.");
                return;
            }

            DialogResult answer = MessageBox.Show("Cancel booking " + booking.BookingId + "?\n\nRM "
                + booking.TotalAmount.ToString("F2") + " will be refunded to " + booking.Payment.MethodName + ".",
                "Cancel booking", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (answer != DialogResult.Yes)
            {
                return;
            }

            if (system.CancelBooking(booking.BookingId))
            {
                MessageBox.Show("Booking " + booking.BookingId + " cancelled. The seats are available again.",
                                "Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            RefreshBookingList();
            UpdateSeatColours();
            UpdateMovieInfo();
        }

        // =====================================================================
        //  Helper methods
        // =====================================================================

        // Returns a CardPayment or EWalletPayment depending on the radio button
        private IPaymentMethod CreatePaymentMethod()
        {
            if (rdoCard.Checked)
            {
                return new CardPayment(txtCardName.Text, txtCardNumber.Text);
            }
            return new EWalletPayment(cboProvider.Text, txtPhone.Text);
        }

        private void UpdateMovieInfo()
        {
            Movie movie = currentShowtime.Movie;
            int seatsLeft = currentShowtime.GetAvailableSeats().Count;
            lblMovieInfo.Text = movie.Title + "\n"
                + movie.Genre + "  |  " + movie.DurationMinutes + " min  |  " + RatingText(movie.Rating) + "\n"
                + currentShowtime.StartTime.ToString("HH:mm") + " - " + currentShowtime.EndTime.ToString("HH:mm")
                + "  |  " + currentShowtime.Hall.Name + "\n"
                + "Seats left: " + seatsLeft + " of " + currentShowtime.Hall.Seats.Count
                + "  |  From RM " + currentShowtime.BasePrice.ToString("F2");
        }

        // Shows the chosen seats and the total. Each seat type calculates its own price (polymorphism).
        private void UpdateSummary()
        {
            if (selectedSeatIds.Count == 0)
            {
                lblSelected.Text = "No seats selected.\nClick on the seats to choose them.";
                lblTotal.Text = "Total: RM 0.00";
                return;
            }

            string text = "";
            decimal total = 0m;
            foreach (string seatId in selectedSeatIds)
            {
                Seat seat = currentShowtime.Hall.GetSeat(seatId);
                decimal price = seat.CalculatePrice(currentShowtime.BasePrice);
                total = total + price;
                text = text + seat.SeatId + "  " + seat.GetSeatTypeName() + "  -  RM " + price.ToString("F2") + "\n";
            }
            lblSelected.Text = text;
            lblTotal.Text = "Total: RM " + total.ToString("F2");
        }

        private void RefreshBookingList()
        {
            lstBookings.Items.Clear();
            foreach (Booking b in myBookings)
            {
                lstBookings.Items.Add(b.BookingId + "  " + b.Showtime.Movie.Title + " "
                    + b.Showtime.StartTime.ToString("HH:mm") + "  RM " + b.TotalAmount.ToString("F2")
                    + "  " + b.Status);
            }
        }

        private string ReceiptText(Booking booking)
        {
            string text = "Booking " + booking.BookingId + " is confirmed!\n\n"
                + "Name:   " + booking.Customer.Name + "\n"
                + "Movie:  " + booking.Showtime.Movie.Title + "\n"
                + "Time:   " + booking.Showtime.StartTime.ToString("HH:mm") + ", " + booking.Showtime.Hall.Name + "\n\n"
                + "Tickets:\n";
            foreach (Ticket ticket in booking.Tickets)
            {
                text = text + "  " + ticket.ToString() + "\n";
            }
            text = text + "\nTotal paid: RM " + booking.TotalAmount.ToString("F2")
                + "\nPaid with: " + booking.Payment.MethodName;
            return text;
        }

        private string RatingText(AgeRating rating)
        {
            if (rating == AgeRating.Plus18)
            {
                return "18";
            }
            return rating.ToString();
        }

        private void ShowWarning(string message)
        {
            MessageBox.Show(message, "Please check", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void ShowError(string message)
        {
            MessageBox.Show(message, "Cannot book", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        // Normal (not bold) text for controls inside a section
        private Font RegularFont()
        {
            return new Font("Segoe UI", 9.75f, FontStyle.Regular);
        }

        private GroupBox MakeGroup(string title, int x, int y, int width, int height)
        {
            GroupBox grp = new GroupBox();
            grp.Text = title;
            grp.Font = new Font("Segoe UI", 9.75f, FontStyle.Bold);
            grp.Location = new Point(x, y);
            grp.Size = new Size(width, height);
            grp.BackColor = Color.White;
            Controls.Add(grp);
            return grp;
        }

        private Label AddLabel(Control parent, string text, int x, int y)
        {
            Label label = new Label();
            label.Text = text;
            label.Font = RegularFont();
            label.Location = new Point(x, y);
            label.AutoSize = true;
            parent.Controls.Add(label);
            return label;
        }

        private TextBox AddTextBox(Control parent, int x, int y, int width)
        {
            TextBox box = new TextBox();
            box.Font = RegularFont();
            box.Location = new Point(x, y);
            box.Size = new Size(width, 25);
            parent.Controls.Add(box);
            return box;
        }
    }
}
