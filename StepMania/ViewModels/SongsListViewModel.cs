using Caliburn.Micro;
using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;

namespace StepMania.ViewModels
{
    public class SongsListViewModel : BaseViewModel, IHandle<KeyPressedEvent>
    {
        public SongsListViewModel(IEventAggregator eventAggregator)
            : base(eventAggregator)
        {

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
    }
}
