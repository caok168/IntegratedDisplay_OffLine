using System;
using System.Collections.Generic;
using System.Text;

namespace InvalidDataProcessing.DataClass
{
    public class ChangeDetectionClass
    {
        /// <summary>
        /// 序号
        /// </summary>
        public int id;
        /// <summary>
        /// 第一层文件的起始指针
        /// </summary>
        public int startPos;
        /// <summary>
        /// 第一层文件的起始里程
        /// </summary>
        public double startMile;
        /// <summary>
        /// 第一层文件的结束指针
        /// </summary>
        public int endPos;
        /// <summary>
        /// 第一层文件的结束里程
        /// </summary>
        public double endMile;
        /// <summary>
        /// 幅值差
        /// </summary>
        public double absOfMax;
    }
}
