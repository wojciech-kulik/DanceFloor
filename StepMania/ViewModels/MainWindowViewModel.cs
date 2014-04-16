using Caliburn.Micro;
using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace StepMania.ViewModels
{
    public class MainWindowViewModel : Conductor<IScreen>.Collection.OneActive
    {
        ISettingsService _settingsService;

        public MainWindowViewModel(ISettingsService settingsService)
        {
            DisplayName = "StepMania";
            _settingsService = settingsService;
        }

        protected override void OnActivate()
        {            
            ActivateItem(IoC.Get<GameViewModel>());
            //ActivateItem(IoC.Get<MenuViewModel>());
        }

        protected override void OnViewAttached(object view, object context)
        {
            base.OnViewAttached(view, context);
            (view as Window).PreviewKeyUp += _settingsService.HandleKeyUp;
        }
    }
}
