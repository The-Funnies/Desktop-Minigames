using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ChatServer
{
    class Client
    {
        public string name {get;}
        public TcpClient client { get; }
        public Thread thread { get; }

        public Client(string name, TcpClient client, Thread thread)
        {
            this.name = name;
            this.client = client;
            this.thread = thread;
        }
    }
}
