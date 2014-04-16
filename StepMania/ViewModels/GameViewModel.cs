//#define DEBUG_TIMING

using Caliburn.Micro;
using Common;
using GameLayer;
using StepMania.Properties;
using StepMania.Views;
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
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

namespace StepMania.ViewModels
{
    public class GameViewModel : BaseViewModel, IHandle<PlayerHitEvent>
    {
        const int PixelsPerSecond = 200;
        const int ArrowWidthHeight = 65;
        const int MarginBetweenArrows = 50;

        const int LeftArrowX = 0;
        const int DownArrowX = ArrowWidthHeight + MarginBetweenArrows;
        const int UpArrowX = DownArrowX + ArrowWidthHeight + MarginBetweenArrows;
        const int RightArrowX = UpArrowX + ArrowWidthHeight + MarginBetweenArrows;

        GameView _view;
        Storyboard _animation;
        IGame _game;

        public GameViewModel(IEventAggregator eventAggregator, IGame game)
            : base(eventAggregator)
        {
            _game = game;
        }

        protected override void OnViewAttached(object view, object context)
        {
            _view = view as GameView;
            _animation = _view.Resources.Values.OfType<Storyboard>().First() as Storyboard;
            LoadSong(_game.Song);
            StartAnimation();
        }

        public void LoadSong(ISong song)
        {
            _view.p1Notes.Children.Clear();

            foreach(var seqElem in song.Sequences[Difficulty.Easy])
            {
                //create bitmap
                var bitmap = new BitmapImage();
                using (var stream = File.OpenRead("..\\..\\Images\\active_arrow.png")) //TODO: load from resources
                {
                    bitmap.BeginInit();
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.StreamSource = stream;
                    bitmap.EndInit();
                }

                //load and prepare image
                Image img = new Image() { Width = ArrowWidthHeight, Height = ArrowWidthHeight, Source = bitmap, Tag = seqElem };
                double top = seqElem.Time.TotalSeconds * PixelsPerSecond;
                Canvas.SetTop(img, top);

                switch(seqElem.Type)
                {
                    case SeqElemType.LeftArrow:
                        img.RenderTransform = new RotateTransform(90, ArrowWidthHeight / 2.0, ArrowWidthHeight / 2.0);
                        Canvas.SetLeft(img, LeftArrowX);
                        break;
                    case SeqElemType.DownArrow:
                        Canvas.SetLeft(img, DownArrowX);
                        break;
                    case SeqElemType.UpArrow:
                        img.RenderTransform = new RotateTransform(180, ArrowWidthHeight / 2.0, ArrowWidthHeight / 2.0);
                        Canvas.SetLeft(img, UpArrowX);
                        break;
                    case SeqElemType.RightArrow:
                        img.RenderTransform = new RotateTransform(-90, ArrowWidthHeight / 2.0, ArrowWidthHeight / 2.0);
                        Canvas.SetLeft(img, RightArrowX);
                        break;
                }                

                //add image to the canvas
                _view.p1Notes.Children.Add(img);

                #if DEBUG_TIMING
                TextBlock tb = new TextBlock() { FontSize = 20, Text = String.Format("{0:N2}", top / PixelsPerSecond) };
                Canvas.SetTop(tb, top);
                switch (seqElem.Type)
                {
                    case SeqElemType.LeftArrow:
                        Canvas.SetLeft(tb, LeftArrowX);
                        break;
                    case SeqElemType.DownArrow:
                        Canvas.SetLeft(tb, DownArrowX);
                        break;
                    case SeqElemType.UpArrow:
                        Canvas.SetLeft(tb, UpArrowX);
                        break;
                    case SeqElemType.RightArrow:
                        Canvas.SetLeft(tb, RightArrowX);
                        break;
                }          
                _view.p1Notes.Children.Add(tb);
                #endif
            }

            #if DEBUG_TIMING
            new Thread(new ThreadStart(() =>
                {
                    while (true)
                    {
                        if (Application.Current == null)
                            return;

                        Application.Current.Dispatcher.BeginInvoke(new System.Action(() =>
                        {
                            var time = (_view.Resources.Values.OfType<Storyboard>().First() as Storyboard).GetCurrentTime();
                            _view.p1PointsBar.Points = time.TotalSeconds.ToString("N2");
                        }));
                        Thread.Sleep(50);
                    }
                })).Start();
            #endif   
        }

        public void StartAnimation()
        {
            _animation.Begin(); 
        }

        /*
        TimeSpan GetSongCurrentTime()
        {
            return _animation.GetCurrentTime();
        }

        string isHit;

        var currentTime = GetSongCurrentTime();
        var note = _view.p1Notes.Children.OfType<Image>().ToList().FirstOrDefault(n => Math.Abs(((double)n.Tag) - currentTime.TotalSeconds) < 0.2);

        if (note != null)
            isHit = "TAK";
        else
            isHit = "NIE";
        if (note != null)
            isHit += " " + (((double)note.Tag) - currentTime.TotalSeconds).ToString("N2");
        _view.p1PointsBar.Points = isHit;*/

        public void Handle(PlayerHitEvent message)
        {
            _view.p1PointsBar.Points = message.Points.ToString();
            _view.p1Health.SetLife(message.Life);
            //TODO: do smth with hit seq elem
        }
    }
}
