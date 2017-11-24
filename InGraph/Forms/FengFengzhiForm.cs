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
    public partial class FengFengzhiForm : Form
    {
        CITDataProcess cdp;
        DataProcessing dataProcessing;

        PreproceingDeviationClass pdc;

        WaveformDataProcess wdp;

        SubFilterByFftAndIfftClass subFilter;



        List<String> dataStrList = null;
        Boolean isReady = true;//是否计算峰峰值

        public FengFengzhiForm()
        {
            InitializeComponent();

            InitTextboxList();
            radioButton_CB_Vt.Checked = true;
            radioButton_CB_Lt.Checked = false;
            radioButton_Fr_Vt.Checked = false;
            radioButton_Fr_Lt.Checked = false;


            try
            {
                cdp = new CITDataProcess();
                dataProcessing = new DataProcessing();

                pdc = new PreproceingDeviationClass();
                wdp = new WaveformDataProcess();

                subFilter = new SubFilterByFftAndIfftClass();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
                MessageBox.Show(ex.Source);
                MessageBox.Show(ex.StackTrace);
            }
        }




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
        /// 接口函数：滤波后，计算峰峰值指标
        /// </summary>
        /// <param name="citFileName">cit文件全路径</param>
        /// <param name="channelNameEn">英文通道名</param>
        /// <param name="channelNameCh">中文通道名</param>
        /// <param name="thresh_p2p">峰峰值的阈值</param>
        /// <param name="Fs">采样频率：2000/6</param>
        /// <param name="Frep_L">下限频率</param>
        /// <param name="Frep_H">上限频率</param>
        /// <returns></returns>
        public List<String> SubProcess(String citFileName,String channelNameEn,String channelNameCh,float thresh_p2p,float Fs,float Frep_L,float Frep_H)
        {
            List<String> dataStrList = new List<String>();

            CITDataProcess.DataHeadInfo dhi = cdp.GetDataInfoHeadNew(citFileName);
            int tds = dhi.iChannelNumber;

            //加速度系统也是按照一米四个点，只是频率高，导致在相同的位置打上很多点
            dhi.iSmaleRate = 4;

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


            double[] d_wx = cdp.GetSingleChannelData(citFileName, dataProcessing.GetChannelNumberByChannelName(citFileName, channelNameEn, channelNameCh));

            double[] d_wx_filter = subFilter.Sub_filter_by_fft_and_ifft(d_wx, d_tt, Fs, Frep_L, Frep_H);


            List<String> tmpDataStrList = pdc.Sub_preprocessing_deviation_by_p2p_on_acc(channelNameCh, d_tt, d_wx_filter, thresh_p2p);
            dataStrList.AddRange(tmpDataStrList);


            return dataStrList;
        }

        public void BackGroundRun()
        {
            if (isReady)
            {
                //dataStrList = PreProcessDeviation(CommonClass.listDIC[0].sFilePath);

                dataStrList = SubProcess(CommonClass.listDIC[0].sFilePath, channelNameEn, channelNameCh, float.Parse(tb_fengzhi.Text), filter_Fs, filter_Frep_L, filter_Frep_H);
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
                dgvr.Cells[2].Value = Math.Round(double.Parse(dataStrArry[1]), 3);
                dgvr.Cells[3].Value = Math.Round(double.Parse(dataStrArry[2]), 3);
                dgvr.Cells[4].Value = Math.Round(double.Parse(dataStrArry[3]), 2);

                dataGridView1.Rows.Add(dgvr);
            }
        }


        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            DisplayDataStrList(dataStrList);
            //MessageBox.Show("完成！");
            splitContainer1.Panel1.Enabled = true;
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

                MainForm.sMainform.MeterFind(indexStart, indexEnd); ;
            }
            else
            {
                MainForm.sMainform.MeterFind(startMile, endMile);
                //MainForm.sMainform.MeterFind(startMile);
            }

        }

        private void button_OK_Click(object sender, EventArgs e)
        {
            Boolean isInputOk = InputCheck();
            if (isInputOk == false)
            {
                return;
            }

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


        private void checkBox_Filter_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_Filter.Checked)
            {
                textBox_Fs.Enabled = true;
                textBox_Frep_L.Enabled = true;
                textBox_Frep_H.Enabled = true;
            }
            else
            {
                textBox_Fs.Enabled = false;
                textBox_Frep_L.Enabled = false;
                textBox_Frep_H.Enabled = false;
            }
        }

        List<TextBox> textboxList;

        private void InitTextboxList()
        {
            textboxList = new List<TextBox>();

            textboxList.Add(textBox_CB_Vt_Fengzhi);

            textboxList.Add(textBox_CB_Lt_Fengzhi);


            textboxList.Add(textBox_Fr_Vt_Fengzhi);

            textboxList.Add(textBox_Fr_Lt_Fengzhi);

        }

        String channelNameEn;
        String channelNameCh;
        TextBox tb_fengzhi;

        private void radioButton_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton radioBtn = (RadioButton)sender;
            String radioName = radioBtn.Name;

            String channelName = radioName.Substring(12);

            channelNameEn = channelName;
            channelNameCh = radioBtn.Text;

            if (radioBtn.Checked)
            {
                foreach (TextBox tb in textboxList)
                {
                    if (tb.Name.Contains(channelName))
                    {
                        tb.Enabled = true;
                        tb_fengzhi = tb;
                    }
                    else
                    {
                        tb.Enabled = false;
                    }
                }
            }
            else
            {
                foreach (TextBox tb in textboxList)
                {
                    if (tb.Name.Contains(channelName))
                    {
                        tb.Enabled = false;
                    }
                    else
                    {
                        tb.Enabled = true;
                    }
                }
            }
        }


        float filter_Fs = 0;
        float filter_Frep_H = 0;
        float filter_Frep_L = 0;
        float fengzhi_CB_Vt = 0;
        float fengzhi_CB_Lt = 0;
        float fengzhi_Fr_Vt = 0;
        float fengzhi_Fr_Lt = 0;


        private Boolean InputCheck()
        {
            Boolean b = true;

            try
            {
                filter_Fs = float.Parse(textBox_Fs.Text);
                filter_Frep_H = float.Parse(textBox_Frep_H.Text);
                filter_Frep_L = float.Parse(textBox_Frep_L.Text);
                fengzhi_CB_Vt = float.Parse(textBox_CB_Vt_Fengzhi.Text);
                fengzhi_CB_Lt = float.Parse(textBox_CB_Lt_Fengzhi.Text);
                fengzhi_Fr_Vt = float.Parse(textBox_Fr_Vt_Fengzhi.Text);
                fengzhi_Fr_Lt = float.Parse(textBox_Fr_Lt_Fengzhi.Text);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
                b = false;
            }

            return b;
        }
    }
}
