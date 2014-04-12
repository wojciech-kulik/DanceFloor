using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationServices
{
    public class HighScoresService : IHighScoresService
    {
        public IList<IHighScore> GetAll()
        {
            //TODO: implement
            return new List<IHighScore>();
        }

        public void Add(IHighScore highScore)
        {
            //TODO: implement
        }

        public bool IsBest(int points, Difficulty difficulty, ISong song)
        {
            //TODO: implement
            return true;
        }

        #region IEnumerable
        public IEnumerator<IHighScore> GetEnumerator()
        {
            //TODO: implement
            throw new NotImplementedException();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            //TODO: implement
            throw new NotImplementedException();
        }
        #endregion
    }
}
