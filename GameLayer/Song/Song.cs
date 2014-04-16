using GameLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace GameLayer
{
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

        public ISequenceElement GetClosestTo(Difficulty difficulty, TimeSpan time, SeqElemType elementType)
        {
            return Sequences[difficulty].GetClosestTo(time, elementType);
        }
    }
}
