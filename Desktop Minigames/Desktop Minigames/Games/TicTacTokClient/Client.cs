using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Desktop_Minigames
{
    delegate void deleg();
    public partial class Client : Form
    {
        private TcpClient client;
        private NetworkStream stream;

        private Button[][] brdBoard;

        private Label turn;
        private int enabled;
        public Client()
        {
            InitializeComponent();

            this.FormClosed += (sender, e) => { Environment.Exit(Environment.ExitCode); };

            Width = Screen.PrimaryScreen.Bounds.Width / 3;
            Height = 2 * Screen.PrimaryScreen.Bounds.Height / 3;
            this.MaximumSize = new Size(this.Width, this.Height);

            client = new TcpClient("213.57.202.58", 8842);
            stream = client.GetStream();

            SendString(Environment.UserName); //send the data array

            Thread thread = new Thread(PlaceName);
            thread.Start();

        }
        //put name on screen yes?
        void PlaceName()
        {
            string name = GetString(ReceiveString());

            turn = new Label();
            turn.Tag = name;
            turn.Font = new Font("Arial", 14);

            GetFirstTurn();
        }

        void GetFirstTurn()
        {
            string firstturn = ReceiveString();

            if (firstturn[0] == 'a')
            {
                if (Controls.Contains(turn))
                {
                    this.Invoke(new deleg(() =>
                    {
                        turn.Text = "It's your turn!";
                        turn.Size = TextRenderer.MeasureText(turn.Text, turn.Font);
                        turn.Location = new Point(Width / 2 - turn.Size.Width / 2, Height / 60);
                    }));
                }
                else
                {
                    turn.Text = "It's your turn!";
                    turn.Size = TextRenderer.MeasureText(turn.Text, turn.Font);
                    turn.Location = new Point(Width / 2 - turn.Size.Width / 2, Height / 60);

                    this.Invoke(new deleg(() =>
                    {
                        Controls.Add(turn);
                    }));
                }
                
                
                enabled = 10;

                for (int i = 0; i < 9; i++)
                {
                    foreach (Button btn in brdBoard[i])
                    {
                        if (btn.BackColor == Color.FromName("Control"))
                        {
                            this.Invoke(new deleg(() =>
                            {
                                btn.BackColor = Color.Gray;
                            }));
                        }
                    }
                }
            }
            else
            {
                Thread thread = new Thread(GetEntry);
                thread.Start();

                this.Invoke(new deleg(() =>
                {
                    this.turn.Text = "It's " + (string)this.turn.Tag + "'s turn";
                    this.turn.Size = TextRenderer.MeasureText(turn.Text, turn.Font);
                    this.turn.Location = new Point(Width / 2 - turn.Size.Width / 2, Height / 60);
                    Controls.Add(turn);
                }));
            }
            
        }
        void GetEntry()
        {

            string clicked = GetString(ReceiveString());
            int len = clicked.Length;

            int howmanyindeciesleft = 0;

            if (len != 1)
            {
                int entry = (int)clicked[0] - 48;

                int index = (int)clicked[1] - 48;

                this.Invoke(new deleg(() =>
                {
                    brdBoard[entry][index].BackColor = Color.Red;
                }));

                if (len == 4)
                {
                    enabled = 10;

                    if (clicked[3] == '1')
                    {
                        for (int j = 0; j < 9; j++)
                        {
                            for (int k = 0; k < 9; k++)
                            {
                                if (j == entry)
                                {
                                    this.Invoke(new deleg(() =>
                                    {
                                        brdBoard[j][k].BackColor = Color.Red;
                                    }));

                                    continue;
                                }
                                if (brdBoard[j][k].BackColor == Color.FromName("Control"))
                                {
                                    this.Invoke(new deleg(() =>
                                    {
                                        brdBoard[j][k].BackColor = Color.Gray;
                                    }));
                                    howmanyindeciesleft++;
                                }
                            }
                        }
                    }
                    else
                    {
                        for (int j = 0; j < 9; j++)
                        {
                            for (int k = 0; k < 9; k++)
                            {
                                if (brdBoard[j][k].BackColor == Color.FromName("Control"))
                                {
                                    this.Invoke(new deleg(() =>
                                    {
                                        brdBoard[j][k].BackColor = Color.Gray;
                                    }));
                                    howmanyindeciesleft++;
                                }
                            }
                        }
                    }
                }
                else
                {
                    enabled = index;

                    if (len == 3)
                    {
                        for (int j = 0; j < 9; j++)
                        {
                            this.Invoke(new deleg(() =>
                            {
                                brdBoard[entry][j].BackColor = Color.Red;
                            }));

                            if (brdBoard[index][j].BackColor == Color.FromName("Control"))
                            {
                                this.Invoke(new deleg(() =>
                                {
                                    brdBoard[index][j].BackColor = Color.Gray;
                                }));
                                howmanyindeciesleft++;
                            }
                        }
                    }
                    else
                    {
                        for (int j = 0; j < 9; j++)
                        {
                            if (brdBoard[index][j].BackColor == Color.FromName("Control"))
                            {
                                this.Invoke(new deleg(() =>
                                {
                                    brdBoard[index][j].BackColor = Color.Gray;
                                }));
                                howmanyindeciesleft++;
                            }
                        }
                    }
                    this.Invoke(new deleg(() =>
                    {
                        turn.Text = "It's your turn!";
                        turn.Size = TextRenderer.MeasureText(turn.Text, turn.Font);
                        turn.Location = new Point(Width / 2 - turn.Size.Width / 2, Height / 60);
                    }));
                }
            }
            else
            {
                for (int i = 0; i < 9; i++)
                {
                    foreach (Button btn in brdBoard[i])
                    {
                        btn.BackColor = Color.Red;
                    }
                }

                MessageBox.Show("You lose");

                stream.Write(new byte[] { 0 }, 0, 1);

                NewGame();
                return;
            }

            if (howmanyindeciesleft == 0)
            {
                MessageBox.Show("It's a tie!");

                stream.Write(new byte[] { 0 }, 0, 1);

                NewGame();
            }
        }
        void BtnClick(object sender, EventArgs args)
        {
            Button btn = (Button)sender;

            if (true)
            {
                if (enabled != 10)
                {
                    foreach (Button btn1 in brdBoard[enabled])
                    {
                        if (btn1.BackColor == Color.Gray)
                        {
                            btn1.BackColor = Color.FromName("Control");
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < 9; i++)
                    {
                        foreach (Button btn1 in brdBoard[i])
                        {
                            if (btn1.BackColor == Color.Gray)
                            {
                                btn1.BackColor = Color.FromName("Control");
                            }
                        }
                    }
                }

                btn.BackColor = Color.Blue;

                string tosend = (string)btn.Tag;
                SendString(tosend);

                string iswin = ReceiveString();
                if (iswin[0] != 'b')
                {
                    if (iswin[0] == 'a')
                    {
                        foreach (Button btn1 in brdBoard[(int)(tosend[0]) - 48])
                        {
                            btn1.BackColor = Color.Blue;
                        }
                    }
                    else if (iswin[0] == 'c')
                    {
                        for (int i = 0; i < 9; i++)
                        {
                            foreach (Button btn1 in brdBoard[i])
                            {
                                btn1.BackColor = Color.Blue;
                            }
                        }

                        MessageBox.Show("You win!");

                        stream.Write(new byte[] { 0 }, 0, 1);

                        NewGame();
                        return;
                    }
                    else if (iswin[0] == 'd')
                    {
                        MessageBox.Show("It's a tie!");

                        stream.Write(new byte[] { 0 }, 0, 1);

                        NewGame();
                        return;
                    }
                }

                Thread thread = new Thread(GetEntry);
                thread.Start();

                turn.Text = "It's " + (string)turn.Tag + "'s turn";
                turn.Size = TextRenderer.MeasureText(turn.Text, turn.Font);
                turn.Location = new Point(Width / 2 - turn.Size.Width / 2, Height / 60);

                
            }
        }
        void NewGame()
        {
            for (int i = 0; i < 9; i++)
            {
                foreach (Button btn in brdBoard[i])
                {
                    btn.BackColor = Color.FromName("Control");
                }
            }

            Thread thread = new Thread(GetFirstTurn);
            thread.Start();

        }
        public int ReceiveInt()
        {
            byte[] buffer = new byte[1];
            stream.Read(buffer, 0, 1);
            return (int)buffer[0];
        }
        public void SendInt(int num)
        {
            stream.Write(new byte[1] { (byte)num }, 0, 1);
        }
        void SendString(string str)
        {
            byte[] data = Encoding.UTF8.GetBytes(str);

            stream.Write(data, 0, data.Length);
        }
        string ReceiveString()
        {
            byte[] data = new byte[256];

            stream.Read(data, 0, data.Length);

            return Encoding.UTF8.GetString(data);
        }
        string GetString(string str)
        {
            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] == '\0')
                {
                    return str.Substring(0, i);
                }
            }
            return str;
        }
        private void Client_Load(object sender, EventArgs e)
        {
            int Width = Screen.PrimaryScreen.Bounds.Width / 3;
            int Height = 16 * Screen.PrimaryScreen.Bounds.Height / 27;
            brdBoard = new Button[9][];
            Button[] btnBoard;
            int floor;
            int margin;
            int x = Width / 23;
            int y = Height / 7;
            for (int i = 0; i < 9; i++)
            {
                if (i == 1 || i == 4 || i == 7)
                {
                    margin = (int)(Width / 3.368);
                }
                else if (i == 0 || i == 3 || i == 6)
                {
                    margin = 0;
                }
                else
                {
                    margin = (int)(Width / 1.684);
                }

                if (i < 3)
                {
                    floor = 0;
                }
                else
                {
                    if (i < 6)
                    {
                        floor = (int)(Width / 3.368);
                    }
                    else
                    {
                        floor = (int)(Width / 1.684);
                    }
                }


                btnBoard = new Button[9];
                for (int j = 0; j < 9; j++)
                {
                    btnBoard[j] = new Button();
                    btnBoard[j].Tag = i != 0 ? (i * 10 + j).ToString() : 0.ToString() + j.ToString();
                    btnBoard[j].Size = new Size(3 * Width / 32, 3 * Height / 32);
                    if (j < 3) btnBoard[j].Location = new Point(j * 3 * Width / 32 + margin + x, floor + y);
                    if (j < 6 && j >= 3) { btnBoard[j].Location = new Point((j - 3) * 3 * Width / 32 + margin + x, 3 * Width / 32 + floor + y); }
                    if (j < 9 && j >= 6) btnBoard[j].Location = new Point((j - 6) * 3 * Width / 32 + margin + x, 3 * Width / 16 + floor + y);
                    btnBoard[j].UseVisualStyleBackColor = true;
                    btnBoard[j].BackColor = Color.FromName("Control");
                    btnBoard[j].Click += BtnClick;
                    Controls.Add(btnBoard[j]);

                }
                brdBoard[i] = btnBoard;
            }

        }

    }
}
