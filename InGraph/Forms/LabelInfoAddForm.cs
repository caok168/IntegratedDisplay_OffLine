using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using InGraph.Classes;
using System.IO;
using DataProcess;

namespace InGraph.Forms
{
    public partial class LabelInfoAddForm : Form
    {
        string sFunSPointXPos="";
        public LabelInfoAddForm()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            using (OleDbConnection sqlconn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + CommonClass.listDIC[0].sAddFile + ";Persist Security Info=True"))
            {
                string sSql = "select max(id) from LabelInfo";
                OleDbCommand sqlcom = new OleDbCommand(sSql, sqlconn);
                sqlconn.Open();
                object obj = sqlcom.ExecuteScalar();
                int iId;
                if (obj.Equals(null) || (obj == DBNull.Value))
                {
                    iId = 1;
                }
                else
                {
                    iId = (int)sqlcom.ExecuteScalar() + 1;
                }
                sSql = "insert into LabelInfo values(" + iId.ToString() + ",'" + sFunSPointXPos.ToString() + "','" + textBox3.Text +"','"+textBox2.Text+"','"+DateTime.Now.Date.ToShortDateString()+ "')";
                sqlcom.CommandText = sSql;
                sqlcom.ExecuteNonQuery();
                sqlconn.Close();
            }
            CommonClass.listDIC[0].listLIC = CommonClass.wdp.GetDataLabelInfo(CommonClass.listDIC[0].sAddFile);
            button2_Click(sender, e);
        }

        private void LabelInfoAddForm_Load(object sender, EventArgs e)
        {
            sFunSPointXPos = this.Tag.ToString();
            textBox1.Text = sFunSPointXPos;

            FileStream fs = new FileStream(CommonClass.listDIC[0].sFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            BinaryReader br = new BinaryReader(fs);

            int gjtds = CommonClass.listDIC[0].listCC.Count;
            byte[] b = new byte[gjtds * 2];

            int i1 = 0, i2 = 2;


            br.BaseStream.Position = int.Parse(sFunSPointXPos);
            b = br.ReadBytes(gjtds * 2);
            if (CommonClass.listDIC[0].bEncrypt)
            {
                b = WaveformDataProcess.ByteXORByte(b);
            }

            short gongli = BitConverter.ToInt16(b, i1);
            short mi1 = BitConverter.ToInt16(b, i2);
            textBox3.Text = ((gongli * 100000 + (mi1 / (float)CommonClass.listDIC[0].iSmaleRate * 100))/1000).ToString();

            br.Close();
            fs.Close();

            textBox1.Text = sFunSPointXPos;
        }
    }
}
