using Caliburn.Micro;
using Common;
using GameLayer;
using StepMania.Constants;
using StepMania.DebugHelpers;
using StepMania.Views;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

namespace StepMania.ViewModels
{
    public class GameViewModel : BaseViewModel, IHandle<PlayerHitEvent>, IHandle<PlayerMissedEvent>, IHandle<GameActionEvent>
    {
        GameView _view;
        Storyboard _animation;
        IGame _game;

        public GameViewModel(IEventAggregator eventAggregator, IGame game)
            : base(eventAggregator)
        {
            _game = game;
            game.GetSongCurrentTime = () => _animation.GetCurrentTime();
        }

        protected override void OnViewAttached(object view, object context)
        {           
            _view = view as GameView;
            _animation = _view.Resources.Values.OfType<Storyboard>().First() as Storyboard;

            LoadSong(DebugSongHelper.GenerateSong(270));
            StartGame();
        }

        public void LoadSong(ISong song)
        {
            _game.Song = song;
            _view.p1Notes.Children.Clear();

            _animation.Children.First().Duration = song.Duration;
            (_animation.Children.First() as DoubleAnimation).From = -GameUIConstants.ArrowWidthHeight;
            (_animation.Children.First() as DoubleAnimation).To = -(GameUIConstants.PixelsPerSecond * song.Duration.TotalSeconds + GameUIConstants.ArrowWidthHeight);

            foreach(var seqElem in song.Sequences[Difficulty.Easy])
            {
                //load and prepare image
                var bitmap = new BitmapImage(new Uri("pack://application:,,,/StepMania;component/Images/active_arrow.png"));
                Image img = new Image() { Width = GameUIConstants.ArrowWidthHeight, Height = GameUIConstants.ArrowWidthHeight, Source = bitmap, Tag = seqElem };

                double top = seqElem.Time.TotalSeconds * GameUIConstants.PixelsPerSecond;
                Canvas.SetTop(img, top);

                switch(seqElem.Type)
                {
                    case SeqElemType.LeftArrow:
                        img.RenderTransform = new RotateTransform(90, GameUIConstants.ArrowWidthHeight / 2.0, GameUIConstants.ArrowWidthHeight / 2.0);
                        Canvas.SetLeft(img, GameUIConstants.LeftArrowX);
                        break;
                    case SeqElemType.DownArrow:
                        Canvas.SetLeft(img, GameUIConstants.DownArrowX);
                        break;
                    case SeqElemType.UpArrow:
                        img.RenderTransform = new RotateTransform(180, GameUIConstants.ArrowWidthHeight / 2.0, GameUIConstants.ArrowWidthHeight / 2.0);
                        Canvas.SetLeft(img, GameUIConstants.UpArrowX);
                        break;
                    case SeqElemType.RightArrow:
                        img.RenderTransform = new RotateTransform(-90, GameUIConstants.ArrowWidthHeight / 2.0, GameUIConstants.ArrowWidthHeight / 2.0);
                        Canvas.SetLeft(img, GameUIConstants.RightArrowX);
                        break;
                }                

                //add image to the canvas
                _view.p1Notes.Children.Add(img);

                DebugSongHelper.AddTimeToNotes(_view, seqElem.Type, top);                
            }

            DebugSongHelper.ShowCurrentTimeInsteadPoints(_animation, _game.MusicPlayerService, _view);  
        }

        bool IsRunning { get; set; }
        public void StartGame()
        {
            _animation.Begin();
            _game.Start();            
            IsRunning = true;
        }

        public void ResumeGame()
        {
            //sometimes _game.MusicPlayerService.CurrentTime returns 0  o_O  so we have to wait for a normal value
            TimeSpan currentTime;
            while ((currentTime = _game.MusicPlayerService.CurrentTime).TotalSeconds == 0) ; //TODO: possible deadloop, but shouldn't happen

            _animation.Resume();
            _animation.Seek(currentTime); //synchronize animation with music (difference will be about <= 0.05s, which is enough)
            _game.Resume();                        
            IsRunning = true;
        }

        public void PauseGame()
        {
            _animation.Pause();
            _game.Pause();
            IsRunning = false;
        }

        public void StopGame()
        {
            _animation.Stop();
            _game.Stop();
            IsRunning = false;
        }

        public void Handle(PlayerHitEvent message)
        {
            if (!IsActive)
                return;

            var notesPanel = message.PlayerID == PlayerID.Player1 ? _view.p1Notes     : _view.p2Notes;
            var pointsBar  = message.PlayerID == PlayerID.Player1 ? _view.p1PointsBar : _view.p2PointsBar;
            var healthBar  = message.PlayerID == PlayerID.Player1 ? _view.p1Health    : _view.p2Health;

            //set player status
            pointsBar.Points = message.Points.ToString();
            healthBar.SetLife(message.Life);
            DebugSongHelper.ShowHitTimeDifferenceInsteadPoints(_view, _animation);

            //remove hit element
            var toRemove = notesPanel.Children.OfType<Image>().FirstOrDefault(img => img.Tag == message.SequenceElement);
            if (toRemove != null)
                notesPanel.Children.Remove(toRemove);
        }

        public void Handle(PlayerMissedEvent message)
        {
            if (!IsActive)
                return;

            var pointsBar = message.PlayerID == PlayerID.Player1 ? _view.p1PointsBar : _view.p2PointsBar;
            var healthBar = message.PlayerID == PlayerID.Player1 ? _view.p1Health    : _view.p2Health;

            pointsBar.Points = message.Points.ToString();
            healthBar.SetLife(message.Life);

            //todo: play miss sound
        }

        public void Handle(GameActionEvent message)
        {
            if (!IsActive)
                return;

            switch(message.GameAction)
            {
                case GameAction.Pause:
                    PauseGame();
                    break;
                case GameAction.Resume:
                    ResumeGame();
                    break;
                case GameAction.Stop:
                    StopGame();
                    break;
            }
        }
    }
}
