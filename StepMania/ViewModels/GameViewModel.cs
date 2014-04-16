using Caliburn.Micro;
using Common;
using GameLayer;
using StepMania.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace StepMania.ViewModels
{
    public class GameViewModel : BaseViewModel, IHandle<GameKeyEvent>
    {
        GameView _view;
        Storyboard _animation;
        IMusicPlayerService _musicPlayerService;

        public GameViewModel(IEventAggregator eventAggregator, IMusicPlayerService musicPlayerService)
            : base(eventAggregator)
        {
            _musicPlayerService = musicPlayerService;
        }

        protected override void OnViewAttached(object view, object context)
        {
            _view = view as GameView;
            _animation = _view.Resources.Values.OfType<Storyboard>().First() as Storyboard;
        }

        TimeSpan GetSongCurrentTime()
        {
            return _animation.GetCurrentTime();
        }

        void PlayerHit( PlayerID playerId, PlayerAction playerAction)
        {            
            string isHit;

            var currentTime = GetSongCurrentTime();
            var note = _view.p1Notes.Children.OfType<Image>().ToList().FirstOrDefault(n => Math.Abs(((double)n.Tag) - currentTime.TotalSeconds) < 0.2);

            if (note != null)
                isHit = "TAK";
            else
                isHit = "NIE";
            if (note != null)
                isHit += " " + (((double)note.Tag) - currentTime.TotalSeconds).ToString("N2");
            _view.p1PointsBar.Points = isHit;


            #if DEBUG_HIT_TIME
            _animation.Pause();
            #endif
        }

        public void Handle(GameKeyEvent message)
        {
            PlayerHit(message.PlayerId, message.PlayerAction);
        }
    }
}
