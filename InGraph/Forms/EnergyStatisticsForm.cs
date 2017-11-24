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


namespace InGraph.Forms
{
    public partial class EnergyStatisticsForm : Form
    {
        protected internal float fSPointMile;  //起点公里标
        protected internal long iSPointXPos;  //起点文件指针
        protected internal float fEPointMile; //终点公里标
        protected internal long iEPointXPos;  //终点文件指针
        //private int severitySum = 0;//总扣分
        //private string iicFilePath = null;
        private string citFilePath = null;
        private CalEnergyWeightClass cewcls = null;

        private Dictionary<string, string> dicChannelName = new Dictionary<string, string>();
        private List<CheckBox> listCB=new List<CheckBox>();
        private String radioButtonText = null;


        public EnergyStatisticsForm()
        {
            InitializeComponent();

            InitDicChannelName();
            InitListCB();
            rb_VACC.Checked = true;
        }

        protected internal void InitTextbox()
        {
            textBoxStartMile.Text = fSPointMile.ToString();
            textBoxEndMile.Text = fEPointMile.ToString();

            citFilePath = CommonClass.listDIC[0].sFilePath;
        }

        private double[] GetChannelData(String channelNameEn, String channelNameCh, long startPos, long endPos)
        {
            int channelIndex = CommonClass.cdp.GetChannelNumberByChannelName(citFilePath, channelNameEn, channelNameCh);
            double[] data = CommonClass.cdp.GetSingleChannelData(citFilePath, channelIndex, iSPointXPos, iEPointXPos);

            return data;
        }

        private void DataProcess_energyweight(long startPos, long endPos, double spdBand,string item_01_NameCh,string item_02_NameCh,string item_03_NameCh)
        {
            double[] data_wx_L = GetChannelData(dicChannelName[item_01_NameCh], item_01_NameCh, startPos, endPos);
            double[] data_wx_R = GetChannelData(dicChannelName[item_02_NameCh], item_02_NameCh, startPos, endPos);
            double[] data_wy = GetChannelData(dicChannelName[item_03_NameCh], item_03_NameCh, startPos, endPos);
            double[] data_wvelo = GetChannelData("Speed", "速度", startPos, endPos);

            double[] data_Km = GetChannelData("Km", "Km", startPos, endPos);
            double[] data_Meter = GetChannelData("Meter", "Meter", startPos, endPos);
            double[] data_tt = new double[data_wvelo.Length];
            for (int i = 0; i < data_tt.Length; i++)
            {
                data_tt[i] = data_Km[i] + data_Meter[i] / 1000;
            }

            if (cewcls == null)
            {
                cewcls = new CalEnergyWeightClass();
            }

            List<List<double>> data = cewcls.CalEnergyWeightProcess("", data_tt, data_wx_L, data_wx_R, data_wvelo, data_wy, spdBand);

            dataGridView_EnergyWeight.Rows.Clear();

            if (data.Count == 0)
            {
                return;
            }
            labelEnergyWeight.Text = Math.Round(data[2][0], 4).ToString();
            labelSpeed.Text = Math.Round(data[3][0], 4).ToString();

            for (int i = 0; i < data[0].Count; i++)
            {
                DataGridViewRow dgvr = new DataGridViewRow();
                dgvr.CreateCells(dataGridView_EnergyWeight);
                dgvr.Cells[0].Value = i + 1;
                dgvr.Cells[1].Value = Math.Round(data[0][i], 8);
                dgvr.Cells[2].Value = Math.Round(data[1][i], 8);

                //dgvr.Cells[1].Value = data[0][i];
                //dgvr.Cells[2].Value = data[1][i];

                dataGridView_EnergyWeight.Rows.Add(dgvr);
            }

        }

        private void button_EnergyWeight_Click(object sender, EventArgs e)
        {
            int checkedNum = 0;
            List<string> listItem=new List<string>();
            foreach (CheckBox cb in listCB)
            {
                if (cb.Checked)
                {
                    checkedNum += 1;
                    listItem.Add(cb.Text);
                }
            }
            if (checkedNum != 2)
            {
                MessageBox.Show("几何：必须选择其中的两个");
                return;
            }
            
            double spdBand = 0;
            try
            {
                spdBand = double.Parse(textBoxSpdBand.Text);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("速度带宽：请输入数字");
                return;
            }

            DataProcess_energyweight(iSPointXPos, iEPointXPos, spdBand,listItem[0],listItem[1],radioButtonText);
        }

        private void buttonExportCsv_EnergyWeight_Click(object sender, EventArgs e)
        {
            String csvFilePath = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "能量权系数");

            if (!Directory.Exists(csvFilePath))
            {
                Directory.CreateDirectory(csvFilePath);
            }

            int fileIndexMax = 0;

            String[] csvFileNames = Directory.GetFiles(csvFilePath, "*.csv", SearchOption.TopDirectoryOnly);
            foreach (string tmp in csvFileNames)
            {
                String[] tmpStr = Path.GetFileName(tmp).Split('_');
                try
                {
                    int tmpIndex = int.Parse(tmpStr[1]);
                    if (tmpIndex > fileIndexMax)
                    {
                        fileIndexMax = tmpIndex;
                    }
                }
                catch (System.Exception ex)
                {

                }

            }


            String csvFileName = String.Format("能量权系数_{0}_{1}_{2}_{3}.csv", fileIndexMax + 1, labelSpeed.Text, fSPointMile, fEPointMile);

            csvFilePath = Path.Combine(csvFilePath, csvFileName);

            StringBuilder sb = new StringBuilder();

            if (dataGridView_EnergyWeight.Rows.Count == 0)
            {
                return;
            }

            for (int i = 0; i < dataGridView_EnergyWeight.ColumnCount; i++)
            {
                sb.AppendFormat("{0},", dataGridView_EnergyWeight.Columns[i].HeaderText);
            }
            sb.AppendLine();

            foreach (DataGridViewRow dgvr in dataGridView_EnergyWeight.Rows)
            {
                for (int i = 0; i < dataGridView_EnergyWeight.ColumnCount; i++)
                {
                    sb.AppendFormat("{0},", double.Parse(dgvr.Cells[i].Value.ToString()));
                }
                sb.AppendLine();
            }

            File.WriteAllText(csvFilePath, sb.ToString(), Encoding.Default);

            MessageBox.Show("导出成功！");
        }

        private void buttonAvg_Click(object sender, EventArgs e)
        {
            String csvFilePath = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "能量权系数");

            if (!Directory.Exists(csvFilePath))
            {
                Directory.CreateDirectory(csvFilePath);
            }

            List<String> csvFileNameList = new List<String>();

            String[] csvFileNames = Directory.GetFiles(csvFilePath, "*.csv", SearchOption.TopDirectoryOnly);
            foreach (string tmp in csvFileNames)
            {
                String[] tmpStr = Path.GetFileName(tmp).Split('_');
                try
                {
                    int tmpIndex = int.Parse(tmpStr[1]);
                    csvFileNameList.Add(tmp);
                }
                catch (System.Exception ex)
                {

                }
            }

            if (csvFileNameList.Count == 0)
            {
                return;
            }

            int len = 512;
            List<double> list1 = new List<double>();
            List<double> list2 = new List<double>();
            for (int i = 0; i < len; i++)
            {
                list1.Add(0);
                list2.Add(0);
            }

            foreach (String tmp in csvFileNameList)
            {
                using (StreamReader sr = new StreamReader(tmp, Encoding.Default))
                {
                    sr.ReadLine();
                    for (int i = 0; i < len; i++)
                    {
                        String[] numbers = sr.ReadLine().Split(',');
                        list1[i] += double.Parse(numbers[1]);
                        list2[i] += double.Parse(numbers[2]);
                    }
                    sr.Close();
                }
            }


            String csvFileName = String.Format("能量权系数_平均值_{0}.csv", csvFileNameList.Count);

            csvFilePath = Path.Combine(csvFilePath, csvFileName);

            StringBuilder sb = new StringBuilder();

            sb.Append("ID,放大系数,能量权系数");
            sb.AppendLine();

            for (int i = 0; i < len; i++)
            {
                sb.AppendFormat("{0},{1},{2}", i + 1, list1[i] / csvFileNameList.Count, list2[i] / csvFileNameList.Count);
                sb.AppendLine();
            }


            File.WriteAllText(csvFilePath, sb.ToString(), Encoding.Default);

            MessageBox.Show("计算成功！");
        }

        private void InitDicChannelName()
        {
            if (dicChannelName.Count > 0)
            {
                dicChannelName.Clear();
            }

            dicChannelName.Add("左高低_中波","L_Prof_SC");
            dicChannelName.Add("右高低_中波","R_Prof_SC");
            dicChannelName.Add("左轨向_中波","L_Align_SC");
            dicChannelName.Add("右轨向_中波","R_Align_SC");
            dicChannelName.Add("超高","Superelevation");
            dicChannelName.Add("水平","Crosslevel");
            dicChannelName.Add("三角坑","Short_Twist");
            dicChannelName.Add("车体横向加速度","LACC");
            dicChannelName.Add("车体垂向加速度","VACC");
        }

        private void InitListCB()
        {
            if (listCB.Count > 0)
            {
                listCB.Clear();
            }

            listCB.Add(cb_Crosslevel);
            listCB.Add(cb_Superelevation);
            listCB.Add(cb_Short_Twist);
            listCB.Add(cb_L_Align_SC);
            listCB.Add(cb_R_Align_SC);
            listCB.Add(cb_R_Prof_SC);
            listCB.Add(cb_L_Prof_SC);
        }

        private void radioButton_CheckedChange(object sender, EventArgs e)
        {
            RadioButton rb = (RadioButton)sender;

            if (rb.Checked)
            {
                radioButtonText = rb.Text;
            }
        }
    }
}
