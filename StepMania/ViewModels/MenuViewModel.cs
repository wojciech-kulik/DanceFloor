using Caliburn.Micro;
using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace StepMania.ViewModels
{
    public class MenuViewModel : BaseViewModel, IHandle<KeyPressedEvent>
    {
        public MenuViewModel(IEventAggregator eventAggregator)
            : base(eventAggregator)
        {

        }

        public void Handle(KeyPressedEvent message)
        {
            if (!IsActive)
                return;

            if (message.Key == Key.Escape)
                CloseGame();
        }

        public void CloseGame()
        {
            Application.Current.MainWindow.Close();
        }
    }
}
