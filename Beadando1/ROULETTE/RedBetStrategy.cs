using Beadando1.Roulette.Model;

namespace Beadando1.Roulette.Strategy
{
    public class RedBetStrategy : IRouletteBetStrategy
    {
        public bool Evaluate(RouletteNumber landed) => landed.IsRed;
        public int GetPayoutMultiplier() => 1;
    }
}
