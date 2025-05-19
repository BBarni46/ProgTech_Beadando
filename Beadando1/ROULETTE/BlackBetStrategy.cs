using Beadando1.Roulette.Model;

namespace Beadando1.Roulette.Strategy
{
    /// <summary>
    /// Fekete mezőkre (1–36 közül a fekete számokra) fogadás – páros kifizetés.
    /// </summary>
    public class BlackBetStrategy : IRouletteBetStrategy
    {
        public bool Evaluate(RouletteNumber landed) =>
            landed.Number != 0 && !landed.IsRed;

        // Fekete tétnél 1:1 a kifizetés
        public int GetPayoutMultiplier() => 1;
    }
}
