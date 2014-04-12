using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public interface IReadOnlySequence : IEnumerable<ISequenceElement>
    {
        Difficulty Difficulty { get; }

        bool AreBombsEnabled { get; set; }

        ISequenceElement GetClosestTo(TimeSpan time, SeqElemType elementType);
    }
}
