using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChatServer
{
    public partial class Server : Form
    {
        List<Client> clients=new List<Client>();
        private TcpListener listener;
        private Thread waitforclients;
        private List<Thread> threads = new List<Thread>();
        public Server()
        {
            waitforclients = new Thread(WaitForClients);
            
            listener = new TcpListener(IPAddress.Any, 8888);
            listener.Start();

            waitforclients.Start();
        }

        public void WaitForClients()
        {
            while (true)
            {
                
                TcpClient tempclient = listener.AcceptTcpClient();
                NetworkStream networkStream = tempclient.GetStream();

                byte[] data = new byte[257];
                networkStream.Read(data, 0, data.Length);
                string name = System.Text.Encoding.ASCII.GetString(data);

                clients.Add(new Client(tempclient, networkStream, name));
                Thread thread = new Thread(CheckMessages);
                threads.Add(thread);
                thread.Start();
            }
        }

        void CheckMessages()
        {

        }
    }
}
