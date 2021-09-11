using System.Collections.Generic;
using System.Text;
using Telegram.Bot.Types.ReplyMarkups;

namespace Task11.TBot
{
    public class BuilderMessageContext
    {
        public static Dictionary<string, string> CurrenciesDesc { get; }


        static BuilderMessageContext()
        {
            CurrenciesDesc = BuildCurrenciesDesc();
        }

        private static Dictionary<string, string> BuildCurrenciesDesc()
        {
            Dictionary<string, string> result = new Dictionary<string, string> {
                { "USD", "United States Dollar" },
                { "EUR", "Euro" },
                { "JPY", "Japanese Yen" },
                { "GBP", "Pound Sterling" },
                { "RUB", "Russian Ruble" },
                { "UAH", "Ukrainian Hryvnia" },
                { "CHF", "Swiss Franc" },
                { "AUD", "Australian Dollar" } };
            return result;
        }

        public static ReplyKeyboardMarkup BuildСurrencyKeyboard(string exception)
        {
            ReplyKeyboardMarkup rkm = new ReplyKeyboardMarkup();
            List<KeyboardButton[]> rows = new List<KeyboardButton[]>();
            List<KeyboardButton> columns = new List<KeyboardButton>();

            int i = 1;
            foreach (KeyValuePair<string, string> pair in CurrenciesDesc)
            {
                if (pair.Key == exception)
                    continue;

                columns.Add(new KeyboardButton(pair.Key));
                if (i++ % 4 == 0)
                {
                    rows.Add(columns.ToArray());
                    columns = new List<KeyboardButton>();
                }
            }
            rows.Add(columns.ToArray());

            rkm.Keyboard = rows.ToArray();
            return rkm;
        }

        public static string BuildSelectMessage(string header)
        {
            StringBuilder builder = new StringBuilder(header);
            foreach (KeyValuePair<string, string> pair in CurrenciesDesc)
            {
                builder.Append(pair.Key);
                builder.Append(": ");
                builder.Append(pair.Value);
                builder.Append("\n");
            }
            return builder.ToString();
        }
    }
}
