using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public interface ISong
    {
        string Title { get; set; }

        string FilePath { get; set; }

        string Author { get; set; }

        DateTime CreateDate { get; set; }

        string BackgroundPath { get; set; }
    }
}
