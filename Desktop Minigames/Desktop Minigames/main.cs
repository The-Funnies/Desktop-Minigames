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
    public class Saver
    {
        public static Minigames Minigames { get; set; }

    }
    public partial class Minigames : Form
    {
        private Button[] games;
        private List<String> gamesNames;
        public static Random random = new Random();
        private Image titleimg = Properties.Resources.minigames_title;
        PictureBox title = new PictureBox();
        private static bool isChatShown = false;
        private Image background_img = GenerateBackground();
        private const int MAIN_BUTTON_SIZE = 100;
        private const int BACKGROUND_PICS_AMOUNT = 35;//The last index of background pics in Properties.Resources
        private int resizeCount = 0;

        protected override void OnResize(EventArgs e)
        {
            if (resizeCount++ > 1)
                ShowLayout(false);
            
        }

        public Minigames()
        {
            title = new PictureBox();
            title.Click += (sender, e) =>
             {
                 GoToGame(games[random.Next(0, games.Length)], e);
             };
            Saver.Minigames = this;
            Shown += ShowChat;
            Width = (int)(Screen.PrimaryScreen.WorkingArea.Size.Width / 2.7);
            Height = (int)(Screen.PrimaryScreen.WorkingArea.Size.Height / 1.25);
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
            ShowLayout(true);

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
           // th.Start();
        }

        private void ShowLayout(bool initialization)
        {
            SetBackground(this, background_img);

            int imgsize = Properties.Resources.minigames_title.Width;
            int i = 0;
            double maxsize = 1.7;
            double multiplier = 0;
            while ((imgsize / (maxsize + i++ * 0.1)) > Width) ;
            multiplier = (maxsize + (i - 1) * 0.1);
            titleimg = Resize(titleimg, (int)(imgsize / multiplier), (int)(Properties.Resources.minigames_title.Height / multiplier));

            title.Image = titleimg;
            title.BackColor = Color.Transparent;
            title.Size = new Size(titleimg.Width, titleimg.Height);
            title.Location = new Point(Width / 2 - title.Size.Width / 2, 0);
            Controls.Add(title);

            for (i = 0; i < games.Length; i++)
            {
                if (initialization)
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
                else
                {
                    games[i].Location = new Point(MAIN_BUTTON_SIZE / 10 + i % 2 * (MAIN_BUTTON_SIZE + MAIN_BUTTON_SIZE / 10), title.Location.Y + title.Height + i / 2 * (MAIN_BUTTON_SIZE + MAIN_BUTTON_SIZE / 10));
                }

            }
            if (initialization)
            {
                foreach (Button btn in games)
                {
                    String logoResourceName = btn.Tag.ToString().Replace(" ", "_");
                    Image gameLogo = Properties.Resources.ResourceManager.GetObject(logoResourceName) as Image;
                    if (gameLogo == null) continue;
                    btn.BackgroundImage = gameLogo;
                    btn.BackgroundImageLayout = ImageLayout.Stretch;
                }
            }
        }

        public void GoToGame(object sender, EventArgs args)
        {
            Button btn = (Button)sender;
            switch (btn.Tag.ToString())
            {
                case "Snake":
                    GoToForm(new Snake());
                    break;
                case "Solitaire":
                    GoToForm(new Solitaire());
                    break;
                case "Flappy Bird":
                    GoToForm(new FlappyBird());
                    break;
                case "Whist":
                    // GoToForm<WhistMain>(new WhistMain());
                    break;
                case "Ultimate Tic Tac Toe":

                    GoToForm(new UltimateTicTacToe());
                    break;
                case "Ultimate Ultimate Tic Tac Toe":
                    GoToForm(new UltimateUltimateTicTacToe());
                    break;
                case "Bullseye":
                    GoToForm(new Bullseye());
                    break;
                case "Damka":
                    GoToForm(new Damka.Damka());
                    break;
                case "Pong":
                    GoToForm(new Pong.HostOrConnect());
                    break;
                case "Tic Tac Toe":
                    GoToForm(new TicTacToe.FirstForm());
                    break;
            }
        }
        private void ChangeMainLabelText(object sender, EventArgs e, bool onSenderEntry = true, Control displayOn = null)
        {

            Control ctrl = sender as Control;
            if (displayOn == null) displayOn = ctrl;
            if (onSenderEntry&&ctrl.Size.Width==MAIN_BUTTON_SIZE)
            {
               // ctrl.Hide();
                ctrl.Size = new Size((int)(MAIN_BUTTON_SIZE * 1.5), (int)(MAIN_BUTTON_SIZE * 1.5));
               // ctrl.Show();
                ctrl.BringToFront();
                displayOn.Text = ctrl.Tag.ToString();
                return;
            }
            ctrl.Hide();
            displayOn.Text = "";
            ctrl.Size = new Size(MAIN_BUTTON_SIZE ,MAIN_BUTTON_SIZE);
            ctrl.SendToBack();
            ctrl.Show();
        }
        public static List<String> noBg = new List<String>
        {
            "Solitaire"
            ,"Damka",
            "FlappyBird"
        };
        public static List<String> noBackButton = new List<String>
        {
            "FlappyBird"
        };
        public static List<String> noFullWindow = new List<String>
        {
            "FlappyBird"
        };

        public void GoToForm<T>(T form) where T : Form
        {
            form.StartPosition = FormStartPosition.Manual;
            form.Location = new Point(this.Location.X, 0);
            form.FormClosed += (object sender, FormClosedEventArgs e) => { Environment.Exit(Environment.ExitCode); };

            form.Text = form.GetType().Name;
            if (!noBg.Contains(form.Text))
            {
                SetBackground(form);
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
            if (!noFullWindow.Contains(form.Text))
            {
                form.WindowState = FormWindowState.Maximized;
            }
            
            if (!noBackButton.Contains(form.Text) & !form.Text.Equals("Minigames"))
            {
                Button backBtn = new Button();
                backBtn.Location = new Point(0, form.Height - 100);
                backBtn.Size = new Size(50, 50);
                backBtn.Click += (sender, e) =>
                {
                    GoToForm(new Minigames());
                    form.Hide();

                };

                form.Controls.Add(backBtn);
            }


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
        public static void SetBackground(Form form, Image bg)
        {
            form.BackColor = Color.White;
            form.BackgroundImage = bg;
            form.BackgroundImageLayout = ImageLayout.Center;
        }
        public static Image GenerateBackground()
        {
            return Properties.Resources.ResourceManager.GetObject("_" + random.Next(0, BACKGROUND_PICS_AMOUNT)) as Image;
        }
        private void ShowChat(object sender, EventArgs e)
        {
            if (isChatShown) return;
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
                MessageBox.Show("Failed to connect to the chat server.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                form = null;
                return;
            }
            else
            {
                if (form == null) return;
                form.StartPosition = FormStartPosition.Manual;
                form.Location = new Point(this.Location.X, 0);
                form.Show();
                isChatShown = true;
            }
        }
    }
}
