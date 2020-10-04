using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Desktop_Minigames
{
    public partial class Snake : Form
    {
        private const int blockSize = 25;
        private const int height = 20, width = 20;
        private bool godMode = false;
        private double wait = 200;
        private Control[,] board = new Control[height, width];
        private SnakeGame game = new SnakeGame(height, width);
        private Queue<byte> commandQueue = new Queue<byte>();
        private Thread gameThread;

        public Snake()
        {
            this.BackColor = Color.FromArgb(0, 0, 100);
            this.Size = new Size(blockSize * (width + 1), blockSize * (height + 2));
            this.Text = "Snake";
            Block[,] blocks = game.GetBoard();
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    Control tmp = new Control();
                    tmp.Location = new Point(j * blockSize, i * blockSize);
                    tmp.BackColor = GetBlockColor(blocks[i, j]);
                    tmp.Size = new Size(blockSize, blockSize);
                    tmp.Tag = new Tuple<int, int>(j, i);
                    tmp.Click += ControlClick;
                    board[i, j] = tmp;
                    Controls.Add(tmp);
                }
            }
            gameThread = new Thread(ProcessGame);
        }

        private void ControlClick(object sender, EventArgs e)
        {
            if (godMode)
            {
                Control control = (Control)sender;
                Tuple<int, int> point = (Tuple<int, int>)control.Tag;
                Block[,] blocks = game.GetBoard();
                if (blocks[point.Item2, point.Item1] == Block.Food)
                {
                    blocks[point.Item2, point.Item1] = Block.Blank;
                }
                else blocks[point.Item2, point.Item1]++;
                board[point.Item2, point.Item1].BackColor = GetBlockColor(blocks[point.Item2, point.Item1]);
            }
        }

        public void ProcessGame()
        {
            while (true)
            {
                if (commandQueue.Count == 0)
                {
                    float[] input = new float[3];
                    input[0] = 1;
                    game.Tick(input);
                }
                else
                {
                    game.ProcessKey(commandQueue.Dequeue());
                }

                if (game.isEnd)
                {
                    if (godMode)
                    {
                        game.isEnd = false;
                    }
                    else
                    {
                        MessageBox.Show("You lost!\nFinal score: " + game.GetScore());
                        game = new SnakeGame(height, width);
                        Block[,] blocks = game.GetBoard();
                        for (int i = 0; i < height; i++)
                        {
                            for (int j = 0; j < width; j++)
                            {
                                board[i, j].BackColor = GetBlockColor(blocks[i, j]);
                            }
                        }
                        commandQueue.Clear();
                        break;
                    }
                }

                Tuple<int, int>[] diffs = game.GetDiff();
                foreach (Tuple<int, int> diff in diffs)
                {
                    if (diff.Item2 >= height || diff.Item1 >= width || diff.Item1 < 0 || diff.Item2 < 0)
                        continue;
                    board[diff.Item2, diff.Item1].BackColor = GetBlockColor(game.GetBoard()[diff.Item2, diff.Item1]);
                }
                Thread.Sleep((int)wait);
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (gameThread.ThreadState == ThreadState.Unstarted || gameThread.ThreadState == ThreadState.Stopped)
            {
                gameThread.Abort();
                gameThread = new Thread(ProcessGame);
                gameThread.Start();
            }

            if (commandQueue.Count >= 6) return base.ProcessCmdKey(ref msg, keyData);
            switch (keyData)
            {
                case Keys.Up:
                    commandQueue.Enqueue(0);
                    break;

                case Keys.Left:
                    commandQueue.Enqueue(1);
                    break;

                case Keys.Right:
                    commandQueue.Enqueue(2);
                    break;

                case Keys.Down:
                    commandQueue.Enqueue(3);
                    break;

                case Keys.OemSemicolon:
                    godMode = !godMode;
                    break;

                case Keys.F1:
                    if (godMode)
                    {
                        wait *= 2;
                    }
                    break;

                case Keys.F2:
                    if (godMode)
                    {
                        wait /= 2;
                    }
                    break;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        public Color GetBlockColor(Block block)
        {
            return block == Block.Blank ? Color.FromName("Black") : block == Block.Snake ? Color.FromName("White") : Color.FromName("Red");
        }
    }
}
