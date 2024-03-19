using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Test.ProjectTest
{
    public partial class frmPowerOnTest : Form
    {

        public frmPowerOnTest()
        {
            InitializeComponent();
        }

        public void RefreshGridView(Dictionary<string, bool> reslit)
        {
            //this.Invoke(new Action(() =>
            //    {
            dataGridView1.Rows.Clear();
            foreach (KeyValuePair<string, bool> kp in reslit)
            {
                if (kp.Value)
                {
                    dataGridView1.Rows.Add(new object[] { kp.Key, "已通过" });
                }
                else
                {
                    dataGridView1.Rows.Add(new object[] { kp.Key, "未通过" });
                }
                //    }
                //}));
            }
        }
    }
}
