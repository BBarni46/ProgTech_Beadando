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
        public ProfileWindow(string username) 
        {
            InitializeComponent();
            UsernameText.Text = $"Bejelentkezve: {username}";
        }
    }
}
