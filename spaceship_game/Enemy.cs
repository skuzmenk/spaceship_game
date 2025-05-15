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


namespace spaceship_game
{
    public abstract class Enemy
    {
        public Image Image { get; protected set; }
        public int Health { get; protected set; }
        public int Score { get; protected set; }
        public double X { get; protected set; }
        public double Y { get; protected set; }
        public int Size { get; set; }
        public MainWindow ParentWindow { get; set; }

        public bool IsDestroyed => Health <= 0;

        public abstract Enemy Clone();

        public virtual void Initialize(double x, double y)
        {
            X = x;
            Y = y;
            Size = 60;
            Image.MouseDown += Enemy_Click;
            Canvas.SetLeft(Image, X);
            Canvas.SetTop(Image, Y);
        }

        private void Enemy_Click(object sender, MouseButtonEventArgs e)
        {

            if (Health > 0)
            {
                Health--;
                ParentWindow?.OnEnemyHit();
            }
        }


    }
}
