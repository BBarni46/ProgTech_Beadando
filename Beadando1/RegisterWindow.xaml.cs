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
    /// Interaction logic for RegisterWindow.xaml
    /// </summary>
    public partial class RegisterWindow : Window
    {
        private MainWindow _mainWindow;

        public RegisterWindow()
        {
            InitializeComponent();
            DataBase.CreateDataBase();
        }


        private void Login_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text;
            string password = PasswordBox.Password;

            using (var conn = new SQLiteConnection(Database.ConnectionString))
            {
                conn.Open();
                var cmd = new SQLiteCommand("SELECT COUNT(*) FROM users WHERE username = @u AND password = @p", conn);
                cmd.Parameters.AddWithValue("@u", username);
                cmd.Parameters.AddWithValue("@p", password);

                long count = (long)cmd.ExecuteScalar();
                if (count > 0)
                {
                    StatusTextBlock.Text = "Sikeres bejelentkezés!";
                    this.Close();
                }
                else
                {
                    StatusTextBlock.Text = "Hibás felhasználónév vagy jelszó.";
                }
            }
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text;
            string password = PasswordBox.Password;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                StatusTextBlock.Text = "Minden mezőt ki kell tölteni!";
                return;
            }

            using (var conn = new SQLiteConnection(Database.ConnectionString))
            {
                conn.Open();
                var cmd = new SQLiteCommand("INSERT INTO users (username, password) VALUES (@u, @p)", conn);
                cmd.Parameters.AddWithValue("@u", username);
                cmd.Parameters.AddWithValue("@p", password);

                try
                {
                    cmd.ExecuteNonQuery();
                    StatusTextBlock.Text = "Regisztráció sikeres.";
                }
                catch (SQLiteException ex)
                {
                    if (ex.ResultCode == SQLiteErrorCode.Constraint)
                        StatusTextBlock.Text = "Ez a felhasználónév már létezik.";
                    else
                        StatusTextBlock.Text = "Hiba történt a regisztráció során.";
                }
            }
        }
    }
}


