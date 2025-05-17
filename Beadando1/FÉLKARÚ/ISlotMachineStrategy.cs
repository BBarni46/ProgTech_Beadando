
namespace Beadando1.FÉLKARÚ

{
    public interface ISlotMachineStrategy
    {
        SpinResult Spin();
    }

        public class SpinResult
        {
            public string[] Symbols { get; set; }
            public decimal Winnings { get; set; }
        }
}

