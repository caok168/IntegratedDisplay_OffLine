using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace InGraph.Classes
{
    /// <summary>
    /// 通道相关类，包含通道的配置信息和该通道的所有采样点的信息
    /// </summary>
    class ChannelsClass
    {
        #region 通道序号
        /// <summary>
        /// 通道序号
        /// </summary>
        public int Id = -1;
        #endregion
        #region 原始通道名
        /// <summary>
        /// 原始通道名
        /// </summary>
        public string Name = string.Empty;
        #endregion
        #region 通道英文名---和原始通道名一样
        /// <summary>
        /// 通道英文名---和原始通道名一样
        /// </summary>
        public string NonChineseName = string.Empty;
        #endregion
        #region 通道中文名
        /// <summary>
        /// 通道中文名
        /// </summary>
        public string ChineseName = string.Empty;
        #endregion
        #region 通道颜色
        /// <summary>
        /// 通道颜色
        /// </summary>
        public int Color = 0;
        #endregion
        #region 通道是否显示
        /// <summary>
        /// 通道是否显示
        /// </summary>
        public bool Visible = true;
        #endregion
        #region 显示比例
        /// <summary>
        /// 显示比例
        /// </summary>
        public float ZoomIn = 1.0f;
        #endregion
        #region 通道单位
        /// <summary>
        /// 通道单位
        /// </summary>
        public string Units = string.Empty;
        #endregion
        #region 是否包含偏移量
        /// <summary>
        /// 是否包含偏移量
        /// </summary>
        public bool MeaOffset = false;
        #endregion
        #region 通道基线位置(波形显示时的垂直方向)
        /// <summary>
        /// 通道基线位置(波形显示时的垂直方向)
        /// </summary>
        public int Location = 1;
        #endregion
        #region 通道波形的线宽
        /// <summary>
        /// 通道波形的线宽
        /// </summary>
        public float fLineWidth = 1.0f;
        #endregion
        #region 是否反转
        /// <summary>
        /// 是否反转
        /// </summary>
        public bool bReverse = false;
        #endregion

        #region 通道拖拽区---通道信息显示--右边的矩形框
        /// <summary>
        /// 通道拖拽区---通道信息显示--右边的矩形框
        /// </summary>
        public Rectangle Rect { set; get; }
        #endregion

        #region 该通道所有的数据--原始数据，直接从cit文件读取，没做处理
        /// <summary>
        /// 该通道所有的数据--原始数据，直接从cit文件读取，没做处理
        /// </summary>
        public float[] Data;
        #endregion

    }
}
