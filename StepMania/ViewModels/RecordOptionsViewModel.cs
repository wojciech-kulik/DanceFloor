using Caliburn.Micro;
using Common;
using GameLayer;
using Microsoft.Win32;
using StepMania.Constants;
using StepMania.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace StepMania.ViewModels
{
    public class RecordOptionsViewModel : BaseViewModel, IHandle<GameKeyEvent>, IHandle<ButtonsPopupEvent>
    {
        #region Song

        private ISong _song;

        public ISong Song
        {
            get
            {
                return _song;
            }
            set
            {
                if (_song != value)
                {
                    _song = value;
                    NotifyOfPropertyChange(() => Song);
                }
            }
        }
        #endregion

        #region Difficulty

        private Difficulty _difficulty;

        public Difficulty Difficulty
        {
            get
            {
                return _difficulty;
            }
            set
            {
                if (_difficulty != value)
                {
                    _difficulty = value;
                    NotifyOfPropertyChange(() => Difficulty);
                }
            }
        }
        #endregion

        ISongsService _songsService;
        RecordOptionsView _view;

        bool _difficultySelection = false;
        int _selectedIndex = 0;
        List<UIElement> _selectableControls = new List<UIElement>();        

        public RecordOptionsViewModel(IEventAggregator eventAggregator, ISongsService songsService)
            : base(eventAggregator)
        {
            Song = new Song();
            Difficulty = Difficulty.Easy;

            _songsService = songsService;
        }

        protected override void OnDeactivate(bool close)
        {
            Song = null; //to avoid memory leak
        }

        protected override void OnViewAttached(object view, object context)
        {
            _view = view as RecordOptionsView;

            _view.btnSong.ButtonBackground = GameUIConstants.PopupSelectedBtnBackground;
            _view.btnBackground.ButtonBackground = GameUIConstants.PopupBtnBackground;
            _view.btnCover.ButtonBackground = GameUIConstants.PopupBtnBackground;

            _view.btnEasy.ButtonBackground = Song.Sequences.ContainsKey(Difficulty.Easy) ? GameUIConstants.RecordCreatedBtnGradient : GameUIConstants.GameModeInactiveBtnGradient;
            _view.btnMedium.ButtonBackground = Song.Sequences.ContainsKey(Difficulty.Medium) ? GameUIConstants.RecordCreatedBtnGradient : GameUIConstants.GameModeInactiveBtnGradient;
            _view.btnHard.ButtonBackground = Song.Sequences.ContainsKey(Difficulty.Hard) ? GameUIConstants.RecordCreatedBtnGradient : GameUIConstants.GameModeInactiveBtnGradient;

            _selectableControls.Add(_view.btnSong);
            _selectableControls.Add(_view.btnBackground);
            _selectableControls.Add(_view.btnCover);
            _selectableControls.Add(_view.tbTitle);
            _selectableControls.Add(_view.tbArtist);
            _selectableControls.Add(_view.tbAuthor);
        }

        void SetFocusOn(int index)
        {
            if (_selectableControls[index] is Controls.MenuButton)
            {
                (_selectableControls[index] as Controls.MenuButton).ButtonBackground = GameUIConstants.PopupSelectedBtnBackground;
            }
            else
            {
                _selectableControls[_selectedIndex].Focusable = true;
                _selectableControls[index].Focus();
            }
        }

        void SetFocusOn(Difficulty difficulty)
        {
            switch (difficulty)
            {
                case Common.Difficulty.Easy:
                    _view.btnEasy.ButtonBackground = GameUIConstants.GameModeSelectedBtnGradient;
                    break;
                case Common.Difficulty.Medium:
                    _view.btnMedium.ButtonBackground = GameUIConstants.GameModeSelectedBtnGradient;
                    break;
                case Common.Difficulty.Hard:
                    _view.btnHard.ButtonBackground = GameUIConstants.GameModeSelectedBtnGradient;
                    break;
            }
        }

        void LoseFocusOn(int index)
        {
            if (_selectableControls[_selectedIndex] is Controls.MenuButton)
            {
                (_selectableControls[_selectedIndex] as Controls.MenuButton).ButtonBackground = GameUIConstants.PopupBtnBackground;
            }
            else
            {
                _selectableControls[_selectedIndex].Focusable = false;
            }
        }

        void LoseFocusOn(Difficulty difficulty)
        {
            switch (difficulty)
            {
                case Common.Difficulty.Easy:
                    _view.btnEasy.ButtonBackground = Song.Sequences.ContainsKey(Difficulty.Easy) ? GameUIConstants.RecordCreatedBtnGradient : GameUIConstants.GameModeInactiveBtnGradient;
                    break;
                case Common.Difficulty.Medium:
                    _view.btnMedium.ButtonBackground = Song.Sequences.ContainsKey(Difficulty.Medium) ? GameUIConstants.RecordCreatedBtnGradient : GameUIConstants.GameModeInactiveBtnGradient;
                    break;
                case Common.Difficulty.Hard:
                    _view.btnHard.ButtonBackground = Song.Sequences.ContainsKey(Difficulty.Hard) ? GameUIConstants.RecordCreatedBtnGradient : GameUIConstants.GameModeInactiveBtnGradient;
                    break;
            }   
        }

        void TryNavigateToRecordView()
        {
            if (_difficultySelection)
            {
                _eventAggregator.Publish(new NavigationExEvent()
                {
                    NavDestination = NavDestination.Record,
                    PageSettings = (vm) =>
                    {
                        (vm as RecordSequenceViewModel).Song = Song;
                        (vm as RecordSequenceViewModel).Difficulty = Difficulty;
                    }
                });
            }
        }

        void UpNavigation()
        {
            if (!_difficultySelection && _selectedIndex == 0)
            {
                _difficultySelection = true;
                LoseFocusOn(_selectedIndex);
                Difficulty = Common.Difficulty.Easy;
                SetFocusOn(Difficulty);
            }
            else if (_difficultySelection && _selectedIndex == 0)
            {
                _difficultySelection = false;
                LoseFocusOn(Difficulty);
                _selectedIndex = _selectableControls.Count - 1;
                SetFocusOn(_selectedIndex);
            }
            else if (_difficultySelection)
            {
                LoseFocusOn(Difficulty);
                _difficultySelection = false;
                SetFocusOn(_selectedIndex);
            }
            else
            {
                LoseFocusOn(_selectedIndex);
                _selectedIndex--;
                SetFocusOn(_selectedIndex);
            }
        }

        void DownNavigation()
        {
            if (!_difficultySelection && _selectedIndex == _selectableControls.Count - 1)
            {
                LoseFocusOn(_selectedIndex);
                _difficultySelection = true;
                Difficulty = Common.Difficulty.Easy;
                SetFocusOn(Difficulty);
            }
            else if (_difficultySelection && _selectedIndex == _selectableControls.Count - 1)
            {
                _difficultySelection = false;
                LoseFocusOn(Difficulty);
                _selectedIndex = 0;
                SetFocusOn(_selectedIndex);
            }
            else if (_difficultySelection)
            {
                LoseFocusOn(Difficulty);
                _difficultySelection = false;
                SetFocusOn(_selectedIndex);
            }
            else
            {                
                LoseFocusOn(_selectedIndex);
                _selectedIndex++;
                SetFocusOn(_selectedIndex);
            }
        }

        void RightNavigation()
        {
            if (_difficultySelection)
            {
                LoseFocusOn(Difficulty);
                Difficulty = (Difficulty)(((int)Difficulty + 1) % 3);
                SetFocusOn(Difficulty);
            }
        }

        void LeftNavigation()
        {
            if (_difficultySelection)
            {
                LoseFocusOn(Difficulty);
                int newVal = (int)Difficulty - 1;
                if (newVal < 0)
                    Difficulty = Difficulty.Hard;
                else
                    Difficulty = (Difficulty)newVal;
                SetFocusOn(Difficulty);
            }
        }

        void InvokeAction()
        {
            if (_difficultySelection)
            {
                TryNavigateToRecordView();
            }
            else if (_selectableControls[_selectedIndex] is Controls.MenuButton)
            {
                var ofd = new OpenFileDialog();                
                switch (_selectedIndex)
                {
                    case 0:
                        ofd.Filter = "Pliki muzyczne (*.mp3, *.ogg)|*.mp3;*.ogg";
                        break;
                    case 1:
                    case 2:
                        ofd.Filter = "Pliki graficzne (*.png, *.jpg)|*.png;*.jpg;*.jpeg";
                        break;
                }

                if (ofd.ShowDialog() == true)
                {
                    switch (_selectedIndex)
                    {
                        case 0:
                            Song.FilePath = ofd.FileName;
                            break;
                        case 1:
                            Song.BackgroundPath = ofd.FileName;
                            break;
                        case 2:
                            Song.CoverPath = ofd.FileName;
                            break;
                    }
                }
            }
        }

        public void Handle(GameKeyEvent message)
        {
            if (!IsActive || IsPopupShowing || message.PlayerId != PlayerID.Player1)
                return;

            switch (message.PlayerAction)
            {
                case PlayerAction.Enter:
                    InvokeAction();
                    break;

                case PlayerAction.Back:
                    IsPopupShowing = true;
                    _eventAggregator.Publish(new ShowPopupEvent() 
                    { 
                        PopupType = PopupType.ButtonsPopup, 
                        PopupSettings = (vm) =>
                        {
                            (vm as ButtonsPopupViewModel).Buttons.AddRange(new List<string>() { "Kontynuuj", "Zapisz utwór", "Powróć do menu" });
                            (vm as ButtonsPopupViewModel).Message = "Co chcesz zrobić?";
                        }
                    });
                    break;

                case PlayerAction.Right:
                    RightNavigation();
                    break;

                case PlayerAction.Left:
                    LeftNavigation();
                    break;

                case PlayerAction.Down:
                    DownNavigation();
                    break;

                case PlayerAction.Up:
                    UpNavigation();
                    break;
            }
        }

        public void Handle(ButtonsPopupEvent message)
        {
            if (!IsActive)
                return;

            IsPopupShowing = false;

            if (message.IsCanceled)
                return;

            if (message.SelectedButton == 2)
            {
                Song song = new Song();
                song.Artist = Song.Artist;
                song.Author = Song.Author;
                song.Title = Song.Title;
                song.Sequences = Song.Sequences;
                song.FilePath = Song.FilePath;
                song.BackgroundPath = Song.BackgroundPath;
                song.CoverPath = Song.CoverPath;
                song.CreateDate = DateTime.Now;
                song.Duration = Song.Duration;

                song.SaveToFile();
                _songsService.AddSong(song);
            }

            if (message.SelectedButton == 2 || message.SelectedButton == 3)
            {        
                _eventAggregator.Publish(new NavigationEvent() { NavDestination = NavDestination.MainMenu });
            }
        }

        public void TextBoxKeyUp(KeyEventArgs e)
        {
            if (e.Key == Key.Tab)
            {
                e.Handled = true;
                DownNavigation();
            }
        }
    }
}
