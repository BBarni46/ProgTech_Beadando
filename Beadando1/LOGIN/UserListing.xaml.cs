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

        // Vissza gomb
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            this.Close(); // bezárja az aktuális ablakot
        }
    }
}

