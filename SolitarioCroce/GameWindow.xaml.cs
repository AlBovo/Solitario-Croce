using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SolitarioCroce
{
    /// <summary>
    /// Logica di interazione per GameWindow.xaml
    /// </summary>
    public partial class GameWindow : Window
    {
        readonly Table table = new Table();

        public GameWindow()
        {
            InitializeComponent();

            Panel.SetZIndex(canva, 0);
            Card[] cards = new Card[5];
            for (int i = 0; i < 5; i++)
                cards[i] = table.GetCardsFromStacks(i);

            BaseTopLeft.Background = create_Image_from_card(new Card(Card.Seeds.Denari, 0));
            BaseTopLeft.Background.Opacity = 0.5;
            BaseTopRight.Background = create_Image_from_card(new Card(Card.Seeds.Coppe, 0));
            BaseTopRight.Background.Opacity = 0.5;
            BaseLowLeft.Background = create_Image_from_card(new Card(Card.Seeds.Spade, 0));
            BaseLowLeft.Background.Opacity = 0.5;
            BaseLowRight.Background = create_Image_from_card(new Card(Card.Seeds.Bastoni, 0));
            BaseLowRight.Background.Opacity = 0.5;

            Canvas[] canvas = { CrossLeft, CrossRight, CrossLow, CrossTop, CrossMid };

            for (int i = 0; i < cards.Length; i++)
                canvas[i].Background = create_Image_from_card(cards[i]);
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
                Panel.SetZIndex(moving_card, 1);
                Canvas source = (Canvas)e.Source;
                source.Opacity = 0.5;
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

            //remove opacity from source
            source.Opacity = 1;
            
        }

        private void Canvas_DragOver(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(typeof(Canvas)))
                return;

            Point dropPosition = e.GetPosition(canva);

            Canvas.SetLeft(moving_card, dropPosition.X - (int)(moving_card.ActualWidth/2));
            Canvas.SetTop(moving_card, dropPosition.Y - (int)(moving_card.ActualHeight/2));
        }

        private void canva_Drop(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(typeof(Canvas)))
                return;
            Canvas source = (Canvas)e.Data.GetData(typeof(Canvas));
            source.Opacity = 1;
            Canvas.SetTop(moving_card, -500);
        }
        private void moving_card_Drop(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(typeof(Canvas)))
                return;

            //starts another drag drop and puts the card behind the canva, the card_drop or canva_drop event is then triggered
            Canvas source = (Canvas)e.Data.GetData(typeof(Canvas));
            Panel.SetZIndex(moving_card, -1);
            DragDrop.DoDragDrop(source, source, DragDropEffects.Move);
        }
    }
}
