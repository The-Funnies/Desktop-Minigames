using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Desktop_Minigames
{
    enum Condition
    {
        Up,
        Down,
        Left,
        Right
    }
    public partial class Fifteen : Form
    {
        private System.Windows.Forms.Timer timer;
        private Button[] buttons;
        public Fifteen()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Button reset = new Button();
            reset.Size = new Size(100, 25);
            reset.Text = "New Game";
            reset.Click += new EventHandler(newGame);
            Controls.Add(reset);
            timer = new System.Windows.Forms.Timer();
            timer.Interval = 1;

            timer.Tick += new EventHandler(moveButton);
            buttons = new Button[15];

            Random rnd = new Random();
            for (int i = 0; i < buttons.Length; i++)
            {
                //adjusting buttons text color location size font and click event
                buttons[i] = new Button();
                buttons[i].Size = new Size(100, 100);
                buttons[i].Text = "" + (i + 1);
                buttons[i].BackColor = Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256));
                buttons[i].Location = new Point((i % 4) * 100, (i / 4) * 100 + 25);
                buttons[i].Font = new Font("Arial", 24, FontStyle.Bold);
                buttons[i].Click += new EventHandler(button_click);

                Controls.Add(buttons[i]);
            }
            //shuffling board
            ShuffleBoard();
            //adjusting form size
            this.Height = (int)((double)buttons[0].Height * 4.4 + 25);
            this.Width = (int)((double)buttons[0].Width * 4.17);

        }
        public void newGame(object sender, EventArgs e)
        {
            if (timer.Enabled)
            {
                return;
            }
           
            ShuffleBoard();
        }
        public void ShuffleBoard()
        {
            Random rnd = new Random();
            foreach (Button b in buttons)
            {
                b.BackColor = Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256));
                if (b.Location.Equals(new Point(300, 325)))
                {
                    
                    for(int i = 0; i < buttons.Length; i++)
                    {
                        bool flag = true;
                        foreach (Button b1 in buttons)
                        {
                            if (b1.Location.Equals(new Point((i % 4) * 100, (i / 4) * 100 + 25)))
                                flag = false;
                        }
                        if (flag)
                        {
                            b.Location = new Point((i % 4) * 100, (i / 4) * 100 + 25);
                        }
                    }
                }
            }


            //shuffling the board
            
            for (int i = 0; i < buttons.Length; i++)
            {
                int index = rnd.Next(0, buttons.Length - 1);
                Point tempP = buttons[i].Location;
                buttons[i].Location = buttons[index].Location;
                buttons[index].Location = tempP;
                Button tempB = buttons[i];
                buttons[i] = buttons[index];
                buttons[index] = tempB;
            }

        }
        public void checkWin()
        {
            //checking if the game is over
            bool flag = true;
            foreach (Button b in buttons)
            {
                int num = int.Parse(b.Text);
                if (!b.Location.Equals(new Point(((num - 1) % 4) * 100, ((num - 1) / 4) * 100+25)))
                {

                    flag = false;
                }
            }
            if (flag)
            {

                if (MessageBox.Show("Continue to play?", "You Won!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {

                    ShuffleBoard();
                }
                else
                {
                    Application.Exit();
                }
            }
        }
        private Condition condition;
        private Button clickedButton;
        public void button_click(object sender, EventArgs e)
        {

            if (timer.Enabled)
                return;

            Button b = sender as Button;
            clickedButton = b;

            bool flag = true;
            foreach (Button button2 in buttons)
            {

                if (button2.Location.Equals(new Point(b.Location.X, b.Location.Y - 100)) || b.Location.Y <= 25)
                {
                    flag = false;
                }
            }
            if (flag)
            {
                condition = Condition.Up;
                timer.Start();
            }
            flag = true;
            foreach (Button button2 in buttons)
            {
                if (button2.Location.Equals(new Point(b.Location.X, b.Location.Y + 100)) || b.Location.Y >= 300)
                {
                    flag = false;
                }
            }
            if (flag)
            {
                condition = Condition.Down;
                timer.Start();
            }
            flag = true;
            foreach (Button button2 in buttons)
            {
                if (button2.Location == new Point(b.Location.X - 100, b.Location.Y) || b.Location.X <= 0)
                {
                    flag = false;
                }
            }
            if (flag)
            {
                condition = Condition.Left;
                timer.Start();
            }
            flag = true;
            foreach (Button button2 in buttons)
            {
                if (button2.Location == new Point(b.Location.X + 100, b.Location.Y) || b.Location.X >= 300)
                {
                    flag = false;
                }
            }
            if (flag)
            {
                condition = Condition.Right;
                timer.Start();
            }

           
        }
        public void checkGameOver()
        {
            for (int i = 0; i < buttons.Length - 2; i++)
            {
                if (!buttons[i].Location.Equals(new Point((i % 4) * 100, (i / 4) * 100 + 25)))
                {
                    return;
                }
            }
            if (buttons[14].Location.Equals(new Point((13 % 4) * 100, (13 / 4) * 100 + 25)) && buttons[13].Location.Equals(new Point((14 % 4) * 100, (14 / 4) * 100 + 25)))
            {
                if (MessageBox.Show("Continue to play?", "You Lost!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {

                    ShuffleBoard();
                }
                else
                {
                    Application.Exit();
                }
            }
        }
        private int counter;
        public void moveButton(object sender, EventArgs e)
        {
            if (condition == Condition.Up)
            {
                clickedButton.Location = new Point(clickedButton.Location.X, clickedButton.Location.Y - 2);

            }
            if (condition == Condition.Down)
            {
                clickedButton.Location = new Point(clickedButton.Location.X, clickedButton.Location.Y + 2);

            }
            if (condition == Condition.Left)
            {
                clickedButton.Location = new Point(clickedButton.Location.X - 2, clickedButton.Location.Y);

            }
            if (condition == Condition.Right)
            {
                clickedButton.Location = new Point(clickedButton.Location.X + 2, clickedButton.Location.Y);

            }
            counter++;
            if (counter == 50)
            {
                counter = 0;
                timer.Stop();
            }
            checkWin();
            checkGameOver();
        }
    }
}
