using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using InGraph.Classes.Layers;

namespace InGraph.Classes
{
    /// <summary>
    /// 具体每一种台账的信息类，目前共五类台账信息
    /// ----曲线,坡度,道岔,长短链,速度区段
    /// </summary>
    class DBChannelsClass
    {
        public int Id = 0;
        /// <summary>
        /// 台账信息中文名
        /// </summary>
        public string Name = string.Empty;
        public string NonChineseName = string.Empty;
        public string ChineseName = string.Empty;
        public int Color = 0;
        /// <summary>
        /// 台账信息是否显示。
        /// </summary>
        public bool Visible = true;
        public int Location = 1;
        public float fLineWidth = 1.0f;

        /// <summary>
        /// 台账信息显示时右边的矩形框
        /// </summary>
        public Rectangle Rect { set; get; }

        public string sGraph = string.Empty;
        public List<DBDataClass> listDBDC;
    }
}
