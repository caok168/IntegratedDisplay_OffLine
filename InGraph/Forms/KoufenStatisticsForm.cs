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

namespace InGraph.Forms
{
    public partial class KoufenStatisticsForm : Form
    {
        protected internal float fSPointMile;  //起点公里标
        protected internal long iSPointXPos;  //起点文件指针
        protected internal float fEPointMile; //终点公里标
        protected internal long iEPointXPos;  //终点文件指针
        private int severitySum = 0;//总扣分
        protected internal string iicFilePath = null;
        private string citFilePath = null;


        public KoufenStatisticsForm()
        {
            InitializeComponent();            
        }

        protected internal void InitTextbox()
        {
            textBoxStartMile.Text = fSPointMile.ToString();
            textBoxEndMile.Text = fEPointMile.ToString();

            citFilePath = CommonClass.listDIC[0].sFilePath;

            if (iicFilePath != null)
            {
                textBox1.Text = iicFilePath;
            }
        }


        protected internal void InitDataGridview()
        {

            int columnIndex = ColumnCalItems.Index;

            dataGridView_koufen.Rows.Clear();

            DataGridViewRow dgvr = new DataGridViewRow();
            dgvr.CreateCells(dataGridView_koufen);
            dgvr.Cells[columnIndex].Value = "一级";
            dataGridView_koufen.Rows.Add(dgvr);

            dgvr = new DataGridViewRow();
            dgvr.CreateCells(dataGridView_koufen);
            dgvr.Cells[columnIndex].Value = "二级";
            dataGridView_koufen.Rows.Add(dgvr);

            dgvr = new DataGridViewRow();
            dgvr.CreateCells(dataGridView_koufen);
            dgvr.Cells[columnIndex].Value = "三级";
            dataGridView_koufen.Rows.Add(dgvr);

            dgvr = new DataGridViewRow();
            dgvr.CreateCells(dataGridView_koufen);
            dgvr.Cells[columnIndex].Value = "四级";
            dataGridView_koufen.Rows.Add(dgvr);

            dgvr = new DataGridViewRow();
            dgvr.CreateCells(dataGridView_koufen);
            dgvr.Cells[columnIndex].Value = "总扣分";
            dataGridView_koufen.Rows.Add(dgvr);

            foreach (DataGridViewColumn dgvc in dataGridView_koufen.Columns)
            {
                dgvc.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

        }


        private void buttonExportCsv_koufen_Click(object sender, EventArgs e)
        {
            String csvFileName = String.Format("扣分统计_{0}_{1}.csv", fSPointMile, fEPointMile);
            String csvFilePath = Path.Combine(Path.GetDirectoryName(citFilePath), csvFileName);

            StringBuilder sb = new StringBuilder();

            if (dataGridView_koufen.Rows.Count == 0)
            {
                return;
            }

            for (int i = 0; i < dataGridView_koufen.ColumnCount; i++)
            {
                sb.AppendFormat("{0},", dataGridView_koufen.Columns[i].HeaderText);
            }
            sb.AppendLine();

            foreach (DataGridViewRow dgvr in dataGridView_koufen.Rows)
            {
                for (int i = 0; i < dataGridView_koufen.ColumnCount; i++)
                {
                    if (dgvr.Cells[i].Value == null)
                    {
                        return;
                    } 
                    else
                    {
                        sb.AppendFormat("{0},", dgvr.Cells[i].Value.ToString());
                    }
                    
                }
                sb.AppendLine();
            }

            File.WriteAllText(csvFilePath, sb.ToString(), Encoding.Default);

            MessageBox.Show("导出成功！");
        }

        private void DataProcess_koufen(String columnName, String channelNameEnArray, String channelNameCh)
        {
            Boolean isHasFix = IsHasFixTable(iicFilePath);
            if (!isHasFix)
            {
                //创建两张fix表，然后在窗口关闭时，删掉;同时需要在窗体标题给予提示：iic未修正
                CommonClass.wdp.CreateIICTable(iicFilePath);
                //把原始表里的tqi拷贝到fix表里
                CommonClass.wdp.TQICopy(iicFilePath, CommonClass.listDIC[0].sKmInc, CommonClass.listDIC[0].listIDC);
            }

            int value_koufen = CommonClass.cdp.GetDefectNum(iicFilePath, channelNameEnArray, fSPointMile, fEPointMile, 1);
            dataGridView_koufen.Rows[0].Cells[columnName].Value = value_koufen;//一级扣分

            value_koufen = CommonClass.cdp.GetDefectNum(iicFilePath, channelNameEnArray, fSPointMile, fEPointMile, 2);
            dataGridView_koufen.Rows[1].Cells[columnName].Value = value_koufen;//二级扣分

            value_koufen = CommonClass.cdp.GetDefectNum(iicFilePath, channelNameEnArray, fSPointMile, fEPointMile, 3);
            dataGridView_koufen.Rows[2].Cells[columnName].Value = value_koufen;//三级扣分

            value_koufen = CommonClass.cdp.GetDefectNum(iicFilePath, channelNameEnArray, fSPointMile, fEPointMile, 4);
            dataGridView_koufen.Rows[3].Cells[columnName].Value = value_koufen;//四级扣分

            value_koufen = CommonClass.cdp.GetSeverityValue(iicFilePath, channelNameEnArray, fSPointMile, fEPointMile);
            dataGridView_koufen.Rows[4].Cells[columnName].Value = value_koufen;//扣分


            severitySum += value_koufen;
        }

        #region 判断是否含有fix表
        /// <summary>
        /// 判断是否含有fix表
        /// </summary>
        /// <param name="mIICFilePath"></param>
        /// <returns></returns>
        private Boolean IsHasFixTable(String mIICFilePath)
        {
            Boolean isHasFixTalbe = false;

            List<String> tableNames = CommonClass.wdp.GetTableNames(mIICFilePath);

            foreach (String tableName in tableNames)
            {
                if (tableName.Contains("fix"))
                {
                    isHasFixTalbe = true;
                    break;
                }
            }

            return isHasFixTalbe;
        }
        #endregion

        private void AddDataToDataGridview_koufen()
        {
            severitySum = 0;
            //高低
            DataProcess_koufen(Column_Prof_kf.Name, "L SURFACE,R SURFACE", "高低_中波");

            //轨向
            DataProcess_koufen(Column_Align_kf.Name, "L ALIGNMENT,R ALIGNMENT", "轨向_中波");

            //轨距
            DataProcess_koufen(Column_GAUGE_kf.Name, "WDGA,TGTGA", "轨距");

            //水平
            DataProcess_koufen(Column_CrossLevel_kf.Name, "CROSSLEVEL", "水平");

            //三角坑
            DataProcess_koufen(Column_Twist_kf.Name, "TWIST", "三角坑");

            //水加
            DataProcess_koufen(Column_LAT_ACCEL_kf.Name, "LAT ACCEL","水加");

            //垂加
            DataProcess_koufen(Column_VERT_ACCEL_kf.Name, "VERT_ACCEL", "垂加");

            //70米高低
            DataProcess_koufen(Column_Prof_70M_kf.Name, "L SURFACE 70M,R SURFACE 70M", "70米高低");

            //70米轨向
            DataProcess_koufen(Column_Align_70M_kf.Name, "L ALIGNMENT 70M,R ALIGNMENT 70M", "70米轨向");

            //曲率变化率
            DataProcess_koufen(Column_CURVATURE_RATE_kf.Name, "CURVATURE RATE", "曲率变化率");

            //轨距变化率
            DataProcess_koufen(Column_GAUGE_RATE_kf.Name, "GAUGE RATE", "轨距变化率");

            //横加变化率
            DataProcess_koufen(Column_LAT_ACCEL_RATE_kf.Name, "LAT ACCEL RATE", "横加变化率");

            //复合不平顺
            DataProcess_koufen(Column_IRREGULAR_kf.Name, "L_IRREGULAR,R_IRREGULAR", "复合不平顺");

            //带通车体横加
            DataProcess_koufen(Column_LATACCEL_NOCUR_kf.Name, "LATACCEL_NOCUR", "带通车体横加");

            labelSeveritySum.Text = severitySum.ToString();

        }

        private void buttonSelectIIC_Click(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = Path.GetDirectoryName(citFilePath);

            DialogResult dr = openFileDialog1.ShowDialog();
            if (dr == DialogResult.OK || dr == DialogResult.Yes)
            {
                iicFilePath = openFileDialog1.FileName;
                textBox1.Text = iicFilePath;
            }
        }

        private void buttonOK_koufen_Click(object sender, EventArgs e)
        {
            if (iicFilePath == null)
            {
                return;
            }
            AddDataToDataGridview_koufen();
        }
    }
}
