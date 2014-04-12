using Caliburn.Micro;
using Common;
using StepMania.Models.Games;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StepMania.Models.Sequences
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
            throw new NotImplementedException();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
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
