using flappy_bird;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
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
        private List<String> gamesNames;
        public static Random random = new Random();
        private PictureBox title;
        private const int MAIN_BUTTON_SIZE = 125;
        private const int BACKGROUND_PICS_AMOUNT = 35;//The last index of background pics in Properties.Resources
        private int resizeCount = 0;
        public Minigames()
        {
            
            Image titleimg = Properties.Resources.minigames_title;
            titleimg = Resize(titleimg, 487, 110);
            title = new PictureBox();
            title.Image = titleimg;
            Shown += (object sender, EventArgs e) =>
            {
                ChatClient form = null;
                Thread t = new Thread(() =>
                {
                    try
                    {
                        form = new ChatClient();
                    }
                    catch { }

                });
                t.Start();

                int ms = 0;
                bool connected = false;
                while (ms < 1000)
                {
                    if (t.ThreadState == ThreadState.Stopped)
                    {
                        connected = true;
                        break;
                    }
                    Thread.Sleep(50);
                    ms += 50;
                }
                if (!connected)
                {
                    t.Abort();
                    MessageBox.Show("Failed to connect to server.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    form = null;
                    return;
                }
                else
                {
                    form.StartPosition = FormStartPosition.Manual;
                    form.Location = new Point(this.Location.X, 0);
                    form.Show();
                    form.WindowState = FormWindowState.Maximized;
                }
            };
            this.BackColor = Color.White;
            this.BackgroundImage = Minigames.GenerateBackground();
            this.BackgroundImageLayout = ImageLayout.Center;
            Width = (int)(Screen.PrimaryScreen.WorkingArea.Size.Width / 3.5);
            Height = (int)(Screen.PrimaryScreen.WorkingArea.Size.Height / 1.25);
            WindowState = FormWindowState.Normal;
            gamesNames = new List<string>
            {
                "Snake",
                "Solitaire",
                "Flappy Bird",
                "Ultimate Tic Tac Toe",
                "Ultimate Ultimate Tic Tac Toe",
                "Bullseye",
                "Damka",
                "Tic Tac Toe",
                "Pong"
            };

            games = new Button[gamesNames.Count];
            title.BackColor = Color.Transparent;
            title.Size = new Size(Width, Height / 7 );
            title.Location = new Point(Width / 2 -title.Size.Width/2, 0);
            Controls.Add(title);

            for (int i = 0; i < games.Length; i++)
            {
                games[i] = new Button
                {
                    Font = new Font("Ariel", 25),
                    Size = new Size(MAIN_BUTTON_SIZE, MAIN_BUTTON_SIZE),
                    Location = new Point(MAIN_BUTTON_SIZE / 10 + i % 2 * (MAIN_BUTTON_SIZE + MAIN_BUTTON_SIZE / 10), title.Location.Y + title.Height + i / 2 * (MAIN_BUTTON_SIZE + MAIN_BUTTON_SIZE / 10))
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
        private void
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
                title.Text = "Minigames";
                return;
            }
            Control ctrl = sender as Control;
            title.Text = ctrl.Tag.ToString();
        }
        public void GoToForm<T>(T form) where T : Form
        {
            form.StartPosition = FormStartPosition.Manual;
            form.Location = new Point(this.Location.X, 0);
            form.FormClosed += (object sender, FormClosedEventArgs e) => { Environment.Exit(Environment.ExitCode); };
            SetBackground(form);
            form.Text = form.GetType().Name;
            this.Hide();
            Controls.Clear();
            try
            {
                form.Show();
            }
            catch
            {
            }
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
        public static void SetBackground(Form form)
        {
            SetBackground(form, GenerateBackground());
        }
        public static void SetBackground(Form form,Image bg)
        {
            form.BackColor = Color.White;
            form.BackgroundImage = bg;
            form.BackgroundImageLayout = ImageLayout.Center;
        }
        public static Image GenerateBackground()
        {
            return Properties.Resources.ResourceManager.GetObject("_" + random.Next(0, BACKGROUND_PICS_AMOUNT)) as Image;
        }
    }
}
