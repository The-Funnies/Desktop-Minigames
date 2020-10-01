using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Desktop_Minigames
{
    public delegate void goToForm(Ohno form);
    public partial class Minigames : Form
    {
        private Button[] games;
        public static Random random = new Random();
        private Label titleLabel;
        private const int MAIN_BUTTON_SIZE = 125;
        public Minigames()
        {
            Width = (int)(Screen.PrimaryScreen.WorkingArea.Size.Width / 3.5);
            Height = (int)(Screen.PrimaryScreen.WorkingArea.Size.Height / 1.25);

            this.BackgroundImage = Properties.Resources.ResourceManager.GetObject("_" + random.Next(0, 36)) as Image;
            games = new Button[7];
            titleLabel = new Label();
            titleLabel.Text = "Minigames";
            titleLabel.BackColor = Color.Transparent;
            titleLabel.Font = new Font("Ariel", 30);
            titleLabel.Size = new Size(Width / 2, Height / 9);
            titleLabel.Location = new Point(Width / 2 - (int)(titleLabel.Size.Width / 2.4), Height / 10);
            Controls.Add(titleLabel);

            for (int i = 0; i < games.Length; i++)
            {
                games[i] = new Button
                {
                    Font = new Font("Ariel", 25),
                    Size = new Size(MAIN_BUTTON_SIZE, MAIN_BUTTON_SIZE),
                    Location = new Point(MAIN_BUTTON_SIZE / 10 + i % 2 * (MAIN_BUTTON_SIZE + MAIN_BUTTON_SIZE / 10), titleLabel.Location.Y + titleLabel.Height + i / 2 * (MAIN_BUTTON_SIZE + MAIN_BUTTON_SIZE / 10))
                };
                games[i].Click += GoToGame;
                games[i].MouseEnter += (sender, e) => ChangeMainLabelText(sender, e);
                games[i].MouseLeave += (sender, e) => ChangeMainLabelText(sender, e, false);

                Controls.Add(games[i]);
            }
            games[0].Tag = "Snake";
            games[1].Tag = "Solitaire";
            games[2].Tag = "Flappy Bird";
            games[3].Tag = "Whist";
            games[4].Tag = "Ultimate Tic Tac Toe";
            games[5].Tag = "Ultimate Ultimate Tic Tac Toe";
            games[6].Tag = "Bullseye";

            foreach (Button btn in games)
            {
                if (btn == null) continue;
                String logoResourceName = btn.Tag.ToString().Replace(" ", "_");
                Image gameLogo = Properties.Resources.ResourceManager.GetObject(logoResourceName) as Image;
                if (gameLogo == null) continue;
                gameLogo = Resize(gameLogo, MAIN_BUTTON_SIZE, MAIN_BUTTON_SIZE);
                btn.BackgroundImage = gameLogo;
            }

            this.FormClosed += (object sender, FormClosedEventArgs e) => { Environment.Exit(Environment.ExitCode); };

            Thread th = new Thread(() =>
            {
                Random random = new Random();
                while (true)
                {
                    if (random.Next(0, 100000) == 66699)
                    {
                        DialogResult response = MessageBox.Show("You have just won a free iphone 5, fresh from india. Would you like to get it now?", "Congratulations!", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);
                        switch (response)
                        {
                            default:
                                MessageBox.Show("A free Iphone 5 has just been sent to you by mail.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                break;
                            case DialogResult.No:
                                response = MessageBox.Show("What do you mean no?? Fuck you!! free punjabi phone!", "WHAT", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Warning);
                                if (response == DialogResult.Ignore)
                                {
                                    this.Invoke(new goToForm((Ohno form) =>
                                    {
                                        form.StartPosition = FormStartPosition.Manual;
                                        form.Location = new Point(this.Location.X, 0);
                                        this.Hide();
                                        Controls.Clear();
                                        form.Show();
                                    }), new Ohno());
                                }
                                break;
                        }
                    }
                    Thread.Sleep(1000);
                }
            });
            th.Start();
        }
        public void GoToGame(object sender, EventArgs args)
        {
            Button btn = (Button)sender;
            switch (btn.Tag.ToString())
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
        private void ChangeMainLabelText(object sender, EventArgs e, bool onSenderEntry = true)
        {
            if (!onSenderEntry)
            {
                titleLabel.Text = "Minigames";
                return;
            }
            Control ctrl = sender as Control;
            titleLabel.Text = ctrl.Tag.ToString();
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
        public static Image Resize(Image image, int w, int h)
        {
            Bitmap bmp = new Bitmap(w, h);
            Graphics grp = Graphics.FromImage(bmp);
            grp.DrawImage(image, 0, 0, w, h);
            grp.Dispose();

            return bmp;
        }
        private void Minigames_Load(object sender, EventArgs e)
        {

        }
    }
}
