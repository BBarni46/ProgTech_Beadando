using System;
using Beadando1.BELÉPÉS;
using Beadando1.Roulette.Model;
using Beadando1.Roulette.Strategy;

namespace Beadando1.Roulette
{
    public class RouletteEngine
    {
        private readonly Random _random = new();

        /// Lefuttat egy kört, és visszaadja a landolt számot, valamint a nyereményt.
        /// A választott fogadási stratégia.
        /// A fogadott összeg.
        public RouletteResult PlayRound(IRouletteBetStrategy strategy, int betAmount)
        {
            int rolled = _random.Next(0, 37);
            var number = new RouletteNumber { Number = rolled };

            bool win = strategy.Evaluate(number);
            int payout = win
                ? strategy.GetPayoutMultiplier() * betAmount
                : 0;

            return new RouletteResult
            {
                LandedNumber = number,
                IsWin = win,
                Winnings = payout
            };
        }
    }
}
