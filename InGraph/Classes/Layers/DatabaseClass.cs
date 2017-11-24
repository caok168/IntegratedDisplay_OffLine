using System;
using System.Collections.Generic;
using System.Text;

namespace InGraph.Classes
{

    /// <summary>
    /// 数据库层---单个cit文件所属线路的台账信息
    /// </summary>
    class DatabaseClass : LayersClass
    {
        /// <summary>
        /// 所有的台账信息---单个cit文件所属线路的台账信息，目前共五类台账信息，listDBCC.Count=5.
        /// ----曲线,坡度,道岔,长短链,速度区段
        /// </summary>
        public List<DBChannelsClass> listDBCC;

    }
}
