using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Data.OleDb;

namespace InGraph.Forms
{
    public partial class AttachListForm : Form
    {
        AttachAddForm aaf = null;

        int id_case = 0;

        List<AnalyAttachClass> listAAC = null;
        String idfFile = null;
        Dictionary<int, String> dicAttachType = new Dictionary<int, String>();

        public AttachListForm(int caseId,String idfFilePath)
        {
            InitializeComponent();

            InitDicAttachType();

            id_case = caseId;
            idfFile = idfFilePath;

            //aaf = new AttachAddForm(id_case, idfFilePath);
            listAAC = AnalyAttachList(idfFile, id_case);
            InitDataGridView(listAAC);

        }
        public AttachListForm()
        {

        }

        private void InitDicAttachType()
        {
            dicAttachType.Clear();

            dicAttachType.Add(0, "图片");
            dicAttachType.Add(1, "word");
            dicAttachType.Add(2, "pdf");
            dicAttachType.Add(3, "二进制文件");
        }

        public  List<AnalyAttachClass> AnalyAttachList(String sAddFile, int caseId)
        {
            List<AnalyAttachClass> listAAC = new List<AnalyAttachClass>();
            try
            {
                using (OleDbConnection sqlconn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + sAddFile + ";Persist Security Info=True"))
                {
                    sqlconn.Open();

                    string sSql = "select * from AnalyAttach  where Id="+caseId;
                    OleDbCommand sqlcom = new OleDbCommand(sSql, sqlconn);
                    OleDbDataReader oleDBr = sqlcom.ExecuteReader();
                    int columnNum = oleDBr.FieldCount;
                    while (oleDBr.Read())
                    {
                        AnalyAttachClass aac = new AnalyAttachClass();
                        aac.id = int.Parse(oleDBr.GetValue(0).ToString());
                        aac.attachId = int.Parse(oleDBr.GetValue(1).ToString());
                        aac.analyDes = oleDBr.GetValue(2).ToString();
                        aac.attachType = int.Parse(oleDBr.GetValue(3).ToString());
                        aac.attachContent = (byte[])(oleDBr.GetValue(4));
                        aac.attachFileName = oleDBr.GetValue(5).ToString();
                        aac.attachMemo = oleDBr.GetValue(6).ToString();

                        listAAC.Add(aac);
                    }
                    oleDBr.Close();
                    sqlconn.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("附件读取异常:" + ex.Message);
            }
            return listAAC;
        }
        public void InitDataGridView(List<AnalyAttachClass> listAAC)
        {
            
            dataGridView1.Rows.Clear();

            if (listAAC == null || listAAC.Count == 0)
            {
                return;
            }

            for (int i = 0; i < listAAC.Count; i++)
            {
                object[] o = new object[7];
                o[0] = listAAC[i].id;
                o[1] = listAAC[i].attachId;
                o[2] = dicAttachType[listAAC[i].attachType];
                o[3] = listAAC[i].attachFileName;
                o[4] = listAAC[i].analyDes;
                o[5] = "打开";
                o[6] = listAAC[i].attachMemo;


                dataGridView1.Rows.Add(o);
            }
        }

        private void buttonAttachAdd_Click(object sender, EventArgs e)
        {
            //using (AttachAddForm aaf = new AttachAddForm())
            //{
            //    aaf.Show(this);
            //}
            aaf = new AttachAddForm(id_case, idfFile);

            DialogResult dr = aaf.ShowDialog(this);
            if (dr == DialogResult.OK)
            {
                listAAC = AnalyAttachList(idfFile, id_case);
                InitDataGridView(listAAC);
            }
        }

        private void buttonAttachOpen_Click(object sender, EventArgs e)
        {

        }

        private void buttonAttachDel_Click(object sender, EventArgs e)
        {
            int id = (int)(dataGridView1.SelectedRows[0].Cells[1].Value);

            try
            {
                using (OleDbConnection sqlconn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + idfFile + ";Persist Security Info=True"))
                {
                    string sqlCreate = "delete from AnalyAttach where AttachId=" + id;
                    OleDbCommand sqlcom = new OleDbCommand(sqlCreate, sqlconn);
                    sqlconn.Open();
                    sqlcom.ExecuteNonQuery();

                    sqlconn.Close();
                }
                buttonAttachRefresh_Click(sender, e);
            }
            catch
            {

            }
        }

        private void buttonAttachDownload_Click(object sender, EventArgs e)
        {
            DialogResult dr = saveFileDialog1.ShowDialog();
            if (dr == DialogResult.Cancel)
            {
                return;
            }

            String filePath = saveFileDialog1.FileName;
            

            int attachId = (int)(dataGridView1.SelectedRows[0].Cells[1].Value);

            for (int i = 0; i < listAAC.Count;i++ )
            {
                if (listAAC[i].attachId == attachId)
                {
                    File.WriteAllBytes(filePath, listAAC[i].attachContent);
                }
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 5)
            {
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    if (i == e.RowIndex)
                    {
                        //处理二进制字节数组
                        MemoryStream ms = new MemoryStream(listAAC[i].attachContent);
                        Image img = Image.FromStream(ms);

                        Form picture = new Form();
                        PictureBox pb = new PictureBox();
                        pb.Image = img;
                        pb.SizeMode = PictureBoxSizeMode.StretchImage;
                        pb.Dock = DockStyle.Fill;

                        picture.Controls.Add(pb);

                        //屏幕宽度
                        int width = SystemInformation.WorkingArea.Width;
                        //屏幕高度（不包括任务栏）
                        int height = SystemInformation.WorkingArea.Height;

                        picture.Width = width / 3;
                        picture.Height = height / 3;
                        picture.StartPosition = FormStartPosition.CenterScreen;
                        picture.TopLevel = true;
                        picture.Text = listAAC[i].attachFileName;
                        picture.Show(this);
                    }
                }
            }
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
            this.Close();
        }

        private void buttonAttachRefresh_Click(object sender, EventArgs e)
        {
            listAAC = AnalyAttachList(idfFile, id_case);
            InitDataGridView(listAAC);
        }
    }

    #region 数据类--保存附件信息---20141028
    /// <summary>
    /// 数据类--保存附件信息
    /// </summary>
    public class AnalyAttachClass
    {
        public int id;
        public int attachId;
        public String analyDes;
        public int attachType;
        public byte[] attachContent;
        public String attachFileName;
        public String attachMemo;
    }
    #endregion

}
