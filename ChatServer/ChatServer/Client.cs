using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ChatServer
{
    class Client
    {
        private TcpClient client;
        private NetworkStream stream;
        private string name;
        public Client(TcpClient client, NetworkStream stream, string name)
        {
            this.client = client;
            this.name = name;
            this.stream = stream;
        }

        public TcpClient GetClient()
        {
            return client;
        }

        public NetworkStream GetNetWorkStream()
        {
            return stream;
        }

        public string GetName()
        {
            return name;
        }


    }
}
