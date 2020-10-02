using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pong
{
    public partial class ServerOptions : Form
    {
        private ServerGUI server;
        public ServerOptions(ServerGUI form)
        {
            InitializeComponent();
            this.server = form;
        }

        private void Ok_Button_Click(object sender, EventArgs e)
        {
            if (radio_Free.Checked)
            {
                this.server.option = ServerOption.freeplay;
            }
            else if(radio_2P.Checked)
            {
                this.server.option = ServerOption.twoP;
            }
            else if (radio_4P.Checked)
            {
                this.server.option = ServerOption.fourP;
            }
            else if (radio_8P.Checked)
            {
                this.server.option = ServerOption.eightP;
            }
            this.Close();
        }
    }

    public enum ServerOption
    {
        freeplay,
        twoP,
        fourP,
        eightP
    }

}
