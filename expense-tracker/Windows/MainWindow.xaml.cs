using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using expense_tracker.Data;
using expense_tracker.Models;
using System.Linq;

namespace expense_tracker.Windows
{

    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private int nextId = 1;
        private Database database;
        private AppSettings settings;
        public string UserName { get; set; }
        public ObservableCollection<Transaction> Transactions { get; set; }
        public ICollectionView TransactionsView { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;

            double screenWidth = SystemParameters.WorkArea.Width;
            double screenHeight = SystemParameters.WorkArea.Height;

            this.Width = screenWidth * 0.8;
            this.Height = screenHeight * 0.8;

            database = new Database();
            settings = SettingsManager.Load();

            if (!settings.DemoDataLoaded)
            {
                database.SeedDemoData();

                settings.DemoDataLoaded = true;
                SettingsManager.Save(settings);
            }

            UserName = settings.UserName;

            Transactions = new ObservableCollection<Transaction>(
                database.GetTransactions()
            );

            nextId = Transactions.Count > 0
                ? Transactions.Max(transaction => transaction.Id) + 1
                : 1;

            TransactionsView = CollectionViewSource.GetDefaultView(Transactions);

            TransactionsView.SortDescriptions.Add(
                new SortDescription(
                    nameof(Transaction.Date),
                    ListSortDirection.Descending
                )
            );

            TransactionsView.Filter = FilterTransactions;

            ExpenseCategories = new ObservableCollection<ExpenseCategory>();

            RefreshExpenseCategories();

            DataContext = this;
        }

        /* Einstellungen */
        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow settingsWindow =
                new SettingsWindow(UserName);

            settingsWindow.Owner = this;

            if (settingsWindow.ShowDialog() == true)
            {
                UserName = settingsWindow.UserName;

                settings.UserName = UserName;
                SettingsManager.Save(settings);

                OnPropertyChanged(nameof(UserName));
            }
        }

        /* Suchleiste */

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TransactionsView.Refresh();
        }

        private bool FilterTransactions(object obj)
        {
            if (obj is not Transaction transaction)
            {
                return false;
            }

            string searchText = SearchTextBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(searchText))
            {
                return true;
            }

            return transaction.Category.Contains(
                       searchText,
                       StringComparison.OrdinalIgnoreCase)
                   ||
                   transaction.Description.Contains(
                       searchText,
                       StringComparison.OrdinalIgnoreCase);
        }

        /* Button Funktionalitäten */

        private void AddTransaction_Click(object sender, RoutedEventArgs e)
        {
            TransactionWindow transactionWindow = 
                new TransactionWindow(nextId);

            transactionWindow.Owner = this;

            if (transactionWindow.ShowDialog() == true)
            {
                Transaction? transaction = transactionWindow.Result;

                if (transaction != null)
                {
                    database.AddTransaction(transaction);

                    Transactions.Add(transaction);

                    RefreshExpenseCategories();

                    OnPropertyChanged(nameof(Balance));
                    OnPropertyChanged(nameof(MonthlyExpenses));

                    nextId++;
                }
            }
        }

        private void EditTransaction_Click(object sender, RoutedEventArgs e)
        {
            if (TransactionList.SelectedItem is Transaction transaction)
            {
                TransactionWindow transactionWindow =
                    new TransactionWindow(transaction.Id, transaction);

                transactionWindow.Owner = this;

                if (transactionWindow.ShowDialog() == true)
                {
                    Transaction? updatedTransaction = transactionWindow.Result;

                    if (updatedTransaction != null)
                    {
                        database.UpdateTransaction(updatedTransaction);

                        int index = Transactions.IndexOf(transaction);

                        Transactions[index] = updatedTransaction;

                        RefreshExpenseCategories();

                        OnPropertyChanged(nameof(Balance));
                        OnPropertyChanged(nameof(MonthlyExpenses));
                    }
                }
            }
        }

        private void DeleteTransaction_Click(object sender, RoutedEventArgs e)
        {
            if (TransactionList.SelectedItem is Transaction transaction)
            {
                if (!settings.DontShowDeleteConfirmation)
                {
                    DeleteConfirmationWindow confirmationWindow =
                        new DeleteConfirmationWindow();

                    confirmationWindow.Owner = this;

                    if (confirmationWindow.ShowDialog() != true)
                    {
                        return;
                    }

                    if (confirmationWindow.DontShowAgain)
                    {
                        settings.DontShowDeleteConfirmation = true;
                        SettingsManager.Save(settings);
                    }
                }

                database.DeleteTransaction(transaction);
                Transactions.Remove(transaction);

                RefreshExpenseCategories();

                OnPropertyChanged(nameof(Balance));
                OnPropertyChanged(nameof(MonthlyExpenses));
            }
        }

        /* Kontostand und Monatsausgaben */

        // Kontostand
        public decimal Balance =>
            Transactions.Sum(t =>
                t.Type == TransactionType.Income
                    ? t.Amount
                    : -t.Amount);

        // Monatliche Ausgaben
        private DateTime selectedMonth =
            new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);

        public string SelectedMonth =>
            selectedMonth.ToString("MMMM yyyy");

        public decimal MonthlyExpenses =>
            Transactions
                .Where(t =>
                    t.Type == TransactionType.Expense &&
                    t.Date.Month == selectedMonth.Month &&
                    t.Date.Year == selectedMonth.Year)
                .Sum(t => t.Amount);

        public bool CanGoToNextMonth =>
            selectedMonth < new DateTime(
                DateTime.Today.Year,
                DateTime.Today.Month,
                1);

        // Buttons
        private void PreviousMonth_Click(object sender, RoutedEventArgs e)
        {
            selectedMonth = selectedMonth.AddMonths(-1);

            OnPropertyChanged(nameof(SelectedMonth));
            OnPropertyChanged(nameof(MonthlyExpenses));
            OnPropertyChanged(nameof(CanGoToNextMonth));

            RefreshExpenseCategories();
        }

        private void NextMonth_Click(object sender, RoutedEventArgs e)
        {
            if (!CanGoToNextMonth)
            {
                return;
            }

            selectedMonth = selectedMonth.AddMonths(1);

            OnPropertyChanged(nameof(SelectedMonth));
            OnPropertyChanged(nameof(MonthlyExpenses));
            OnPropertyChanged(nameof(CanGoToNextMonth));

            RefreshExpenseCategories();
        }

        public ObservableCollection<ExpenseCategory> ExpenseCategories { get; set; }
        private decimal GetCategoryExpenses(string category)
        {
            return Transactions
                .Where(t =>
                    t.Type == TransactionType.Expense &&
                    t.Category == category &&
                    t.Date.Month == selectedMonth.Month &&
                    t.Date.Year == selectedMonth.Year)
                .Sum(t => t.Amount);
        }

        private void RefreshExpenseCategories()
        {
            ExpenseCategories.Clear();

            string[] categories =
            {
                "Essen",
                "Miete",
                "Transport",
                "Entertainment",
                "Shopping",
                "Sonstiges"
            };

            foreach (string category in categories)
            {
                decimal amount = GetCategoryExpenses(category);

                double percentage = MonthlyExpenses > 0
                    ? (double)(amount / MonthlyExpenses * 100)
                    : 0;

                ExpenseCategories.Add(
                    new ExpenseCategory
                    {
                        Name = category,
                        Amount = amount,
                        Percentage = percentage
                    });
            }
        }

        /* Automatische Changes */

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(
                this,
                new PropertyChangedEventArgs(propertyName));
        }
    }
}