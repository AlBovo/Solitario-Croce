using System.Windows;

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
            if(times == 5) easterEgg();
        }

        public void easterEgg()
        {
            byte[] data = Convert.FromBase64String("RScgdG9ybmF0byBaZWI4OSEhIQ==");
            txt_easterEgg.Text = System.Text.Encoding.UTF8.GetString(data);
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
    }
}