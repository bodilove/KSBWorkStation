using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.CreatSN
{
      public abstract class ASNCreater
    {
        public string ProductNum = "";
        public string ProductName = "";
        public abstract string CreatSN(int RollingNum,string ProductNum);

        //public string CreatSN()
        //{

        //}
    }
}
