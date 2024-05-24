using System.Windows;

namespace SolitarioCroce
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public void Game_Click(object sender, RoutedEventArgs e)
        {
            GameWindow game = new GameWindow();
            this.Close();
            game.Show();
        }

        public void Rules_Click(object sender, RoutedEventArgs e)
        {
            Rules rules = new Rules();
            this.Close();
            rules.Show();
        }
    }
}