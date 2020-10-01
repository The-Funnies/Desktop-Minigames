using WhistCommon;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Desktop_Minigames
{
    public partial class WhistServer : Form
    {
        
        public WhistServer()
        {

            InitializeComponent();

            HttpChannel chnl = new HttpChannel(1234);
            ChannelServices.RegisterChannel(chnl, false);

            RemotingConfiguration.RegisterWellKnownServiceType(
                typeof(ServerPart),
                "_Server_",
                WellKnownObjectMode.Singleton);
        }
    
        private void WhistServer_Load(object sender, EventArgs e)
        {

        }
    }
    class ServerPart : MarshalByRefObject, WhistCommon
    {
        private List<Card>[] pcards = new List<Card>[4];
        private int id;
        public ServerPart()
        {
            id = -1;

            //split cards
            Packet packet = new Packet();
            packet.Shuffle();
            Card[,] pcards1 = packet.GetPcards();
            for (int i = 0; i < 13; i++)
            {
                pcards[0].Add(pcards1[0, i]);
                pcards[1].Add(pcards1[1, i]);
                pcards[2].Add(pcards1[2, i]);
                pcards[3].Add(pcards1[3, i]);
            }

        }
        public int GetId(string name)
        {
            id++;
            return id;
        }

        public Card[] GetHand(int clientid)
        {
            return pcards[clientid].ToArray();
        }
    }
}
