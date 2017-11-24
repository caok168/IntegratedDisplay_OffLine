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
    /// 数据处理类：计算峰峰值指标
    /// </summary>
    public class PreproceingDeviationClass
    {
        public DataProcessAdvanceClass ppmc;

        public AccelerateNewClass ac;

        public PreproceingDeviationClass()
        {

            ppmc = new DataProcessAdvanceClass();

            ac = new AccelerateNewClass();
        }


        /// <summary>
        /// 计算峰峰值指标，并根据其判断轨道几何不良区段
        /// </summary>
        /// <param name="channelName">通道名中文名</param>
        /// <param name="tt">里程信息</param>
        /// <param name="wx">轨道几何不平顺，高低或是轨向</param>
        /// <param name="wvelo">速度</param>
        /// <param name="wx_gauge">轨距</param>
        /// <param name="thresh_gauge">峰峰值的阈值，取8.0</param>
        /// <returns></returns>
        public List<String> WideGaugePreProcess(String channelName, double[] tt, double[] wx, double[] wvelo, double[] wx_gauge, double thresh_gauge)
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
                    MWNumericArray d_thresh_gauge = new MWNumericArray(thresh_gauge);


                    //调用算法
                    MWNumericArray resultArrayAB = (MWNumericArray)ppmc.sub_preprocessing_deviation_by_p2p(d_tt, d_wx, d_wvelo, d_wx_gauge, d_thresh_gauge);
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
        /// 计算峰峰值指标，车体或是构架加速度
        /// </summary>
        /// <param name="channelName">通道名中文名</param>
        /// <param name="tt">里程信息</param>
        /// <param name="wx">加速度滤波后的通道：车体垂，车体横，构架垂，构架横</param>
        /// <param name="thresh_p2p">峰峰值的阈值：车体垂-0.12g，车体横-0.08g，构架垂-0.5g，构架横-0.8g</param>
        /// <returns></returns>
        public List<String> Sub_preprocessing_deviation_by_p2p_on_acc(String channelName, double[] tt, double[] wx, double thresh_p2p)
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


                    for (int j = 0; j < ThisTimeLength; j++)
                    {
                        tmp_tt[j] = tt[index + j];
                        tmp_wx[j] = wx[index + j];
                    }

                    MWNumericArray d_tt = new MWNumericArray(tmp_tt);
                    MWNumericArray d_wx = new MWNumericArray(tmp_wx);
                    MWNumericArray d_thresh_gauge = new MWNumericArray(thresh_p2p);


                    //调用算法
                    MWNumericArray resultArrayAB = (MWNumericArray)ac.sub_preprocessing_deviation_by_p2p_on_acc(d_tt, d_wx, thresh_p2p);
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
