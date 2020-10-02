using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Pong
{
    public class Client
    {
        private TcpClient client;
        private NetworkStream stream;
        public string name;
        public int lineIndex;
        public Client(string ip, int port)
        {
            this.client = new TcpClient(ip, port);
            this.stream = this.client.GetStream();
        }
        public Client(TcpClient client)
        {
            this.client = client;
            this.stream = client.GetStream();
        }
        public void SendInt(int num)
        {
            this.stream.Write(new byte[1] { (byte)num }, 0, 1);
        }

        public void SendMyRacket(int racketTop)
        {
            byte[] toSend = BitConverter.GetBytes(racketTop);
            this.stream.Write(toSend, 0, toSend.Length);
        }
        public int ReciveRacket()
        {
            byte[] buffer = new byte[4];
            this.stream.Read(buffer, 0, 4);
            return BitConverter.ToInt32(buffer, 0);
        }
        public int ReciveInt()
        {
            byte[] buffer = new byte[1];
            this.stream.Read(buffer, 0, 1);
            return (int)buffer[0];
        }

        public void SendName(string name)
        {
            byte[] nameArr = Utilities.StringToByteArray(name);
            this.stream.Write(nameArr, 0, nameArr.Length);
        }

        public string ReciveName()
        {
            byte[] buffer = new byte[12];
            this.stream.Read(buffer, 0, 12);
            return Utilities.ByteArrayToString(buffer.Where(b => b != 0).ToArray());
        }

        public void SendServer(bool isOpponentLostConnection, WhoWins winner, int ballX, int ballY, int enemyY)
        {
            byte[] toSend = new byte[14];
            toSend[0] = (byte)(isOpponentLostConnection ? 1 : 0);
            toSend[1] = (byte)(int)winner;
            toSend = AddNumberToByteArr(toSend, ballX, 2);
            toSend = AddNumberToByteArr(toSend, ballY, 6);
            toSend = AddNumberToByteArr(toSend, enemyY, 10);
            this.stream.Write(toSend, 0, toSend.Length);
        }

        public Tuple<bool, WhoWins, int, int, int> ReciveServer()
        {
            byte[] buffer = new byte[14];
            this.stream.Read(buffer, 0, buffer.Length);
            bool isOpponentLostConnection = (int)buffer[0] != 0;
            WhoWins winner = (WhoWins)(int)buffer[1];
            int[] locations = new int[3];
            for (int i = 0; i < 3; i++)
            {
                locations[i] = BitConverter.ToInt32(buffer, 2 + (i * 4));
            }
            return new Tuple<bool, WhoWins, int, int, int>(isOpponentLostConnection, winner, locations[0], locations[1], locations[2]);
        }

        private byte[] AddNumberToByteArr(byte[] arr, int numToAdd, int startIndex)
        {
            byte[] tmpNum = BitConverter.GetBytes(numToAdd);
            for (int i = 0; i < 4; i++)
            {
                arr[i + startIndex] = tmpNum[i];
            }
            return arr;
        }

        public void SendServerFirstAnswer(bool isAccepted, string name, ServerOption serverOption)
        {
            byte[] nameArr = Utilities.StringToByteArray(name);
            byte[] toSend = new byte[12];
            for (int i = 0; i < nameArr.Length; i++)
            {
                toSend[i] = nameArr[i];
            }
            toSend[10] = Convert.ToByte(isAccepted? 1 : 0);
            toSend[11] = Convert.ToByte((int)serverOption);
            this.stream.Write(toSend, 0, toSend.Length);
        }

        public Tuple<string, bool, ServerOption> ReciveServerFirstAnswer()
        {
            byte[] buffer = new byte[12];
            this.stream.Read(buffer, 0, 12);
            string name = Utilities.ByteArrayToString(buffer.Take(10).Where(b => b != 0).ToArray());
            bool isAccepted = buffer[10] != 0;
            ServerOption serverOption = (ServerOption)Convert.ToInt32(buffer[11]);
            return new Tuple<string, bool, ServerOption>(name, isAccepted, serverOption);
        }

        public void FirstSend(string name, int[] resolution)
        {
            byte[] nameArr = Utilities.StringToByteArray(name);
            byte[] toSend = new byte[12];
            for (int i = 0; i < nameArr.Length; i++)
            {
                toSend[i] = nameArr[i];
            }
            toSend[10] = Convert.ToByte(Math.Round((double)resolution[0] / 10));
            toSend[11] = Convert.ToByte(Math.Round((double)resolution[1] / 10));
            this.stream.Write(toSend, 0, toSend.Length);
        }

        public Tuple<string, int[]> FirstRecive()
        {
            byte[] buffer = new byte[12];
            this.stream.Read(buffer, 0, 12);
            string name = Utilities.ByteArrayToString(buffer.Take(10).Where(b => b != 0).ToArray());
            int[] resolution = new int[2] { Convert.ToInt32(buffer[10] * 10), Convert.ToInt32(buffer[11] * 10) };
            return new Tuple<string, int[]>(name, resolution);
        }
        public bool IsConnected()
        {
            return !((this.client.Client.Poll(1, SelectMode.SelectRead) && (this.client.Client.Available == 0)) || !this.client.Client.Connected);
        }
        public void Close()
        {
            this.client.Close();
        }
    }
}
