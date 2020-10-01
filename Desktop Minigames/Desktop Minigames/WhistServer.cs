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
            HttpChannel chnl = new HttpChannel(8888);
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
        private Player[] players = new Player[4];
        private List<Card>[] pcards = new List<Card>[4];
        int id = -1;
        public ServerPart()
        {
            //split cards
            Packet packet = new Packet();
            packet.Shuffle();
            pcards = packet.GetPcards();

        }
        public int GetId(string name)
        {
            
            id++;
            players[id] = new Player(pcards[id], id, name);
            if (id == 3)
            {
                pcards = null;
            }
            return id;
        }

        public Card[] GetHand(int clientid)
        {
            return players[clientid].Getpcards().ToArray();
        }
    }
}
