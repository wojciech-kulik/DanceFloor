using GameLayer;
using StepMania.Constants;
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
        }

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
                const int FullMoveLength = GameUIConstants.SongItemWidth;

                TranslateTransform transform = null;
                runInUI(() => transform = (moveablePanel.RenderTransform as TranslateTransform));

                int offset;
                int max_offset = 0;
                runInUI(() => max_offset = ((moveablePanel.Children[0] as ItemsControl).Items.Count - 1) * FullMoveLength);

                while (holdKey.HasValue)
                {
                    runInUI(() => 
                    {
                        if (holdKey.HasValue)
                        {
                            offset = holdKey.Value == Key.Right ? -PixelSpeed : PixelSpeed;
                            transform.X = Math.Max(-max_offset, Math.Min(0, transform.X + offset));
                        }                            
                    });
                    Thread.Sleep(SleepTime);
                }

                //calculate how many pixels has left
                int diff = 0;
                runInUI(() => diff = (int)transform.X % FullMoveLength);

                //animate to proper place
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




        public System.Collections.IEnumerable ItemsSource
        {
            get { return (System.Collections.IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ItemsSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(System.Collections.IEnumerable), typeof(SongsList), new PropertyMetadata(null));


    }
}
