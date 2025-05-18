using Beadando1.Roulette.Model;

namespace Beadando1.Roulette.Strategy
{
    public class NumberBetStrategy : IRouletteBetStrategy
    {
        private readonly int _chosen;
        public NumberBetStrategy(int chosen) => _chosen = chosen;
        public bool Evaluate(RouletteNumber landed) => landed.Number == _chosen;
        public int GetPayoutMultiplier() => 35;
    }
}
