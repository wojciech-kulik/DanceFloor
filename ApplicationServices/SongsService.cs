using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationServices
{
    public class SongsService : ISongsService
    {

        public IReadOnlyCollection<ISong> GetAllSongs()
        {
            //TODO: retrieve songs (new list without references)
            return new List<ISong>();
        }
    }
}
