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
    /// Interaction logic for ProfileWIndow.xaml
    /// </summary>
    public partial class ProfileWindow : Window
    {
        private string username;
        private string connectionString = "Server=localhost;Database=users;Uid=root;Pwd=;";
        public ProfileWindow(string username) 
        {
            InitializeComponent();
            this.username = username;
            UsernameText.Text = $"Bejelentkezve: {username}";
            LoadBalance();
        }
        private void LoadBalance()
        {
            try
            {
                using var connection = new MySqlConnection(connectionString);
                connection.Open();

                var command = new MySqlCommand("SELECT egyenleg FROM felhasználók WHERE név = @nev", connection);
                command.Parameters.AddWithValue("@nev", username);

                var result = command.ExecuteScalar();
                if (result != null)
                {
                    double balance = Convert.ToDouble(result);
                    BalanceText.Text = $"Egyenleg: {balance} $";
                }
                else
                {
                    BalanceText.Text = "Hiba: Nincs ilyen felhasználó.";
                }
            }
            catch
            {
                BalanceText.Text = "Hiba az egyenleg lekérésekor.";
            }
        }

        private void AddBalance_Click(object sender, RoutedEventArgs e)
        {
            ModifyBalance(isAddition: true);
        }

        private void WithdrawBalance_Click(object sender, RoutedEventArgs e)
        {
            ModifyBalance(isAddition: false);
        }

        private void ModifyBalance(bool isAddition)
        {
            if (!double.TryParse(AmountTextBox.Text.Trim(), out double amount) || amount <= 0)
            {
                MessageBox.Show("Adj meg érvényes összeget!", "Hiba", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!isAddition)
                amount = -amount;

            try
            {
                using var connection = new MySqlConnection(connectionString);
                connection.Open();

                var selectCmd = new MySqlCommand("SELECT egyenleg FROM felhasználók WHERE név = @nev", connection);
                selectCmd.Parameters.AddWithValue("@nev", username);

                double currentBalance = Convert.ToDouble(selectCmd.ExecuteScalar());
                double newBalance = currentBalance + amount;

                if (newBalance < 0)
                {
                    MessageBox.Show("Nincs elég egyenleg!", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var updateCmd = new MySqlCommand("UPDATE felhasználók SET egyenleg = @egyenleg WHERE név = @nev", connection);
                updateCmd.Parameters.AddWithValue("@egyenleg", newBalance);
                updateCmd.Parameters.AddWithValue("@nev", username);
                updateCmd.ExecuteNonQuery();

                MessageBox.Show($"Új egyenleg: {newBalance} $", "Siker", MessageBoxButton.OK, MessageBoxImage.Information);
                AmountTextBox.Clear();
                LoadBalance();
            }
            catch
            {
                MessageBox.Show("Adatbázis hiba!", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
