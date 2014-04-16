using Caliburn.Micro;
using Common;
using GameLayer;
using StepMania.Constants;
using StepMania.DebugHelpers;
using StepMania.Views;
using System.IO;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

namespace StepMania.ViewModels
{
    public class GameViewModel : BaseViewModel, IHandle<PlayerHitEvent>
    {
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

            /*var doubleAnim = new DoubleAnimation(-GameConstants.ArrowWidthHeight, -(GameConstants.PixelsPerSecond * song.Duration.TotalSeconds + GameConstants.ArrowWidthHeight), song.Duration);
            _animation = new Storyboard();
            _animation.Children.Add(doubleAnim);
            Storyboard.SetTarget(doubleAnim, _view.p1Notes);
            Storyboard.SetTargetProperty(doubleAnim, new System.Windows.PropertyPath("RenderTransform.(TranslateTransform.Y)"));*/

            LoadSong(DebugSongHelper.GenerateSong());
            StartAnimation();
        }

        public void LoadSong(ISong song)
        {
            _game.Song = song;
            _view.p1Notes.Children.Clear();

            _animation.Children.First().Duration = song.Duration;
            (_animation.Children.First() as DoubleAnimation).From = -GameConstants.ArrowWidthHeight;
            (_animation.Children.First() as DoubleAnimation).To = -(GameConstants.PixelsPerSecond * song.Duration.TotalSeconds + GameConstants.ArrowWidthHeight);

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
                Image img = new Image() { Width = GameConstants.ArrowWidthHeight, Height = GameConstants.ArrowWidthHeight, Source = bitmap, Tag = seqElem };
                double top = seqElem.Time.TotalSeconds * GameConstants.PixelsPerSecond;
                Canvas.SetTop(img, top);

                switch(seqElem.Type)
                {
                    case SeqElemType.LeftArrow:
                        img.RenderTransform = new RotateTransform(90, GameConstants.ArrowWidthHeight / 2.0, GameConstants.ArrowWidthHeight / 2.0);
                        Canvas.SetLeft(img, GameConstants.LeftArrowX);
                        break;
                    case SeqElemType.DownArrow:
                        Canvas.SetLeft(img, GameConstants.DownArrowX);
                        break;
                    case SeqElemType.UpArrow:
                        img.RenderTransform = new RotateTransform(180, GameConstants.ArrowWidthHeight / 2.0, GameConstants.ArrowWidthHeight / 2.0);
                        Canvas.SetLeft(img, GameConstants.UpArrowX);
                        break;
                    case SeqElemType.RightArrow:
                        img.RenderTransform = new RotateTransform(-90, GameConstants.ArrowWidthHeight / 2.0, GameConstants.ArrowWidthHeight / 2.0);
                        Canvas.SetLeft(img, GameConstants.RightArrowX);
                        break;
                }                

                //add image to the canvas
                _view.p1Notes.Children.Add(img);

                DebugSongHelper.AddTimeToNotes(_view, seqElem.Type, top);                
            }

            DebugSongHelper.ShowCurrentTimeInsteadPoints(_animation, _view);  
        }

        public void StartAnimation()
        {
            _animation.Begin(); 
        }

        public void Handle(PlayerHitEvent message)
        {
            _view.p1PointsBar.Points = message.Points.ToString();
            _view.p1Health.SetLife(message.Life);
            DebugSongHelper.ShowHitTimeDifferenceInsteadPoints(_view, _animation);
            //TODO: do smth with hit seq elem
        }
    }
}
