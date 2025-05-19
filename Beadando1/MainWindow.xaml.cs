using Beadando1.MUSIC;
using System.IO;
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
        private string? loggedInUser = null;
        public MainWindow()
        {

            InitializeComponent();

            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            string projectRoot = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory)!.Parent!.Parent!.Parent!.FullName;
            string fullPath = System.IO.Path.Combine(projectRoot, "Sound", "menu.mp3");

            MusicState.mediaPlayer.Open(new Uri(fullPath, UriKind.Absolute));
            MusicState.mediaPlayer.MediaEnded += (s, e) => MusicState.mediaPlayer.Position = TimeSpan.Zero;
            MusicState.mediaPlayer.Play();

            if (MusicState.isMuted)
            {
                ToggleMuteButton.Content = "🔇";
                MusicState.mediaPlayer.Pause();
            }
            else
            {
                ToggleMuteButton.Content = "🔊";
                MusicState.mediaPlayer.Play();
            }
        }

        private void Play_Click(object sender, RoutedEventArgs e)
        {
            if (loggedInUser == null)
            {
                var registerWindow = new RegisterWindow(this);
                registerWindow.ShowDialog();

                if (loggedInUser == null)
                {
                    return;
                }   
            }
            GameSelect win = new GameSelect(this);
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
        private void AuthButton_Click(object sender, RoutedEventArgs e)
        {
            if (loggedInUser == null)
            {
                var registerWindow = new RegisterWindow(this);
                registerWindow.ShowDialog();
            }
            else
            {
                var profileWindow = new ProfileWindow(loggedInUser);
                profileWindow.ShowDialog();
            }
        }
        public void SetLoggedInUser(string username)
        {
            loggedInUser = username;
            RegisterButton.Content = username; 
        }
        //GITHUB PRÓBA , siker
        //cicah yeayea aha
        //uj sor
    }
}