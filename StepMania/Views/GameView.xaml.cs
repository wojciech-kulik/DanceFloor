using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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
        public GameView()
        {
            InitializeComponent();
            Loaded += GameView_Loaded;
        }

        void GameView_Loaded(object sender, RoutedEventArgs e)
        {
            const int LeftArrowX = 65;
            const int DownArrowX = 115;
            const int UpArrowX = 295;
            const int RightArrowX = 345;

            int[] arrows = { LeftArrowX, DownArrowX, UpArrowX, RightArrowX };
            int[] arrowRotate = { 90, 0, 180, -90 };

            int seconds = 300;
            int pixelsPerSecond = 200;
            Random r = new Random();

            for (int i = 2; i < seconds; i++ )
            {
                int count = r.Next(1, 3);
                for (int j = 0; j < count; j++)
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
                    img.RenderTransform = new RotateTransform(arrowRotate[arrow]);

                    p1Notes.Children.Add(img);
                    Canvas.SetLeft(img, arrows[arrow]);
                    Canvas.SetTop(img, i * pixelsPerSecond + r.Next(0, pixelsPerSecond));                    
                }
            }

            (Resources.Values.OfType<Storyboard>().First() as Storyboard).Begin();
        }
    }
}
