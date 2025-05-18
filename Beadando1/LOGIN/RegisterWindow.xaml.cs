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
                StatusTextBlock.Text = "Kérlek töltsd ki az összes mezőt.";
                return;
            }

            if (DataBase.RegisterUser(username, password, out string error))
                StatusTextBlock.Text = "Regisztráció sikeres.";
            else
                StatusTextBlock.Text = error;
        }
        
        private void ShowUsers_Click(object sender, RoutedEventArgs e)
        {
            var userListWindow = new UserListing();
            userListWindow.ShowDialog(); 
        }
    
    }
}


