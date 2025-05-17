using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Beadando1
{
    public partial class Game1 : Window
    {
        private readonly GameSelect _gameSelect;
        private readonly CardFactory _cardFactory = new();
        private readonly List<Card> _playerCards = new();
        private readonly List<Card> _dealerCards = new();
        private readonly Random _random = new();
        private int _betAmount = 0;

        public Game1(GameSelect gameSelect)
        {
            InitializeComponent();
            _gameSelect = gameSelect;

            this.Left = _gameSelect.Left;
            this.Top = _gameSelect.Top;
            this.WindowStartupLocation = WindowStartupLocation.Manual;

            UpdateBalanceLabel();

            HitButton.IsEnabled = false;
            StandButton.IsEnabled = false;
            NewGameButton.IsEnabled = false;
        }

        private void StartNewGame()
        {
            _playerCards.Clear();
            _dealerCards.Clear();
            PlayerCardsPanel.Children.Clear();
            DealerCardsPanel.Children.Clear();

            DealCard(_playerCards, PlayerCardsPanel);
            DealCard(_dealerCards, DealerCardsPanel);
            DealCard(_playerCards, PlayerCardsPanel);
            DealCard(_dealerCards, DealerCardsPanel);

            UpdateScoreLabel(PlayerScoreLabel, _playerCards);
            UpdateScoreLabel(DealerScoreLabel, _dealerCards);

            HitButton.IsEnabled = true;
            StandButton.IsEnabled = true;

            BetTextBox.IsEnabled = false;
            PlaceBetButton.IsEnabled = false;

            NewGameButton.IsEnabled = false;
        }

        private void DealCard(List<Card> hand, StackPanel panel)
        {
            Card card = _cardFactory.CreateRandomCard();
            hand.Add(card);
            panel.Children.Add(CreateCardImage(card));
        }

        private Image CreateCardImage(Card card)
        {
            try
            {
                string projectRoot = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory)!.Parent!.Parent!.Parent!.FullName;
                string path = Path.Combine(projectRoot, "Cards", card.ImageName);

                if (!File.Exists(path))
                    throw new FileNotFoundException($"A kép nem található: {path}");

                BitmapImage image = new BitmapImage(new Uri(path, UriKind.Absolute));
                return new Image { Source = image, Height = 100, Margin = new Thickness(5) };
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Nem sikerült betölteni a képet: {card.ImageName}\nHiba: {ex.Message}", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                return new Image { Height = 100, Margin = new Thickness(5) };
            }
        }

        private void UpdateScoreLabel(Label label, List<Card> hand)
        {
            int score = hand.Sum(card => card.Value);
            label.Content = $"Érték: {score}";
        }

        private void HitButton_Click(object sender, RoutedEventArgs e)
        {
            DealCard(_playerCards, PlayerCardsPanel);
            UpdateScoreLabel(PlayerScoreLabel, _playerCards);

            if (_playerCards.Sum(c => c.Value) > 21)
            {
                EndGame(false);
            }
        }

        private void StandButton_Click(object sender, RoutedEventArgs e)
        {
            while (_dealerCards.Sum(c => c.Value) < 17)
            {
                DealCard(_dealerCards, DealerCardsPanel);
            }

            int playerScore = _playerCards.Sum(c => c.Value);
            int dealerScore = _dealerCards.Sum(c => c.Value);
            UpdateScoreLabel(DealerScoreLabel, _dealerCards);

            if (dealerScore > 21 || playerScore > dealerScore)
                EndGame(true);
            else if (dealerScore == playerScore)
                EndGame(null);
            else
                EndGame(false);
        }

        private void NewGameButton_Click(object sender, RoutedEventArgs e)
        {
            PlayerCardsPanel.Children.Clear();
            DealerCardsPanel.Children.Clear();
            PlayerScoreLabel.Content = "";
            DealerScoreLabel.Content = "";
            BetTextBox.IsEnabled = true;
            PlaceBetButton.IsEnabled = true;

            HitButton.IsEnabled = false;
            StandButton.IsEnabled = false;

            
            NewGameButton.IsEnabled = false;

            _betAmount = 0; 
        }

        private void PlaceBetButton_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(BetTextBox.Text, out int bet) && bet > 0 && bet <= UserSession.Balance)
            {
                _betAmount = bet;
                UserSession.Balance -= bet;
                UpdateBalanceLabel();
                StartNewGame();
            }
            else
            {
                MessageBox.Show("Érvénytelen tét. Az összeg legyen pozitív szám, és ne haladja meg az egyenleged!");
            }
        }

        private void EndGame(bool? playerWon)
        {
            if (playerWon == true)
            {
                MessageBox.Show("Nyertél!");
                UserSession.Balance += _betAmount * 2;
            }
            else if (playerWon == null)
            {
                MessageBox.Show("Döntetlen!");
                UserSession.Balance += _betAmount;
            }
            else
            {
                MessageBox.Show("Vesztettél!");
            }

            UpdateBalanceLabel();

            HitButton.IsEnabled = false;
            StandButton.IsEnabled = false;

            NewGameButton.IsEnabled = true;
            BetTextBox.IsEnabled = true;
            PlaceBetButton.IsEnabled = true;

            if (!DataBase.UpdateUserBalance(UserSession.Id, UserSession.Balance, out string error))
            {
                MessageBox.Show(error, "Mentési hiba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UpdateBalanceLabel()
        {
            BalanceLabel.Content = $"Egyenleg: {UserSession.Balance} $";
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
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            MusicState.mediaPlayer.Stop();

            string projectRoot = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory)!.Parent!.Parent!.Parent!.FullName;
            string menuMusicPath = System.IO.Path.Combine(projectRoot, "Sound", "menu.mp3");
            MusicState.mediaPlayer.Open(new Uri(menuMusicPath, UriKind.Absolute));
            MusicState.mediaPlayer.MediaEnded += (s, e2) => MusicState.mediaPlayer.Position = TimeSpan.Zero;
            if (!MusicState.isMuted)
                MusicState.mediaPlayer.Play();

           
            GameSelect gs = new GameSelect(_gameSelect._mainWindow); 
            gs.Left = this.Left;
            gs.Top = this.Top;
            gs.Show();

            this.Close();
        }
    }

    public class Card
    {
        public string Suit { get; set; }
        public string Rank { get; set; }
        public int Value { get; set; }
        public string ImageName => $"{Rank}_of_{Suit}.png";
    }

    public interface ICardFactory
    {
        Card CreateRandomCard();
    }

    public class CardFactory : ICardFactory
    {
        private readonly Random _random = new();
        private readonly string[] suits = { "hearts", "diamonds", "clubs", "spades" };
        private readonly string[] ranks = { "2", "3", "4", "5", "6", "7", "8", "9", "10", "jack", "queen", "king", "ace" };

        public Card CreateRandomCard()
        {
            string suit = suits[_random.Next(suits.Length)];
            string rank = ranks[_random.Next(ranks.Length)];
            int value = rank switch
            {
                "jack" or "queen" or "king" => 10,
                "ace" => 11,
                _ => int.Parse(rank)
            };
            return new Card { Suit = suit, Rank = rank, Value = value };
        }
    }
}
