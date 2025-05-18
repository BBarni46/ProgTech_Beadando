using Beadando1.Roulette.Model;

namespace Beadando1.Roulette.Strategy
{
    public interface IRouletteBetStrategy
    {
        bool Evaluate(RouletteNumber landed);
        int GetPayoutMultiplier();
    }
}
