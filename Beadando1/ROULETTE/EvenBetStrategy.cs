using Beadando1.Roulette.Model;

namespace Beadando1.Roulette.Strategy
{
    public class EvenBetStrategy : IRouletteBetStrategy
    {
        public bool Evaluate(RouletteNumber landed) => landed.IsEven;
        public int GetPayoutMultiplier() => 1;
    }
}
