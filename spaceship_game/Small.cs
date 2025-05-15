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
    public class SmallAsteroid : Enemy, IMovable
    {
        public SmallAsteroid()
        {
            Health = 1;
            Score = 1;
            Image = new Image { Source = MainWindow.LoadImage("Resources/meteor.png") };
        }

        public void Move()
        {
            Size++;
            Image.Width = Image.Height = Size;
        }
        public override Enemy Clone() => new SmallAsteroid();
    }
}
