using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public interface IEditableSequence : IReadOnlySequence
    {
        new Difficulty Difficulty { get; set; }

        void AddElement(ISequenceElement element);

        void Clear();
    }
}
