using Beadando1.Roulette.Model;

namespace Beadando1.Roulette.Strategy
{
    /// <summary>
    /// Zöldre (0) fogadás – ritka, de ha jön, nagyot fizet.
    /// </summary>
    public class GreenBetStrategy : IRouletteBetStrategy
    {
        public bool Evaluate(RouletteNumber landed) =>
            landed.Number == 0;

        // Zöld tétnél ugyanakkora a kifizetés, mint a számfogadásnál
        public int GetPayoutMultiplier() => 35;
    }
}
