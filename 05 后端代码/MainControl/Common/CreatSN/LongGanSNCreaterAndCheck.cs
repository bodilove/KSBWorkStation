using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.CreatSN
{
    public class LongGanSNCreaterAndCheck
    {
        string FactoryNum = "L";
        string LineNum = "H02";

        public string CreatSN(int RollingNum, Common.Product.Model.ProductConfigInfo CurrentProductConfigInfo)
        {
            return CreatSNBySAPNum(RollingNum, CurrentProductConfigInfo.SAPPartNum);
        }


        public string CreatSNbyCoustomPartNum(int RollingNum, string CustomPartNum)
        {
            string fixPart = FactoryNum + LineNum + CustomPartNum;
            string rollNum = RollingNum.ToString().PadLeft(5, '0');
            string SN = fixPart +" " +System.DateTime.Now.ToString("yyMMdd") + rollNum;
            return SN;
        }

        public string CreatSNBySAPNum(int RollingNum, string SAPNUM)
        {
            string fixPart = FactoryNum + LineNum + SAPNUM;
            string rollNum = RollingNum.ToString().PadLeft(5, '0');
            string SN = fixPart + " " + System.DateTime.Now.ToString("yyMMdd") + rollNum;
            return SN;
        }



        public bool CheckMainpart(string PartNum)//检查本体条码
        {
            return true;
        }
    }
}
