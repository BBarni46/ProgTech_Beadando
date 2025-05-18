using Beadando1.BELÉPÉS;
using Beadando1.FÉLKARÚ;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for Game2.xaml
    /// </summary>
    public partial class Game2 : Window
    {
        private GameSelect _gameSelect;  // Itt van a mező, osztály szinten kell 
        private ISlotMachineStrategy slotMachineStrategy;
        private readonly string connectionString = "Server=localhost;Database=users;Uid=root;Pwd=;";

        public Game2(GameSelect gameSelect)
        {
            InitializeComponent();
            slotMachineStrategy = new BasicSlotStrategy();
            UpdateBalanceText();
            _gameSelect = gameSelect;
        }
        private void UpdateBalanceText()
        {
            BalanceText.Text = $"Egyenleg: {UserSession.Balance} $";
        }

        private void Spin_Click(object sender, RoutedEventArgs e)
        {
            const decimal costPerSpin = 5;

            if (UserSession.Balance < costPerSpin)
            {
                MessageBox.Show("Nincs elég egyenleged a pörgetéshez!", "Hiba", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            UserSession.Balance -= costPerSpin;

            var result = slotMachineStrategy.Spin();
            Slot1.Text = result.Symbols[0];
            Slot2.Text = result.Symbols[1];
            Slot3.Text = result.Symbols[2];

            decimal winnings = result.Winnings;
            UserSession.Balance += winnings;

            SaveBalanceToDatabase(UserSession.Balance);

            MessageText.Text = winnings > 0
                ? $"Gratulálok! Nyeremény: {winnings} $"
                : "Nem nyertél, próbáld újra!";

            UpdateBalanceText();
        }

        private void SaveBalanceToDatabase(decimal newBalance)
        {
            try
            {
                using var connection = new MySqlConnection(connectionString);
                connection.Open();

                var cmd = new MySqlCommand("UPDATE felhasználók SET egyenleg = @balance WHERE név = @nev", connection);
                cmd.Parameters.AddWithValue("@balance", newBalance);
                cmd.Parameters.AddWithValue("@nev", UserSession.Username);
                cmd.ExecuteNonQuery();
            }
            catch
            {
                MessageBox.Show("Hiba az egyenleg mentésekor!", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}

