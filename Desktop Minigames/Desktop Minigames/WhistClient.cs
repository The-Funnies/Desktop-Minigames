using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WhistCommon;

namespace Desktop_Minigames
{
    public partial class WhistClient : Form
    {
        private WhistCommon common;
        private int clientid;
        public WhistClient(string name, string ip)
        {
            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;
            int width = this.Width;
            int height = this.Height;

            HttpChannel channel = new HttpChannel();
            ChannelServices.RegisterChannel(channel, false);
            common = (WhistCommon)Activator.GetObject(
                typeof(WhistCommon),

                "http://localhost:8888/_Server_");

            clientid = common.GetId(name);
        }

        private void WhistClient_Load(object sender, EventArgs e)
        {

        }
    }
}
