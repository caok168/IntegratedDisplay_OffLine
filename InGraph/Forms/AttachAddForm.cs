using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using System.IO;
using InGraph.Classes;

namespace InGraph.Forms
{
    public partial class AttachAddForm : Form
    {
        int id_case = 0;
        String attachFilePath = null;
        String idfFilePath = null;
        String attachFileName = null;

        public AttachAddForm(int caseId,String idfFilePath)
        {
            InitializeComponent();

            id_case = caseId;
            comboBox1.SelectedIndex = 0;
            this.idfFilePath = idfFilePath;

            this.DialogResult = DialogResult.OK;
        }

        private Boolean Invalidate()
        {
            if (comboBox1.SelectedIndex == -1)
            {
                MessageBox.Show("请选择附件类型！");
                return false;
            } 

            if (attachFilePath == null || !File.Exists(attachFilePath))
            {
                MessageBox.Show("请选择附件！");
                return false;
            }

            return true;
        }

        private void CreateTableAnalyAttach(String idfFilePath)
        {
            List<String> tableNames = CommonClass.wdp.GetTableNames(idfFilePath);
            if (!tableNames.Contains("AnalyAttach"))
            {
                try
                {
                    using (OleDbConnection sqlconn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + idfFilePath + ";Persist Security Info=True"))
                    {
                        string sqlCreate = "CREATE TABLE AnalyAttach (" +
                            "Id integer NULL," +
                            "AttachId integer primary key," +
                            "AnalysisDes varchar(255) NULL," +
                            "AttachType integer NULL," +
                            "AttachContent image NULL," +
                            "AttachFileName varchar(255) NULL," +
                            "AttachMemo varchar(255) NULL);";
                        OleDbCommand sqlcom = new OleDbCommand(sqlCreate, sqlconn);
                        sqlconn.Open();
                        sqlcom.ExecuteNonQuery();
                        sqlconn.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        public void AnalysisAttachInsertInto(String sFilePath,String attachType,String analysisDes, String memoInfo, String attachPath)
        {
            FileStream fileStream = new FileStream(attachPath, FileMode.Open);
            byte[] bFile = new byte[fileStream.Length];//分配数组大小
            fileStream.Read(bFile, 0, (int)fileStream.Length);//将文件内容读进数组
            fileStream.Close();//关闭文件对象

            CreateTableAnalyAttach(sFilePath);


            #region 获取案例id
            int attachId = 0;
            try
            {
                using (OleDbConnection sqlconn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + sFilePath + ";Persist Security Info=True"))
                {
                    sqlconn.Open();

                    String sSql = "select max(AttachId) from AnalyAttach";
                    OleDbCommand sqlcom = new OleDbCommand(sSql, sqlconn);
                    OleDbDataReader oledbReader = sqlcom.ExecuteReader();
                    Boolean isNull = oledbReader.HasRows;//是否是第一条记录，第一条记录id为1；
                    if (isNull == false)
                    {
                        attachId = 1;
                    }
                    else
                    {
                        while (oledbReader.Read())
                        {
                            if (String.IsNullOrEmpty(oledbReader.GetValue(0).ToString()))
                            {
                                attachId = 1;
                            }
                            else
                            {
                                attachId = int.Parse(oledbReader.GetValue(0).ToString()) + 1;
                            }
                        }
                    }

                    sqlconn.Close();
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("获取附件id异常：" + ex.Message);
            }
            #endregion

            try
            {
                using (OleDbConnection sqlconn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + sFilePath + ";Persist Security Info=True"))
                {
                    sqlconn.Open();
                    OleDbCommand com = sqlconn.CreateCommand();
                    com.CommandText = "insert into AnalyAttach values(@Id,@AttachId,"
                    + "@AttachDes,@AttachType,@AttachContent,@AttachFileName,@AttachMemo)";

                    com.Parameters.AddWithValue("@Id", id_case);
                    com.Parameters.AddWithValue("@AttachId", attachId);
                    com.Parameters.AddWithValue("@AttachDes", analysisDes);
                    com.Parameters.AddWithValue("@AttachType", comboBox1.SelectedIndex);
                    com.Parameters.AddWithValue("@AttachContent", bFile);
                    com.Parameters.AddWithValue("@AttachFileName", attachFileName);
                    com.Parameters.AddWithValue("@AttachMemo", memoInfo);


                    com.ExecuteNonQuery();
                    sqlconn.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("新增附件异常:" + ex.Message);
            }
        }

        private void buttonExplore_Click(object sender, EventArgs e)
        {
            DialogResult dr = openFileDialog1.ShowDialog();
            if (dr==DialogResult.OK)
            {
                textBoxAttachFilePath.Text = openFileDialog1.FileName;
                attachFilePath = textBoxAttachFilePath.Text;
                attachFileName = Path.GetFileName(attachFilePath);
            }            ;
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            Boolean result = Invalidate();
            if (result == false)
            {
                return;
            }

            AnalysisAttachInsertInto(idfFilePath, comboBox1.SelectedIndex.ToString(), richTextBoxAttachDescribe.Text, "", attachFilePath);

            this.Dispose();
            this.Close();

            this.DialogResult = DialogResult.OK;
        }


        
        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
            this.Close();

            this.DialogResult = DialogResult.OK;
        }

        private void AttachAddForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }
    }
}
