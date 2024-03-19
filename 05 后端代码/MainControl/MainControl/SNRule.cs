using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MainControl
{
   public class SNRule
    {
       //BYD车高传感器
       public string CreatSN(string ProductNum, int RollingNum,System.DateTime CurrentDate)
       {
           switch (ProductNum)
           {
               case "BYDheightSensor":
                   return BYDheightSensor(ProductNum, RollingNum, CurrentDate);
                   break;
               default:
                   return "Erro";
                   break;
           }
       }

       public string BYDheightSensor(string ProductNum, int RollingNum, System.DateTime CurrentDate)
       {
           return "AAAAAA" + RollingNum;
       }
    }
}
