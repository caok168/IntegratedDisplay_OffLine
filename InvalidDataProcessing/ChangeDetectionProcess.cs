using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

using DataProcess;

using MathWorks.MATLAB.NET.Arrays;
using MathWorks.MATLAB.NET.Utility;
using DataProcessAdvance;

namespace InvalidDataProcessing
{
    /// <summary>
    /// 数据处理类：微小变化自动识别
    /// </summary>
    public class ChangeDetectionProcess
    {

        public DataProcessAdvanceClass ppmc;

        public ChangeDetectionProcess()
        {
            try
            {
                ppmc = new DataProcessAdvanceClass();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
                MessageBox.Show(ex.Source);
                MessageBox.Show(ex.StackTrace);
                MessageBox.Show("hahha");
            }
            
        }



        

        #region 变化智能识别--调用matlab算法
        /// <summary>
        /// 变化智能识别--调用matlab算法
        /// 几何不平顺目前包括左右高低，左右轨向
        /// </summary>
        /// <param name="channelName">中文通道名</param>
        /// <param name="tt_1">第1组数的里程</param>
        /// <param name="wx_1">第1组数的几何不平顺</param>
        /// <param name="wvelo_1">第1组数的速度</param>
        /// <param name="wx_gauge_1">第1组数的轨距</param>
        /// <param name="tt_2">第2组数的里程</param>
        /// <param name="wx_2">第2组数的几何不平顺</param>
        /// <param name="wvelo_2">第2组数的速度</param>
        /// <param name="wx_gauge_2">第2组数的轨距</param>
        /// <returns></returns>
        public List<String> ChangeDetectionPrcs(String channelName, double[] tt_1, double[] wx_1, double[] wvelo_1, double[] wx_gauge_1, double[] tt_2, double[] wx_2, double[] wvelo_2, double[] wx_gauge_2)
        {
            List<String> dataStrList = new List<String>();
            String dataStr = null;

            try
            {
                int oneTimeLength = 600000; //一次处理的点数,150公里
                int len1 = Math.Min(tt_1.Length, wx_1.Length);
                int len2 = Math.Min(tt_2.Length, wx_2.Length);
                int len = Math.Min(len1, len2);

                for (int i = 0; i < len; i += oneTimeLength)
                {
                    int remain = 0;
                    int index = (i / oneTimeLength) * oneTimeLength;
                    remain = len - oneTimeLength * (i / oneTimeLength + 1);
                    int ThisTimeLength = remain > 0 ? oneTimeLength : (remain += oneTimeLength);
                    double[] tmp_tt_1 = new double[ThisTimeLength];
                    double[] tmp_wx_1 = new double[ThisTimeLength];
                    double[] tmp_wvelo_1 = new double[ThisTimeLength];
                    double[] tmp_wx_gauge_1 = new double[ThisTimeLength];

                    double[] tmp_tt_2 = new double[ThisTimeLength];
                    double[] tmp_wx_2 = new double[ThisTimeLength];
                    double[] tmp_wvelo_2 = new double[ThisTimeLength];
                    double[] tmp_wx_gauge_2 = new double[ThisTimeLength];

                    for (int j = 0; j < ThisTimeLength; j++)
                    {
                        tmp_tt_1[j] = tt_1[index + j];
                        tmp_wx_1[j] = wx_1[index + j];
                        tmp_wvelo_1[j] = wvelo_1[index + j];
                        tmp_wx_gauge_1[j] = wx_gauge_1[index + j];

                        tmp_tt_2[j] = tt_2[index + j];
                        tmp_wx_2[j] = wx_2[index + j];
                        tmp_wvelo_2[j] = wvelo_2[index + j];
                        tmp_wx_gauge_2[j] = wx_gauge_2[index + j];

                    }

                    MWNumericArray d_tt_1 = new MWNumericArray(tmp_tt_1);
                    MWNumericArray d_wx_1 = new MWNumericArray(tmp_wx_1);
                    MWNumericArray d_wvelo_1 = new MWNumericArray(tmp_wvelo_1);
                    MWNumericArray d_wv_gauge_1 = new MWNumericArray(tmp_wx_gauge_1);

                    MWNumericArray d_tt_2 = new MWNumericArray(tmp_tt_2);
                    MWNumericArray d_wx_2 = new MWNumericArray(tmp_wx_2);
                    MWNumericArray d_wvelo_2 = new MWNumericArray(tmp_wvelo_2);
                    MWNumericArray d_wv_gauge_2 = new MWNumericArray(tmp_wx_gauge_2);


                    //调用算法
                    MWNumericArray resultArrayAB = (MWNumericArray)ppmc.sub_abrupt_change_detection(d_tt_1, d_wx_1, d_wvelo_1, d_wv_gauge_1, d_tt_2, d_wx_2, d_wvelo_2, d_wv_gauge_2);
                    double[,] tmpArray = (double[,])resultArrayAB.ToArray();

                    for (int j = 0; j < tmpArray.GetLength(0); j++)
                    {
                        tmpArray[j, 0] = tmp_tt_2[(long)(tmpArray[j, 0])];
                        tmpArray[j, 1] = tmp_tt_2[(long)(tmpArray[j, 1])];
                        dataStr = String.Format("{0},{1},{2},{3}",channelName, tmpArray[j, 0], tmpArray[j, 1], tmpArray[j, 2]);
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
        #endregion
    }
}
