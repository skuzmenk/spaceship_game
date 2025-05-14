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
            LoadHighScore();
        }
        private void LoadHighScore()
        {
            if (File.Exists(highScorePath))
            {
                string json = File.ReadAllText(highScorePath);
                var record = JsonSerializer.Deserialize<HighScore>(json);
                if (record != null) highScore = record.Score;
            }
        }
        public class HighScore { public int Score { get; set; } }

        private void SaveHighScore()
        {
            var record = new HighScore { Score = highScore };
            string json = JsonSerializer.Serialize(record);
            File.WriteAllText(highScorePath, json);
        }
        private async void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Application.Current.Shutdown();
            }

            if (e.Key == Key.Space && count == 1)
            {
                gameCanvas.Children.Clear();
                this.Background = Brushes.Black;
                InitializeStars();
                count = 0;
                score = 0;
                SetupUI();
            }
        }
        private void SetupUI()
        {
            this.Content = gameCanvas;
            gameCanvas.Children.Clear();
            InitializeStars();
            scoreLabel = new Label
            {
                Foreground = Brushes.Yellow,
                FontSize = 24,
                Content = "Score: 0"
            };
            heart1 = new Image
            {
                Source = LoadImage("Resources/heart.png"),
                Width = 25,
                Height = 25
            };
            heart2 = new Image
            {
                Source = LoadImage("Resources/heart.png"),
                Width = 25,
                Height = 25
            };
            heart3 = new Image
            {
                Source = LoadImage("Resources/heart.png"),
                Width = 25,
                Height = 25
            };

            ship = new Image
            {
                Source = LoadImage("Resources/Ship.png"),
                Width = ActualWidth,
                Height = 250
            };

            Canvas.SetLeft(scoreLabel, ActualWidth - 150);
            Canvas.SetTop(scoreLabel, 20);

            Canvas.SetLeft(ship, 0);
            Canvas.SetTop(ship, ActualHeight - ship.Height);
            Canvas.SetLeft(heart1, 10);
            Canvas.SetTop(heart1, 10);
            Canvas.SetLeft(heart2, 30);
            Canvas.SetTop(heart2, 10);
            Canvas.SetLeft(heart3, 50);
            Canvas.SetTop(heart3, 10);
            gameCanvas.Children.Add(scoreLabel);
            gameCanvas.Children.Add(ship);
            gameCanvas.Children.Add(heart1);
            gameCanvas.Children.Add(heart2);
            gameCanvas.Children.Add(heart3);
        }
        private void InitializeStars()
        {
            gameCanvas.Children.Clear();
            for (int i = 0; i < 1000; i++)
            {
                Rectangle star = new()
                {
                    Width = 2,
                    Height = 2,
                    Fill = Brushes.White
                };
                Canvas.SetLeft(star, random.NextDouble() * ActualWidth);
                Canvas.SetTop(star, random.NextDouble() * ActualHeight);
                gameCanvas.Children.Add(star);
            }
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