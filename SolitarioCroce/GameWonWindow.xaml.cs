using System.Windows;

namespace SolitarioCroce
{
    /// <summary>
    /// Logica di interazione per GameWonPage.xaml
    /// </summary>
    public partial class GameWonPage : Window
    {
        public GameWonPage()
        {
            InitializeComponent();
        }

        private void GameWon(object sender, RoutedEventArgs e)
        {
            MainWindow main = new MainWindow();
            this.Close();
            main.Show();
        }
    }
}
