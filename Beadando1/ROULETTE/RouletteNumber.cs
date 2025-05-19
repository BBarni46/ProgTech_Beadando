namespace Beadando1.Roulette.Model
{
    public class RouletteNumber
    {
        public int Number { get; set; }
        public bool IsRed => new[] {
            1,3,5,7,9,12,14,16,18,19,21,23,25,27,30,32,34,36
        }.Contains(Number);
        public bool IsEven => Number != 0 && Number % 2 == 0;
    }
}
