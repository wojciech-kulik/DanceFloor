using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StepMania.ViewModels
{
    public class MainWindowViewModel : Conductor<IScreen>.Collection.OneActive
    {
        public MainWindowViewModel()
        {
            DisplayName = "StepMania";
        }

        protected override void OnActivate()
        {            
            ActivateItem(IoC.Get<GameViewModel>());
            //ActivateItem(IoC.Get<MenuViewModel>());
        }
    }
}
