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
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool isGameOver = false;
        private int lives = 3;
        private int count = 1;
        private int health = 0;
        private int score = 0;
        private int speed = 60;
        private int highScore = 0;
        private string highScorePath = "highscore.json";

        private Random random = new();
        private SoundPlayer shoot = LoadSound("Resources/zvuk-vyistrela-iz-lazera-23704.wav");
        private SoundPlayer player = LoadSound("Resources/tyajelyiy-raskatistyiy-zvuk-vzryiva.wav");
        private SoundPlayer gameOver = LoadSound("Resources/uroven-ne-proyden.wav");
        private SoundPlayer bossRound = LoadSound("Resources/jutkii-smeh-odinochnyii-mujskoi-korotkii.wav");

        private Image ship;
        private Image heart1;
        private Image heart2;
        private Image heart3;
        private Canvas gameCanvas;
        private Label scoreLabel;
        public MainWindow()
        {
            InitializeComponent();
            this.WindowState = WindowState.Maximized;  
            this.WindowStyle = WindowStyle.None;
            this.KeyDown += Window_KeyDown;
            this.Topmost = true;  
            gameCanvas = new Canvas();
            this.Content = gameCanvas;
            ShowStartScreen();     
        }
        private async void Window_KeyDown(object sender, KeyEventArgs e)
        {

        }
        private void ShowStartScreen()
        {
            gameCanvas.Children.Clear();

            Image background = new()
            {
                Source = LoadImage("Resources/CosmoBattle.jpg"),

                Stretch = Stretch.Fill
            };
            this.SizeChanged += (s, e) =>
            {
                background.Width = this.Width;
                background.Height = this.Height;
            };
            gameCanvas.Children.Add(background);

            Label startLabel = new()
            {
                Content = "Press Space to start",
                Foreground = Brushes.Yellow,
                FontSize = 20,
                FontWeight = FontWeights.Bold
            };
            Canvas.SetLeft(startLabel, 650);
            Canvas.SetTop(startLabel, 750);
            gameCanvas.Children.Add(startLabel);

        }
        private static SoundPlayer LoadSound(string path)
        {
            return new SoundPlayer(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path));
        }

        public static BitmapImage LoadImage(string path)
        {
            return new BitmapImage(new Uri(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path)));
        }
    }
}