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
        IMusicPlayerService musicPlayerService;
        IEventAggregator _eventAggregator;

        public Game(IEventAggregator eventAggregator, IMusicPlayerService musicPlayerService)
        {
            Players = new BindableCollection<IPlayer>();
            this.musicPlayerService = musicPlayerService;

            _eventAggregator = eventAggregator;
            eventAggregator.Subscribe(this);

            Players.Add(new Player() { Difficulty = Difficulty.Easy, IsGameOver = false, Life = 100, Points = 0 });
            Players.Add(new Player() { Difficulty = Difficulty.Easy, IsGameOver = false, Life = 100, Points = 0 });
        }

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

        private HitResult GetPoints(Player player, TimeSpan hitTime, PlayerAction playerAction)
        {
            SeqElemType type = SeqElemType.DownArrow; //TODO: map from player Action
            ISequenceElement hitElement = Song.GetClosestTo(player.Difficulty, hitTime, type);

            //TODO: calculate points and life
            int points = 0; 
            int life = 0;

            return new HitResult(points, life, hitElement.Type == SeqElemType.Bomb);
        }

        public void Handle(GameKeyEvent message)
        {
            Players[0].Points += 10;
            _eventAggregator.Publish(new PlayerHitEvent() 
            { 
                PlayerID = Common.PlayerID.Player1, 
                IsBomb = false,
                Life = 100 - new Random().Next(0, 50), 
                Points = Players[0].Points,
                SequenceElement = null
            }); 
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
