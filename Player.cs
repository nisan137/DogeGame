using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Shapes;

namespace DodgeGame
{
    public class Player : GamePiece
    {
        TextBlock livesTbl;
        private int _lives;

        public int Lives
        {
            get { return _lives; }
            set 
            {
                _lives = value;
                livesTbl.Text = "Lives : " + _lives;
            }
        }

        public Player(Grid MasterGrid)
        {
            livesTbl = (TextBlock)MasterGrid.FindName("livesTbl");
            Shape.Source = new BitmapImage(new Uri(@"ms-appx:///Assets/Simba.png"));
            Speed = 5;
            Lives = 3;
        }
    }
}
