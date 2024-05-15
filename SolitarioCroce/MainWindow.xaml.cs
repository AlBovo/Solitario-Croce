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

        /// <summary>
        /// function to call to choose which movement to perform
        /// </summary>
        /// <param name="source">the canvas of origin in the drag and drop event</param>
        /// <param name="target">the canvas of destination in the drag and drop event</param>
        /// <returns>bool: true if stack-stack else stack-base</returns>
        private (bool, int, int) GetTypeAndId_Movement(Canvas source, Canvas target)
        {
            (bool type, int idx) source_type = (source.Tag.ToString()![0] == 's', int.Parse($"{source.Tag.ToString()![1]}"));
            (bool type, int idx) target_type = (source.Tag.ToString()![0] == 's', int.Parse($"{source.Tag.ToString()![1]}"));

            if (source_type.type && target_type.type)
                return (true, source_type.idx, target_type.idx);
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
                DragDrop.DoDragDrop((Canvas)e.Source, (Canvas)e.Source, DragDropEffects.Move);
            }
        }

        private void Card_Drop(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(typeof(Canvas)))
                return;

            //watch out when you keep the left button pressed on nothing and pass over another card
            Canvas source = (Canvas)e.Data.GetData(typeof(Canvas));
            Canvas target = (Canvas)e.Source;

            target.Background = source.Background;
        }

    }
}