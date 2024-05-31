using System.Windows;

namespace SolitarioCroce
{
    /// <summary>
    /// Logica di interazione per GameLostPage.xaml
    /// </summary>
    public partial class GameLostPage : Window
    {
        public GameLostPage()
        {
            InitializeComponent();
        }

        private void GameLost(object sender, RoutedEventArgs e)
        {
            MainWindow main = new MainWindow();
            this.Close();
            main.Show();
        }
    }
}
