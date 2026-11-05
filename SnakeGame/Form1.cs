using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic;


namespace SnakeGame
{
    public partial class Form1 : Form
    {

        private Circle bonusFood = new Circle();
        private bool isBonusFoodActive = false;
        private int eatenFoodCount = 0;

        private List<Circle> Snake = new List<Circle>();
        private Circle food = new Circle();

        int maxWidth;
        int maxHeight;

        int score;
        int highScore;

        Random random = new Random();
        bool goLeft, goRight, goUp, goDown;

        public Form1()
        {
            InitializeComponent();
            new Settings();
            this.KeyPreview = true;
            dgvLeadership.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

        }

        private void btnStart_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadHighScores();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void KeyIsDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left && Settings.directions != "right")
            {
                goLeft = true;
            }
            if (e.KeyCode == Keys.Right && Settings.directions != "left")
            {
                goRight = true;
            }
            if (e.KeyCode == Keys.Up && Settings.directions != "down")
            {
                goUp = true;
            }
            if (e.KeyCode == Keys.Down && Settings.directions != "up")
            {
                goDown = true;
            }
        }

        private void KeyIsUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                goLeft = false;
            }
            if (e.KeyCode == Keys.Right)
            {
                goRight = false;
            }
            if (e.KeyCode == Keys.Up)
            {
                goUp = false;
            }
            if (e.KeyCode == Keys.Down)
            {
                goDown = false;
            }
        }

        private void StartGame(object sender, EventArgs e)
        {
            RestartGame();
        }

        private void TakeSnapShot(object sender, EventArgs e)
        {
            Label caption = new Label();
            caption.Text = "I scored: " + score + " and my HighScore is: " + highScore + " on the SnakeGame";
            caption.Font = new Font("Arial", 12, FontStyle.Bold);
            caption.ForeColor = Color.LightBlue;
            caption.AutoSize = false;
            caption.Width = picCanvas.Width;
            caption.Height = 30;
            caption.TextAlign = ContentAlignment.MiddleCenter;
            picCanvas.Controls.Add(caption);
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.FileName = "SnakeGame_Score.png";
            saveFileDialog.DefaultExt = ".png";
            saveFileDialog.Filter = "PNG Image|*.png|JPEG Image|*.jpg|Bitmap Image|*.bmp";
            saveFileDialog.ValidateNames = true;

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                int weight = Convert.ToInt32(picCanvas.Width);
                int height = Convert.ToInt32(picCanvas.Height);
                Bitmap bmp = new Bitmap(Width, Height);
                picCanvas.DrawToBitmap(bmp, new Rectangle(0, 0, Width, height));
                bmp.Save(saveFileDialog.FileName, ImageFormat.Jpeg);
                picCanvas.Controls.Remove(caption);
            }
        }

        private void GameTimerEvent(object sender, EventArgs e)
        {
            // setting the direction of the snake based on the key pressed
            if (goLeft)
            {
                Settings.directions = "left";
            }

            if (goRight)
            {
                Settings.directions = "right";
            }
            if (goUp)
            {
                Settings.directions = "up";
            }
            if (goDown)
            {
                Settings.directions = "down";
            }
            // Move the snake in the specified direction

            for (int i = Snake.Count - 1; i >= 0; i--)
            {
                if (i == 0) // Move the head of the snake
                {
                    switch (Settings.directions)
                    {
                        case "left":
                            Snake[i].x--; // *Decrease the x-coordinate to move left because the origin (0,0) is at the top-left corner of the canvas. MaxLeft is 0 , and MaxRight is the width of the canvas. So, to move left, we need to decrease the x-coordinate.
                            break;
                        case "right":
                            Snake[i].x++; // ? Increase the x-coordinate to move right because the origin (0,0) is at the top-left corner of the canvas. MaxLeft is 0 , and MaxRight is the width of the canvas. So, to move right, we need to increase the x-coordinate.
                            break;
                        case "up":
                            Snake[i].y--;
                            break;
                        case "down":
                            Snake[i].y++;
                            break;
                    }
                    if (Snake[i].x < 0)
                    {
                        Snake[i].x = maxWidth; // If the snake goes out of the left boundary, it appears on the right boundary.
                    }
                    if (Snake[i].x > maxWidth)
                    {
                        Snake[i].x = 0; // If the snake goes out of the right boundary, it appears on the left boundary.
                    }
                    if (Snake[i].y < 0)
                    {
                        Snake[i].y = maxHeight; // If the snake goes out of the top boundary, it appears on the bottom boundary.
                    }
                    if (Snake[i].y > maxHeight)
                    {
                        Snake[i].y = 0; // If the snake goes out of the bottom boundary, it appears on the top boundary.
                    }



                    //if (Snake[i].x == food.x && Snake[i].y == food.y)
                    //{
                    //    EatFood();
                    //}
                    // Normal yem kontrolü
                    if (!isBonusFoodActive && Snake[i].x == food.x && Snake[i].y == food.y)
                    {
                        EatFood();
                    }
                    // Özel yem kontrolü
                    else if (isBonusFoodActive && Snake[i].x == bonusFood.x && Snake[i].y == bonusFood.y)
                    {
                        EatFood();
                    }
                    for (int j = 1; j < Snake.Count; j++)
                    {
                        if (Snake[i].x == Snake[j].x && Snake[i].y == Snake[j].y)
                        {
                            GameOver();
                        }
                    }



                }
                else
                {
                    Snake[i].x = Snake[i - 1].x; // Move the body of the snake to the position of the previous segment
                    Snake[i].y = Snake[i - 1].y; // Move the body of the snake to the position of the previous segment



                }


            }

            picCanvas.Invalidate(); // Refresh the canvas to trigger the Paint event and redraw the snake and food

        }

        private void UpdatePictureBoxGraphics(object sender, PaintEventArgs e)
        {
            Graphics canvas = e.Graphics;
            Brush snakeColor;

            for (int i = 0; i < Snake.Count; i++)
            {
                if (i == 0)
                {
                    snakeColor = Brushes.Black; 
                }
                else
                {
                    snakeColor = Brushes.DarkGreen; 
                }
                canvas.FillEllipse(snakeColor, new Rectangle(Snake[i].x * Settings.Width, Snake[i].y * Settings.Height, Settings.Width, Settings.Height));
            }

            if (isBonusFoodActive)
            {
              
                canvas.FillEllipse(Brushes.Gold, new Rectangle(bonusFood.x * Settings.Width, bonusFood.y * Settings.Height, Settings.Width, Settings.Height));
            }
            else
            {

                canvas.FillEllipse(Brushes.DarkRed, new Rectangle(food.x * Settings.Width, food.y * Settings.Height, Settings.Width, Settings.Height));
            }
        }
        private void RestartGame()
        {
            maxWidth = picCanvas.Width / Settings.Width - 1;
            maxHeight = picCanvas.Height / Settings.Height - 1;

            Snake.Clear();

            btnStart.Enabled = false;
            btnSnap.Enabled = false;
            score = 0;
            txtScore.Text = "Score: " + score;
            eatenFoodCount = 0;
            isBonusFoodActive = false;

            Circle head = new Circle { x = 10, y = 5 };
            Snake.Add(head); // Index 0 is the head of the snake

            for (int i = 0; i < 10; i++)
            {
                Circle body = new Circle();
                Snake.Add(body);
            }

            food = new Circle { x = random.Next(2, maxWidth), y = random.Next(2, maxHeight) }; // 2 is the minimum so it does not appear so close to the snake. And the maximum is the maximum width and height of the canvas.
            gameTimer.Start();
            this.Focus();
            picCanvas.Focus();
        }

        private void txtHighScore_Click(object sender, EventArgs e)
        {

        }

        private void dgvLeadership_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void EatFood()
        {
            if (isBonusFoodActive)
            {
                score += 2; 
                isBonusFoodActive = false; 

                
                food = new Circle { x = random.Next(2, maxWidth), y = random.Next(2, maxHeight) };
            }
            else
            {
                score++; 
                eatenFoodCount++; 

                
                if (eatenFoodCount % 10 == 0)
                {
                    isBonusFoodActive = true;
                    bonusFood = new Circle { x = random.Next(2, maxWidth), y = random.Next(2, maxHeight) };
                }
                else
                {
                    food = new Circle { x = random.Next(2, maxWidth), y = random.Next(2, maxHeight) };
                }
            }

            txtScore.Text = "Score: " + score;

            
            Circle body = new Circle
            {
                x = Snake[Snake.Count - 1].x,
                y = Snake[Snake.Count - 1].y
            };
            Snake.Add(body);
        }
        private void GameOver()
        {
            gameTimer.Stop();
            btnStart.Enabled = true;
            btnSnap.Enabled = true;


            string playerName = ShowInputBox("Skorunuz: " + score + "\nLütfen adınızı giriniz:", "Oyun Bitti");

            if (!string.IsNullOrWhiteSpace(playerName))
            {
                SaveScoreToDatabase(playerName, score); // ADO.NET ile kaydet
                LoadHighScores(); // DataGridView'i güncelle
            }

            if (score > highScore)
            {
                highScore = score;
                txtHighScore.Text = "High Score: " + Environment.NewLine + highScore;
                txtHighScore.ForeColor = Color.Maroon;
                txtHighScore.TextAlign = ContentAlignment.MiddleCenter;
            }
        }

        private void picCanvas_Click(object sender, EventArgs e)
        {

        }

        private void SaveScoreToDatabase(string playerName, int scoreValue)
        {
            // Kendi SQL Server sunucu adını yaz (Örn: DESKTOP-QJR5I0K\SQLEXPRESS veya .)
            string connectionString = @"Server=DESKTOP-QJR5I0K\SQLEXPRESS;Database=DbSnakeGame;Integrated Security=True;TrustServerCertificate=True;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO Tbl_Scores (PlayerName, Score, Date) VALUES (@p1, @p2, @p3)";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Parametreli sorgu kullanarak SQL Injection önlenir
                    command.Parameters.AddWithValue("@p1", playerName);
                    command.Parameters.AddWithValue("@p2", scoreValue);
                    command.Parameters.AddWithValue("@p3", DateTime.Now);

                    connection.Open();
                    command.ExecuteNonQuery(); // Veri ekleme işlemini çalıştırır
                }
            }
        }
        private void LoadHighScores()
        {
            string connectionString = @"Server=DESKTOP-QJR5I0K\SQLEXPRESS;Database=DbSnakeGame;Integrated Security=True;TrustServerCertificate=True;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Oyuncu Adı ve Skor sütunlarını çekiyoruz
                string query = "SELECT TOP 5 PlayerName AS 'Oyuncu Adı', Score AS 'Skor' FROM Tbl_Scores ORDER BY Score DESC";
                SqlDataAdapter da = new SqlDataAdapter(query, connection);
                DataTable dt = new DataTable();
                da.Fill(dt);

                dgvLeadership.DataSource = dt;
            }

            // --- TABLO GÖRÜNÜMÜNÜ TEMİZLEME VE DÜZELTME AYARLARI ---

            // 1. Sütunların genişliğini DataGridView'e tam sığdırır (Yana kaydırma çubuğu yok olur)
            dgvLeadership.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // 2. En sol taraftaki ok işaretli gri sütunu (RowHeaders) gizler
            dgvLeadership.RowHeadersVisible = false;

            // 3. Alt ve yan kaydırma çubuklarını tamamen kapatır
            dgvLeadership.ScrollBars = ScrollBars.None;

            // 4. Kullanıcının satır/sütun eklemesini veya boyutlandırmasını engeller
            dgvLeadership.AllowUserToAddRows = false;
            dgvLeadership.AllowUserToResizeRows = false;
            dgvLeadership.AllowUserToResizeColumns = false;
            dgvLeadership.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvLeadership.ReadOnly = true;
        }
        // Saf C# ile kullanıcıdan isim alan yardımcı metot
        private string ShowInputBox(string prompt, string title)
        {
            Form promptForm = new Form()
            {
                Width = 350,
                Height = 170,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = title,
                StartPosition = FormStartPosition.CenterScreen,
                MaximizeBox = false,
                MinimizeBox = false
            };

            Label textLabel = new Label() { Left = 20, Top = 20, Text = prompt, AutoSize = true };
            TextBox textBox = new TextBox() { Left = 20, Top = 50, Width = 290 };
            Button confirmation = new Button() { Text = "Tamam", Left = 210, Width = 100, Top = 85, DialogResult = DialogResult.OK };

            promptForm.Controls.Add(textLabel);
            promptForm.Controls.Add(textBox);
            promptForm.Controls.Add(confirmation);
            promptForm.AcceptButton = confirmation;

            return promptForm.ShowDialog() == DialogResult.OK ? textBox.Text : "Oyuncu";
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            // Eğer basılan tuş ok tuşlarından biriyse, DataGridView'in tuşu yakalamasını engelle
            if (keyData == Keys.Left || keyData == Keys.Right || keyData == Keys.Up || keyData == Keys.Down)
            {
                // Olayı doğrudan KeyIsDown metoduna manuel yönlendiriyoruz
                KeyEventArgs args = new KeyEventArgs(keyData);
                KeyIsDown(this, args);
                return true; // Tuşu biz işledik, DataGridView dokunmasın!
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}
