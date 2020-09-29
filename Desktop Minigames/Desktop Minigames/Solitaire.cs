using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Desktop_Minigames
{
    class Card
    {
        private int num;
        private String type;

        public Card(int num, string type)
        {
            this.num = num;
            this.type = type;
        }
        public int getNum() { return num; }
        public String getType() { return type; }
        public void setNum(int num) { this.num = num; }
        public void setType(String type) { this.type = type; }
        public Card(Card c)
        {
            num = c.num;
            type = c.type;
        }
        public override string ToString()
        {
            return $"|{type}||{num}|";
        }
    }
    class Deck
    {
        private Card[] cards = new Card[52];
        private string[] slots = new string[4];
        public Deck(Card[] cards)
        {
            this.cards = cards;
        }
        public Deck()
        {
            //Spade Heart Diamond and Club
            String t = null;
            int Counter = 1;
            for (int i = 1; i <= 4; i++)
            {
                if (i == 1) { t = "Spade"; }
                if (i == 2) { t = "Heart"; }
                if (i == 3) { t = "Diamond"; }
                if (i == 4) { t = "Club"; }
                slots[i - 1] = t;
                for (int n = 1; n <= 13; n++)
                {
                    Card card = new Card(n, t);
                    this.cards[Counter - 1] = card;
                    //Console.WriteLine(Counter);
                    Console.WriteLine(card.ToString());
                    Counter++;
                }
            }
        }
        public Deck(Deck d)
        {
            this.cards = d.cards;
        }
        public void setCard(Card c , int i)
        {
            cards[i] = c;
        }
        public override string ToString()
        {
            return cards.ToString();
        }
        public Card getCard(int i)
        {
            return cards[i];
        }
        public string getSlot(int i)
        {
            return slots[i];
        }
        public bool areFollowingNum(Card c1, Card c2)
        //are the Cards one after the other
        {
            if (c1.getNum() == c2.getNum() - 1)
            {
                return true;
            }
            return false;
        }
        public bool areDifferentcolors(Card c1, Card c2)
        //are the Cards diffrent colors
        {
            if ((c1.getType() == "Spade" || c1.getType() == "Club") && (c2.getType() == "Heart" || c2.getType() == "Diamond"))
            {
                return true;
            }
            else if ((c2.getType() == "Spade" || c2.getType() == "Club") && (c1.getType() == "Heart" || c1.getType() == "Diamond"))
            {
                return true;
            }
            return false;
        }
    }
    public partial class Solitaire : Form
    {
        private PictureBox[] slotsPB = new PictureBox[4];
        private Deck deck = new Deck();
        private PictureBox[] cards = new PictureBox[52];
        private List<PictureBox> dealer = new List<PictureBox>();
        private List<PictureBox>[] lines = new List<PictureBox>[7];
        private bool bruh_you_found_an_easter_egg;
        private PictureBox[] hidCards = new PictureBox[27];
        private Image hiddenCardImg = Image.FromFile("..\\..\\PNG\\yellow_back.png");
        private Image glowImg = Image.FromFile("..\\..\\PNG\\glowDark.png");
        private PictureBox glowingC = new PictureBox();
        private PictureBox glowPB = new PictureBox();
        private PictureBox openCard = new PictureBox();
        private Label dealerCounterLabel = new Label();
        private Label emptyDealerLabel = new Label();
        private PictureBox[] emptyCards = new PictureBox[7];
        private int slotSCount, slotHCount, slotDCount, slotCCount;
        private PictureBox slotSpb, slotHpb, slotDpb, slotCpb;
        private static Stopwatch playTime = new Stopwatch();
        private int movesCounter = 0;
        private Label movesCounterL = new Label();
        private string elapsedTime;
        private Random rnd = new Random();
        private Button toggleTheme = new Button();
        private int bruhCount = 0;
        private Button finish = new Button();
        public Solitaire()
        {
            InitializeComponent();
            buildSlotim();
            buildDeck();
        }
        public void buildDeck()
        {
            {
                dealerCounterLabel.Location = new Point(630, 30);
                dealerCounterLabel.Width = 25;
                dealerCounterLabel.Height = 15;
                dealerCounterLabel.Text = "0";
                Controls.Add(dealerCounterLabel);
                movesCounterL.Location = new Point(715, 75);
                movesCounterL.Width = 100;
                movesCounterL.Height = 20;
                movesCounterL.Text = $"moves: {movesCounter}";
                Controls.Add(movesCounterL);

                GoDark();
                toggleTheme.Text = "Go Light";
                toggleTheme.Location = new Point(700, 120);
                toggleTheme.Size = new Size(80, 40);
                toggleTheme.Click += ToggleTheme_Click;
                Controls.Add(toggleTheme);
            } //Add Counters
            for (int i = 0; i < 7; i++)
            {
                emptyCards[i] = new PictureBox();
                Image img = Image.FromFile("..\\..\\PNG\\emptyCard.png");
                img = Resize(img, 86, 132);
                emptyCards[i].Size = img.Size;
                emptyCards[i].Image = img;
                //cards[i].Tag = deck.getCard(i);
                emptyCards[i].Click += emptyCardClick;
                emptyCards[i].Location = new Point(470 + i * 130, 300);
                Controls.Add(emptyCards[i]);
            } //Add empty cards
            for (int i = 0; i < 51; i++)
            {
                Card tempC = deck.getCard(i);
                int tempNum = rnd.Next(i, 52);
                deck.setCard(deck.getCard(tempNum), i);
                deck.setCard(tempC, tempNum);
            } //Shuffle
            for (int i = 0; i < 7; i++)
            {
                lines[i] = new List<PictureBox>();
            } //Create lines
            for (int i = 0; i < 52; i++)
            {
                cards[i] = new PictureBox();
                Image img = Image.FromFile("..\\..\\PNG\\" + deck.getCard(i).getNum().ToString() + deck.getCard(i).getType()[0] + ".png");
                img = Resize(img, 86, 132);
                cards[i].Size = img.Size;
                cards[i].Image = img;
                cards[i].Tag = deck.getCard(i);
                cards[i].Click += cardClick;
                if (i > 27)
                {
                    hiddenCardImg = Resize(hiddenCardImg, 86, 132);
                    dealer.Add(new PictureBox());
                    dealer[i - 28].Location = new Point(470, 50);
                    dealer[i - 28].Size = hiddenCardImg.Size;
                    dealer[i - 28].Image = hiddenCardImg;
                    dealer[i - 28].Tag = cards[i];
                    dealer[i - 28].Click += dealerClick;
                    Controls.Add(dealer[i - 28]);
                }
                else if (i < 27 && i != 0 && i != 2 && i != 5 && i != 9 && i != 14 && i != 20)
                {
                    hidCards[i] = new PictureBox();
                    Image img2 = Image.FromFile("..\\..\\PNG\\yellow_back.png");
                    img2 = Resize(img2, 86, 132);
                    hidCards[i].Size = img2.Size;
                    hidCards[i].Image = img2;
                    hidCards[i].Tag = cards[i];

                } //Create hidden lines cards

            } //Create PictureBoxes
            for (int i = 0; i < 28; i++)
            {
                if (i == 0)
                {

                    lines[0].Add(cards[0]);
                    lines[0][0].Location = new Point(470, 300);
                    //                    Controls.Add(lines[0][0]);
                }
                if (i == 1 || i == 2)
                {
                    if (i != 2)
                    {
                        lines[1].Add(hidCards[i]);
                    }
                    else
                    {
                        lines[1].Add(cards[i]);
                    }
                    lines[1][i - 1].Location = new Point(600, 300 + (i - 1) * 20);
                    //                    Controls.Add(lines[1][i - 1]);
                }
                if (i >= 3 && i <= 5)
                {
                    if (i != 5)
                    {
                        lines[2].Add(hidCards[i]);
                    }
                    else
                    {
                        lines[2].Add(cards[i]);
                    }
                    lines[2][i - 3].Location = new Point(730, 300 + (i - 3) * 20);
                    //                    Controls.Add(lines[2][i - 3]);
                }
                if (i >= 6 && i <= 9)
                {
                    if (i != 9)
                    {
                        lines[3].Add(hidCards[i]);
                    }
                    else
                    {
                        lines[3].Add(cards[i]);
                    }
                    lines[3][i - 6].Location = new Point(860, 300 + (i - 6) * 20);
                    //                    Controls.Add(lines[3][i - 6]);
                }
                if (i >= 10 && i <= 14)
                {
                    if (i != 14)
                    {
                        lines[4].Add(hidCards[i]);
                    }
                    else
                    {
                        lines[4].Add(cards[i]);
                    }
                    lines[4][i - 10].Location = new Point(990, 300 + (i - 10) * 20);
                    //                    Controls.Add(lines[4][i - 10]);
                }
                if (i >= 15 && i <= 20)
                {
                    if (i != 20)
                    {
                        lines[5].Add(hidCards[i]);
                    }
                    else
                    {
                        lines[5].Add(cards[i]);
                    }
                    lines[5][i - 15].Location = new Point(1120, 300 + (i - 15) * 20);
                    //                    Controls.Add(lines[5][i - 15]);
                }
                if (i >= 21 && i <= 27)
                {
                    if (i != 27)
                    {
                        lines[6].Add(hidCards[i]);
                    }
                    else
                    {
                        lines[6].Add(cards[i]);
                    }
                    lines[6][i - 21].Location = new Point(1250, 300 + (i - 21) * 20);
                    //                    Controls.Add(lines[6][i - 21]);

                }
            } //Add the PictureBoxes to the list
            for (int i = 0; i < 7; i++)
            {
                for (int j = 0; j <= i; j++)
                {
                    Controls.Add(lines[i][j]);
                    lines[i][j].BringToFront();
                }
            } //add the lines to the form
        }
        public void buildSlotim()
        {
            for (int i = 0; i < 4; i++)
            {
                slotsPB[i] = new PictureBox();
                slotsPB[i].Tag = deck.getSlot(i);
                slotsPB[i].Location = new Point(860 + 130 * i, 52);
                Image img = Image.FromFile($"..\\..\\PNG\\slot{slotsPB[i].Tag}.png");
                img = Resize(img, 86, 132);
                slotsPB[i].Size = img.Size;
                slotsPB[i].Image = img;
                slotsPB[i].Click += slotClick;
                Controls.Add(slotsPB[i]);
            }
        }
        public void GoDark()
        {
            this.BackColor = Color.FromName("Black");
            movesCounterL.ForeColor = Color.FromName("White");
            glowImg = Image.FromFile("..\\..\\PNG\\glowDark.png");
            dealerCounterLabel.ForeColor = Color.FromName("White");
            emptyDealerLabel.ForeColor = Color.FromName("White");
            toggleTheme.ForeColor = Color.FromName("White");
            toggleTheme.BackColor = Color.FromArgb(30, 30, 30);
            finish.ForeColor = Color.FromName("White");
            finish.BackColor = Color.FromArgb(30, 30, 30);
            toggleTheme.Text = "Go Light";
        }
        public void GoLight()
        {
            this.BackColor = SystemColors.Control;
            movesCounterL.ForeColor = Color.FromName("Black");
            dealerCounterLabel.ForeColor = Color.FromName("Black");
            glowImg = Image.FromFile("..\\..\\PNG\\glow.png");
            emptyDealerLabel.ForeColor = Color.FromName("Black");
            toggleTheme.ForeColor = Color.FromName("Black");
            toggleTheme.BackColor = SystemColors.Control;
            finish.ForeColor = Color.FromName("Black");
            finish.BackColor = SystemColors.Control;
            toggleTheme.Text = "Go Dark";
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.F11)
            {
                if (FormBorderStyle == FormBorderStyle.Sizable)
                {
                    FormBorderStyle = FormBorderStyle.None;
                    WindowState = FormWindowState.Maximized;
                }
                else
                {
                    FormBorderStyle = FormBorderStyle.Sizable;
                    WindowState = FormWindowState.Normal;
                }
            }
            return false;
        }
        private void ToggleTheme_Click(object sender, EventArgs e)
        {
            if (this.BackColor == Color.FromName("Black"))
            {
                GoLight();
                return;
            }
            GoDark();
        }
        public void cardClick(object sender, EventArgs e)
        {
            bool isfirstT = true;
            if (isfirstT)
            {
                isfirstT = false;
                playTime.Start();
            }
            PictureBox selectedC = (PictureBox)sender;
            Console.WriteLine($"You clicked {(Card)selectedC.Tag}");
            bool isCardFromDealer = false;
            bool Replaceable = false;
            bool moveToSlotAble = false;
            for (int i = 0; i < 7; i++)
            {
                if (lines[i].Contains(selectedC))
                {
                    for (int j = 0; j < lines[i].Count; j++)
                    {
                        if (lines[i][j] == selectedC && j + 1 <= lines[i].Count - 1)
                        {
                            Replaceable = true;
                            break;
                        }
                    }
                }
            }
            for (int i = 0; i < 7; i++)
            {
                if (lines[i].Contains(glowingC))
                {
                    for (int j = 0; j < lines[i].Count; j++)
                    {
                        if (lines[i][j] == glowingC && j + 1 == lines[i].Count)
                        {
                            moveToSlotAble = true;
                            break;
                        }
                    }
                }
            }
            if (glowingC == null || glowingC.Tag == null)
            {
                glowImg = Resize(glowImg, 95, 145);
                glowPB.Location = new Point(selectedC.Location.X - 5, selectedC.Location.Y - 5);
                glowPB.Image = glowImg;
                glowPB.Size = glowImg.Size;
                Controls.Add(glowPB);
                glowingC = selectedC;
            }//Apply glow
            else
            {
                if (selectedC == glowingC)
                {
                    Controls.Remove(glowPB);
                    glowingC = null;
                    Console.WriteLine($"You removed the glow from {(Card)selectedC.Tag}");
                }//Remove glow on same card

                else if (deck.areDifferentcolors((Card)selectedC.Tag, (Card)glowingC.Tag) && (deck.areFollowingNum((Card)glowingC.Tag, (Card)selectedC.Tag)) && !Replaceable && !(selectedC.Location.X == 600 && selectedC.Location.Y == 50) && (!(selectedC.Equals(slotSpb) || selectedC.Equals(slotHpb) || selectedC.Equals(slotDpb) || selectedC.Equals(slotCpb))))
                {
                    Controls.Remove(glowPB);
                    for (int i = 0; i < dealer.Count; i++)
                    {
                        if (((PictureBox)dealer[i].Tag).Equals(glowingC))
                        {
                            glowingC.Location = new Point(selectedC.Location.X, selectedC.Location.Y + 20);
                            glowingC.BringToFront();
                            Console.WriteLine($"You moved {(Card)glowingC.Tag} from the dealer to under {(Card)selectedC.Tag}");
                            openCard = null;
                            isCardFromDealer = true;
                            for (int j = 0; j < 7; j++)
                            {
                                if (lines[j].Contains(selectedC))
                                {
                                    lines[j].Add((PictureBox)dealer[i].Tag);
                                }
                            }
                            Controls.Remove(dealer[i]);
                            dealer.Remove(dealer[i]);
                            break;
                        }
                    }//Move Card from dealer
                    if ((glowingC.Equals(slotSpb) || glowingC.Equals(slotHpb) || glowingC.Equals(slotDpb) || glowingC.Equals(slotCpb)) && !isCardFromDealer)
                    {
                        removeFromSlots(selectedC);
                    }
                    else if (!isCardFromDealer)
                    {
                        int j, k = 0;
                        PictureBox[] tempCs = new PictureBox[0];
                        for (int i = 0; i < 7; i++)
                        {
                            if (lines[i].Contains(glowingC))
                            {
                                for (j = 0; j < lines[i].Count; j++)
                                {
                                    if (lines[i][j] == glowingC)
                                    {
                                        tempCs = new PictureBox[lines[i].Count - j];
                                        for (k = 0; k < tempCs.Length; k++)
                                        {
                                            tempCs[k] = lines[i][j + k];
                                        }
                                    }
                                }
                                for (int u = 0; u < tempCs.Length; u++)
                                {
                                    tempCs[u].Location = new Point(selectedC.Location.X, selectedC.Location.Y + (u + 1) * 20);
                                    tempCs[u].BringToFront();
                                    lines[i].Remove(tempCs[u]);
                                }
                                if (lines[i].Count != 0)
                                {
                                    try
                                    {
                                        lines[i][lines[i].Count - 1].Image = ((PictureBox)lines[i][lines[i].Count - 1].Tag).Image;
                                        lines[i][lines[i].Count - 1].Click += cardClick;
                                        lines[i][lines[i].Count - 1].Tag = ((PictureBox)lines[i][lines[i].Count - 1].Tag).Tag;
                                        bruhCount++;
                                    }
                                    catch { }
                                }
                            }
                        }
                        for (int i = 0; i < 7; i++)
                        {
                            if (lines[i].Contains(selectedC))
                            {
                                for (int r = 0; r < tempCs.Length; r++)
                                {
                                    lines[i].Add(tempCs[r]);
                                }
                            }
                        }
                        k = 0;
                        Console.WriteLine($"You moved {(Card)glowingC.Tag} and {tempCs.Length - 1} other Cards to under {(Card)selectedC.Tag}");
                    } //Move Cards from line to line
                    movesCounter++;
                    glowingC = null;
                }
                else if (((Card)selectedC.Tag).getNum() == ((Card)glowingC.Tag).getNum() - 1 && (((Card)selectedC.Tag).getType() == ((Card)glowingC.Tag).getType()) && (selectedC.Equals(slotSpb) || selectedC.Equals(slotHpb) || selectedC.Equals(slotDpb) || selectedC.Equals(slotCpb)))
                {
                    Controls.Remove(glowPB);
                    for (int i = 0; i < dealer.Count; i++)
                    {
                        if (((PictureBox)dealer[i].Tag).Equals(glowingC))
                        {
                            glowingC.Location = new Point(selectedC.Location.X, selectedC.Location.Y);
                            glowingC.BringToFront();
                            Controls.Remove(dealer[i]);
                            dealer.Remove(dealer[i]);
                            openCard = null;
                            Console.WriteLine($"You moved {(Card)glowingC.Tag} to slot {((Card)selectedC.Tag).getType()}");
                            isCardFromDealer = true;
                            break;
                        }
                    }//Move from dealer to slot
                    if (!isCardFromDealer && moveToSlotAble)
                    {
                        glowingC.Location = new Point(selectedC.Location.X, selectedC.Location.Y);
                        glowingC.BringToFront();
                        for (int i = 0; i < 7; i++)
                        {
                            if (lines[i].Contains(glowingC))
                            {
                                lines[i].Remove(glowingC);
                                if (lines[i].Count != 0)
                                {
                                    try
                                    {
                                        lines[i][lines[i].Count - 1].Image = ((PictureBox)lines[i][lines[i].Count - 1].Tag).Image;
                                        lines[i][lines[i].Count - 1].Click += cardClick;
                                        lines[i][lines[i].Count - 1].Tag = ((PictureBox)lines[i][lines[i].Count - 1].Tag).Tag;
                                        bruhCount++;
                                    }
                                    catch { }
                                }
                            }
                        }
                        Console.WriteLine($"You moved {(Card)glowingC.Tag} to slot {((Card)selectedC.Tag).getType()}");
                    }//Move from lines to slot
                    {
                        if (((Card)selectedC.Tag).getType() == "Spade")
                        {
                            slotSCount++;
                            slotSpb = glowingC;
                        }
                        else if (((Card)selectedC.Tag).getType() == "Heart")
                        {
                            slotHCount++;
                            slotHpb = glowingC;
                        }
                        else if (((Card)selectedC.Tag).getType() == "Diamond")
                        {
                            slotDCount++;
                            slotDpb = glowingC;
                        }
                        else if (((Card)selectedC.Tag).getType() == "Club")
                        {
                            slotCCount++;
                            slotCpb = glowingC;
                        }
                    }
                    glowingC = null;
                    movesCounter++;
                }
                else if (!(selectedC.Equals(slotSpb) || selectedC.Equals(slotHpb) || selectedC.Equals(slotDpb) || selectedC.Equals(slotCpb)))
                {
                    glowImg = Resize(glowImg, 95, 145);
                    glowPB.Location = new Point(selectedC.Location.X - 5, selectedC.Location.Y - 5);
                    glowPB.Image = glowImg;
                    glowPB.Size = glowImg.Size;
                    Controls.Add(glowPB);
                    glowingC = selectedC;
                }
            }
            if (glowingC != null) { Console.WriteLine($"GlowingC is {(Card)glowingC.Tag}"); }
            checkWin();
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            movesCounterL.Text = $"moves: {movesCounter}";
        }
        public void finishClick(object sender, EventArgs e)
        {
            elapsedTime = String.Format("{0:00}:{1:00}", playTime.Elapsed.Minutes, playTime.Elapsed.Seconds);
            for (int i = 0; i < 52; i++)
            {
                foreach (Control PB in this.Controls)
                {
                    if (PB.Tag != null && ((PB.Tag).GetType() == typeof(Card)))
                    {
                        if (((Card)PB.Tag).getNum() != 13)
                        {
                            Controls.Remove(PB);
                            Thread.Sleep(100);
                        }
                        else if (((Card)PB.Tag).getType() == "Spade")
                        {
                            PB.Location = new Point(860, 52);
                        }
                        else if (((Card)PB.Tag).getType() == "Heart")
                        {
                            PB.Location = new Point(860 + 130, 52);
                        }
                        else if (((Card)PB.Tag).getType() == "Diamond")
                        {
                            PB.Location = new Point(860 + 130 * 2, 52);
                        }
                        else if (((Card)PB.Tag).getType() == "Club")
                        {
                            PB.Location = new Point(860 + 130 * 3, 52);
                        }
                    }
                }
            }
            MessageBox.Show($"You've won the game!\nYour time was {elapsedTime}\nYou played {movesCounter} moves ", "Congratulations", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Environment.Exit(Environment.ExitCode);
        }
        public void slotClick(object sender, EventArgs e)
        {
            PictureBox selectedSlot = (PictureBox)sender;
            bool isCardFromDealer = false;
            if (glowingC != null && glowingC.Tag != null && ((Card)glowingC.Tag).getNum() == 1 && ((Card)glowingC.Tag).getType() == (string)selectedSlot.Tag)
            {
                for (int i = 0; i < dealer.Count; i++)
                {
                    if (((PictureBox)dealer[i].Tag).Equals(glowingC))
                    {
                        Controls.Remove(dealer[i]);
                        dealer.Remove(dealer[i]);
                        glowingC.Location = new Point(selectedSlot.Location.X, selectedSlot.Location.Y);
                        glowingC.BringToFront();
                        Console.WriteLine($"You moved {(Card)glowingC.Tag} to slot {selectedSlot.Tag}");
                        isCardFromDealer = true;
                        Controls.Remove(glowPB);
                        openCard = null;
                        break;
                    }
                }//Move from dealer to slot
                if (!isCardFromDealer)
                {
                    Controls.Remove(glowPB);
                    Console.WriteLine($"You moved {(Card)glowingC.Tag} to slot {selectedSlot.Tag}");
                    for (int i = 0; i < 7; i++)
                    {
                        if (lines[i].Contains(glowingC))
                        {
                            glowingC.Location = new Point(selectedSlot.Location.X, selectedSlot.Location.Y);
                            glowingC.BringToFront();
                            lines[i].Remove(glowingC);
                            if (lines[i].Count != 0)
                            {
                                try
                                {
                                    lines[i][lines[i].Count - 1].Image = ((PictureBox)lines[i][lines[i].Count - 1].Tag).Image;
                                    lines[i][lines[i].Count - 1].Click += cardClick;
                                    lines[i][lines[i].Count - 1].Tag = ((PictureBox)lines[i][lines[i].Count - 1].Tag).Tag;
                                    bruhCount++;
                                }
                                catch { }
                            }
                        }
                    }
                    Controls.Remove(glowPB);
                }
                if ((string)selectedSlot.Tag == "Spade")
                {
                    slotSCount++;
                    slotSpb = glowingC;
                }
                else if ((string)selectedSlot.Tag == "Heart")
                {
                    slotHCount++;
                    slotHpb = glowingC;
                }
                else if ((string)selectedSlot.Tag == "Diamond")
                {
                    slotDCount++;
                    slotDpb = glowingC;
                }
                else if ((string)selectedSlot.Tag == "Club")
                {
                    slotCCount++;
                    slotCpb = glowingC;
                }
                glowingC = null;
                movesCounter++;
            }
        }
        private void dealerClick(object sender, EventArgs e)
        {
            //Counter
            if (dealer.Count <= int.Parse(dealerCounterLabel.Text))
            {
                dealerCounterLabel.Text = "1";
            }
            else
            {
                dealerCounterLabel.Text = "" + (int.Parse(dealerCounterLabel.Text) + 1);
            }
            //Dealer Click
            PictureBox openDealer = (PictureBox)sender;
            openCard = (PictureBox)openDealer.Tag;
            openCard.Location = new Point(600, 50);
            Controls.Add(openCard);
            Controls.Remove(openDealer);
            if (!dealer[0].Equals(openDealer))
            {
                for (int i = 0; i < dealer.Count; i++)
                {
                    if (dealer[i].Equals(openDealer))
                    {
                        Controls.Add(dealer[i - 1]);
                        if (((PictureBox)dealer[i - 1].Tag).Location.X == 600 && ((PictureBox)dealer[i - 1].Tag).Location.Y == 50)
                        {
                            Controls.Remove((PictureBox)dealer[i - 1].Tag);
                        }
                    }
                }
            }
            else
            {
                if (dealer.Count == 1)
                {
                    Controls.Add(dealer[0]);
                    //Controls.Remove((PictureBox)dealer[dealer.Count - 1].Tag);
                    emptyDealerLabel.Location = new Point(480, 75);
                    emptyDealerLabel.Height = 50;
                    emptyDealerLabel.Width = 120;
                    emptyDealerLabel.Text = "All of the cards \n are out of the \n dealer!";
                    Controls.Add(emptyDealerLabel);
                }
                else
                {
                    Controls.Add(dealer[dealer.Count - 1]);
                    Controls.Remove((PictureBox)dealer[dealer.Count - 1].Tag);
                }
            }
            //Remove Glow
            foreach (Control shem in Controls)
            {
                if (shem.Location.X == 595 && shem.Location.Y == 45 && glowingC != null)
                {
                    Controls.Remove(shem);
                    glowingC = null;
                    Controls.Remove(glowPB);
                }
            }
        }
        public void emptyCardClick(object sender, EventArgs e)
        {
            PictureBox selectedEmptyCard = (PictureBox)sender;
            bool isCardFromDealer = false;
            if (glowingC != null && glowingC.Tag != null && ((Card)glowingC.Tag).getNum() == 13)
            {
                PictureBox[] tempCs = new PictureBox[0];
                for (int i = 0; i < dealer.Count; i++)
                {
                    if (((PictureBox)dealer[i].Tag).Equals(glowingC))
                    {
                        dealer.Remove(dealer[i]);
                        glowingC.Location = new Point(selectedEmptyCard.Location.X, selectedEmptyCard.Location.Y);
                        glowingC.BringToFront();
                        Console.WriteLine($"You moved {(Card)glowingC.Tag} to an empty line in line {selectedEmptyCard.Tag}");
                        //Controls.Remove(dealer[i]);
                        Controls.Remove(glowPB);
                        openCard = null;
                        isCardFromDealer = true;
                        break;
                    }
                }//Move from dealer to slot
                if (!isCardFromDealer)
                {
                    int j, k = 0;
                    for (int i = 0; i < 7; i++)
                    {
                        if (lines[i].Contains(glowingC))
                        {
                            for (j = 0; j < lines[i].Count; j++)
                            {
                                if (lines[i][j] == glowingC)
                                {
                                    tempCs = new PictureBox[lines[i].Count - j];
                                    for (k = 0; k < tempCs.Length; k++)
                                    {
                                        tempCs[k] = lines[i][j + k];
                                    }
                                }
                            }
                            for (int u = 0; u < tempCs.Length; u++)
                            {
                                tempCs[u].Location = new Point(selectedEmptyCard.Location.X, selectedEmptyCard.Location.Y + (u) * 20);
                                tempCs[u].BringToFront();
                                lines[i].Remove(tempCs[u]);
                            }
                            if (lines[i].Count != 0)
                            {
                                try
                                {
                                    lines[i][lines[i].Count - 1].Image = ((PictureBox)lines[i][lines[i].Count - 1].Tag).Image;
                                    lines[i][lines[i].Count - 1].Click += cardClick;
                                    lines[i][lines[i].Count - 1].Tag = ((PictureBox)lines[i][lines[i].Count - 1].Tag).Tag;
                                    bruhCount++;
                                }
                                catch { }
                            }
                        }
                    }
                    k = 0;
                    Controls.Remove(glowPB);
                    Console.WriteLine($"You moved {(Card)glowingC.Tag} to an empty slot along with {tempCs.Length - 1} other cards");
                }
                for (int i = 0; i < 7; i++)
                {
                    if (selectedEmptyCard == emptyCards[i])
                    {
                        if (isCardFromDealer)
                        {
                            lines[i].Add(glowingC);
                        }
                        else
                        {
                            for (int r = 0; r < tempCs.Length; r++)
                            {
                                lines[i].Add(tempCs[r]);
                            }
                        }
                    }
                }
                glowingC = null;
                movesCounter++;
            }
        }
        public void checkWin()
        {
            if (dealer.Count == 0 && bruhCount == 21)
            {
                finish.Text = "Finish";
                finish.Font = new Font("Space Mono", 30);
                finish.Location = new Point(470, 200);
                finish.Size = new Size(870, 80);
                finish.Click += finishClick;
                Controls.Add(finish);
                Console.WriteLine("done");
            }
            if (slotSCount == 13 && slotHCount == 13 && slotDCount == 13 && slotCCount == 13)
            {
                elapsedTime = String.Format("{0:00}:{1:00}", playTime.Elapsed.Minutes, playTime.Elapsed.Seconds);
                MessageBox.Show($"You've won the game!\nYour time was {elapsedTime}\nYou played {movesCounter} moves ", "Congratulations", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Environment.Exit(Environment.ExitCode);
            }
        }
        public void removeFromSlots(PictureBox selectedC)
        {
            Console.WriteLine($"You moved {(Card)glowingC.Tag} from the slots to under {(Card)selectedC.Tag}");
            for (int j = 0; j < 7; j++)
            {
                if (lines[j].Contains(selectedC))
                {
                    lines[j].Add(glowingC);
                }
            }
            PictureBox tempPB = new PictureBox();
            if (((Card)glowingC.Tag).getNum() != 1)
            {
                Card prevC = new Card(((Card)glowingC.Tag).getNum() - 1, ((Card)glowingC.Tag).getType());
                foreach (Control PB in this.Controls)
                {
                    if (PB.Tag != null && ((PB.Tag).GetType() == typeof(Card)) && ((Card)PB.Tag).getType() == prevC.getType() && ((Card)PB.Tag).getNum() == prevC.getNum())
                    {
                        tempPB = (PictureBox)PB;
                        break;
                    }
                }
                if (((Card)glowingC.Tag).getType() == "Spade")
                {
                    slotSCount--;
                    slotSpb = tempPB;
                }
                else if (((Card)glowingC.Tag).getType() == "Heart")
                {
                    slotHCount--;
                    slotHpb = tempPB;
                }
                else if (((Card)glowingC.Tag).getType() == "Diamond")
                {
                    slotDCount--;
                    slotDpb = tempPB;
                }
                else if (((Card)glowingC.Tag).getType() == "Club")
                {
                    slotCCount--;
                    slotCpb = tempPB;
                }
            }
            else
            {
                if (((Card)selectedC.Tag).getType() == "Spade")
                {
                    slotSCount--;
                    slotSpb = null;
                }
                else if (((Card)selectedC.Tag).getType() == "Heart")
                {
                    slotHCount--;
                    slotHpb = null;
                }
                else if (((Card)selectedC.Tag).getType() == "Diamond")
                {
                    slotDCount--;
                    slotDpb = null;
                }
                else if (((Card)selectedC.Tag).getType() == "Club")
                {
                    slotCCount--;
                    slotCpb = null;
                }
            }
            glowingC.Location = new Point(selectedC.Location.X, selectedC.Location.Y + 20);
            glowingC.BringToFront();
            glowingC = null;
        }
        public Image Resize(Image image, int w, int h)
        {
            Bitmap bmp = new Bitmap(w, h);
            Graphics grp = Graphics.FromImage(bmp);
            grp.DrawImage(image, 0, 0, w, h);
            grp.Dispose();
            return bmp;
        }
    }
}
