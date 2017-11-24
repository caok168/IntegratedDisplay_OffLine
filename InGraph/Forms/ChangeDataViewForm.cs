using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Data.OleDb;
using InGraph.Classes;

namespace InGraph.Forms
{
    public partial class ChangeDataViewForm : Form
    {
        DataTable dt_Change = null;
        String sChangeTableName = "ChangeInfo";
        String sIdfPath = null;
        OleDbDataAdapter oledbDataAdapter = null;


        public ChangeDataViewForm()
        {
            InitializeComponent();

            sIdfPath = CommonClass.listDIC[0].sAddFile;

            GetChangeData();
        }


        private void InitDataTableChange(ref DataTable dt, String tableName)
        {
            if (dt == null)
            {
                dt = new DataTable(tableName);
            }
            dt.Columns.Clear();
            dt.Columns.Add("ID", System.Type.GetType("System.Int32"));
            dt.Columns.Add("对比的文件名", System.Type.GetType("System.String"));
            dt.Columns.Add("通道名", System.Type.GetType("System.String"));
            dt.Columns.Add("起始里程", System.Type.GetType("System.String"));
            dt.Columns.Add("结束里程", System.Type.GetType("System.String"));
            dt.Columns.Add("幅值绝对值差", System.Type.GetType("System.String"));
        }

        #region 绑定数据
        /// <summary>
        /// 绑定数据
        /// </summary>
        /// <param name="_DT">内存表</param>
        /// <param name="dgw">绑定的DataGridView</param>
        public void DataSource(DataTable _DT, DataGridView dgw)
        {
            if (dgw.Rows.Count > 0)
            {
                dgw.DataSource = null;
            }
            if (dgw.Columns.Count > 0)
            {
                dgw.Columns.Clear();
            }

            BindingSource source = new BindingSource();
            source.DataSource = _DT;
            foreach (DataColumn col in _DT.Columns)
            {
                DataGridViewTextBoxColumn DC = new DataGridViewTextBoxColumn();
                DC.DataPropertyName = col.ColumnName;
                DC.HeaderText = col.ColumnName;
                DC.Name = col.ColumnName;
                DC.Resizable = DataGridViewTriState.True;
                DC.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

                //DC.Width = 100;
                
                DC.ReadOnly = true;
                dgw.Columns.Add(DC);

            }
            dgw.DataSource = source;

        }
        #endregion



        private void GetChangeData()
        {
            try
            {
                InitDataTableChange(ref dt_Change, sChangeTableName);

                using (OleDbConnection odb = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + sIdfPath + ";Persist Security Info=True"))
                {
                    odb.Open();

                    String sqlText = String.Format("SELECT * FROM {0};", sChangeTableName);

                    oledbDataAdapter = new OleDbDataAdapter(sqlText, odb);
                    oledbDataAdapter.Fill(dt_Change);

                    odb.Close();

                    DataSource(dt_Change, dataGridView1);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.RemoveAt(dataGridView1.CurrentCell.RowIndex);

            using (OleDbConnection odb = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + sIdfPath + ";Persist Security Info=True"))
            {
                odb.Open();

                String sqlText = String.Format("SELECT * FROM {0};", sChangeTableName);

                oledbDataAdapter = new OleDbDataAdapter(sqlText, odb);

                OleDbCommandBuilder oledbCommandBuilder = new OleDbCommandBuilder(oledbDataAdapter);
                oledbDataAdapter.Update(dt_Change);

                MessageBox.Show("删除成功");

                odb.Close();
            }
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1)
            {
                return;
            }

            Boolean bIndex = CommonClass.listDIC[0].bIndex;

            float startMile = float.Parse(dataGridView1[3, e.RowIndex].Value.ToString());
            float endMile = float.Parse(dataGridView1[4, e.RowIndex].Value.ToString());

            if (bIndex)
            {
                long indexStart = CommonClass.wdp.GetNewIndexMeterPositon(CommonClass.listDIC[0].listIC, (long)(startMile * 1000), CommonClass.listDIC[0].iChannelNumber, CommonClass.listDIC[0].sKmInc, 0);
                long indexEnd = CommonClass.wdp.GetNewIndexMeterPositon(CommonClass.listDIC[0].listIC, (long)(endMile * 1000), CommonClass.listDIC[0].iChannelNumber, CommonClass.listDIC[0].sKmInc, 0);

                MainForm.sMainform.MeterFind(indexStart, indexEnd); ;
            }
            else
            {
                MainForm.sMainform.MeterFind(startMile, endMile);
            }

        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
