using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Threading;

namespace TicTacToe
{
    public partial class MultiplayerGame : Form
    {
        public TcpClient client = new TcpClient();
        public NetworkStream stream;
        private Board board;
        private bool isO;
        private bool canPlay;
        public MultiplayerGame()
        {
            InitializeComponent();
            this.board = new Board(this);
            this.Controls.Add(this.board);

        }

        public void SetLocalhost()
        {
            try
            {
                this.client = new TcpClient("127.0.0.1", ServerGUI.PORT);
                this.stream = this.client.GetStream();
            }
            catch (Exception)
            {
                SetLocalhost();
            }
        }
        private void MultiplayerGame_Load(object sender, EventArgs e)
        {
            FirstRecived();
        }

        private void FirstRecived()
        {
            Thread reciveWhoStarts = new Thread(() =>
            {
                this.isO = ReciveFromServer() == 11 ? true : false;
                if (this.isO)
                {
                    SetInfoLabelText("Your turn");
                }
                else
                {
                    SetInfoLabelText("Opponents turn");
                    this.board.DisableAll();
                    int index = ReciveFromServer();
                    this.board.EnableAll();
                    ChancgeCellValue(board.GetCellByIndex(index), 2);
                    SetInfoLabelText("Your turn");
                }
                this.canPlay = true;
            });
            reciveWhoStarts.Start();
        }

        private int ReciveFromServer()
        {
            try
            {
                byte[] buffer = new byte[2];
                int length = this.stream.Read(buffer, 0, 2);
                return buffer[0];
            }
            catch (Exception)
            {
                CloseAll();
                return 0;
            }
            
        }

        private void MultiplayerGame_FormClosing(object sender, FormClosingEventArgs e)
        {
            CloseAll();
        }

        private void SetInfoLabelText(string text)
        {
            if (infoLabel.InvokeRequired)
            {
                infoLabel.Invoke((MethodInvoker)delegate ()
                {
                    infoLabel.Text = text;
                });
            }
            else
            {
                infoLabel.Text = text;
            }
        }
        private void CloseAll()
        {
            if (!(this.client == null)) this.client.Close();
            Environment.Exit(Environment.ExitCode);
        }

        private void Connection_Click(object sender, EventArgs e)
        {
            MessageBox.Show(IsClientConnected().ToString());
        }

        public void Button_Clicked(object sender, EventArgs e)
        {
            if (!canPlay) return;
            if (!IsClientConnected())
            {
                MessageBox.Show("Lost connection");
                CloseAll();
            }
            Cell clickedCell = sender as Cell;
            if (clickedCell.GetValue() != 0) return;
            this.board.DisableAll();
            SetInfoLabelText("Opponents turn");
            clickedCell.SetTextByValue(isO ? 2 : 1);
            int whoWins = Board.IsThrereAWinner(board.GetValues());
            if (whoWins == 2)
            {
                byte[] buffer = new byte[2] { 100, (byte)clickedCell.GetIndex() };
                this.stream.Write(buffer, 0, 2);
                MessageBox.Show("You Won!");                
                Reset();
                return;
            }
            else if (whoWins == 1)
            {
                byte[] buffer = new byte[2] { 99, (byte)clickedCell.GetIndex() };
                this.stream.Write(buffer, 0, 2);
                MessageBox.Show("Tie");               
                Reset();
                return;
            }
            byte[] bufferIndex = new byte[2] { (byte)clickedCell.GetIndex(), 0 };

            this.stream.Write(bufferIndex, 0, 2);

            byte[] recivedBuffer = new byte[2];
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += (s, a) =>
            {
                try
                {
                    this.stream.Read(recivedBuffer, 0, 2);
                }
                catch (Exception)
                {
                    recivedBuffer[0] = 202;
                }
            };
            worker.RunWorkerCompleted += (s, a) =>
            {
                if (recivedBuffer[0] == 202)
                {
                    MessageBox.Show("Opponenet disconnected, you won!");
                    CloseAll();
                }
                if (recivedBuffer[0] == 100)
                {
                    board.GetCellByIndex(recivedBuffer[1]).SetTextByValue(isO ? 1 : 2);
                    MessageBox.Show("You Lost :(");
                    Reset();
                    return;
                }
                else if (recivedBuffer[0] == 99)
                {
                    board.GetCellByIndex(recivedBuffer[1]).SetTextByValue(isO ? 1 : 2);
                    MessageBox.Show("Tie");
                    Reset();
                    return;
                }
                else
                {
                    board.GetCellByIndex(recivedBuffer[0]).SetTextByValue(isO ? 1 : 2);
                }
                SetInfoLabelText("Your turn");
                this.board.EnableAll();
            };
            worker.RunWorkerAsync();
        }

        private void Reset()
        {
            SetInfoLabelText("");
            this.canPlay = false;
            this.board.Reset();
            FirstRecived();
        }

        private void ChancgeCellValue(Cell cell, int value)
        {
            cell.Invoke((MethodInvoker)delegate ()
            {
                cell.SetTextByValue(value);
            });
        }

        private bool IsClientConnected()
        {
            return !((this.client.Client.Poll(1, SelectMode.SelectRead) && (this.client.Client.Available == 0)) || !this.client.Client.Connected);
        }
    }
}
