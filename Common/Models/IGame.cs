using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public interface IGame : IPlayable
    {
        Func<TimeSpan> GetSongCurrentTime { get; set; }

        BindableCollection<IPlayer> Players { get; set; }

        bool Multiplayer { get; }

        ISong Song { get; set; }
    }
}
