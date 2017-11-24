using System;
using System.Collections.Generic;
using System.Text;

namespace InvalidDataProcessing.Model
{
    /// <summary>
    /// 焊接接头结果实体
    /// </summary>
    public class WeldModel
    {
        /// <summary>
        /// 里程
        /// </summary>
        public string Mile { get; set; }
        /// <summary>
        /// 速度
        /// </summary>
        public string Speed { get; set; }
        /// <summary>
        /// 有效值
        /// </summary>
        public string RmsValue { get; set; }
        /// <summary>
        /// 冲击指数
        /// </summary>
        public string PeakValue { get; set; }
        /// <summary>
        /// 采样编号
        /// </summary>
        public string SampleNo { get; set; }
    }
}
