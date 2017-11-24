using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using InGraph.Classes;
using DataProcess;
using System.Data.OleDb;
using System.IO;

namespace InGraph.Forms
{
    public partial class LabelInfoForm : Form
    {
        public LabelInfoForm()
        {
            InitializeComponent();
        }

        private void LabelInfoForm_Load(object sender, EventArgs e)
        {
            int gjtds = CommonClass.listDIC[0].listCC.Count;
            byte[] b = new byte[gjtds * 2];


            for (int i = 0; i < CommonClass.listDIC[0].listLIC.Count; i++)
            {

                DataGridViewRow dgvr = new DataGridViewRow();
                dgvr.CreateCells(dataGridView1);

                dgvr.Cells[0].Value = CommonClass.listDIC[0].listLIC[i].iID;
                dgvr.Cells[1].Value = CommonClass.listDIC[0].listLIC[i].sMileIndex;

                dgvr.Cells[2].Value = CommonClass.listDIC[0].listLIC[i].sMile;

                dgvr.Cells[3].Value=CommonClass.listDIC[0].listLIC[i].sMemoText;
                dgvr.Cells[4].Value = CommonClass.listDIC[0].listLIC[i].logDate;

                dataGridView1.Rows.Add(dgvr);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1)
            {
                return;
            }

            String value = dataGridView1[1, e.RowIndex].Value.ToString();

            //根据文件指针来定位波形，更精确
            long pos = long.Parse(value);
            MainForm.sMainform.MeterFind(pos);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                //using (OleDbConnection odb = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + CommonClass.listDIC[0].sFilePath + ";Persist Security Info=True"))
                //{
                //    odb.Open();

                //    foreach (DataGridViewRow dgwr in dataGridView1.SelectedRows)
                //    {
                //        dgwr.Cells[2].Value = "无效";

                //        float gongli = float.Parse(dgwr.Cells[0].Value.ToString());
                //        int fromPost = (int)gongli;
                //        int fromMinor = (int)(gongli * 1000) % 1000;
                //        fromMinor = ((fromMinor % 100 == 0) ? fromMinor : fromMinor + 1);

                //        String cmdText = String.Format("update {0} set valid = 0 where FromPost = {1} and FromMinor = {2}", "LableInfo", fromPost, fromMinor);
                //        OleDbCommand sqlcom = new OleDbCommand(cmdText, odb);
                //        sqlcom.ExecuteNonQuery();
                //    }            

                //    odb.Close();
                //}
                Application.DoEvents();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
