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

namespace Beadando1
{
    /// <summary>
    /// Interaction logic for Game1.xaml
    /// </summary>
    public partial class Game1 : Window
    {
        private GameSelect _gameSelect;  // Itt van a mező, osztály szinten kell deklarálni

        public Game1(GameSelect gameSelect)
        {
            InitializeComponent();

            _gameSelect = gameSelect;  // A GameSelect példány tárolása

            // Pozíció öröklése a GameSelect ablakról
            this.Left = _gameSelect.Left;
            this.Top = _gameSelect.Top;
            this.WindowStartupLocation = WindowStartupLocation.Manual;
        }
    }
}

