using StepMania.DebugHelpers;
using StepMania.ViewModels;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace StepMania.Views
{
    /// <summary>
    /// Interaction logic for GameView.xaml
    /// </summary>
    public partial class GameView : UserControl
    {
        public GameView()
        {
            InitializeComponent();

            DebugSongHelper.ConfigureGameViewForStartStopAnimation(this, GameView_KeyUp, GameView_Loaded, GameView_Unloaded);
        }

        public void GameView_KeyUp(object sender, KeyEventArgs e)
        {   
            if (e.Key == Key.Space)
                (DataContext as GameViewModel).ResumeGame();
            else if (e.Key == Key.Back)
                (DataContext as GameViewModel).PauseGame();
        }       

        void GameView_Unloaded(object sender, RoutedEventArgs e)
        {      
            ((Parent as ContentControl).Parent as Window).PreviewKeyUp -= GameView_KeyUp;       
        }

        void GameView_Loaded(object sender, RoutedEventArgs e)
        {
            ((Parent as ContentControl).Parent as Window).PreviewKeyUp += GameView_KeyUp;       
        }
    }
}
