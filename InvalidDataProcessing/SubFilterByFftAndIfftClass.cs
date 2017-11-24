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
    /// 滤波：车体，架构采样后的滤波
    /// </summary>
    public class SubFilterByFftAndIfftClass
    {
        public AccelerateNewClass accelerationCls;

        public SubFilterByFftAndIfftClass()
        {
            accelerationCls = new AccelerateNewClass();
        }



        #region 对信号进行滤波：车体，架构采样后的滤波
        /// <summary>
        /// 对信号进行滤波：车体，架构采样后的滤波
        /// </summary>
        /// <param name="wx">原始信号：车体垂，车体横，架构垂，架构横</param>
        /// <param name="tt">里程</param>
        /// <param name="Fs">采样频率：2000/6</param>
        /// <param name="Freq_L">滤波下限频率</param>
        /// <param name="Freq_H">滤波上限频率</param>
        /// <returns>滤波后的信号</returns>
        public double[] Sub_filter_by_fft_and_ifft(double[] wx, double[] tt, double Fs, double Freq_L, double Freq_H)
        {
            double[] retVal = new double[wx.Length];

            List<double> retValList = new List<double>();

            try
            {
                int oneTimeLength = 1000000; //一次处理的点数

                for (int i = 0; i < tt.Length; i += oneTimeLength)
                {
                    int remain = 0;
                    int index = (i / oneTimeLength) * oneTimeLength;
                    remain = tt.Length - oneTimeLength * (i / oneTimeLength + 1);
                    int ThisTimeLength = remain > 0 ? oneTimeLength : (remain += oneTimeLength);
                    double[] tmp_tt = new double[ThisTimeLength];
                    double[] tmp_wx = new double[ThisTimeLength];


                    for (int j = 0; j < ThisTimeLength; j++)
                    {
                        tmp_tt[j] = tt[index + j];
                        tmp_wx[j] = wx[index + j];
                    }

                    MWNumericArray d_tt = new MWNumericArray(tmp_tt);
                    MWNumericArray d_wx = new MWNumericArray(tmp_wx);
                    MWNumericArray d_Fs = new MWNumericArray(Fs);
                    MWNumericArray d_Frep_L = new MWNumericArray(Freq_L);
                    MWNumericArray d_Frep_H = new MWNumericArray(Freq_H);

                    //调用算法
                    MWNumericArray resultArrayAB = (MWNumericArray)accelerationCls.sub_filter_by_fft_and_ifft(d_wx, d_tt,d_Fs,d_Frep_L,d_Frep_H);
                    double[,] tmpArray = (double[,])resultArrayAB.ToArray();

                    for (int j = 0; j < tmpArray.GetLength(1); j++)
                    {
                        //tmpArray[j, 0] = tmp_tt[(long)(tmpArray[j, 0])];
                        //tmpArray[j, 1] = tmp_tt[(long)(tmpArray[j, 1])];

                        retValList.Add(Math.Round(tmpArray[0, j],4));
                    }
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Source);
                MessageBox.Show(ex.Message);
            }


            return retValList.ToArray();
        }
        #endregion
    }
}
