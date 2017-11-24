using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.IO;
using System.Windows.Forms;
using InGraph.Classes;
using System.Xml;
using System.Data.OleDb;
namespace InGraph
{
    /// <summary>
    /// 系统配置控件类
    /// </summary>
    public partial class SystemConfigForm : Form
    {
        public SystemConfigForm()
        {
            InitializeComponent();

            InitTbStdList();
            InitTbDiyList();
            InitCombobox();
            InitDicExcptnType();
            SetTextBoxEnable(false);


            tabControl1.TabPages.Remove(tabPage1);
            tabControl1.TabPages.Remove(tabPage4);
        }

        #region 事件响应函数---控件加载事件
        /// <summary>
        /// 事件响应函数---控件加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ServerConfigForm_Load(object sender, EventArgs e)
        {
            ServerIPTextBox.Text = CommonClass.ServerAddress;
            ServerPortTextBox.Text = CommonClass.ServerPort.ToString();
            MeterageRadiusTextBox.Text = CommonClass.MeterageRadius.ToString();
            WaveFileTextBox1.Text = CommonClass.sExportWaveFilePath;
            WaveDataTextBox1.Text = CommonClass.sExportWaveDataPath;
            WaveSplitTextBox1.Text = CommonClass.sWaveSplitPath;
            AutoScrollTextBox1.Text=CommonClass.iAutoScroll.ToString();
            checkBoxIsShowInvalidData.Checked = CommonClass.bInvalidDataShow;
            dataGridView1.Rows.Clear();
            for (int i = 0; i < CommonClass.sArrayConfigFile.Length; i++)
            {
                object[] o = new object[2];
                o[0] = (i + 1).ToString();
                o[1] = CommonClass.sArrayConfigFile[i].ToString();
                dataGridView1.Rows.Add(o);
            }

            panel1.BackColor = Color.FromArgb(CommonClass.iHisLayerLabelColor[0]);
            panel2.BackColor = Color.FromArgb(CommonClass.iHisLayerLabelColor[1]);
            panel3.BackColor = Color.FromArgb(CommonClass.iHisLayerLabelColor[2]);
            panel4.BackColor = Color.FromArgb(CommonClass.iHisLayerLabelColor[3]);
            panel5.BackColor = Color.FromArgb(CommonClass.iHisLayerLabelColor[4]);
            panel6.BackColor = Color.FromArgb(CommonClass.iHisLayerLabelColor[5]);
            panel7.BackColor = Color.FromArgb(CommonClass.iHisLayerLabelColor[6]);
            panel8.BackColor = Color.FromArgb(CommonClass.iHisLayerLabelColor[7]);
            panel9.BackColor = Color.FromArgb(CommonClass.iHisLayerLabelColor[8]);
            panel10.BackColor = Color.FromArgb(CommonClass.iHisLayerLabelColor[9]);


            XmlDocument xml = new XmlDocument();
            xml.Load(CommonClass.AppConfigPath);
            checkBox_qx.Checked = Boolean.Parse(xml.DocumentElement["pwmis"].ChildNodes[0].Attributes["graph"].Value);
            checkBox_pd.Checked = Boolean.Parse(xml.DocumentElement["pwmis"].ChildNodes[1].Attributes["graph"].Value);
            checkBox_dch.Checked = Boolean.Parse(xml.DocumentElement["pwmis"].ChildNodes[2].Attributes["graph"].Value);
            checkBox_suduji.Checked = Boolean.Parse(xml.DocumentElement["pwmis"].ChildNodes[4].Attributes["graph"].Value);

        }
        #endregion

        #region 事件响应函数---应用按钮--服务器设置
        /// <summary>
        /// 事件响应函数---应用按钮--服务器设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonServerSet_Click(object sender, EventArgs e)
        {
            //保存服务器地址
            try
            {
                CommonClass.ServerAddress = ServerIPTextBox.Text;
                CommonClass.ServerPort = int.Parse(ServerPortTextBox.Text);
                XmlDocument xd = new XmlDocument();
                xd.Load(CommonClass.AppConfigPath);
                xd.DocumentElement["ServerConfig"].Attributes["server"].Value = CommonClass.ServerAddress;
                xd.DocumentElement["ServerConfig"].Attributes["port"].Value = CommonClass.ServerPort.ToString();
                xd.Save(CommonClass.AppConfigPath);
                MessageBox.Show("保存成功!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion


        private Boolean Textbox_Check(TextBox tb)
        {
            String tbStr = tb.Text;

            if (String.IsNullOrEmpty(tbStr))
            {
                MessageBox.Show("输入框不能为空！");
                return false;
            }
            else
            {
                try
                {
                    int mile = int.Parse(tbStr);
                    if (mile <= 0 || mile >= 100)
                    {
                        MessageBox.Show("请输入大于0小于100的整数！");
                        return false;
                    }
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show("请输入整数！");
                    return false;
                }

            }

            return true;

        }



        #region 事件响应函数---应用--其他设置应用
        /// <summary>
        /// 事件响应函数---应用--其他设置应用
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonOtherSet_Click(object sender, EventArgs e)
        {
            Boolean retVal1 = Textbox_Check(MeterageRadiusTextBox);
            Boolean retVal2 = Textbox_Check(AutoScrollTextBox1);
            if (!retVal1 || !retVal2)
            {                
                return;
            }
            

            CommonClass.MeterageRadius = int.Parse(MeterageRadiusTextBox.Text);
            CommonClass.iAutoScroll = int.Parse(AutoScrollTextBox1.Text);
            CommonClass.bInvalidDataShow = checkBoxIsShowInvalidData.Checked;
            //保存测量半径
            try
            {
                XmlDocument xd = new XmlDocument();
                xd.Load(CommonClass.AppConfigPath);
                xd.DocumentElement["MeterageRadius"].InnerText = CommonClass.MeterageRadius.ToString();
                xd.DocumentElement["InvalidDataShow"].InnerText = CommonClass.bInvalidDataShow.ToString();
                xd.DocumentElement["AutoScroll"].InnerText = CommonClass.iAutoScroll.ToString();

                xd.DocumentElement["pwmis"].ChildNodes[0].Attributes["graph"].Value = checkBox_qx.Checked.ToString();
                xd.DocumentElement["pwmis"].ChildNodes[1].Attributes["graph"].Value = checkBox_pd.Checked.ToString();
                xd.DocumentElement["pwmis"].ChildNodes[2].Attributes["graph"].Value = checkBox_dch.Checked.ToString();
                xd.DocumentElement["pwmis"].ChildNodes[4].Attributes["graph"].Value = checkBox_suduji.Checked.ToString();

                xd.Save(CommonClass.AppConfigPath);
                MessageBox.Show("保存成功!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region 事件响应函数---输入测量半径---最小为1
        /// <summary>
        /// 事件响应函数---输入测量半径---最小为1
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MeterageRadiusTextBox_Leave(object sender, EventArgs e)
        {
            try
            {
                int i = int.Parse(MeterageRadiusTextBox.Text);
                if (i == 0)
                {
                    MeterageRadiusTextBox.Text = "1";
                }
            }
            catch
            {
                MeterageRadiusTextBox.Text = "1";
            }
        }
        #endregion

        #region 事件响应函数----返回按钮
        /// <summary>
        /// 事件响应函数----返回按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion

        #region 事件响应函数----保存配置文件按钮
        /// <summary>
        /// 事件响应函数----保存配置文件按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonConfigFileSet_Click(object sender, EventArgs e)
        {
            try
            {
                XmlDocument xd = new XmlDocument();
                xd.Load(CommonClass.AppConfigPath);
                //xd.DocumentElement["ConfigFiles"].ChildNodes[

                for (int i = 0; i < xd.DocumentElement["ConfigFiles"].ChildNodes.Count;i++ )
                {
                    string sName = xd.DocumentElement["ConfigFiles"].ChildNodes[i].Name;
                    string sId = sName.Substring(7, sName.Length - 7);
                    object o1 = dataGridView1.Rows[int.Parse(sId) - 1].Cells[1].Value;
                    if (o1 == null)
                    { 
                        xd.DocumentElement["ConfigFiles"].ChildNodes[i].InnerText = ""; 
                    }
                    else
                    {
                        xd.DocumentElement["ConfigFiles"].ChildNodes[i].InnerText = o1.ToString(); 
                    }
                    CommonClass.sArrayConfigFile[int.Parse(sId) - 1]=xd.DocumentElement["ConfigFiles"].ChildNodes[i].InnerText;

                }
                    //xd.DocumentElement["ConfigFiles"].Attributes["default" + (i + 1).ToString()].Value = dataGridView1.Rows[i].Cells[1].Value.ToString();

                xd.Save(CommonClass.AppConfigPath);

                MessageBox.Show("保存成功!");
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region 事件响应函数---选择配置文件按钮
        /// <summary>
        /// 事件响应函数---选择配置文件按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonSelectConfigFile_Click(object sender, EventArgs e)
        {
            SelectConfigFilesOpenFileDialog1.FileName = "";
            DialogResult dr = SelectConfigFilesOpenFileDialog1.ShowDialog();
            if (dr == DialogResult.OK)
            {
                for (int i = 0; i < dataGridView1.SelectedRows.Count; i++)
                {
                    dataGridView1.SelectedRows[i].Cells[1].Value = SelectConfigFilesOpenFileDialog1.FileName;
                }
            }
        }
        #endregion


        #region 事件响应函数---选择按钮--导出波形片段路径
        /// <summary>
        /// 事件响应函数---选择按钮--导出波形片段路径
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonExptSpltWavFlePth_Click(object sender, EventArgs e)
        {
            DialogResult dr= folderBrowserDialog1.ShowDialog();
            if (dr == DialogResult.OK)
            {
                WaveFileTextBox1.Text = folderBrowserDialog1.SelectedPath;
            }
        }
        #endregion

        #region 事件响应函数---选择按钮--导出波形数据路径
        /// <summary>
        /// 事件响应函数---选择按钮--导出波形数据路径
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonExptWavDataPth_Click(object sender, EventArgs e)
        {
            DialogResult dr = folderBrowserDialog1.ShowDialog();
            if (dr == DialogResult.OK)
            {
                WaveDataTextBox1.Text = folderBrowserDialog1.SelectedPath;
            }
        }
        #endregion

        #region 事件响应函数---应用按钮--导出设置
        /// <summary>
        /// 事件响应函数---应用按钮--导出设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonExptSet_Click(object sender, EventArgs e)
        {
            try
            {
                XmlDocument xd = new XmlDocument();
                xd.Load(CommonClass.AppConfigPath);
                //xd.DocumentElement["ConfigFiles"].ChildNodes[
                xd.DocumentElement["ExportData"].ChildNodes[0].InnerText = WaveFileTextBox1.Text;
                xd.DocumentElement["ExportData"].ChildNodes[1].InnerText = WaveDataTextBox1.Text;
                xd.DocumentElement["ExportData"].ChildNodes[2].InnerText = WaveSplitTextBox1.Text;
                CommonClass.sExportWaveFilePath=WaveFileTextBox1.Text ;
                CommonClass.sExportWaveDataPath=WaveDataTextBox1.Text;
                CommonClass.sWaveSplitPath = WaveSplitTextBox1.Text;
                //xd.DocumentElement["ConfigFiles"].Attributes["default" + (i + 1).ToString()].Value = dataGridView1.Rows[i].Cells[1].Value.ToString();

                xd.Save(CommonClass.AppConfigPath);

                MessageBox.Show("保存成功!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region 事件响应函数---图层颜色选择
        /// <summary>
        /// 事件响应函数---图层颜色选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void panelColorSelect_Click(object sender, EventArgs e)
        {
            DialogResult dr = colorDialog1.ShowDialog();
            if (dr == DialogResult.OK)
            {
                ((Panel)sender).BackColor = colorDialog1.Color;
            }

        }
        #endregion

        #region 事件响应函数---应用按钮--图层颜色应用
        /// <summary>
        /// 事件响应函数---应用按钮--图层颜色应用
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonColorSet_Click(object sender, EventArgs e)
        {
            CommonClass.iHisLayerLabelColor[0] = panel1.BackColor.ToArgb();
            CommonClass.iHisLayerLabelColor[1] = panel2.BackColor.ToArgb();
            CommonClass.iHisLayerLabelColor[2] = panel3.BackColor.ToArgb();
            CommonClass.iHisLayerLabelColor[3] = panel4.BackColor.ToArgb();
            CommonClass.iHisLayerLabelColor[4] = panel5.BackColor.ToArgb();
            CommonClass.iHisLayerLabelColor[5] = panel6.BackColor.ToArgb();
            CommonClass.iHisLayerLabelColor[6] = panel7.BackColor.ToArgb();
            CommonClass.iHisLayerLabelColor[7] = panel8.BackColor.ToArgb();
            CommonClass.iHisLayerLabelColor[8] = panel9.BackColor.ToArgb();
            CommonClass.iHisLayerLabelColor[9] = panel10.BackColor.ToArgb();
            try
            {
                XmlDocument xd = new XmlDocument();
                xd.Load(CommonClass.AppConfigPath);
                //xd.DocumentElement["ConfigFiles"].ChildNodes[
                xd.DocumentElement["HisLayer"].ChildNodes[0].InnerText = CommonClass.iHisLayerLabelColor[0].ToString();
                xd.DocumentElement["HisLayer"].ChildNodes[1].InnerText = CommonClass.iHisLayerLabelColor[1].ToString();
                xd.DocumentElement["HisLayer"].ChildNodes[2].InnerText = CommonClass.iHisLayerLabelColor[2].ToString();
                xd.DocumentElement["HisLayer"].ChildNodes[3].InnerText = CommonClass.iHisLayerLabelColor[3].ToString();
                xd.DocumentElement["HisLayer"].ChildNodes[4].InnerText = CommonClass.iHisLayerLabelColor[4].ToString();
                xd.DocumentElement["HisLayer"].ChildNodes[5].InnerText = CommonClass.iHisLayerLabelColor[5].ToString();
                xd.DocumentElement["HisLayer"].ChildNodes[6].InnerText = CommonClass.iHisLayerLabelColor[6].ToString();
                xd.DocumentElement["HisLayer"].ChildNodes[7].InnerText = CommonClass.iHisLayerLabelColor[7].ToString();
                xd.DocumentElement["HisLayer"].ChildNodes[8].InnerText = CommonClass.iHisLayerLabelColor[8].ToString();
                xd.DocumentElement["HisLayer"].ChildNodes[9].InnerText = CommonClass.iHisLayerLabelColor[9].ToString();

                xd.Save(CommonClass.AppConfigPath);

                MessageBox.Show("保存成功!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region 事件响应函数---选择按钮--分割波形路径选择
        /// <summary>
        /// 事件响应函数---选择按钮--分割波形路径选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonSpltWavPth_Click(object sender, EventArgs e)
        {
            DialogResult dr = folderBrowserDialog1.ShowDialog();
            if (dr == DialogResult.OK)
            {
                WaveSplitTextBox1.Text = folderBrowserDialog1.SelectedPath;
            }
        }
        #endregion


        #region 字典--存放项目类型的中文英文对照
        /// <summary>
        /// 字典--存放项目类型的中文英文对照
        /// </summary>
        public Dictionary<String, String> dicExcptnType = new Dictionary<String, String>();
        #endregion
        private List<TextBox> tbStdList = new List<TextBox>();
        private List<TextBox> tbDiyList = new List<TextBox>();
        private int standerdType = 0;
        private int defectLevel = 1;
        private int speedClass = 120;
        private List<ExceptionStndAndDiyClass> excptnStdAndDiyClsList = new List<ExceptionStndAndDiyClass>();
        #region 数据类--超限标准值和自定义值表
        /// <summary>
        /// 数据类--超限标准值和自定义值表
        /// </summary>
        private class ExceptionStndAndDiyClass
        {
            #region 序号
            /// <summary>
            /// 序号
            /// </summary>
            public int id;
            #endregion
            #region 速度等级
            /// <summary>
            /// 速度等级
            /// </summary>
            public int speed;
            #endregion
            #region 超限等级
            /// <summary>
            /// 超限等级
            /// </summary>
            public int level;
            #endregion
            #region 超限类型
            /// <summary>
            /// 超限类型
            /// </summary>
            public String type;
            #endregion
            #region 超限值--国家标准
            /// <summary>
            /// 超限值--国家标准
            /// </summary>
            public float valueStandard;
            #endregion
            #region 超限值--自定义
            /// <summary>
            /// 超限值--自定义
            /// </summary>
            public float valueDIY;
            #endregion
            #region 标准类型
            /// <summary>
            /// 标准类型
            /// </summary>
            public int standardType;
            #endregion
        }
        #endregion

        private void InitCombobox()
        {
            comboBoxStanderdType.SelectedIndex = 0;
            comboBoxDefectLevel.SelectedIndex = 0;
            comboBoxSpeedClass.SelectedIndex = 0;
        }

        private void InitTbStdList()
        {
            if (tbStdList.Count > 0)
            {
                tbStdList.Clear();
            }
            tbStdList.Add(tb_Std_Align_SC);
            tbStdList.Add(tb_Std_Align_SC_120);
            tbStdList.Add(tb_Std_Align_SC_70);
            tbStdList.Add(tb_Std_CrossLevel);
            tbStdList.Add(tb_Std_WideGage);
            tbStdList.Add(tb_Std_NarrowGage);
            tbStdList.Add(tb_Std_Short_Twist);
            tbStdList.Add(tb_Std_LACC);
            tbStdList.Add(tb_Std_VACC);
            tbStdList.Add(tb_Std_Prof_SC);
            tbStdList.Add(tb_Std_Prof_SC_120);
            tbStdList.Add(tb_Std_Prof_SC_70);
        }
        private void InitTbDiyList()
        {
            if (tbDiyList.Count > 0)
            {
                tbDiyList.Clear();
            }
            tbDiyList.Add(tb_Diy_Align_SC);
            tbDiyList.Add(tb_Diy_Align_SC_120);
            tbDiyList.Add(tb_Diy_Align_SC_70);
            tbDiyList.Add(tb_Diy_CrossLevel);
            tbDiyList.Add(tb_Diy_WideGage);
            tbDiyList.Add(tb_Diy_NarrowGage);
            tbDiyList.Add(tb_Diy_Short_Twist);
            tbDiyList.Add(tb_Diy_LACC);
            tbDiyList.Add(tb_Diy_VACC);
            tbDiyList.Add(tb_Diy_Prof_SC);
            tbDiyList.Add(tb_Diy_Prof_SC_120);
            tbDiyList.Add(tb_Diy_Prof_SC_70);
        }

        private void SetTextBoxEnable(Boolean b)
        {
            foreach (TextBox tb in tbStdList)
            {
                tb.Text = "";
                tb.Enabled = b;
            }

            foreach (TextBox tb in tbDiyList)
            {
                tb.Text = "";
                tb.Enabled = b;
            }
        }

        private void InitDicExcptnType()
        {
            dicExcptnType.Clear();

            dicExcptnType.Add("Prof_SC", "高低_中波");
            dicExcptnType.Add("Prof_SC_70", "高低_70长波");
            dicExcptnType.Add("Prof_SC_120", "高低_120长波");
            dicExcptnType.Add("Align_SC", "轨向_中波");
            dicExcptnType.Add("Align_SC_70", "轨向_70长波");
            dicExcptnType.Add("Align_SC_120", "轨向_120长波");
            dicExcptnType.Add("WideGage", "大轨距");
            dicExcptnType.Add("NarrowGage", "小轨距");
            dicExcptnType.Add("CrossLevel", "水平");
            dicExcptnType.Add("Short_Twist", "三角坑");
            dicExcptnType.Add("LACC", "车体横加");
            dicExcptnType.Add("VACC", "车体垂加");
        }

        private Boolean CheckTbStdList()
        {
            StringBuilder sbError = new StringBuilder();
            Boolean retVal = true;//是否有输入值格式不正确

            foreach (TextBox tb in tbStdList)
            {
                String excptnType = tb.Name.Substring(7);

                try
                {
                    if (tb.Enabled)
                    {
                        float value = float.Parse(tb.Text.Trim());

                    }
                }
                catch (System.Exception ex)
                {
                    sbError.AppendLine(tb.Text);
                }
            }

            sbError.ToString().Trim();
            if (sbError.Length == 0)
            {
                retVal = true;
                return retVal;
            }
            sbError.Insert(0, "输入值不正确：\n");
            MessageBox.Show(sbError.ToString());
            retVal = false;
            return retVal;
        }

        private Boolean CheckTbDiyList()
        {
            StringBuilder sbError = new StringBuilder();
            Boolean retVal = true;//是否有输入值格式不正确

            foreach (TextBox tb in tbDiyList)
            {
                String excptnType = tb.Name.Substring(8);

                try
                {
                    if (tb.Enabled)
                    {
                        float value = float.Parse(tb.Text.Trim());

                        foreach (TextBox tbnew in tbStdList)
                        {
                            if (tbnew.Name.Substring(7).Equals(excptnType))
                            {
                                float tmpVal = float.Parse(tbnew.Text);
                                if (tmpVal > 0)
                                {
                                    if (value > 0 && value < tmpVal)
                                    {

                                    }
                                    else
                                    {
                                        String errSth = String.Format("{0}:输入值必须大于0，小于{1}", dicExcptnType[excptnType],tmpVal.ToString());
                                        sbError.AppendLine(errSth);
                                    }
                                }

                                if (tmpVal < 0)
                                {
                                    if (value < 0 && value > tmpVal)
                                    {

                                    }
                                    else
                                    {
                                        String errSth = String.Format("{0}:输入值必须小于0，大于{1}", dicExcptnType[excptnType], tmpVal.ToString());
                                        sbError.AppendLine(errSth);
                                    }
                                }

                            }
                        }

                    }
                }
                catch (System.Exception ex)
                {
                    sbError.AppendLine(tb.Text);
                }
            }

            sbError.ToString().Trim();
            if (sbError.Length == 0)
            {
                retVal = true;
                return retVal;
            }
            sbError.Insert(0, "输入值不正确：\n");
            MessageBox.Show(sbError.ToString());
            retVal = false;
            return retVal;
        }

        private Boolean UpdateExcptnValRecmmd()
        {
            Boolean retVal = true;//是否有输入值格式不正确

            foreach (TextBox tb in tbDiyList)
            {
                String excptnType = tb.Name.Substring(7);

                if (tb.Enabled)
                {
                    float value = float.Parse(tb.Text.Trim());

                    foreach (ExceptionStndAndDiyClass tmpCls in excptnStdAndDiyClsList)
                    {
                        if (tmpCls.type == excptnType)
                        {
                            tmpCls.valueDIY = value;
                        }
                    }
                }
            }

            foreach (TextBox tb in tbStdList)
            {
                String excptnType = tb.Name.Substring(7);

                if (tb.Enabled)
                {
                    float value = float.Parse(tb.Text.Trim());

                    foreach (ExceptionStndAndDiyClass tmpCls in excptnStdAndDiyClsList)
                    {
                        if (tmpCls.type == excptnType)
                        {
                            tmpCls.valueStandard = value;
                        }
                    }
                }
            }



            using (OleDbConnection sqlconn = new OleDbConnection(CommonClass.sDBConnectionString))
            {
                sqlconn.Open();

                String cmd;
                OleDbCommand sqlcom;

                foreach (ExceptionStndAndDiyClass tmpCls in excptnStdAndDiyClsList)
                {
                    cmd = String.Format("update 大值国家标准表 set VALUEDIY={0},VALUESTANDARD={1} where ID = {2}", tmpCls.valueDIY,tmpCls.valueStandard, tmpCls.id);
                    sqlcom = new OleDbCommand(cmd, sqlconn);
                    sqlcom.ExecuteNonQuery();
                }

                sqlconn.Close();
            }

            MessageBox.Show("保存成功");
            return retVal;
        }

        private void SetTextBoxRecmmdEnable()
        {
            SetTextBoxEnable(false);

            foreach (ExceptionStndAndDiyClass tmpCls in excptnStdAndDiyClsList)
            {
                for (int i = 0; i < tbStdList.Count; i++)
                {
                    if (tbStdList[i].Name.Substring(7).Equals(tmpCls.type.ToString()))
                    {
                        tbStdList[i].Text = tmpCls.valueStandard.ToString();

                        if (tmpCls.valueDIY == 0)
                        {
                            //把textbox设为enable
                            if (tmpCls.valueStandard >= 0)
                            {
                                tbDiyList[i].Text = (tmpCls.valueStandard - 1).ToString();
                            }
                            else
                            {
                                tbDiyList[i].Text = (tmpCls.valueStandard + 1).ToString();
                            }
                        }
                        else
                        {
                            tbDiyList[i].Text = tmpCls.valueDIY.ToString();
                        }

                        if (checkBoxSenior.Checked && comboBoxStanderdType.SelectedIndex == 1)
                        {
                            tbStdList[i].Enabled = true;
                        }                        
                        tbDiyList[i].Enabled = true;
                    }
                }
            }
        }

        private void QueryExcptn(int standerdType,int defectLevel,int speedClass)
        {
            using (OleDbConnection sqlconn = new OleDbConnection(CommonClass.sDBConnectionString))
            {
                String cmd = String.Format("select *  from 大值国家标准表 where CLASS = {0} and SPEED = {1} and STANDARDTYPE = {2}", defectLevel, speedClass, standerdType);
                OleDbCommand sqlcom = new OleDbCommand(cmd, sqlconn);

                sqlconn.Open();
                OleDbDataReader oddr = sqlcom.ExecuteReader();

                excptnStdAndDiyClsList.Clear();
                ExceptionStndAndDiyClass tmpCls;

                while (oddr.Read())
                {
                    //把当前的超限值读取
                    tmpCls = new ExceptionStndAndDiyClass();
                    tmpCls.id = int.Parse(oddr[0].ToString());
                    tmpCls.speed = int.Parse(oddr[1].ToString());
                    tmpCls.level = int.Parse(oddr[2].ToString());
                    tmpCls.type = oddr[3].ToString();
                    tmpCls.valueStandard = float.Parse(oddr[4].ToString());
                    //如果自定义值不存在，自动置比国家标准值小1
                    if (oddr[5].ToString() == "")
                    {
                        //tmpCls.valueDIY = 0f;

                        if (tmpCls.valueStandard >= 0)
                        {
                            tmpCls.valueDIY = tmpCls.valueStandard - 1;
                        }
                        else
                        {
                            tmpCls.valueDIY = tmpCls.valueStandard + 1;
                        }
                    }
                    else
                    {
                        tmpCls.valueDIY = float.Parse(oddr[5].ToString());
                    }
                    tmpCls.standardType = int.Parse(oddr[6].ToString());

                    excptnStdAndDiyClsList.Add(tmpCls);
                }

                oddr.Close();
                sqlconn.Close();
            }

            SetTextBoxRecmmdEnable();
        }

        private void comboBoxStanderdType_SelectedIndexChanged(object sender, EventArgs e)
        {
            standerdType = comboBoxStanderdType.SelectedIndex;

            checkBoxSenior_CheckedChanged(sender, e);
        }

        private void comboBoxDefectLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            defectLevel = int.Parse(comboBoxDefectLevel.SelectedItem.ToString());
        }

        private void comboBoxSpeedClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            speedClass = int.Parse(comboBoxSpeedClass.SelectedItem.ToString());
        }

        private void buttonQurey_Click(object sender, EventArgs e)
        {
            QueryExcptn(standerdType, defectLevel, speedClass);
        }

        private void button_Save_Click(object sender, EventArgs e)
        {
            button_Save.Enabled = false;
            Boolean retVal1 = CheckTbStdList();
            Boolean retVal2 = CheckTbDiyList();
            if (retVal1 == false || retVal2 == false)
            {
                return;
            }
            Boolean retval = UpdateExcptnValRecmmd();
            button_Save.Enabled = true;
            if (retval == false)
            {
                return;
            }
        }

        private void checkBoxSenior_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxSenior.Checked && comboBoxStanderdType.SelectedIndex == 1)
            {
                for (int i = 0; i < tbDiyList.Count;i++ )
                {
                    if (tbDiyList[i].Enabled)
                    {
                        tbStdList[i].Enabled = true;
                    }
                }
            } 
            else
            {
                for (int i = 0; i < tbDiyList.Count; i++)
                {
                    if (tbDiyList[i].Enabled)
                    {
                        tbStdList[i].Enabled = false;
                    }
                }
            }
        }



    }
}
