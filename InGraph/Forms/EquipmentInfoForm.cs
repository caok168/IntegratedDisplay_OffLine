using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using InGraph.Classes;
using System.Data.OleDb;

namespace InGraph.Forms
{
    /// <summary>
    /// 台账信息显示---控件类
    /// </summary>
    public partial class EquipmentInfoForm : Form
    {
        public EquipmentInfoForm()
        {
            InitializeComponent();
        }

        private void EquipmentInfoForm_Load(object sender, EventArgs e)
        {
            EquipmentInfoForm_Resize(sender, e);
            DataTypeToolStripComboBox1.Items.Clear();
            for (int i = 0; i < CommonClass.listPWMISTable.Count;i++ )
            {
                DataTypeToolStripComboBox1.Items.Add(CommonClass.listPWMISTable[i].name);
            }
            if (DataTypeToolStripComboBox1.Items.Count > 0)
            {
                DataTypeToolStripComboBox1.SelectedIndex = 0;
            }
            toolStripComboBox1.SelectedIndex = 0;
        }

        private void EquipmentInfoForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        private void EquipmentInfoForm_Resize(object sender, EventArgs e)
        {
            dataGridView1.Left = 0;
            dataGridView1.Top = toolStrip1.Height;
            dataGridView1.Width = this.ClientSize.Width;
            dataGridView1.Height = this.ClientSize.Height-toolStrip1.Height;
        }

        private void EquipmentInfoForm_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {
 
            }
        }

        private void GetData(string tablename,string sSql,string sDisplayField)
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();
            try
            {
                using (OleDbConnection sqlconn = new OleDbConnection(CommonClass.sDBConnectionString))
                {
                    OleDbCommand sqlcom = new OleDbCommand("select " + sDisplayField + " from " + tablename + " where 线编号='" +
                        CommonClass.listDIC[0].sLineCode +
                        "' and 行别='" + CommonClass.listDIC[0].sDir + "' " + sSql, sqlconn);

                    sqlconn.Open();
                    OleDbDataReader oddr = sqlcom.ExecuteReader();
                    for (int i = 0; i < oddr.VisibleFieldCount; i++)
                    {
                        dataGridView1.Columns.Add(oddr.GetName(i), oddr.GetName(i));
                    }
                    while (oddr.Read())
                    {
                        object[] o = new object[oddr.VisibleFieldCount];
                        oddr.GetValues(o);
                        dataGridView1.Rows.Add(o);
                    }
                    sqlconn.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            //CommonClass.listDIC[0].sLineCode

        }

        public void SetData()
        {
            GetData(CommonClass.listPWMISTable[DataTypeToolStripComboBox1.SelectedIndex].table_name, GetSQL(), CommonClass.listPWMISTable[DataTypeToolStripComboBox1.SelectedIndex].sDisplayField);
        }

        private void DataTypeComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetData(CommonClass.listPWMISTable[DataTypeToolStripComboBox1.SelectedIndex].table_name, GetSQL(), CommonClass.listPWMISTable[DataTypeToolStripComboBox1.SelectedIndex].sDisplayField);
        }

        private void QueryToolStripButton1_Click(object sender, EventArgs e)
        {

            GetData(CommonClass.listPWMISTable[DataTypeToolStripComboBox1.SelectedIndex].table_name, GetSQL(), CommonClass.listPWMISTable[DataTypeToolStripComboBox1.SelectedIndex].sDisplayField);

        }
        private string GetSQL()
        {
            string sSql = "";
            try
            {
                if (CommonClass.listPWMISTable[DataTypeToolStripComboBox1.SelectedIndex].startMileage.Length > 0)
                {
                    sSql += (" and " + CommonClass.listPWMISTable[DataTypeToolStripComboBox1.SelectedIndex].startMileage + " >= " + StartMeterToolStripTextBox1.Text);
                }
                if (CommonClass.listPWMISTable[DataTypeToolStripComboBox1.SelectedIndex].endMileage.Length > 0)
                {
                    sSql += (" and " + CommonClass.listPWMISTable[DataTypeToolStripComboBox1.SelectedIndex].endMileage + " <= " + EndMeterToolStripTextBox1.Text);
                }
                else
                {
                    sSql += (" and " + CommonClass.listPWMISTable[DataTypeToolStripComboBox1.SelectedIndex].startMileage + " <= " + EndMeterToolStripTextBox1.Text);
                }
                sSql += " order by " + CommonClass.listPWMISTable[DataTypeToolStripComboBox1.SelectedIndex].startMileage;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return sSql;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
           
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 2 && e.RowIndex > -1 && e.ColumnIndex>-1)
            {
                CommonClass.sLastSelectText = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
            }
        }
    }
}
