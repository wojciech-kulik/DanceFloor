using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class PausePopupEvent : ClosePopupEvent
    {
        public bool Resume { get; set; }

        public bool PlayAgain { get; set; }

        public bool Exit { get; set; }
    }
}
