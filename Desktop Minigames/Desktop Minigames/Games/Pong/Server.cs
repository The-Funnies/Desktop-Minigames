using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Pong
{
    class Server
    {
        private TcpListener listener;
        public Server()
        {
            this.listener = new TcpListener(IPAddress.Any, Constants.PORT);
        }
        public Client WaitForPlayer()
        {
            Client client = new Client(listener.AcceptTcpClient());
            return client;
        }
        public void StartLitsening()
        {
            this.listener.Start();
        }
        public void StopLitsening()
        {
            this.listener.Stop();
        }
    }
}
