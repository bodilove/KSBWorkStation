using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.CreatSN
{
    public class DemoSNCreater : ASNCreater
    {
        public override string CreatSN(int RollingNum, string ProductNum)
        {
            string rollNum = RollingNum.ToString().PadLeft(5, '0');
            string SN = ProductNum + System.DateTime.Now.ToString("yyMMdd") + rollNum;
            return SN;
        }
    }
}
