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
    /// Interaction logic for Game3.xaml
    /// </summary>
    public partial class Game3 : Window
    {
        private GameSelect _gameSelect;  // Itt van a mező, osztály szinten kell deklarálni

        public Game3(GameSelect gameSelect)
        {
            InitializeComponent();

            _gameSelect = gameSelect; 
      
            this.Left = _gameSelect.Left;
            this.Top = _gameSelect.Top;
            this.WindowStartupLocation = WindowStartupLocation.Manual;
        }
    }
}
