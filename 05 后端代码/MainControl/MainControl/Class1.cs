using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
namespace MainControl
{
    class Class1
    {


        //#region 洪修改print服务

        //BackgroundWorker worker = null;
        //ServicePrint BC = null;
        //ServiceHost serviceHost = null;

       


        //void worker_DoWork(object sender, DoWorkEventArgs e)
        //{
        //    try
        //    {
        //        BC = new ServicePrint();
        //        BC.myPrint = new ServicePrint.PrintEvent(Pinritlabel);

        //        //BC._hbManager = new HeartBeatManager(DiccallbackList, DicSlaveStationList, DichbRecordList);
        //        //BC._hbManager.LossHeartHandle = new HeartBeatManager.LossHeartHandledelegate(LossHandle);
        //        //BC.Channel_ClosingHandler = new EventHandler(Channel_Closing);
        //        //BC.frmMaster = this;
        //        serviceHost = new ServiceHost(BC);

        //        if (serviceHost.State != CommunicationState.Opened)
        //        {
        //            serviceHost.Open();
        //        }

        //        e.Result = "正常";
        //    }
        //    catch (Exception ex)
        //    {
        //        e.Result = ex.Message;
        //    }
        //}

        //void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        //{
        //    if (e.Result != null)
        //    {
        //        if (e.Result.ToString() == "正常")
        //        {

        //            this.Text = "服务正在运行......";
        //        }
        //        else
        //        {
        //            this.Text = string.Format("错误：{0}", e.Result);
        //        }
        //        this.Text = this.Text;
        //    }
        //}

        //void StartServer()
        //{
        //    if (!worker.IsBusy)
        //    {
        //        this.Text = "正在启动......";


        //        worker.RunWorkerAsync();
        //    }
        //}



        //void StopSever()
        //{
        //    serviceHost.Close();
        //    worker.CancelAsync();
        //    this.Text = "服务已经停止";
        //}
        //#endregion
    }
}
