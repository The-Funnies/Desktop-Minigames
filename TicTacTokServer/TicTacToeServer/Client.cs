using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace WhistServer
{
    class Client
    {
        public string name { get; }
        public TcpClient client { get; }
        public NetworkStream stream { get; }

        public Client(string name, TcpClient client, NetworkStream stream)
        {
            this.name = name;
            this.client = client;
            this.stream = stream;
        }
    }
}
