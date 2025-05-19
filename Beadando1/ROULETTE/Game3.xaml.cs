using Beadando1.ADATB;
using Beadando1.BELÉPÉS;
using Beadando1.MUSIC;
using Beadando1.Roulette.Strategy;
using Beadando1.Roulette;
using System;
using System.Windows;

namespace Beadando1
{
    public partial class Game3 : Window
    {
        private readonly GameSelect _gameSelect;
        private readonly RouletteEngine _engine;
        private int _betAmount = 0;

        public Game3(GameSelect gameSelect)
        {
            InitializeComponent();

            if (MusicState.isMuted)
            {
                MuteButton.Content = "🔇";
                MusicState.mediaPlayer.Pause();
            }
            else
            {
                MuteButton.Content = "🔊";
                MusicState.mediaPlayer.Play();
            }

            _gameSelect = gameSelect;
            _engine = new RouletteEngine();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;

            UpdateBalanceLabel();
        }

        private void UpdateBalanceLabel()
        {
            BalanceLabel.Content = $"Egyenleg: {UserSession.Balance} $";
        }

        private void PlaceBetButton_Click(object sender, RoutedEventArgs e)
        {
            // 1) Tét validálása
            if (!int.TryParse(BetTextBox.Text, out _betAmount)
                || _betAmount <= 0
                || _betAmount > UserSession.Balance)
            {
                MessageBox.Show("Érvénytelen tét.");
                return;
            }

            // 2) Stratégia kiválasztása
            IRouletteBetStrategy strategy = null;
            if (NumberRadio.IsChecked == true
                && int.TryParse(NumberTextBox.Text, out int num)
                && num >= 0 && num <= 36)
            {
                strategy = new NumberBetStrategy(num);
            }
            else if (RedRadio.IsChecked == true)
            {
                strategy = new RedBetStrategy();
            }
            else if (BlackRadio.IsChecked == true)
            {
                strategy = new BlackBetStrategy();
            }
            else if (GreenRadio.IsChecked == true)
            {
                strategy = new GreenBetStrategy();
            }

            if (strategy == null)
            {
                MessageBox.Show("Válassz érvényes fogadást.");
                return;
            }

            // 3) Levonjuk a tétet
            UserSession.Balance -= _betAmount;
            UpdateBalanceLabel();

            // 4) Pörgetés (az engine-be most adjuk át a tétet is)
            var result = _engine.PlayRound(strategy, _betAmount);
            if (result?.LandedNumber == null)
            {
                MessageBox.Show("Hiba történt a játékban.");
                return;
            }

            // 5) Eredmény megjelenítése
            bool isGreen = result.LandedNumber.Number == 0;
            string color = isGreen
                ? "Zöld"
                : (result.LandedNumber.IsRed ? "Piros" : "Fekete");
            ResultLabel.Content =
                $"Eredmény: {result.LandedNumber.Number} ({color})";

            // 6) Kifizetés számítása
            if (result.IsWin)
            {
                int multiplier;
                if (isGreen)
                    multiplier = 13;
                else if (strategy is NumberBetStrategy)
                    multiplier = 5;
                else
                    multiplier = 2;  // piros vagy fekete

                int payout = _betAmount * multiplier;
                MessageBox.Show($"Nyertél! Nyeremény: {payout} $");

                UserSession.Balance += payout;
            }
            else
            {
                MessageBox.Show("Vesztettél.");
            }

            // 7) Egyenleg frissítése és mentés
            UpdateBalanceLabel();

            if (!DataBase.UpdateUserBalance(
                    UserSession.Id,
                    UserSession.Balance,
                    out string error))
            {
                MessageBox.Show(error,
                    "Mentési hiba",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            var gs = new GameSelect(_gameSelect._mainWindow)
            {
                Left = this.Left,
                Top = this.Top,
                WindowStartupLocation = WindowStartupLocation.Manual
            };
            gs.Show();
            this.Close();
        }

        private void MuteButton_Click(object sender, RoutedEventArgs e)
        {
            if (MusicState.isMuted)
            {
                MusicState.mediaPlayer.Play();
                MusicState.isMuted = false;
                MuteButton.Content = "🔊";
            }
            else
            {
                MusicState.mediaPlayer.Pause();
                MusicState.isMuted = true;
                MuteButton.Content = "🔇";
            }
        }
    }
}
