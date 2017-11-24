using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using DataProcess;
using InGraph.Classes;
using InGraph.DataProcessClass;
using System.Threading;

namespace InGraph.Forms
{
    /// <summary>
    /// 窗体--轴箱加速度批处理计算
    /// </summary>
    public partial class AcclrtnBatProcsForm : Form
    {
        string[] sFiles = null;//同一个目录下的cit文件列表
        CitProcessClass citProcessCls = null;
        private float fRuntime = 0f;
        private float fBatStartTime = 0f;
        private List<String> citSourceCheckFileList = new List<String>();
        List<String> citSctnCheckFilePathList = new List<String>();
        bool isCalcRmsFinished = false;

        List<GEO2CITBind> listGEO2CIT = null;
        CITDataProcess citDataProcess = null;


        public AcclrtnBatProcsForm()
        {
            InitializeComponent();

            citProcessCls = new CitProcessClass();
            citDataProcess = new CITDataProcess();
        }

        delegate void GetDataPreCallBack();
        private void GetDataPre()
        {
            if (this.listViewFile.InvokeRequired)
            {
                GetDataPreCallBack g = new GetDataPreCallBack(GetDataPre);
                this.Invoke(g, new object[] { });
            } 
            else
            {
                listViewFile.Enabled = false;
                Application.DoEvents();
                GetData();
                Application.DoEvents();
                listViewFile.Enabled = true;
            }

        }
        private void GetData()
        {
            listViewFile.Items.Clear();
            
            try
            {
                sFiles = Directory.GetFiles(textBoxFileDirectory.Text, "*.cit", checkBoxbSubDirectory.Checked ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
            }
            catch
            {
                //MessageBox.Show("获取所有文件出错，请检查子目录");
                return;
            }
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

        private void GetDataRmsIdf()
        {
            listViewFile.Items.Clear();

            try
            {
                sFiles = Directory.GetFiles(textBoxFileDirectory.Text, "*Rms.idf", checkBoxbSubDirectory.Checked ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
            }
            catch
            {
                //MessageBox.Show("获取所有文件出错，请检查子目录");
                return;
            }
            listViewFile.BeginUpdate();
            foreach (string v in sFiles)
            {

                string status = citProcessCls.ReadTableCitFileInfo(v);
                string[] sDataInfoHead = status.Split(new char[] { ',' });
                // iDataType; sDataVersion; sTrackCode; sTrackName; iDir; 
                //sTrain; sDate; sTime; iRunDir;iKmInc; 
                //fkmFrom; fkmTo; iSmaleRate; iChannelNumber;
                if (sDataInfoHead[0].Contains("0"))
                {
                    listViewFile.Items.Add(sDataInfoHead[1]);

                    listViewFile.Items[listViewFile.Items.Count - 1].SubItems.Add(sDataInfoHead[2]);
                    listViewFile.Items[listViewFile.Items.Count - 1].SubItems.Add(sDataInfoHead[3]);
                    listViewFile.Items[listViewFile.Items.Count - 1].SubItems.Add(sDataInfoHead[4]);
                    listViewFile.Items[listViewFile.Items.Count - 1].SubItems.Add(sDataInfoHead[5]);
                    listViewFile.Items[listViewFile.Items.Count - 1].SubItems.Add(sDataInfoHead[6]);
                    listViewFile.Items[listViewFile.Items.Count - 1].SubItems.Add(sDataInfoHead[7]);
                    listViewFile.Items[listViewFile.Items.Count - 1].SubItems.Add(Path.GetFileName(v));
                    listViewFile.Items[listViewFile.Items.Count - 1].SubItems.Add((new FileInfo(v)).Length.ToString());
                    listViewFile.Items[listViewFile.Items.Count - 1].SubItems.Add(Path.GetDirectoryName(v));
                }


                Application.DoEvents();

            }
            listViewFile.EndUpdate();
        }

        //private void CalcRMS()
        //{
        //    int sampleFreq = Int32.Parse(textBoxSampleFrep.Text);
        //    int maxFreq = Int32.Parse(textBoxMaxFreq.Text);
        //    int windowLen = Int32.Parse(textBoxWindowLen.Text);

        //    foreach (String citFilePath in citSourceCheckFileList)
        //    {
        //        try
        //        {
        //            citProcessCls.CreateAllSectionFile(citFilePath, sampleFreq, maxFreq, windowLen,freq_Low,frep_High);
        //        }
        //        catch (System.Exception ex)
        //        {
        //            MessageBox.Show(ex.Message);
        //            MessageBox.Show(ex.StackTrace);
        //        }
        //    }
        //}

        private List<String> CalcRMSNew()
        {
            List<String> m_sectionFilePathList = new List<String>();

            int sampleFreq = Int32.Parse(textBoxSampleFrep.Text);
            int maxFreq = Int32.Parse(textBoxMaxFreq.Text);
            //2017-06-23 增加下线频率参数
            int minFreq = Int32.Parse(textBoxMinFreq.Text);
            int windowLen = Int32.Parse(textBoxWindowLen.Text);

            foreach (String citFilePath in citSourceCheckFileList)
            {
                try
                {
                    List<string> tmpSectionFilePathList = citProcessCls.CreateAllSectionFileNew(citFilePath, sampleFreq, maxFreq, minFreq, windowLen, freq_Low, frep_High, freq_Low_Low, frep_High_High);
                    if (tmpSectionFilePathList != null && tmpSectionFilePathList.Count > 0)
                    {
                        m_sectionFilePathList.AddRange(tmpSectionFilePathList);
                    }
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    MessageBox.Show(ex.StackTrace);
                }
            }

            return m_sectionFilePathList;
        }

        private void MaxIn50M()
        {
            try
            {
                citProcessCls.CalcSegmentRMS(citSctnCheckFilePathList, Int32.Parse(textBoxSectionLen.Text));

            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
                MessageBox.Show(ex.StackTrace);
            }
        }
        private void BatProcess()
        {
            List<String> m_sectionFilePathList = null;

            try
            {
                //CalcRMS();
                // liyang:  这一步生成两个新的cit文件
                // 原始cit是：CitData_160424063432_CNGX.cit, 新生成的是： CitData_160424063432_CNGX_new.cit 和 CitData_160424063432_CNGX_new_02.cit
                // m_sectionFilePathList == CitData_160424063432_CNGX_new_02.cit
                m_sectionFilePathList=CalcRMSNew();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            //List<String> citSourceCheckFileWithoutExtensionList = new List<String>();
            //foreach (String filePath in citSourceCheckFileList)
            //{
            //    String tmpName = Path.GetFileNameWithoutExtension(filePath);
            //    citSourceCheckFileWithoutExtensionList.Add(tmpName);
            //}

            citSctnCheckFilePathList.Clear();

            //sFiles = Directory.GetFiles(textBoxFileDirectory.Text, "*.cit", checkBoxbSubDirectory.Checked ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);

            //foreach (String filePath in sFiles)
            //{
            //    String tmpName = Path.GetFileNameWithoutExtension(filePath);
            //    foreach (String tmpStr in citSourceCheckFileWithoutExtensionList)
            //    {
            //        if (tmpName.Contains(tmpStr) && (tmpName.Length > tmpStr.Length))
            //        {
            //            citSctnCheckFilePathList.Add(filePath);
            //            break;
            //        }
            //    }
            //}

            citSctnCheckFilePathList.AddRange(m_sectionFilePathList);

            try
            {
                // 这一步完成后，生成： CitData_160424063432_CNGX_new_02_Rms.idf
                MaxIn50M();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
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

        private void GetCitSourceCheckFileList_150C()
        {
            InitListGEO2CIT();

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

                String destDirectory = Path.GetDirectoryName(citFilePath);
                String destCitName = Path.GetFileNameWithoutExtension(citFilePath) + "_new.cit";
                String destFilePath = Path.Combine(destDirectory, destCitName);
                if (File.Exists(destFilePath))
                {
                    File.Delete(destFilePath);
                }
                File.Copy(citFilePath, destFilePath, true);

                String sLineCode = listViewFile.CheckedItems[i].SubItems[1].Text;
                String sLineName = listViewFile.CheckedItems[i].SubItems[0].Text;
                citDataProcess.ModifyCITHeader_150C(destFilePath, sLineCode, sLineName, listGEO2CIT);

                citSourceCheckFileList.Add(destFilePath);
            }
        }

        private void InitListGEO2CIT()
        {
            String configPath = Path.Combine(Application.StartupPath, "GEOConfig");
            configPath = Path.Combine(configPath, "CRH2-150C - AC.csv");
            try
            {
                if (listGEO2CIT == null || listGEO2CIT.Count == 0)
                {
                    listGEO2CIT = new List<GEO2CITBind>();
                    StreamReader sr = new StreamReader(configPath, Encoding.Default);
                    while (sr.Peek() != -1)
                    {
                        string[] sSplit = sr.ReadLine().Trim().Split(new char[] { '=' });
                        GEO2CITBind fa = new GEO2CITBind();
                        fa.sCIT = sSplit[0].Trim();
                        fa.sGEO = sSplit[1].Trim();
                        fa.sChinese = sSplit[2].Trim();
                        listGEO2CIT.Add(fa);
                    }
                    sr.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void buttonFolderBrowser_Click(object sender, EventArgs e)
        {
            DialogResult dr = folderBrowserDialog1.ShowDialog();
            if (dr == DialogResult.OK)
            {
                textBoxFileDirectory.Text = folderBrowserDialog1.SelectedPath;
            }

            GetDataPre();
        }


        private void checkBoxbSubDirectory_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxSegmentMax.Checked)
            {
                GetDataRmsIdf();
            } 
            else
            {
                GetDataPre();
            }          
        
        }

        private void listViewFile_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                //双击保持状态
                if (e.Clicks > 1)
                {
                    ListViewItem lvi = listViewFile.GetItemAt(e.X, e.Y);
                    if (lvi == null)
                        return;
                    lvi.Checked = !lvi.Checked;
                }

                //单击选中
                if (listViewFile.GetItemAt(e.X, e.Y).Checked)
                {
                    listViewFile.GetItemAt(e.X, e.Y).Checked = false;
                }
                else
                {
                    listViewFile.GetItemAt(e.X, e.Y).Checked = true;
                }

            }
            catch
            {

            }
        }

        private void listViewFile_EnabledChanged(object sender, EventArgs e)
        {
            if (listViewFile.Enabled)
            {

                for (int i = 0; i < listViewFile.Items.Count; i++)
                {
                    listViewFile.Items[i].UseItemStyleForSubItems = true;
                    try
                    {
                        if (listViewFile.Items[i].SubItems[2].Text.EndsWith("下"))
                        {
                            listViewFile.Items[i].BackColor = Color.LightCyan;                            
                        }
                        else
                        {
                            listViewFile.Items[i].BackColor = Color.LightBlue;
                        }
                    }
                    catch
                    {

                    }
                }
            }
        }

        private void setControlEnable(bool b)
        {
            //通带频率--架构
            textBox_Fr_Lt_Hz_H.Enabled = b;
            textBox_Fr_Lt_Hz_L.Enabled = b;
            textBox_Fr_Vt_Hz_H.Enabled = b;
            textBox_Fr_Vt_Hz_L.Enabled = b;
            //阻带频率--架构
            textBox_Fr_Lt_Hz_H_H.Enabled = b;
            textBox_Fr_Lt_Hz_L_L.Enabled = b;
            textBox_Fr_Vt_Hz_H_H.Enabled = b;
            textBox_Fr_Vt_Hz_L_L.Enabled = b;
            //通带频率--架构
            textBox_CB_Lg_Hz_H.Enabled = b;
            textBox_CB_Lg_Hz_L.Enabled = b;
            textBox_CB_Lt_Hz_H.Enabled = b;
            textBox_CB_Lt_Hz_L.Enabled = b;
            textBox_CB_Vt_Hz_H.Enabled = b;
            textBox_CB_Vt_Hz_L.Enabled = b;
            //阻带频率--架构
            textBox_CB_Lg_Hz_H_H.Enabled = b;
            textBox_CB_Lg_Hz_L_L.Enabled = b;
            textBox_CB_Lt_Hz_H_H.Enabled = b;
            textBox_CB_Lt_Hz_L_L.Enabled = b;
            textBox_CB_Vt_Hz_H_H.Enabled = b;
            textBox_CB_Vt_Hz_L_L.Enabled = b;

            textBoxSampleFrep.Enabled = b;
            textBoxMaxFreq.Enabled = b;
            textBoxSectionLen.Enabled = b;
            buttonCalcMovingRMS.Enabled = b;

            textBoxWindowLen.Enabled = b;
            buttonMaxIn50M.Enabled = b;

            checkBox150C.Enabled = b;
        }


        private void CalAvgRms(List<String> rmsIdfPaths,String channelName)
        {
            double avg_rms = 0;
            double avg_spd = 0;
            List<double> rmsList = new List<double>();
            List<double> spdList = new List<double>();
            

            foreach (string rmsIdfPath in rmsIdfPaths)
            {
                List<double> rmsListTmp = new List<double>();
                List<double> spdListTmp = new List<double>();

                List<String> tableNames = CommonClass.wdp.GetTableNames(rmsIdfPath);
                String tableName = null;
                foreach (String tName in tableNames)
                {
                    if (tName.Contains(channelName))
                    {
                        tableName = tName;
                        break;
                    }
                }
                foreach (String tName in tableNames)
                {
                    if (tName.Contains("AB_Lt_L"))
                    {
                        label4.Text = "左轴横";
                        label8.Text = "左轴横";
                        break;
                    }
                    if (tName.Contains("AB_Lt_R"))
                    {
                        label4.Text = "右轴横";
                        label8.Text = "右轴横";
                    }
                }


                citProcessCls.ReadTableRms(rmsIdfPath, tableName, out rmsListTmp, out spdListTmp);

                rmsList.AddRange(rmsListTmp);
                spdList.AddRange(spdListTmp);
            }

            citProcessCls.sub_calculate_mean_rms(spdList.ToArray(), rmsList.ToArray(), out avg_rms, out avg_spd);

            foreach (TextBox tb in tbListAvgSeg)
            {
                if (tb.Name.Contains(channelName))
                {
                    tb.Text = avg_rms.ToString();
                    break;
                }
            }
            foreach (TextBox tb in tbListAvgSpd)
            {
                if (tb.Name.Contains(channelName))
                {
                    tb.Text = avg_spd.ToString();
                    break;
                }
            }
        }
        private void CalPeak(List<String> rmsIdfPaths, String channelName)
        {
            foreach (TextBox tb in tbListAvgSeg)
            {
                if (tb.Name.Contains(channelName))
                {
                    double tmpVal = 0;
                    Boolean tmp = double.TryParse(tb.Text,out tmpVal);
                    if (tmp == false)
                    {
                        return;
                    }
                    break;
                }
            }


            double seg_rms = 0;
            List<double> rmsList = new List<double>();

            foreach (TextBox tb in tbListAvgSeg)
            {
                if (tb.Name.Contains(channelName))
                {
                    seg_rms = double.Parse(tb.Text);
                    break;
                }
            }


            foreach (string rmsIdfPath in rmsIdfPaths)
            {
                List<double> rmsListTmp = new List<double>();
                List<double> spdListTmp = new List<double>();

                List<String> tableNames = CommonClass.wdp.GetTableNames(rmsIdfPath);
                String tableName = null;
                foreach (String tName in tableNames)
                {
                    if (tName.Contains(channelName))
                    {
                        tableName = tName;
                        break;
                    }
                }


                citProcessCls.ReadTableRms(rmsIdfPath, tableName, out rmsListTmp, out spdListTmp);

                double[] peakArray = citProcessCls.sub_calculate_peak_factor(rmsListTmp.ToArray(), seg_rms);

                citProcessCls.UpdatePeak(rmsIdfPath, tableName, peakArray);
            }         
        }


        List<TextBox> tbListAvgSeg = new List<TextBox>();
        List<TextBox> tbListAvgSpd = new List<TextBox>();
        private void InitTbAvgSeg()
        {
            if (tbListAvgSeg.Count > 0)
            {
                tbListAvgSeg.Clear();
            }

            tbListAvgSeg.Add(textBox_AB_Lt_L_Avg_Seg);
            tbListAvgSeg.Add(textBox_AB_Vt_L_Avg_Seg);
            tbListAvgSeg.Add(textBox_AB_Vt_R_Avg_Seg);
        }
        private void InitTbAvgSpd()
        {
            if (tbListAvgSpd.Count > 0)
            {
                tbListAvgSpd.Clear();
            }

            tbListAvgSpd.Add(textBox_AB_Lt_L_Avg_Spd);
            tbListAvgSpd.Add(textBox_AB_Vt_L_Avg_Spd);
            tbListAvgSpd.Add(textBox_AB_Vt_R_Avg_Spd);
        }

        #region 存放通带下限
        /// <summary>
        /// 存放通带下限
        /// </summary>
        List<TextBox> freq_Low = new List<TextBox>();//存放通带下限
        #endregion
        #region 存放通带上限
        /// <summary>
        /// 存放通带上限
        /// </summary>
        List<TextBox> frep_High = new List<TextBox>();//存放通带上限
        #endregion

        #region 存放阻带下限
        /// <summary>
        /// 存放阻带下限
        /// </summary>
        List<TextBox> freq_Low_Low = new List<TextBox>();//存放阻带下限
        #endregion
        #region 存放阻带上限
        /// <summary>
        /// 存放阻带上限
        /// </summary>
        List<TextBox> frep_High_High = new List<TextBox>();//存放阻带上限
        #endregion

        private Boolean ParseTextBoxes()
        {
            Boolean retVal = true;
            float textBoxValue = 0;
            try
            {
                #region 通带
                //构架横
                textBoxValue = float.Parse(textBox_Fr_Lt_Hz_H.Text);
                textBoxValue = float.Parse(textBox_Fr_Lt_Hz_L.Text);
                //构架垂
                textBoxValue = float.Parse(textBox_Fr_Vt_Hz_H.Text);
                textBoxValue = float.Parse(textBox_Fr_Vt_Hz_L.Text);
                //车体横
                textBoxValue = float.Parse(textBox_CB_Lt_Hz_H.Text);
                textBoxValue = float.Parse(textBox_CB_Lt_Hz_L.Text);
                //车体垂
                textBoxValue = float.Parse(textBox_CB_Vt_Hz_H.Text);
                textBoxValue = float.Parse(textBox_CB_Vt_Hz_L.Text);
                //车体纵
                textBoxValue = float.Parse(textBox_CB_Lg_Hz_H.Text);
                textBoxValue = float.Parse(textBox_CB_Lg_Hz_L.Text);
                #endregion

                #region 阻带
                //构架横
                textBoxValue = float.Parse(textBox_Fr_Lt_Hz_H_H.Text);
                textBoxValue = float.Parse(textBox_Fr_Lt_Hz_L_L.Text);
                //构架垂
                textBoxValue = float.Parse(textBox_Fr_Vt_Hz_H_H.Text);
                textBoxValue = float.Parse(textBox_Fr_Vt_Hz_L_L.Text);
                //车体横
                textBoxValue = float.Parse(textBox_CB_Lt_Hz_H_H.Text);
                textBoxValue = float.Parse(textBox_CB_Lt_Hz_L_L.Text);
                //车体垂
                textBoxValue = float.Parse(textBox_CB_Vt_Hz_H_H.Text);
                textBoxValue = float.Parse(textBox_CB_Vt_Hz_L_L.Text);
                //车体纵
                textBoxValue = float.Parse(textBox_CB_Lg_Hz_H_H.Text);
                textBoxValue = float.Parse(textBox_CB_Lg_Hz_L_L.Text);
                #endregion
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
                retVal = false;

                return retVal;
            }

            //通带下限
            freq_Low = new List<TextBox>();
            freq_Low.Add(textBox_Fr_Lt_Hz_L);
            freq_Low.Add(textBox_Fr_Vt_Hz_L);
            freq_Low.Add(textBox_CB_Vt_Hz_L);
            freq_Low.Add(textBox_CB_Lt_Hz_L);
            freq_Low.Add(textBox_CB_Lg_Hz_L);
            //通带上限
            frep_High = new List<TextBox>();
            frep_High.Add(textBox_Fr_Vt_Hz_H);
            frep_High.Add(textBox_Fr_Lt_Hz_H);
            frep_High.Add(textBox_CB_Vt_Hz_H);
            frep_High.Add(textBox_CB_Lt_Hz_H);
            frep_High.Add(textBox_CB_Lg_Hz_H);


            //阻带下限
            freq_Low_Low = new List<TextBox>();
            freq_Low_Low.Add(textBox_Fr_Lt_Hz_L_L);
            freq_Low_Low.Add(textBox_Fr_Vt_Hz_L_L);
            freq_Low_Low.Add(textBox_CB_Vt_Hz_L_L);
            freq_Low_Low.Add(textBox_CB_Lt_Hz_L_L);
            freq_Low_Low.Add(textBox_CB_Lg_Hz_L_L);
            //阻带上限
            frep_High_High = new List<TextBox>();
            frep_High_High.Add(textBox_Fr_Vt_Hz_H_H);
            frep_High_High.Add(textBox_Fr_Lt_Hz_H_H);
            frep_High_High.Add(textBox_CB_Vt_Hz_H_H);
            frep_High_High.Add(textBox_CB_Lt_Hz_H_H);
            frep_High_High.Add(textBox_CB_Lg_Hz_H_H);


            return retVal;
        }

        /// <summary>
        /// 计算加速度有效值
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonCalcMovingRMS_Click(object sender, EventArgs e)
        {
            //如果解析文本框出错，则返回
            if (ParseTextBoxes() == false)
            {
                return;
            }

            if (CommonClass.listDIC.Count > 0)
            {
                MessageBox.Show("请关闭波形文件再执行此功能！");
                return;
            }

            if (checkBox150C.Checked)
            {
                GetCitSourceCheckFileList_150C();
            } 
            else
            {
                GetCitSourceCheckFileList();
            }
            

            if (citSourceCheckFileList.Count == 0)
            {
                MessageBox.Show("没有选择加速度数据文件！");
                return;
            }

            pictureBoxCalcRMS.Visible = true;            
            setControlEnable(false);
            checkBoxBatProcs.Enabled = false;
            
            
            toolStripStatusLabelStatus.Text = "正在计算......";
            fRuntime = Environment.TickCount & 0x7fffffff;
            backgroundWorkerCalcRMS.RunWorkerAsync();

            timer1.Enabled = true;
            timer1.Start();
        }

        private void backgroundWorkerCalcRMS_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            pictureBoxCalcRMS.Visible = false;
            setControlEnable(true);
            checkBoxBatProcs.Enabled = true;
            timer1.Stop();
            timer1.Enabled = false;
            
            if (checkBoxBatProcs.Checked == false)
            {
                fRuntime = ((Environment.TickCount & 0x7fffffff) - fRuntime) / 1000f;
                toolStripStatusLabelStatus.Text = "加速度有效值计算完毕 耗时(秒)" + fRuntime.ToString("f2");
            }
            
            isCalcRmsFinished = true;
        }

        private void backgroundWorkerCalcRMS_DoWork(object sender, DoWorkEventArgs e)
        {
            //CalcRMS();

            List<String> m_sectionFilePathList = null;

            try
            {
                m_sectionFilePathList = CalcRMSNew();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void buttonMaxIn50M_Click(object sender, EventArgs e)
        {
            citSctnCheckFilePathList.Clear();
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
                citSctnCheckFilePathList.Add(citFilePath);
            }


            pictureBoxMaxin50M.Visible = true;
            setControlEnable(false);
            checkBoxBatProcs.Enabled = false;
            toolStripStatusLabelStatus.Text = "正在计算......";
            fRuntime = Environment.TickCount & 0x7fffffff;
            backgroundWorkerMaxIn50M.RunWorkerAsync();

        }

        private void backgroundWorkerMaxIn50M_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            pictureBoxMaxin50M.Visible = false;
            setControlEnable(true);
            checkBoxBatProcs.Enabled = true;

            fRuntime = ((Environment.TickCount & 0x7fffffff) - fRuntime) / 1000f;
            toolStripStatusLabelStatus.Text = "50米区段大值计算完毕并导出 耗时(秒)" + fRuntime.ToString("f2");
        }

        private void backgroundWorkerMaxIn50M_DoWork(object sender, DoWorkEventArgs e)
        {
            MaxIn50M();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            GetDataPre();
            Application.DoEvents();
        }

        private void checkBoxBatProcs_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxBatProcs.Checked)
            {                
                setControlEnable(false);
                buttonBatProcs.Enabled = true;
            } 
            else
            {                
                setControlEnable(true);                
                buttonBatProcs.Enabled = false;
            }
        }

        private void buttonBatProcs_Click(object sender, EventArgs e)
        {
            //如果解析文本框出错，则返回
            if (ParseTextBoxes() == false)
            {
                return;
            }

            if (CommonClass.listDIC.Count > 0)
            {
                MessageBox.Show("请关闭波形文件再执行此功能！");
                return;
            }


            if (checkBox150C.Checked)
            {
                GetCitSourceCheckFileList_150C();
            }
            else
            {
                GetCitSourceCheckFileList();
            }

            pictureBoxCalcRMS.Visible = true;
            checkBoxBatProcs.Enabled = false;
            buttonBatProcs.Enabled = false;

            fRuntime = Environment.TickCount & 0x7fffffff;
            fBatStartTime = fRuntime;
            backgroundWorkerBat.RunWorkerAsync();
            toolStripStatusLabelStatus.Text = "正在计算......";

            timer1.Enabled = true;
            timer1.Start();
        }

        private void backgroundWorkerBat_DoWork(object sender, DoWorkEventArgs e)
        {
            BatProcess();
        }

        private void backgroundWorkerBat_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            pictureBoxCalcRMS.Visible = false;
            checkBoxBatProcs.Enabled = true;
            buttonBatProcs.Enabled = true;
            timer1.Stop();
            timer1.Enabled = false;

            fRuntime = ((Environment.TickCount & 0x7fffffff) - fBatStartTime) / 1000f;
            toolStripStatusLabelStatus.Text = "批计算完毕并导出 耗时(秒)" + fRuntime.ToString("f2");
        }

        private void checkBoxSegmentMax_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxSegmentMax.Checked)
            {
                checkBox150C.Enabled = false;
                panelAcle.Enabled = false;
                panelSeg.Enabled = false;
                panelBatProcess.Enabled = false;
                buttonAvgSeg.Enabled = true;
                panelPeak.Enabled = true;
                panelSpd.Enabled = true;

                GetDataRmsIdf();

            } 
            else
            {
                checkBox150C.Enabled = true;
                panelAcle.Enabled = true;
                panelSeg.Enabled = true;
                panelBatProcess.Enabled = true;
                buttonAvgSeg.Enabled = false;
                panelPeak.Enabled = false;
                panelSpd.Enabled = false;

                GetDataPre();
            }
        }

        private void buttonAvgSeg_Click(object sender, EventArgs e)
        {
            InitTbAvgSeg();
            InitTbAvgSpd();

            citSctnCheckFilePathList.Clear();
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
                citSctnCheckFilePathList.Add(citFilePath);
            }

            CalAvgRms(citSctnCheckFilePathList, "AB_Lt");
            CalAvgRms(citSctnCheckFilePathList, "AB_Vt_L");
            CalAvgRms(citSctnCheckFilePathList, "AB_Vt_R");
        }

        private void buttonPeak_Click(object sender, EventArgs e)
        {
            // liyang: 去掉这个sleep.
            //Thread.Sleep(5000);
            InitTbAvgSeg();
            InitTbAvgSpd();

            panelAvgSeg.Enabled = false;
            panelPeak.Enabled = false;
            panelSpd.Enabled = false;

            citSctnCheckFilePathList.Clear();
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
                citSctnCheckFilePathList.Add(citFilePath);
            }

            CalPeak(citSctnCheckFilePathList, "AB_Lt");
            CalPeak(citSctnCheckFilePathList, "AB_Vt_L");
            CalPeak(citSctnCheckFilePathList, "AB_Vt_R");

            MessageBox.Show("写入轨道冲击指数成功");
            panelAvgSeg.Enabled = true;
            panelPeak.Enabled = true;
            panelSpd.Enabled = true;
        }

    }
}
