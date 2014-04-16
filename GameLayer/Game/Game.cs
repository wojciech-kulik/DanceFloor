using Caliburn.Micro;
using Common;
using GameLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLayer
{
    public class Game : NotificableObject, IGame, IHandle<GameKeyEvent>
    {
        List<ISequenceElement> alreadyHit = new List<ISequenceElement>();

        IMusicPlayerService musicPlayerService;
        IEventAggregator _eventAggregator;

        public Game(IEventAggregator eventAggregator, IMusicPlayerService musicPlayerService)
        {
            Players = new BindableCollection<IPlayer>();
            this.musicPlayerService = musicPlayerService;

            _eventAggregator = eventAggregator;
            eventAggregator.Subscribe(this);

            Players.Add(new Player() { Difficulty = Difficulty.Easy, PlayerID = PlayerID.Player1, IsGameOver = false, Life = 100, Points = 0 });
            Players.Add(new Player() { Difficulty = Difficulty.Easy, PlayerID = PlayerID.Player2, IsGameOver = false, Life = 100, Points = 0 });
        }

        //NEED TO BE SET BY VIEWMODEL (for example you can get this from animation.GetCurrentTime())
        public Func<TimeSpan> GetSongCurrentTime { get; set; }

        #region Players

        private BindableCollection<IPlayer> _players;

        public BindableCollection<IPlayer> Players
        {
            get
            {
                return _players;
            }
            set
            {
                if (_players != value)
                {
                    _players = value;
                    NotifyPropertyChanged("Players");
                }
            }
        }
        #endregion

        #region Multiplayer
        public bool Multiplayer
        {
            get
            {
                return Players.Count > 1;
            }
        }
        #endregion

        #region Song

        private ISong _song;

        public ISong Song
        {
            get
            {
                return _song;
            }
            set
            {
                if (_song != value)
                {
                    _song = value;
                    NotifyPropertyChanged("Song");
                }
            }
        }
        #endregion

        private void SetLifePoints(IPlayer player, int deltaLifePoints)
        {
            player.Life = Math.Max(0, player.Life + deltaLifePoints);
            if (player.Life == 0)
                player.IsGameOver = true;
        }

        //returns null if nothing hit
        private ISequenceElement SetPoints(IPlayer player, TimeSpan hitTime, PlayerAction playerAction)
        {
            SeqElemType type = GameHelper.PlayerActionToSeqElemType(playerAction);
            ISequenceElement hitElement = Song.GetClosestTo(player.Difficulty, hitTime, type, alreadyHit);

            if (hitElement == null)
            {
                SetLifePoints(player, -GameConstants.MissLifePoints);
                return null;
            }             

            double diff = Math.Abs(hitElement.Time.TotalSeconds - hitTime.TotalSeconds);

            if (!hitElement.IsBomb)
            {
                if (diff <= GameConstants.BestHitTime)
                    player.Points += GameConstants.BestHitPoints;
                else if (diff <= GameConstants.MediumHitTime)
                    player.Points += GameConstants.MediumHitPoints;
                else if (diff <= GameConstants.WorstHitTime)
                    player.Points += GameConstants.WorstHitPoints; 
            }
            else
            {
                SetLifePoints(player, -GameConstants.BombLifePoints);
            }

            alreadyHit.Add(hitElement);
            return hitElement;
        }

        public void Handle(GameKeyEvent message)
        {
            IPlayer player = Players.First(p => p.PlayerID == message.PlayerId);
            ISequenceElement hitElem = SetPoints(player, GetSongCurrentTime(), message.PlayerAction);

            if (hitElem != null)
            {
                _eventAggregator.Publish(new PlayerHitEvent()
                {
                    PlayerID = player.PlayerID,                    
                    Life = player.Life,
                    Points = player.Points,
                    IsBomb = hitElem.IsBomb,
                    SequenceElement = hitElem
                });
            }
            else
            {
                _eventAggregator.Publish(new PlayerMissedEvent()
                {
                    PlayerID = player.PlayerID,
                    Points = player.Points,
                    Life = player.Life,
                    PlayerAction = message.PlayerAction
                });
            }
        }

        #region IPlayable
        public void Start()
        {
            throw new NotImplementedException();
        }

        public void Pause()
        {
            throw new NotImplementedException();
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
