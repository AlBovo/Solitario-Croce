using System.Windows;

namespace SolitarioCroce
{
    /// <summary>
    /// Logica di interazione per Credits.xaml
    /// </summary>
    public partial class Credits : Window
    {
        public Credits()
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
