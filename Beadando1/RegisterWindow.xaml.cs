﻿using System;
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
using Microsoft.Data.Sqlite;
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

            if (DataBase.LoginUser(username, password))
            {
                StatusTextBlock.Text = "Sikeres bejelentkezés!";
                this.Close();
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
            userListWindow.ShowDialog(); // vagy .Show() ha nem modális
        }
    }
}


