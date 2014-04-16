using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationServices
{
    public class SequenceCreationService : NotificableObject, ISequenceCreationService
    {
        Dictionary<Difficulty, ISequence> sequences = new Dictionary<Difficulty,ISequence>();
        IMusicPlayerService musicPlayerService;

        public SequenceCreationService(IMusicPlayerService musicPlayerService)
        {
            this.musicPlayerService = musicPlayerService;
        }


        public ISong Song { get; set; }

        public Difficulty Difficulty { get; set; }



        public void SaveSequenceToFile(string path)
        {
            //TODO: save to file
        }


        #region IEnumerable
        public IEnumerator<ISequence> GetEnumerator()
        {
            return sequences.Values.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return sequences.Values.GetEnumerator();
        }
        #endregion
    
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
            //TODO: implement
            IsRunning = true;
        }

        public void Resume()
        {
            //TODO: implement
            IsRunning = true;
        }

        public void Pause()
        {
            //TODO: Implement
            IsRunning = false;
        }

        public void Stop()
        {
            //TODO: implement
            IsRunning = false;
        }
        #endregion
    }
}
