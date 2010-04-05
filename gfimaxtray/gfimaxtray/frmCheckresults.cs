using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace gfimaxtray
{
    public partial class frmCheckresults : Form
    {
        public frmCheckresults()
        {
            InitializeComponent();
        }

        public void init(ArrayList FailedDevices)
        {
            ArrayList tmpList = new ArrayList();
            foreach (failedCheck tmpCheck in FailedDevices)
            {
                if (!tmpCheck.IsOffline)
                {
                    tmpList.Add(tmpCheck);
                }
            }

            dataGrid1.DataSource = tmpList;

        }
    }
}
