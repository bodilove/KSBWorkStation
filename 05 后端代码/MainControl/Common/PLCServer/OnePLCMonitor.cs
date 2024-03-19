using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Common.PLCServer
{
    public partial class OnePLCMonitor : UserControl
    {

        public TabControl TabParent = null;
        public OnePLCMonitor()
        {
            InitializeComponent();
        }


        public OnePLCMonitor(PLCS7 plc, TabControl tabParent)
        {
            InitializeComponent();
            CurrentPLC = plc;
            TabParent = tabParent;
            dgvEvent.Columns.Clear();
            dgvprocess.Columns.Clear();
            dgvEMG.Columns.Clear();
        }
         void UplblConnect(bool res)
        {
            lblConnectState.Text = res ? "在线" : "离线";
            lblConnectState.BackColor = res ? Color.Green : Color.Red;
        }
         PLCS7 CurrentPLC = null;
         public void UPUI()
         {
            
         }
         public void StarMonitor()
         {
             if (CurrentPLC != null)
             {
                 if (TabParent.SelectedTab.Name == CurrentPLC.tabPage.Name && TabParent.SelectedTab.Text == CurrentPLC.tabPage.Text)
                 {


                     UplblConnect(CurrentPLC.IsConnect);

                     dgvEvent.DataSource = CurrentPLC.GetTaskStateData();
                     dgvprocess.DataSource = CurrentPLC.GetStationFlows();
                     dgvEMG.DataSource = CurrentPLC.GetEMGs();
                    
                 }
             }
             timer1.Enabled = true;
         }
         public void StopMonitor()
         {
             timer1.Enabled = false;
         }

         private void timer1_Tick(object sender, EventArgs e)
         {
             if (CurrentPLC != null)
             {
                 if (TabParent.SelectedTab.Name == CurrentPLC.tabPage.Name && TabParent.SelectedTab.Text == CurrentPLC.tabPage.Text)
                 {


                     UplblConnect(CurrentPLC.IsConnect);

                     dgvEvent.DataSource = CurrentPLC.GetTaskStateData();
                     dgvprocess.DataSource = CurrentPLC.GetStationFlows();
                     dgvEMG.DataSource = CurrentPLC.GetEMGs();
                   //  Console.WriteLine("刷新: " + CurrentPLC.tabPage.Text);
                 }
             }
         }

         private void btnResetPLC_Click(object sender, EventArgs e)
         {
             btnResetPLC.Enabled = false;
            // string text, string caption, MessageBoxButtons buttons)
             if (DialogResult.Yes == MessageBox.Show("手动重启与PLC有风险，是否确定PLC已经初始化完成？", "警告", MessageBoxButtons.YesNo))
             {
               
                 Application.DoEvents();
                 CurrentPLC.ClosePLCServer();
                 System.Threading.Thread.Sleep(1000);
                 if (CurrentPLC.OpenPLCServer())
                 {
                     MessageBox.Show( "重启PLC成功。","通知");
                 }
                 else
                 {
                     MessageBox.Show("重启PLC失败。", "通知");
                 }
             }
             btnResetPLC.Enabled = true;
         }

         private void btnDisposeSelectEvent_Click(object sender, EventArgs e)
         {
            // int SelectEvent = dgvEvent.SelectedCells[0].RowIndex;
             //dgvEvent.Rows[SelectEvent].Cells.con
             frmEvent fm = new frmEvent(this.CurrentPLC);
             fm.ShowDialog(this);

         }
    }
}
