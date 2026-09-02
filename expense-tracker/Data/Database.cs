using expense_tracker.Models;
using Microsoft.Data.Sqlite;

namespace expense_tracker.Data
{
    public class Database
    {
        private readonly string connectionString;

        public Database(
            string connectionString = "Data Source=expense-tracker.db")
        {
            this.connectionString = connectionString;

            CreateTable();
        }

        private void CreateTable()
        {
            using SqliteConnection connection = new SqliteConnection(connectionString);

            connection.Open();

            string sql = """
                CREATE TABLE IF NOT EXISTS Transactions (
                    Id INTEGER PRIMARY KEY,
                    Amount REAL NOT NULL,
                    Date TEXT NOT NULL,
                    Category TEXT NOT NULL,
                    Description TEXT NOT NULL,
                    Type INTEGER NOT NULL
                );
                """;

            using SqliteCommand command = new SqliteCommand(sql, connection);

            command.ExecuteNonQuery();
        }

        /* Transaktionsfunktionen */
        public void AddTransaction(Transaction transaction)
        {
            using SqliteConnection connection = new SqliteConnection(connectionString);

            connection.Open();

            string sql = """
                INSERT INTO Transactions (Id, Amount, Date, Category, Description, Type)
                VALUES ($id, $amount, $date, $category, $description, $type);
                """;

            using SqliteCommand command = new SqliteCommand(sql, connection);

            command.Parameters.AddWithValue("$id", transaction.Id);
            command.Parameters.AddWithValue("$amount", transaction.Amount);
            command.Parameters.AddWithValue("$date", transaction.Date);
            command.Parameters.AddWithValue("$category", transaction.Category);
            command.Parameters.AddWithValue("$description", transaction.Description);
            command.Parameters.AddWithValue("$type", transaction.Type);

            command.ExecuteNonQuery();
        }

        public List<Transaction> GetTransactions()
        {
            List<Transaction> transactions = new List<Transaction>();

            using (SqliteConnection connection =
                new SqliteConnection(connectionString))
            {
                connection.Open();

                string sql = """
                    SELECT Id, Amount, Date, Category, Description, Type
                    FROM Transactions
                    ORDER BY Id;
                    """;

                using (SqliteCommand command = new SqliteCommand(sql, connection))
                {
                    using (SqliteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Transaction transaction = new Transaction(
                                reader.GetInt32(0),
                                reader.GetDecimal(1),
                                reader.GetDateTime(2),
                                reader.GetString(3),
                                reader.GetString(4),
                                (TransactionType)reader.GetInt32(5)
                            );

                            transactions.Add(transaction);
                        }
                    }
                }
            }

            return transactions;
        }

        public void UpdateTransaction(Transaction transaction)
        {
            using SqliteConnection connection = new SqliteConnection(connectionString);

            connection.Open();

            string sql = """
                UPDATE Transactions
                SET Amount = $amount,
                    Date = $date,
                    Category = $category,
                    Description = $description,
                    Type = $type
                WHERE Id = $id;
                """;

            using SqliteCommand command = new SqliteCommand(sql, connection);

            command.Parameters.AddWithValue("$id", transaction.Id);
            command.Parameters.AddWithValue("$amount", transaction.Amount);
            command.Parameters.AddWithValue("$date", transaction.Date);
            command.Parameters.AddWithValue("$category", transaction.Category);
            command.Parameters.AddWithValue("$description", transaction.Description);
            command.Parameters.AddWithValue("$type", transaction.Type);

            command.ExecuteNonQuery();
        }

        public void DeleteTransaction(Transaction transaction)
        {
            using SqliteConnection connection = new SqliteConnection(connectionString);

            connection.Open();

            string sql = """
                DELETE FROM Transactions
                WHERE Id = $id;
                """;

            using SqliteCommand command = new SqliteCommand(sql, connection);

            command.Parameters.AddWithValue("$id", transaction.Id);

            command.ExecuteNonQuery();
        }

        /* Demo Data */
        public void SeedDemoData()
        {
            if (GetTransactions().Count != 0)
            {
                return;
            }

            List<Transaction> demoTransactions =
            [
                new Transaction(
                    1,
                    1850.00m,
                    DateTime.Today.AddDays(-12),
                    "Lohn",
                    "Gehalt",
                    TransactionType.Income),

                new Transaction(
                    2,
                    720.00m,
                    DateTime.Today.AddDays(-10),
                    "Miete",
                    "Miete",
                    TransactionType.Expense),

                new Transaction(
                    3,
                    48.50m,
                    DateTime.Today.AddDays(-8),
                    "Essen",
                    "Wocheneinkauf",
                    TransactionType.Expense),

                new Transaction(
                    4,
                    35.00m,
                    DateTime.Today.AddDays(-6),
                    "Transport",
                    "Tankfüllung",
                    TransactionType.Expense),

                new Transaction(
                    5,
                    29.99m,
                    DateTime.Today.AddDays(-4),
                    "Entertainment",
                    "Streaming",
                    TransactionType.Expense),

                new Transaction(
                    6,
                    120.00m,
                    DateTime.Today.AddDays(-2),
                    "Shopping",
                    "Kleidung",
                    TransactionType.Expense),

                new Transaction(
                    7,
                    42.30m,
                    DateTime.Today.AddDays(-1),
                    "Essen",
                    "Restaurant",
                    TransactionType.Expense),

                new Transaction(
                    8,
                    18.90m,
                    DateTime.Today,
                    "Sonstiges",
                    "Geschenk",
                    TransactionType.Expense)
            ];

            foreach (Transaction transaction in demoTransactions)
            {
                AddTransaction(transaction);
            }
        }
    }
}