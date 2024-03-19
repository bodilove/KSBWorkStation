using System;
using System.Collections.Generic;
using System.Reflection;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Common;
using System.IO;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;
using System.Collections.Concurrent;

namespace Common.PLCServer
{
    public class OverallServiceNew
    {

        bool UIRefresh = false;
        TabControl currentTab = null;
        /// <summary>
        /// UI刷新间隔时间
        /// </summary>
        int RefreshUItime = 2000;

        public GeneralConfig CurrentConfig;// = new GeneralConfig();
        // string Path = Application.StartupPath + @"\GNConfig\GNConfig.cfg";
      
        object libMothed = null;//动态调用的方法库
        Dictionary<PLCHandle, PLCS7> PLCLlst = new Dictionary<PLCHandle, PLCS7>();
  //      bool Isconnect = false;

        /// <summary>
        /// 在构造方法中，传入了配置信息和方法库
        /// </summary>
        /// <param name="currentConfig"></param>
        /// <param name="ClassObj">frmFormMain</param>
        public OverallServiceNew(GeneralConfig currentConfig, object ClassObj)
        {
            CurrentConfig = currentConfig;
            libMothed = ClassObj;
        }






        public void ConnectAllPlc(TabControl tbc)
        {
            currentTab = tbc;
            if (currentTab != null)
            {
                currentTab.TabPages.Clear();
            }
            PLCLlst.Clear();//
            //开启多个服务
            foreach (PLCHandle plcH in CurrentConfig.plclst)
            {
                PLCS7 p = new PLCS7(libMothed, plcH);
                if (p.OpenPLCServer())//开启与plc的通讯连接
                {
                    if (currentTab != null)
                    {
                        p.InitControl(currentTab);
                        currentTab.TabPages.Add(p.tabPage);
                    }
                    PLCLlst.Add(plcH, p);

                }
            }
            if (currentTab != null)
            {
                currentTab.SelectedIndexChanged += new System.EventHandler(this.tabConrolMonitor_SelectedIndexChanged);
            }
        }
        private void tabConrolMonitor_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (currentTab != null)
            {
                foreach (KeyValuePair<PLCHandle, PLCS7> kp in PLCLlst)
                {
                    if (currentTab.SelectedTab.Text == kp.Key.Name)
                    {
                        kp.Value.MonitorControl.StarMonitor();
                    }
                    else
                    {
                        kp.Value.MonitorControl.StopMonitor();
                    }
                 
                }
            }
        }

        public void SetCurrentWorkType(byte CurrentWorkType)
        {
            foreach (KeyValuePair<PLCHandle, PLCS7> kp in PLCLlst)
            {
                kp.Value.SetCurrentWorkType = CurrentWorkType;
            }
        }

        public Dictionary<string,byte> GetCurrentWorkType()
        {
            Dictionary<string, byte> dic = new Dictionary<string, byte>();
            foreach (KeyValuePair<PLCHandle, PLCS7> kp in PLCLlst)
            {
                dic.Add(kp.Key.Name,kp.Value.GetCurrentWorkType);
            }
            return dic;
        }

        public void SetCanWork()
        {
            foreach (KeyValuePair<PLCHandle, PLCS7> kp in PLCLlst)
            {
                kp.Value.SetCanWork();
            
            }
        }


        public void ResetNotWork()
        {
            foreach (KeyValuePair<PLCHandle, PLCS7> kp in PLCLlst)
            {
                kp.Value.ResetNotWork();

            }
        }

        public void DisConnectAllPlc()
        {
          
            foreach (KeyValuePair<PLCHandle, PLCS7> kp in PLCLlst)
            {
                kp.Value.ResetNotWork();
                kp.Value.DisposeControl();
                kp.Value.ClosePLCServer();
            }
            PLCLlst.Clear();
            if (currentTab != null)
            {
                currentTab.TabPages.Clear();
 
            }
        }

        public ParaAndResultStrut ExcutPC2PLCEvent(object para, string PLCName, string StationName, string EventName)
        {
            //找到PLC
            ParaAndResultStrut returnValue = null;
            try
            {
                PLCS7 plc = null;
              
                foreach (KeyValuePair<PLCHandle, PLCS7> kp in PLCLlst)
                {
                    if (kp.Key.Name == PLCName)
                    {
                        plc = kp.Value;
                    }
                }
                if (plc != null)
                {

              returnValue=      plc.ExcutPC2PLCEvent(para, StationName, EventName);
              
                }
                else
                {
                    throw new Exception("未找到指定PLC模块");
                }
             
            }
            catch (Exception ex)
            {
                //throw new Exception("写入PLC数据报错");
                returnValue = null;
            }
            return returnValue;
        }

        public object ExcutPC2PLCEvent(object para, int PLCId, int StationId, int EventId)
        {
            //找到PLC
            object returnValue = null;
            try
            {
                PLCS7 plc = null;
                if(PLCId<PLCLlst.Count)
                {
                    foreach (KeyValuePair<PLCHandle, PLCS7> kp in PLCLlst)
                    {
                        if (kp.Key.id == PLCId)
                        {
                            plc = kp.Value;
                        }
                    }
                }
                else
                {
                    throw new Exception("未找到指定PLC模块");
                }

                if (plc != null)
                {

                    plc.ExcutPC2PLCEvent(para, StationId, EventId);

                }
                else
                {
                    throw new Exception("未找到指定PLC模块");
                }
            }
            catch (Exception ex)
            {
                //throw new Exception("写入PLC数据报错");
                returnValue = null;
            }
            return returnValue;
        }


        public DataTable GetSelectStationWarning(int PLCId,int StationId)
        {
            PLCS7 plc = null;
            if (PLCId < PLCLlst.Count)
            {
                foreach (KeyValuePair<PLCHandle, PLCS7> kp in PLCLlst)
                {
                    if (kp.Key.id == PLCId)
                    {
                        plc = kp.Value;
                    }
                }
            }
            else
            {
                return null;
                //throw new Exception("未找到指定PLC模块");
            }
           return plc.GetCurrentStWarning(StationId);
        }


        public DataTable GetSelectStationEMGWarning(int PLCId, int StationId)
        {
            PLCS7 plc = null;
            if (PLCId < PLCLlst.Count)
            {
                foreach (KeyValuePair<PLCHandle, PLCS7> kp in PLCLlst)
                {
                    if (kp.Key.id == PLCId)
                    {
                        plc = kp.Value;
                    }
                }
            }
            else
            {
                return null;
               // throw new Exception("未找到指定PLC模块");
            }
            return plc.GetCunrrentStEMGWarning(StationId);
        }


        public int GetFlow(int PLCId, int StationId,out string Message)
        {
            PLCS7 plc = null;
            if (PLCId < PLCLlst.Count)
            {
                foreach (KeyValuePair<PLCHandle, PLCS7> kp in PLCLlst)
                {
                    if (kp.Key.id == PLCId)
                    {
                        plc = kp.Value;
                    }
                }
            }
            else
            {
                Message = "";
                return -1;
             //   throw new Exception("未找到指定PLC模块");
            }
            return plc.GetCurrentStFlowMessage(StationId, out Message);
        }


        public DataTable GetSelectPLCTaskState(int PLCId)
        {
            PLCS7 plc = null;
            if (PLCId < PLCLlst.Count)
            {
                foreach (KeyValuePair<PLCHandle, PLCS7> kp in PLCLlst)
                {
                    if (kp.Key.id == PLCId)
                    {
                        plc = kp.Value;
                    }
                }
            }
            else
            {
                return null;
               // throw new Exception("未找到指定PLC模块");
            }
            return plc.GetTaskStateData();
        }

        public DataTable GetSelectPLCFlowState(int PLCId)
        {
            PLCS7 plc = null;
            if (PLCId < PLCLlst.Count)
            {
                foreach (KeyValuePair<PLCHandle, PLCS7> kp in PLCLlst)
                {
                    if (kp.Key.id == PLCId)
                    {
                        plc = kp.Value;
                    }
                }
            }
            else
            {
                return null;
                // throw new Exception("未找到指定PLC模块");
            }
            return plc.GetStationFlows();
        }

        public DataTable GetSelectPLCEMGWarning(int PLCId)
        {
            PLCS7 plc = null;
            if (PLCId < PLCLlst.Count)
            {
                foreach (KeyValuePair<PLCHandle, PLCS7> kp in PLCLlst)
                {
                    if (kp.Key.id == PLCId)
                    {
                        plc = kp.Value;
                    }
                }
            }
            else
            {
                return null;
                // throw new Exception("未找到指定PLC模块");
            }
            return plc.GetEMGs();
        }


        public bool GetPLCConnectState(int PLCId)
        {
            PLCS7 plc = null;
           
            if (PLCId < PLCLlst.Count)
            {
                foreach (KeyValuePair<PLCHandle, PLCS7> kp in PLCLlst)
                {
                    if (kp.Key.id == PLCId)
                    {
                        plc = kp.Value;
                    }
                }
            }
            else
            {
                return false;
           
            }
            return plc.IsConnect;
        }



     
    }
}
