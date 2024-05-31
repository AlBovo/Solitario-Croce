using System.Windows;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SolitarioCroce
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    public partial class MainWindow : Window
    {
        public static bool music = true;
        public static int times = 0;
        public MainWindow()
        {
            InitializeComponent();
            music = true;

            logo.ImageSource = new BitmapImage(new Uri("pack://application:,,,/icons/logo.png"));
            logo.Stretch = Stretch.UniformToFill;
            if (times == 5) easterEgg();
            else
            {
                string relativePath = "music/soundtrack.mp3";
                string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
                string path = Path.Combine(baseDirectory, relativePath);

                playMusic(path);
            }

            set_music_icon();
        }

        private void set_music_icon()
        {
            music_icon.ImageSource = music ? new BitmapImage(new Uri("pack://application:,,,/icons/pause.png")) : new BitmapImage(new Uri("pack://application:,,,/icons/play.png"));
            music_icon.Stretch = Stretch.UniformToFill;
        }

        public void playMusic(string path)
        {
            if (File.Exists(path))
            {
                try
                {
                    BackgroundMusic.Source = new Uri(path, UriKind.Absolute);
                    BackgroundMusic.MediaEnded += BackgroundMusic_MediaEnded; // Riproduci in loop

                    //play solo se la musica è attivata
                    if (music)
                        BackgroundMusic.Play();
                }
                catch { MessageBox.Show("error while playing music"); }
            }
        }

        public void easterEgg()
        {
            string relativePath = "music/easterEgg.mp3";
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string path = Path.Combine(baseDirectory, relativePath);
            if (File.Exists(path))
            {
                try
                {
                    playMusic(path);
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
            if (music) BackgroundMusic.Stop();
            GameWindow game = new GameWindow();
            this.Close();
            game.Show();
        }

        public void Rules_Click(object sender, RoutedEventArgs e)
        {
            if (music) BackgroundMusic.Stop();
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

        private void btn_enableMusic_Click(object sender, RoutedEventArgs e)
        {
            music = !music;
            set_music_icon();

            if (!music)
                BackgroundMusic.Stop();
            else
                BackgroundMusic.Play();

        }
    }
}