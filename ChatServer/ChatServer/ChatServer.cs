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

namespace ChatServer
{
    public partial class ChatServer : Form
    {
        private TcpListener listener;
        private List<Client> clients = new List<Client>();
        Thread waitforclients;
        public ChatServer()
        {
            listener = new TcpListener(IPAddress.Any, 8888);
            listener.Start();
            FormClosed += (object sender, FormClosedEventArgs e) => { Environment.Exit(Environment.ExitCode); };
            waitforclients = new Thread(WaitForClient);
            waitforclients.Start();
        }
        void WaitForClient()
        {
            while (true)
            {
                TcpClient client = listener.AcceptTcpClient();
                byte[] data = new byte[257];
                client.GetStream().Read(data, 0, data.Length);

                string name = Encoding.UTF8.GetString(data);
                Thread thread = new Thread(CheckMessages);

                clients.Add(new Client(name, client, thread));
                thread.Start(clients.Count - 1);

                foreach (Client client1 in clients)
                {
                    try
                    {
                        client1.client.GetStream().Write(data, 0, data.Length);
                    }
                    catch
                    {

                    }
                }

                Thread.Sleep(50);
            }
            
        }
        
        void CheckMessages(object num)
        {
            int n = (int)num;
            while (true)
            {
                byte[] data = new byte[257];
                try
                {
                    clients[n].client.GetStream().Read(data, 0, data.Length);
                }
                catch
                {

                }

                
                string message = Encoding.UTF8.GetString(data).Substring(0, BackSlash0(Encoding.UTF8.GetString(data)));

                if (message == "")
                {
                    clients[n].thread.Abort();
                    return;
                }

                string mes = clients[n].name.Substring(0, BackSlash0(clients[n].name)) + "\n" + message;
                
                byte[] data1 = Encoding.UTF8.GetBytes(mes);


                for (int i=0;i<clients.Count; i++)
                {
                    if (i != n)
                    {
                        try
                        {
                            clients[i].client.GetStream().Write(data1, 0, data1.Length);
                        }
                        catch
                        {

                        }
                        
                    }
                }

            }
        }
        int BackSlash0(string mes)
        {
            for (int i = 0; i < mes.Length; i++)
            {
                if (mes[i] == '\0')
                {
                    return i ;
                }
            }
            return mes.Length;
        }
    }
}
