using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationServices
{
    public class SequenceCreationService : ISequenceCreationService
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
        public void Start()
        {
 	        throw new NotImplementedException();
        }

        public void Pause()
        {
 	        throw new NotImplementedException();
        }

        public void Stop()
        {
 	        throw new NotImplementedException();
        }
        #endregion
    }
}
