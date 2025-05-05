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
        }


        private void Login_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text;
            string password = PasswordBox.Password;

            if (username == "Bajnok" && password == "cicah")
            {
                StatusTextBlock.Text = "Sikeres bejelentkezés!";
                this.Close();
                //ide cuccok majd ha lehet adatb
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

            StatusTextBlock.Text = "Regisztráció sikeres.";
        }
    }
}


