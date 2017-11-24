using System;
using System.Collections.Generic;
using System.Text;

using System.Windows.Forms;
using MathWorks.MATLAB.NET.Arrays;
using MathWorks.MATLAB.NET.Utility;
using AccelerateNew;

namespace InvalidDataProcessing
{
    /// <summary>
    /// 计算轴箱加速度移动有效值
    /// </summary>
    class CalculateMovingRmsClass
    {
        public AccelerateNewClass accelerationCls;

        public CalculateMovingRmsClass()
        {
            accelerationCls = new AccelerateNewClass();
        }

    }
}
