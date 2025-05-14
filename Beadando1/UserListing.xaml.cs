using Microsoft.Data.Sqlite;
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
    /// Interaction logic for UserListing.xaml
    /// </summary>  
    public partial class UserListing : Window
    {
        private string connectionString = "Server=localhost;Database=users;Uid=root;Pwd=;";
        public UserListing()
        {
            InitializeComponent();
            LoadUsers();
        }

        private void LoadUsers()
        {
            string connectionString = "Server=localhost;Database=users;Uid=root;Pwd=;";

            using var connection = new MySqlConnection(connectionString);
            connection.Open();

            // A táblanév és oszlopnevek a te struktúrád szerint
            var command = new MySqlCommand("SELECT név, egyenleg FROM felhasználók;", connection);
            using var reader = command.ExecuteReader();

            UserListBox.Items.Clear();

            while (reader.Read())
            {
                string nev = reader.GetString("név");
                double egyenleg = reader.GetDouble("egyenleg");
                UserListBox.Items.Add($"{nev} - {egyenleg} $");
            }
        }

        private void AddBalance_Click(object sender, RoutedEventArgs e)
        {
            ModifyBalance(true);
        }

        // Pénz kivétele
        private void WithdrawBalance_Click(object sender, RoutedEventArgs e)
        {
            ModifyBalance(false);
        }

        // Egyenleg módosítása (feltöltés vagy levonás)
        private void ModifyBalance(bool isAddition)
        {
            if (UserListBox.SelectedItem == null)
            {
                MessageBox.Show("Válassz ki egy felhasználót!");
                return;
            }

            string selectedUser = UserListBox.SelectedItem.ToString().Split('-')[0].Trim(); // Felhasználó neve
            if (!double.TryParse(AmountTextBox.Text, out double amount) || amount <= 0)
            {
                MessageBox.Show("Adj meg érvényes összeget!");
                return;
            }

            // Ha pénzlevonás, akkor az összeget negatívan kezeljük
            if (!isAddition) amount = -amount;

            try
            {
                using var connection = new MySqlConnection(connectionString);
                connection.Open();

                // Aktuális egyenleg lekérése
                var selectCommand = new MySqlCommand("SELECT egyenleg FROM felhasználók WHERE név = @nev", connection);
                selectCommand.Parameters.AddWithValue("@nev", selectedUser);
                double currentBalance = Convert.ToDouble(selectCommand.ExecuteScalar());

                double newBalance = currentBalance + amount;

                // Ellenőrzés, hogy elérhető-e a kiválasztott összeg
                if (newBalance < 0)
                {
                    MessageBox.Show("Nincs elég egyenleg.");
                    return;
                }

                // Egyenleg frissítése az adatbázisban
                var updateCommand = new MySqlCommand("UPDATE felhasználók SET egyenleg = @egyenleg WHERE név = @nev", connection);
                updateCommand.Parameters.AddWithValue("@egyenleg", newBalance);
                updateCommand.Parameters.AddWithValue("@nev", selectedUser);
                updateCommand.ExecuteNonQuery();

                MessageBox.Show($"Új egyenleg: {newBalance} $");
                AmountTextBox.Clear();
                LoadUsers(); // újratöltjük a felhasználókat, hogy frissüljön az egyenleg
            }
            catch
            {
                MessageBox.Show("Adatbázis hiba.");
            }
        }

        // Vissza gomb
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            this.Close(); // bezárja az aktuális ablakot
        }
    }
}

