using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;

namespace flappy_bird
{
    delegate void updateCounter();
    delegate void changePipeLocation(Pipe p);
    delegate void moveControlToLocation(Control c, Point p);
    delegate void singleControl(Control c);
    public partial class Form1 : Form
    {
        private List<Pipe> pipes = new List<Pipe>();
        private static Random rnd = new Random();
        private int direction = -3;
        private Label bird;
        private bool gameOver = false;
        private int gameCounter = 0;
        Label gameCounterLabel = new Label();
        public Form1()
        {
            InitializeComponent();

            this.Size=new Size(800, 400+Consts.BIRD_HEIGHT*2-15);
            Image bird_image = Image.FromFile("../../pictures/flappy_bird_bird.jpg");
            bird_image = Consts.ResizeImage(bird_image, 50, 50);
            Image background_image = Image.FromFile("../../pictures/flappy_bird_background.png");
            background_image = Consts.ResizeImage(background_image, this.Width, this.Height);
            
            gameCounterLabel.Text = "" + gameCounter;
            gameCounterLabel.Size = new Size(50, 20);
            Controls.Add(gameCounterLabel);

            bird = new Label();
            bird.Size = new Size(Consts.BIRD_WIDTH, Consts.BIRD_HEIGHT);
            bird.Image = bird_image;
            bird.Location = new Point(200, 200);
            this.BackgroundImage = background_image;
            Controls.Add(bird);

            Button start_button = new Button();
            start_button.Text = "Start!";
            start_button.Click += new EventHandler(start);
            start_button.Location = new Point(100, 200);
            start_button.Size = new Size(200, 200);
            Controls.Add(start_button);
        }

        public void start(object sender, EventArgs e)
        {
            Button b = sender as Button;
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
                    Thread.Sleep(10);
                    Invoke(new moveControlToLocation(moveControl), pipe.LowerPipe, new Point(pipe.LowerPipe.Location.X - 6, pipe.LowerPipe.Location.Y));
                   
                    Invoke(new moveControlToLocation(moveControl), pipe.HigherPipe, new Point(pipe.HigherPipe.Location.X - 6, pipe.HigherPipe.Location.Y));
                }
                updateGame();
                //this.Invalidate();
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
            int lowerPipeHeight = rnd.Next(20, (this.Height - Consts.BIRD_HEIGHT * 4 - 20));
            int higherPipeHeight = Height - lowerPipeHeight - Consts.BIRD_HEIGHT * 4;

            pipe.LowerPipe.Size = new Size(Consts.PIPE_WIDTH, lowerPipeHeight);
            pipe.HigherPipe.Size = new Size(Consts.PIPE_WIDTH, higherPipeHeight);

            pipe.LowerPipe.Image = Consts.ResizeImage(pipe.LowerPipe.Image, Consts.PIPE_WIDTH, lowerPipeHeight);
            pipe.HigherPipe.Image = Consts.ResizeImage(pipe.HigherPipe.Image, Consts.PIPE_WIDTH, higherPipeHeight);

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
                if ((birdCounter++) % 5 == 0 && direction > -6)
                {
                    birdCounter = 1;
                    direction--;
                }
                Console.WriteLine(y);
                y = Cursor.Position.Y;
                if (y > 400)
                {
                    y = 400;
                }
                else if(y<0)
                {
                    y = 0;
                }
                Invoke(new moveControlToLocation(moveControl), bird, new Point(bird.Location.X, y));
            }
        }
        public void createPipes(int x)
        {
            int lowerPipeHeight = rnd.Next(20, (this.Height - Consts.BIRD_HEIGHT * 4 - 20));
            int higherPipeHeight = Height - lowerPipeHeight - Consts.BIRD_HEIGHT * 4;

            Image lowerPipeImage = Image.FromFile("../../pictures/flappy_bird_pipe.png");
            lowerPipeImage = Consts.ResizeImage(lowerPipeImage, Consts.PIPE_WIDTH, lowerPipeHeight);

            Image higherPipeImage = Image.FromFile("../../pictures/flappy_bird_pipe_rotated.png");
            higherPipeImage = Consts.ResizeImage(higherPipeImage, Consts.PIPE_WIDTH, higherPipeHeight);

            Label lowerPipe = new Label();
            lowerPipe.Location = new Point(x, 400 + Consts.BIRD_HEIGHT - lowerPipeHeight);
            lowerPipe.Image = lowerPipeImage;
            lowerPipe.Size = new Size(Consts.PIPE_WIDTH, lowerPipeHeight);

            Label higherPipe = new Label();
            higherPipe.Location = new Point(x, 0);
            higherPipe.Image = higherPipeImage;
            higherPipe.Size = new Size(Consts.PIPE_WIDTH, higherPipeHeight);
            Invoke(new singleControl(addControl), higherPipe);
            Invoke(new singleControl(addControl), lowerPipe);

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
                    Invoke(new updateCounter(changeCounter));
                }
                if(pipes[i].LowerPipe.Location.X-Consts.BIRD_WIDTH+1<Consts.BIRD_X
                    && pipes[i].LowerPipe.Location.X + Consts.BIRD_WIDTH - 1 > Consts.BIRD_X
                        &&(pipes[i].LowerPipe.Location.Y<=bird.Location.Y+Consts.BIRD_HEIGHT
                            ||pipes[i].HigherPipe.Location.Y+pipes[i].HigherPipe.Height>=bird.Location.Y))
                {
                    gameOver = true;
                }
            }
        }
        private void changeCounter()
        {
            gameCounterLabel.Text = "" + gameCounter;
        }
        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            if (direction >= 5)
            {
                direction++;
            }
            else
            {
                direction = 5;
            }
        }
    }
}

