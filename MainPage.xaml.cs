using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;


// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace DodgeGame
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public partial class MainPage : Page
    {

        private GameLogic gameLogic { get; set; }
        int numOfEnemy;

        public MainPage()
        {
            this.InitializeComponent();
        }

        private void Exit_Clicked(object sender, RoutedEventArgs e)
        {
            Application.Current.Exit();
        }

        private void Start_Clicked(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(numOfEnemyTbx.Text, out numOfEnemy) && numOfEnemy > 1 && numOfEnemy < 21)
            {
                cnvs.Children.Clear();
                gameLogic = new GameLogic(MasterGrid);
                gameLogic.brd.CreatEnemy(numOfEnemy);
                startGrid.Visibility = Visibility.Collapsed;
                ComboBoxItem DifficultyCbi = (ComboBoxItem)DifficultyComBox.SelectedItem; // allow to hold the spesific item
                gameLogic.DifficultySpeed = int.Parse(DifficultyCbi.Name.Substring(3, 1)) * 0.1;
            }
        }

        private void GoBack_Clicked(object sender, RoutedEventArgs e)
        {
            startGrid.Visibility = Visibility.Visible;
        }

        private void NewGame_Clicked(object sender, RoutedEventArgs e)
        {
            cnvs.Children.Clear();
            gameLogic = new GameLogic(MasterGrid);
            gameLogic.brd.CreatEnemy(numOfEnemy);
        }

        private void StartPause_Clicked(object sender, RoutedEventArgs e)
        {
            gameLogic.PausesStart();
        }

        private void Save_Clicked(object sender, RoutedEventArgs e)
        {
            gameLogic.SaveFile();
            gameLogic.timer.Stop();
        }

        private void Load_Tapped(object sender, TappedRoutedEventArgs e)
        {
            startGrid.Visibility = Visibility.Collapsed;
            gameLogic.LoadGame();
        }
    }
}
