using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test.Query
{
    public class ChartStruct
    {
        public string StepName = "";
        public string Discrption = "";
        public int[] TestTime = new int[12];
        public int[] PassTime = new int[12];
        public int[] FailTime = new int[12];
    }
}
