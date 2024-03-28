using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainControl.Entity
{
   
    /// <summary>
    /// 工艺条件
    /// </summary>
    public enum EnumConditional
    {
        /// <summary>
        /// Between
        /// </summary>
        [Description("Between")]
        Between = 0,

        /// <summary>
        /// Equal
        /// </summary>
        [Description("Equal")]
        Equal = 1,

        /// <summary>
        /// HexStringBetween
        /// </summary>
        [Description("HexStringBetween")]
        HexStringBetween = 2,

        /// <summary>
        /// HexStringBetween
        /// </summary>
        [Description("CaseEqual")]
        CaseEqual = 3,
        /// <summary>
        /// Contains
        /// </summary>
        [Description("Contains")]
        Contains = 4,
        /// <summary>
        /// HexStringMatch
        /// </summary>
        [Description("HexStringMatch")]
        HexStringMatch = 5,
        /// <summary>
        /// HexStringBetween
        /// </summary>
        [Description("HotBetween")]
        HotBetween = 6,
        /// <summary>
        /// More
        /// </summary>
        [Description("More")]
        More = 7,
        /// <summary>
        /// xx
        /// </summary>
        [Description("xx")]
        xx = 8
    }
}
