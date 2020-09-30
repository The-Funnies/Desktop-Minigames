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

    public partial class Minigames : Form
    {
        public Minigames()
        {
            Width = (int)(Screen.PrimaryScreen.WorkingArea.Size.Width / 3.5);
            Height = (int)(Screen.PrimaryScreen.WorkingArea.Size.Height / 1.25);

            Label minigames = new Label();
            minigames.Text = "Minigames";
            minigames.Font = new Font("Ariel", 30);
            minigames.Size = new Size(Width/2 , Height/9);
            minigames.Location = new Point(Width / 2 -(int)(minigames.Size.Width /2.4), Height / 10);
            Controls.Add(minigames);

            Button snake = new Button();
            snake.Text = "Snake";
            snake.Font = new Font("Ariel", 25);
            snake.Size = new Size(150,150);
            snake.Location = new Point(Width / 7, Height / 4);
            snake.Click += GoToGame;
            Controls.Add(snake);

            Button solitaire = new Button();
            solitaire.Text = "Solitaire";
            solitaire.Font = new Font("Ariel", 25);
            solitaire.Size = new Size(150, 150);
            solitaire.Location = new Point((int)(Width / 1.8), Height / 4);
            solitaire.Click += GoToGame;
            Controls.Add(solitaire);

            Button flappy = new Button();
            flappy.Text = "Flappy Bird";
            flappy.Font = new Font("Ariel", 25);
            flappy.Size = new Size(150, 150);
            flappy.Location = new Point(Width / 7, (int)(Height / 2.15));
            flappy.Click += GoToGame;
            Controls.Add(flappy);

            Button whist = new Button();
            whist.Text = "Whist";
            whist.Font = new Font("Ariel", 25);
            whist.Size = new Size(150, 150);
            whist.Location = new Point((int)(Width / 1.8), (int)(Height / 2.15));
            whist.Click += GoToGame;
            Controls.Add(whist);
        }
        public void GoToGame(object sender,EventArgs args)
        {
            Button but = (Button)sender;
            if (but.Text == "Snake")
            {
                Snake hostForm = new Snake();
                hostForm.StartPosition = FormStartPosition.Manual;
                hostForm.Location = new Point(this.Location.X, 0);
                this.Hide();
                Controls.Clear();
                hostForm.Show();
            }
            else
            {
                if (but.Text == "Solitaire")
                {
                    Solitaire hostForm = new Solitaire();
                    hostForm.StartPosition = FormStartPosition.Manual;
                    hostForm.Location = new Point(this.Location.X, 0);
                    this.Hide();
                    Controls.Clear();
                    hostForm.Show();
                }
                else
                {
                    if (but.Text == "Flappy Bird")
                    {
                        Flappy hostForm = new Flappy();
                        hostForm.StartPosition = FormStartPosition.Manual;
                        hostForm.Location = new Point(this.Location.X, 0);
                        this.Hide();
                        Controls.Clear();
                        hostForm.Show();
                    }
                    else
                    {
                        if (but.Text == "Whist")
                        {
                            WhistMain hostForm = new WhistMain();
                            hostForm.StartPosition = FormStartPosition.Manual;
                            hostForm.Location = new Point(this.Location.X, 0);
                            this.Hide();
                            Controls.Clear();
                            hostForm.Show();
                        }
                    }
                }
            }
        }

        private void Minigames_Load(object sender, EventArgs e)
        {

        }
    }
}
