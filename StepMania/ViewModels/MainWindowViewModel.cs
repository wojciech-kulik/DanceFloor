using Caliburn.Micro;
using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace StepMania.ViewModels
{
    public class MainWindowViewModel : Conductor<IScreen>.Collection.OneActive, IHandle<NavigationEvent>
    {
        ISettingsService _settingsService;
        IEventAggregator _eventAggregator;

        public MainWindowViewModel(ISettingsService settingsService, IEventAggregator eventAggregator)
        {
            DisplayName = "StepMania";
            _settingsService = settingsService;
            _eventAggregator = eventAggregator;
            _eventAggregator.Subscribe(this);
        }

        protected override void OnActivate()
        {            
            ActivateItem(IoC.Get<MenuViewModel>());
        }

        protected override void OnViewAttached(object view, object context)
        {
            base.OnViewAttached(view, context);
            (view as Window).PreviewKeyUp += _settingsService.HandleKeyUp;
        }

        public void Handle(NavigationEvent message)
        {
            switch (message.NavDestination)
            {
                case NavDestination.MainMenu:
                    ActivateItem(IoC.Get<MenuViewModel>());
                    break;
                case NavDestination.SongsList:
                    ActivateItem(IoC.Get<SongsListViewModel>());
                    break;
                case NavDestination.Game:
                    ActivateItem(IoC.Get<GameViewModel>());
                    break;
                case NavDestination.CloseGame:
                    Application.Current.MainWindow.Close();
                    break;
            }
        }
    }
}
