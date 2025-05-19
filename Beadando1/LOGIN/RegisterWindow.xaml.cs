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
using Beadando1.ADATB;
using Beadando1.BELÉPÉS;
using Microsoft.Data.Sqlite;
using MySql.Data.MySqlClient;
using Serilog;
namespace Beadando1
{
    /// <summary>
    /// Interaction logic for RegisterWindow.xaml
    /// </summary>
    public partial class RegisterWindow : Window
    {
        private MainWindow mainWindow;
        public RegisterWindow(MainWindow mainWindow)
        {
            InitializeComponent();
            Log.Information("Regiszter ablak megnyílt megfelelően");
            this.mainWindow = mainWindow;
        }
        private void Login_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text;
            string password = PasswordBox.Password;

            if (DataBase.LoginUser(username, password, out int id, out decimal balance))
            {
                UserSession.Id = id;
                UserSession.Username = username;
                UserSession.Balance = balance;

                StatusTextBlock.Text = "Sikeres bejelentkezés!";
                Logger.Log($"Felhasználó bejelentkezett: {UserSession.Username}");
                mainWindow.SetLoggedInUser(username);
                Close();
            }
            else
            {
                StatusTextBlock.Text = "Hibás felhasználónév vagy jelszó.";
            }
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text;
            string password = PasswordBox.Password;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                Log.Warning("Hiányzó mezők a regisztrációnál");
                StatusTextBlock.Text = "Kérlek töltsd ki az összes mezőt.";
                return;
            }

            bool containsNumber = password.Any(char.IsDigit);
            bool containsSpecial = password.Any(ch => !char.IsLetterOrDigit(ch));

            if (!containsNumber || !containsSpecial)
            {
                Log.Warning("Hibás jelszó megadás");
                StatusTextBlock.Text = "A jelszónak tartalmaznia kell legalább egy számot és egy speciális karaktert!";
                return;
            }
            Log.Debug("Regisztráció próba: {Username}", username);

            if (DataBase.RegisterUser(username, password, out string error))
            {
                Log.Information("Sikeres regisztráció: {Username}", username);
                StatusTextBlock.Text = "Regisztráció sikeres.";
            }
            else
            {
                Log.Error("Regisztráció sikertelen: {Error}", error);
                StatusTextBlock.Text = error;
            }
        }
        
        private void ShowUsers_Click(object sender, RoutedEventArgs e)
        {
            var userListWindow = new UserListing();
            userListWindow.ShowDialog(); 
        }
    
    }
}


