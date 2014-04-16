using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationServices
{
    public class MusicPlayerService : NotificableObject, IMusicPlayerService
    {
        #region CurrentTime

        private TimeSpan _currentTime;

        public TimeSpan CurrentTime
        {
            get
            {
                return _currentTime;
            }
            protected set
            {
                if (_currentTime != value)
                {
                    _currentTime = value;
                    NotifyPropertyChanged("CurrentTime");
                }
            }
        }
        #endregion

        #region Duration

        private TimeSpan _duration;

        public TimeSpan Duration
        {
            get
            {
                return _duration;
            }
            protected set
            {
                if (_duration != value)
                {
                    _duration = value;
                    NotifyPropertyChanged("Duration");
                }
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
            protected set
            {
                if (_song != value)
                {
                    _song = value;
                    NotifyPropertyChanged("Song");
                }
            }
        }
        #endregion

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
    }
}
