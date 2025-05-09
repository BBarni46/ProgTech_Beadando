using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Beadando1
{
    /// <summary>
    /// Interaction logic for Game1.xaml
    /// </summary>
    public partial class Game1 : Window
    {
        private GameSelect _gameSelect;  

        public Game1(GameSelect gameSelect)
        {
            InitializeComponent();
            StartNewGame();
            _gameSelect = gameSelect;

            
            this.Left = _gameSelect.Left;
            this.Top = _gameSelect.Top;
            this.WindowStartupLocation = WindowStartupLocation.Manual;
        }
        private List<string> deck;
        private List<string> playerHand;
        private List<string> dealerHand;

        private Random rng = new Random();


        private void StartNewGame()
        {
            deck = GenerateDeck();
            playerHand = new List<string>();
            dealerHand = new List<string>();

            playerHand.Add(DrawCard());
            playerHand.Add(DrawCard());

            dealerHand.Add(DrawCard());
            dealerHand.Add(DrawCard());

            UpdateUI();
        }

        private List<string> GenerateDeck()
        {
            var suits = new[] { "A", "B", "C", "D" };
            var ranks = new[] { "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K", "A" };

            return suits.SelectMany(suit => ranks.Select(rank => $"{rank}{suit}")).OrderBy(_ => rng.Next()).ToList();
        }

        private string DrawCard()
        {
            if (deck.Count == 0) deck = GenerateDeck(); 
            var card = deck[0];
            deck.RemoveAt(0);
            return card;
        }

        private int CalculateScore(List<string> hand)
        {
            int score = 0;
            int aces = 0;

            foreach (var card in hand)
            {
                string rank = card.Substring(0, card.Length - 1); 

                if (int.TryParse(rank, out int val))
                {
                    score += val;
                }
                else if (rank == "A")
                {
                    score += 11;
                    aces++;
                }
                else
                {
                    score += 10;
                }
            }

            while (score > 21 && aces > 0)
            {
                score -= 10;
                aces--;
            }

            return score;
        }

        private void UpdateUI()
        {
            PlayerCardsText.Text = string.Join(", ", playerHand);
            DealerCardsText.Text = string.Join(", ", dealerHand);

            int playerScore = CalculateScore(playerHand);
            int dealerScore = CalculateScore(dealerHand);

            PlayerScoreText.Text = $"Pontszám: {playerScore}";
            DealerScoreText.Text = $"Pontszám: {dealerScore}";

            if (playerScore > 21)
            {
                MessageBox.Show("Túlhúztál!");
                DisableGame();
            }
        }

        private void HitButton_Click(object sender, RoutedEventArgs e)
        {
            playerHand.Add(DrawCard());
            UpdateUI();
        }

        private void StandButton_Click(object sender, RoutedEventArgs e)
        {
            int dealerScore = CalculateScore(dealerHand);
            while (dealerScore < 17)
            {
                dealerHand.Add(DrawCard());
                dealerScore = CalculateScore(dealerHand);
            }

            int playerScore = CalculateScore(playerHand);
            UpdateUI();

            if (dealerScore > 21 || playerScore > dealerScore)
                MessageBox.Show("Nyertél!");
            else if (playerScore == dealerScore)
                MessageBox.Show("Döntetlen!");
            else
                MessageBox.Show("Vesztettél!");

            DisableGame();
        }

        private void NewGameButton_Click(object sender, RoutedEventArgs e)
        {
            EnableGame();
            StartNewGame();
        }

        private void DisableGame()
        {
            HitButton.IsEnabled = false;
            StandButton.IsEnabled = false;
        }

        private void EnableGame()
        {
            HitButton.IsEnabled = true;
            StandButton.IsEnabled = true;
        }
    }
}

