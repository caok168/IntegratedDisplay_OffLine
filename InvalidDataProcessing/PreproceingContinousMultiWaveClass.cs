using System;
using System.Collections.Generic;
using System.Text;

using DataProcess;
using System.Windows.Forms;
using MathWorks.MATLAB.NET.Arrays;
using MathWorks.MATLAB.NET.Utility;
using DataProcessAdvance;
using AccelerateNew;

namespace InvalidDataProcessing
{
    /// <summary>
    /// 数据处理类：计算连续多波指标，并根据其判断轨道几何不良区段
    /// </summary>
    public class PreproceingContinousMultiWaveClass
    {
        public DataProcessAdvanceClass ppmc;
        public AccelerateNewClass ac;

        public PreproceingContinousMultiWaveClass()
        {

            ppmc = new DataProcessAdvanceClass();

            ac = new AccelerateNewClass();
        }

        /// <summary>
        /// 计算连续多波指标，并根据其判断轨道几何不良区段
        /// </summary>
        /// <param name="channelName">中文通道名</param>
        /// <param name="tt">里程信息</param>
        /// <param name="wx">轨道几何不平顺，高低或轨向</param>
        /// <param name="wvelo">速度信息</param>
        /// <param name="wx_gauge">轨距</param>
        /// <param name="thresh_multi_wave">连续多波的个数，取位：3</param>
        /// <param name="thresh_multi_peak">连续多波峰值，取3.0</param>
        /// <returns></returns>
        public List<String> ContinousMultiWavePreprocess(String channelName, double[] tt, double[] wx, double[] wvelo, double[] wx_gauge, double thresh_multi_wave,double thresh_multi_peak)
        {
            List<String> dataStrList = new List<String>();
            String dataStr = null;

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
                    double[] tmp_wvelo = new double[ThisTimeLength];
                    double[] tmp_wx_gauge = new double[ThisTimeLength];

                    for (int j = 0; j < ThisTimeLength; j++)
                    {
                        tmp_tt[j] = tt[index + j];
                        tmp_wx[j] = wx[index + j];
                        tmp_wvelo[j] = wvelo[index + j];
                        tmp_wx_gauge[j] = wx_gauge[index + j];
                    }

                    MWNumericArray d_tt = new MWNumericArray(tmp_tt);
                    MWNumericArray d_wx = new MWNumericArray(tmp_wx);
                    MWNumericArray d_wvelo = new MWNumericArray(tmp_wvelo);
                    MWNumericArray d_wx_gauge = new MWNumericArray(tmp_wx_gauge);
                    MWNumericArray d_thresh_multi_wave = new MWNumericArray(thresh_multi_wave);
                    MWNumericArray d_thresh_multi_peak = new MWNumericArray(thresh_multi_peak);

                    //调用算法
                    MWNumericArray resultArrayAB = (MWNumericArray)ppmc.sub_preprocessing_continous_multi_wave(d_tt, d_wx, d_wvelo, d_wx_gauge, d_thresh_multi_wave, d_thresh_multi_peak);
                    double[,] tmpArray = (double[,])resultArrayAB.ToArray();

                    for (int j = 0; j < tmpArray.GetLength(0); j++)
                    {
                        tmpArray[j, 0] = tmp_tt[(long)(tmpArray[j, 0])];
                        tmpArray[j, 1] = tmp_tt[(long)(tmpArray[j, 1])];
                        dataStr = String.Format("{0},{1},{2},{3}", channelName, tmpArray[j, 0], tmpArray[j, 1], tmpArray[j, 2]);
                        dataStrList.Add(dataStr);
                    }
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Source);
                MessageBox.Show(ex.Message);
            }

            return dataStrList;
        }


        /// <summary>
        /// 计算连续多波指标，车体或是构架加速度
        /// </summary>
        /// <param name="channelName">中文通道名</param>
        /// <param name="tt">里程信息</param>
        /// <param name="wx">输入的加速度信号：构架或是车体</param>
        /// <param name="thresh_multi_wave">连续多波的个数，取位：车体垂-3，车体横-3，构架垂-3，构架横-6</param>
        /// <param name="thresh_multi_peak">连续多波峰值，车体垂-0.05，车体横-0.05，构架垂-0.25，构架横-0.8</param>
        /// <returns></returns>
        public List<String> Sub_preprocessing_continous_multi_wave_on_acc(String channelName, double[] tt, double[] wx,  double thresh_multi_wave, double thresh_multi_peak)
        {
            List<String> dataStrList = new List<String>();
            String dataStr = null;

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
                    double[] tmp_wvelo = new double[ThisTimeLength];
                    double[] tmp_wx_gauge = new double[ThisTimeLength];

                    for (int j = 0; j < ThisTimeLength; j++)
                    {
                        tmp_tt[j] = tt[index + j];
                        tmp_wx[j] = wx[index + j];
                    }

                    MWNumericArray d_tt = new MWNumericArray(tmp_tt);
                    MWNumericArray d_wx = new MWNumericArray(tmp_wx);

                    MWNumericArray d_thresh_multi_wave = new MWNumericArray(thresh_multi_wave);
                    MWNumericArray d_thresh_multi_peak = new MWNumericArray(thresh_multi_peak);

                    //调用算法
                    MWNumericArray resultArrayAB = (MWNumericArray)ac.sub_preprocessing_continous_multi_wave_on_acc(d_tt, d_wx, d_thresh_multi_wave, d_thresh_multi_peak);
                    double[,] tmpArray = (double[,])resultArrayAB.ToArray();

                    for (int j = 0; j < tmpArray.GetLength(0); j++)
                    {
                        tmpArray[j, 0] = tmp_tt[(long)(tmpArray[j, 0])];
                        tmpArray[j, 1] = tmp_tt[(long)(tmpArray[j, 1])];
                        dataStr = String.Format("{0},{1},{2},{3}", channelName, tmpArray[j, 0], tmpArray[j, 1], tmpArray[j, 2]);
                        dataStrList.Add(dataStr);
                    }
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Source);
                MessageBox.Show(ex.Message);
            }

            return dataStrList;
        }
    }
}
