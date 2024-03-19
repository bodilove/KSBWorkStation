using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Common;
using Common.PLCServer;
using Common.PLCData.Model;
using Common.PLCData.BLL;
using System.Threading;
using Common.Process.BLL;
using Common.Process.Model;
using System.IO;
using Common.StationProduct.Model;
using Library;
using System.ServiceModel;

namespace MainControl
{
    public partial class frmMain
    {
        #region 洪修改print服务

        BackgroundWorker worker = null;
        TestDataServer BC = null;
        ServiceHost serviceHost = null;




        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                BC = new TestDataServer();
                //  BC.myPrint = new TestDataServer.PrintEvent(Pinritlabel);

                //BC._hbManager = new HeartBeatManager(DiccallbackList, DicSlaveStationList, DichbRecordList);
                //BC._hbManager.LossHeartHandle = new HeartBeatManager.LossHeartHandledelegate(LossHandle);
                //BC.Channel_ClosingHandler = new EventHandler(Channel_Closing);
                //BC.frmMaster = this;
                serviceHost = new ServiceHost(BC);

                if (serviceHost.State != CommunicationState.Opened)
                {
                    serviceHost.Open();
                }

                e.Result = "正常";
            }
            catch (Exception ex)
            {
                e.Result = ex.Message;
            }
        }

        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Result != null)
            {
                if (e.Result.ToString() == "正常")
                {

                    lblDataServerState.Text = "数据服务状态:  " + "正在运行...";
                }
                else
                {
                    lblDataServerState.Text = "数据服务状态:  " + string.Format("错误：{0}", e.Result);
                }
                this.Text = this.Text;
            }
        }

        void StartServer()
        {
            if (!worker.IsBusy)
            {
                lblDataServerState.Text = "数据服务状态:  " + "正在启动......";


                worker.RunWorkerAsync();
            }
        }



        void StopSever()
        {
            serviceHost.Close();
            worker.CancelAsync();
            this.Text = "数据服务状态:  " + "停止";
        }
        #endregion

    }
}
