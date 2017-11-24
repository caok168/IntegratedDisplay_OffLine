using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using InGraph.Classes;
using Microsoft.VisualBasic;
using System.Data.OleDb;
using DataProcess;

namespace InGraph.Forms
{
    /// <summary>
    /// 索引设置---控件类
    /// </summary>
    public partial class IndexForm : Form
    {
        public IndexForm()
        {
            InitializeComponent();
        }

        private void CancelButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #region 把一条索引信息保存在 IndexOri表
        /// <summary>
        /// 把一条索引信息保存在 IndexOri表
        /// </summary>
        /// <param name="lPostion">文件指针</param>
        /// <param name="iKM"></param>
        /// <param name="fMeter"></param>
        /// <param name="sIndexKm">新的索引里程</param>
        public void SetIndexInfo(long lPostion, int iKM, float fMeter, string sIndexKm)
        {
            CommonClass.wdp.InsertLayerIndexInfo(CommonClass.listDIC[0].sAddFile, "", "1", lPostion.ToString(), sIndexKm.ToString());
            ShowList();
        }
        #endregion

        private void IndexForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            MainForm.sMainform.MainGraphicsPictureBox1.Cursor = Cursors.Default;
            MainForm.sMainform.wasIndex = false;
            e.Cancel = true;
            this.Visible = false;

        }

        private void IndexForm_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                ShowList();
            }
        }
        #region 索引列表显示
        /// <summary>
        /// 索引列表显示
        /// </summary>
        private void ShowList()
        {
            //
            List<DataProcess.IndexOriClass> listIOC = CommonClass.wdp.GetLayerIndexInfo(CommonClass.listDIC[0].sAddFile, "0", CommonClass.listDIC[0].sKmInc);
            索引显示表格dataGridView.Rows.Clear();
            for (int i = 0; i < listIOC.Count; i++)
            {
                object[] oPara = new object[4];
                oPara[0] = listIOC[i].iId;
                oPara[1] = listIOC[i].iIndexId;
                oPara[2] = long.Parse(listIOC[i].IndexPoint);
                oPara[3] = float.Parse(listIOC[i].IndexMeter);
                索引显示表格dataGridView.Rows.Add(oPara);
            }
        }
        #endregion

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //删除
            CommonClass.wdp.deleteLayerIndexInfo(CommonClass.listDIC[0].sAddFile);
            //重新保存索引库
            for (int i = 0; i < 索引显示表格dataGridView.Rows.Count; i++)
            {
                CommonClass.wdp.InsertLayerAllIndexInfo(CommonClass.listDIC[0].sAddFile, 
                    索引显示表格dataGridView.Rows[i].Cells[0].Value.ToString(),
                    索引显示表格dataGridView.Rows[i].Cells[1].Value.ToString(),
                    索引显示表格dataGridView.Rows[i].Cells[2].Value.ToString(),
                    索引显示表格dataGridView.Rows[i].Cells[3].Value.ToString());
            }

            //创建计算后的索引库
            List<CDLClass> listCDL = CommonClass.GetCDL();
            CommonClass.wdp.CreateIndexInfo(CommonClass.listDIC[0].sFilePath, CommonClass.listDIC[0].sAddFile,
                "1", listCDL, CommonClass.listDIC[0].sKmInc, CommonClass.listDIC[0].iChannelNumber, CommonClass.listDIC[0].iSmaleRate);
            MessageBox.Show("创建成功！");

            CommonClass.listDIC[0].listIC = CommonClass.wdp.GetDataIndexInfo(CommonClass.listDIC[0].sAddFile);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (索引显示表格dataGridView.SelectedRows.Count == 1)
            {
                CommonClass.wdp.deleteLayerIndexInfo(CommonClass.listDIC[0].sAddFile, 索引显示表格dataGridView.SelectedRows[0].Cells[0].Value.ToString());
                索引显示表格dataGridView.Rows.RemoveAt(索引显示表格dataGridView.SelectedRows[0].Index);
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (button3.Text.Equals(">"))
            {
                button3.Text = "<";
                this.Width = 950;
            }
            else
            {
                button3.Text = ">";
                this.Width = 500;
            }
        }

        //获取新索引
        private void button4_Click(object sender, EventArgs e)
        {
            List<IndexStaClass> listIndex = CommonClass.wdp.GetDataIndexInfo(CommonClass.listDIC[0].sAddFile);
            dataGridView1.Rows.Clear();            
            for (int i = 0; i < listIndex.Count; i++)
            {
                DataGridViewRow dgvr = new DataGridViewRow();
                dgvr.CreateCells(dataGridView1);

                dgvr.Cells[0].Value = listIndex[i].iID;
                dgvr.Cells[1].Value = listIndex[i].sType;
                dgvr.Cells[2].Value = float.Parse(listIndex[i].lContainsMeter);
                dgvr.Cells[3].Value = listIndex[i].lContainsPoint;

                float tmp = (float.Parse(listIndex[i].lContainsMeter) * 1000) / listIndex[i].lContainsPoint;

                dgvr.Cells[4].Value = tmp;
                dgvr.Cells[5].Value = listIndex[i].lStartPoint;
                dgvr.Cells[6].Value = listIndex[i].lEndPoint;
                dgvr.Cells[7].Value = float.Parse(listIndex[i].lStartMeter);
                dgvr.Cells[8].Value = float.Parse(listIndex[i].LEndMeter);

                if (tmp > 0.2510000 || tmp < 0.2490000)
                {
                    dgvr.Cells[4].Style.ForeColor = Color.Red;
                } 
                else
                {
                    dgvr.Cells[4].Style.ForeColor = Color.Black;
                }

                dataGridView1.Rows.Add(dgvr);
            }
        }

        private void IndexForm_Load(object sender, EventArgs e)
        {

        }

        private void 索引显示表格dataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                String value = 索引显示表格dataGridView[2, e.RowIndex].Value.ToString();

                //根据文件指针来定位波形，更精确
                long pos = long.Parse(value);
                MainForm.sMainform.MeterFind(pos);
            }
        }
    }
}
