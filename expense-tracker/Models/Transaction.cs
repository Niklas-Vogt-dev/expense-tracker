namespace expense_tracker.Models
{
    public enum TransactionType
    {
        Income,
        Expense
    }

    public class Transaction
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public TransactionType Type { get; set; }

        public Transaction(
            int id,
            decimal amount,
            DateTime date,
            string category,
            string description,
            TransactionType type)
        {
            Id = id;
            Amount = amount;
            Date = date;
            Category = category;
            Description = description;
            Type = type;
        }
    }
}
