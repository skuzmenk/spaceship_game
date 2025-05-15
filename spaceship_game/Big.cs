using System;
using System.IO;
using System.Media;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static spaceship_game.MainWindow;


namespace spaceship_game
{
    public class BigAsteroid : Enemy, IMovable
    {
        public BigAsteroid()
        {
            Health = 2;
            Score = 2;
            Image = new Image { Source = MainWindow.LoadImage("Resources/strong.png") };
        }

        public void Move()
        {
            Size++;
            Image.Width = Image.Height = Size;
        }
        public override Enemy Clone() => new BigAsteroid();
    }
}
