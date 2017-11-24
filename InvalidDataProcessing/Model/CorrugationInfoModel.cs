using System;
using System.Collections.Generic;
using System.Text;

namespace InvalidDataProcessing.Model
{
    public class CorrugationInfoModel
    {
        /// <summary>
        /// 波磨区段开始里程
        /// </summary>
        public string StartMile { get; set; }

        /// <summary>
        /// 波磨区段结束里程
        /// </summary>
        public string EndMile { get; set; }

        /// <summary>
        /// 波磨有效值
        /// </summary>
        public string RmsValue { get; set; }

        /// <summary>
        /// 波磨指数
        /// </summary>
        public string PeakValue { get; set; }

        /// <summary>
        /// 波磨区段平均速度
        /// </summary>
        public string AvgSpeed { get; set; }

        /// <summary>
        /// 第1主频
        /// </summary>
        public string FirstBasicFrequency { get; set; }

        /// <summary>
        /// 波长
        /// </summary>
        public string WaveLength { get; set; }

        /// <summary>
        /// 能量集中率
        /// </summary>
        public string EnergyRatio { get; set; }

    }
}
