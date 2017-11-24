using System;
using System.Collections.Generic;
using System.Text;

namespace InGraph.Classes.Layers
{
    /// <summary>
    /// 详细台账信息类---
    /// </summary>
    class DBDataClass
    {
        /// <summary>
        /// 台账信息起点
        /// </summary>
        public long lStartMeter;
        /// <summary>
        /// 台账信息终点
        /// </summary>
        public long lEndMeter;
        /// <summary>
        /// 每种台账信息所关注的，譬如：曲线半径，坡度，速度
        /// </summary>
        public string sText;
        /// <summary>
        /// 台账信息类别：----曲线,坡度,道岔,长短链,速度区段
        /// </summary>
        public string sDBType;
    }
}
