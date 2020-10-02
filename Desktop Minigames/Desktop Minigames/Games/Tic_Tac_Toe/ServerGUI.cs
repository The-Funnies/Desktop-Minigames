using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TicTacToe
{
    public partial class ServerGUI : Form
    {
        public const int PORT = 5000;
        const string SERVER_IP = "0.0.0.0";

        private TcpClient clientO;
        private NetworkStream streamO;

        TcpClient clientX;
        private NetworkStream streamX;

        private Random rnd = new Random();

        public ServerGUI()
        {
            InitializeComponent();
            ipBox.Text = new WebClient().DownloadString("http://icanhazip.com");
        }

        private void ServerGUI_Load(object sender, EventArgs e)
        {

        }

        private void ServerGUI_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!(this.clientO == null)) this.clientO.Close();
            if (!(this.clientX == null)) this.clientX.Close();
            Environment.Exit(Environment.ExitCode);
        }

        private void ServerGUI_Shown(object sender, EventArgs e)
        {
            WaitForPlayers();

            Thread gameThread = new Thread(() =>
            {
                while (true)
                {
                    if (IsFinishedConnecting()) break;
                }
                Game();
            });
            gameThread.Start();
        }

        private void Game()
        {
            AddText(ConsoleTextBox, "Starting game...");
            DecideWhoStarts();
            SendWhoStarts();
            RunGame();
        }
        private void DecideWhoStarts()
        {
            int random = this.rnd.Next(1, 3);
            if (random == 2)
            {
                TcpClient tmpClient = this.clientO;
                NetworkStream tmpStream = this.streamO;
                this.clientO = this.clientX;
                this.clientX = tmpClient;
                this.streamO = this.streamX;
                this.streamX = tmpStream;
            }
        }

        private void SendWhoStarts()
        {
            byte[] buffer = new byte[2] { 11, 0 };
            this.streamO.Write(buffer, 0, 2);
            buffer[0] = 10;
            this.streamX.Write(buffer, 0, 2);
        }

        private void RunGame()
        {
            while (true)
            {
                byte[] bufferIndex = new byte[2];

                try
                {
                    streamO.Read(bufferIndex, 0, 2);
                }
                catch (Exception)
                {
                    try
                    {
                        bufferIndex[0] = 202;
                        streamX.Write(bufferIndex, 0, 2);
                        AddText(ConsoleTextBox, "Player O disconnected");
                    }
                    catch
                    {
                        break;
                    }
                    break;
                }

                AddMessegeText('O', bufferIndex[0]);

                try
                {
                    streamX.Write(bufferIndex, 0, 2);
                }
                catch (Exception)
                {
                    try
                    {
                        bufferIndex[0] = 202;
                        streamO.Write(bufferIndex, 0, 2);
                        AddText(ConsoleTextBox, "Player X disconnected");
                    }
                    catch
                    {
                        break;
                    }
                    break;
                }

                CheckIfReset(bufferIndex[0]);

                try
                {
                    streamX.Read(bufferIndex, 0, 2);
                }
                catch (Exception)
                {
                    try
                    {
                        bufferIndex[0] = 202;
                        streamO.Write(bufferIndex, 0, 2);
                        AddText(ConsoleTextBox, "Player X disconnected");
                    }
                    catch
                    {
                        break;
                    }
                    break;
                }

                AddMessegeText('X', bufferIndex[0]);

                try
                {
                    streamO.Write(bufferIndex, 0, 2);
                }
                catch (Exception)
                {
                    try
                    {
                        bufferIndex[0] = 202;
                        streamX.Write(bufferIndex, 0, 2);
                        AddText(ConsoleTextBox, "Player O disconnected");
                    }
                    catch
                    {
                        break;
                    }
                    break;
                }

                CheckIfReset(bufferIndex[0]);

            }
        }

        private void CheckIfReset(int value)
        {
            if (value == 100 || value == 99)
            {
                ClearTextBox(ConsoleTextBox);
                Game();
            }
        }

        private void AddMessegeText(char player, int value)
        {
            if (value == 100)
            {
                AddText(ConsoleTextBox, "player " + player + " has won");
            }
            else if (value == 99)
            {
                AddText(ConsoleTextBox, "its a tie");
            }
            else
            {
                AddText(ConsoleTextBox, "sending to player " + player + " index " + value.ToString());
            }
        }

        private void WaitForPlayers()
        {
            try
            {
                IPAddress address = IPAddress.Parse(SERVER_IP);
                TcpListener listener = new TcpListener(address, PORT);
                ConsoleTextBox.Text += "Waiting for players..." + Environment.NewLine;
                listener.Start();

                Thread waitForPlayers = new Thread(() =>
                {
                    this.clientO = listener.AcceptTcpClient();
                    this.streamO = clientO.GetStream();
                    AddText(ConsoleTextBox, "Player 1 connected: " + clientO.Client.RemoteEndPoint.ToString());
                    AddText(Players, clientO.Client.RemoteEndPoint.ToString());

                    this.clientX = listener.AcceptTcpClient();
                    this.streamX = clientX.GetStream();
                    AddText(ConsoleTextBox, "Player 2 connected: " + clientX.Client.RemoteEndPoint.ToString());
                    AddText(Players, clientX.Client.RemoteEndPoint.ToString());
                    listener.Stop();
                });
                waitForPlayers.Start();
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "error");
            }
        }

        private void AddText(TextBox box, string text)
        {
            box.Invoke((MethodInvoker)delegate ()
            {
                box.Text += text + Environment.NewLine;
            });
        }

        private void ClearTextBox(TextBox box)
        {
            box.Invoke((MethodInvoker)delegate ()
            {
                box.Text = "";
            });
        }

        public bool IsFinishedConnecting()
        {
            return (this.clientO != null && this.clientX != null && this.streamO != null && this.streamX != null);
        }

        private bool IsClientConnected(TcpClient client)
        {
            return !((client.Client.Poll(1, SelectMode.SelectRead) && (client.Client.Available == 0)) || !client.Client.Connected);
        }
    }
}
