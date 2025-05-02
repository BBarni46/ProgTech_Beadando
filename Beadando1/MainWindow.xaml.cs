﻿using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Beadando1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
      
        public MainWindow()
        {
            InitializeComponent();
            string projectRoot = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory)!.Parent!.Parent!.FullName;
            string fullPath = System.IO.Path.Combine(projectRoot, "Sound", "menu.mp3");

            MusicState.mediaPlayer.Open(new Uri(fullPath, UriKind.Absolute));
            MusicState.mediaPlayer.MediaEnded += (s, e) => MusicState.mediaPlayer.Position = TimeSpan.Zero;
            MusicState.mediaPlayer.Play();
        }

        private void Play_Click(object sender, RoutedEventArgs e)
        {
            // Az új ablak megnyitása
            GameSelect win = new GameSelect(this);
            win.Left = this.Left;
            win.Top = this.Top;
            win.WindowStartupLocation = WindowStartupLocation.Manual;
            win.Show();
            this.Close();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        public void ToggleMuteButton_Click(object sender, RoutedEventArgs e)
        {
            if (MusicState.isMuted)
            {
                MusicState.mediaPlayer.Play();
                MusicState.isMuted = false;
                ToggleMuteButton.Content = "🔊";
            }
            else
            {
                MusicState.mediaPlayer.Pause();
                MusicState.isMuted = true;
                ToggleMuteButton.Content = "🔇";
            }
        }

        //GITHUB PRÓBA , siker
        //cicah
    }
}