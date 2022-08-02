using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace DodgeGame
{
    public class Board
    {
        public Player player { get; set; }
        public List<Enemy> Enemies { get; set; }
        public Grid MasterGrid { get; set; }
        public Canvas cnvs { get; set; }
        public TextBlock numOfEnemyComBox { get; set; }

        public Board(Grid MasterGrid)
        {
            this.MasterGrid = MasterGrid;
            cnvs = (Canvas)MasterGrid.FindName("cnvs");
            Enemies = new List<Enemy>();
            player = new Player(MasterGrid);
            CreatePiece(player);
        }

        public void CreatEnemy(int numberOfEnemies)
        {
            for (int i = 0; i < numberOfEnemies; i++)
            {
                Enemy enemy1 = new Enemy();
                CreatePiece(enemy1);
                Enemies.Add(enemy1);
            }
        }

        private void CreatePiece(GamePiece piece)
        {
            SetLoc(piece);
            if (SameLocation(piece, out Enemy a))
            {
                CreatePiece(piece);
            }
            else
            {
                cnvs.Children.Add(piece.Shape);
            }
        }

        private void SetLoc(GamePiece piece)
        {
            Random random = new Random();

            piece.Left = random.Next((int)(cnvs.ActualWidth - piece.Shape.Width));
            piece.Top = random.Next((int)(cnvs.ActualHeight - piece.Shape.Height));
        }

        public bool SameLocation(GamePiece piece, out Enemy enemy1)
        {
            enemy1 = null;
            foreach (Enemy enemy in Enemies)
            {
                if (piece == enemy)
                {
                    continue;
                }
                if (Math.Abs(piece.Left - enemy.Left) < piece.Shape.Width * 0.7 &&
                    Math.Abs(piece.Top - enemy.Top) < piece.Shape.Height * 0.7)
                {
                    enemy1 = enemy;
                    return true;
                }
            }
            return false;
        }

        public void PieceOutOfBounds(GamePiece piece) //chack if the player out of bounds.
        {
            if (piece.Left < 0)
                piece.Left = 0;
            if (piece.Left > cnvs.ActualWidth - piece.Shape.Width)
                piece.Left = cnvs.ActualWidth - piece.Shape.Width;
            if (piece.Top < 0)
                piece.Top = 0;
            if (piece.Top > cnvs.ActualHeight - piece.Shape.Height)
                piece.Top = cnvs.ActualHeight - piece.Shape.Height;
        }
    }
}
