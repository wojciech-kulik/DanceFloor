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
    public class Game : NotificableObject, IGame, IHandle<GameKeyEvent>, IHandle<GameActionEvent>
    {
        List<ISequenceElement> _alreadyHit = new List<ISequenceElement>();
        IEventAggregator _eventAggregator;
        Timer _missedTimer = new Timer(250);

        public Game(IEventAggregator eventAggregator, IMusicPlayerService musicPlayerService)
        {
            Players = new BindableCollection<IPlayer>();
            MusicPlayerService = musicPlayerService;

            _eventAggregator = eventAggregator;
            eventAggregator.Subscribe(this);

            _missedTimer.Elapsed += lookForMissedNotes_Tick;            
        }

        //NEED TO BE SET BY VIEWMODEL (for example you can get this from animation.GetCurrentTime())
        public Func<TimeSpan> GetSongCurrentTime { get; set; }

        public IMusicPlayerService MusicPlayerService { get; private set; }

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
                    MusicPlayerService.FilePath = value != null ? value.FilePath : null;
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

            //this collection is beeing modified and we access it from thread, so we should make a copy first
            var copyOfAlreadyHit = _alreadyHit.ToList(); 
            var missed = Song.Sequences[player.Difficulty]
                .Except(copyOfAlreadyHit)
                .Where(e => e.Time.TotalSeconds - currenTime < -GameConstants.WorstHitTime)
                .ToList();

            if (missed.Count == 0)
                return;

            _alreadyHit.AddRange(missed.ToList());
            SetLifePoints(player, -GameConstants.MissLifePoints * missed.Count);

            foreach(var missedElem in missed)
            {
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
        //synchronized with UI (if animation time attached to GetSongCurrentTime)
        private ISequenceElement SetPoints(IPlayer player, TimeSpan hitTime, PlayerAction playerAction)
        {           
            SeqElemType type = GameHelper.PlayerActionToSeqElemType(playerAction);
            ISequenceElement hitElement = Song.GetClosestTo(player.Difficulty, hitTime, type, _alreadyHit);

            if (hitElement == null)
            {
                SetLifePoints(player, -GameConstants.WrongMomentOrActionLifePoints);
                return null;
            }                

            if (!hitElement.IsBomb)
            {
                double diff = Math.Abs(hitElement.Time.TotalSeconds - hitTime.TotalSeconds);

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

            _alreadyHit.Add(hitElement);
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

        public void Handle(GameActionEvent message)
        {
            switch (message.GameAction)
            {
                case GameAction.Pause:
                    Pause();
                    break;
                case GameAction.Resume:
                    Resume();
                    break;
                case GameAction.Stop:
                    Stop();
                    break;
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

            MusicPlayerService.Start();
            _missedTimer.Start();
            IsRunning = true;
        }

        public void Resume()
        {
            if (IsRunning)
                return;

            MusicPlayerService.Resume();
            _missedTimer.Start();
            IsRunning = true;
        }

        public void Pause()
        {
            MusicPlayerService.Pause();
            _missedTimer.Stop();
            IsRunning = false;
        }

        public void Stop()
        {
            MusicPlayerService.Stop();
            _missedTimer.Stop();
            IsRunning = false;
        }
        #endregion
    }
}
