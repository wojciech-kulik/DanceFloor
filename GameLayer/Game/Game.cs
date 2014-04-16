using Caliburn.Micro;
using Common;
using GameLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Threading.Tasks;
using System.Diagnostics;

namespace GameLayer
{
    public class Game : NotificableObject, IGame, IHandle<GameKeyEvent>
    {
        List<ISequenceElement> alreadyHit = new List<ISequenceElement>();

        IMusicPlayerService _musicPlayerService;
        IEventAggregator _eventAggregator;
        Timer missedTimer = new Timer(250);

        public Game(IEventAggregator eventAggregator, IMusicPlayerService musicPlayerService)
        {
            Players = new BindableCollection<IPlayer>();
            _musicPlayerService = musicPlayerService;

            _eventAggregator = eventAggregator;
            eventAggregator.Subscribe(this);            

            missedTimer.Elapsed += lookForMissedNotes_Tick;            
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

        private void lookForMissedNotes_Tick(object sender, ElapsedEventArgs e)
        {
            LookForMissedNotes(Players.First());
        }

        private void LookForMissedNotes(IPlayer player)
        {
            var currenTime = GetSongCurrentTime().TotalSeconds;

            var missed = Song.Sequences[player.Difficulty]
                .Except(alreadyHit)
                .Where(e => e.Time.TotalSeconds - currenTime < -GameConstants.WorstHitTime)
                .ToList();

            if (missed.Count == 0)
                return;

            alreadyHit.AddRange(missed.ToList());
            SetLifePoints(player, -GameConstants.MissLifePoints * missed.Count);

            foreach(var missedElem in missed)
            {
                Debug.WriteLine("Life: " + player.Life.ToString());
                _eventAggregator.Publish(new PlayerMissedEvent()
                {
                    PlayerID = player.PlayerID,
                    Points = player.Points,
                    Life = player.Life,
                    Reason = MissReason.NotHit
                });
            }   
        }

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
            if (!IsRunning)
                return;

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
                    Reason = MissReason.WrongMomentOrAction
                });
            }
        }

        #region IPlayable

        #region IsRunning

        private bool _isRunning;

        public bool IsRunning
        {
            get
            {
                return _isRunning;
            }
            protected set
            {
                if (_isRunning != value)
                {
                    _isRunning = value;
                    NotifyPropertyChanged("IsRunning");
                }
            }
        }
        #endregion

        public void Start()
        {
            Players.Clear();
            Players.Add(new Player() { Difficulty = Difficulty.Easy, PlayerID = PlayerID.Player1, IsGameOver = false, Life = GameConstants.FullLife, Points = 0 });
            Players.Add(new Player() { Difficulty = Difficulty.Easy, PlayerID = PlayerID.Player2, IsGameOver = false, Life = GameConstants.FullLife, Points = 0 });

            missedTimer.Start();
            IsRunning = true;
        }

        public void Resume()
        {
            missedTimer.Start();
            IsRunning = true;
        }

        public void Pause()
        {
            missedTimer.Stop();
            IsRunning = false;
        }

        public void Stop()
        {
            missedTimer.Stop();
            IsRunning = false;
        }
        #endregion
    }
}
