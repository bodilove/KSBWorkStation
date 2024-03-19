using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Test.ProjectFileEditor
{
    public partial class frmAddOneTestPowerOn : Form
    {
        public frmAddOneTestPowerOn()
        {
            InitializeComponent();
        }
        public String Name;

        public string SN;
        private void frmAddOneTestPowerOn_Load(object sender, EventArgs e)
        {

        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            Name = txtName.Text;
            SN = txtSN.Text;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
