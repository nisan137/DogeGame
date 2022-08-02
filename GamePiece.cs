using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Shapes;

namespace DodgeGame
{
    public class GamePiece
    {
        public Image Shape { get; set; }
        public double Speed { get; set; }

        private double _left;
        public double Left
        {
            get { return _left; }
            set 
            {
                _left = value;
                Canvas.SetLeft(Shape, _left);
            }
        }

        private double _top;
        public double Top
        {
            get { return _top; }
            set 
            {
                _top = value;
                Canvas.SetTop(Shape, _top);
            }
        }

        public GamePiece() //constractor
        {
            Shape = new Image();
            Shape.Height = 50;
            Shape.Width = 50;
        }
    }
}


