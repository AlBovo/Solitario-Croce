using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SolitarioCroce
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        readonly Table table = new Table();

        public MainWindow()
        {
            InitializeComponent();

            Panel.SetZIndex(canva, -100);
            Card[] cards = new Card[5];
            for (int i = 0; i < 5; i++)
                cards[i] = table.GetCardsFromStacks(i);

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
            (bool type, int idx) target_type = (target.Tag.ToString()![0] == 's', int.Parse($"{target.Tag.ToString()![1]}"));

            if (source_type.type && target_type.type)
                return (true, source_type.idx, target_type.idx);
            else
                return (false, source_type.idx, target_type.idx);
        }

        private void GetDeckCard(object sender, RoutedEventArgs e)
        {
            Card card = table.deck.GetCard();

            if (table.deck.IsEmpty())
            {
                Deck.Background = Brushes.Transparent;
                Deck.IsEnabled = false;
            }

            PickedCards.Background = create_Image_from_card(card);
            table.AddCardToPicked(card);
        }

        private ImageBrush create_Image_from_card(Card card)
        {
            ImageBrush image = new ImageBrush();
            image.ImageSource = new BitmapImage(new Uri(card.Path()));
            image.Stretch = Stretch.UniformToFill;
            return image;
        }

        private void Dragging_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Canvas source = (Canvas)e.Source;
                moving_card.Background = source.Background;
                moving_card.Tag = source.Tag;
                DragDrop.DoDragDrop(source, source, DragDropEffects.Move);
            }
        }

        private void Card_Drop(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(typeof(Canvas)))
                return;

            //watch out when you keep the left button pressed on nothing and pass over another card
            Canvas source = (Canvas)e.Data.GetData(typeof(Canvas));
            Canvas target = (Canvas)e.Source;

            (bool Type, int source_idx, int target_idx) Action = GetTypeAndId_Movement(source, target);
            bool movementOK;

            if (Action.Type) //stack
                movementOK = table.ChangeCardStack(Action.source_idx, Action.target_idx);
            else             //base
                movementOK = table.ChangeCardBase(Action.source_idx, Action.target_idx);

            if (movementOK)
            {
                target.Background = source.Background;
                Card below = table.GetCardsFromStacks(Action.source_idx);
                if (below != null)
                    source.Background = create_Image_from_card(below);
                else
                    source.Background = Brushes.Transparent;
            }
        }

        private void Canvas_DragOver(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(typeof(Canvas)))
                return;

            Point dropPosition = e.GetPosition(canva);

            Canvas.SetLeft(moving_card, dropPosition.X+10);
            Canvas.SetTop(moving_card, dropPosition.Y+10);
        }

        private void canvas_Drop(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(typeof(Canvas)))
                return;

            Canvas.SetTop(moving_card, -500);
        }
    }
}