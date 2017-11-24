using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using InGraph.Classes;

namespace InGraph
{
    /// <summary>
    /// 新增无效信息---控件类
    /// </summary>
    public partial class InvalidDataAddForm : Form
    {
        public InvalidDataAddForm()
        {
            InitializeComponent();
        }

        private void buttonHide_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void InvalidDataAddForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            //
            CommonClass.wdp.InvalidDataInsertInto(CommonClass.listDIC[0].sAddFile,
textBoxStartPoint1.Text, textBoxEndPoint.Text, textBoxStartMile1.Text, textBoxEndMile.Text, comboBox1.SelectedIndex, textBoxMemo.Text, "手工标识");

            this.Hide();
        }

        private void InvalidDataAddForm_Load(object sender, EventArgs e)
        {
            //comboBox1
            comboBox1.Items.Clear();
            using (OleDbConnection sqlconn = new OleDbConnection(CommonClass.sDBConnectionString))
            {
                string sSql = "select * from 无效区段类型 order by i_no";
                OleDbCommand sqlcom = new OleDbCommand(sSql, sqlconn);
                sqlconn.Open();
                OleDbDataReader oledr = sqlcom.ExecuteReader();
                while (oledr.Read())
                {
                    comboBox1.Items.Add(oledr[1].ToString());
                }
                oledr.Close();
                sqlconn.Close();
            }
            comboBox1.SelectedIndex = 0;
        }
    }
}
