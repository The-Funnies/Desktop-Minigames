using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace flappy_bird
{
    delegate void noParamsDelegate();
    delegate void changePipeLocation(Pipe p);
    delegate void moveControlToLocation(Control c, Point p);
    delegate void singleControlDelegate(Control c);
    public partial class FlappyBird : Form
    {
        private List<Pipe> pipes = new List<Pipe>();
        private static Random rnd = new Random();
        private int direction = -3;
        private PictureBox bird;
        private bool gameOver = false;
        private int gameCounter = 0;
        Label gameCounterLabel = new Label();
        public FlappyBird()
        {
            InitializeComponent();
         
            this.Size = new Size(800, 400 + Consts.BIRD_HEIGHT * 2 - 15);
            Image bird_image = Desktop_Minigames.Properties.Resources.flappy_bird_bird;
            bird_image = Consts.ResizeImage(bird_image, Consts.BIRD_WIDTH, Consts.BIRD_HEIGHT);
            Image background_image = Desktop_Minigames.Properties.Resources.flappy_bird_background;
            background_image = Consts.ResizeImage(background_image, this.Width, this.Height);

            gameCounterLabel.Text = "" + gameCounter;
            gameCounterLabel.Font = new Font("Arial",20);
            gameCounterLabel.Size = new Size(50, 50);
            gameCounterLabel.BackColor = Color.Transparent;
            Controls.Add(gameCounterLabel);

            bird = new PictureBox();
            bird.BackColor = Color.Transparent;
            bird.Size = new Size(Consts.BIRD_WIDTH, Consts.BIRD_HEIGHT);
            bird.Image = bird_image;
            bird.Location = new Point(200, 200);
            this.BackgroundImage = background_image;
            Controls.Add(bird);

            TransparentPictureBox start_button = new TransparentPictureBox();
            start_button.Click += new EventHandler(start);
            start_button.Location = new Point(300, 200);
            start_button.Size = new Size(200, 100);
            start_button.Padding = new Padding(5);
            start_button.Dock = DockStyle.Fill;
            start_button.Image = Consts.ResizeImage(Desktop_Minigames.Properties.Resources.flappy_bird_start_button, 200, 100);

            Controls.Add(start_button);
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
        public void start(object sender, EventArgs e)
        {
            TransparentPictureBox b = sender as TransparentPictureBox;
            Controls.Remove(b);

            Thread bird_thread = new Thread(moveBird);
            bird_thread.Start();

            Thread pipes_thread = new Thread(generatePipes);
            pipes_thread.Start();

        }
        private int generateCounter = 0;
        public void generatePipes()
        {
            createPipes(this.Width);
            createPipes(this.Width + 450);
            while (!gameOver)
            {
                if ((generateCounter += 5) >= this.Width / 2)
                {
                    generateCounter = 0;
                }
                foreach (Pipe pipe in pipes)
                {
                    Thread.Sleep(5);
                    Invoke(new moveControlToLocation(moveControl), pipe.LowerPipe, new Point(pipe.LowerPipe.Location.X - 6, pipe.LowerPipe.Location.Y));

                    Invoke(new moveControlToLocation(moveControl), pipe.HigherPipe, new Point(pipe.HigherPipe.Location.X - 6, pipe.HigherPipe.Location.Y));
                }
                updateGame();                
                for (int i = 0; i < pipes.Count; i++)
                {
                    if (pipes[i].HigherPipe.Location.X < -Consts.PIPE_WIDTH)
                    {
                        Invoke(new changePipeLocation(changePipeLocation), pipes[i]);
                    }
                }
            }
        }
        public void changePipeLocation(Pipe pipe)
        {
            int lowerPipeHeight = rnd.Next(20, (this.Height - Consts.BIRD_HEIGHT * Consts.PIPE_BIRDS_SPACE - 20));
            int higherPipeHeight = Height - lowerPipeHeight - Consts.BIRD_HEIGHT * Consts.PIPE_BIRDS_SPACE;


            pipe.LowerPipe.Size = new Size(Consts.PIPE_WIDTH, lowerPipeHeight);
            pipe.HigherPipe.Size = new Size(Consts.PIPE_WIDTH, higherPipeHeight);

            pipe.LowerPipe.Image = Consts.ResizeImage(Desktop_Minigames.Properties.Resources.flappy_bird_pipe, Consts.PIPE_WIDTH, lowerPipeHeight);
            pipe.HigherPipe.Image = Consts.ResizeImage(Desktop_Minigames.Properties.Resources.flappy_bird_pipe_rotated, Consts.PIPE_WIDTH, higherPipeHeight);

            moveControl(pipe.LowerPipe, new Point(this.Width, 400 + Consts.BIRD_HEIGHT - lowerPipeHeight));
            moveControl(pipe.HigherPipe, new Point(this.Width, pipe.HigherPipe.Location.Y));
        }
        private int birdCounter = 0;
        public void moveBird()
        {
            while (!gameOver)
            {
                int y = bird.Location.Y - direction;
                if (y > 400)
                {
                    y = 400;
                }
                if (y < 0)
                {
                    y = 0;
                }
                if ((birdCounter++) % 10 == 0 && direction > -Consts.BIRD_MAX_DISTANCE)
                {
                    birdCounter = 1;
                    direction--;
                }
                Thread.Sleep(1);
                Invoke(new moveControlToLocation(moveControl), bird, new Point(bird.Location.X, y));
            }
            gameOverScreen();
        }
        public void createPipes(int x)
        {
            int lowerPipeHeight = rnd.Next(20, (this.Height - Consts.BIRD_HEIGHT * Consts.PIPE_BIRDS_SPACE - 20));
            int higherPipeHeight = Height - lowerPipeHeight - Consts.BIRD_HEIGHT * Consts.PIPE_BIRDS_SPACE;

            Image lowerPipeImage = Desktop_Minigames.Properties.Resources.flappy_bird_pipe;
            lowerPipeImage = Consts.ResizeImage(lowerPipeImage, Consts.PIPE_WIDTH, lowerPipeHeight);

            Image higherPipeImage = Desktop_Minigames.Properties.Resources.flappy_bird_pipe_rotated;
            higherPipeImage = Consts.ResizeImage(higherPipeImage, Consts.PIPE_WIDTH, higherPipeHeight);

            PictureBox lowerPipe = new PictureBox();
            lowerPipe.Location = new Point(x, 400 + Consts.BIRD_HEIGHT - lowerPipeHeight);
            lowerPipe.Image = lowerPipeImage;
            lowerPipe.Size = new Size(Consts.PIPE_WIDTH, lowerPipeHeight);

            PictureBox higherPipe = new PictureBox();
            higherPipe.Location = new Point(x, 0);
            higherPipe.Image = higherPipeImage;
            higherPipe.Size = new Size(Consts.PIPE_WIDTH, higherPipeHeight);
            Invoke(new singleControlDelegate(addControl), higherPipe);
            Invoke(new singleControlDelegate(addControl), lowerPipe);

            lowerPipe.Click += new EventHandler(pipeClick);
            higherPipe.Click += new EventHandler(pipeClick);

            lowerPipe.BackColor = Color.Transparent;
            higherPipe.BackColor = Color.Transparent;

            Pipe pipe = new Pipe();
            pipe.LowerPipe = lowerPipe;
            pipe.HigherPipe = higherPipe;
            pipes.Add(pipe);
        }
        public void moveControl(Control c, Point p)
        {
            c.Location = p;
        }
        public void addControl(Control c)
        {
            Controls.Add(c);
        }
        private void updateGame()
        {
            for (int i = 0; i < pipes.Count; i++)
            {
                if (pipes[i].LowerPipe.Location.X == Consts.BIRD_X)
                {
                    gameCounter++;
                    Invoke(new noParamsDelegate(changeCounter));
                }
                if (pipes[i].LowerPipe.Location.X < Consts.BIRD_X
                    && pipes[i].LowerPipe.Location.X + Consts.PIPE_WIDTH-15 > Consts.BIRD_X
                        && (pipes[i].LowerPipe.Location.Y <= bird.Location.Y + Consts.BIRD_HEIGHT
                            || pipes[i].HigherPipe.Location.Y + pipes[i].HigherPipe.Height >= bird.Location.Y))
                {
                    gameOver = true;
                }
            }
        }
        private void changeCounter()
        {
            gameCounterLabel.Text = "" + gameCounter;
        }
        private void pipeClick(object sender, EventArgs e)
        {
            clickOnScreen();
        }
        private void clickOnScreen()
        {
            if (direction >= Consts.BIRD_MAX_DISTANCE)
            {
                direction++;
            }
            else
            {
                direction = Consts.BIRD_MAX_DISTANCE;
            }
        }
        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            clickOnScreen();
        }
        private void removeAllPipes()
        {
            for(int i = 0; i < pipes.Count; i++)
            {
                Controls.Remove(pipes[i].LowerPipe);
                Controls.Remove(pipes[i].HigherPipe);
            }
            pipes.Clear();
        }
        private void gameOverScreen()
        {
            if (MessageBox.Show("Your score was " + gameCounter + "\nContinue Playing?", "Game Over", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                Invoke(new noParamsDelegate(removeAllPipes));
                Invoke(new noParamsDelegate(createNewGame));
            }
            else
            {
                
                Invoke(new noParamsDelegate(GotoMain));
            }
            
        }
        private void GotoMain()
        {
            GoToForm(new Desktop_Minigames.Minigames());
        }
        public void GoToForm<T>(T form) where T : Form
        {
            form.StartPosition = FormStartPosition.Manual;
            form.Location = new Point(this.Location.X, 0);
            form.FormClosed += (object sender, FormClosedEventArgs e) => { Environment.Exit(Environment.ExitCode); };

            form.Text = form.GetType().Name;
            if (!Desktop_Minigames.Minigames.noBg.Contains(form.Text))
            {
                Desktop_Minigames.Minigames.SetBackground(form);
            }

            this.Hide();

            Controls.Clear();
            try
            {
                form.Show();
            }
            catch
            {
            }
            if (!Desktop_Minigames.Minigames.noFullWindow.Contains(form.Text))
            {
                form.WindowState = FormWindowState.Maximized;
            }

            
        }
        
        private void createNewGame()
        {
            TransparentPictureBox start_button = new TransparentPictureBox();
            start_button.Click += new EventHandler(start);
            start_button.Location = new Point(300, 200);
            start_button.Size = new Size(200, 100);
            start_button.Padding = new Padding(5);
            start_button.Dock = DockStyle.Fill;
            start_button.Image = Consts.ResizeImage(Desktop_Minigames.Properties.Resources.flappy_bird_start_button, 200, 100);
            Controls.Add(start_button);
            bird.Location = new Point(200, 200);
            gameOver = false;
            gameCounter = 0;
            changeCounter();
        }
        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            if (e.KeyChar == ' ')
            {
                clickOnScreen();
                e.Handled = true;
            }
        }

        private void FlappyBird_Load(object sender, EventArgs e)
        {

        }
    }
}

