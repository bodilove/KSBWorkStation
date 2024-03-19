using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test.ProjectTest
{
    /// <summary>
    /// 测试历史库
    /// </summary>
    public class Station_ProductInfo
    {

        public string UUTestId { get; set; }


        public string SN { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public string StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public string EndTime { get; set; }

        /// <summary>
        /// 测试结果
        /// </summary>
        public string Result { get; set; }

        /// <summary>
        /// 默认=0  点检=1  测试=2 返工 =3
        /// </summary>
        public int PrdType { get; set; }

        /// <summary>
        /// 操作人
        /// </summary>
        public string UserID { get; set; }

        /// <summary>
        /// 产品Partnum
        /// </summary>
        public string PartNum { get; set; }

        /// <summary>
        /// 工站名称
        /// </summary>
        public string StationID { get; set; }

    }
}
