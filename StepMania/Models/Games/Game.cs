using Caliburn.Micro;
using Common;
using StepMania.Models.Sequences;
using StepMania.Models.Songs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StepMania.Models.Games
{
    public class Game : NotificableObject
    {
        public Game()
        {
            Players = new BindableCollection<Player>();
        }

        #region Players

        private BindableCollection<Player> _players;

        public BindableCollection<Player> Players
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

        private Song _song;

        public Song Song
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
    }
}
