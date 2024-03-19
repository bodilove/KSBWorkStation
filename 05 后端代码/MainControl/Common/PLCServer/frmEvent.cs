using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Common.PLCServer
{

   
    public partial class frmEvent : Form
    {
        public frmEvent(PLCS7 PLC)
        {
            CurrentPLC = PLC;
            InitializeComponent();
        }
        PLCS7 CurrentPLC = null;


        public void ShowEvent()
        {
            btnOK.Enabled = false; ;
            dgveventTask.Rows.Clear();
            foreach (KeyValuePair<int, CutromTask> kp in CurrentPLC.Dictask)
            {
                if (kp.Value.task.Status == System.Threading.Tasks.TaskStatus.Running)
                {
                    dgveventTask.Rows.Add(new object[] { kp.Value.StationId, kp.Value.StationName, kp.Value.EventId, kp.Value.EventName });
                }
            }
            if (dgveventTask.Rows.Count > 0)
            {
                btnOK.Enabled = true; ;
            }
         
          
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgveventTask.SelectedCells[0].RowIndex >= 0)
                {
                    int stationId = Convert.ToInt32(dgveventTask.Rows[dgveventTask.SelectedCells[0].RowIndex].Cells["ColStationID"].Value);
                    int EventId = Convert.ToInt32(dgveventTask.Rows[dgveventTask.SelectedCells[0].RowIndex].Cells["colEventId"].Value);
                    CurrentPLC.DisposeEvent(stationId, EventId, 2000);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            this.Close();
        }

        private void btnCanncel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmEvent_Load(object sender, EventArgs e)
        {
            ShowEvent();
        }


    }
}
