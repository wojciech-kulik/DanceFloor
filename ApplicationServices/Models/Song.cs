using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace ApplicationServices
{
    [Serializable]
    public class Song : NotificableObject, ISong
    {
        public Song()
        {
            Sequences = new Dictionary<Difficulty, IReadOnlySequence>();
        }

        public Dictionary<Difficulty, IReadOnlySequence> Sequences { get; set; }

        #region Title

        private string _title;

        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                if (_title != value)
                {
                    _title = value;
                    NotifyPropertyChanged("Title");
                }
            }
        }
        #endregion

        #region Artist

        private string _artist;

        public string Artist
        {
            get
            {
                return _artist;
            }
            set
            {
                if (_artist != value)
                {
                    _artist = value;
                    NotifyPropertyChanged("Artist");
                }
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
                    _filePath = value;
                    NotifyPropertyChanged("FilePath");
                }
            }
        }
        #endregion

        #region Author

        private string _author;

        public string Author
        {
            get
            {
                return _author;
            }
            set
            {
                if (_author != value)
                {
                    _author = value;
                    NotifyPropertyChanged("Author");
                }
            }
        }
        #endregion

        #region CreateDate

        private DateTime _createDate;

        public DateTime CreateDate
        {
            get
            {
                return _createDate;
            }
            set
            {
                if (_createDate != value)
                {
                    _createDate = value;
                    NotifyPropertyChanged("CreateDate");
                }
            }
        }
        #endregion

        #region BackgroundPath

        private string _backgroundPath;

        public string BackgroundPath
        {
            get
            {
                return _backgroundPath;
            }
            set
            {
                if (_backgroundPath != value)
                {
                    _backgroundPath = value;
                    NotifyPropertyChanged("BackgroundPath");
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
            set
            {
                if (_duration != value)
                {
                    _duration = value;
                    NotifyPropertyChanged("Duration");
                }
            }
        }
        #endregion

        public ISequenceElement GetClosestTo(Difficulty difficulty, TimeSpan time, SeqElemType elementType, IList<ISequenceElement> alreadyHit)
        {
            return Sequences[difficulty].GetClosestTo(time, elementType, alreadyHit);
        }

        public void LoadFromFile(string path)
        {
            if (!File.Exists(path))
                throw new ArgumentException("Nieprawidłowa ścieżka do pliku: \n" + path);

            Song song;
            using (var stream = File.OpenRead(path))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                song = (Song)formatter.Deserialize(stream);
            }

            Sequences = song.Sequences;
            Title = song.Title;
            Artist = song.Artist;
            FilePath = song.FilePath;
            Author = song.Author;
            CreateDate = song.CreateDate;
            BackgroundPath = song.BackgroundPath;
            Duration = song.Duration;
        }

        public void SaveToFile(string path)
        {
            using (var stream = File.Create(path))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, this);
            }
        }
    }
}
