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
    public class Sequence : NotificableObject, ISequence
    {
        private BindableCollection<ISequenceElement> SequenceElements = new BindableCollection<ISequenceElement>();

        #region Difficulty

        private Difficulty _difficulty;

        public Difficulty Difficulty
        {
            get
            {
                return _difficulty;
            }
            set
            {
                if (_difficulty != value)
                {
                    _difficulty = value;
                    NotifyPropertyChanged("Difficulty");
                }
            }
        }
        #endregion

        #region IReadOnlySequence
        public IEnumerator<ISequenceElement> GetEnumerator()
        {
            return SequenceElements.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return SequenceElements.GetEnumerator();
        }

        #region AreBombsEnabled

        private bool _areBombsEnabled;

        public bool AreBombsEnabled
        {
            get
            {
                return _areBombsEnabled;
            }
            set
            {
                if (_areBombsEnabled != value)
                {
                    _areBombsEnabled = value;
                    NotifyPropertyChanged("AreBombsEnabled");
                }
            }
        }
        #endregion

        #endregion

        public void AddElement(ISequenceElement element)
        {
            SequenceElements.Add(element);
        }

        public void Clear()
        {
            SequenceElements.Clear();
        }

        public ISequenceElement GetClosestTo(TimeSpan time, SeqElemType elementType)
        {
            if (time.TotalSeconds <= SequenceElements.First().Time.TotalSeconds)
                return SequenceElements.First();

            //SequenceElements uporządkowany rosnąco wg Time
            for(int i = 0; i < SequenceElements.Count - 1; i++)
            {
                int timeToCurrent = (int)SequenceElements[i].Time.TotalMilliseconds - (int)time.TotalMilliseconds;
                int timeToNext = (int)SequenceElements[i+1].Time.TotalMilliseconds - (int)time.TotalMilliseconds;

                //jezeli to ostatnia iteracja lub
                //jezeli najbliższym elementem jest jeden z dwoch aktualnie sprawdzanych
                if (i + 1 == SequenceElements.Count - 1 || timeToCurrent <= 0 && timeToNext >= 0)
                {
                    return Math.Abs(timeToCurrent) < Math.Abs(timeToNext) ? SequenceElements[i] : SequenceElements[i+1];
                }
            }

            return null;
        }
        
    }
}
