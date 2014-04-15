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

namespace StepMania.Controls
{
    /// <summary>
    /// Interaction logic for LifeBar.xaml
    /// </summary>
    public partial class LifeBar : UserControl
    {
        public LifeBar()
        {
            InitializeComponent();
        }



        public void SetLife(int percent)
        {
            bState.Width = ActualWidth * percent / 100;
        }
    }
}
