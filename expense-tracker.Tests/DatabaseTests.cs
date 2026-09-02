using expense_tracker.Data;
using expense_tracker.Models;

namespace expense_tracker.Tests
{
    public class DatabaseTests
    {
        private string CreateTestDatabase()
        {
            return Path.Combine(
                Path.GetTempPath(),
                $"expense-tracker-test-{Guid.NewGuid()}.db"
            );
        }

        private void Cleanup(string databasePath)
        {
            if (!File.Exists(databasePath))
                return;

            for (int i = 0; i < 5; i++)
            {
                try
                {
                    File.Delete(databasePath);
                    return;
                }
                catch (IOException)
                {
                    Thread.Sleep(100);
                }
            }
        }

        [Fact]
        public void Database_CreatesDatabase()
        {
            string databasePath = CreateTestDatabase();

            try
            {
                Database database = new Database(
                    $"Data Source={databasePath}"
                );

                Assert.True(File.Exists(databasePath));
            }
            finally
            {
                Cleanup(databasePath);
            }
        }

        [Fact]
        public void AddTransaction_AddsTransactionToDatabase()
        {
            string databasePath = CreateTestDatabase();

            try
            {
                Database database = new Database(
                    $"Data Source={databasePath}"
                );

                Transaction transaction = new Transaction(
                    1,
                    25.50m,
                    new DateTime(2026, 8, 30),
                    "Food",
                    "Lunch",
                    TransactionType.Expense
                );

                database.AddTransaction(transaction);

                List<Transaction> transactions =
                    database.GetTransactions();

                Assert.Single(transactions);

                Transaction savedTransaction =
                    transactions[0];

                Assert.Equal(transaction.Id, savedTransaction.Id);
                Assert.Equal(transaction.Amount, savedTransaction.Amount);
                Assert.Equal(transaction.Date, savedTransaction.Date);
                Assert.Equal(transaction.Category, savedTransaction.Category);
                Assert.Equal(transaction.Description, savedTransaction.Description);
                Assert.Equal(transaction.Type, savedTransaction.Type);
            }
            finally
            {
                Cleanup(databasePath);
            }
        }

        [Fact]
        public void AddTransaction_AllowsEmptyDescription()
        {
            string databasePath = CreateTestDatabase();

            try
            {
                Database database = new Database(
                    $"Data Source={databasePath}"
                );

                Transaction transaction = new Transaction(
                    1,
                    10.00m,
                    new DateTime(2026, 8, 30),
                    "Food",
                    "",
                    TransactionType.Expense
                );

                database.AddTransaction(transaction);

                List<Transaction> transactions =
                    database.GetTransactions();

                Assert.Single(transactions);
                Assert.Equal("", transactions[0].Description);
            }
            finally
            {
                Cleanup(databasePath);
            }
        }

        [Fact]
        public void AddTransaction_CanAddMultipleTransactions()
        {
            string databasePath = CreateTestDatabase();

            try
            {
                Database database = new Database(
                    $"Data Source={databasePath}"
                );

                Transaction transaction1 = new Transaction(
                    1,
                    25.50m,
                    new DateTime(2026, 8, 30),
                    "Food",
                    "Lunch",
                    TransactionType.Expense
                );

                Transaction transaction2 = new Transaction(
                    2,
                    2500.00m,
                    new DateTime(2026, 8, 31),
                    "Salary",
                    "August salary",
                    TransactionType.Income
                );

                Transaction transaction3 = new Transaction(
                    3,
                    49.99m,
                    new DateTime(2026, 9, 1),
                    "Entertainment",
                    "Cinema",
                    TransactionType.Expense
                );

                database.AddTransaction(transaction1);
                database.AddTransaction(transaction2);
                database.AddTransaction(transaction3);

                List<Transaction> transactions =
                    database.GetTransactions();

                Assert.Equal(3, transactions.Count);

                Assert.Equal(transaction1.Id, transactions[0].Id);
                Assert.Equal(transaction2.Id, transactions[1].Id);
                Assert.Equal(transaction3.Id, transactions[2].Id);
            }
            finally
            {
                Cleanup(databasePath);
            }
        }

        [Fact]
        public void GetTransactions_ReturnsEmptyListWhenDatabaseIsEmpty()
        {
            string databasePath = CreateTestDatabase();

            try
            {
                Database database = new Database(
                    $"Data Source={databasePath}"
                );

                List<Transaction> transactions =
                    database.GetTransactions();

                Assert.Empty(transactions);
            }
            finally
            {
                Cleanup(databasePath);
            }
        }

        [Fact]
        public void UpdateTransactions_UpdateTransactionInDatabase()
        {
            string databasePath = CreateTestDatabase();

            try
            {
                Database database = new Database(
                    $"Data Source={databasePath}"
                );

                Transaction transaction = new Transaction(
                    1,
                    25.50m,
                    new DateTime(2026, 8, 30),
                    "Food",
                    "Lunch",
                    TransactionType.Expense
                );

                database.AddTransaction(transaction);

                // Transaction ändern
                transaction.Amount = 30.00m;
                transaction.Date = new DateTime(2026, 8, 31);
                transaction.Category = "Entertainment";
                transaction.Description = "Cinema";
                transaction.Type = TransactionType.Expense;

                database.UpdateTransaction(transaction);

                // Aktualisierte Transaction aus der Datenbank laden
                List<Transaction> transactions =
                    database.GetTransactions();

                Assert.Single(transactions);

                Transaction updatedTransaction =
                    transactions[0];

                Assert.Equal(transaction.Id, updatedTransaction.Id);
                Assert.Equal(transaction.Amount, updatedTransaction.Amount);
                Assert.Equal(transaction.Date, updatedTransaction.Date);
                Assert.Equal(transaction.Category, updatedTransaction.Category);
                Assert.Equal(transaction.Description, updatedTransaction.Description);
                Assert.Equal(transaction.Type, updatedTransaction.Type);
            }
            finally
            {
                Cleanup(databasePath);
            }
        }

        [Fact]
        public void DeleteTransaction_DeleteTransactionFromDatabase()
        {
            string databasePath = CreateTestDatabase();

            try
            {
                Database database = new Database(
                    $"Data Source={databasePath}"
                );

                Transaction transaction = new Transaction(
                    1,
                    25.50m,
                    new DateTime(2026, 8, 30),
                    "Food",
                    "Lunch",
                    TransactionType.Expense
                );

                database.AddTransaction(transaction);

                database.DeleteTransaction(transaction);

                List<Transaction> transactions =
                    database.GetTransactions();

                Assert.Empty(transactions);
            }
            finally
            {
                Cleanup(databasePath);
            }
        }

        [Fact]
        public void DeleteTransaction_OnlyDeletesSelectedTransaction()
        {
            string databasePath = CreateTestDatabase();

            try
            {
                Database database = new Database(
                    $"Data Source={databasePath}"
                );

                Transaction transaction1 = new Transaction(
                    1,
                    25.50m,
                    new DateTime(2026, 8, 30),
                    "Food",
                    "Lunch",
                    TransactionType.Expense
                );

                Transaction transaction2 = new Transaction(
                    2,
                    2500.00m,
                    new DateTime(2026, 8, 31),
                    "Salary",
                    "August salary",
                    TransactionType.Income
                );

                database.AddTransaction(transaction1);
                database.AddTransaction(transaction2);

                database.DeleteTransaction(transaction1);

                List<Transaction> transactions =
                    database.GetTransactions();

                Assert.Single(transactions);
                Assert.Equal(transaction2.Id, transactions[0].Id);
            }
            finally
            {
                Cleanup(databasePath);
            }
        }
    }
}