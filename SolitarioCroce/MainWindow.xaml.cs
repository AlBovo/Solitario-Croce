using System.Text;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Table table = new Table();

        public MainWindow()
        {
            InitializeComponent();

            Card[] cards = table.GetCardsFromStacks();
            Canvas[] canvas = { CrossLeft, CrossRight, CrossLow, CrossTop, CrossMid };

            for (int i = 0; i < cards.Length; i++)
            {
                ImageBrush image = new ImageBrush();
                image.ImageSource = new BitmapImage(new Uri(cards[i].Path()));
                image.Stretch = Stretch.UniformToFill;

                canvas[i].Background = image;
            }
        }

        private void GetDeckCard(object sender, RoutedEventArgs e)
        {
            Card card = table.deck.GetCard();

            if (table.deck.IsEmpty())
            {
                Deck.Background = Brushes.Transparent;
                Deck.IsEnabled = false;
            }

            ImageBrush image = new ImageBrush();
            image.ImageSource = new BitmapImage(new Uri(card.Path()));
            image.Stretch = Stretch.UniformToFill;

            PickedCards.Background = image;
        }

        private void Dragging_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragDrop.DoDragDrop(PickedCards, PickedCards, DragDropEffects.Move);
            }
        }

        private void BaseLowRight_Drop(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(typeof(Canvas)))
                return;

            Canvas source = (Canvas)e.Data.GetData(typeof(Canvas));
            BaseLowRight.Background = source.Background;
        }

    }
}