using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beadando1.FÉLKARÚ
{
        public class BasicSlotStrategy : ISlotMachineStrategy
        {
            private static readonly string[] symbols = { "🍒", "🍋", "🍉", "🔔", "⭐", "💎", "7️⃣", "🍇" };
            private readonly Random random = new();

            public SpinResult Spin()
            {
                string[] result = new string[3];
                for (int i = 0; i < 3; i++)
                {
                    result[i] = symbols[random.Next(symbols.Length)];
                }

                decimal winnings = CalculateWinnings(result);
                return new SpinResult
                {
                    Symbols = result,
                    Winnings = winnings
                };
            }

            private decimal CalculateWinnings(string[] symbols)
            {
                if (symbols[0] == symbols[1] && symbols[1] == symbols[2])
                {
                    return 50; // hármas kapás
                }
                else if (symbols[0] == symbols[1] || symbols[1] == symbols[2] || symbols[0] == symbols[2])
                {
                    return 10; // kettes kapás
                }

                return 0;
            }
        }
}

