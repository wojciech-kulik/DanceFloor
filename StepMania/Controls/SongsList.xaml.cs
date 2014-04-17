using System;
using System.Collections.Generic;
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

namespace StepMania.Controls
{
    /// <summary>
    /// Interaction logic for SongsList.xaml
    /// </summary>
    public partial class SongsList : UserControl
    {
        Thread animation;
        Key? holdKey, lastHoldKey;

        public SongsList()
        {
            InitializeComponent();
            DataContext = this;

            Items = new List<string>() { "adsas", "adsas", "adsas", "adsas", "adsas", "adsas", "adsas", "adsas", "adsas", "adsas", "adsas", "adsas", "adsas", "adsas", "adsas", "adsas", "adsas", "adsas", "adsas", "adsas" };
        }

        public List<string> Items { get; set; }


        public void SongsList_HandleKeyUp(object sender, KeyEventArgs e)
        {
            holdKey = null;        
        }

        public void SongsList_HandleKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Left && e.Key != Key.Right)
                return;

            if (holdKey.HasValue || (animation != null && animation.IsAlive))
                return;
            else
            {
                holdKey = lastHoldKey = e.Key;
            }

            Action<Action> runInUI = (a) => Application.Current.Dispatcher.Invoke(a);
                    
            animation = new Thread(new ThreadStart(() => 
            {
                const int SleepTime = 5;
                const int PixelSpeed = 4;
                const int FullMoveLength = 250;

                TranslateTransform transform = null;
                runInUI(() => transform = (moveablePanel.RenderTransform as TranslateTransform));

                while (holdKey.HasValue)
                {
                    runInUI(() => 
                    {
                        if (holdKey.HasValue)
                            transform.X += holdKey.Value == Key.Right ? -PixelSpeed : PixelSpeed;
                    });
                    Thread.Sleep(SleepTime);
                }

                //calculate how many pixels has left
                int diff = 0;
                runInUI(() => diff = (int)transform.X % FullMoveLength);

                //align to proper place
                while (Math.Abs(diff) > PixelSpeed)
                {
                    runInUI(() => 
                    {
                        transform.X += lastHoldKey.Value == Key.Right ? -PixelSpeed : PixelSpeed;
                        diff = (int)transform.X % FullMoveLength;
                    });
                    Thread.Sleep(SleepTime);
                }

                //last alignment
                runInUI(() => transform.X += (moveablePanel.RenderTransform as TranslateTransform).X % FullMoveLength);
            }));
            animation.Start();
        }
    }
}
