using Caliburn.Micro;
using Common;
using System.Windows.Input;
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
                _eventAggregator.Publish(new NavigationEvent() { NavDestination = NavDestination.MainMenu });
            }
            else if (message.Key == Key.Return)
            {
                _eventAggregator.Publish(new NavigationEvent() { NavDestination = NavDestination.Game });
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
