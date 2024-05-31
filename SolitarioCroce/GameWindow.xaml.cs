using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Threading;
using System.Windows.Threading;
using System.IO;

namespace SolitarioCroce
{
    /// <summary>
    /// Logica di interazione per GameWindow.xaml
    /// </summary>
    public partial class GameWindow : Window
    {
        DispatcherTimer timer; // creating a new timer
        double time = 0;
        readonly Table table = new Table();
        bool drop_from_user = true;
        public static bool music = true;

        public GameWindow()
        {
            InitializeComponent();

            retro.ImageSource = new BitmapImage(new Uri(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "images/RETRO.jpg")));
            Panel.SetZIndex(canva, 0);
            Card[] cards = new Card[5];
            for (int i = 0; i < 5; i++)
                cards[i] = table.GetCardsFromStacks(i);

            try
            {
                BaseTopLeft.Background = create_Image_from_card(new Card(Card.Seeds.Denari, 0));
                BaseTopRight.Background = create_Image_from_card(new Card(Card.Seeds.Coppe, 0));
                BaseLowLeft.Background = create_Image_from_card(new Card(Card.Seeds.Spade, 0));
                BaseLowRight.Background = create_Image_from_card(new Card(Card.Seeds.Bastoni, 0));
            }
            catch
            {
                BaseTopLeft.Background = create_Image_from_card(new Card(Card.Seeds.Denari, 1));
                BaseTopRight.Background = create_Image_from_card(new Card(Card.Seeds.Coppe, 1));
                BaseLowLeft.Background = create_Image_from_card(new Card(Card.Seeds.Spade, 1));
                BaseLowRight.Background = create_Image_from_card(new Card(Card.Seeds.Bastoni, 1));
            }

            BaseTopLeft.Background.Opacity = 0.5;
            BaseTopRight.Background.Opacity = 0.5;
            BaseLowLeft.Background.Opacity = 0.5;
            BaseLowRight.Background.Opacity = 0.5;

            Canvas[] canvas = { CrossLeft, CrossRight, CrossLow, CrossTop, CrossMid };

            for (int i = 0; i < cards.Length; i++)
                canvas[i].Background = create_Image_from_card(cards[i]);
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(10); // this timer will trigger every 10 milliseconds
            timer.Start(); // starting the timer
            timer.Tick += timer_Tick; // with each tick it will trigger this function

            string relativePath = "music/soundtrack.mp3";
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string path = Path.Combine(baseDirectory, relativePath);

            set_music_icon();
            playMusic(path);

        }

        private void timer_Tick(object sender, EventArgs e)
        {
            time += timer.Interval.TotalSeconds;
            txt_Timer.Text = $"{time:0.00}";
        }
        public void playMusic(string path)
        {
            if (File.Exists(path))
            {
                try
                {
                    BackgroundMusic.Source = new Uri(path, UriKind.Absolute);
                    BackgroundMusic.MediaEnded += BackgroundMusic_MediaEnded; // Riproduci in loop

                    //play solo se la musica è attivata
                    if (music)
                        BackgroundMusic.Play();
                }
                catch { MessageBox.Show("error while playing music"); }
            }
        }

        private void BackgroundMusic_MediaEnded(object sender, RoutedEventArgs e)
        {
            BackgroundMusic.Position = TimeSpan.Zero;
            BackgroundMusic.Play();
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
            byte status = table.GetStatusGame();
            switch (status)
            {
                case 1:
                    GameLost();
                    break;

                case 2:
                    GameWon();
                    break;

                default:
                    break;
            }
        }

        private ImageBrush create_Image_from_card(Card card)
        {
            ImageBrush image = new ImageBrush();
            image.ImageSource = new BitmapImage(new Uri(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, card.Path())));
            image.Stretch = Stretch.UniformToFill;
            return image;
        }

        private void Dragging_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                drop_from_user = true;
                Panel.SetZIndex(moving_card, 1);
                Canvas source = (Canvas)e.Source;
                source.Opacity = 0.5;
                moving_card.Background = source.Background;
                DragDrop.DoDragDrop(source, source, DragDropEffects.Move);
            }
        }

        private void GameLost()
        {
            if (music) BackgroundMusic.Stop();
            GameLostPage lost = new GameLostPage();
            this.Close();
            lost.Show();
        }

        private void GameWon()
        {
            if (music) BackgroundMusic.Stop();
            GameWonPage won = new GameWonPage();
            this.Close();
            won.Show();
        }

        private void Card_Drop(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(typeof(Canvas)))
                return;

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

                byte status = table.GetStatusGame();
                switch (status)
                {
                    case 1:
                        GameLost();
                        break;

                    case 2:
                        GameWon();
                        break;

                    default:
                        break;
                }

            }

            //remove opacity from source
            source.Opacity = 1;

        }

        private void Canvas_DragOver(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(typeof(Canvas)))
                return;

            Point dropPosition = e.GetPosition(canva);

            Canvas.SetLeft(moving_card, dropPosition.X - (int)(moving_card.ActualWidth / 2));
            Canvas.SetTop(moving_card, dropPosition.Y - (int)(moving_card.ActualHeight / 2));
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
            if (drop_from_user)
            {
                drop_from_user = false;
                DragDrop.DoDragDrop(source, source, DragDropEffects.Move);
            }
        }

        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            GameWindow game = new GameWindow();
            var result = MessageBox.Show(
                "Sei sicuro di voler resettare?",
                "Reset",
                MessageBoxButton.YesNo
            );

            if (result == MessageBoxResult.Yes)
            {
                this.Close();
                game.Show();
            }
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
                if (music) BackgroundMusic.Stop();
                this.Close();
                main.Show();
            }
        }

        private void btn_enableMusic_Click(object sender, RoutedEventArgs e)
        {
            music = !music;
            set_music_icon();

            if (!music)
                BackgroundMusic.Stop();
            else
                BackgroundMusic.Play();
        }

        private void set_music_icon()
        {
            music_icon.ImageSource = music ? new BitmapImage(new Uri("pack://application:,,,/icons/pause.png")) : new BitmapImage(new Uri("pack://application:,,,/icons/play.png"));
            music_icon.Stretch = Stretch.UniformToFill;
        }
    }
}
