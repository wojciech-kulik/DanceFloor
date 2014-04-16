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
                return _mediaPlayer.Position;
            }
        }
        #endregion

        #region Duration

        public TimeSpan Duration
        {
            get
            {
                return _mediaPlayer.NaturalDuration.TimeSpan;
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
                    if (!String.IsNullOrWhiteSpace(value))
                    {
                        if (!File.Exists(value))
                        {
                            throw new ArgumentException("Podany plik muzyczny nie istnieje:\n" + FilePath);
                        }
                        _mediaPlayer.Open(new Uri(value));
                    }

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

        private MediaPlayer _mediaPlayer = new MediaPlayer();

        public void Start()
        {
            _mediaPlayer.Play();
            IsRunning = true;
        }

        public void Resume()
        {
            _mediaPlayer.Play();
            IsRunning = true;
        }

        public void Pause()
        {
            _mediaPlayer.Pause();
            IsRunning = false;
        }

        public void Stop()
        {
            _mediaPlayer.Stop();
            _mediaPlayer.Close();
            IsRunning = false;
        }
    }
}
