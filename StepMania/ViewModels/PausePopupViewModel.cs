using Caliburn.Micro;
using Common;
using GameLayer;
using StepMania.Constants;
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
    public class PausePopupViewModel : BaseViewModel, IPopup, IHandle<GameKeyEvent>
    {
        PausePopupView _view;
        int _selectedIndex = 0;
        List<MenuButton> buttons = new List<MenuButton>();

        public bool IsShowing { get; set; }

        #region Message

        private string _message;

        public string Message
        {
            get
            {
                return _message;
            }
            set
            {
                if (_message != value)
                {
                    _message = value;
                    NotifyOfPropertyChange(() => Message);
                }
            }
        }
        #endregion

        public PausePopupViewModel(IEventAggregator eventAggregator)
            : base(eventAggregator)
        {
        }

        protected override void OnViewAttached(object view, object context)
        {
            _view = view as PausePopupView;

            buttons.Add(_view.btnResume);
            buttons.Add(_view.btnPlayAgain);
            buttons.Add(_view.btnExit);
        }

        public void Handle(GameKeyEvent message)
        {
            if (!IsShowing)
                return;

            if (message.PlayerAction == PlayerAction.Up || message.PlayerAction == PlayerAction.Down)
            {
                buttons[_selectedIndex].ButtonBackground = GameUIConstants.PopupBtnBackground;

                if (message.PlayerAction == PlayerAction.Up)
                {
                    _selectedIndex--;
                    if (_selectedIndex < 0)
                        _selectedIndex = 2;
                }
                else
                {
                    _selectedIndex = (_selectedIndex + 1) % 3;
                }

                buttons[_selectedIndex].ButtonBackground = GameUIConstants.PopupSelectedBtnBackground;
            }
            else if (message.PlayerAction == PlayerAction.Enter)
            {
                _eventAggregator.Publish(new PausePopupEvent() 
                {
                    Resume = _selectedIndex == 0,
                    PlayAgain = _selectedIndex == 1,
                    Exit = _selectedIndex == 2 
                });
            }
            else if (message.PlayerAction == PlayerAction.Back)
            {
                _eventAggregator.Publish(new PausePopupEvent() { Resume = true });
            }
        }
    }
}
