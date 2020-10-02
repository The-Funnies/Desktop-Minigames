using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pong
{
    public partial class PongGame : Form
    {
        public Client server;
        private string name;

        private Panel playground;
        private PictureBox ball;
        private PictureBox myRacket;
        private PictureBox enemyRacket;
        private Label infoLabel;
        private Label myScore;
        private Label enemyScore;
        private bool canExit;

        public PongGame(string name)
        {
            InitializeComponent();

            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.ResizeRedraw, true);

            this.canExit = true;
            this.name = name;
        }

        private void InitializeMyComponent()
        {
            //Set window to fullscreen mode
            this.FormBorderStyle = FormBorderStyle.None;
            this.TopMost = true;
            this.Bounds = Screen.PrimaryScreen.Bounds;
            
            //playground
            this.playground = new Panel();
            this.playground.Dock = DockStyle.Fill;
            this.Controls.Add(playground);

            //myRacket
            this.myRacket = new PictureBox();
            this.myRacket.Size = new Size(Constants.RACKET_WIDTH, Constants.RACKET_HEIGHT);
            this.myRacket.BackColor = Color.Black;
            this.myRacket.Location = new Point(this.playground.Right - 40 - this.myRacket.Width, this.playground.Height / 2 - this.myRacket.Height / 2);
            this.playground.Controls.Add(myRacket);

            //enemyRacket
            this.enemyRacket = new PictureBox();
            this.enemyRacket.Size = new Size(Constants.RACKET_WIDTH, Constants.RACKET_HEIGHT);
            this.enemyRacket.BackColor = Color.Black;
            this.enemyRacket.Location = new Point(this.playground.Left + 40, this.playground.Height / 2 - this.enemyRacket.Height / 2);
            this.playground.Controls.Add(enemyRacket);

            //infoLabel
            this.infoLabel = new Label();
            this.infoLabel.AutoSize = false;
            this.infoLabel.Size = new Size(700, 700);
            this.infoLabel.Location = new Point(this.playground.Width / 2 - this.infoLabel.Width / 2, this.playground.Height / 2 - this.infoLabel.Height / 2);
            this.infoLabel.TextAlign = ContentAlignment.MiddleCenter;
            this.infoLabel.Font = new Font(this.Font.FontFamily, 16);
            this.playground.Controls.Add(infoLabel);

            //myScore
            this.myScore = new Label();
            this.myScore.AutoSize = false;
            this.myScore.Size = new Size(100, 50);
            this.myScore.Font = new Font(this.Font.FontFamily, 35);
            this.myScore.Location = new Point(this.playground.Width / 2 + 10, 50);
            this.myScore.TextAlign = ContentAlignment.MiddleCenter;
            this.playground.Controls.Add(this.myScore);

            //enemyScore
            this.enemyScore = new Label();
            this.enemyScore.AutoSize = false;
            this.enemyScore.Size = new Size(100, 50);
            this.enemyScore.Font = new Font(this.Font.FontFamily, 35);
            this.enemyScore.Location = new Point(this.playground.Width / 2 - 110, 50);
            this.enemyScore.TextAlign = ContentAlignment.MiddleCenter;
            this.playground.Controls.Add(this.enemyScore);

            //ball
            this.ball = new PictureBox();
            this.ball.BackColor = Color.Transparent;
            this.ball.Image = Desktop_Minigames.Properties.Resources.Redball_remove;
            this.ball.Size = new Size(Constants.BALLWIDTH, Constants.BALLHEIGHT);
            this.ball.SizeMode = PictureBoxSizeMode.StretchImage;
            this.ball.Hide();
            this.playground.Controls.Add(this.ball);
        }

        private void ResetBoard()
        {
            this.ball.Invoke((MethodInvoker)delegate ()
            {
                this.ball.Location = new Point(playground.Width / 2, playground.Height / 2);
                this.ball.BringToFront();
            });
            this.myRacket.Invoke((MethodInvoker)delegate ()
            {
                this.myRacket.Location = new Point(this.playground.Right - 40 - this.myRacket.Width, this.playground.Height / 2 - this.myRacket.Height / 2);
                this.myRacket.BringToFront();
            });
            this.enemyRacket.Invoke((MethodInvoker)delegate ()
            {
                this.enemyRacket.Location = new Point(this.playground.Left + 40, this.playground.Height / 2 - this.enemyRacket.Height / 2);
                this.enemyRacket.BringToFront();
            });

        }
        private void PongGame_Shown(object sender, EventArgs e)
        {
            InitializeMyComponent();
            int[] resolution = new int[2] { this.playground.Width, this.playground.Height };

            Thread GameThread = new Thread(() =>
            {
                this.server.FirstSend(this.name, resolution);
                Tuple<string, bool, ServerOption> results = this.server.ReciveServerFirstAnswer();
                if (results.Item2)
                {
                    WriteInfo(results.Item3, results.Item1);
                    if (results.Item3 == ServerOption.freeplay)
                    {
                        FreePlay();
                    }
                }
                else
                {
                    MessageBox.Show($"You cannot join {results.Item1}'s server because" +
                        $" your screen resolution is {resolution[0]},{resolution[1]} and not 1920,1080", "error");
                    Application.Exit();
                }
            });
            GameThread.Start();
        }

        private void FreePlay()
        {
            ResetScore();
            while (true)
            {
                ResetBoard();
                bool[] didIWin = PlayGame();
                HideBall();
                if (didIWin[0])
                {
                    AddScore(myScore);
                }
                else
                {
                    AddScore(enemyScore);
                }
                if (didIWin[1])
                {
                    break;
                }
            }           
        }
        private void SetRacketPosition()
        {
            while (true)
            {
                ChangeTop(this.myRacket, Cursor.Position.Y - (this.myRacket.Height / 2));
            }
        }

        private void ChangeTop(PictureBox box, int top)
        {
            if (box.InvokeRequired)
            {
                box.Invoke((MethodInvoker)delegate ()
                {
                    box.Top = top;
                });
            }
        }

        private void ChangeLeft(PictureBox box, int left)
        {
            if (box.InvokeRequired)
            {
                box.Invoke((MethodInvoker)delegate ()
                {
                    box.Left = left;
                });
            }
        }

        private bool[] PlayGame()
        {
            this.canExit = false;
            string opponentName = server.ReciveName();
            SetInfoText($"Playing against {opponentName}");
            for (int i = 0; i < 3; i++)
            {
                int startIn = server.ReciveInt();
                SetInfoText($"Starting in {startIn.ToString()}");
            }
            Thread.Sleep(1000);
            SetInfoText("");
            this.canExit = true;
            ShowBall();
            Thread SetRacketWithCursor = new Thread(SetRacketPosition);
            SetRacketWithCursor.Start();

            Tuple<bool, WhoWins, int, int, int> results;
            while (true)
            {
                if (!this.server.IsConnected())
                {
                    SetRacketWithCursor.Abort();
                    SetInfoText("Lost connection...");
                    return new bool[2] {false, true};
                }
                results = this.server.ReciveServer();
                if (results.Item1)
                {
                    SetRacketWithCursor.Abort();
                    SetInfoText("Opponent disconnected, you won");
                    return new bool[2] { true, true };
                }
                ChangeLeft(this.ball, results.Item3);
                ChangeTop(this.ball, results.Item4);
                ChangeTop(this.enemyRacket, results.Item5);
                this.Update();
                this.Invalidate();

                switch (results.Item2)
                {
                    case WhoWins.you:
                        SetRacketWithCursor.Abort();
                        SetInfoText("you won!");
                        return new bool[2] { true, false };

                    case WhoWins.enemy:
                        SetRacketWithCursor.Abort();
                        SetInfoText("You lost");
                        return new bool[2] { false, false };
                }
                server.SendMyRacket(this.myRacket.Top);
            }
        }

        protected override CreateParams CreateParams
        {
            get
            {
                var cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;    // Turn on WS_EX_COMPOSITED
                return cp;
            }
        }

        private void PongGame_FormClosed(object sender, FormClosedEventArgs e)
        {
            Environment.Exit(Environment.ExitCode);
        }

        private void ShowBall()
        {
            this.ball.Invoke((MethodInvoker)delegate ()
            {
                this.ball.Show();
            });
        }

        private void HideBall()
        {
            this.ball.Invoke((MethodInvoker)delegate ()
            {
                this.ball.Hide();
            });
        }

        private void SetInfoText(string text)
        {
            if (this.infoLabel.InvokeRequired)
            {
                this.infoLabel.Invoke((MethodInvoker)delegate ()
                {
                    this.infoLabel.Text = text;
                });
            }
            else
            {
                this.infoLabel.Text = text;
            }
        }
        private void AddInfoText(string text)
        {
            if (this.infoLabel.InvokeRequired)
            {
                this.infoLabel.Invoke((MethodInvoker)delegate ()
                {
                    this.infoLabel.Text += text + Environment.NewLine;
                });
            }
            else
            {
                this.infoLabel.Text += text + Environment.NewLine;
            }
        }

        private void MinimizeWindow()
        {
            this.Invoke((MethodInvoker)delegate ()
            {
                this.FormBorderStyle = FormBorderStyle.SizableToolWindow;
                this.TopMost = false;
                this.Bounds = new Rectangle(new Point(400, 300), new Size(300, 300));
            });
        }

        private void PongGame_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Escape)
            {
                if (this.canExit)
                {
                    Environment.Exit(Environment.ExitCode);
                }
            }

            if (e.KeyCode == Keys.F11)
            {
                if (this.FormBorderStyle == FormBorderStyle.Sizable)
                {
                    this.FormBorderStyle = FormBorderStyle.None;
                    this.TopMost = true;
                    this.Bounds = Screen.PrimaryScreen.Bounds; 
                }
                else
                {
                    this.FormBorderStyle = FormBorderStyle.Sizable;
                }
            }

        }

        private void ResetScore()
        {
            this.myScore.Invoke((MethodInvoker)delegate ()
            {
               this.myScore.Text = "0";
            });

            this.enemyScore.Invoke((MethodInvoker)delegate ()
            {
                this.enemyScore.Text = "0";
            });
        }

        private void AddScore(Label scoreLabel)
        {
            scoreLabel.Invoke((MethodInvoker)delegate ()
            {
                scoreLabel.Text = (int.Parse(scoreLabel.Text) + 1).ToString();
            });
        }

        private void WriteInfo(ServerOption option, string name)
        {
            AddInfoText($"You have successfully connected to {name}'s server");
            switch (option)
            {
                case ServerOption.freeplay:
                    AddInfoText("The server's gamemode is set to free play, you will play against " +
                        "\nyour opponent for infinite number of games");
                    break;

                case ServerOption.twoP:
                    AddInfoText("The server's gamemode is set to Two player tournament, you will play\n" +
                        " 5 games against your opponent and the player with most wins will win!");
                    break;

                case ServerOption.fourP:
                    AddInfoText("The server's gamemode is set to Four player tournament, you will play\n" +
                        " 3 games against your first opponent and then if you win you will play at the finals!");
                    break;

                case ServerOption.eightP:
                    AddInfoText("The server's gamemode is set Eight player tournament, you will play\n" +
                        " 3 games against your first opponent and then if you win you will play at the semi-finals \n" +
                        "and then if you win you will play at the finals!");
                    break;
            }
            AddInfoText("The right racket is your racket, the left racket is your enemy's racket");
            AddInfoText("Press escape to exit and press f11 to exit fullscreen");
            string playerOrPlayers = option == ServerOption.freeplay || option == ServerOption.twoP ? "player" : "players";
            AddInfoText($"\nWaiting for other {playerOrPlayers} to join...");
        }
       
    }
}
