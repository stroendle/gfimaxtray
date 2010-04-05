using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace gfimaxtray
{
    public partial class frmKonfiguration : Form
    {
        public frmKonfiguration()
        {
            InitializeComponent();
        }

        private void frmKonfiguration_Load(object sender, EventArgs e)
        {
            textBoxDBServer.Text = clsRegistry.getRegistryValue("dbserver");
            textBoxAPIKey.Text = clsRegistry.getRegistryValue("apikey");
        }

        private void buttonSpeichern_Click(object sender, EventArgs e)
        {
            clsRegistry.setRegistryValue("dbserver", textBoxDBServer.Text);
            clsRegistry.setRegistryValue("apikey", textBoxAPIKey.Text);
            this.Close();
        }
    }
}
