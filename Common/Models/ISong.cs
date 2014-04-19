using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public interface ISong : IBaseModel
    {
        string Title { get; set; }

        string Artist { get; set; }

        string FilePath { get; set; }

        string Author { get; set; }

        DateTime CreateDate { get; set; }

        string BackgroundPath { get; set; }

        TimeSpan Duration { get; set; }

        bool IsSelected { get; set; }

        Dictionary<Difficulty, IReadOnlySequence> Sequences { get; set; }



        ISequenceElement GetClosestTo(Difficulty difficulty, TimeSpan time, SeqElemType elementType, IList<ISequenceElement> alreadyHit);

        void LoadFromFile(string path);

        void SaveToFile(string path);
    }
}
