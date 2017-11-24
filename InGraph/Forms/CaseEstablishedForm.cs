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
using System.IO;

namespace InGraph.Forms
{
    public partial class CaseEstablishedForm : Form
    {
        int id = 0;//案例id

        public CaseEstablishedForm()
        {
            InitializeComponent();
        }

        private void CreatTableAnalysisInfo(String idfFilePath)
        {
            List<String> tableNames = CommonClass.wdp.GetTableNames(idfFilePath);
            if (!tableNames.Contains("AnalysisAttach"))
            {
                #region 创建AnalysisInfo
                try
                {
                    using (OleDbConnection sqlconn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + idfFilePath + ";Persist Security Info=True"))
                    {
                        string sqlCreate = "CREATE TABLE AnalysisInfo (" +
                            "Id integer primary key," +
                            "LineCode varchar(255) NULL," +
                            "LineName varchar(255) NULL," +
                            "LineDir varchar(255) NULL," +
                            "DetectDate date NULL," +
                            "SecFlag varchar(255) NULL," +
                            "StartPoint varchar(255) NULL," +
                            "StartMile varchar(255) NULL," +
                            "EndPoint varchar(255) NULL," +
                            "EndMile varchar(255) NULL," +
                            "AnalySisType varchar(255) NULL," +
                            "ManageType varchar(255) NULL," +
                            "ImportFlag varchar(255) NULL," +
                            "Opdate date NULL," +
                            "CITName varchar(255) NULL," +
                            "MemoInfo varchar(255) NULL);";
                        OleDbCommand sqlcom = new OleDbCommand(sqlCreate, sqlconn);
                        sqlconn.Open();
                        sqlcom.ExecuteNonQuery();
                        sqlconn.Close();
                    }
                }
                catch
                {

                }
                #endregion
            }
        }

        public void AnalysisInfoInsertInto(String sFilePath, String startMile, String endMile, int analysisType, int manageType, int secFlag, String analysisDes, String memoInfo, String attachPath)
        {
            //FileStream fileStream = new FileStream(attachPath, FileMode.Open);
            //byte[] bFile = new byte[fileStream.Length];//分配数组大小
            //fileStream.Read(bFile, 0, (int)fileStream.Length);//将文件内容读进数组
            //fileStream.Close();//关闭文件对象

            CreatTableAnalysisInfo(sFilePath);

            #region 获取案例id
  
            try
            {
                using (OleDbConnection sqlconn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + sFilePath + ";Persist Security Info=True"))
                {
                    sqlconn.Open();

                    String sSql = "select max(Id) from AnalysisInfo";
                    OleDbCommand sqlcom = new OleDbCommand(sSql, sqlconn);
                    OleDbDataReader oledbReader = sqlcom.ExecuteReader();
                    Boolean isNull = oledbReader.HasRows;//是否是第一条记录，第一条记录id为1；
                    if (isNull == false)
                    {
                        id = 1;
                    }
                    else
                    {
                        while (oledbReader.Read())
                        {
                            if (String.IsNullOrEmpty(oledbReader.GetValue(0).ToString()))
                            {
                                id = 1;
                            }
                            else
                            {
                                id = int.Parse(oledbReader.GetValue(0).ToString()) + 1;
                            }
                        }
                    }

                    sqlconn.Close();
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("获取案例id异常：" + ex.Message);
            }
            #endregion

            try
            {
                using (OleDbConnection sqlconn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + sFilePath + ";Persist Security Info=True"))
                {
                    sqlconn.Open();
                    OleDbCommand com = sqlconn.CreateCommand();
                    com.CommandText = "insert into AnalysisInfo values(@Id,@LineCode,@LineName,@LineDir,@DetectDate,@SecFlag,@StartPoint,@StartMile,@EndPoint,"
                    + "@EndMile,@AnalySisType,@ManageType,@ImportFlag,@Opdate,@CITName,@MemoInfo)";

                    com.Parameters.AddWithValue("@Id", id);
                    com.Parameters.AddWithValue("@LineCode", CommonClass.listDIC[0].sLineCode);
                    com.Parameters.AddWithValue("@LineName", CommonClass.listDIC[0].sTrackName);
                    com.Parameters.AddWithValue("@LineDir", CommonClass.listDIC[0].sDir);
                    com.Parameters.AddWithValue("@DetectDate", CommonClass.listDIC[0].sDate);
                    com.Parameters.AddWithValue("@SecFlag", secFlag.ToString());
                    com.Parameters.AddWithValue("@StartPoint", "");
                    com.Parameters.AddWithValue("@StartMile", startMile);
                    com.Parameters.AddWithValue("@EndPoint", "");
                    com.Parameters.AddWithValue("@EndMile", endMile);
                    com.Parameters.AddWithValue("@AnalySisType", analysisType.ToString());
                    com.Parameters.AddWithValue("@ManageType", manageType.ToString());
                    com.Parameters.AddWithValue("@ImportFlag", "1");
                    com.Parameters.AddWithValue("@Opdate", DateTime.Now.Date.ToShortDateString());
                    com.Parameters.AddWithValue("@CITName",Path.GetFileName(CommonClass.listDIC[0].sFilePath));
                    //com.Parameters.AddWithValue("@AttachDes", analysisDes);
                    //com.Parameters.AddWithValue("@AttachType", "");
                    //com.Parameters.AddWithValue("@AttachContent", bFile);
                    //com.Parameters.AddWithValue("@AttachFileName", "");
                    com.Parameters.AddWithValue("@MemoInfo", memoInfo);


                    com.ExecuteNonQuery();
                    sqlconn.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("新增案例异常:" + ex.Message);
            }
        }


        private void buttonAddAttach_Click(object sender, EventArgs e)
        {
            DialogResult dr=openFileDialog1.ShowDialog();
            if (dr==DialogResult.OK)
            {
                //textBoxAttachFile.Text = openFileDialog1.FileName;
            }
        }

        private void buttonSaveCase_Click(object sender, EventArgs e)
        {
            AnalysisInfoInsertInto(CommonClass.listDIC[0].sAddFile, textBoxStartMile.Text,
                textBoxEndMile.Text, comboBoxAnalysisType.SelectedIndex, comboBoxManageType.SelectedIndex,
                comboBoxSecFlag.SelectedIndex, "", richTextBoxMemoInfo.Text, "");

            labelAttachAdd.Enabled = true;
        }

        private AttachAddForm aaf = null;
        private void labelAttachAdd_Click(object sender, EventArgs e)
        {
            //using (AttachListForm alf = new AttachListForm())
            //{
            //    alf.Show(this);
            //}

            AttachListForm alf = new AttachListForm(id, CommonClass.listDIC[0].sAddFile);
            alf.Show(this);

            //aaf = new AttachAddForm(id_case, idfFile);

            //DialogResult dr = aaf.ShowDialog(this);
            //if (dr == DialogResult.OK)
            //{

            //}
        }
    }
}
