using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace StepMania
{
    /// <summary>
    /// Interaction logic for MainWindowView.xaml
    /// </summary>
    public partial class MainWindowView : NavigationWindow
    {
        public MainWindowView()
        {
            InitializeComponent();
            Loaded += MainWindowView_Loaded;
        }

        void MainWindowView_Loaded(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("Views/MainView.xaml", UriKind.Relative));
        }
    }
}
