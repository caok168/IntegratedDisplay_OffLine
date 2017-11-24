using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using InGraph.Classes;
using CitFileProcess;
using MathWorks.MATLAB.NET.Arrays;
using InvalidDataProcessing.Model;

namespace InGraph.Forms
{
    /// <summary>
    /// 波磨窗体
    /// </summary>
    public partial class AccIrtnCorrugation : Form
    {
        private List<String> citSourceCheckFileList = new List<String>();

        CitFileProcess.CitFileHelper citHelper = new CitFileProcess.CitFileHelper();
        IdfFileHelper idfHelper = new IdfFileHelper();

        public AccIrtnCorrugation()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 浏览按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonFolderBrowser_Click(object sender, EventArgs e)
        {
            DialogResult dr = folderBrowserDialog1.ShowDialog();
            if (dr == DialogResult.OK)
            {
                textBoxFileDirectory.Text = folderBrowserDialog1.SelectedPath;
            }

            listViewFile.Items.Clear();

            try
            {
                string[] sFiles = Directory.GetFiles(textBoxFileDirectory.Text, "*.cit", SearchOption.TopDirectoryOnly);

                listViewFile.BeginUpdate();
                foreach (string v in sFiles)
                {

                    string status = CommonClass.cdp.QueryDataInfoHead(v);
                    string[] sDataInfoHead = status.Split(new char[] { ',' });
                    // iDataType; sDataVersion; sTrackCode; sTrackName; iDir; 
                    //sTrain; sDate; sTime; iRunDir;iKmInc; 
                    //fkmFrom; fkmTo; iSmaleRate; iChannelNumber;
                    if (sDataInfoHead[0].Contains("0"))
                    {
                        listViewFile.Items.Add(sDataInfoHead[4]);

                        listViewFile.Items[listViewFile.Items.Count - 1].SubItems.Add(sDataInfoHead[3]);
                        listViewFile.Items[listViewFile.Items.Count - 1].SubItems.Add(sDataInfoHead[5]);
                        listViewFile.Items[listViewFile.Items.Count - 1].SubItems.Add(sDataInfoHead[10]);
                        listViewFile.Items[listViewFile.Items.Count - 1].SubItems.Add(sDataInfoHead[7]);
                        listViewFile.Items[listViewFile.Items.Count - 1].SubItems.Add(sDataInfoHead[8]);
                        listViewFile.Items[listViewFile.Items.Count - 1].SubItems.Add(sDataInfoHead[6]);
                        listViewFile.Items[listViewFile.Items.Count - 1].SubItems.Add(Path.GetFileName(v));
                        listViewFile.Items[listViewFile.Items.Count - 1].SubItems.Add((new FileInfo(v)).Length.ToString());
                        listViewFile.Items[listViewFile.Items.Count - 1].SubItems.Add(Path.GetDirectoryName(v));
                    }
                    Application.DoEvents();

                }
                listViewFile.EndUpdate();

            }
            catch
            {
                //MessageBox.Show("获取所有文件出错，请检查子目录");
                return;
            }
        }

        /// <summary>
        /// 全选变化事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ckb_All_CheckedChanged(object sender, EventArgs e)
        {
            if (ckb_All.Checked)
            {
                listViewFile.Focus();
                foreach (ListViewItem item in listViewFile.Items)
                {
                    item.Checked = true;
                }
            }
            else
            {
                listViewFile.Focus();
                foreach (ListViewItem item in listViewFile.Items)
                {
                    item.Checked = false;
                }
            }
        }


        delegate void GetCitSourceCheckFileListCallback();
        private void GetCitSourceCheckFileList()
        {
            if (this.listViewFile.InvokeRequired)
            {
                GetCitSourceCheckFileListCallback d = new GetCitSourceCheckFileListCallback(GetCitSourceCheckFileList);
                this.Invoke(d, new object[] { });
            }
            else
            {
                citSourceCheckFileList.Clear();
                for (int i = 0; i < listViewFile.CheckedItems.Count; i++)
                {
                    String citFilePath = null;
                    if (listViewFile.CheckedItems[i].SubItems[9].Text.EndsWith("\\"))
                    {
                        citFilePath = listViewFile.CheckedItems[i].SubItems[9].Text + listViewFile.CheckedItems[i].SubItems[7].Text;
                    }
                    else
                    {
                        citFilePath = listViewFile.CheckedItems[i].SubItems[9].Text + "\\" + listViewFile.CheckedItems[i].SubItems[7].Text;
                    }
                    citSourceCheckFileList.Add(citFilePath);
                }
            }

        }


        /// <summary>
        /// 计算波磨有效值按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_CalcRms_Click(object sender, EventArgs e)
        {
            GetCitSourceCheckFileList();

            try
            {
                InvalidDataProcessing.CalculateCorrugationClass calculateCorrugation = new InvalidDataProcessing.CalculateCorrugationClass();

                int fs = Convert.ToInt32(this.txt_fs.Text);
                int len_win_imp = Convert.ToInt32(this.txt_hj_len_win_imp.Text);
                int FilterFreq_L = Convert.ToInt32(this.txt_hj_FilterFreq_L.Text);
                int FilterFreq_H = Convert.ToInt32(this.txt_hj_FilterFreq_H.Text);

                int len_win = Convert.ToInt32(this.txt_bm_len_win.Text);

                double Wavelen_L = Convert.ToDouble(this.txt_bm_Wavelen_L.Text);
                double Wavelen_H = Convert.ToDouble(this.txt_bm_Wavelen_H.Text);
                int len_downsample = Convert.ToInt32(this.txt_len_downsample.Text);

                int len_merge = Convert.ToInt32(this.txt_len_merge.Text);

                double mean_rms_nacc = Convert.ToDouble(this.txtBM_Avg.Text);
                double thresh_tii = Convert.ToDouble(this.txt_thresh_tii.Text);

                foreach (var sFile in citSourceCheckFileList)
                {
                    string citFileNew = sFile.Substring(0, sFile.Length - 4) + "corrugationRms.cit";
                    var header = citHelper.GetDataInfoHead(sFile);

                    CreateCitHeader(citFileNew, header);

                    string idfFilePath = citFileNew.Replace(".cit", ".idf");
                    CreateIdfFile(idfFilePath);

                    WriteCitInfoToIdf(idfFilePath, header);

                    header = citHelper.GetDataInfoHead(sFile);

                    int bytesneed = header.iChannelNumber * 2 * 16000000;


                    long startPos = citHelper.GetSamplePointStartOffset(header.iChannelNumber, 4);

                    long citFileLength = citHelper.GetFileLength(sFile);

                    int count = Convert.ToInt32((citFileLength - startPos) / bytesneed);

                    for (int i = 0; i < count; i++)
                    {
                        long endPos = startPos + bytesneed;

                        var data_miles = citHelper.GetMilesData(sFile, startPos, endPos);
                        var data_speed = citHelper.GetSingleChannelData(sFile, 3, startPos, endPos);

                        List<double[]> citDataList = new List<double[]>();

                        //轴箱左垂
                        var channelData = citHelper.GetSingleChannelData(sFile, 4, startPos, endPos);
                        //有效值
                        MWNumericArray rmsArray = calculateCorrugation.CalcCorrugationRmsProcess(data_miles, data_speed, channelData, fs, len_win_imp, FilterFreq_L, FilterFreq_H, len_win, Wavelen_L, Wavelen_H, len_downsample);
                        List<double[]> rmsValueList = calculateCorrugation.GetCalcCorrugationRms(rmsArray);
                        if (rmsValueList.Count == 3)
                        {
                            double[] kmValue = new double[rmsValueList[0].Length];
                            double[] mValue = new double[rmsValueList[0].Length];

                            for (int k = 0; k < rmsValueList[0].Length; k++)
                            {
                                kmValue[k] = (int)rmsValueList[0][k];
                                mValue[k] = rmsValueList[0][k] - kmValue[k];
                            }

                            citDataList.Add(kmValue);
                            citDataList.Add(mValue);
                            citDataList.Add(rmsValueList[2]);
                            citDataList.Add(rmsValueList[1]);
                        }
                        //区段大值
                        MWNumericArray peakArray = calculateCorrugation.CalcCorrugationMaxProcess(rmsArray, len_merge);
                        List<double[]> peakValueList = calculateCorrugation.GetCalcCorrugationMax(peakArray); //peakValueList 里程、大值、速度
                        WritePeakInfoToIdf(idfFilePath, "AB_Vt_L_RMS_11" + "Peakvalue", peakValueList[0].ToList(), peakValueList[2].ToList(), peakValueList[1].ToList());

                        //轴箱右垂 通道
                        channelData = citHelper.GetSingleChannelData(sFile, 5, startPos, endPos);
                        //有效值
                        rmsArray = calculateCorrugation.CalcCorrugationRmsProcess(data_miles, data_speed, channelData, fs, len_win_imp, FilterFreq_L, FilterFreq_H, len_win, Wavelen_L, Wavelen_H, len_downsample);
                        var rmsValueArray = calculateCorrugation.GetCalcCorrugationRmsValue(rmsArray);
                        citDataList.Add(rmsValueArray);
                        //区段大值
                        peakArray = calculateCorrugation.CalcCorrugationMaxProcess(rmsArray, len_merge);
                        peakValueList = calculateCorrugation.GetCalcCorrugationMax(peakArray); //peakValueList 里程、大值、速度
                        WritePeakInfoToIdf(idfFilePath, "AB_Vt_R_RMS_11" + "Peakvalue", peakValueList[0].ToList(), peakValueList[2].ToList(), peakValueList[1].ToList());

                        //轴箱左横 通道
                        channelData = citHelper.GetSingleChannelData(sFile, 6, startPos, endPos);
                        //有效值
                        rmsArray = calculateCorrugation.CalcCorrugationRmsProcess(data_miles, data_speed, channelData, fs, len_win_imp, FilterFreq_L, FilterFreq_H, len_win, Wavelen_L, Wavelen_H, len_downsample);
                        rmsValueArray = calculateCorrugation.GetCalcCorrugationRmsValue(rmsArray);
                        citDataList.Add(rmsValueArray);
                        //区段大值
                        peakArray = calculateCorrugation.CalcCorrugationMaxProcess(rmsArray, len_merge);
                        peakValueList = calculateCorrugation.GetCalcCorrugationMax(peakArray); //peakValueList 里程、大值、速度
                        WritePeakInfoToIdf(idfFilePath, "AB_Lt_L_RMS_11" + "Peakvalue", peakValueList[0].ToList(), peakValueList[2].ToList(), peakValueList[1].ToList());

                        CreateCitData(citFileNew, citDataList);

                        startPos = endPos;
                    }

                    //var channelList = citHelper.GetDataChannelInfoHead(sFile);

                    //string strContent = "";
                    //StringBuilder sb = new StringBuilder();
                    //for (int i = 0; i < channelList.Count; i++)
                    //{
                    //    sb.AppendLine(channelList[i].sID + "," + channelList[i].sNameCh + "," + channelList[i].sNameEn + "," + channelList[i].fOffset + "," + channelList[i].fScale + "," + channelList[i].sUnit);
                    //}

                    //strContent = sb.ToString();

                }

                MessageBox.Show("计算有效值完成", "消息", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("计算有效值出错：" + ex.Message + ex.StackTrace, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        /// <summary>
        /// 计算波磨指数平均值按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCorruAvg_Click(object sender, EventArgs e)
        {
            GetCitSourceCheckFileList();

            InvalidDataProcessing.CalculateCorrugationClass calculateCorrugation = new InvalidDataProcessing.CalculateCorrugationClass();

            if (citSourceCheckFileList.Count != 1)
            {
                MessageBox.Show("请选择一个cit文件", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                foreach (var sFile in citSourceCheckFileList)
                {
                    string citFileNew = sFile.Substring(0, sFile.Length - 4) + "corrugationRms.cit";
                    string idfFilePath = citFileNew.Replace(".cit", ".idf");

                    if (!File.Exists(idfFilePath))
                    {
                        MessageBox.Show("不存在" + idfFilePath + "该文件，请先计算有效值生成此文件", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    List<double> mileData = new List<double>();
                    List<double> peakData = new List<double>();
                    List<double> speedData = new List<double>();

                    //第一个通道【轴箱左垂】
                    idfHelper.ReadTablePeak(idfFilePath, "AB_Vt_L_RMS_11" + "Peakvalue", out mileData, out peakData, out speedData);
                    this.txtAvg1.Text = GetAvgValue(calculateCorrugation, mileData, peakData, speedData).ToString();

                    //第二个通道【轴箱右垂】
                    idfHelper.ReadTablePeak(idfFilePath, "AB_Vt_R_RMS_11" + "Peakvalue", out mileData, out peakData, out speedData);
                    //指数平均值
                    this.txtAvg2.Text = GetAvgValue(calculateCorrugation, mileData, peakData, speedData).ToString();

                    //第三个通道【轴箱左横】
                    idfHelper.ReadTablePeak(idfFilePath, "AB_Lt_L_RMS_11" + "Peakvalue", out mileData, out peakData, out speedData);
                    //指数平均值
                    this.txtAvg3.Text = GetAvgValue(calculateCorrugation, mileData, peakData, speedData).ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("计算波磨指数平均值出错：" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 获取波磨指数平均值
        /// </summary>
        /// <param name="calculateCorrugation"></param>
        /// <param name="mileData"></param>
        /// <param name="peakData"></param>
        /// <param name="speedData"></param>
        /// <returns></returns>
        private double GetAvgValue(InvalidDataProcessing.CalculateCorrugationClass calculateCorrugation,List<double> mileData, List<double> peakData, List<double> speedData)
        {
            //指数平均值
            MWNumericArray avgArray = calculateCorrugation.CalcCorrugationAvgProcess(mileData.ToArray(), peakData.ToArray(), speedData.ToArray());
            List<double[]> avgValueList = calculateCorrugation.GetCalcCorrugationAvg(avgArray);
            double s = 0;
            double k = 0;
            double avgValue = 0;
            for (int i = 0; i < avgValueList[0].Length; i++)
            {
                s += avgValueList[1][i] * avgValueList[2][i];
                k += avgValueList[2][i];
            }
            if (k != 0)
            {
                avgValue = Math.Round(s / k, 3);
            }

            return avgValue;
        }

        /// <summary>
        /// 提取波磨信息按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBMInfo_Click(object sender, EventArgs e)
        {
            try
            {
                int fs = Convert.ToInt32(this.txt_fs.Text);
                int len_downsample = Convert.ToInt32(this.txt_len_downsample.Text);
                int len_merge = Convert.ToInt32(this.txt_len_merge.Text);

                double thresh_tii = Convert.ToDouble(this.txt_thresh_tii.Text);

                GetCitSourceCheckFileList();

                InvalidDataProcessing.CalculateCorrugationClass calculateCorrugation = new InvalidDataProcessing.CalculateCorrugationClass();

                if (citSourceCheckFileList.Count != 1)
                {
                    MessageBox.Show("请选择一个cit文件", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                foreach (var sFile in citSourceCheckFileList)
                {
                    string citFileNew = sFile.Substring(0, sFile.Length - 4) + "corrugationRms.cit";

                    if (!File.Exists(citFileNew))
                    {
                        MessageBox.Show("路径下不存在" + citFileNew + "该文件,请计算有效值后,再提取波磨信息", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    var header = citHelper.GetDataInfoHead(citFileNew);
                    int bytesneed = header.iChannelNumber * 2 * 16000000;

                    long startPos = citHelper.GetSamplePointStartOffset(header.iChannelNumber, 4);

                    long citFileLength = citHelper.GetFileLength(sFile);

                    int count = Convert.ToInt32((citFileLength - startPos) / bytesneed);

                    double mean_rms_nacc1 = 0;
                    double mean_rms_nacc2 = 0;
                    double mean_rms_nacc3 = 0;

                    if (!double.TryParse(this.txtAvg1.Text.Trim(), out mean_rms_nacc1) || !double.TryParse(this.txtAvg2.Text.Trim(), out mean_rms_nacc2) || !double.TryParse(this.txtAvg3.Text.Trim(), out mean_rms_nacc3))
                    {
                        MessageBox.Show("通道波磨指数平均值为非数字");
                        return;
                    }

                    string channelName1 = citHelper.GetChannelNameEn(4, citFileNew);
                    string channelName2 = citHelper.GetChannelNameEn(5, citFileNew);
                    string channelName3 = citHelper.GetChannelNameEn(6, citFileNew);


                    List<double[]> result1 = new List<double[]>();
                    List<double[]> result2 = new List<double[]>();
                    List<double[]> result3 = new List<double[]>();

                    for (int i = 0; i < count; i++)
                    {
                        long endPos = startPos + bytesneed;

                        if (endPos < citFileLength)
                        {
                            try
                            {
                                double[] mileData = citHelper.GetMilesData(citFileNew, startPos, endPos);
                                double[] rmsData = citHelper.GetSingleChannelData(citFileNew, 4, startPos, endPos);
                                double[] speedData = citHelper.GetSingleChannelData(citFileNew, 3, startPos, endPos);


                                var result = GetCorrugationInfo(channelName1, mileData, rmsData, speedData, fs, len_downsample, mean_rms_nacc1, thresh_tii, calculateCorrugation);
                                result1.AddRange(result);

                                rmsData = citHelper.GetSingleChannelData(citFileNew, 5, startPos, endPos);
                                result = GetCorrugationInfo(channelName2, mileData, rmsData, speedData, fs, len_downsample, mean_rms_nacc2, thresh_tii, calculateCorrugation);
                                result2.AddRange(result);

                                rmsData = citHelper.GetSingleChannelData(citFileNew, 6, startPos, endPos);
                                result = GetCorrugationInfo(channelName3, mileData, rmsData, speedData, fs, len_downsample, mean_rms_nacc3, thresh_tii, calculateCorrugation);
                                result3.AddRange(result);

                                startPos = endPos;
                            }
                            catch (Exception ex)
                            {
                                break;
                            }
                        }
                    }

                    SaveCorrugationInfo(result1, channelName1, calculateCorrugation);
                    SaveCorrugationInfo(result2, channelName2, calculateCorrugation);
                    SaveCorrugationInfo(result3, channelName3, calculateCorrugation);

                    MessageBox.Show("提取波磨信息完成", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("提取波磨信息出错：" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private List<double[]> GetCorrugationInfo(string channelName, double[] mileData, double[] rmsData, double[] speedData, int fs, int len_downsample, double mean_rms_nacc1, double thresh_tii, InvalidDataProcessing.CalculateCorrugationClass calculateCorrugation)
        {
            List<double[]> bmInfoList=new List<double[]>();
            try
            {
                //提取波磨信息
                MWNumericArray bmInfoArray = calculateCorrugation.GetCorrugationInfomationProcess(mileData, rmsData, speedData, fs, len_downsample, mean_rms_nacc1, thresh_tii);
                bmInfoList = calculateCorrugation.GetCorrugationInfomation(bmInfoArray);

                return bmInfoList;
            }
            catch (Exception ex)
            {
                return bmInfoList;
            }
        }

        private void SaveCorrugationInfo(List<double[]> bmInfoList, string channelName, InvalidDataProcessing.CalculateCorrugationClass calculateCorrugation)
        {
            if (bmInfoList != null)
            {
                List<CorrugationInfoModel> listModel = new List<CorrugationInfoModel>();
                for (int i = 0; i < bmInfoList[0].Length; i++)
                {
                    CorrugationInfoModel model = new CorrugationInfoModel();
                    model.StartMile = bmInfoList[0][i].ToString();
                    model.EndMile = bmInfoList[1][i].ToString();
                    model.RmsValue = bmInfoList[2][i].ToString();
                    model.PeakValue = bmInfoList[3][i].ToString();
                    model.AvgSpeed = bmInfoList[4][i].ToString();
                    model.FirstBasicFrequency = bmInfoList[5][i].ToString();
                    model.WaveLength = bmInfoList[6][i].ToString();
                    model.EnergyRatio = bmInfoList[7][i].ToString();

                    listModel.Add(model);
                }
                string folderPath = this.textBoxFileDirectory.Text;
                calculateCorrugation.CreateExcel(listModel, folderPath, channelName + "Info");
            }
        }

        /// <summary>
        /// 提取波磨波形按钮时间
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBMWave_Click(object sender, EventArgs e)
        {
            try
            {
                int fs = Convert.ToInt32(this.txt_fs.Text);
                int len_win = Convert.ToInt32(this.txt_bm_len_win.Text);
                double thresh_tii = Convert.ToDouble(this.txt_thresh_tii.Text);

                GetCitSourceCheckFileList();

                InvalidDataProcessing.CalculateCorrugationClass calculateCorrugation = new InvalidDataProcessing.CalculateCorrugationClass();

                if (citSourceCheckFileList.Count != 1)
                {
                    MessageBox.Show("请选择一个cit文件", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                foreach (var sFile in citSourceCheckFileList)
                {
                    string citFileNew = sFile.Substring(0, sFile.Length - 4) + "corrugationRms.cit";

                    if (!File.Exists(citFileNew))
                    {
                        MessageBox.Show("路径下不存在" + citFileNew + "该文件,请计算有效值后,再提取波磨波形", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    GetCorrugationWaveData(sFile, citFileNew, fs, thresh_tii, len_win, calculateCorrugation);
                }

                MessageBox.Show("提取波磨波形完成", "信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("提取波磨波形出错：" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void GetCorrugationWaveAllData(string sFile, string citFileNew,int fs,double thresh_tii,int len_win, InvalidDataProcessing.CalculateCorrugationClass calculateCorrugation)
        {
            GetCorrugationWaveAllDataOneChannel(4, sFile, citFileNew, fs, thresh_tii, len_win, calculateCorrugation);
            GetCorrugationWaveAllDataOneChannel(5, sFile, citFileNew, fs, thresh_tii, len_win, calculateCorrugation);
            GetCorrugationWaveAllDataOneChannel(6, sFile, citFileNew, fs, thresh_tii, len_win, calculateCorrugation);
        }

        private void GetCorrugationWaveData(string sFile, string citFileNew, int fs, double thresh_tii, int len_win, InvalidDataProcessing.CalculateCorrugationClass calculateCorrugation)
        {
            GetCorrugationWaveOneChannel(4, sFile, citFileNew, fs, thresh_tii, len_win, calculateCorrugation);
            GetCorrugationWaveOneChannel(5, sFile, citFileNew, fs, thresh_tii, len_win, calculateCorrugation);
            GetCorrugationWaveOneChannel(6, sFile, citFileNew, fs, thresh_tii, len_win, calculateCorrugation);
        }

        private void GetCorrugationWaveAllDataOneChannel(int channelID, string sFile, string citFileNew, int fs, double thresh_tii, int len_win, InvalidDataProcessing.CalculateCorrugationClass calculateCorrugation)
        {
            string channelName = citHelper.GetChannelNameEn(channelID, citFileNew);

            string excelPath = this.textBoxFileDirectory.Text + "\\" + channelName + "Info.csv";
            List<CorrugationInfoModel> listModels = calculateCorrugation.GetCorrugationInfoList(excelPath);

            var data_miles = citHelper.GetMilesData(sFile);
            var data_speed = citHelper.GetSingleChannelData(sFile, 3);

            //轴箱左垂
            var channelData = citHelper.GetSingleChannelData(sFile, channelID);
            //提取波磨波形
            MWNumericArray bmWaveArray = calculateCorrugation.GetCorrugationWaveProcess(listModels, data_miles, data_speed, channelData, fs, thresh_tii, len_win);
            //波磨波形结果
            List<double[]> listResult = calculateCorrugation.GetCorrugationWave(bmWaveArray);
        }

        private void GetCorrugationWaveOneChannel(int channelID, string sFile, string citFileNew, int fs, double thresh_tii, int len_win, InvalidDataProcessing.CalculateCorrugationClass calculateCorrugation)
        {
            var header = citHelper.GetDataInfoHead(sFile);

            int bytesneed = header.iChannelNumber * 2 * 16000000;

            long startPos = citHelper.GetSamplePointStartOffset(header.iChannelNumber, channelID);

            long citFileLength = citHelper.GetFileLength(sFile);

            int count = Convert.ToInt32((citFileLength - startPos) / bytesneed);

            string channelName = citHelper.GetChannelNameEn(channelID, citFileNew);

            string excelPath = this.textBoxFileDirectory.Text + "\\" + channelName + "Info.csv";
            if (!File.Exists(excelPath))
            {
                MessageBox.Show("路径下不存在" + excelPath + "该文件,请提取波磨信息后再进行提取波磨波形", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            List<CorrugationInfoModel> listModels = calculateCorrugation.GetCorrugationInfoList(excelPath);

            string citFileWave = sFile.Substring(0, sFile.Length - 4) + "corrugationWave.cit";
            CreateWaveCitHeader(citFileWave, header);

            for (int i = 0; i < count; i++)
            {
                long endPos = startPos + bytesneed;

                var data_miles = citHelper.GetMilesData(sFile, startPos, endPos);
                var data_speed = citHelper.GetSingleChannelData(sFile, 3, startPos, endPos);

                //轴箱左垂
                var channelData = citHelper.GetSingleChannelData(sFile, channelID, startPos, endPos);

                //提取波磨波形
                MWNumericArray bmWaveArray = calculateCorrugation.GetCorrugationWaveProcess(listModels, data_miles, data_speed, channelData, fs, thresh_tii, len_win);

                List<double[]> listResult = calculateCorrugation.GetCorrugationWave(bmWaveArray);

                CreateCitData(citFileWave, listResult);

                startPos = endPos;
            }
        }

        #region 创建波磨波形wave cit文件

        /// <summary>
        /// 创建波磨波形CIT文件
        /// </summary>
        /// <param name="citFileName">文件名称路径</param>
        /// <param name="dataHeadInfo">文件头信息</param>
        private void CreateWaveCitHeader(string citFileName, DataHeadInfo dataHeadInfo)
        {
            dataHeadInfo.iChannelNumber = 12;

            if (dataHeadInfo.sTrackCode == "\0\0\0\0")
            {
                dataHeadInfo.sTrackCode = "";
            }

            List<DataChannelInfo> channelList = new List<DataChannelInfo>();
            channelList.Add(new DataChannelInfo { sID = 1, sNameCh = "Coru_Start", sNameEn = "波磨起始里程", fOffset = 0, fScale = 1, sUnit = "" });
            channelList.Add(new DataChannelInfo { sID = 2, sNameCh = "Coru_End", sNameEn = "波磨终止里程", fOffset = 0, fScale = 1, sUnit = "" });
            channelList.Add(new DataChannelInfo { sID = 3, sNameCh = "Coru_RMS", sNameEn = "波磨有效值", fOffset = 0, fScale = 1, sUnit = "" });

            channelList.Add(new DataChannelInfo { sID = 4, sNameCh = "Coru_Index", sNameEn = "波磨指数", fOffset = 0, fScale = 1, sUnit = "" });
            channelList.Add(new DataChannelInfo { sID = 5, sNameCh = "Speed", sNameEn = "速度", fOffset = 0, fScale = 1, sUnit = "" });
            channelList.Add(new DataChannelInfo { sID = 6, sNameCh = "Main_Freq", sNameEn = "主频", fOffset = 0, fScale = 1, sUnit = "" });

            channelList.Add(new DataChannelInfo { sID = 7, sNameCh = "Wave_Len", sNameEn = "波长", fOffset = 0, fScale = 1, sUnit = "" });
            channelList.Add(new DataChannelInfo { sID = 8, sNameCh = "ConE_rate", sNameEn = "能量集中率", fOffset = 0, fScale = 1, sUnit = "" });
            channelList.Add(new DataChannelInfo { sID = 9, sNameCh = "CoruSeg_Len", sNameEn = "波磨区段长度", fOffset = 0, fScale = 1, sUnit = "" });

            channelList.Add(new DataChannelInfo { sID = 10, sNameCh = "Coru_Orig", sNameEn = "波磨原始波形", fOffset = 0, fScale = 1, sUnit = "" });
            channelList.Add(new DataChannelInfo { sID = 11, sNameCh = "CoruSeg_mile", sNameEn = "波磨区段里程", fOffset = 0, fScale = 1, sUnit = "" });
            channelList.Add(new DataChannelInfo { sID = 12, sNameCh = "Len_Data", sNameEn = "数据长度", fOffset = 0, fScale = 1, sUnit = "" });
            
            citHelper.WriteCitFileHeadInfo(citFileName, dataHeadInfo, channelList);

            citHelper.WriteDataExtraInfo(citFileName, "");
        }


        #endregion


        /// <summary>
        /// 创建cit文件头
        /// </summary>
        /// <param name="citFileName">cit文件路径</param>
        /// <param name="dataHeadInfo">文件头文件信息</param>
        private void CreateCitHeader(string citFileName, DataHeadInfo dataHeadInfo)
        {
            dataHeadInfo.iChannelNumber = 6;

            if (dataHeadInfo.sTrackCode == "\0\0\0\0")
            {
                dataHeadInfo.sTrackCode = "";
            }

            List<DataChannelInfo> channelList = new List<DataChannelInfo>();
            channelList.Add(new DataChannelInfo { sID = 1, sNameCh = "公里", sNameEn = "KM", fOffset = 0, fScale = 1, sUnit = "Km" });
            channelList.Add(new DataChannelInfo { sID = 2, sNameCh = "米", sNameEn = "M", fOffset = 0, fScale = 4, sUnit = "m" });
            channelList.Add(new DataChannelInfo { sID = 3, sNameCh = "速度", sNameEn = "SPEED", fOffset = 0, fScale = 10, sUnit = "Km/h" });
            //channelList.Add(new DataChannelInfo { sID = 4, sNameCh = "AB_Vt_L_RMS_11corrugationRMS", sNameEn = "AB_Vt_L_RMS_11corrugationRMS", fOffset = 0, fScale = 100, sUnit = "" });
            //channelList.Add(new DataChannelInfo { sID = 5, sNameCh = "AB_Vt_R_RMS_11corrugationRMS", sNameEn = "AB_Vt_R_RMS_11corrugationRMS", fOffset = 0, fScale = 100, sUnit = "" });
            //channelList.Add(new DataChannelInfo { sID = 6, sNameCh = "AB_Lt_L_RMS_11corrugationRMS", sNameEn = "AB_Lt_L_RMS_11corrugationRMS", fOffset = 0, fScale = 100, sUnit = "" });

            channelList.Add(new DataChannelInfo { sID = 4, sNameCh = "AB_Vt_L_RMS_11" + "_coru", sNameEn = "AB_Vt_L_RMS_11"+"_coru", fOffset = 0, fScale = 100, sUnit = "" });
            channelList.Add(new DataChannelInfo { sID = 5, sNameCh = "AB_Vt_R_RMS_11" + "_coru", sNameEn = "AB_Vt_R_RMS_11"+"_coru", fOffset = 0, fScale = 100, sUnit = "" });
            channelList.Add(new DataChannelInfo { sID = 6, sNameCh = "AB_Lt_L_RMS_11" + "_coru", sNameEn = "AB_Lt_L_RMS_11"+"_coru", fOffset = 0, fScale = 100, sUnit = "" });

            citHelper.WriteCitFileHeadInfo(citFileName, dataHeadInfo, channelList);

            citHelper.WriteDataExtraInfo(citFileName, "");
        }

        /// <summary>
        /// 写入cit数据
        /// </summary>
        /// <param name="citFileName"></param>
        /// <param name="dataList"></param>
        private void CreateCitData(string citFileName,List<double[]> dataList)
        {
            citHelper.WriteChannelData(citFileName, dataList);
        }

        /// <summary>
        /// 创建idf文件
        /// </summary>
        /// <param name="idfFileName"></param>
        private void CreateIdfFile(string idfFileName)
        {
            idfHelper.CreateDB(idfFileName);
            idfHelper.CreateTableCitFileInfo(idfFileName);
            idfHelper.CreateTable(idfFileName, "AB_Vt_L_RMS_11" + "Peakvalue");
            idfHelper.CreateTable(idfFileName, "AB_Vt_R_RMS_11" + "Peakvalue");
            idfHelper.CreateTable(idfFileName, "AB_Lt_L_RMS_11" + "Peakvalue");
        }

        /// <summary>
        /// 写入citInfo信息
        /// </summary>
        /// <param name="idfFileName"></param>
        /// <param name="dataHeadInfo"></param>
        private void WriteCitInfoToIdf(string idfFileName,DataHeadInfo dataHeadInfo)
        {
            idfHelper.WriteTableCitFileInfo(idfFileName, dataHeadInfo);
        }

        /// <summary>
        /// 写入区段大值信息
        /// </summary>
        /// <param name="idfFilePath"></param>
        /// <param name="tableName"></param>
        /// <param name="data_KM"></param>
        /// <param name="data_SPEED"></param>
        /// <param name="data_Peak"></param>
        private void WritePeakInfoToIdf(string idfFilePath, String tableName, List<double> data_KM, List<double> data_SPEED, List<double> data_Peak)
        {
            idfHelper.InsertIntoSegmentPeakTable(idfFilePath, tableName, data_KM, data_SPEED, data_Peak);
        }

        

        

        



        /***
         * 
         * 
         * 
1,公里,KM,0,1,Km
2,米,M,0,4,m
3,速度,SPEED,0,10,Km/h
4,AB_Vt_L_RMS_11,AB_Vt_L_RMS_11,0,100,    --轴箱左垂
5,AB_Vt_R_RMS_11,AB_Vt_R_RMS_11,0,100,    --轴箱右垂
6,AB_Lt_L_RMS_11,AB_Lt_L_RMS_11,0,100,    --轴箱左横
7,Fr_Vt_L_11,Fr_Vt_L_11,0,250,g
8,Fr_Lt_L_11,Fr_Lt_L_11,0,250,g
9,CB_Lg_R_11,CB_Lg_R_11,0,10000,g
10,CB_Lt_R_11,CB_Lt_R_11,0,10000,g
11,CB_Vt_R_11,CB_Vt_R_11,0,10000,g

         * 
         * */

    }
}
