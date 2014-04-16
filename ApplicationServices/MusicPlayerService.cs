using Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ApplicationServices
{
    public class MusicPlayerService : NotificableObject, IMusicPlayerService
    {
        #region CurrentTime

        public TimeSpan CurrentTime
        {
            get
            {
                return _soundPlayer.Position;
            }
        }
        #endregion

        #region Duration

        public TimeSpan Duration
        {
            get
            {
                return _soundPlayer.NaturalDuration.TimeSpan;
            }
        }
        #endregion

        #region FilePath

        private string _filePath;

        public string FilePath
        {
            get
            {
                return _filePath;
            }
            set
            {
                if (_filePath != value)
                {
                    if (!File.Exists(value))
                    {
                        throw new ArgumentException("Podany plik muzyczny nie istnieje:\n" + FilePath);
                    }                    
                    _soundPlayer.Open(new Uri(value));

                    _filePath = value;
                    NotifyPropertyChanged("FilePath");
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

        private MediaPlayer _soundPlayer = new MediaPlayer();

        public void Start()
        {
            _soundPlayer.Play();
            IsRunning = true;
        }

        public void Resume()
        {
            _soundPlayer.Play();
            IsRunning = true;
        }

        public void Pause()
        {
            _soundPlayer.Pause();
            IsRunning = false;
        }

        public void Stop()
        {
            _soundPlayer.Stop();
            _soundPlayer.Close();
            IsRunning = false;
        }
    }
}
