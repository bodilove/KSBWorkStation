using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test.ProjectTest
{
    /// <summary>
    /// 单步测试数据
    /// </summary>
   public  class Station_TestInfo
    {
        /// <summary>
        /// 
        /// </summary>
       public string ID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string UUTestId { get; set; }
       /// <summary>
        /// 测试名字
       /// </summary>
       public string TestName{ get; set; }
       /// <summary>
       /// 测试步
       /// </summary>
       public string StepName { get; set; }
       /// <summary>
       /// 比对方式
       /// </summary>
       public string ComPareMode { get; set; }
       /// <summary>
       /// 测试时间
       /// </summary>
       public string  TestTime { get; set; }
       /// <summary>
       /// 下限
       /// </summary>
       public string Llimit { get; set; }
       /// <summary>
       /// 上限
       /// </summary>
       public string Ulimit { get; set; }
       /// <summary>
       /// 比对值
       /// </summary>
       public string Nom { get; set; }
       /// <summary>
       /// 测试值
       /// </summary>
       public string TestValue { get; set; }
       /// <summary>
       /// 测试值单位
       /// </summary>
       public string Unit { get; set; }
       /// <summary>
       /// 测试结果
       /// </summary>
       public string Result { get; set; }
       /// <summary>
       /// 测试描述
       /// </summary>
       public string Description { get; set; }
   
       /// <summary>
       /// 产品PartNum
       /// </summary>
       public string Product_ID { get; set; }

       public string SpanTime { get; set; }

    }
}
