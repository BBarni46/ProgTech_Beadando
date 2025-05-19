using Beadando1.Roulette.Model;

namespace Beadando1.Roulette.Strategy
{
    /// Zöldre (0) fogadás – ritka, de ha jön, nagyot fizet.
    public class GreenBetStrategy : IRouletteBetStrategy
    {
        public bool Evaluate(RouletteNumber landed) =>
            landed.Number == 0;

        public int GetPayoutMultiplier() => 35;
    }
}
