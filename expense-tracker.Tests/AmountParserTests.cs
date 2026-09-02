using expense_tracker.Helpers;

namespace expense_tracker.Tests
{
    public class AmountParserTests
    {
        [Fact]
        public void TryParseAmount_ParsesWholeNumber()
        {
            bool result = AmountParser.TryParseAmount(
                "30",
                out decimal amount
            );

            Assert.True(result);
            Assert.Equal(30m, amount);
        }

        [Fact]
        public void TryParseAmount_ParsesDecimalWithComma()
        {
            bool result = AmountParser.TryParseAmount(
                "30,99",
                out decimal amount
            );

            Assert.True(result);
            Assert.Equal(30.99m, amount);
        }

        [Fact]
        public void TryParseAmount_ParsesDecimalWithPoint()
        {
            bool result = AmountParser.TryParseAmount(
                "30.99",
                out decimal amount
            );

            Assert.True(result);
            Assert.Equal(30.99m, amount);
        }

        [Fact]
        public void TryParseAmount_ParsesGermanThousandsFormat()
        {
            bool result = AmountParser.TryParseAmount(
                "1.234,56",
                out decimal amount
            );

            Assert.True(result);
            Assert.Equal(1234.56m, amount);
        }

        [Fact]
        public void TryParseAmount_RejectsNegativeAmount()
        {
            bool result = AmountParser.TryParseAmount(
                "-30",
                out decimal amount
            );

            Assert.False(result);
        }

        [Fact]
        public void TryParseAmount_RejectsMoreThanTwoDecimalPlaces()
        {
            bool result = AmountParser.TryParseAmount(
                "30,999",
                out decimal amount
            );

            Assert.False(result);
        }

        [Fact]
        public void TryParseAmount_RejectsInvalidText()
        {
            bool result = AmountParser.TryParseAmount(
                "abc",
                out decimal amount
            );

            Assert.False(result);
        }

        [Fact]
        public void TryParseAmount_RejectsEmptyInput()
        {
            bool result = AmountParser.TryParseAmount(
                "",
                out decimal amount
            );

            Assert.False(result);
        }

        [Fact]
        public void TryParseAmount_RejectsMultipleDecimalPoints()
        {
            bool result = AmountParser.TryParseAmount(
                "30.99.50",
                out decimal amount
            );

            Assert.False(result);
        }

        [Fact]
        public void TryParseAmount_RejectsMoreThanTwoDecimalPlacesWithPoint()
        {
            bool result = AmountParser.TryParseAmount(
                "30.999",
                out decimal amount
            );

            Assert.False(result);
        }
    }
}