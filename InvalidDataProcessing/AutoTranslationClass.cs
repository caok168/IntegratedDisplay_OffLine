using System;
using System.Collections.Generic;
using System.Text;

using DataProcess;
using System.Windows.Forms;
using MathWorks.MATLAB.NET.Arrays;
using MathWorks.MATLAB.NET.Utility;
using DataProcessAdvance;

namespace InvalidDataProcessing
{
    /// <summary>
    /// 自动对齐功能类
    /// </summary>
    public class AutoTranslationClass
    {
        public DataProcessAdvanceClass ppmc;

        public AutoTranslationClass()
        {
            ppmc = new DataProcessAdvanceClass();
        }

        #region 波形自动对齐
        /// <summary>
        /// 自动对齐
        /// </summary>
        /// <param name="ww">输入参考波形1的数据</param>
        ///WW(1,:)：左高低
        ///WW(2,:)：右高低
        ///WW(3,:)：左轨向
        ///WW(4,:)：右轨向
        ///WW(5,:)：轨距------(总共5列数据)
        /// <param name="ww1">参考波形2的数据</param>
        /// <returns>移动的点数</returns>
        /// 备注：波形1不动，波形2减去点数
        ///       波形2不动，波形1加上点数
        public int AutoTranslation(float[][] ww, float[][] ww1)
        {
            int retVal = 0;

            int rowLen_ww = ww.GetLength(0);
            int colLen_ww = ww[0].Length;
            MWNumericArray d_ww = new MWNumericArray(MWArrayComplexity.Real,rowLen_ww,colLen_ww);
            //matlab中矩阵的下标是从1开始的，而C#是从0开始的
            for (int i = 0; i < rowLen_ww; i++)
            {
                for (int j = 0; j < colLen_ww;j++ )
                {
                    d_ww[i + 1, j + 1] = ww[i][j];
                }
            }

            int rowLen_ww1 = ww1.GetLength(0);
            int colLen_ww1 = ww1[0].Length;
            MWNumericArray d_ww1 = new MWNumericArray(MWArrayComplexity.Real, rowLen_ww1, colLen_ww1);
            //matlab中矩阵的下标是从1开始的，而C#是从0开始的
            for (int i = 0; i < rowLen_ww1; i++)
            {
                for (int j = 0; j < colLen_ww1; j++)
                {
                    d_ww1[i + 1, j + 1] = ww1[i][j];
                }
            }

            try
            {
                //调用算法
                MWArray resultArray = ppmc.sub_preprocessing_alignment_data(d_ww, d_ww1);
                //MWArray resultArray = null;
                double[,] tmpArray = (double[,])(resultArray.ToArray());

                retVal = (int)tmpArray[0,0];
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


            return retVal;
        }
        #endregion
    }
}
