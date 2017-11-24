using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using InGraph.Classes;
using System.IO;

using InvalidDataProcessing;

namespace InGraph.Forms
{
    public partial class DataStatisticsForm : Form
    {
        protected internal float fSPointMile;  //起点公里标
        protected internal long iSPointXPos;  //起点文件指针
        protected internal float fEPointMile; //终点公里标
        protected internal long iEPointXPos;  //终点文件指针

        private double calSum = 0f;//标准差之和
        private int severitySum = 0;//总扣分

        private string citFilePath = null;
        private string iicFilePath = null;



        public DataStatisticsForm()
        {
            InitializeComponent();

            //openFileDialog1.InitialDirectory = Path.GetDirectoryName(citFilePath);

            //if (dataGridView_fuzhi.ColumnCount == 0)
            //{
            //    dataGridView_fuzhi.Columns.Add(ColumnCalItems);
            //    dataGridView_fuzhi.Columns.Add(Column_L_Prof);
            //    dataGridView_fuzhi.Columns.Add(Column_R_Prof);
            //    dataGridView_fuzhi.Columns.Add(Column_L_Align);
            //    dataGridView_fuzhi.Columns.Add(Column_R_Align);
            //    dataGridView_fuzhi.Columns.Add(Column_Gage);
            //    dataGridView_fuzhi.Columns.Add(Column_CrossLevel);
            //    dataGridView_fuzhi.Columns.Add(Column_Twist);
            //}
            dataGridView_fuzhi.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

        }

        protected internal void InitTextbox()
        {
            textBoxStartMile.Text = fSPointMile.ToString();
            textBoxEndMile.Text = fEPointMile.ToString();


            citFilePath = CommonClass.listDIC[0].sFilePath;
        }

        protected internal void InitDataGridview()
        {


            dataGridView_fuzhi.Rows.Clear();

            int columnIndex = ColumnCalItems.Index;

            DataGridViewRow dgvr = new DataGridViewRow();
            dgvr.CreateCells(dataGridView_fuzhi);
            dgvr.Cells[columnIndex].Value = "最大值";
            dataGridView_fuzhi.Rows.Add(dgvr);

            dgvr = new DataGridViewRow();
            dgvr.CreateCells(dataGridView_fuzhi);
            dgvr.Cells[columnIndex].Value = "最小值";
            dataGridView_fuzhi.Rows.Add(dgvr);

            dgvr = new DataGridViewRow();
            dgvr.CreateCells(dataGridView_fuzhi);
            dgvr.Cells[columnIndex].Value = "平均值";
            dataGridView_fuzhi.Rows.Add(dgvr);

            dgvr = new DataGridViewRow();
            dgvr.CreateCells(dataGridView_fuzhi);
            dgvr.Cells[columnIndex].Value = "标准差";
            dataGridView_fuzhi.Rows.Add(dgvr);

        }

        private void DataProcess_fuzhi(String columnName,String channelNameEn,String channelNameCh)
        {
            int channelIndex = CommonClass.cdp.GetChannelNumberByChannelName(citFilePath, channelNameEn, channelNameCh);
            double[] data = CommonClass.cdp.GetSingleChannelData(citFilePath, channelIndex, iSPointXPos, iEPointXPos);
            dataGridView_fuzhi.Rows[0].Cells[columnName].Value = CommonClass.cdp.GetMaxValue(data);//最大值
            dataGridView_fuzhi.Rows[1].Cells[columnName].Value = CommonClass.cdp.GetMinValue(data);//最小值
            dataGridView_fuzhi.Rows[2].Cells[columnName].Value = Math.Round(CommonClass.cdp.GetAverageValue(data),2);//平均值
            dataGridView_fuzhi.Rows[3].Cells[columnName].Value = Math.Round(CommonClass.cdp.GetStanderdDeviation(data),2);//标准差
            //dataGridView1.Rows[4].Cells[columnName].Value=CommonClass.cdp.GetSeverityValue()

            calSum += Math.Round(CommonClass.cdp.GetStanderdDeviation(data), 2);
        }

        private void AddDataToDataGridview_fuzhi()
        {
            calSum = 0;
            //左高低
            DataProcess_fuzhi(Column_L_Prof.Name, "L_Prof_SC", "左高低_中波");

            //右高低
            DataProcess_fuzhi(Column_R_Prof.Name, "R_Prof_SC", "右高低_中波");

            //左轨向
            DataProcess_fuzhi(Column_L_Align.Name, "L_Align_SC", "左轨向_中波");

            //右轨向
            DataProcess_fuzhi(Column_R_Align.Name, "R_Align_SC", "右轨向_中波");

            //轨距
            DataProcess_fuzhi(Column_Gage.Name, "Gage", "轨距");

            //水平
            DataProcess_fuzhi(Column_CrossLevel.Name, "Crosslevel", "水平");

            //三角坑
            DataProcess_fuzhi(Column_Twist.Name, "Short_Twist", "三角坑");            

        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            AddDataToDataGridview_fuzhi();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            this.Enabled = false;
            backgroundWorker1.RunWorkerAsync();
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Enabled = true;
            labelCalSum.Text = calSum.ToString();
        }

        private void DataStatisticsForm_Load(object sender, EventArgs e)
        {
            
        }

        private void buttonExportCsv_fuzhi_Click(object sender, EventArgs e)
        {
            String csvFileName = String.Format("幅值统计_{0}_{1}.csv", fSPointMile, fEPointMile);
            String csvFilePath = Path.Combine(Path.GetDirectoryName(citFilePath), csvFileName);

            StringBuilder sb = new StringBuilder();

            if (dataGridView_fuzhi.Rows.Count == 0)
            {
                return;
            }

            for (int i = 0; i < dataGridView_fuzhi.ColumnCount; i++)
            {
                sb.AppendFormat("{0},", dataGridView_fuzhi.Columns[i].HeaderText);
            }
            sb.AppendLine();

            foreach (DataGridViewRow dgvr in dataGridView_fuzhi.Rows)
            {
                for (int i = 0; i < dataGridView_fuzhi.ColumnCount; i++)
                {
                    sb.AppendFormat("{0},", dgvr.Cells[i].Value.ToString());
                }
                sb.AppendLine();
            }

            File.WriteAllText(csvFilePath, sb.ToString(), Encoding.Default);

            MessageBox.Show("导出成功！");
        }


    }
}
