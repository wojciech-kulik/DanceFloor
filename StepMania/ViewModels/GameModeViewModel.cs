using Caliburn.Micro;
using Common;
using GameLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StepMania.ViewModels
{
    public class GameModeViewModel : BaseViewModel, IHandle<GameKeyEvent>
    {
        public GameModeViewModel(IEventAggregator eventAggregator, IGame game)
            :base(eventAggregator)
        {
            Game = game;
        }

        #region Game

        private IGame _game;

        public IGame Game
        {
            get
            {
                return _game;
            }
            set
            {
                if (_game != value)
                {
                    _game = value;
                    NotifyOfPropertyChange(() => Game);
                }
            }
        }
        #endregion

        public void Handle(GameKeyEvent message)
        {
            if (!IsActive)
                return;

            var player = message.PlayerId == PlayerID.Player1 ? Game.Player1 : Game.Player2;

            switch (message.PlayerAction)
            {
                case PlayerAction.Enter:
                    _eventAggregator.Publish(new NavigationExEvent() { NavDestination = NavDestination.Game, PageSettings = (vm) => (vm as GameViewModel).Game = Game });
                    break;
                case PlayerAction.Back:
                    _eventAggregator.Publish(new NavigationExEvent() { NavDestination = NavDestination.SongsList, PageSettings = (vm) => (vm as SongsListViewModel).SelectedSong = Game.Song });
                    break;
                case PlayerAction.Right:
                    Game.IsMultiplayer = true;
                    NotifyOfPropertyChange(() => Game);
                    break;
                case PlayerAction.Left:
                    Game.IsMultiplayer = false;
                    NotifyOfPropertyChange(() => Game);
                    break;
                case PlayerAction.Down:
                    player.Difficulty = (Difficulty)(((int)player.Difficulty + 1) % 3);
                    NotifyOfPropertyChange(() => Game);
                    break;
                case PlayerAction.Up:
                {
                    int newVal = (int)player.Difficulty - 1;
                    if (newVal < 0)
                        player.Difficulty = Difficulty.Hard;
                    else
                        player.Difficulty = (Difficulty)newVal;
                    NotifyOfPropertyChange(() => Game);
                    break;
                }
            }
        }
    }
}
