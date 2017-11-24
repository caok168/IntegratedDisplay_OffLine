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
using InvalidDataProcessing;
using DataProcess;

namespace InGraph.Forms
{
    public partial class GongLvPuForm : Form
    {
        CITDataProcess cdp;
        DataProcessing dataProcessing;
        WaveformDataProcess wdp;
        GongLvPuClass glpc;

        List<String> dataStrList = null;

        public GongLvPuForm()
        {
            InitializeComponent();

            radioButton_CB_Vt.Checked = false;
            radioButton_CB_Lt.Checked = false;
            radioButton_Fr_Vt.Checked = false;
            radioButton_Fr_Lt.Checked = false;

            string sLength = CommonClass.cdp.QueryDataMileageRange(CommonClass.listDIC[0].sFilePath, false, CommonClass.listDIC[0].bEncrypt);
            string[] sSE = sLength.Split(',');
            string[] sSE1 = sSE[1].Split('-');
            textBox_Km_Start.Text = sSE1[0].Trim();
            textBox_Km_End.Text = sSE1[1].Trim();


            try
            {
                cdp = new CITDataProcess();
                dataProcessing = new DataProcessing();
                wdp = new WaveformDataProcess();
                glpc = new GongLvPuClass();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
                MessageBox.Show(ex.Source);
                MessageBox.Show(ex.StackTrace);
            }
        }


        String channelNameEn;
        String channelNameCh;

        private void radioButton_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton radioBtn = (RadioButton)sender;

            if (radioBtn.Checked)
            {

                String radioName = radioBtn.Name;

                String channelName = radioName.Substring(12);

                channelNameEn = channelName;
                channelNameCh = radioBtn.Text;
            }

        }


        float km_Start = 0;
        float km_End = 0;
        float fourier = 0;
        float timeLen = 0;

        private Boolean InputCheck()
        {
            Boolean b = true;

            try
            {
                km_Start = float.Parse(textBox_Km_Start.Text);
                km_End = float.Parse(textBox_Km_End.Text);
                fourier = float.Parse(textBox_Fourier.Text);
                timeLen = float.Parse(textBox_Time.Text);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
                b = false;
            }

            return b;
        }



        private void button_OK_Click(object sender, EventArgs e)
        {
            Boolean isInputOk = InputCheck();
            if (isInputOk == false)
            {
                return;
            }

            panel2.Enabled = false;
            backgroundWorker1.RunWorkerAsync();
        }


        /// <summary>
        /// 接口函数：计算功率谱
        /// </summary>
        /// <param name="citFileName">cit全路径文件名</param>
        /// <param name="channelNameEn">英文通道名</param>
        /// <param name="channelNameCh">中文通道名</param>
        /// <param name="kmStart">起始里程</param>
        /// <param name="kmEnd">结束里程</param>
        /// <param name="fourierLen">傅里叶变换窗长</param>
        /// <param name="timeLen">时间步长</param>
        /// <returns>频率和幅值谱</returns>
        private List<String> SubProcess(String citFileName, String channelNameEn, String channelNameCh, float kmStart, float kmEnd, float fourierLen, float timeLen)
        {
            double[] retVal = new double[2];

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
            //先把整条线的数据取出，然后再从中取出一段数
            double[] d_tt = wdp.GetDataMileageInfoDouble(citFileName, tds, 4, CommonClass.listDIC[0].bEncrypt,
                CommonClass.listDIC[0].listIC, CommonClass.listDIC[0].bIndex, startPos, endPos, CommonClass.listDIC[0].sKmInc);//里程

            double[] d_wx = cdp.GetSingleChannelData(citFileName, dataProcessing.GetChannelNumberByChannelName(citFileName, channelNameEn, channelNameCh));

            int indexStart = 0;
            int indexEnd = d_tt.Length-1;
            
            if (d_tt[0] < d_tt[d_tt.Length-1])
            {
                //增里程
                for (int i = 0; i < d_tt.Length;i++ )
                {
                    if (d_tt[i]>=kmStart)
                    {
                        indexStart = i;
                        break;
                    }
                }
                for (int i = 0; i < d_tt.Length; i++)
                {
                    if (d_tt[i] >= kmEnd)
                    {
                        indexEnd = i;
                        break;
                    }
                }

            } 
            else
            {
                //减里程
                for (int i = 0; i < d_tt.Length; i++)
                {
                    if (d_tt[i] <= kmStart)
                    {
                        indexStart = i;
                        break;
                    }
                }
                for (int i = 0; i < d_tt.Length; i++)
                {
                    if (d_tt[i] <= kmEnd)
                    {
                        indexEnd = i;
                        break;
                    }
                }
            }
            int len=indexEnd - indexStart + 1;
            double[] tt_new = new double[len];
            double[] wx_new = new double[len];

            Array.Copy(d_tt, indexStart, tt_new, 0, len);
            Array.Copy(d_wx, indexStart, wx_new, 0, len);

            List<String> str_retVal = glpc.Sub_Fourier_analysis(channelNameCh,tt_new, wx_new, fourierLen, timeLen);


            return str_retVal;
        }

        Boolean isReady = true;

        private Boolean IsReady()
        {
            Boolean retVal = false;

            if (radioButton_CB_Lt.Checked || radioButton_CB_Vt.Checked || radioButton_Fr_Lt.Checked || radioButton_Fr_Vt.Checked)
            {
                retVal = true;
            }

            return retVal;
        }

        public void BackGroundRun()
        {
            if (IsReady())
            {
                dataStrList = SubProcess(CommonClass.listDIC[0].sFilePath, channelNameEn, channelNameCh, km_Start, km_End, fourier, timeLen);
                
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
                dgvr.Cells[2].Value = Math.Round(double.Parse(dataStrArry[1]), 5);
                dgvr.Cells[3].Value = Math.Round(double.Parse(dataStrArry[2]), 5);

                dataGridView1.Rows.Add(dgvr);
            }
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            DisplayDataStrList(dataStrList);
            panel2.Enabled = true;
        }

        private void button_ExportExcel_Click(object sender, EventArgs e)
        {
            String excelPath = null;
            String excelName = null;

            if (dataGridView1.Rows.Count == 0)
            {
                return;
            }


            excelPath = Path.GetDirectoryName(CommonClass.listDIC[0].sFilePath);
            excelName = Path.GetFileNameWithoutExtension(CommonClass.listDIC[0].sFilePath);


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
    }
}
