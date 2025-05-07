using Microsoft.Data.Sqlite;
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
            public UserListing()
            {
                InitializeComponent();
                LoadUsers();
            }

            private void LoadUsers()
            {
                using var connection = new SqliteConnection("Data Source=users.db");
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = "SELECT username FROM users;";

                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    string username = reader.GetString(0);
                    UserListBox.Items.Add(username);
                }
            }
        }
}

