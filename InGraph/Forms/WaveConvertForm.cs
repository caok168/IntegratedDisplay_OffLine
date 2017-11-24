using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Data.OleDb;
using InGraph.Classes;
using DataProcess;

namespace InGraph.Forms
{
    public partial class WaveConvertForm : Form
    {

        #region 变量
        #region 字典：存放线路名和线路编号
        private List<LineCodeAndNameClass> listLineCodeAndName = new List<LineCodeAndNameClass>();
        #endregion
        #region 线路代码：AHHX
        /// <summary>
        /// 线路代码：AHHX
        /// </summary>
        private String lineShortName = null;
        #endregion
        #region 线路编码：ZX01
        /// <summary>
        /// 线路编码：ZX01
        /// </summary>
        private String lineCode = null;
        #endregion
        CITDataProcess citProcess = new CITDataProcess();
        GEODataProcess geoProcess = new GEODataProcess();
        private Dictionary<String,String> dicTrainCodeAndConfigPath=new Dictionary<String,String>();
        private Dictionary<String, String> dicLinedir = new Dictionary<String, String>();
        CITDataProcess.DataHeadInfo dhi ;
        List<DataProcess.GEO2CITBind> listGEO2CIT = null;

        #region 是否把减里程统一为增里程
        /// <summary>
        /// 是否把减里程统一为增里程
        /// </summary>
        Boolean isMergeKmInc = false;
        #endregion
        #region 是否把反向统一为正向
        /// <summary>
        /// 是否把反向统一为正向
        /// </summary>
        Boolean isReverseToForward = false;
        #endregion

        String sourceIICPath = null;
        String destIICPath = null;

        int iType = 1;//此变量为307时，geo转换有特殊情况。

        #endregion
        

        public WaveConvertForm()
        {
            InitializeComponent();

            //初始化字典
            InitDicLineDir();
            //InitDicLineCodeAndName();
            InitListLineCodeAndName();
            InitDicTrainCodeAndConfigPath();

            //初始化comboboxLineName
            InitComboboxLineName();
            InitComboboxTrainCode();

        }

        

        #region 内部函数：初始化listLineCodeAndName
        /// <summary>
        /// 内部函数：初始化listLineCodeAndName
        /// </summary>
        private void InitListLineCodeAndName()
        {
            if (listLineCodeAndName != null && listLineCodeAndName.Count > 0)
            {
                listLineCodeAndName.Clear();
            }            

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
                        LineCodeAndNameClass lineCodeAndName = new LineCodeAndNameClass();
                        lineCodeAndName.lineCode = oldr[0].ToString();
                        lineCodeAndName.lineName = oldr[1].ToString();

                        listLineCodeAndName.Add(lineCodeAndName);

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

            foreach (LineCodeAndNameClass obj in listLineCodeAndName)
            {
                comboBoxLineName.Items.Add(obj.lineName);
            }

        }

        private void InitDicLineDir()
        {
            dicLinedir.Clear();
            dicLinedir.Add("S", "上");
            dicLinedir.Add("X", "下");
            dicLinedir.Add("D", "单");
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

        private void InitDicTrainCodeAndConfigPath()
        {
            dicTrainCodeAndConfigPath.Clear();
            String configPath=Path.Combine(Application.StartupPath,"GEOConfig");
            String[] configFiles = Directory.GetFiles(configPath, "*.csv",SearchOption.AllDirectories);
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

        private void SetContralEnable(Boolean b)
        {
            groupBoxGeoSelect.Enabled = b;
            groupBoxCitHeadEditer.Enabled = b;
            buttonConvertData.Enabled = b;
        }

        private void ConvertData()
        {
            String geoFilePath = textBoxGeoPath.Text;
            String geoNameWithoutExtension = Path.GetFileNameWithoutExtension(geoFilePath).ToUpper();
            String geoDirectory = Path.GetDirectoryName(geoFilePath);
            String destFileName = Path.Combine(geoDirectory, geoNameWithoutExtension + ".cit");
            geoProcess.ConvertData(textBoxGeoPath.Text, destFileName, listGEO2CIT, iType, dhi);

            if (isMergeKmInc)
            {
                citProcess.ModifyCitMergeKmInc(destFileName);
            }
            if (isReverseToForward)
            {
                citProcess.ModifyCitReverseToForward(destFileName);
            }
            //把IIC里的大值和tqi做相应的处理
            if (panelIIC.Enabled == true)
            {
                ConvertIIC();
            }
        }

        private void ConvertIIC()
        {
            //没有选择iic文件时，不进行后续处理
            if (sourceIICPath == null)
            {
                return;
            }

            File.Copy(sourceIICPath, destIICPath, true);

            /*
             * 将反向检测波形转换为可与正向检测波形直接对比的标准CIT波形。同时，将反向检测的偏差文件对应修正。
             * 当反向检测时：
             * 左高低与右高低对调，即A=左高低，左高低=右高低，右高低=左高低。
             * 左轨向与右轨向对调，然后幅值*（-1）。
             * 水平、超高、三角坑、曲率、曲率变化率*（-1）。
             */
            
            #region 大值处理
            if (isReverseToForward)
            {
                //左右高低对调
                CommonClass.wdp.IICChannelSwap(destIICPath, "defects", "defecttype", "L SURFACE", "defecttype", "R SURFACE");
                //左右轨向对调
                CommonClass.wdp.IICChannelSwap(destIICPath, "defects", "defecttype", "L ALIGNMENT", "defecttype", "R ALIGNMENT");
                //幅值*(-1)
                CommonClass.wdp.IICChannelFlip(destIICPath, "defects", "maxval1", "maxval1", "defecttype", "L ALIGNMENT");//左轨距
                CommonClass.wdp.IICChannelFlip(destIICPath, "defects", "maxval1", "maxval1", "defecttype", "R ALIGNMENT");//右轨距
                CommonClass.wdp.IICChannelFlip(destIICPath, "defects", "maxval1", "maxval1", "defecttype", "CROSSLEVEL");//水平
                //CommonClass.wdp.IICChannelFlip(iicFilePath, "defects", "maxval1", "maxval1", "defecttype", "R ALIGNMENT");//超高--没有这项大值
                CommonClass.wdp.IICChannelFlip(destIICPath, "defects", "maxval1", "maxval1", "defecttype", "TWIST");//三角坑
                //CommonClass.wdp.IICChannelFlip(iicFilePath, "defects", "maxval1", "maxval1", "defecttype", "R ALIGNMENT");//曲率--没有这项大值
                CommonClass.wdp.IICChannelFlip(destIICPath, "defects", "maxval1", "maxval1", "defecttype", "CURVATURE RATE");//曲率变化率
            }
            #endregion

            #region TQI处理
            if (isReverseToForward)
            {
                //左右高低对调
                CommonClass.wdp.IICChannelSwap(destIICPath, "tqi", "TQIMetricName", "L_STDSURF", "TQIMetricName", "R_STDSURF");
                //左右轨向对调
                CommonClass.wdp.IICChannelSwap(destIICPath, "tqi", "TQIMetricName", "L_STDALIGN", "TQIMetricName", "R_STDALIGN");
            }
            if (isMergeKmInc)
            {
                //减里程调整为增里程时，tqi里程要减去0.2
                CommonClass.wdp.IICTqi(destIICPath, "tqi", "BasePost", "BasePost");
            }
            #endregion
            //删除已经创建的fix表
            CommonClass.wdp.DropFixTalbe(destIICPath);
        }


        private void btnGeoSelect_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "GEO波形文件|*.geo";
            DialogResult dr = openFileDialog1.ShowDialog();
            if (dr == DialogResult.OK)
            {
                textBoxGeoPath.Text = openFileDialog1.FileName;

                String[] sDHI = Path.GetFileNameWithoutExtension(textBoxGeoPath.Text).Split('-');
                lineShortName = sDHI[0].ToUpper();

                try
                {                    
                    String time = String.Format("{0}:{1}:{2}", sDHI[4].Substring(0, 2), sDHI[4].Substring(2, 2), sDHI[4].Substring(4, 2));
                    String date = String.Format("{0}/{1}/{2}", sDHI[3].Substring(2, 2), sDHI[3].Substring(0, 2), sDHI[3].Substring(4, 4));

                    lineCode = GetLineCodeByShortName(lineShortName);
                    //当线路代码在台账中找不到
                    if (lineCode != null)
                    {

                        foreach (LineCodeAndNameClass obj in listLineCodeAndName)
                        {
                            if (obj.lineCode == lineCode)
                            {
                                comboBoxLineName.SelectedItem = obj.lineName;
                            }
                        }
                    }
                    else
                    {
                        //String str = String.Format("线路代码{0}在台账信息中找不到，请联系软件开发人员！", lineShortName);
                        //MessageBox.Show(str);
                        //return;
                    }

                    if (dicLinedir.ContainsKey(lineShortName.Substring(3, 1)))
                    {
                        comboBoxLineDir.SelectedItem = dicLinedir[lineShortName.Substring(3, 1)];
                    } 
                    else
                    {
                        comboBoxLineDir.SelectedIndex = 0;
                    }

                    dateTimePickerData.Value = DateTime.Parse(date);

                    dateTimePickerTime.Value = DateTime.Parse(time);

                }
                catch (System.Exception ex)
                {
                    comboBoxLineDir.SelectedIndex = 0;

                    dateTimePickerData.Value = DateTime.Now;

                    dateTimePickerTime.Value = DateTime.Now;

                    //MessageBox.Show(ex.Message);
                }

            }
            else
            {
                textBoxGeoPath.Text = "";
            }

            sourceIICPath = null;
            textBoxIICFilePath.Text = "";
            destIICPath = null;
        }

        private void buttonConvertData_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxGeoPath.Text))
            {
                MessageBox.Show("请先选择一个波形文件！");
                return;
            }

            if (comboBoxLineName.SelectedIndex < 0
                || comboBoxLineDir.SelectedIndex < 0 || comboBoxTrainCode.SelectedIndex < 0
                || comboBoxRunDir.SelectedIndex < 0 || comboBoxKmInc.SelectedIndex < 0 ||
                string.IsNullOrEmpty(textBoxStartPos.Text) || string.IsNullOrEmpty(textBoxEndPos.Text))
            {
                MessageBox.Show("请完善基础信息！");
                return;
            }

            

            for (int i = 0; i < listLineCodeAndName.Count; i++)
            {
                if (i == comboBoxLineName.SelectedIndex)
                {
                    lineCode = listLineCodeAndName[i].lineCode;
                    break;
                }
            }
           


            try
            {
                listGEO2CIT = new List<GEO2CITBind>();
                String trainCode = (String)comboBoxTrainCode.SelectedItem;
                if (trainCode.Contains("999307"))
                {
                    iType = 307;//iType=307时，geo转换有特殊情况
                } 
                else
                {
                    iType = 1;
                }

                StreamReader sr = new StreamReader(dicTrainCodeAndConfigPath[trainCode], Encoding.Default);
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
                dhi = new CITDataProcess.DataHeadInfo();
                dhi.iDataType = 1;
                dhi.sDataVersion = "3.0.0";
                dhi.sTrackCode = lineCode;
                dhi.sTrackName = comboBoxLineName.SelectedItem.ToString();
                dhi.sTrain = comboBoxTrainCode.SelectedItem.ToString();
                dhi.sDate = dateTimePickerData.Value.ToString("yyyy-MM-dd");
                dhi.sTime = dateTimePickerTime.Value.ToString("HH:mm:ss");
                dhi.iSmaleRate = 4;
                dhi.iRunDir = comboBoxRunDir.SelectedIndex;
                dhi.iKmInc = comboBoxKmInc.SelectedIndex;
                dhi.iDir = comboBoxLineDir.SelectedIndex + 1;

                //geoProcess.ConvertData(textBoxGeoPath.Text, textBoxGeoPath.Text + "_new" + ".cit", listGEO2CIT, 1, dhi);

                //MessageBox.Show("波形转换完成！\n新文件位置及名字:" + textBoxGeoPath.Text + "_new" + ".cit");

                SetContralEnable(false);
                backgroundWorker1.RunWorkerAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void comboBoxLineDir_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((String)(comboBoxLineDir.SelectedItem) == "单" && (String)(comboBoxRunDir.SelectedItem) == "反")
            {
                isReverseToForward = true;
            } 
            else
            {
                isReverseToForward = false;
            }

            if ((String)(comboBoxLineDir.SelectedItem) == "单" && (String)(comboBoxKmInc.SelectedItem) == "减")
            {
                isMergeKmInc = true;
            }
            else
            {
                isMergeKmInc = false;
            }

            if ((String)(comboBoxLineDir.SelectedItem) == "单" && ((String)(comboBoxKmInc.SelectedItem) == "减" || (String)(comboBoxRunDir.SelectedItem) == "反"))
            {
                panelIIC.Enabled = true;
            } 
            else
            {
                panelIIC.Enabled = false;
            }

        }

        private void comboBoxLineName_SelectedIndexChanged(object sender, EventArgs e)
        {
            String m_LineName = (String)(comboBoxLineName.SelectedItem);

            //读取线路代码表，获取线路代码对应的线路编号和线路名
            try
            {
                //Share Exclusive
                using (OleDbConnection sqlconn = new OleDbConnection(CommonClass.sDBConnectionString))
                {
                    sqlconn.Open();

                    String cmdStr = String.Format("select 线路代码,线路编号,PWMIS原始线路名 from 线路代码表 where PWMIS原始线路名='{0}'", m_LineName);

                    OleDbCommand sqlcom = new OleDbCommand(cmdStr, sqlconn);

                    OleDbDataReader oldr = sqlcom.ExecuteReader();
                    while (oldr.Read())
                    {
                        lineCode = oldr[1].ToString();
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

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            ConvertData();
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            SetContralEnable(true);
            MessageBox.Show("波形转换完成！\n新文件位置及名字:" + textBoxGeoPath.Text + "_new" + ".cit");
        }

        private void Textbox_Check(object sender, EventArgs e)
        {
            TextBox tb = (TextBox)sender;
            String tbStr = tb.Text;

            if (String.IsNullOrEmpty(tbStr))
            {
                MessageBox.Show("里程不能为空！");
                return;
            }
            else
            {
                try
                {
                    float mile = float.Parse(tbStr);
                    if (mile < 0)
                    {
                        MessageBox.Show("里程数必须大于或等于零！");
                        return;
                    }
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show("请输入数字！");
                    return;
                }
            }
        }

        private void comboBoxRunDir_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((String)(comboBoxLineDir.SelectedItem) == "单" && (String)(comboBoxRunDir.SelectedItem) == "反")
            {
                isReverseToForward = true;
            } 
            else
            {
                isReverseToForward = false;
            }

            if ((String)(comboBoxLineDir.SelectedItem) == "单" && ((String)(comboBoxKmInc.SelectedItem) == "减" || (String)(comboBoxRunDir.SelectedItem) == "反"))
            {
                panelIIC.Enabled = true;
            }
            else
            {
                panelIIC.Enabled = false;
            }
        }

        private void comboBoxKmInc_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((String)(comboBoxLineDir.SelectedItem) == "单" && (String)(comboBoxKmInc.SelectedItem) == "减")
            {
                isMergeKmInc = true;
            }
            else
            {
                isMergeKmInc = false;
            }

            if ((String)(comboBoxLineDir.SelectedItem) == "单" && ((String)(comboBoxKmInc.SelectedItem) == "减" || (String)(comboBoxRunDir.SelectedItem) == "反"))
            {
                panelIIC.Enabled = true;
            }
            else
            {
                panelIIC.Enabled = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "IIC文件|*.iic";
            DialogResult dr = openFileDialog1.ShowDialog();
            if (dr == DialogResult.OK)
            {
                sourceIICPath = openFileDialog1.FileName;
                textBoxIICFilePath.Text = sourceIICPath;
                String iicDiretory = Path.GetDirectoryName(sourceIICPath);
                String iicFileName = Path.GetFileNameWithoutExtension(sourceIICPath);
                destIICPath = Path.Combine(iicDiretory, iicFileName + "_new" + ".iic");
            }
        }


        class LineCodeAndNameClass
        {
            public String lineCode;
            public String lineName;
        }

        private void comboBoxLineName_TextChanged(object sender, EventArgs e)
        {
            //String search = comboBoxLineName.Text.Trim();
            //if (string.IsNullOrWhiteSpace(search))
            //{
            //    return;
            //}
            //comboBoxLineName.Items.Clear();

            //results = listLineCodeAndName.FindAll(t => t.lineName.Contains(search));

            //foreach (LineCodeAndNameClass item in results)
            //{
            //    comboBoxLineName.Items.Add(item.lineName);
            //}
            //comboBoxLineName.DroppedDown = true;
            //this.Cursor = Cursors.Arrow;
            //comboBoxLineName.SelectionStart = search.Length;

        }
    }
}
