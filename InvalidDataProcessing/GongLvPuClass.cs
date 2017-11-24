using System;
using System.Collections.Generic;
using System.Text;

using System.Windows.Forms;
using MathWorks.MATLAB.NET.Arrays;
using MathWorks.MATLAB.NET.Utility;
using AccelerateNew;

namespace InvalidDataProcessing
{
    public class GongLvPuClass
    {
        public AccelerateNewClass accelerationCls;

        public GongLvPuClass()
        {
            accelerationCls = new AccelerateNewClass();
        }

        /// <summary>
        /// 计算功率谱
        /// </summary>
        /// <param name="channelName">通道名</param>
        /// <param name="tt">里程信号</param>
        /// <param name="wx">加速度信号：车体或是构架</param>
        /// <param name="Nlen">傅立叶变换窗长，一般取 2的倍数，如 1024；</param>
        /// <param name="dt">时间步长：3/1000；</param>
        /// <returns></returns>
        public List<String> Sub_Fourier_analysis(String channelName,double[] tt, double[] wx, float Nlen, float dt)
        {
            List<String> dataStrList = new List<String>();
            String dataStr = null;

            double[] retVal = new double[2];

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

                    MWNumericArray d_Nlen = new MWNumericArray(Nlen);
                    MWNumericArray d_dt = new MWNumericArray(dt);

                    //调用算法
                    MWNumericArray resultArrayAB = (MWNumericArray)accelerationCls.sub_Fourier_analysis(d_tt, d_wx, d_Nlen, d_dt);
                    double[,] tmpArray = (double[,])resultArrayAB.ToArray();

                    for (int j = 0; j < tmpArray.GetLength(0); j++)
                    {
                        //tmpArray[j, 0] = tmp_tt[(long)(tmpArray[j, 0])];
                        //tmpArray[j, 1] = tmp_tt[(long)(tmpArray[j, 1])];

                        //retValList.Add(Math.Round(tmpArray[0, j], 4));

                        //tmpArray[j, 0] = tmp_tt[(long)(tmpArray[j, 0])];
                        //tmpArray[j, 1] = tmp_tt[(long)(tmpArray[j, 1])];
                        dataStr = String.Format("{0},{1},{2}", channelName, tmpArray[j, 0], tmpArray[j, 1]);
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
