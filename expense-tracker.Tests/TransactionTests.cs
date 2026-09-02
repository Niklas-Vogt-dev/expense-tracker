using expense_tracker.Models;

namespace expense_tracker.Tests
{
    public class TransactionTests
    {
        [Fact]
        public void Transaction_CanBeCreated()
        {
            Transaction transaction = new Transaction(
                1,
                25.50m,
                new DateTime(2026, 8, 30),
                "Food",
                "Lunch",
                TransactionType.Expense
            );

            Assert.Equal(1, transaction.Id);
            Assert.Equal(25.50m, transaction.Amount);
            Assert.Equal("Food", transaction.Category);
            Assert.Equal("Lunch", transaction.Description);
            Assert.Equal(TransactionType.Expense, transaction.Type);
        }

        [Fact]
        public void Transaction_CanBeIncome()
        {
            Transaction transaction = new Transaction(
                1,
                2500.00m,
                new DateTime(2026, 8, 31),
                "Salary",
                "August salary",
                TransactionType.Income
            );

            Assert.Equal(TransactionType.Income, transaction.Type);
        }

        [Fact]
        public void Transaction_CanHaveEmptyDescription()
        {
            Transaction transaction = new Transaction(
                2,
                10.00m,
                new DateTime(2026, 8, 30),
                "Food",
                "",
                TransactionType.Expense
            );

            Assert.Equal("", transaction.Description);
        }
    }
}
