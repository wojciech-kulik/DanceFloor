using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StepMania.ViewModels
{
    public class GameViewModel
    {
        IMusicPlayerService _musicPlayerService;

        public GameViewModel()
        {
            bool doo = false ;
        }

        public GameViewModel(IMusicPlayerService musicPlayerService)
        {
            _musicPlayerService = musicPlayerService;
        }
    }
}
