using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

using DataProcess;
using InGraph.Classes;
using InvalidDataProcessing;
using System.Data.OleDb;

namespace InGraph.Forms
{
    public partial class ChangeDetectionForm : Form
    {
        List<IndexStaClass> indexStaClsList_0 = null;
        List<IndexStaClass> indexStaClsList_1 = null;
        double startMeter_0 = 0;
        double endMeter_0 = 0;
        double startMeter_1 = 0;
        double endMeter_1 = 0;
        double startMile = 0;
        double endMile = 0;
        MilePosClass milePosClsStart_0;
        MilePosClass milePosClsEnd_0;
        MilePosClass milePosClsStart_1;
        MilePosClass milePosClsEnd_1;
        List<ChangeDetectionClass> changeDetectionClsList = new List<ChangeDetectionClass>();

        CITDataProcess cdp;
        DataProcessing dataProcessing;
        ChangeDetectionProcess changeDetcPro;
        PreproceingDeviationClass pdc;
        PreproceingContinousMultiWaveClass pcmw;
        WaveformDataProcess wdp;
        

        List<String> dataStrList = null;
        Boolean isReady = true;//是否计算微小变化

        int id = 0;//存放微小变化在idf中id号



        public ChangeDetectionForm()
        {
            InitializeComponent();

            try
            {
                cdp = new CITDataProcess();
                dataProcessing = new DataProcessing();

                changeDetcPro = new ChangeDetectionProcess();
                pdc = new PreproceingDeviationClass();
                pcmw = new PreproceingContinousMultiWaveClass();
                wdp = new WaveformDataProcess();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
                MessageBox.Show(ex.Source);
                MessageBox.Show(ex.StackTrace);
            }


        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                BackGroundRun();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
                MessageBox.Show(ex.Source);
                MessageBox.Show(ex.StackTrace);
            }
            
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            DisplayDataStrList(dataStrList);
            //MessageBox.Show("完成！");
            splitContainer1.Panel1.Enabled = true;
        }

        private Boolean IsTwoCitIndex()
        {
            if (!(CommonClass.listDIC[0].bIndex && CommonClass.listDIC[1].bIndex))
            {
                return false;
            }
            indexStaClsList_0 = CommonClass.wdp.GetDataIndexInfo(CommonClass.listDIC[0].sAddFile);
            indexStaClsList_1 = CommonClass.wdp.GetDataIndexInfo(CommonClass.listDIC[1].sAddFile);

            if ((indexStaClsList_0 == null || indexStaClsList_0.Count == 0) && (indexStaClsList_1 == null || indexStaClsList_1.Count == 0))
            {
                return false;
            }


            return true;
        }
        private Boolean AvailableCheck()
        {
            if(CommonClass.listDIC[0].sKmInc != CommonClass.listDIC[1].sKmInc)
            {
                MessageBox.Show("两个波形增减里程不一致！");
                return false;
            }

            Boolean isKmInc_0 = false; //false代表减里程
            Boolean isKmInc_1 = false; //false代表减里程
            startMeter_0 = double.Parse(indexStaClsList_0[1].lStartMeter);
            endMeter_0 = double.Parse(indexStaClsList_0[indexStaClsList_0.Count - 2].LEndMeter);
            startMeter_1 = double.Parse(indexStaClsList_1[1].lStartMeter);
            endMeter_1 = double.Parse(indexStaClsList_1[indexStaClsList_1.Count - 2].LEndMeter);
            if (startMeter_0 < endMeter_0)
            {
                isKmInc_0 = true;//true代表增里程
            }
            if (startMeter_1 < endMeter_1)
            {
                isKmInc_1 = true;//true代表增里程
            }
            if (isKmInc_0 != isKmInc_1)
            {
                MessageBox.Show("两个波形增减里程不一致！");
                return false;
            }

            if (((startMeter_1 >= startMeter_0 && startMeter_1 >= endMeter_0) || (startMeter_0 >= startMeter_1 && startMeter_0 >= endMeter_1)) && isKmInc_0 == true)
            {
                //MessageBox.Show("两个波形没有重合部分！");
                //return false;
            }

            //这里还需要判断两个波形没有重合的部分

            //if (()*() <= 0 &&)
            //{
            //}

            return true;
        }
        private void InitStartMileAndEndMile()
        {
            if (CommonClass.listDIC[0].sKmInc.Equals("增"))
            {
                startMile = startMeter_0;
                if (startMeter_1 > startMeter_0)
                {
                    startMile = startMeter_1;
                }

                endMile = endMeter_0;
                if (endMeter_1 < endMeter_0)
                {
                    endMile = endMeter_1;
                }
            }
            else
            {
                startMile = startMeter_0;
                if (startMeter_1 < startMeter_0)
                {
                    startMile = startMeter_1;
                }

                endMile = endMeter_0;
                if (endMeter_1 > endMeter_0)
                {
                    endMile = endMeter_1;
                }
            }
        }
        private void InitMilePosCls()
        {
            milePosClsStart_0 = new MilePosClass((long)(startMile * 1000), indexStaClsList_0, CommonClass.listDIC[0].iChannelNumber, CommonClass.listDIC[0].sKmInc);
            milePosClsEnd_0 = new MilePosClass((long)(endMile * 1000), indexStaClsList_0, CommonClass.listDIC[0].iChannelNumber, CommonClass.listDIC[0].sKmInc);
            milePosClsStart_1 = new MilePosClass((long)(startMile * 1000), indexStaClsList_1, CommonClass.listDIC[1].iChannelNumber, CommonClass.listDIC[1].sKmInc);
            milePosClsEnd_1 = new MilePosClass((long)(endMile * 1000), indexStaClsList_1, CommonClass.listDIC[1].iChannelNumber, CommonClass.listDIC[1].sKmInc);
        }
        private double[] GetMileWithIdf(List<IndexStaClass> listIC, MilePosClass startCls, MilePosClass endCls,int tds)
        {
            List<double> retVal = new List<double>();
            Boolean isKmInc = false;//false代表减里程
            int i = 0;

            if(double.Parse(listIC[0].lStartMeter) < double.Parse(listIC[listIC.Count-1].LEndMeter))
            {
                isKmInc = true;//增里程
            }

            for (i = 0; i < listIC.Count;i++ )
            {
                if (startCls.GetPos() >= listIC[i].lStartPoint && startCls.GetPos() <= listIC[i].lEndPoint)
                {
                    long jvli = Math.Abs(startCls.mile-(long)(double.Parse(listIC[i].LEndMeter)*1000));

                    int iCount = (int)(jvli / (float.Parse(listIC[i].lContainsMeter) * 1000 / listIC[i].lContainsPoint));

                    long lCurPos = startCls.GetPos() - listIC[i].lStartPoint;
                    int iIndex = 0;
                    if (listIC[i].sType.Contains("长链"))
                    {


                        int iKM = 0;
                        double dCDLMeter = float.Parse(listIC[i].lContainsMeter) * 1000;
                        if (isKmInc == false)
                        {
                            iKM = (int)float.Parse(listIC[i].LEndMeter);
                        }
                        else
                        {
                            iKM = (int)float.Parse(listIC[i].lStartMeter);
                        }
                        for (iIndex = 0; iIndex < iCount && (startCls.GetPos() + iIndex * tds * 2) < listIC[i].lEndPoint; iIndex++)
                        {
                            float f = (1 + iIndex) * ((float.Parse(listIC[i].lContainsMeter) * 1000 / listIC[i].lContainsPoint));
                            WaveMeter wm = new WaveMeter();
                            if (isKmInc == false)
                            {
                                wm.Km = iKM;
                                wm.Meter = (float)(dCDLMeter - f);

                                retVal.Add(wm.Km + wm.Meter / 1000f);
                            }
                            else
                            {
                                wm.Km = iKM;
                                wm.Meter = f;

                                retVal.Add(wm.Km + wm.Meter / 1000f);
                            }
                            wm.lPosition = (startCls.GetPos() + (iIndex * tds * 2));

                        }
                    }
                    else
                    {
                        double dMeter = float.Parse(listIC[i].lStartMeter) * 1000;
                        for (iIndex = 0; iIndex < iCount && (startCls.GetPos() + iIndex * tds * 2) < listIC[i].lEndPoint; iIndex++)
                        {
                            float f = (1 + iIndex) * ((float.Parse(listIC[i].lContainsMeter) * 1000 / listIC[i].lContainsPoint));
                            WaveMeter wm = new WaveMeter();
                            if (isKmInc == false)
                            {
                                wm.Km = (int)((dMeter - f) / 1000);
                                wm.Meter = (float)((dMeter - f) % 1000);

                                retVal.Add(wm.Km + wm.Meter / 1000f);
                            }
                            else
                            {
                                wm.Km = (int)((dMeter + f) / 1000);
                                wm.Meter = (float)((dMeter + f) % 1000);

                                retVal.Add(wm.Km + wm.Meter / 1000f);
                            }
                            wm.lPosition = (startCls.GetPos() + (iIndex * tds * 2));

                        }
                    }



                    break;
                }
            }

            for (++i; i < listIC.Count;i++ )
            {
                if (endCls.GetPos() >= listIC[i].lStartPoint && endCls.GetPos() <= listIC[i].lEndPoint)
                {
                    long jvli = Math.Abs(endCls.mile - (long)(double.Parse(listIC[i].lStartMeter) * 1000));

                    int iCount = (int)(jvli / (float.Parse(listIC[i].lContainsMeter) * 1000 / listIC[i].lContainsPoint));

                    long lCurPos = listIC[i].lStartPoint;
                    int iIndex = 0;
                    if (listIC[i].sType.Contains("长链"))
                    {


                        int iKM = 0;
                        double dCDLMeter = float.Parse(listIC[i].lContainsMeter) * 1000;
                        if (isKmInc == false)
                        {
                            iKM = (int)float.Parse(listIC[i].LEndMeter);
                        }
                        else
                        {
                            iKM = (int)float.Parse(listIC[i].lStartMeter);
                        }
                        for (iIndex = 0; iIndex < iCount && (listIC[i].lStartPoint + iIndex * tds * 2) < endCls.GetPos(); iIndex++)
                        {
                            float f = (1 + iIndex) * ((float.Parse(listIC[i].lContainsMeter) * 1000 / listIC[i].lContainsPoint));
                            WaveMeter wm = new WaveMeter();
                            if (isKmInc == false)
                            {
                                wm.Km = iKM;
                                wm.Meter = (float)(dCDLMeter - f);

                                retVal.Add(wm.Km + wm.Meter / 1000f);
                            }
                            else
                            {
                                wm.Km = iKM;
                                wm.Meter = f;

                                retVal.Add(wm.Km + wm.Meter / 1000f);
                            }
                            wm.lPosition = (listIC[i].lStartPoint + (iIndex * tds * 2));

                        }
                    }
                    else
                    {
                        double dMeter = float.Parse(listIC[i].lStartMeter) * 1000;
                        for (iIndex = 0; iIndex < iCount && (listIC[i].lStartPoint + iIndex * tds * 2) < endCls.GetPos(); iIndex++)
                        {
                            float f = (1 + iIndex) * ((float.Parse(listIC[i].lContainsMeter) * 1000 / listIC[i].lContainsPoint));
                            WaveMeter wm = new WaveMeter();
                            if (isKmInc == false)
                            {
                                wm.Km = (int)((dMeter - f) / 1000);
                                wm.Meter = (float)((dMeter - f) % 1000);

                                retVal.Add(wm.Km + wm.Meter / 1000f);
                            }
                            else
                            {
                                wm.Km = (int)((dMeter + f) / 1000);
                                wm.Meter = (float)((dMeter + f) % 1000);

                                retVal.Add(wm.Km + wm.Meter / 1000f);
                            }
                            wm.lPosition = (startCls.GetPos() + (iIndex * tds * 2));

                        }
                    }

                    break;
                }


                

                int iCount_m = (int)(listIC[i].lContainsPoint);

                long lCurPos_m = listIC[i].lStartPoint;
                int iIndex_m = 0;
                if (listIC[i].sType.Contains("长链"))
                {


                    int iKM = 0;
                    double dCDLMeter = float.Parse(listIC[i].lContainsMeter) * 1000;
                    if (isKmInc == false)
                    {
                        iKM = (int)float.Parse(listIC[i].LEndMeter);
                    }
                    else
                    {
                        iKM = (int)float.Parse(listIC[i].lStartMeter);
                    }
                    for (iIndex_m = 0; iIndex_m < iCount_m && (listIC[i].lStartPoint + iIndex_m * tds * 2) < listIC[i].lEndPoint; iIndex_m++)
                    {
                        float f = (1 + iIndex_m) * ((float.Parse(listIC[i].lContainsMeter) * 1000 / listIC[i].lContainsPoint));
                        WaveMeter wm = new WaveMeter();
                        if (isKmInc == false)
                        {
                            wm.Km = iKM;
                            wm.Meter = (float)(dCDLMeter - f);

                            retVal.Add(wm.Km + wm.Meter / 1000f);
                        }
                        else
                        {
                            wm.Km = iKM;
                            wm.Meter = f;

                            retVal.Add(wm.Km + wm.Meter / 1000f);
                        }
                        wm.lPosition = (listIC[i].lStartPoint + (iIndex_m * tds * 2));

                    }
                }
                else
                {
                    double dMeter = float.Parse(listIC[i].lStartMeter) * 1000;
                    for (iIndex_m = 0; iIndex_m < iCount_m && (listIC[i].lStartPoint + iIndex_m * tds * 2) < listIC[i].lEndPoint; iIndex_m++)
                    {
                        float f = (1 + iIndex_m) * ((float.Parse(listIC[i].lContainsMeter) * 1000 / listIC[i].lContainsPoint));
                        WaveMeter wm = new WaveMeter();
                        if (isKmInc == false)
                        {
                            wm.Km = (int)((dMeter - f) / 1000);
                            wm.Meter = (float)((dMeter - f) % 1000);

                            retVal.Add(wm.Km + wm.Meter / 1000f);
                        }
                        else
                        {
                            wm.Km = (int)((dMeter + f) / 1000);
                            wm.Meter = (float)((dMeter + f) % 1000);

                            retVal.Add(wm.Km + wm.Meter / 1000f);
                        }
                        wm.lPosition = (startCls.GetPos() + (iIndex_m * tds * 2));

                    }
                }



            }



            

            return retVal.ToArray();
        }

        public void BackGroundRun()
        {
            if (isReady)
            {
                try
                {
                    if (CommonClass.listDIC.Count != 2)
                    {
                        MessageBox.Show("智能识别只支持两个波形比较！");
                        return ;
                    }

                    if (!IsTwoCitIndex())
                    {
                        MessageBox.Show("未加载索引文件 或 索引文件为空");
                        return;
                    }
                    if (!AvailableCheck())
                    {
                        return;
                    }

                    InitStartMileAndEndMile();
                    InitMilePosCls();
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    MessageBox.Show(ex.Source);
                    MessageBox.Show(ex.StackTrace);
                }

                dataStrList = ChangeDetectionProcess(CommonClass.listDIC[0].sFilePath, "", CommonClass.listDIC[1].sFilePath, "");
            }
            
        }

        private void DisplayDataStrList(List<String> dataStrList)
        {
            int index = 0;

            dataGridView1.Rows.Clear();

            if (dataStrList == null || dataStrList.Count == 0)
            {
                return;
            }
            foreach (String dataStr in dataStrList)
            {
                String[] dataStrArry = dataStr.Split(',');

                DataGridViewRow dgvr = new DataGridViewRow();
                dgvr.CreateCells(dataGridView1);

                dgvr.Cells[0].Value = ++index;
                dgvr.Cells[1].Value = dataStrArry[0];
                dgvr.Cells[2].Value = Math.Round(double.Parse(dataStrArry[1]),3);
                dgvr.Cells[3].Value = Math.Round(double.Parse(dataStrArry[2]), 3);
                dgvr.Cells[4].Value = Math.Round(double.Parse(dataStrArry[3]), 2);

                dataGridView1.Rows.Add(dgvr);
            }
        }



        #region 接口函数：把两个对齐的波形文件，进行对比，找出变化的区段
        /// <summary>
        /// 接口函数：把两个对齐的波形文件，进行对比，找出变化的区段
        /// </summary>
        /// <param name="cit_1">第一个cit波形文件</param>
        /// <param name="idf_1">第一个idf文件</param>
        /// <param name="cit_2">第二个cit波形文件</param>
        /// <param name="idf_2">第二个idf文件</param>
        /// <returns></returns>
        public List<String> ChangeDetectionProcess(String cit_1, String idf_1, String cit_2, String idf_2)
        {
            List<String> dataStrList = new List<String>();

            long startPos_0 = milePosClsStart_0.GetPos();
            long endPos_0 = milePosClsEnd_0.GetPos();
            long startPos_1 = milePosClsStart_1.GetPos();
            long endPos_1 = milePosClsEnd_1.GetPos();

            double[] d_tt_1 = GetMileWithIdf(indexStaClsList_0,milePosClsStart_0,milePosClsEnd_0,CommonClass.listDIC[0].iChannelNumber);
            double[] d_wvelo_1 = cdp.GetSingleChannelData(cit_1, dataProcessing.GetChannelNumberByChannelName(cit_1, "Speed", "速度"), startPos_0, endPos_0);
            double[] d_wx_gauge_1 = cdp.GetSingleChannelData(cit_1, dataProcessing.GetChannelNumberByChannelName(cit_1, "Gage", "轨距"), startPos_0, endPos_0);
            

            double[] d_tt_2 = GetMileWithIdf(indexStaClsList_1, milePosClsStart_1, milePosClsEnd_1, CommonClass.listDIC[1].iChannelNumber);
            double[] d_wvelo_2 = cdp.GetSingleChannelData(cit_2, dataProcessing.GetChannelNumberByChannelName(cit_2, "Speed", "速度"), startPos_1, endPos_1);
            double[] d_wx_gauge_2 = cdp.GetSingleChannelData(cit_2, dataProcessing.GetChannelNumberByChannelName(cit_2, "Gage", "轨距"), startPos_1, endPos_1);

            double[] d_wx_1 = cdp.GetSingleChannelData(cit_1, dataProcessing.GetChannelNumberByChannelName(cit_1, "L_Prof_SC", "左高低_中波"), startPos_0, endPos_0);
            double[] d_wx_2 = cdp.GetSingleChannelData(cit_2, dataProcessing.GetChannelNumberByChannelName(cit_2, "L_Prof_SC", "左高低_中波"), startPos_1, endPos_1);

            List<String> tmpDataStrList = changeDetcPro.ChangeDetectionPrcs("左高低_中波", d_tt_1, d_wx_1, d_wvelo_1, d_wx_gauge_1,d_tt_2, d_wx_2, d_wvelo_2, d_wx_gauge_2);
            dataStrList.AddRange(tmpDataStrList);

            d_wx_1 = cdp.GetSingleChannelData(cit_1, dataProcessing.GetChannelNumberByChannelName(cit_1, "R_Prof_SC", "右高低_中波"), startPos_0, endPos_0);
            d_wx_2 = cdp.GetSingleChannelData(cit_2, dataProcessing.GetChannelNumberByChannelName(cit_2, "R_Prof_SC", "右高低_中波"), startPos_1, endPos_1);

            tmpDataStrList = changeDetcPro.ChangeDetectionPrcs("右高低_中波",  d_tt_1, d_wx_1, d_wvelo_1, d_wx_gauge_1, d_tt_2, d_wx_2, d_wvelo_2, d_wx_gauge_2);
            dataStrList.AddRange(tmpDataStrList);

            d_wx_1 = cdp.GetSingleChannelData(cit_1, dataProcessing.GetChannelNumberByChannelName(cit_1, "L_Align_SC", "左轨向_中波"), startPos_0, endPos_0);
            d_wx_2 = cdp.GetSingleChannelData(cit_2, dataProcessing.GetChannelNumberByChannelName(cit_2, "L_Align_SC", "左轨向_中波"), startPos_1, endPos_1);

            tmpDataStrList = changeDetcPro.ChangeDetectionPrcs("左轨向_中波", d_tt_1, d_wx_1, d_wvelo_1, d_wx_gauge_1, d_tt_2, d_wx_2, d_wvelo_2, d_wx_gauge_2);
            dataStrList.AddRange(tmpDataStrList);

            d_wx_1 = cdp.GetSingleChannelData(cit_1, dataProcessing.GetChannelNumberByChannelName(cit_1, "R_Align_SC", "右轨向_中波"), startPos_0, endPos_0);
            d_wx_2 = cdp.GetSingleChannelData(cit_2, dataProcessing.GetChannelNumberByChannelName(cit_2, "R_Align_SC", "右轨向_中波"), startPos_1, endPos_1);

            tmpDataStrList = changeDetcPro.ChangeDetectionPrcs("右轨向_中波", d_tt_1, d_wx_1, d_wvelo_1, d_wx_gauge_1, d_tt_2, d_wx_2, d_wvelo_2, d_wx_gauge_2);
            dataStrList.AddRange(tmpDataStrList);

            return dataStrList;
        }
        #endregion

        /// <summary>
        /// 接口函数：计算峰峰值指标
        /// </summary>
        /// <param name="citFileName">cit文件全路径</param>
        /// <returns></returns>
        public List<String> PreProcessDeviation(String citFileName)
        {
            List<String> dataStrList = new List<String>();

            //cdp.QueryDataInfoHead(citFileName);

            //double[] d_tt = cdp.GetMilesDataDouble(citFileName);//里程
            CITDataProcess.DataHeadInfo dhi = cdp.GetDataInfoHeadNew(citFileName);
            int tds =dhi.iChannelNumber;

            long startPos = 0;
            long endPos = 0;

            using (FileStream fs = new FileStream(citFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (BinaryReader br = new BinaryReader(fs))
                {
                    byte[] b = new byte[tds * 2];
                    br.ReadBytes(120);//120
                    br.ReadBytes(65 * tds);//65
                    br.ReadBytes(BitConverter.ToInt32(br.ReadBytes(4), 0));


                    startPos = br.BaseStream.Position;
                    endPos = br.BaseStream.Length;

                    br.Close();
                }
                fs.Close();

            }

            double[] d_tt = wdp.GetDataMileageInfoDouble(citFileName, tds, dhi.iSmaleRate, CommonClass.listDIC[0].bEncrypt,
                CommonClass.listDIC[0].listIC, CommonClass.listDIC[0].bIndex, startPos, endPos, CommonClass.listDIC[0].sKmInc);//里程

            double[] d_wvelo = cdp.GetSingleChannelData(citFileName, dataProcessing.GetChannelNumberByChannelName(citFileName, "Speed", "速度"));
            double[] d_gauge = cdp.GetSingleChannelData(citFileName, dataProcessing.GetChannelNumberByChannelName(citFileName, "Gage", "轨距"));


            double[] d_wx = cdp.GetSingleChannelData(citFileName, dataProcessing.GetChannelNumberByChannelName(citFileName, "L_Prof_SC", "左高低_中波"));


            List<String> tmpDataStrList = pdc.WideGaugePreProcess("左高低_中波", d_tt, d_wx, d_wvelo, d_gauge, 8.0);
            dataStrList.AddRange(tmpDataStrList);

            d_wx = cdp.GetSingleChannelData(citFileName, dataProcessing.GetChannelNumberByChannelName(citFileName, "R_Prof_SC", "右高低_中波"));

            tmpDataStrList = pdc.WideGaugePreProcess("右高低_中波", d_tt, d_wx, d_wvelo, d_gauge, 8.0);
            dataStrList.AddRange(tmpDataStrList);

            d_wx = cdp.GetSingleChannelData(citFileName, dataProcessing.GetChannelNumberByChannelName(citFileName, "L_Align_SC", "左轨向_中波"));

            tmpDataStrList = pdc.WideGaugePreProcess("左轨向_中波", d_tt, d_wx, d_wvelo, d_gauge, 8.0);
            dataStrList.AddRange(tmpDataStrList);

            d_wx = cdp.GetSingleChannelData(citFileName, dataProcessing.GetChannelNumberByChannelName(citFileName, "R_Align_SC", "右轨向_中波"));

            tmpDataStrList = pdc.WideGaugePreProcess("右轨向_中波", d_tt, d_wx, d_wvelo, d_gauge, 8.0);
            dataStrList.AddRange(tmpDataStrList);

            return dataStrList;
        }

        /// <summary>
        /// 接口函数：计算连续多波指标
        /// </summary>
        /// <param name="citFileName">cit文件全路径名</param>
        /// <returns></returns>
        public List<String> PreproceingContinousMultiWave(String citFileName)
        {
            List<String> dataStrList = new List<String>();

            CITDataProcess.DataHeadInfo dhi = cdp.GetDataInfoHeadNew(citFileName);
            int tds = dhi.iChannelNumber;

            long startPos = 0;
            long endPos = 0;


            using (FileStream fs = new FileStream(citFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (BinaryReader br = new BinaryReader(fs))
                {
                    byte[] b = new byte[tds * 2];
                    br.ReadBytes(120);//120
                    br.ReadBytes(65 * tds);//65
                    br.ReadBytes(BitConverter.ToInt32(br.ReadBytes(4), 0));


                    startPos = br.BaseStream.Position;
                    endPos = br.BaseStream.Length;

                    br.Close();
                }
                fs.Close();

            }


            //double[] d_tt = cdp.GetMilesDataDouble(citFileName);//里程
            double[] d_tt = wdp.GetDataMileageInfoDouble(citFileName, tds, dhi.iSmaleRate, CommonClass.listDIC[0].bEncrypt, 
                CommonClass.listDIC[0].listIC, CommonClass.listDIC[0].bIndex,startPos,endPos,CommonClass.listDIC[0].sKmInc);//里程

            double[] d_wvelo = cdp.GetSingleChannelData(citFileName, dataProcessing.GetChannelNumberByChannelName(citFileName, "Speed", "速度"));
            double[] d_gauge = cdp.GetSingleChannelData(citFileName, dataProcessing.GetChannelNumberByChannelName(citFileName, "Gage", "轨距"));


            double[] d_wx = cdp.GetSingleChannelData(citFileName, dataProcessing.GetChannelNumberByChannelName(citFileName, "L_Prof_SC", "左高低_中波"));


            List<String> tmpDataStrList = pcmw.ContinousMultiWavePreprocess("左高低_中波", d_tt, d_wx, d_wvelo, d_gauge, 3.0, 3.0);
            dataStrList.AddRange(tmpDataStrList);

            d_wx = cdp.GetSingleChannelData(citFileName, dataProcessing.GetChannelNumberByChannelName(citFileName, "R_Prof_SC", "右高低_中波"));

            tmpDataStrList = pcmw.ContinousMultiWavePreprocess("右高低_中波", d_tt, d_wx, d_wvelo, d_gauge, 3.0, 3.0);
            dataStrList.AddRange(tmpDataStrList);

            d_wx = cdp.GetSingleChannelData(citFileName, dataProcessing.GetChannelNumberByChannelName(citFileName, "L_Align_SC", "左轨向_中波"));

            tmpDataStrList = pcmw.ContinousMultiWavePreprocess("左轨向_中波", d_tt, d_wx, d_wvelo, d_gauge, 3.0, 3.0);
            dataStrList.AddRange(tmpDataStrList);

            d_wx = cdp.GetSingleChannelData(citFileName, dataProcessing.GetChannelNumberByChannelName(citFileName, "R_Align_SC", "右轨向_中波"));

            tmpDataStrList = pcmw.ContinousMultiWavePreprocess("右轨向_中波", d_tt, d_wx, d_wvelo, d_gauge, 3.0, 3.0);
            dataStrList.AddRange(tmpDataStrList);

            return dataStrList;
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1)
            {
                return;
            }

            Boolean bIndex = CommonClass.listDIC[0].bIndex;

            float startMile = float.Parse(dataGridView1[2, e.RowIndex].Value.ToString());
            float endMile = float.Parse(dataGridView1[3, e.RowIndex].Value.ToString());

            if (bIndex)
            {
                long indexStart = CommonClass.wdp.GetNewIndexMeterPositon(CommonClass.listDIC[0].listIC, (long)(startMile * 1000), CommonClass.listDIC[0].iChannelNumber, CommonClass.listDIC[0].sKmInc, 0);
                long indexEnd = CommonClass.wdp.GetNewIndexMeterPositon(CommonClass.listDIC[0].listIC, (long)(endMile * 1000), CommonClass.listDIC[0].iChannelNumber, CommonClass.listDIC[0].sKmInc, 0);

                MainForm.sMainform.MeterFind(indexStart, indexEnd);;
            } 
            else
            {
                MainForm.sMainform.MeterFind(startMile,endMile);
            }

        }



        private void button_OK_Click(object sender, EventArgs e)
        {
            splitContainer1.Panel1.Enabled = false;
            backgroundWorker1.RunWorkerAsync();
        }

        private void button_ExportExcel_Click(object sender, EventArgs e)
        {
            String excelPath = null;
            String excelName = null;

            if (dataGridView1.Rows.Count == 0)
            {
                return;
            }


            excelPath = Path.GetDirectoryName(CommonClass.listDIC[1].sFilePath);
            excelName = Path.GetFileNameWithoutExtension(CommonClass.listDIC[1].sFilePath);


            excelName = excelName + "." + this.Text + ".csv";

            excelPath = Path.Combine(excelPath, excelName);

            StreamWriter sw = new StreamWriter(excelPath, false, Encoding.Default);

            StringBuilder sbtmp = new StringBuilder();
            for (int i = 0; i < dataGridView1.Columns.Count; i++)
            {
                sbtmp.Append(dataGridView1.Columns[i].HeaderText + ",");
            }
            sbtmp.Remove(sbtmp.Length - 1, 1);
            sw.WriteLine(sbtmp.ToString());

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                for (int j = 0; j < dataGridView1.Rows[i].Cells.Count; j++)
                {
                    sw.Write(dataGridView1.Rows[i].Cells[j].Value.ToString());
                    if ((j + 1) != dataGridView1.Rows[i].Cells.Count)
                    {
                        sw.Write(",");
                    }
                    else
                    {
                        sw.Write("\n");
                    }
                }
            }

            sw.Close();

            MessageBox.Show("导出成功！");
        }

        /// <summary>
        /// 把展现在Datagridview表格中德识别的变化信息写入到第一层的idf文件中
        /// </summary>
        /// <param name="idfPath">底层文件的idf文件全路径</param>
        /// <param name="citFileName">第二层文件的cit文件名</param>
        /// <param name="dgv">Datagridview表格</param>
        private void ChangeInfoInsertInto(String idfPath,String citFileName,DataGridView dgv)
        {
            CommonClass.wdp.CreatTableChangeInfo(idfPath);

            id = CommonClass.wdp.GetIdInTableFromIdf(idfPath, "ChangeInfo");

            if (dataGridView1.Rows.Count == 0)
            {
                return;
            }

            try
            {
                using (OleDbConnection sqlconn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + idfPath + ";Persist Security Info=True"))
                {
                    sqlconn.Open();
                    OleDbCommand com = null;

                    for (int i = 0; i < dgv.Rows.Count; i++)
                    {
                        com = sqlconn.CreateCommand();
                        com.CommandText = "insert into ChangeInfo values(@Id,@FileName,@ChannelName,@StartMeter,@EndMeter,@AbsoluteValue)";

                        com.Parameters.AddWithValue("@Id", id++);
                        com.Parameters.AddWithValue("@FileName", citFileName);
                        com.Parameters.AddWithValue("@ChannelName", dgv.Rows[i].Cells[1].Value.ToString());
                        com.Parameters.AddWithValue("@StartMeter", dgv.Rows[i].Cells[2].Value.ToString());
                        com.Parameters.AddWithValue("@EndMeter", dgv.Rows[i].Cells[3].Value.ToString());
                        com.Parameters.AddWithValue("@AbsoluteValue", dgv.Rows[i].Cells[4].Value.ToString());

                        com.ExecuteNonQuery();
                    }
                    
                    sqlconn.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("新增微小变化信息异常:" + ex.Message);
            }

        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            ChangeInfoInsertInto(CommonClass.listDIC[0].sAddFile, Path.GetFileName(CommonClass.listDIC[1].sFilePath), dataGridView1);
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
        /// <summary>
        /// 通道中文名
        /// </summary>
        public String channelNameCh;
    }
    #endregion

    public class MilePosClass
    {
        public long mile;
        public List<IndexStaClass> indexStaClsList;
        public int tds;
        public String sKmInc;


        public MilePosClass(long mile, List<IndexStaClass> indexStaClsList, int tds, String sKmInc)
        {
            this.mile = mile;
            this.indexStaClsList = indexStaClsList;
            this.tds = tds;
            this.sKmInc = sKmInc;
        }

        public long GetPos()
        {
            long pos=CommonClass.wdp.GetNewIndexMeterPositon(indexStaClsList, mile, tds, sKmInc, 0);
            return pos;
        }


    }
}
