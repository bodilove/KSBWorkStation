using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.PLCData.Model;

namespace Common.PLCData.BLL
{
   public class PLCData30
    {
       public PLCDataCommon PLCDatacommon = null;
       public string CVCamerMessage = "";
       int CAMER1index = 100;

       public PLCData30(byte[] Message)
       {
           PLCDataCommonBLL plcbll = new PLCDataCommonBLL();
           PLCDatacommon = plcbll.ByteToPLCDataCommon(Message);
           CVCamerMessage = Convert.ToString(Message[CAMER1index], 2).PadLeft(8, '0');
       }
    }



   public class PLCData40_1
   {
       public PLCDataCommon PLCDatacommon = null;
       public string CVCamerMessage = "";
       int CAMER1index = 100;

       public PLCData40_1(byte[] Message)
       {
           PLCDataCommonBLL plcbll = new PLCDataCommonBLL();
           PLCDatacommon = plcbll.ByteToPLCDataCommon(Message);
           CVCamerMessage = Convert.ToString(Message[CAMER1index], 2).PadLeft(8, '0');
       }
   }
}
