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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SolitarioCroce
{
    /// <summary>
    /// Logica di interazione per Rules.xaml
    /// </summary>
    public partial class Rules : Window
    {
        public Rules()
        {
            InitializeComponent();
            LoadWebView();
        }

        private async void LoadWebView()
        {
            await iframe.EnsureCoreWebView2Async();
            iframe.NavigateToString(
                @"<html>" +
                @"<iframe width=""392"" height=""395"" src=""https://www.youtube.com/embed/g7TJviLmuMg"" title=""Solitario Croce - Video dimostrativo - Solitari Free"" frameborder=""0"" allow=""accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture; web-share"" referrerpolicy=""strict-origin-when-cross-origin"" allowfullscreen></iframe>" +
                @"</html>"
            );
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            this.Close();
            mainWindow.Show();
        }
    }
}