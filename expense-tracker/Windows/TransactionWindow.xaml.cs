using System.Windows;
using System.Windows.Controls;
using expense_tracker.Helpers;
using expense_tracker.Models;

namespace expense_tracker.Windows
{
    public partial class TransactionWindow : Window
    {

        private int id;

        public Transaction? Result { get; private set; }

        public TransactionWindow(int id, Transaction? transaction = null)
        {
            InitializeComponent();

            WindowStartupLocation = WindowStartupLocation.CenterOwner;

            this.id = id;

            DatePicker.SelectedDate = DateTime.Today;

            if (transaction != null)
            {
                TransactionWindowTitle.Text = "Transaktion bearbeiten";

                TxtAmount.Text = transaction.Amount.ToString();

                CmbType.SelectedIndex = (int)transaction.Type;

                CmbCategory.SelectedIndex = transaction.Category switch
                {
                    "Essen" => 0,
                    "Miete" => 1,
                    "Transport" => 2,
                    "Entertainment" => 3,
                    "Shopping" => 4,
                    "Lohn" => 5,
                    "Sonstiges" => 6,
                    _ => 6
                };

                TxtDescription.Text = transaction.Description;

                DatePicker.SelectedDate = transaction.Date;
            }
        }

        /* Button Funktionalitäten */

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            string amountText = TxtAmount.Text.Trim();

            if (!AmountParser.TryParseAmount(
                amountText,
                out decimal amount))
            {
                MessageBox.Show(
                    "Bitte geben Sie einen gültigen Betrag ein.",
                    "Ungültige Eingabe",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );

                return;
            }

            if (amount <= 0)
            {
                MessageBox.Show(
                    "Der Betrag muss größer als 0 sein.",
                    "Ungültige Eingabe",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );

                return;
            }

            TransactionType type = CmbType.SelectedIndex == 0
                ? TransactionType.Income
                : TransactionType.Expense;

            string category = ((ComboBoxItem)CmbCategory.SelectedItem).Content.ToString();

            string description = TxtDescription.Text.Trim();

            DateTime date = DatePicker.SelectedDate ?? DateTime.Today;

            Result = new Transaction(
                id,
                amount,
                date,
                category,
                description,
                type
            );

            DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
