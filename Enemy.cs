using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace DodgeGame
{
    public class Enemy : GamePiece
    {
        public Enemy()
        {
            Shape.Source = new BitmapImage(new Uri(@"ms-appx:///Assets/Scar.png"));
            Speed = 1;
        }
    }
}

