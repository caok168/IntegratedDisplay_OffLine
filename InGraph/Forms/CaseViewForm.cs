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
using DataProcess;

namespace InGraph.Forms
{
    public partial class CaseViewForm : Form
    {
        List<AnalysisInfoClass> listAIC;

        int id = 0;//案例id

        Dictionary<int, String> dicAnalysisType = new Dictionary<int, String>();
        Dictionary<int, String> dicManageType = new Dictionary<int, String>();
        Dictionary<int, String> dicSecFlag = new Dictionary<int, String>();

        public CaseViewForm()
        {
            InitializeComponent();

            InitDicAnalysisType();
            InitDicManageType();
            InitSecFlag();

            GetAnalysisInfoData();


        }

        public void GetAnalysisInfoData()
        {
            dataGridView1.Rows.Clear();
            try
            {
                if (CommonClass.listDIC[0].iAppMode == 0)
                {
                    CommonClass.listDIC[0].listAIC = CommonClass.wdp.AnalysisInfoList(CommonClass.listDIC[0].sAddFile);
                }
                else
                {

                }
                listAIC = CommonClass.listDIC[0].listAIC;

                if (listAIC == null || listAIC.Count == 0)
                {
                    return;
                }

                for (int i = 0; i < listAIC.Count; i++)
                {
                    object[] o = new object[8];
                    o[0] = listAIC[i].id;
                    o[1] = listAIC[i].startMile;
                    o[2] = listAIC[i].endMile;
                    o[3] = dicAnalysisType[listAIC[i].analysisType];
                    o[4] = dicManageType[listAIC[i].manageType];

                    //sqlcom.CommandText = "select i_name from 无效区段类型 where i_no=" + listAIC[i].iType.ToString();

                    o[5] = listAIC[i].secFlag;

                    //o[6] = listAIC[i].attachDes;

                    //处理二进制字节数组
                    //MemoryStream ms = new MemoryStream(listAIC[i].attachContent);
                    //Image img = Image.FromStream(ms);
                    //o[7] = img;
                    o[6] = listAIC[i].attachNum;
                    o[7] = listAIC[i].memoInfo;
                    dataGridView1.Rows.Add(o);
                }
                //using (OleDbConnection sqlconn = new OleDbConnection(CommonClass.listDIC[0].sAddFile))
                //{
                //    OleDbCommand sqlcom = new OleDbCommand("", sqlconn);
                //    sqlconn.Open();
                //    for (int i = 0; i < listAIC.Count; i++)
                //    {
                //        object[] o = new object[9];
                //        o[0] = listAIC[i].id;
                //        o[1] = long.Parse(listAIC[i].startMile);
                //        o[2] = long.Parse(listAIC[i].endMile);
                //        o[3] = float.Parse(listAIC[i].analysisType);
                //        o[4] = float.Parse(listAIC[i].manageType);

                //        //sqlcom.CommandText = "select i_name from 无效区段类型 where i_no=" + listAIC[i].iType.ToString();

                //        o[5] = listAIC[i].secFlag;

                //        o[6] = listAIC[i].attachDes;

                //        //处理二进制字节数组
                //        MemoryStream ms = new MemoryStream(listAIC[i].attachContent);
                //        Image img = Image.FromStream(ms);
                //        o[7] = img;
                //        o[8] = listAIC[i].memoInfo;
                //        dataGridView1.Rows.Add(o);
                //    }
                //    sqlconn.Close();
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show("读取案例出错：" + ex.Message);
            }
        }

        private void buttonDelCase_Click(object sender, EventArgs e)
        {
            id = (int)(dataGridView1.SelectedRows[0].Cells[0].Value);

            CommonClass.wdp.DeleteAnalysisInfo(CommonClass.listDIC[0].sAddFile,id);
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
            this.Close();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 6)
            {
                id = (int)(dataGridView1.SelectedRows[0].Cells[0].Value);
                AttachListForm alf = new AttachListForm(id, CommonClass.listDIC[0].sAddFile);
                alf.Show(this);
            }

        }

        private void buttonCaseRefresh_Click(object sender, EventArgs e)
        {
            GetAnalysisInfoData();
        }

        private void InitDicAnalysisType()
        {
            dicAnalysisType.Clear();

            dicAnalysisType.Add(0, "科研");
            dicAnalysisType.Add(1, "维修");
        }
        private void InitDicManageType()
        {
            dicManageType.Clear();

            dicManageType.Add(0, "复合");
            dicManageType.Add(1, "闭环管理");
        }
        private void InitSecFlag()
        {
            dicSecFlag.Clear();

            dicSecFlag.Add(0, "单点");
            dicSecFlag.Add(1, "区段");
        }
    }
}
