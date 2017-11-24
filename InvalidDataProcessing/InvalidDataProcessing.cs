using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Data.OleDb;
using System.Text;
using System.Windows.Forms;
using System.IO;
using DataProcess;

using MathWorks.MATLAB.NET.Arrays;
using MathWorks.MATLAB.NET.Utility;
using DataProcessAdvance;

namespace InvalidDataProcessing
{
    public class DataProcessing
    {
        public  string sLastSelectPath = "";
        public CITDataProcess cdp;
        public WaveformDataProcess wdp;
        public DataProcessAdvanceClass ppmc;


        public DataProcessing()
        {

            ppmc = new DataProcessAdvanceClass();

            cdp = new CITDataProcess();
            wdp = new WaveformDataProcess();
        }


        public class PointIDX
        {
            public double s = 0;
            public double e = 0;
            public int type = 0;

            public  new int GetType()
            {
                int retVal = 6;//其他
                if (type == 0)
                {  //速度偏低
                    retVal = 3;//对应于innerdb里的无效区段类型
                }
                if (type == 1)
                {  //分布异常---阳光干扰
                    retVal = 1;
                }
                if (type == 2)
                {  //局部毛刺
                    retVal = 8;
                }
                if (type == 3)
                {  //轨距加宽--加宽道岔
                    retVal = 5;
                }
                if (type == 5)
                {  //单边轨距拉直线
                    retVal = 11;
                }

                return retVal;
            }
        }

        #region 变化智能识别信息
        /// <summary>
        /// 变化只能识别信息
        /// </summary>
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
        #endregion
        

       

        #region 接口函数：无效数据滤除---处理多个通道数据
        /// <summary>
        /// 接口函数：无效数据滤除---处理多个通道数据
        /// </summary>
        /// <param name="FileName"></param>
        /// <param name="sAddFileName"></param>
        /// <returns></returns>
        public bool GetDataInfoMulti(string FileName, string sAddFileName)
        {
            string EnName = "";
            string CnName = "速度";
            try
            {
                //根据信息读取里程及数据信息
                cdp.QueryDataInfoHead(FileName);
                //double[] tt0 = CommonClassIDP.cdp.GetSingleChannelData(FileName, GetChannelNumberByChannelName(FileName, "Km", "公里"));

                double[] tt = cdp.GetMilesDataDouble(FileName);

                double[] wvelo = cdp.GetSingleChannelData(FileName, GetChannelNumberByChannelName(FileName, "Speed", "速度"));
                //double[] Short_Twist = CommonClassIDP.cdp.GetSingleChannelData(FileName, GetChannelNumberByChannelName(FileName, "Short_Twist", "三角坑"));
                double[] L_Prof_SC = cdp.GetSingleChannelData(FileName, GetChannelNumberByChannelName(FileName, "L_Prof_SC", "左高低_中波"));
                double[] R_Prof_SC = cdp.GetSingleChannelData(FileName, GetChannelNumberByChannelName(FileName, "R_Prof_SC", "右高低_中波"));
                double[] L_Align_SC = cdp.GetSingleChannelData(FileName, GetChannelNumberByChannelName(FileName, "L_Align_SC", "左轨向_中波"));
                double[] R_Align_SC = cdp.GetSingleChannelData(FileName, GetChannelNumberByChannelName(FileName, "R_Align_SC", "右轨向_中波"));
                double[] Gage = cdp.GetSingleChannelData(FileName, GetChannelNumberByChannelName(FileName, "Gage", "轨距"));
                double[] Crosslevel = cdp.GetSingleChannelData(FileName, GetChannelNumberByChannelName(FileName, "Crosslevel", "水平"));

                int tmpChannelNumber = GetChannelNumberByChannelName(FileName, "Gage_L", "单边轨距左");
                double[] Gage_L = null;
                if (tmpChannelNumber == -1)
                {
                    Gage_L = new double[wvelo.Length];
                } 
                else
                {
                    Gage_L = cdp.GetSingleChannelData(FileName, tmpChannelNumber);
                }

                tmpChannelNumber = GetChannelNumberByChannelName(FileName, "Gage_R", "单边轨距右");
                double[] Gage_R = null;
                if (tmpChannelNumber == -1)
                {
                    Gage_R = new double[wvelo.Length];
                }
                else
                {
                    Gage_R = cdp.GetSingleChannelData(FileName, tmpChannelNumber);
                }


                //调用刘博士的算法---处理多个通道
                preProcess(tt, L_Prof_SC,R_Prof_SC,L_Align_SC,R_Align_SC,Gage,Crosslevel, wvelo,Gage_L,Gage_R, FileName, sAddFileName, "自动标识");


                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.StackTrace);
                MessageBox.Show(ex.Source);
                MessageBox.Show(ex.Message);
            }
            finally
            {
            }
            return true;
        }
        #endregion

        #region 无效数据滤除--处理多个通道
        /// <summary>
        /// 无效数据滤除--处理多个通道
        /// </summary>
        /// <param name="tt">里程数组：公里</param>
        /// <param name="wx_prof_L">轨道左高低不平顺</param>
        /// <param name="wx_prof_R">轨道右高低不平顺</param>
        /// <param name="wx_align_L">轨道左轨向不平顺</param>
        /// <param name="wx_align_R">轨道右轨向不平顺</param>
        /// <param name="wx_gauge">轨距</param>
        /// <param name="wx_level">水平</param>
        /// <param name="wvelo">速度：km/h</param>
        /// <param name="wgauge_L">单边轨距左</param>
        /// <param name="wgauge_R">单边轨距右</param>
        /// <param name="FileName">cit文件名</param>
        /// <param name="sAddFileName">idf文件名</param>
        /// <param name="swx">英文通道名：这里统一写成"自动标识"</param>
        /// <returns>结果</returns>
        private bool preProcess(double[] tt, double[] wx_prof_L, double[] wx_prof_R, double[] wx_align_L, double[] wx_align_R, double[] wx_gauge, double[] wx_level, double[] wvelo,double[] wgauge_L,double[] wgauge_R, string FileName, string sAddFileName, string swx)
        {
            try
            {
                List<PointIDX> Lidx = new List<PointIDX>();
                //调用刘博士的算法，得到索引数组idx
                int oneTimeLength = 1000000; //一次处理的点数

                for (int i = 0; i < tt.Length; i += oneTimeLength)
                {
                    int remain = 0;
                    int index = (i / oneTimeLength) * oneTimeLength;
                    remain = tt.Length - oneTimeLength * (i / oneTimeLength + 1);
                    int ThisTimeLength = remain > 0 ? oneTimeLength : (remain += oneTimeLength);
                    double[] tmp_tt = new double[ThisTimeLength];
                    double[] tmp_wx_prof_L = new double[ThisTimeLength];
                    double[] tmp_wx_prof_R = new double[ThisTimeLength];
                    double[] tmp_wx_align_L = new double[ThisTimeLength];
                    double[] tmp_wx_align_R = new double[ThisTimeLength];
                    double[] tmp_wx_gauge = new double[ThisTimeLength];
                    double[] tmp_wx_level = new double[ThisTimeLength];
                    double[] tmp_wvelo = new double[ThisTimeLength];

                    double[] tmp_wgauge_L = new double[ThisTimeLength];
                    double[] tmp_wgauge_R = new double[ThisTimeLength];

                    for (int j = 0; j < ThisTimeLength; j++)
                    {
                        tmp_tt[j] = tt[index + j];
                        tmp_wx_prof_L[j] = wx_prof_L[index + j];
                        tmp_wx_prof_R[j] = wx_prof_R[index + j];
                        tmp_wx_align_L[j] = wx_align_L[index + j];
                        tmp_wx_align_R[j] = wx_align_R[index + j];
                        tmp_wx_gauge[j] = wx_gauge[index + j];
                        tmp_wx_level[j] = wx_level[index + j];
                        tmp_wvelo[j] = wvelo[index + j];

                        tmp_wgauge_L[j] = wgauge_L[index + j];
                        tmp_wgauge_R[j] = wgauge_R[index + j];
                    }

                    MWNumericArray d_tt = new MWNumericArray(tmp_tt);
                    MWNumericArray d_wx_prof_L = new MWNumericArray(tmp_wx_prof_L);
                    MWNumericArray d_wx_prof_R = new MWNumericArray(tmp_wx_prof_R);
                    MWNumericArray d_wx_align_L = new MWNumericArray(tmp_wx_align_L);
                    MWNumericArray d_wx_align_R = new MWNumericArray(tmp_wx_align_R);
                    MWNumericArray d_wx_gauge = new MWNumericArray(tmp_wx_gauge);
                    MWNumericArray d_wx_level = new MWNumericArray(tmp_wx_level);
                    MWNumericArray d_wvelo = new MWNumericArray(tmp_wvelo);

                    MWNumericArray d_wgauge_L = new MWNumericArray(tmp_wgauge_L);
                    MWNumericArray d_wgauge_R = new MWNumericArray(tmp_wgauge_R);

                    //调用算法
                    MWNumericArray resultArrayAB = (MWNumericArray)ppmc.sub_identify_abnormal_point(d_tt, d_wx_prof_L, d_wx_prof_R, d_wx_align_L, d_wx_align_R, d_wx_gauge, d_wx_level, d_wvelo, d_wgauge_L, d_wgauge_R);
                    double[,] result = (double[,])resultArrayAB.ToArray();
                    if (result.GetLength(1) == 0) continue;
                    Lidx.Clear();
                    for (int m = 0; m < result.GetLength(0); m++)
                    {
                        PointIDX pi = new PointIDX();
                        pi.s = result[m, 0] + index;
                        pi.e = result[m, 1] + index;
                        pi.type = (int)(result[m, 2]);
                        Lidx.Add(pi);
                    }

                    //按对处理索引数组
                    List<PointIDX>.Enumerator listCredentials = Lidx.GetEnumerator();

                    try
                    {
                        using (OleDbConnection sqlconn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + sAddFileName + ";Persist Security Info=True"))
                        {
                            sqlconn.Open();
                            OleDbCommand sqlcom;

                            string sSql = null;

                            int id = 0;//无效区段id
                            sSql = "select max(Id) from InvalidData";
                            sqlcom = new OleDbCommand(sSql, sqlconn);
                            OleDbDataReader oledbReader = sqlcom.ExecuteReader();
                            Boolean isNull = oledbReader.HasRows;//是否是第一条记录，第一条记录id为1；
                            if (isNull == false)
                            {
                                id = 1;
                            }
                            else
                            {
                                while (oledbReader.Read())
                                {
                                    if (String.IsNullOrEmpty(oledbReader.GetValue(0).ToString()))
                                    {
                                        id = 1;
                                    }
                                    else
                                    {
                                        id = int.Parse(oledbReader.GetValue(0).ToString()) + 1;
                                    }
                                    
                                }
                            }

                            while (listCredentials.MoveNext())
                            {
                                //根据索引值获取对应的文件指针。
                                double sPox = cdp.GetPosByIdx(FileName, listCredentials.Current.s);
                                double ePox = cdp.GetPosByIdx(FileName, listCredentials.Current.e);
                                //根据文件指针，获取里程信息。
                                double smile = cdp.GetMileByPos(sAddFileName, FileName, sPox);
                                double emile = cdp.GetMileByPos(sAddFileName, FileName, ePox);
                                int type = listCredentials.Current.GetType();

                                sSql = "insert into InvalidData values(" + (id++).ToString() + ",'" + sPox.ToString() +
    "','" + ePox.ToString() + "','" + smile.ToString() + "','" + emile.ToString() + "',"+type+",'无效数据',0,'" + swx + "')";

                                //插入数据库
                                sqlcom = new OleDbCommand(sSql, sqlconn);
                                sqlcom.ExecuteNonQuery();
                            }

                            sqlconn.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("无效区段设置异常:" + ex.Message);
                    }
                }
                //InfoLabel2.Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(swx + "通道处理出错");
                MessageBox.Show(ex.Source);
                MessageBox.Show(ex.StackTrace);
                MessageBox.Show(ex.Message);
                return false;
            }
            return true;
        }
        #endregion

        private string toKM(double m)
        {
            string retStr = "";
            string km = null;
            float meter = 0f;
            string[] ms = m.ToString().Split(new char[] { '.' });
            if (ms.Length == 1)
            {
                km = ms[0];
                meter = 0f;

                retStr = "K" + km + "+" + meter;
            } 
            else
            {
                km = ms[0];
                meter = float.Parse("0." + ms[1]) * 1000;

                retStr = "K" + km + "+" + meter;
            }

            return retStr;
        }
        private double[,] test(double[] a)
        {
            double[,] idx = new double[2, 2];
            idx[0, 0] = 100000;
            idx[1, 0] = 200000;
            idx[0, 1] = 300000;
            idx[1, 1] = 400000;

            return idx;
        }

        public int GetChannelNumberByChannelName(string FileName, string EnName, string CnName)
        {
            List<CITDataProcess.DataChannelInfo> dci = cdp.GetDataChannelInfoHeadNew(FileName);
            int ChannelNumber = -1;
            for (int i = 0; i < dci.Count; i++)
            {
                if ((dci[i].sNameEn.Contains(EnName) && (EnName != "")) || (dci[i].sNameCh.Contains(CnName)) && (CnName != ""))
                {
                    ChannelNumber = i + 1;
                    break;
                }
            }
            return ChannelNumber;
        }

    }
}
