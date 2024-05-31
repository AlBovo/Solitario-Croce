using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
            var result = MessageBox.Show(
                "Sei sicuro di voler tornare al menu?",
                "Torna il menu",
                MessageBoxButton.YesNo
            );

            if (result == MessageBoxResult.Yes)
            {
                this.Close();
                main.Show();
            }
            

        }
    }
}
