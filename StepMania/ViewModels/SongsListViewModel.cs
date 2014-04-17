using Caliburn.Micro;
using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using GameLayer;

namespace StepMania.ViewModels
{
    public class SongsListViewModel : BaseViewModel, IHandle<KeyPressedEvent>
    {
        public SongsListViewModel(IEventAggregator eventAggregator)
            : base(eventAggregator)
        {
            Songs = new BindableCollection<ISong>();

            for (int i = 0; i < 20; i++)
            {
                var song = new Song() { Title = "Sdsadsadsadsas sda sadsa a", Artist = "Shakira", BackgroundPath = "../Images/game_background.jpg" };
                Songs.Add(song);
            }
        }

        public void Handle(KeyPressedEvent message)
        {
            if (!IsActive)
                return;

            if (message.Key == Key.Escape)
            {
                (Application.Current.MainWindow.DataContext as MainWindowViewModel).ActivateItem(IoC.Get<MenuViewModel>());
            }
            else if (message.Key == Key.Return)
            {
                (Application.Current.MainWindow.DataContext as MainWindowViewModel).ActivateItem(IoC.Get<GameViewModel>());
            }
        }

        #region Songs

        private BindableCollection<ISong> _songs;

        public BindableCollection<ISong> Songs
        {
            get
            {
                return _songs;
            }
            set
            {
                if (_songs != value)
                {
                    _songs = value;
                    NotifyOfPropertyChange(() => Songs);
                }
            }
        }
        #endregion
    }
}
