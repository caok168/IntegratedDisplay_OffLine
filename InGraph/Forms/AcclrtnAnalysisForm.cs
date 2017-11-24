using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Data.OleDb;
using Steema.TeeChart.Styles;
using InGraph.Classes;
using System.Reflection;

namespace InGraph.Forms
{
    /// <summary>
    /// 窗体---轴箱加速度分析
    /// </summary>
    public partial class AcclrtnAnalysisForm : Form
    {

        String citFilePath_section = null;
        String citDirectoryPath = null;
        String idfFile = null;
        String tableName = null;
        // liyang: linename从idf 文件中的第一个表CitFileInfo的第二列来。
        String lineName = null;
        DaoChaMileRange daoChaMileRange = null;
        String iniFilePath = Application.StartupPath + "\\config.ini";
        List<Dictionary<String, String>> dicList = null;
        Dictionary<String, String> dic = null;
        Boolean reShow = false;

        #region true：轨道冲击指数；false：加速度有效值
        /// <summary>
        /// true：轨道冲击指数；false：加速度有效值
        /// </summary>
        Boolean isTPI = false;//散点图是否显示轨道冲击指数
        #endregion


        public AcclrtnAnalysisForm()
        {
            InitializeComponent();

            tabControl2.TabPages.Remove(tabPage4);
            tabControl1.TabPages.Remove(tabPage1);
        }


        private void checkBoxCompare_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxCompare.Checked)
            {
                textBoxCompareFileName.Enabled = true;
                buttonCompareFileBrowser.Enabled = true;
                buttonCompareOK.Enabled = true;
            } 
            else
            {
                textBoxCompareFileName.Enabled = false;
                buttonCompareFileBrowser.Enabled = false;
                buttonCompareOK.Enabled = false;
            }
        }

        private void textBoxFolder_TextChanged(object sender, EventArgs e)
        {

        }

        private void buttonFolder_Click(object sender, EventArgs e)
        {
            DialogResult dr = folderBrowserDialog1.ShowDialog();
            if (dr == DialogResult.Cancel)
            {
                return;
            }
            if (dr == DialogResult.OK)
            {
                textBoxFolder.Text = folderBrowserDialog1.SelectedPath;
            }

            //给加速度功率谱的选择框赋值
            citDirectoryPath = textBoxFolder.Text;

            //comboBoxFilePowerSpectrun.Items.Clear();
            //String searchPatten_X_cit = "*X.cit";
            //String[] xCitFilesPath = Directory.GetFiles(citDirectoryPath, searchPatten_X_cit, SearchOption.TopDirectoryOnly);
            
            //foreach (String filePath in xCitFilesPath)
            //{
            //    String tmpStr = Path.GetFileName(filePath);
            //    comboBoxFilePowerSpectrun.Items.Add(tmpStr);
            //}
            //String searchPatten_S_cit = "*S.cit";
            //String[] sCitFilesPath = Directory.GetFiles(citDirectoryPath, searchPatten_S_cit, SearchOption.TopDirectoryOnly);
            //foreach (String filePath in sCitFilesPath)
            //{
            //    String tmpStr = Path.GetFileName(filePath);
            //    comboBoxFilePowerSpectrun.Items.Add(tmpStr);
            //}

            //if (comboBoxFilePowerSpectrun.Items.Count != 0)
            //{
            //    comboBoxFilePowerSpectrun.SelectedIndex = 0;
            //    citFilePath_source = (String)(comboBoxFilePowerSpectrun.SelectedItem);
            //}


            comboBoxFileAcclrtn.Items.Clear();
            String searchPatten_rmsidf = "*Rms.idf";
            String[] idfFilesPath = Directory.GetFiles(citDirectoryPath, searchPatten_rmsidf, SearchOption.TopDirectoryOnly);

            foreach (String filePath in idfFilesPath)
            {
                String tmpStr = Path.GetFileName(filePath);
                ////排除原始cit文件
                //if (!(tmpStr.Contains("X.cit") || tmpStr.Contains("S.cit")))
                //{
                //    comboBoxFileAcclrtn.Items.Add(tmpStr);
                //}
                comboBoxFileAcclrtn.Items.Add(tmpStr);
            }
            if (comboBoxFileAcclrtn.Items.Count != 0)
            {
                comboBoxFileAcclrtn.SelectedIndex = 0;
                citFilePath_section = (String)(comboBoxFileAcclrtn.SelectedItem);
            }

            AddMenuItem();

        }

        private void comboBoxFilePowerSpectrun_Click(object sender, EventArgs e)
        {
            if (textBoxFolder.Text.Equals(""))
            {
                MessageBox.Show("请选择加速度波形文件所在文件夹！");
            } 
        }

        private double[][] QuerySUDU(string sSQL)
        {


            double[][] dReturnValue = new double[2][];
            using (OleDbConnection sqlconn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + this.idfFile + ";Persist Security Info=True"))
            {

                List<double> l1 = new List<double>();
                List<double> l2 = new List<double>();
                OleDbCommand sqlcom = new OleDbCommand(sSQL, sqlconn);

                sqlconn.Open();
                OleDbDataReader sdr = sqlcom.ExecuteReader();

                while (sdr.Read())
                {
                    l1.Add(float.Parse(sdr.GetValue(0).ToString()));
                    l2.Add(float.Parse(sdr.GetValue(1).ToString()));
                }
                sdr.Close();
                sqlconn.Close();


                dReturnValue[0] = new double[l1.Count];
                dReturnValue[1] = new double[l2.Count];
                dReturnValue[0] = l1.ToArray();
                dReturnValue[1] = l2.ToArray();

            }

            return dReturnValue;

        }

        private void PictureChart(String idfFile, String tableName)
        {
            SegmentRmsClass segmentRmsCls = GetTableValue(idfFile, tableName, " where valid = 1");
            double[][] d = QuerySUDU("select KiloMeter,Speed from " + tableName + " where valid = 1 order by KiloMeter");

            tChart1.Series.Clear();
            tChart1.Chart.Header.Text = "50米区段大值分布散点图";

            //Points ps = new Points();
            //for (int i = 0; i < segmentRmsCls.idList.Count; i++)
            //{
            //    ps.Add(segmentRmsCls.kiloMeterList[i], segmentRmsCls.segmentRmsList[i]);
            //}
            //ps.Color = Color.Blue;
            ////l.LinePen.Width = 1;
            //ps.Pointer.VertSize = 3;
            //ps.Pointer.HorizSize = 3;            
            //ps.ShowInLegend = false;


            Points psBlue = new Points();
            Points psRed = new Points();

            if (isTPI == false)
            {
                tChart1.Text = "50米区段大值散点图";
                for (int i = 0; i < segmentRmsCls.segmentRmsItemClsList.Count; i++)
                {
                    if (segmentRmsCls.segmentRmsItemClsList[i].segmentRmsPeak < float.Parse(textBoxPeakLine.Text))
                    {
                        psBlue.Add(segmentRmsCls.segmentRmsItemClsList[i].kiloMeter, segmentRmsCls.segmentRmsItemClsList[i].segmentRms);
                    }
                    else
                    {
                        if (reShow)
                        {
                            psRed.Add(segmentRmsCls.segmentRmsItemClsList[i].kiloMeter, segmentRmsCls.segmentRmsItemClsList[i].segmentRms);
                        }
                        else
                        {
                            psBlue.Add(segmentRmsCls.segmentRmsItemClsList[i].kiloMeter, segmentRmsCls.segmentRmsItemClsList[i].segmentRms);
                        }
                    }
                }
            } 
            else
            {
                tChart1.Text = "轨道冲击指数散点图";
                for (int i = 0; i < segmentRmsCls.segmentRmsItemClsList.Count; i++)
                {
                    if (segmentRmsCls.segmentRmsItemClsList[i].segmentRmsPeak < float.Parse(textBoxPeakLine.Text))
                    {
                        psBlue.Add(segmentRmsCls.segmentRmsItemClsList[i].kiloMeter, segmentRmsCls.segmentRmsItemClsList[i].segmentRmsPeak);
                    }
                    else
                    {
                        if (reShow)
                        {
                            psRed.Add(segmentRmsCls.segmentRmsItemClsList[i].kiloMeter, segmentRmsCls.segmentRmsItemClsList[i].segmentRmsPeak);
                        }
                        else
                        {
                            psBlue.Add(segmentRmsCls.segmentRmsItemClsList[i].kiloMeter, segmentRmsCls.segmentRmsItemClsList[i].segmentRmsPeak);
                        }
                    }
                }
            }


            psBlue.Color = Color.Blue;
            psBlue.Pointer.VertSize = 3;
            psBlue.Pointer.HorizSize = 3;
            psBlue.ShowInLegend = false;

            psRed.Color = Color.Red;
            psRed.Pointer.VertSize = 3;
            psRed.Pointer.HorizSize = 3;
            psRed.ShowInLegend = false;

            tChart1.Axes.Left.Automatic = true;
            tChart1.Axes.Bottom.Automatic = true;

            tChart1.Axes.Bottom.Title.Text = "里程";
            tChart1.Axes.Bottom.Title.TextAlign = StringAlignment.Center;

            if (isTPI == true)
            {
                tChart1.Axes.Left.Title.Text = "轨道冲击指数";
            }
            else
            {
                tChart1.Axes.Left.Title.Text = "有效值";
            }
            tChart1.Axes.Left.Title.TextAlign = StringAlignment.Center;
            tChart1.Axes.Left.AutomaticMinimum = false;
            tChart1.Axes.Left.Minimum = 0;

            tChart1.Axes.Right.Title.Text = "速度";
            tChart1.Axes.Right.Title.TextAlign = StringAlignment.Center;
            tChart1.Axes.Right.AutomaticMinimum = false;
            tChart1.Axes.Right.Minimum = 0;
            tChart1.Axes.Right.MinAxisIncrement = 1;

            Line l = new Line();
            for (int i = 0; i < d[0].Length; i++)
            {
                l.Add(d[0][i], d[1][i]);
            }
            l.Color = Color.RosyBrown;
            l.LinePen.Width = 3;
            l.ShowInLegend = false;
            l.CustomVertAxis = tChart1.Axes.Right;
            //tChart1.Axes.Right.Automatic = true;
            tChart1.Axes.Right.Visible = true;
            tChart1.Series.Add(l);

            String pictureName = String.Format("{0}_{1}.jpg", tableName, tChart1.Text);
            String picturePath = Path.Combine(citDirectoryPath, pictureName);

            tChart1.Series.Add(psBlue);
            tChart1.Series.Add(psRed);
            //tChart1.Export.Image.JPEG.Save(citDirectoryPath + "\\50米区段大值分布散点图.jpg");
            tChart1.Export.Image.JPEG.Save(picturePath);
        }

        //liyang: 选中一个idf, 或者在图表(左边)右键菜单选中一个别的通道时候，调用。
        private void ShowMaxIn50m(String idfFile, String tableName)
        {
            SegmentRmsClass segmentRmsCls = GetTableValue(idfFile, tableName,"");
            //ShowList(segmentRmsCls);
            ShowDataGridView(segmentRmsCls);

            // liyang: 用来显示最后一列，是否道岔，如果this.daoChaMileRange为空(道岔位置表还没有加载)，这个函数不起作用。
            ShowDataGridView(this.daoChaMileRange);
        }


        private void ShowDataGridView(SegmentRmsClass segmentRmsCls)
        {           

            dataGridView1.Rows.Clear();
            for (int i = 0; i < segmentRmsCls.segmentRmsItemClsList.Count; i++)
            {
               
                DataGridViewRow dgvr = new DataGridViewRow();
                dgvr.CreateCells(dataGridView1); 

                dgvr.Cells[0].Value = segmentRmsCls.segmentRmsItemClsList[i].id;
                dgvr.Cells[1].Value = segmentRmsCls.segmentRmsItemClsList[i].kiloMeter;
                dgvr.Cells[2].Value = segmentRmsCls.segmentRmsItemClsList[i].speed;
                dgvr.Cells[3].Value = segmentRmsCls.segmentRmsItemClsList[i].segmentRms;
                dgvr.Cells[4].Value = segmentRmsCls.segmentRmsItemClsList[i].segmentRmsPeak;
                dgvr.Cells[5].Value = segmentRmsCls.segmentRmsItemClsList[i].valid;

                // liyang: 增加一列的数据，表示是否是道岔，这一列先加到所有列的后面，因为程序中别的地方用列的索引的时候是写死的
                //dgvr.Cells[5].Value = segmentRmsCls.segmentRmsItemClsList[i].isDaoCha ? "是" ："否";
                dgvr.Cells[6].Value = "请加载道岔位置表";

                if (segmentRmsCls.segmentRmsItemClsList[i].valid == 0)
                {
                    dgvr.DefaultCellStyle.BackColor = Color.LightGray;
                }
                else
                {
                    if (segmentRmsCls.segmentRmsItemClsList[i].segmentRmsPeak >= float.Parse(textBoxPeakLine.Text))
                    {
                        dgvr.Cells[4].Style.BackColor = Color.Red;
                    }
                }
                dataGridView1.Rows.Add(dgvr);
                
            }

            AddListMenuItem();
        }

        private void ShowDataGridView()
        {
            string yes = "是";

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (float.Parse(dataGridView1.Rows[i].Cells[5].Value.ToString()) == 0)
                {
                    dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.LightGray;
                }
                else
                {
                    // liyang: 这里加入对 是否 道岔的判断

                    string isdaocha = dataGridView1.Rows[i].Cells[6].Value.ToString();
                    // liyang: 针对于直线和曲线
                    if (!yes.Equals(isdaocha))
                    {
                        if (float.Parse(dataGridView1.Rows[i].Cells[4].Value.ToString()) >= float.Parse(textBoxPeakLine.Text))
                        {
                            dataGridView1.Rows[i].Cells[4].Style.BackColor = Color.Red;
                        }
                        else
                        {
                            dataGridView1.Rows[i].Cells[4].Style.BackColor = Color.White;
                        }
                    }
                    else //liyang:针对于道岔
                    {
                        if (float.Parse(dataGridView1.Rows[i].Cells[4].Value.ToString()) >= float.Parse(textBoxDaoChaPeakLine.Text))
                        {
                            dataGridView1.Rows[i].Cells[4].Style.BackColor = Color.Red;
                        }
                        else
                        {
                            dataGridView1.Rows[i].Cells[4].Style.BackColor = Color.White;
                        }
                    }

                }
            }
        }

        private void ShowDataGridView(DaoChaMileRange mr)
        {
            if (null == mr)
                return;

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                float mile = float.Parse(dataGridView1.Rows[i].Cells[1].Value.ToString());
                if (mr.IsDaoChao(mile))
                {
                    dataGridView1.Rows[i].Cells[6].Value = "是";
                }
                else
                {
                    dataGridView1.Rows[i].Cells[6].Value = "否";
                }
            }
        }

        private void AddListMenuItem()
        {
            contextMenuStrip2.Items.Clear();

            ToolStripMenuItem tsmi_Valid = new ToolStripMenuItem();
            tsmi_Valid.Text = "设置有效";
            tsmi_Valid.Click+=new EventHandler(tsmi_Valid_Click);
            contextMenuStrip2.Items.Add(tsmi_Valid);

            ToolStripMenuItem tsmi_Invalid = new ToolStripMenuItem();
            tsmi_Invalid.Text = "设置无效";
            tsmi_Invalid.Click+=new EventHandler(tsmi_Invalid_Click);
            contextMenuStrip2.Items.Add(tsmi_Invalid);

        }

        private void tsmi_Valid_Click(object sender, EventArgs e)
        {
            int selectItemNum = dataGridView1.SelectedRows.Count;
            StringBuilder sb = new StringBuilder();
            foreach (DataGridViewRow dgvr in dataGridView1.SelectedRows)
            {
                dgvr.Cells[5].Value = "1";
                dgvr.DefaultCellStyle.BackColor = Color.White;
                sb.AppendFormat("{0},", int.Parse(dgvr.Cells[0].Value.ToString()));
            }

            sb.Remove(sb.Length - 1, 1);


            String sql = "update " + this.tableName + " set valid = 1 where Id in (" + sb.ToString() + ")";
            ExcuteSQL(sql, idfFile);

            PictureChart(idfFile, this.tableName);
        }
        private void tsmi_Invalid_Click(object sender, EventArgs e)
        {
            int selectItemNum = dataGridView1.SelectedRows.Count;
            StringBuilder sb1 = new StringBuilder();

            foreach (DataGridViewRow dgvr in dataGridView1.SelectedRows)
            {
                dgvr.Cells[5].Value = "0";
                dgvr.DefaultCellStyle.BackColor = Color.Gray;
                sb1.AppendFormat("{0},", int.Parse(dgvr.Cells[0].Value.ToString()));
            }

            sb1.Remove(sb1.Length - 1, 1);

            //把里程为0的都设置为无效
            StringBuilder sb2 = new StringBuilder();
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                string str = dataGridView1.Rows[i].Cells[1].Value.ToString();
                if (str == "0")
                {
                    dataGridView1.Rows[i].Cells[5].Value = "0";
                    dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.Gray;
                    sb2.AppendFormat("{0},", dataGridView1.Rows[i].Cells[0].Value.ToString());
                }
            }

            if (!String.IsNullOrEmpty(sb2.ToString()))
            {
                sb2.Remove(sb2.Length - 1, 1);
                String sql2 = "update " + this.tableName + " set valid = 0 where Id in (" + sb2.ToString() + ")";
                ExcuteSQL(sql2, idfFile);
            }
            
            

            String sql1 = "update " + this.tableName + " set valid = 0 where Id in (" + sb1.ToString() + ")";            
            ExcuteSQL(sql1, idfFile);
            

            PictureChart(idfFile, this.tableName);
        }

        private void ExcuteSQL(String sql, String fileName)
        {
            using (OleDbConnection sqlconn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + fileName + ";Persist Security Info=True"))
            {

                String sSQL = sql;
                OleDbCommand sqlcom = new OleDbCommand(sSQL, sqlconn);

                sqlconn.Open();
                OleDbDataReader sdr = sqlcom.ExecuteReader();

                sqlconn.Close();
            }
        }

        private void toolStripMenuItem_Click(object sender, EventArgs e)
        {
            String tableName = ((ToolStripMenuItem)sender).Text;
            this.tableName = tableName;
            PictureChart(idfFile, this.tableName);

            ShowMaxIn50m(idfFile, this.tableName);

            SetMenuItemCheck(this.tableName);
        }

        private void SetMenuItemCheck(String tsmiName)
        {
            foreach (ToolStripMenuItem tsmi in contextMenuStrip1.Items)
            {
                if (tsmiName == tsmi.Text)
                {
                    tsmi.Checked = true;
                }
                else
                {
                    tsmi.Checked = false;
                }
            }
        }

        //liyang: 选中一个idf，就调用这个函数
        private void AddMenuItem()
        {
            String searchPattern_rmsidf = "*Rms.idf";
            String[] rmsIdfFiles = Directory.GetFiles(citDirectoryPath, searchPattern_rmsidf, SearchOption.TopDirectoryOnly);
            
            foreach (string rmsIdfFile in rmsIdfFiles)
            {
                //if (rmsIdfFile.Contains(citFilePath_section.Split('.')[0]))
                //{
                //    idfFile = rmsIdfFile;
                //    break;
                //}

                if (rmsIdfFile.Contains(comboBoxFileAcclrtn.SelectedItem.ToString()))
                {
                    idfFile = rmsIdfFile;

                    List<String> tableNames = CommonClass.wdp.GetTableNames(rmsIdfFile);

                    // liyang: 获得lineName
                    GetLineName(idfFile, tableNames[0]);

                    tableNames.RemoveAt(0);

                    contextMenuStrip1.Items.Clear();
                    foreach (String tableName in tableNames)
                    {
                        ToolStripMenuItem tsmi = new ToolStripMenuItem(tableName);
                        tsmi.Click += new EventHandler(toolStripMenuItem_Click);
                        contextMenuStrip1.Items.Add(tsmi);
                    }
                    this.tableName = tableNames[0];
                    // liyang: 这个函数用来画左边的teechart 图形： 50米区段大值散点值
                    PictureChart(idfFile, this.tableName);

                    // liyang: 这个函数用来画右边的列表图： 超标值列表
                    ShowMaxIn50m(idfFile, this.tableName);

                    // liyang: 这个只是把上下文菜单中的第一项打上勾
                    SetMenuItemCheck(this.tableName);


                    break;
                }
            }
            //idfFile = Path.Combine(citDirectoryPath, idfFile);

            //List<String> tableNames =  CommonClass.wdp.GetTableNames(idfFile);
            
            //contextMenuStrip1.Items.Clear();
            //foreach (String tableName in tableNames)
            //{
            //    ToolStripMenuItem tsmi = new ToolStripMenuItem(tableName);
            //    tsmi.Click += new EventHandler(toolStripMenuItem_Click);
            //    contextMenuStrip1.Items.Add(tsmi);
            //}
            //this.tableName = tableNames[0];
            //PictureChart(idfFile, this.tableName);

            
            //ShowMaxIn50m(idfFile, this.tableName);

            //SetMenuItemCheck(this.tableName);
        }

        // liyang: 从cit文件的第一个表中，把line name 取出来。
        private void GetLineName(String idfFilePath, String tableName)
        {
            using (OleDbConnection sqlconn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + idfFilePath + ";Persist Security Info=True"))
            {
                String sSQL = "select LineName from " + tableName;
                OleDbCommand sqlcom = new OleDbCommand(sSQL, sqlconn);
                sqlconn.Open();
                OleDbDataReader sdr = sqlcom.ExecuteReader();
                while (sdr.Read())
                {
                    this.lineName = sdr.GetValue(0).ToString();
                }
                sdr.Close();
                sqlconn.Close();
            }
        }

        private SegmentRmsClass GetTableValue(String idfFilePath, String tableName,String condition)
        {
            SegmentRmsClass srCls = new SegmentRmsClass();            

            srCls.tableName = tableName;

            using (OleDbConnection sqlconn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + idfFilePath + ";Persist Security Info=True"))
            {

                String sSQL = "select Id,KiloMeter,Speed,Segment_RMS,Segment_RMS_Peak,valid from " + tableName + condition;
                OleDbCommand sqlcom = new OleDbCommand(sSQL, sqlconn);

                sqlconn.Open();
                OleDbDataReader sdr = sqlcom.ExecuteReader();

                while (sdr.Read())
                {
                    SegmentRmsItemClass segmentRmsItemCls = new SegmentRmsItemClass();

                    segmentRmsItemCls.id = (int.Parse(sdr.GetValue(0).ToString()));
                    segmentRmsItemCls.kiloMeter = (float.Parse(sdr.GetValue(1).ToString()));
                    segmentRmsItemCls.speed = (float.Parse(sdr.GetValue(2).ToString()));
                    segmentRmsItemCls.segmentRms = (float.Parse(sdr.GetValue(3).ToString()));
                    segmentRmsItemCls.segmentRmsPeak = (float.Parse(sdr.GetValue(4).ToString()));
                    segmentRmsItemCls.valid = (int.Parse(sdr.GetValue(5).ToString()));
                    segmentRmsItemCls.channelName = tableName;

                    srCls.segmentRmsItemClsList.Add(segmentRmsItemCls);
                }
                sdr.Close();
                sqlconn.Close();
            }

            return srCls;
        }

        private void comboBoxFileAcclrtn_SelectedIndexChanged(object sender, EventArgs e)
        {
            citFilePath_section = (String)(comboBoxFileAcclrtn.SelectedItem);
            AddMenuItem();
        }

        private void buttonThresholdSet_Click(object sender, EventArgs e)
        {
            ThresholdConfigForm tcForm = new ThresholdConfigForm(iniFilePath);
            DialogResult dr = tcForm.ShowDialog();
            if (dr == DialogResult.Cancel)
            {
                dicList = (List<Dictionary<String, String>>)(tcForm.Tag);
            }

            if (dicList != null && dicList.Count > 0)
            {
                comboBoxThreshold.Items.Clear();
                foreach (Dictionary<String, String> dicc in dicList)
                {
                    comboBoxThreshold.Items.Add(dicc["TrainNo"]);
                }

                comboBoxThreshold.SelectedIndex = 0;

            }

        }

        private void tabPage2_Enter(object sender, EventArgs e)
        {
            StringBuilder sbIniConfig = new StringBuilder();
            StringBuilder sbTrain = new StringBuilder();
            
        }

        private void comboBoxThreshold_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dicList != null && dicList.Count > 0)
            {
                String m_dic = (String)(comboBoxThreshold.SelectedItem);
                foreach (Dictionary<String, String> dicc in dicList)
                {
                    if (dicc["TrainNo"] == m_dic)
                    {
                        dic = dicc;
                    }                    
                }
            }
        }

        private void buttonReShow_Click(object sender, EventArgs e)
        {
            if (reShow)
            {
                reShow = false;
            } 
            else
            {
                reShow = true;
            }
            PictureChart(idfFile, this.tableName);
            //ShowMaxIn50m(idfFile, this.tableName);
        }

        private void buttonExport_Click(object sender, EventArgs e)
        {
            String csvFilePath = null;
            StringBuilder sbExport = new StringBuilder();
            String yes = "是";

            // liyang: 最终导出的csv中，增加是否道岔这一列。
            String head = "里程,速度,阀值,轨道冲击指数,50米区段大值,项目,是否道岔";
            sbExport.AppendLine(head);

            // liyang: GetList() 是取得idf文件中的所有的表的数据
            List<SegmentRmsItemClass> segmentRmsItemClsList = GetList();

            if (segmentRmsItemClsList == null)
            {
                return;
            }

            segmentRmsItemClsList.Sort(CompareByKiloMeter);

            

            for (int i = 0; i < segmentRmsItemClsList.Count; i++)
            {
                string fazhi = textBoxPeakLine.Text; // 曲线或者直线超限阀值，默认为4

                if (segmentRmsItemClsList[i].isDaoCha.Equals(yes))
                    fazhi = textBoxDaoChaPeakLine.Text; //道岔超限阀值,默认为6

                sbExport.AppendFormat("{0},{1},{2},{3},{4},{5},{6}", segmentRmsItemClsList[i].kiloMeter, 
                                                                     segmentRmsItemClsList[i].speed, 
                                                                     fazhi,//textBoxPeakLine.Text, 
                                                                     segmentRmsItemClsList[i].segmentRmsPeak, 
                                                                     segmentRmsItemClsList[i].segmentRms, 
                                                                     segmentRmsItemClsList[i].channelName,
                                                                     segmentRmsItemClsList[i].isDaoCha);
                sbExport.AppendLine();
            }


            String csvFileName = String.Format("{0}.csv", "轴箱加速度超限值");
            csvFilePath = Path.Combine(citDirectoryPath, csvFileName);

            StreamWriter sw = new StreamWriter(csvFilePath, false, Encoding.Default);
            sw.Write(sbExport.ToString());
            sw.Close();

            MessageBox.Show("导出成功！");
        }

        private void buttonDaoChaMileImport_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Multiselect = false;
            fileDialog.Title = "请选择道岔位置表";
            fileDialog.Filter = "所有文件(*.*)|*.*";
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                // liyang: 得到文件的full path
                string filename = fileDialog.FileName;

                // liyang: 解析这个文件，构造DaoChaMileRange对象。修改列表中“是否道岔”这一列的值
                this.daoChaMileRange = new DaoChaMileRange(filename, this.lineName);

                if (this.daoChaMileRange.Valid)
                {
                    ShowDataGridView(this.daoChaMileRange);
                }
                else
                {
                    this.daoChaMileRange = null;
                }

            }
        }
       

        // liyang: 焦点离开时调用。 按回车没用，只能点击它旁边的导出按钮来触发。
        // 这个函数用来调整直线曲线相关的数据。
        private void textBoxPeakLine_TextChanged(object sender, EventArgs e)
        {
            try
            {
                float tmp = float.Parse(textBoxPeakLine.Text);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("请在超限阈值中输入数字！");
                return;
            }
            PictureChart(idfFile, this.tableName);

            //ShowList();
            // liyang: 这里把每一行的颜色做一个调整，比如 变红
            ShowDataGridView();
            
            //ShowMaxIn50m(idfFile, this.tableName);

            //SetMenuItemCheck(this.tableName);
        }

        // liyang: 这个函数用来调整道岔相关的数据。和上面的函数类似的。后期可以把这两个函数合成
        private void textBoxDaoChaPeakLine_TextChanged(object sender, EventArgs e)
        {
            try
            {
                float tmp = float.Parse(textBoxDaoChaPeakLine.Text);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("请在超限阈值中输入数字！");
                return;
            }

            // liyang: 这里把每一行的颜色做一个调整，比如 变红
            ShowDataGridView();

        }

        private void buttonRMS_Click(object sender, EventArgs e)
        {
            isTPI = false;
            buttonRms.Enabled = false;

            buttonTPI.Enabled = true;
        }

        private void buttonTPI_Click(object sender, EventArgs e)
        {
            isTPI = true;
            buttonTPI.Enabled = false;

            buttonRms.Enabled = true;
        }

        #region 读取各个断面文件区段大值中的超标值
        /// <summary>
        /// 读取各个断面文件区段大值中的超标值
        /// </summary>
        /// <returns>超标值列表</returns>
        private List<SegmentRmsItemClass> GetList()
        {
            List<SegmentRmsItemClass> tmp = new List<SegmentRmsItemClass>();

            String searchPattern_rmsidf = "*Rms.idf";
            String[] rmsIdfFiles = Directory.GetFiles(citDirectoryPath, searchPattern_rmsidf, SearchOption.TopDirectoryOnly);

            List<String> tableNames = null;

            if (String.IsNullOrEmpty(textBoxPeakLine.Text))
            {
                MessageBox.Show("请设置直线和曲线超限值！");
                return tmp; 
            }
            if (String.IsNullOrEmpty(textBoxDaoChaPeakLine.Text))
            {
                MessageBox.Show("请设置道岔超限值！");
                return tmp; 
            }
            //liyang: 修改成不加载道岔位置表，也能导出。只根据“曲线或直线超限阀值（默认为4）” 导出 
            //if (null == this.daoChaMileRange)
            //{
            //    MessageBox.Show("请导入道岔位置表！");
            //    return null;
            //}

            String condition = null;
            String strYes = "是";
            String strNo = "否";

            try
            {

                foreach (string rmsIdfFile in rmsIdfFiles)
                {
                    if (rmsIdfFile.Contains(comboBoxFileAcclrtn.SelectedItem.ToString()))
                    {
                        String[] tmpStr = rmsIdfFile.Split('.');

                        tableNames = CommonClass.wdp.GetTableNames(rmsIdfFile);
                        tableNames.RemoveAt(0);

                        foreach (String str in tableNames)
                        {
                            condition = String.Format(" where (valid = 1)", textBoxPeakLine.Text);
                            SegmentRmsClass srCls = GetTableValue(rmsIdfFile, str, condition);

                            
                            
                            

                            for (int i = 0; i < srCls.segmentRmsItemClsList.Count; i++)
                            {
                                bool isDaoCha = false;
                                if (null != this.daoChaMileRange)
                                {// liyang: 加入是否道岔的判断，并置上。
                                    isDaoCha = this.daoChaMileRange.IsDaoChao(srCls.segmentRmsItemClsList[i].kiloMeter);
                                    srCls.segmentRmsItemClsList[i].isDaoCha = isDaoCha ? strYes : strNo;
                                }
                                else
                                {
                                    srCls.segmentRmsItemClsList[i].isDaoCha = "未加载道岔位置表";
                                }

                                // liyang: 加入针对于道岔的峰值的判断。
                                //liyang:是道岔就按照道岔的超限值判断
                                if (isDaoCha)
                                {
                                    if (srCls.segmentRmsItemClsList[i].segmentRmsPeak >= float.Parse(textBoxDaoChaPeakLine.Text))
                                        tmp.Add(srCls.segmentRmsItemClsList[i]);
                                }
                                else //liyang:不是道岔就按照直线曲线的超限值判断
                                {
                                    if (srCls.segmentRmsItemClsList[i].segmentRmsPeak >= float.Parse(textBoxPeakLine.Text))
                                        tmp.Add(srCls.segmentRmsItemClsList[i]);    
                                }
                            }

                            //tmp.AddRange(srCls.segmentRmsItemClsList);
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
           
            return tmp;
        }
        #endregion
        

        #region 按公里排序
        /// <summary>
        /// 从大到小排序
        /// </summary>
        /// <param name="segment1">第一个</param>
        /// <param name="segment2">第二个</param>
        /// <returns></returns>
        public int CompareByKiloMeter(SegmentRmsItemClass segment1,SegmentRmsItemClass segment2)
        {
            if (segment1 == null)
            {
                if (segment2 == null)
                {
                    return 0;
                }

                return 1;
            }

            if (segment2 == null)
            {
                return -1;
            }

            int retval=segment1.kiloMeter.CompareTo(segment2.kiloMeter);
            return retval;
        }
        #endregion

        
    }



    public class SegmentRmsClass
    {
        public String tableName;

        public List<SegmentRmsItemClass> segmentRmsItemClsList = new List<SegmentRmsItemClass>();
    }
    /// <summary>
    /// 超标值列表中的一条数据
    /// </summary>
    public class SegmentRmsItemClass
    {
        /// <summary>
        /// 超标值id
        /// </summary>
        public int id;
        /// <summary>
        /// 超标值的里程
        /// </summary>
        public float kiloMeter;
        /// <summary>
        /// 超标值处的速度
        /// </summary>
        public float speed;
        /// <summary>
        /// 50米区段大值
        /// </summary>
        public float segmentRms;
        /// <summary>
        /// 轨道冲击指数
        /// </summary>
        public float segmentRmsPeak;
        /// <summary>
        /// 是否有效
        /// </summary>
        public int valid;
        /// <summary>
        /// 通道名
        /// </summary>
        public String channelName;
        /// <summary>
        /// 是否是道岔，这个域是实时计算出来的
        /// </summary>
        public String isDaoCha = "否"; // "是"   或者  "否"。
    }
}
