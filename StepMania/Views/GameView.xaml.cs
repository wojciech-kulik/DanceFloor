using StepMania.Helpers;
using StepMania.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace StepMania.Views
{
    /// <summary>
    /// Interaction logic for GameView.xaml
    /// </summary>
    public partial class GameView : UserControl
    {
        List<Key> supportedKeys = new List<Key>() { Key.Left, Key.Down, Key.Up, Key.Right, Key.W, Key.A, Key.S, Key.D };

        public GameView()
        {
            InitializeComponent();
            Loaded += GameView_Loaded;            
        }

        public void GameView_KeyUp(object sender, KeyEventArgs e)
        {
            #if DEBUG_HIT_TIME
            if (e.Key == Key.Space)
            {
                (Resources.Values.OfType<Storyboard>().First() as Storyboard).Resume();
                return;
            }
            #endif

            if (supportedKeys.Contains(e.Key))
                ((GameViewModel)DataContext).PlayerHit(ControlHelper.KeyToPlayerID(e.Key), ControlHelper.KeyToPlayerAction(e.Key));
        }

        void GameView_Loaded(object sender, RoutedEventArgs e)
        {
            ((Parent as ContentControl).Parent as Window).PreviewKeyUp += GameView_KeyUp;

            const int LeftArrowX = 0;
            const int DownArrowX = 115;
            const int UpArrowX = 230;
            const int RightArrowX = 345;

            int[] arrows = { LeftArrowX, DownArrowX, UpArrowX, RightArrowX };
            int[] arrowRotate = { 90, 0, 180, -90 };

            int seconds = 300;
            int pixelsPerSecond = 200;
            Random r = new Random();

            for (int i = 2; i < seconds; i++ )
            {
                int count = r.Next(1, 3);
                for (int j = 0; j < 1; j++)
                {
                    Image img = new Image();
                    img.Width = 65;
                    img.Height = 65;

                    var bitmap = new BitmapImage();
                    using (var stream = File.OpenRead("..\\..\\Images\\active_arrow.png"))
                    {
                        bitmap.BeginInit();
                        bitmap.CacheOption = BitmapCacheOption.OnLoad;
                        bitmap.StreamSource = stream;
                        bitmap.EndInit();
                    }

                    img.Source = bitmap;                    

                    int arrow = r.Next(0, 4);
                    img.RenderTransform = new RotateTransform(arrowRotate[arrow], 32.5, 32.5);

                    double top = i * pixelsPerSecond + r.Next(0, pixelsPerSecond);
                    img.Tag = top / pixelsPerSecond;
                    Canvas.SetLeft(img, arrows[arrow]);
                    Canvas.SetTop(img, top);
                    p1Notes.Children.Add(img);
                    
                    #if DEBUG_TIMING
                    TextBlock tb = new TextBlock();
                    tb.FontSize = 20;
                    tb.Text = String.Format("{0:N2}", top / pixelsPerSecond);
                    tb.Foreground = new SolidColorBrush(Colors.Black);
                    Canvas.SetLeft(tb, arrows[arrow]);
                    Canvas.SetTop(tb, top);
                    p1Notes.Children.Add(tb);
                    #endif                    
                }
            }

            #if DEBUG_TIMING
            new Thread(new ThreadStart(() =>
                {
                    while (true)
                    {
                        Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                        {
                            var time = (Resources.Values.OfType<Storyboard>().First() as Storyboard).GetCurrentTime();
                            this.p1PointsBar.Points = time.TotalSeconds.ToString("N2");
                        }));
                        Thread.Sleep(50);
                    }
                })).Start();
            #endif

            (Resources.Values.OfType<Storyboard>().First() as Storyboard).Begin();    
        }
    }
}
