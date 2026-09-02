using System.Globalization;

namespace expense_tracker.Helpers
{
    public static class AmountParser
    {
        public static bool TryParseAmount(
            string input,
            out decimal amount)
        {
            amount = 0;

            if (string.IsNullOrWhiteSpace(input))
            {
                return false;
            }

            if (input.Contains('-'))
            {
                return false;
            }

            // Deutsches Format:
            // 1.234,56
            if (input.Contains(','))
            {
                string[] parts = input.Split(',');

                if (parts.Length != 2)
                {
                    return false;
                }

                if (parts[1].Length > 2)
                {
                    return false;
                }

                input = input.Replace(".", "");

                return decimal.TryParse(
                    input,
                    NumberStyles.Number,
                    CultureInfo.GetCultureInfo("de-DE"),
                    out amount
                );
            }

            // Punkt als Dezimaltrennzeichen:
            // 30.99
            if (input.Contains('.'))
            {
                string[] parts = input.Split('.');

                if (parts.Length != 2)
                {
                    return false;
                }

                if (parts[1].Length > 2)
                {
                    return false;
                }

                return decimal.TryParse(
                    input,
                    NumberStyles.Number,
                    CultureInfo.InvariantCulture,
                    out amount
                );
            }

            // Ganze Zahl
            return decimal.TryParse(
                input,
                NumberStyles.Number,
                CultureInfo.InvariantCulture,
                out amount
            );
        }
    }
}