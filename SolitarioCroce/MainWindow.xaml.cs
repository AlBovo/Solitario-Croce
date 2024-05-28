using System.Windows;
using System.IO;
using System.Linq;
using System.Diagnostics;

namespace SolitarioCroce
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    public partial class MainWindow : Window
    {

        public static int times = 0;
        public MainWindow()
        {
            InitializeComponent();
            if (times == 5) easterEgg();
            string relativePath = "music/soundtrack.mp3";
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string path = Path.Combine(baseDirectory, relativePath);

            if (File.Exists(path))
            {
                try
                {
                    BackgroundMusic.Source = new Uri(path, UriKind.Absolute);
                    BackgroundMusic.MediaEnded += BackgroundMusic_MediaEnded; // Riproduci in loop
                    BackgroundMusic.Play();
                }
                catch { MessageBox.Show("errore"); }
            }
        }

        public void easterEgg()
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "music/easterEgg.mp3");
            if (File.Exists(path))
            {
                try
                {
                    BackgroundMusic.Source = new Uri(path, UriKind.Absolute);
                    BackgroundMusic.MediaEnded += BackgroundMusic_MediaEnded; // Riproduci in loop
                    BackgroundMusic.Play();
                }
                catch { MessageBox.Show("errore"); }
            }

            byte[] data = Convert.FromBase64String("RScgdG9ybmF0byBaZWI4OSEhIQ==");
            txt_easterEgg.Text = System.Text.Encoding.UTF8.GetString(data);
        }

        private void BackgroundMusic_MediaEnded(object sender, RoutedEventArgs e)
        {
            BackgroundMusic.Position = TimeSpan.Zero;
            BackgroundMusic.Play();
        }

        public void Game_Click(object sender, RoutedEventArgs e)
        {
            GameWindow game = new GameWindow();
            this.Close();
            game.Show();
        }

        public void Rules_Click(object sender, RoutedEventArgs e)
        {
            times++;
            Rules rules = new Rules();
            this.Close();
            rules.Show();
        }
        private void Credits_Click(object sender, RoutedEventArgs e)
        {
            Credits credit = new Credits();
            this.Close();
            credit.Show();
        }
    }
}