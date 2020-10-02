using flappy_bird;
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
        //private Button[] games;
        private Button[] games;
        private List<String> gamesNames;
        public static Random random = new Random();
        private Label titleLabel;
        private const int MAIN_BUTTON_SIZE = 125;
        private const int BACKGROUND_PICS_AMOUNT = 35;//The last index of background pics in Properties.Resources
        public Minigames()
        {
            InitializeComponent();
            Width = (int)(Screen.PrimaryScreen.WorkingArea.Size.Width / 3.5);
            Height = (int)(Screen.PrimaryScreen.WorkingArea.Size.Height / 1.25);
            this.BackgroundImage = GenerateBackground();
            gamesNames = new List<string>();
            gamesNames.Add("Snake");
            gamesNames.Add("Solitaire");
            gamesNames.Add("Flappy Bird");
            gamesNames.Add("Ultimate Tic Tac Toe");
            gamesNames.Add("Ultimate Ultimate Tic Tac Toe");
            gamesNames.Add("Bullseye");
            gamesNames.Add("Damka");
            gamesNames.Add("Tic Tac Toe");
            gamesNames.Add("Pong");


            games = new Button[gamesNames.Count];
            titleLabel = new Label();
            titleLabel.Text = "Minigames";
            titleLabel.BackColor = Color.Transparent;
            titleLabel.Font = new Font("Ariel", 30);
            titleLabel.Size = new Size(Width, Height/9);
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
                games[i].Tag = gamesNames[i];
                
                Controls.Add(games[i]);
            }
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
                    GoToForm<Form1>(new Form1());
                    break;
                case "Whist":
                   // GoToForm<WhistMain>(new WhistMain());
                    break;
                case "Ultimate Tic Tac Toe":
                    GoToForm<UltimateTicTacToe>(new UltimateTicTacToe());
                    break;
                case "Ultimate Ultimate Tic Tac Toe":
                    GoToForm<UltimateUltimateTicTacToe>(new UltimateUltimateTicTacToe());
                    break;
                case "Bullseye":
                    GoToForm<Bullseye>(new Bullseye());
                    break;
                case "Damka":
                    GoToForm<Damka.Damka>(new Damka.Damka());
                    break;
                case "Pong":
                    GoToForm<Pong.HostOrConnect>(new Pong.HostOrConnect());
                    break;
                case "Tic Tac Toe":
                    GoToForm<TicTacToe.FirstForm>(new TicTacToe.FirstForm());
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
            form.BackgroundImage = GenerateBackground();
            form.Show();
            form.WindowState = FormWindowState.Maximized;

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
        public Image GenerateBackground()
        {
            return Properties.Resources.ResourceManager.GetObject("_" + random.Next(0, BACKGROUND_PICS_AMOUNT)) as Image;
        }
    }
}
