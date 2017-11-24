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
    public class CalEnergyWeightClass
    {
        public DataProcessAdvanceClass ppmc;

        public CalEnergyWeightClass()
        {
            ppmc = new DataProcessAdvanceClass();
        }


        /// <summary>
        /// 计算能量权系数
        /// </summary>
        /// <param name="channelName"></param>
        /// <param name="tt">里程</param>
        /// <param name="wx_L">左高低或是左轨向</param>
        /// <param name="wx_R">右高低或是右轨向</param>
        /// <param name="wvelo">速度</param>
        /// <param name="wy">车体垂向加速度或是车体横向加速度</param>
        /// <param name="velo_band">速度带宽(默认10)</param>
        /// <returns></returns>
        public List<List<double>> CalEnergyWeightProcess(String channelName, double[] tt, double[] wx_L, double[] wx_R, double[] wvelo, double[] wy, double velo_band)
        {
            List<List<double>> retVal = new List<List<double>>();
            List<double> tmpList= new List<double>();

            try
            {
                int oneTimeLength = 1000000; //一次处理的点数
                oneTimeLength = tt.Length;

                for (int i = 0; i < tt.Length; i += oneTimeLength)
                {
                    int remain = 0;
                    int index = (i / oneTimeLength) * oneTimeLength;
                    remain = tt.Length - oneTimeLength * (i / oneTimeLength + 1);
                    int ThisTimeLength = remain > 0 ? oneTimeLength : (remain += oneTimeLength);
                    double[] tmp_tt = new double[ThisTimeLength];
                    double[] tmp_wx_L = new double[ThisTimeLength];
                    double[] tmp_wvelo = new double[ThisTimeLength];
                    double[] tmp_wx_R = new double[ThisTimeLength];
                    double[] tmp_wy = new double[ThisTimeLength];

                    for (int j = 0; j < ThisTimeLength; j++)
                    {
                        tmp_tt[j] = tt[index + j];
                        tmp_wx_L[j] = wx_L[index + j];
                        tmp_wvelo[j] = wvelo[index + j];
                        tmp_wx_R[j] = wx_R[index + j];
                        tmp_wy[j] = wy[index + j];
                    }

                    MWNumericArray d_tt = new MWNumericArray(tmp_tt);
                    MWNumericArray d_wx_L = new MWNumericArray(tmp_wx_L);
                    MWNumericArray d_wvelo = new MWNumericArray(tmp_wvelo);
                    MWNumericArray d_wx_R = new MWNumericArray(tmp_wx_R);
                    MWNumericArray d_wy = new MWNumericArray(tmp_wy);
                    MWNumericArray d_velo_band = new MWNumericArray(velo_band);


                    //调用算法
                    MWArray[] resultArrayAB = ppmc.sub_calculate_prof_energy_weight_coefficient(4,d_wx_L, d_wx_R, d_wy, d_wvelo,d_tt,d_velo_band);
                    double[,] tmpArray = (double[,])resultArrayAB[0].ToArray();
                    for (int j = 0; j < tmpArray.GetLength(0); j++)
                    {
                        tmpList.Add(tmpArray[j, 0]);
                    }
                    retVal.Add(tmpList);
                    

                    tmpArray = (double[,])resultArrayAB[1].ToArray();
                    tmpList = new List<double>();
                    for (int j = 0; j < tmpArray.GetLength(0); j++)
                    {
                        tmpList.Add(tmpArray[j, 0]);
                    }
                    retVal.Add(tmpList);


                    tmpArray = (double[,])resultArrayAB[2].ToArray();
                    tmpList = new List<double>();
                    for (int j = 0; j < tmpArray.GetLength(0); j++)
                    {
                        tmpList.Add(tmpArray[j, 0]);
                    }
                    retVal.Add(tmpList);

                    tmpArray = (double[,])resultArrayAB[3].ToArray();
                    tmpList = new List<double>();
                    for (int j = 0; j < tmpArray.GetLength(0); j++)
                    {
                        tmpList.Add(tmpArray[j, 0]);
                    }
                    retVal.Add(tmpList);

                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Source);
                MessageBox.Show(ex.Message);
            }


            return retVal;
        }
    }
}
