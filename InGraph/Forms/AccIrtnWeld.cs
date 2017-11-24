using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using InvalidDataProcessing;
using InvalidDataProcessing.Model;

namespace InGraph.Forms
{
    public partial class AccIrtnWeld : Form
    {
        CalculateCorrugationClass calculateCorrugation = new CalculateCorrugationClass();

        CitFileProcess.CitFileHelper citHelper = new CitFileProcess.CitFileHelper();

        List<WeldModel> weldList = new List<WeldModel>();

        string citFilePath = "";

        public AccIrtnWeld()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 选择cit文件事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Select_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();

            //判断用户是否正确的选择了文件
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                citFilePath = fileDialog.FileName;
                this.txt_FilePath.Text = citFilePath;
            }

            //citFilePath = @"H:\工作文件汇总\铁科院\程序\离线加速度\cit文件\CitData_160612060534_CHSS_11corrugationRms.cit";
        }

        /// <summary>
        /// 处理按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Process_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(citFilePath))
            {
                MessageBox.Show("请选择波磨有效值cit文件");
            }
            else
            {
                toolStripStatusLabelStatus.Text = "正在处理......";

                backgroundWorker1.RunWorkerAsync();
            }
        }

        /// <summary>
        /// 导出按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Export_Click(object sender, EventArgs e)
        {
            try
            {
                if (String.IsNullOrEmpty(citFilePath))
                {
                    MessageBox.Show("cit文件所处的路径为空");
                }
                else
                {
                    string filePath = citFilePath.Replace("Rms", "Weld").Replace("cit", "xlsx");

                    calculateCorrugation.CreateWeltExcel(weldList, filePath);

                    MessageBox.Show("导出成功");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void WeldProcess(double[] mileData,double[] speedData,double[] rmsData,double[] peakData,int fs,double dist_weld)
        {
            //CalculateCorrugationClass calculateCorrugation = new CalculateCorrugationClass();

            List<double[]> list = calculateCorrugation.GetWeldInfomation(mileData, speedData, rmsData, peakData, fs, dist_weld);

            for (int i = 0; i < list[0].Length; i++)
            {
                WeldModel model = new WeldModel();
                model.Mile = list[0][i].ToString();
                model.Speed = list[1][i].ToString();
                model.RmsValue = list[2][i].ToString();
                model.PeakValue = list[3][i].ToString();
                model.SampleNo = list[4][i].ToString();

                weldList.Add(model);
            }
        }

        /// <summary>
        /// 展示列表
        /// </summary>
        /// <param name="weldList"></param>
        private void Display(List<WeldModel> weldList)
        {
            dataGridView1.Rows.Clear();
            if (weldList == null || weldList.Count == 0)
            {
                return;
            }

            foreach (WeldModel model in weldList)
            {
                DataGridViewRow dgvr = new DataGridViewRow();
                dgvr.CreateCells(dataGridView1);

                dgvr.Cells[0].Value = model.Mile;
                dgvr.Cells[1].Value = model.Speed;
                dgvr.Cells[2].Value = Math.Round(double.Parse(model.RmsValue), 3);
                dgvr.Cells[3].Value = Math.Round(double.Parse(model.PeakValue), 3);
                dgvr.Cells[4].Value = model.SampleNo;

                dataGridView1.Rows.Add(dgvr);
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            weldList.Clear();
            //测试数据
            //weldList.Add(new WeldModel { Mile = "1", Speed = "2", RmsValue = "3", PeakValue = "4", SampleNo = "5" });

            //citFilePath = @"H:\工作文件汇总\铁科院\程序\离线加速度\cit文件\CitData_160612060534_CHSS_11corrugationRms.cit";
            double[] mileData = citHelper.GetMilesData(citFilePath);
            double[] speedData = citHelper.GetSingleChannelData(citFilePath, 3);
            int channelId = 4;
            if (this.radioButton1.Checked)
            {
                channelId = 4;
            }
            else if (this.radioButton2.Checked)
            {
                channelId = 5;
            }
            else if (this.radioButton3.Checked)
            {
                channelId = 6;
            }
            double[] rmsData = citHelper.GetSingleChannelData(citFilePath, channelId);
            double avg = rmsData.ToList().Average();
            double[] avgRmsData = new double[rmsData.Length];
            for (int i = 0; i < avgRmsData.Length; i++)
            {
                avgRmsData[i] = rmsData[i] / avg;
            }
            int fs = Convert.ToInt32(this.txt_fs.Text.Trim());
            double dist_weld = Convert.ToDouble(this.txt_dist_weld.Text.Trim());

            WeldProcess(mileData, speedData, rmsData, avgRmsData, fs, dist_weld);

        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            toolStripStatusLabelStatus.Text = "处理完毕，正在加载数据...";

            Display(weldList);

            toolStripStatusLabelStatus.Text = "处理完毕";
        }

        
    }
}
