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
    public class UFO : Enemy, IMovable
    {
        private double angle = 0;

        public UFO()
        {
            Health = 3;
            Score = 100;
            Image = new Image { Source = MainWindow.LoadImage("Resources/UFO.png") };
        }

        public void Move()
        {
            Size++;
            Image.Width = Image.Height = Size;

            angle += 0.1;

            Y += 1.5;
            X += Math.Sin(angle) * 3;

            Canvas.SetLeft(Image, X);
            Canvas.SetTop(Image, Y);
        }
        public override Enemy Clone() => new UFO();
    }
}
