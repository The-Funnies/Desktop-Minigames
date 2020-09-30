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

            Button[] games = new Button[6];

            for (int i = 0; i < games.Length; i++)
            {
                games[i] = new Button
                {
                    Font = new Font("Ariel", 25),
                    Size = new Size(150, 150),
                    Location = new Point(i % 2 == 0 ? Width / 7 : (int)(Width / 1.8), minigames.Location.Y + minigames.Height + (Height / 4) * (i / 2))
                };
                games[i].Click += GoToGame;
                Controls.Add(games[i]);
            }

            games[0].Text = "Snake";
            games[1].Text = "Solitaire";
            games[2].Text = "Flappy Bird";
            games[3].Text = "Whist";
            games[4].Text = "Ultimate Tic Tac Toe";
            games[5].Text = "Ultimate Ultimate Tic Tac Toe";
        }
        public void GoToGame(object sender,EventArgs args)
        {
            Button but = (Button)sender;
            switch (but.Text)
            {
                case "Snake":
                    GoToForm<Snake>(new Snake());
                    break;
                case "Solitaire":
                    GoToForm<Solitaire>(new Solitaire());
                    break;
                case "Flappy Bird":
                    GoToForm<Flappy>(new Flappy());
                    break;
                case "Whist":
                    GoToForm<WhistMain>(new WhistMain());
                    break;
                case "Ultimate Tic Tac Toe":
                    GoToForm<UltimateTicTacToe>(new UltimateTicTacToe());
                    break;
                case "Ultimate Ultimate Tic Tac Toe":
                    GoToForm<UltimateUltimateTicTacToe>(new UltimateUltimateTicTacToe());
                    break;
            }
        }

        public void GoToForm<T>(T form) where T : Form
        {
            form.StartPosition = FormStartPosition.Manual;
            form.Location = new Point(this.Location.X, 0);
            form.FormClosed += (object sender, FormClosedEventArgs e) => { Environment.Exit(Environment.ExitCode); };
            this.Hide();
            Controls.Clear();
            form.Show();
        }

        private void Minigames_Load(object sender, EventArgs e)
        {

        }
    }
}
