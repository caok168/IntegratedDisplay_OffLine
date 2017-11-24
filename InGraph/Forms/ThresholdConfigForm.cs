using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace InGraph.Forms
{
    public partial class ThresholdConfigForm : Form
    {
        public ThresholdConfigForm()
        {
            InitializeComponent();
        }

        public ThresholdConfigForm(String filePath)
        {
            InitializeComponent();
            this.iniFilePath = filePath;
        }

        String iniFilePath = null;
        //String iniFilePath = Application.StartupPath + "\\config.ini";
        List<Dictionary<String, String>> dicList = null;

        private void WriteIni(Dictionary<String, String> configDic, String filePath)
        {

            StringBuilder sb = new StringBuilder();

            StreamWriter myWriter = new StreamWriter(filePath, false, Encoding.Default);

            int i = 0;
            for (; i < dicList.Count; i++)
            {
                if (configDic["TrainNo"] == dicList[i]["TrainNo"])
                {
                    dicList[i].Clear();
                    foreach (KeyValuePair<String, String> kvp in configDic)
                    {
                        dicList[i][kvp.Key] = kvp.Value;
                    }
                    break;
                }
            }
            if (i == dicList.Count)
            {
                dicList.Add(configDic);
            }


            foreach (Dictionary<String, String> dic in dicList)
            {
                foreach (KeyValuePair<String, String> kvp in dic)
                {
                    sb.AppendFormat("{0}={1}", kvp.Key, kvp.Value);
                    sb.AppendLine();
                }
            }

            myWriter.Write(sb.ToString());

            myWriter.Close();


        }
        private List<Dictionary<String, String>> ReadIni(String filePath)
        {
            List<Dictionary<String, String>> mdicList = new List<Dictionary<String, String>>();
            Dictionary<String, String> dic = null;

            if (!File.Exists(filePath))
            {
                using (File.Create(filePath))
                {
                    return null;
                }
            }


            StreamReader sr = new StreamReader(filePath, Encoding.Default);

            String str = null;

            try
            {
                while ((str = sr.ReadLine()) != null)
                {
                    String[] strSplt = str.Split('=');
                    if (strSplt[0] == "TrainNo")
                    {
                        dic = new Dictionary<String, String>();
                        mdicList.Add(dic);
                        dic.Add(strSplt[0], strSplt[1]);
                    }
                    else
                    {
                        dic.Add(strSplt[0], strSplt[1]);
                    }
                }
                sr.Close();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("配置文件格式不对！");
            }


            return mdicList;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (dicList == null)
            {
                dicList = new List<Dictionary<String, String>>();
            }


            Dictionary<String, String> dic = new Dictionary<String, String>();


            foreach (DataGridViewRow dgvr in dataGridView1.Rows)
            {
                if (dgvr.Cells[0].Value == null)
                {
                }
                else
                {
                    String str = dgvr.Cells[1].Value == null ? "0" : ((String)(dgvr.Cells[1].Value)).Trim();
                    dic.Add(((String)(dgvr.Cells[0].Value)).Trim(), str);
                }

            }

            StringBuilder sb = new StringBuilder();
            foreach (KeyValuePair<String, String> kvp in dic)
            {
                if (kvp.Key == "TrainNo")
                {
                }
                else
                {
                    try
                    {
                        float.Parse(kvp.Value);
                    }
                    catch (System.Exception ex)
                    {
                        sb.AppendFormat("{0}:{1}", ex.Message, kvp.Value);
                        sb.AppendLine();
                    }
                }
            }
            if (sb.Length != 0)
            {
                MessageBox.Show(sb.ToString());
                return;
            }



            WriteIni(dic, iniFilePath);

            this.Tag = dicList;


            if (dicList != null && dicList.Count > 0)
            {
                comboBox1.Items.Clear();
                foreach (Dictionary<String, String> dicc in dicList)
                {
                    comboBox1.Items.Add(dicc["TrainNo"]);
                }

                comboBox1.SelectedIndex = 0;

            }
            MessageBox.Show("保存成功！");
            this.Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            String trainNo = (String)(comboBox1.SelectedItem);

            dataGridView1.Rows.Clear();

            foreach (Dictionary<String, String> dic in dicList)
            {
                if (trainNo == dic["TrainNo"])
                {

                    foreach (KeyValuePair<String, String> kvp in dic)
                    {
                        dataGridView1.Rows.Add(kvp.Key, kvp.Value);
                    }
                }
            }
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 1)
            {
                if (e.RowIndex == 0 || e.RowIndex == -1)
                {

                }
                else
                {
                    try
                    {
                        if (dataGridView1[1, e.RowIndex].Value != null)
                        {
                            float.Parse((String)(dataGridView1[1, e.RowIndex].Value));
                        }
                    }
                    catch (System.Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

                }
            } 
        }

        private void ThresholdConfigForm_Load(object sender, EventArgs e)
        {
            dicList = ReadIni(iniFilePath);

            if (dicList != null && dicList.Count > 0)
            {
                foreach (Dictionary<String, String> dic in dicList)
                {
                    comboBox1.Items.Add(dic["TrainNo"]);
                }

                comboBox1.SelectedIndex = 0;

            }
        }

    }
}
