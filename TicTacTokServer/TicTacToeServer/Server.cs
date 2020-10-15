using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using WhistServer;

namespace TicTacToeServer
{
    class Server
    {
        private TcpListener listener;
        private Client[] clients = new Client[2];
        private int[][] board;

        public Server()
        {
            listener = new TcpListener(IPAddress.Any, 8842);
            listener.Start();

            WaitForClients();
        }

        void WaitForClients()
        {
            for (int i = 0; i < 2; i++)
            {
                TcpClient client = listener.AcceptTcpClient();

                byte[] data = new byte[256];
                client.GetStream().Read(data, 0, data.Length);

                string name = Encoding.ASCII.GetString(data);

                clients[i] = new Client(name, client, client.GetStream());

            }

            for (int i = 0; i < 2; i++)
            {
                SendString(clients[(i + 1) % 2].name, i);
            }

            StartGame();
        }
        void StartGame()
        {
            Random rnd = new Random();
            int turn = rnd.Next(0, 2);//a random player will play first

            SendString("a", turn);//send who is first to the players
            SendString("b", (turn + 1) % 2);

            board = new int[9][];
            for (int i = 0; i < 9; i++)
            {
                board[i] = new int[9];
            }

            for (int a = turn; a < 81 + turn; a++)
            {

                int id = a % 2;

                string clicked = GetString(ReceiveString(id));
                int entry = int.Parse(clicked) / 10;
                int index = int.Parse(clicked) % 10;

                board[entry][index] = id + 1;

                
                if (IsSmallWinner(id, entry))
                {
                    board[entry] = new int[id + 1];

                    int howmanyentries = 0;

                    for (int i = 0; i < 9; i++)
                    {
                        if (board[i].Length != 9)
                        {
                            howmanyentries++;
                        }
                    }
                    if (howmanyentries == 9)
                    {
                        SendString("d", id);
                        SendString("0", (id + 1) % 2);
                        break;
                    }

                    if (IsBigWinner(id))
                    {
                        //player won

                        SendString("c", id);
                        SendString("0", (id + 1) % 2);

                        break;
                    }
                    else
                    {
                        SendString("a", id);

                        if (board[index].Length != 9)
                        {
                            SendString(clicked + "01", (id + 1) % 2);
                        }
                        else
                        {
                            SendString(clicked + "0", (id + 1) % 2);
                        }
                    }
                    continue;
                }
                else
                {
                    SendString("b", id);
                }

                int howmanyindeciesmarked = 0;
                for (int i = 0; i < 9; i++)//if entry is full
                {
                    if (board[entry][i] == 0)
                    {
                        howmanyindeciesmarked++;
                    }
                }
                if (howmanyindeciesmarked == 9)
                {
                    board[entry] = new int[0];
                }


                if (board[index].Length != 9)//if entry is full
                {
                    SendString(clicked + "00", (id + 1) % 2);
                }
                else
                {
                    SendString(clicked, (id + 1) % 2);
                } 
            }
            //Game Done

            clients[0].stream.Read(new byte[1], 0, 1); 
            clients[1].stream.Read(new byte[1], 0, 1);

            StartGame();
        }
        bool IsBigWinner(int id)
        {
            for (int i = 0; i < 3; i++)
            {
                if (board[3 * i].Length == id + 1 && board[3 * i + 1].Length == id + 1 && board[3 * i + 2].Length == id + 1)
                {
                    return true;
                }
                if (board[i].Length == id + 1 && board[i + 3].Length == id + 1 && board[i + 6].Length == id + 1)
                {
                    return true;
                }
            }
            if (board[0].Length == id + 1 && board[4].Length == id + 1 && board[8].Length == id + 1)
            {
                return true;
            }
            if (board[2].Length == id + 1 && board[4].Length == id + 1 && board[6].Length == id + 1)
            {
                return true;
            }
            return false;
        }
        bool IsSmallWinner(int clientid, int entry)
        {
            for (int i = 0; i < 3; i++)
            {
                if (board[entry][3 * i] == clientid + 1 && board[entry][3 * i + 1] == clientid + 1 && board[entry][3 * i + 2] == clientid + 1)
                {
                    return true;
                }
                if (board[entry][i] == clientid + 1 && board[entry][i + 3] == clientid + 1 && board[entry][i + 6] == clientid + 1)
                {
                    return true;
                }
            }
            if (board[entry][0] == clientid + 1 && board[entry][4] == clientid + 1 && board[entry][8] == clientid + 1)
            {
                return true;
            }
            if (board[entry][2] == clientid + 1 && board[entry][4] == clientid + 1 && board[entry][6] == clientid + 1)
            {
                return true;
            }
            return false;
        }
        public void SendInt(int num, int clientid)
        {
            clients[clientid].stream.Write(new byte[1] { (byte)num }, 0, 1);
        }
        string ReceiveString(int clientid)
        {
            byte[] data = new byte[256];

            clients[clientid].stream.Read(data, 0, data.Length);

            return Encoding.UTF8.GetString(data);
        }
        void SendString(string str, int clientid)
        {
            byte[] data = Encoding.UTF8.GetBytes(str);

            clients[clientid].stream.Write(data, 0, data.Length);
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
    }
}
