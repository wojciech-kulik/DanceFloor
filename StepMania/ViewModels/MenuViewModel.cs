using Caliburn.Micro;
using Common;
using GameLayer;
using StepMania.Controls;
using StepMania.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace StepMania.ViewModels
{
    public class MenuViewModel : BaseViewModel, IHandle<GameKeyEvent>, IHandle<PopupClosedEvent>
    {
        MenuView _view;
        int _activeButton = 0;
        int _buttonsCount;

        public MenuViewModel(IEventAggregator eventAggregator)
            : base(eventAggregator)
        {

        }

        protected override void OnViewAttached(object view, object context)
        {
            _view = view as MenuView;

            SetupButtons();
            _buttonsCount = _view.menuPanel.Children.OfType<MenuButton>().Count();
            _activeButton = 0;
            ActivateButton(_activeButton);
        }

        private void SetupButtons()
        {
            foreach (var b in _view.menuPanel.Children.OfType<MenuButton>())
            {
                b.ButtonBackground = new LinearGradientBrush() { EndPoint = new Point(0.5, 1), StartPoint = new Point(0.5, 0) };
                (b.ButtonBackground as LinearGradientBrush).GradientStops.Add(new GradientStop() { Offset = 1 });
                (b.ButtonBackground as LinearGradientBrush).GradientStops.Add(new GradientStop());
            }
        }

        private void ActivateButton(int index)
        {
            foreach (var b in _view.menuPanel.Children.OfType<MenuButton>())
            {
                (b.ButtonBackground as LinearGradientBrush).GradientStops[0].Color = new Color() { A = 0xDE, R = 0x2B, G = 0xAC, B = 0x1F };
                (b.ButtonBackground as LinearGradientBrush).GradientStops[1].Color = new Color() { A = 0xDE, R = 0x73, G = 0xF0, B = 0x64 };
            }

            var button = _view.menuPanel.Children.OfType<MenuButton>().Skip(index).First();
            (button.ButtonBackground as LinearGradientBrush).GradientStops[0].Color = new Color() { A = 0xED, R = 0x8B, G = 0x8B, B = 0x8B };
            (button.ButtonBackground as LinearGradientBrush).GradientStops[1].Color = new Color() { A = 0xED, R = 0xFF, G = 0xFF, B = 0xFF };
        }

        public void Handle(GameKeyEvent message)
        {
            if (!IsActive || IsPopupShowing)
                return;
            
            if (message.PlayerAction == PlayerAction.Enter)
            {
                ISong song = new Song()
                {
                    FilePath = @"Utwory\Billy Talent - Diamond on a Landmine.mp3"
                };

                switch(_activeButton)
                {
                    case 0:
                        _eventAggregator.Publish(new NavigationEvent() { NavDestination = NavDestination.SongsList });
                        break;
                    case 3:
                        _eventAggregator.Publish(new NavigationExEvent()
                        {
                            NavDestination = NavDestination.Record,
                            PageSettings = (vm) =>
                            {
                                (vm as RecordSequenceViewModel).Song = song;
                                (vm as RecordSequenceViewModel).Difficulty = Difficulty.Easy;
                            }
                        });
                        break;
                    case 4:
                        CloseGame();
                        break;
                }
            }
            else if (message.PlayerAction == PlayerAction.Back)
            {
                CloseGame();
            }
            else if (message.PlayerAction == PlayerAction.Down)
            {
                _activeButton++;
                if (_activeButton >= _buttonsCount)
                    _activeButton = 0;
                ActivateButton(_activeButton);
            }
            else if (message.PlayerAction == PlayerAction.Up)
            {
                _activeButton--;
                if (_activeButton < 0)
                    _activeButton = _buttonsCount - 1;
                ActivateButton(_activeButton);
            }
        }

        private void CloseGame()
        {
            IsPopupShowing = true;
            _eventAggregator.Publish(new ShowPopupEvent() { PopupType = PopupType.ClosingPopup});
        }

        public void Handle(PopupClosedEvent message)
        {
            if (!IsActive)
                return;

            IsPopupShowing = false;
            if (message.YesSelected)
                Application.Current.MainWindow.Close();
        }
    }
}
