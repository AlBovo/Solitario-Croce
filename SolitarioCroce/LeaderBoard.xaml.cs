using System.Windows;

namespace SolitarioCroce
{
    /// <summary>
    /// Logica di interazione per LeaderBoard.xaml
    /// </summary>
    public partial class LeaderBoard : Window
    {
        public LeaderBoard()
        {
            InitializeComponent();
        }

        private void Menu_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = new MainWindow();
            this.Close();
            main.Show();
        }
    }
}
