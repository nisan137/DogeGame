using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace DodgeGame
{
    public class GameLogic
    {
        public Board brd { get; set; }
        public DispatcherTimer timer { get; set; }

        bool isUp, isDown, isLeft, isRight;

        public bool isPaused;

        public double DifficultySpeed { get; set; }

        public GameLogic(Grid MasterGrid) //constactor
        {
            brd = new Board(MasterGrid);

            isUp = isRight = isLeft = isDown = isPaused = false;
            Window.Current.CoreWindow.KeyDown += CoreWindow_KeyDown;
            Window.Current.CoreWindow.KeyUp += CoreWindow_KeyUp;

            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 0, 0, 10);
            timer.Tick += Timer_Tick;
            timer.Start();
        }
        private void Timer_Tick(object sender, object e)
        {
            PlayerMove();
            EnemyMove();
            CheckCollision();
        }

        private void CoreWindow_KeyUp(Windows.UI.Core.CoreWindow sender, Windows.UI.Core.KeyEventArgs args)
        {
            switch (args.VirtualKey)
            {
                case Windows.System.VirtualKey.Up:
                    isUp = false;
                    break;
                case Windows.System.VirtualKey.Left:
                    isLeft = false;
                    break;
                case Windows.System.VirtualKey.Right:
                    isRight = false;
                    break;
                case Windows.System.VirtualKey.Down:
                    isDown = false;
                    break;
            }
        }

        private void CoreWindow_KeyDown(Windows.UI.Core.CoreWindow sender, Windows.UI.Core.KeyEventArgs args)
        {
            switch (args.VirtualKey)
            {
                case Windows.System.VirtualKey.Up:
                    isUp = true;
                    break;
                case Windows.System.VirtualKey.Left:
                    isLeft = true;
                    break;
                case Windows.System.VirtualKey.Right:
                    isRight = true;
                    break;
                case Windows.System.VirtualKey.Down:
                    isDown = true;
                    break;
            }
        }

        private void PlayerMove()
        {
            if (isUp)
            {
                brd.player.Top -= brd.player.Speed;
            }
            if (isDown)
            {
                brd.player.Top += brd.player.Speed;
            }
            if (isLeft)
            {
                brd.player.Left -= brd.player.Speed;
            }
            if (isRight)
            {
                brd.player.Left += brd.player.Speed;
            }

            brd.PieceOutOfBounds(brd.player);
        }

        private void EnemyMove()
        {
            foreach (Enemy enemy in brd.Enemies)
            {
                double goLeft = brd.player.Left - enemy.Left;
                double goTop = brd.player.Top - enemy.Top;

                if (goLeft > 0)
                {
                    enemy.Left += enemy.Speed;
                }
                else
                {
                    enemy.Left -= enemy.Speed;
                }

                if (goTop > 0)
                {
                    enemy.Top += enemy.Speed;
                }
                else
                {
                    enemy.Top -= enemy.Speed;
                }

            }
        }

        private void CheckCollision()
        {
            if (brd.SameLocation(brd.player, out Enemy enemy1))
            {
                if (enemy1 != null)
                {
                    brd.cnvs.Children.Remove(enemy1.Shape);
                    brd.Enemies.Remove(enemy1);
                    brd.player.Lives--;
                    if (brd.player.Lives == 0)
                    {
                        PrintMessege("Game Over", "You Lost 💩");
                    }
                    if (brd.Enemies.Count <= 1)
                    {
                        PrintMessege("Game Over", "You Won 🥇");
                    }
                }
            }
            else
            {
                for (int i = 0; i < brd.Enemies.Count; i++)
                {
                    if (brd.SameLocation(brd.Enemies[i], out Enemy a)) // chack collision, if was removw one enemy.
                    {
                        brd.cnvs.Children.Remove(brd.Enemies[i].Shape);  // remove the enemy from the canvas.
                        brd.Enemies.Remove(brd.Enemies[i]); // remove the enemy from the array.
                        brd.Enemies.ForEach(enemy => enemy.Speed += 0.3); //to the other enemies add the speed.
                    }
                    if (brd.Enemies.Count <= 1) //if the number of enemies is 1 or less game over.
                    {
                        PrintMessege("Game Over", "You Won 🥇");
                    }
                }
            }
        }

        async void PrintMessege(string head, string text)
        {
            timer.Stop();
            MessageDialog message = new MessageDialog(text, head);
            await message.ShowAsync();
        }

        //function- pause and cotinue the game
        public void PausesStart() 
        {
            if (isPaused)
            {
                timer.Start();
                isPaused = false;
            }
            else
            {
                timer.Stop();
                isPaused = true;
            }
        }

        //SaveFile method is determins what happens when save game button is tapped and what data is going to be saved 
        public async void SaveFile()
        {
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            StorageFile savedFile = await storageFolder.CreateFileAsync("savedGame.txt", CreationCollisionOption.ReplaceExisting);

            //for every enemy we created, save his position and speed.
            foreach (Enemy enemy in brd.Enemies)
            {
                await FileIO.AppendTextAsync(savedFile, $"{enemy.Left}|{enemy.Top}|{enemy.Speed}{Environment.NewLine}");
            }
            await FileIO.AppendTextAsync(savedFile, $"{brd.player.Left}|{brd.player.Top}|{brd.player.Speed}|{brd.player.Lives}");
            PrintMessege("Game has been Saved!", "Continue");
        }

        //LoadGame method is responsible for the way that the game is loading the saved files 
        public async void LoadGame()
        {
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            StorageFile savedFile = await storageFolder.GetFileAsync("savedGame.txt");
            string FullText = await FileIO.ReadTextAsync(savedFile);
            ReadFile(FullText);
            timer.Start();
        }

        //ReadFile is responsible for the way the game can read the saving files 
        private void ReadFile(string FullText)
        {
            string[] lines = FullText.Split("\n");
            string[] line;

            brd.cnvs.Children.Clear();
            brd.Enemies.Clear();

            for (int i = 0; i < lines.Length - 1; i++)
            {
                line = lines[i].Split("|");
                Enemy enemy = new Enemy();
                ReCreate(enemy, line);
                brd.Enemies.Add(enemy);
            }

            line = lines[lines.Length - 1].Split("|");
            brd.player.Lives = int.Parse(line[3]);
            ReCreate(brd.player, line);
        }

        //ReCreatePiece is a method thats recreating the game pieces in according to the data that is written in the saving files 
        private void ReCreate(GamePiece gamepiece, string[] line)
        {
            gamepiece.Left = double.Parse(line[0]);
            gamepiece.Top = double.Parse(line[1]);
            gamepiece.Speed = double.Parse(line[2]);

            brd.cnvs.Children.Add(gamepiece.Shape);
        }
    }
}
