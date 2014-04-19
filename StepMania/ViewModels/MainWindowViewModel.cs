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
    public class MainWindowViewModel : Conductor<IScreen>.Collection.OneActive, IHandle<NavigationEvent>, IHandle<NavigationExEvent>, IHandle<PopupClosedEvent>, IHandle<ShowPopupEvent>
    {
        ISettingsService _settingsService;
        IEventAggregator _eventAggregator;

        #region PopupItem

        private IPopup _popupItem;

        public IPopup PopupItem
        {
            get
            {
                return _popupItem;
            }
            set
            {
                if (_popupItem != value)
                {
                    _popupItem = value;
                    NotifyOfPropertyChange(() => PopupItem);
                }
            }
        }
        #endregion

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

            //DebugHelpers.DebugSongHelper.GenerateRandomSongsDB();
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
            }
        }

        public void Handle(NavigationExEvent message)
        {
            IScreen vm = null;

            switch (message.NavDestination)
            {
                case NavDestination.Game:
                    vm = IoC.Get<GameViewModel>();
                    break;

                case NavDestination.GameMode:
                    vm = IoC.Get<GameModeViewModel>();
                    break;

                case NavDestination.SongsList:
                    vm = IoC.Get<SongsListViewModel>();
                    break;

                case NavDestination.Record:
                    vm = IoC.Get<RecordSequenceViewModel>();
                    break;
            }

            if (vm != null)
            {
                if (message.PageSettings != null)
                    message.PageSettings(vm);
                ActivateItem(vm);
            }
        }

        public void Handle(PopupClosedEvent message)
        {
            PopupItem.IsShowing = false;
            PopupItem = null;
        }

        public void Handle(ShowPopupEvent message)
        {
            switch (message.PopupType)
            {
                case PopupType.ClosingPopup:
                    PopupItem = IoC.Get<ClosingPopupViewModel>();
                    PopupItem.IsShowing = true;
                    break;
            }
        }
    }
}
