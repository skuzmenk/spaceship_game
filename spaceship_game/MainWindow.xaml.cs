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
                isGameOver = false;
                gameCanvas.Children.Clear();
                this.Background = Brushes.Black;
                InitializeStars();
                count = 0;
                score = 0;
                SetupUI();
                await PlayGameRounds();
            }
        }
        private async Task PlayGameRounds()
        {
            while (true)
            {
                await PlayRound(new SmallAsteroid(), 10);
                if (isGameOver)
                {
                    break;
                }
                await PlayRound(new BigAsteroid(), 10);
                if (isGameOver)
                {
                    break;
                }
                bossRound.Play();
                await PlayRound(new UFO(), 1);
                if (isGameOver)
                {
                    break;
                }
                speed += 30;
            }
        }
        public void OnEnemyHit()
        {
            shoot.Play();
            speed--;
        }
        private async Task PlayRound(Enemy enemyType, int count)
        {
            for (int i = 0; i < count; i++)
            {
                Enemy enemy = enemyType.Clone();
                enemy.ParentWindow = this;
                enemy.Initialize(random.NextDouble() * (ActualWidth - 200), random.NextDouble() * (ActualHeight - 400));
                gameCanvas.Children.Add(enemy.Image);
                health = enemy.Health;

                var movable = enemy as IMovable;

                while (enemy.Size < 160)
                {
                    movable?.Move();
                    await Task.Delay(speed);

                    if (enemy.IsDestroyed)
                    {
                        gameCanvas.Children.Remove(enemy.Image);
                        player.Play();
                        score += enemy.Score;
                        scoreLabel.Content = $"Score: {score}";
                        break;
                    }
                }

                if (enemy.Size >= 160)
                {
                    gameCanvas.Children.Remove(enemy.Image);
                    LoseLife();
                }
                if (isGameOver)
                {
                    GameOver();
                    break;
                }
            }
        }
        private void LoseLife()
        {
            lives--;
            if (lives == 0)
            {
                isGameOver = true;
                this.Background = Brushes.Red;
                GameOver();
                return;
            }
            switch (lives)
            {

                case 2:
                    gameCanvas.Children.Remove(heart3);
                    break;
                case 1:
                    gameCanvas.Children.Remove(heart2);
                    break;
                case 0:
                    gameCanvas.Children.Remove(heart1);
                    this.Background = Brushes.Red;
                    break;
            }
        }
        private TextBlock CreateText(string text, int size, double left, double top)
        {
            var txt = new TextBlock
            {
                Text = text,
                FontSize = size,
                FontWeight = FontWeights.Bold,
                Foreground = Brushes.Black,
                Background = Brushes.Transparent,
                TextAlignment = TextAlignment.Center
            };
            Canvas.SetLeft(txt, left);
            Canvas.SetTop(txt, top);
            return txt;
        }
        private void GameOver()
        {
            gameOver.Play();
            gameCanvas.Children.Clear();
            gameCanvas.Children.Add(CreateText("Game over", 100, 500, 300));
            gameCanvas.Children.Add(CreateText("press Esc to exit", 20, 10, 20));
            gameCanvas.Children.Add(CreateText($"score: {score}", 20, 730, 650));
            gameCanvas.Children.Add(CreateText($"High Score: {highScore}", 20, 700, 680));
            gameCanvas.Children.Add(CreateText("Press Space to play again", 20, 650, 780));

            if (score > highScore)
            {
                highScore = score;
                SaveHighScore();
            }
            count = 1;
            lives = 3;
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
            Button exitButton = new()
            {
                Content = "Exit",
                Width = 150,
                Height = 50,
                FontSize = 18,
                Background = Brushes.Black,
                Foreground = Brushes.Yellow,
                FontWeight = FontWeights.Bold
            };
            Canvas.SetLeft(exitButton, 680);
            Canvas.SetTop(exitButton, 700);
            exitButton.Click += async (s, e) =>
            {
                Application.Current.Shutdown();
            };
            gameCanvas.Children.Add(exitButton);
          
            Button startButton = new()
            {
                Content = "Start",
                Width = 150,
                Height = 50,
                FontSize = 18,
                Background = Brushes.Black,
                Foreground = Brushes.Yellow,
                FontWeight = FontWeights.Bold
            };
            Canvas.SetLeft(startButton, 680);
            Canvas.SetTop(startButton, 650);
            startButton.Click += async (s, e) =>
            {
                isGameOver = false;
                gameCanvas.Children.Clear();
                this.Background = Brushes.Black;
                InitializeStars();
                count = 0;
                score = 0;
                SetupUI();
                await PlayGameRounds();
            };
            gameCanvas.Children.Add(startButton);
        }
        private static SoundPlayer LoadSound(string path)
        {
            return new SoundPlayer(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path));
        }

        public static BitmapImage LoadImage(string path)
        {
            return new BitmapImage(new Uri(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path)));
        }
        public interface IMovable
        {
            void Move();
        }
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
}