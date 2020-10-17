using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Desktop_Minigames
{
    public class TransparentPictureBox : Control
    {
        private Timer refresher;
        private Image _image = null;
        public TransparentPictureBox()
        {
            refresher = new Timer();
            refresher.Tick += new EventHandler(this.TimerOnTick);
            refresher.Interval = 50;
            refresher.Start();
        }
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x20;
                return cp;
            }
        }

        protected override void OnMove(EventArgs e)
        {
            base.RecreateHandle();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (_image != null)
                e.Graphics.DrawImage(_image, (Width / 2) - (_image.Width / 2), (Height / 2) - (_image.Height / 2));
        }
        protected override void OnPaintBackground(PaintEventArgs e)
        {
        }
        private void TimerOnTick(object source, EventArgs e)
        {
            base.RecreateHandle();
            refresher.Stop();
        }
        public Image Image
        {
            get
            {
                return _image;
            }
            set
            {
                _image = value;
                base.RecreateHandle();
            }
        }


        private Timer timer1;
        private IContainer components;
    }

    public partial class Bullseye : Form
    {
        private PictureBox selectedSlot = new PictureBox();
        private TransparentPictureBox selectedGlow = new TransparentPictureBox();
        private PictureBox[] slots = new PictureBox[4];
        private int MovesCounter;
        private int[] MainNums = new int[4];
        private Random rnd = new Random();
        private Label movesCounterL = new Label();
        private int movesCounter = 0;
        private Image[] numImage = new Image[10];
        private List<Control> memos = new List<Control>();
        Button startB = new Button();
        RadioButton[] rdoB = new RadioButton[4];
        private int Difficulty;
        private int maxT;
        public Bullseye()
        {

            InitializeComponent();
            startMenu();
            
           
        }

        public void BuildGame(object sender, EventArgs e)
        {
            Controls.Remove(startB);
            for (int i = 0; i < 10; i++)
            {
                
                numImage[i] = Properties.Resources.ResourceManager.GetObject($"Bull{i}") as Image;
                numImage[i] = Resize(numImage[i], 50, 50);
            }
            for (int i = 0; i < 4; i++)
            {
                if (rdoB[i].Checked) { Difficulty = i; }
                Controls.Remove(rdoB[i]);
                MainNums[i] = rnd.Next(10);
                for (int j = 0; j < i; j++)
                {
                    if (MainNums[i] == MainNums[j]) { i = i - 1; }
                }
            }
            switch (Difficulty)
            {
                default:
                    maxT = 999;
                    break;
                case 0:
                    maxT = 16;
                    break;
                case 1:
                    maxT = 12;
                    break;
                case 2:
                    maxT = 8;
                    break;
            }
            Console.WriteLine(MainNums[0].ToString() + MainNums[1].ToString() + MainNums[2].ToString() + MainNums[3].ToString());
            for (int i = 0; i < 4; i++)
            {
                slots[i] = new PictureBox();
                slots[i].Location = new Point(10 + 50 * i, 400);
                Image img = Properties.Resources.Bull_empty as Image;
                img = Resize(img, 50, 50);
                slots[i].Size = img.Size;
                slots[i].Image = img;
                Controls.Add(slots[i]);
            }

            movesCounterL.Location = new Point(220, 410);
            movesCounterL.Width = 50;
            movesCounterL.Height = 50;
            movesCounterL.Text = $"moves played: {movesCounter}/{maxT}";
            Controls.Add(movesCounterL);

            Image img1 = Properties.Resources.Bull_glow;
            img1 = Resize(img1, 50, 50);
            selectedGlow.Size = img1.Size;
            selectedGlow.Image = img1;
            selectedGlow.Tag = 0;
            selectedGlow.Location = slots[0].Location;
            
            for (int i = 0; i < this.Controls.Count; i++)
            {
                if(  Controls[i] is Label || Controls[i] is PictureBox)
                    this.Controls[i].BackColor = Color.Transparent;
            }
            Controls.Add(selectedGlow);
            selectedGlow.BringToFront();
        }

        private void startMenu()
        {
            startB.Location = new Point(95, 150);
            startB.Width = 100;
            startB.Height = 50;
            startB.Text = $"Start Game";
            startB.Click += new EventHandler(BuildGame);
            Controls.Add(startB);
            for (int i = 0; i < 4; i++)
            {
                rdoB[i] = new RadioButton();
                rdoB[i].Width = 70;
                rdoB[i].Height = 20;
            }
            rdoB[0].Text = $"Easy";
            rdoB[1].Text = $"Normal";
            rdoB[1].Checked = true;
            rdoB[2].Text = $"Hard";
            rdoB[3].Text = $"Unlimited";
            for (int i = 0; i < 4; i++)
            {
                Controls.Add(rdoB[i]);
                rdoB[i].Location = new Point(120, 210 + i * 20);
            }
            for (int i = 0; i < this.Controls.Count; i++)
            {
                this.Controls[i].BackColor = Color.Transparent;
            }
        }

        public void NextMove()
        {
            //Move the Slots above and clear them
            isGameLost();
            Control newControl = new HistoryCheck((int)slots[0].Tag, (int)slots[1].Tag, (int)slots[2].Tag, (int)slots[3].Tag, GetBullseyes());
            newControl.Width = 300;
            newControl.Height = 50;
            memos.Add(newControl);
            newControl.Location = new Point(0, 340);
            Controls.Add(newControl);

            for (int i = memos.Count() - 1; i > 0; i--)
            {
                if (i == 1)
                {
                    memos[i - 1].Location = new Point(memos[i - 1].Location.X, memos[i - 1].Location.Y - 50);
                }
                else
                {
                    memos[i - 1].Location = memos[i - 2].Location;
                }

            }

            //Reset the slots
            for (int i = 0; i < 4; i++)
            {

                slots[i].Tag = null;
                slots[i].Location = new Point(10 + 50 * i, 400);
                slots[i].Size = new Size(50,50);
                slots[i].Image = Resize(Properties.Resources.Bull_empty as Image,50,50);
                Controls.Add(slots[i]);
            }
            //Add glow
            selectedGlow.Location = slots[0].Location;
            selectedGlow.Tag = 0;
            movesCounter++;
            //
            movesCounterL.Text = $"Moves Played:\n {movesCounter}/{maxT}";
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Enter)
            {
                bool Bcheck = true;
                for (int j = 0; j < 4; j++)
                {
                    if (slots[j].Tag == null)
                    {
                        Bcheck = false;
                    }
                }
                if (Bcheck)
                {
                    if (GetBulls() == 4) { GameWon(); }
                    else { NextMove(); }
                }
            }
            if (keyData == Keys.Right)
            {
                //Move Right
                switch ((int)selectedGlow.Tag)
                {
                    case 0:
                        selectedGlow.Tag = 1;
                        selectedGlow.Location = slots[1].Location;
                        break;
                    case 1:
                        selectedGlow.Tag = 2;
                        selectedGlow.Location = slots[2].Location;
                        break;
                    case 2:
                        selectedGlow.Tag = 3;
                        selectedGlow.Location = slots[3].Location;
                        break;
                    case 3:
                        selectedGlow.Tag = 0;
                        selectedGlow.Location = slots[0].Location;
                        break;
                }
            }
            if (keyData == Keys.Left)
            {
                //Move Left
                switch ((int)selectedGlow.Tag)
                {
                    case 0:
                        selectedGlow.Tag = 3;
                        selectedGlow.Location = slots[3].Location;
                        break;
                    case 1:
                        selectedGlow.Tag = 0;
                        selectedGlow.Location = slots[0].Location;
                        break;
                    case 2:
                        selectedGlow.Tag = 1;
                        selectedGlow.Location = slots[1].Location;
                        break;
                    case 3:
                        selectedGlow.Tag = 2;
                        selectedGlow.Location = slots[2].Location;
                        break;
                }
            }
            for (int i = 0; i < 10; i++)
            {
                if (keyData == Keys.D0 + i || keyData == Keys.NumPad0 + i)
                {
                    bool Bcheck = true;
                    bool Bcheck1 = true;
                    int BcheckTempnum = 0;
                    int BcheckTempnum1 = 0;
                    for (int j = 0; j < 4; j++)
                    {
                        if (slots[j].Tag != null && (int)slots[j].Tag == i)
                        {
                            Bcheck = false;
                            BcheckTempnum = j;
                            if (slots[(int)selectedGlow.Tag].Tag != null)
                            {
                                BcheckTempnum1 = (int)slots[(int)selectedGlow.Tag].Tag;
                            }
                            else
                            {
                                Bcheck1 = false;
                            }
                        }
                    }
                    if (Bcheck)
                    {
                        slots[(int)selectedGlow.Tag].Image = numImage[i];
                        slots[(int)selectedGlow.Tag].Tag = i;
                        if ((int)selectedGlow.Tag != 3)
                        {
                            selectedGlow.Location = slots[(int)selectedGlow.Tag + 1].Location;
                            selectedGlow.Tag = (int)selectedGlow.Tag + 1;
                        }
                        else
                        {
                            selectedGlow.Location = slots[0].Location;
                            selectedGlow.Tag = 0;
                        }
                    }
                    else if (Bcheck1)
                    {
                        slots[BcheckTempnum].Image = numImage[BcheckTempnum1];
                        slots[BcheckTempnum].Tag = BcheckTempnum1;
                        slots[(int)selectedGlow.Tag].Image = numImage[i];
                        slots[(int)selectedGlow.Tag].Tag = i;
                        if ((int)selectedGlow.Tag != 3)
                        {
                            selectedGlow.Location = slots[(int)selectedGlow.Tag + 1].Location;
                            selectedGlow.Tag = (int)selectedGlow.Tag + 1;
                        }
                        else
                        {
                            selectedGlow.Location = slots[0].Location;
                            selectedGlow.Tag = 0;
                        }
                    }
                    selectedGlow.BringToFront();
                }
            }
            return false;
        }
        public Image Resize(Image image, int w, int h)
        {
            Bitmap bmp = new Bitmap(w, h);
            Graphics grp = Graphics.FromImage(bmp);
            grp.DrawImage(image, 0, 0, w, h);
            grp.Dispose();
            return bmp;
        }
        public int GetBulls()
        {
            int Bulls = 0;
            for (int i = 0; i < 4; i++)
            {
                if ((int)slots[i].Tag == MainNums[i])
                {
                    Bulls++;
                }
            }
            return Bulls;
        }
        public int GetSeyes()
        {
            int Seyes = 0;
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if ((int)slots[i].Tag == MainNums[j])
                    {
                        Seyes++;
                    }
                }
            }
            return Seyes;
        }
        public int[] GetBullseyes()
        {
            int[] bullseyes = new int[4];
            int bulls = GetBulls();
            int seyes = GetSeyes() - GetBulls();
            for (int i = 0; i < 4; i++)
            {
                if (i < bulls)
                {
                    bullseyes[i] = 2;
                }
                else if (i < seyes + bulls)
                {
                    bullseyes[i] = 1;
                }
                else
                {
                    bullseyes[i] = 0;
                }
            }
            return bullseyes;
        }
        public void GameWon()
        {
            DialogResult response = MessageBox.Show($"Well played! you win the game\n you played {movesCounter + 1} moves\n Would you like to play again?", "Congrats!", MessageBoxButtons.YesNo);
            switch (response)
            {
                default:
                    Controls.Clear();
                    startMenu();
                    break;
                case DialogResult.No:
                    Minigames form = new Minigames();
                    form.StartPosition = FormStartPosition.Manual;
                    form.Location = new Point(this.Location.X, 0);
                    form.FormClosed += (object sender, FormClosedEventArgs e) => { Environment.Exit(Environment.ExitCode); };
                    form.Show();
                    this.Hide();
                    break;

            }
        }
        //private void timer1_Tick(object sender, EventArgs e)
        //{
        //    movesCounterL.Text = $"Moves Played:\n {movesCounter}/{maxT}";
        //}
        public void isGameLost()
        {
            int maxT = 0;
            switch (Difficulty)
            {
                default:
                    maxT = 999;
                    break;
                case 0:
                    maxT = 16;
                    break;
                case 1:
                    maxT = 12;
                    break;
                case 2:
                    maxT = 8;
                    break;
            }
            if (memos.Count == maxT)
            {
                DialogResult response = MessageBox.Show($"Oh no! you lost the game\n you run out of moves\n Would you like to play again?", "Saddos Maximus", MessageBoxButtons.YesNo);
                switch (response)
                {
                    default:
                        Application.Restart();
                        break;
                    case DialogResult.No:
                        Environment.Exit(Environment.ExitCode);
                        break;

                }
            }
        }
    }
}
