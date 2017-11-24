using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using InGraph.Classes;
using DataProcess;
using System.IO;

namespace InGraph.Forms
{
    public partial class CitFilterForm : Form
    {
        #region 字典：存放线路名和线路编号
        /// <summary>
        /// 字典：存放线路名和线路编号
        /// </summary>
        private Dictionary<String, String> dicLineCodeAndName = new Dictionary<String, String>();
        #endregion

        private Dictionary<String, String> dicTrainCodeAndConfigPath = new Dictionary<String, String>();

        #region 线路编码：ZX01
        /// <summary>
        /// 线路编码：ZX01
        /// </summary>
        private String lineCode = null;
        #endregion

        #region 线路名称
        /// <summary>
        /// 线路名称
        /// </summary>
        private String lineName = null;
        #endregion

        CITDataProcess citProcess = new CITDataProcess();
        GEODataProcess geoProcess = new GEODataProcess();

        String destFilePath = null;
        List<DataProcess.GEO2CITBind> listGEO2CIT = null;

        delegate void SetEnabledCallbackcomboBoxTrainCode();


        public CitFilterForm()
        {
            InitializeComponent();

            InitDicLineCodeAndName();
            InitDicTrainCodeAndConfigPath();

            InitComboboxLineName();
            //InitComboboxTrainCode();

            //comboBoxTrainCode.SelectedItem = "CIT001";
        }


        #region 内部函数：初始化字典
        /// <summary>
        /// 内部函数：初始化字典
        /// </summary>
        private void InitDicLineCodeAndName()
        {
            dicLineCodeAndName.Clear();

            //获取线路编号和线路名
            try
            {
                //Share Exclusive
                using (OleDbConnection sqlconn = new OleDbConnection(CommonClass.sDBConnectionString))
                {
                    sqlconn.Open();

                    OleDbCommand sqlcom = new OleDbCommand("select 自定义线路编号,线路名 from 自定义线路代码表 ", sqlconn);

                    OleDbDataReader oldr = sqlcom.ExecuteReader();
                    while (oldr.Read())
                    {
                        dicLineCodeAndName.Add(oldr[0].ToString(), oldr[1].ToString());
                    }
                    oldr.Close();

                    sqlconn.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion


        private void InitComboboxLineName()
        {
            comboBoxLineName.Items.Clear();
            foreach (KeyValuePair<String, String> keyValuePair in dicLineCodeAndName)
            {
                comboBoxLineName.Items.Add(keyValuePair.Value);
            }
        }

        private void InitDicTrainCodeAndConfigPath()
        {
            dicTrainCodeAndConfigPath.Clear();
            String configPath = Path.Combine(Application.StartupPath, "GEOConfig");
            String[] configFiles = Directory.GetFiles(configPath, "*.csv", SearchOption.AllDirectories);
            foreach (String configFile in configFiles)
            {
                dicTrainCodeAndConfigPath.Add(Path.GetFileNameWithoutExtension(configFile), configFile);
            }
        }

        private void InitComboboxTrainCode()
        {
            comboBoxTrainCode.Items.Clear();
            foreach (KeyValuePair<String, String> keyValuePair in dicTrainCodeAndConfigPath)
            {
                comboBoxTrainCode.Items.Add(keyValuePair.Key);
            }
        }

        private String GetLineCodeByShortName(String shortName)
        {
            String retVal = null;
            String m_LineShortName = null;
            String m_LineCode = null;
            //读取线路代码表，获取线路代码对应的线路编号和线路名
            try
            {
                //Share Exclusive
                using (OleDbConnection sqlconn = new OleDbConnection(CommonClass.sDBConnectionString))
                {
                    sqlconn.Open();

                    String cmdStr = String.Format("select 线路代码,线路编号 from 线路代码表 where 线路代码='{0}'", shortName);

                    OleDbCommand sqlcom = new OleDbCommand(cmdStr, sqlconn);

                    OleDbDataReader oldr = sqlcom.ExecuteReader();
                    while (oldr.Read())
                    {
                        m_LineShortName = oldr[0].ToString();
                        m_LineCode = oldr[1].ToString();
                    }
                    oldr.Close();

                    sqlconn.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            retVal = m_LineCode;
            return retVal;
        }


        private void btnGeoSelect_Click(object sender, EventArgs e)
        {
            DialogResult dr = openFileDialog1.ShowDialog();
            if (dr == DialogResult.OK)
            {
                textBoxCitPath.Text = openFileDialog1.FileName;

                String[] sDHI = Path.GetFileNameWithoutExtension(textBoxCitPath.Text).Split('_');
                String lineShortName = sDHI[sDHI.Length-1].ToUpper();

                lineCode = GetLineCodeByShortName(lineShortName);

                //当线路代码在台账中找不到
                if (lineCode != null)
                {
                    comboBoxLineName.SelectedItem = dicLineCodeAndName[lineCode];
                }
                else
                {
                    //String str = String.Format("线路代码{0}在台账信息中找不到，请联系软件开发人员！", lineShortName);
                    //MessageBox.Show(str);
                    //return;
                }

            }
            else
            {
                textBoxCitPath.Text = "";
            }
        }

        private void comboBoxLineName_SelectedIndexChanged(object sender, EventArgs e)
        {
            lineName = (String)(comboBoxLineName.SelectedItem);
            foreach (KeyValuePair<String, String> keyValuePair in dicLineCodeAndName)
            {
                if (keyValuePair.Value == lineName)
                {
                    lineCode = keyValuePair.Key;
                    break;
                }
            }
        }

        private void buttonCitFilter_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxCitPath.Text))
            {
                MessageBox.Show("请先选择一个波形文件！");
                return;
            }

            if (checkBoxIs0haoche.Checked)
            {
                if (comboBoxLineName.SelectedIndex < 0)
                {
                    MessageBox.Show("请选择检测线路！");
                    return;
                }
                if (comboBoxTrainCode.SelectedIndex < 0)
                {
                    MessageBox.Show("请选择检测车号！");
                    return;
                }
            }


            String destDirectory = Path.GetDirectoryName(textBoxCitPath.Text);
            String destCitName = Path.GetFileNameWithoutExtension(textBoxCitPath.Text).ToUpper() + "_new.cit";
            destFilePath = Path.Combine(destDirectory, destCitName);

            File.Copy(textBoxCitPath.Text, destFilePath, true);

            if (checkBoxIs0haoche.Checked)
            {
                try
                {
                    listGEO2CIT = new List<GEO2CITBind>();
                    StreamReader sr = new StreamReader(dicTrainCodeAndConfigPath[(String)(comboBoxTrainCode.SelectedItem)], Encoding.Default);
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
                catch (System.Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

            try
            {
                //listGEO2CIT = new List<GEO2CITBind>();
                //StreamReader sr = new StreamReader(dicTrainCodeAndConfigPath[(String)(comboBoxTrainCode.SelectedItem)], Encoding.Default);
                //while (sr.Peek() != -1)
                //{
                //    string[] sSplit = sr.ReadLine().Trim().Split(new char[] { '=' });
                //    GEO2CITBind fa = new GEO2CITBind();
                //    fa.sCIT = sSplit[0].Trim();
                //    fa.sGEO = sSplit[1].Trim();
                //    fa.sChinese = sSplit[2].Trim();
                //    listGEO2CIT.Add(fa);
                //}
                //sr.Close();

                SetObjectEnable(false);
                backgroundWorker1.RunWorkerAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void CitFilter()
        {
            try
            {
                if (this.comboBoxTrainCode.InvokeRequired)
                {
                    SetEnabledCallbackcomboBoxTrainCode d = new SetEnabledCallbackcomboBoxTrainCode(CitFilter);
                    this.Invoke(d, new object[] { });
                }
                else
                {
                    if (checkBoxIs0haoche.Checked == true)
                    {
                        citProcess.ModifyCitHeader_New(destFilePath, lineCode, lineName, listGEO2CIT);
                    }

                    citProcess.CitFilter(destFilePath);//高频滤波
                    //citProcess.CitEncrypt(destFilePath);//补零加密
                }
            }
            catch
            {
            }

        }

        private void SetObjectEnable(Boolean b)
        {
            groupBoxGeoSelect.Enabled = b;
            panel1.Enabled = b;
            buttonCitFilter.Enabled = b;
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            CitFilter();
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            SetObjectEnable(true);

            MessageBox.Show("滤波完成！\n新文件位置及名字:" + destFilePath);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxIs0haoche.Checked)
            {
                comboBoxLineName.Enabled = true;
                comboBoxTrainCode.SelectedIndex = 0;
            } 
            else
            {
                comboBoxLineName.Enabled = false;
            }
        }

    }
}
