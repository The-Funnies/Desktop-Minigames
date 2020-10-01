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
        private int width;
        private int height;
        public WhistClient(string name, string ip)
        {
            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;
            width = this.Width;
            height = this.Height;

            HttpChannel channel = new HttpChannel();
            ChannelServices.RegisterChannel(channel, false);

            common = (WhistCommon)Activator.GetObject(
                typeof(WhistCommon),
                "http://localhost:1234/_Server_");

            int clientid = common.GetId(name);
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.F11)
            {
                if (FormBorderStyle == FormBorderStyle.Sizable)
                {
                    FormBorderStyle = FormBorderStyle.None;
                    WindowState = FormWindowState.Maximized;
                }
                else
                {
                    FormBorderStyle = FormBorderStyle.Sizable;
                    WindowState = FormWindowState.Normal;
                }
            }
            return false;
        }
        private void WhistClient_Load(object sender, EventArgs e)
        {

        }
    }
}
