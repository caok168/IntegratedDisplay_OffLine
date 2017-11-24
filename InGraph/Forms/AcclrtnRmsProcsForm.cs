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
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;

namespace InGraph.Forms
{
    public partial class AcclrtnRmsProcsForm : Form
    {
        private List<String> citSourceCheckFileList = new List<String>();

        public AcclrtnRmsProcsForm()
        {
            InitializeComponent();
        }

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

        private void btn_BuildExcel_Click(object sender, EventArgs e)
        {
            GetCitSourceCheckFileList();

            if (this.ckb_merge.Checked)
            {
                CitMergeCalculation();
            }
            else
            {
                CommonCalculation();
            }
        }

        /// <summary>
        /// 普通逐个计算
        /// </summary>
        private void CommonCalculation()
        {

            try
            {
                DataProcess.CITDataProcess citProcess = new DataProcess.CITDataProcess();
                InvalidDataProcessing.CalculateDistrubtionClass calculateDistrubtion = new InvalidDataProcessing.CalculateDistrubtionClass();

                foreach (var sFile in citSourceCheckFileList)
                {
                    int channelId1 = citProcess.GetChannelNumberByChannelNameLike(sFile, "SPEED", "速度");
                    int channelId2 = citProcess.GetChannelNumberByChannelNameLike(sFile, "AB_Vt_L_RMS", "左轴垂有效值");
                    int channelId3 = citProcess.GetChannelNumberByChannelNameLike(sFile, "AB_Vt_R_RMS", "右轴垂有效值");
                    int channelId4 = citProcess.GetChannelNumberByChannelNameLike(sFile, "AB_Lt_L_RMS", "左轴横有效值");

                    double[] d1 = citProcess.GetSingleChannelData(sFile, channelId1);
                    //double[] d2 = citProcess.GetSingleChannelData(sFile, channelId2);
                    //double[] d3 = citProcess.GetSingleChannelData(sFile, channelId3);
                    //double[] d4 = citProcess.GetSingleChannelData(sFile, channelId4);

                    string folderPath = sFile.Substring(0, sFile.LastIndexOf('\\'));
                    string fileName = sFile.Substring(sFile.LastIndexOf('\\') + 1);

                    //CreateExcelNew(d1, folderPath, "cit数据1", "速度");
                    //CreateExcelNew(d2, folderPath, "cit数据2", "左轴垂有效值AB_Vt_L_RMS");

                    double rmsMean1 = Convert.ToDouble(this.txtRmsMean1.Text);
                    double rmsMean2 = Convert.ToDouble(this.txtRmsMean2.Text);
                    double rmsMean3 = Convert.ToDouble(this.txtRmsMean3.Text);

                    double[] d2 = citProcess.GetSingleChannelData(sFile, channelId2);

                    calculateDistrubtion.CalDistrubtionProcess(d2, d1, folderPath, fileName + "左轴垂_rms_distribution", rmsMean1, fileName + "左轴垂_tii_distribution");
                    Array.Clear(d2, 0, d2.Length);

                    double[] d3 = citProcess.GetSingleChannelData(sFile, channelId3);
                    calculateDistrubtion.CalDistrubtionProcess(d3, d1, folderPath, fileName + "右轴垂_rms_distribution", rmsMean2, fileName + "右轴垂_tii_distribution");
                    Array.Clear(d3, 0, d3.Length);

                    double[] d4 = citProcess.GetSingleChannelData(sFile, channelId4);
                    calculateDistrubtion.CalDistrubtionProcess(d4, d1, folderPath, fileName + "左轴横_rms_distribution", rmsMean3, fileName + "左轴横_tii_distribution");
                    Array.Clear(d4, 0, d4.Length);

                    MessageBox.Show("完成");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + ex.StackTrace);
            }

        }

        private void CreateExcelNew(double[] d1, string folderPath, string fileName, string sheetName)
        {
            HSSFWorkbook workbook = new HSSFWorkbook();
            ISheet sheet = workbook.CreateSheet(sheetName);

            //IRow rowHeader = sheet.CreateRow(0);
            ICellStyle style = workbook.CreateCellStyle();
            style.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
            style.VerticalAlignment = VerticalAlignment.Center;


            int rowNum = d1.Length / 250;
            //rowNum = 25000;
            int no = 0;
            int no2 = 0;
            for (int j = 25000; j < rowNum; j++)
            {
                IRow row = sheet.CreateRow(j);
                no = j * 250;
                for (int col = 0; col < 250; col++)
                {
                    no2 = no + col;
                    if (no2 < d1.Length)
                    {
                        ICell cell1 = row.CreateCell(col);
                        cell1.SetCellValue(d1[no2]);
                        cell1.CellStyle = style;
                    }
                }
            }

            Array.Clear(d1, 0, d1.Length);

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);
            string fileNamePath = folderPath + "\\" + fileName + ".xls";
            FileStream fs = new FileStream(fileNamePath, FileMode.OpenOrCreate);
            workbook.Write(fs);
            fs.Close();
            workbook = null;
        }

        /// <summary>
        /// cit合并计算
        /// </summary>
        private void CitMergeCalculation()
        {
            List<double> list1 = new List<double>();
            List<double> list2 = new List<double>();
            List<double> list3 = new List<double>();
            List<double> list4 = new List<double>();

            string folderPath = "";

            try
            {

                DataProcess.CITDataProcess citProcess = new DataProcess.CITDataProcess();

                foreach (var sFile in citSourceCheckFileList)
                {
                    if (folderPath == "")
                    {
                        folderPath = sFile.Substring(0, sFile.LastIndexOf('\\'));
                    }
                    int channelId1 = citProcess.GetChannelNumberByChannelNameLike(sFile, "SPEED", "速度");
                    double[] d1 = citProcess.GetSingleChannelData(sFile, channelId1);
                    list1.AddRange(d1);
                }
                double[] speedArray = list1.ToArray();
                list1.Clear();

                InvalidDataProcessing.CalculateDistrubtionClass calculateDistrubtion = new InvalidDataProcessing.CalculateDistrubtionClass();
                GetDataAndProcess(citProcess, calculateDistrubtion, speedArray, folderPath, "AB_Vt_L_RMS", "左轴垂有效值");
                GetDataAndProcess(citProcess, calculateDistrubtion, speedArray, folderPath, "AB_Vt_R_RMS", "右轴垂有效值");
                GetDataAndProcess(citProcess, calculateDistrubtion, speedArray, folderPath, "AB_Lt_L_RMS", "左轴横有效值");

                MessageBox.Show("完成");
            }
            catch (Exception ex)
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

        /// <summary>
        /// 全选
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


        private void GetDataAndProcess(DataProcess.CITDataProcess citProcess, InvalidDataProcessing.CalculateDistrubtionClass calculateDistrubtion, double[] arraySpeed, string folderPath, string channelNameEn, string channelNameCn)
        {
            List<double> list = new List<double>();

            double[] d2 = null;

            foreach (var sFile in citSourceCheckFileList)
            {
                int channelId2 = citProcess.GetChannelNumberByChannelNameLike(sFile, channelNameEn, channelNameCn);
                d2 = citProcess.GetSingleChannelData(sFile, channelId2);
                list.AddRange(d2);
            }
            double[] array = list.ToArray();
            list.Clear();

            string fileName = "有效值概率分布计算_";
            string datetime = DateTime.Now.ToString("yyyyMMdd_HHmm");
            calculateDistrubtion.CalDistrubtionProcess_Prev(array, arraySpeed, folderPath, fileName + channelNameEn + "_" + datetime);
            Array.Clear(array, 0, array.Length);

            Array.Clear(d2, 0, d2.Length);
            GC.Collect();
        }
    }
}
