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

namespace InGraph.Forms
{
    /// <summary>
    /// 无效信息查看---控件类
    /// </summary>
    public partial class InvalidDataForm : Form
    {
        InvalidDataStatisticsForm statisticsForm;

        public InvalidDataForm()
        {
            InitializeComponent();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void InvalidDataForm_Load(object sender, EventArgs e)
        {

        }

        public void GetInvalidData()
        {
            dataGridView1.Rows.Clear();
            try
            {
                if (CommonClass.listDIC[0].iAppMode == 0)
                {
                    CommonClass.listDIC[0].listIDC = CommonClass.wdp.InvalidDataList(CommonClass.listDIC[0].sAddFile);
                }
                else
                {

                }
                List<InvalidDataClass> listIDC = CommonClass.listDIC[0].listIDC;
                using (OleDbConnection sqlconn = new OleDbConnection(CommonClass.sDBConnectionString))
                {
                    OleDbCommand sqlcom = new OleDbCommand("", sqlconn);
                    sqlconn.Open();
                    for (int i = 0; i < listIDC.Count; i++)
                    {
                        object[] o = new object[9];
                        o[0] = listIDC[i].iId;
                        o[1] = long.Parse(listIDC[i].sStartPoint);
                        o[2] = long.Parse(listIDC[i].sEndPoint);
                        o[3] = float.Parse(listIDC[i].sStartMile);
                        o[4] = float.Parse(listIDC[i].sEndMile);

                        sqlcom.CommandText = "select i_name from 无效区段类型 where i_no=" + listIDC[i].iType.ToString();

                        o[5] = sqlcom.ExecuteScalar();

                        o[6] = listIDC[i].sMemoText;
                        o[7] = listIDC[i].iIsShow;
                        o[8] = listIDC[i].ChannelType;
                        dataGridView1.Rows.Add(o);
                    }
                    sqlconn.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("请删除idf文件，错误描述：" + ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count == 0)
            {
                return;
            }
            int index = dataGridView1.SelectedRows[0].Index;//选中的行的索引
            int rowsCount = dataGridView1.Rows.Count;
            int index_row_display_old = dataGridView1.FirstDisplayedScrollingRowIndex;//显示在datagridview中的第一行的索引。
            int index_row_display_new = 0;
            int index_new = 0;//删除单条记录后，光标应停留的行的索引。
            int VerticalScrollVal=dataGridView1.VerticalScrollingOffset;
            if (dataGridView1.SelectedRows.Count == 1)
            {
                CommonClass.wdp.InvalidDataDelete(CommonClass.listDIC[0].sAddFile, dataGridView1.SelectedRows[0].Cells[1].Value.ToString(), dataGridView1.SelectedRows[0].Cells[2].Value.ToString());
                CommonClass.listDIC[0].listIDC = CommonClass.wdp.InvalidDataList(CommonClass.listDIC[0].sAddFile);
                GetInvalidData();
                //MessageBox.Show("删除成功！");
            }

            //如果选择删除最后一个，则删除完成后，跳到该条的上一条
            if (rowsCount == 1)
            {
                return;
            }
            else
            {
                //删除的是最后一行，删完后还停留在最后一行
                if (index == rowsCount-1)
                {
                    index_new = dataGridView1.Rows.Count - 1;
                    
                }                    
                else //其余中间行，删完后删完自动跳到下一行
                {
                    index_new = index;
                }
                //datagridview显示的第一行的索引
                if (index_row_display_old == 0)
                {
                    index_row_display_new = index_row_display_old;
                }
                else
                {
                    index_row_display_new = index_row_display_old - 1;
                }
            }

            dataGridView1.FirstDisplayedScrollingRowIndex = index_row_display_old;
            dataGridView1.Rows[index_new].Selected = true;

            String value = dataGridView1[1, index_new].Value.ToString();

            //根据文件指针来定位波形，更精确
            long pos = long.Parse(value);
            MainForm.sMainform.MeterFind(pos);

            

        }

        private void InvalidDataForm_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                GetInvalidData();
            }
        }

        private void InvalidDataForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (CommonClass.listDIC[0].listIC == null || CommonClass.listDIC[0].listIC.Count == 0)
            {
                MessageBox.Show("没有进行里程校正，不需要更新里程！");
                return;
            }
            if (CommonClass.listDIC[0].listIDC.Count == 0)
            {
                return;
            }
            try
            {
                for (int i = 0; i < CommonClass.listDIC[0].listIDC.Count; i++)
                {

                    int iStartMile = CommonClass.wdp.PointToMeter(CommonClass.listDIC[0].listIC,
                        long.Parse(CommonClass.listDIC[0].listIDC[i].sStartPoint), CommonClass.listDIC[0].iChannelNumber,
                        CommonClass.listDIC[0].sKmInc);
                    int iEndMile = CommonClass.wdp.PointToMeter(CommonClass.listDIC[0].listIC,
                        long.Parse(CommonClass.listDIC[0].listIDC[i].sEndPoint), CommonClass.listDIC[0].iChannelNumber,
                        CommonClass.listDIC[0].sKmInc);
                    CommonClass.wdp.InvalidDataUpdate(CommonClass.listDIC[0].sAddFile, CommonClass.listDIC[0].listIDC[i].sStartPoint,
                        CommonClass.listDIC[0].listIDC[i].sEndPoint, (iStartMile / 1000f).ToString(), (iEndMile / 1000f).ToString());
                    Application.DoEvents();
                }
            }
            catch
            {

            }
            MessageBox.Show("更新成功！");
            GetInvalidData();
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

        private float StringToFloat(String value)
        {
            String tmpStr = value.Replace("K", "");
            tmpStr = tmpStr.Replace(".", "");
            tmpStr = tmpStr.Replace("+", ".");

            float retVal = float.Parse(tmpStr);

            return retVal;
        }

        private void buttonStatistics_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count == 0)
            {
                return;
            }

            int totalCount = dataGridView1.Rows.Count;
            int totalGongli = 0;
            int startMile = 0;
            int endMile = 0;

            if (CommonClass.listDIC[0].bIndex)
            {
                startMile = (int)(float.Parse(CommonClass.listDIC[0].listIC[0].lStartMeter) * 1000);
                endMile = (int)(float.Parse(CommonClass.listDIC[0].listIC[CommonClass.listDIC[0].listIC.Count - 1].LEndMeter) * 1000);
            }
            else
            {
                //后去没有索引过后
                string sLength = CommonClass.cdp.QueryDataMileageRange(CommonClass.listDIC[0].sFilePath, false, CommonClass.listDIC[0].bEncrypt);
                string[] sSE = sLength.Split(',');
                string[] sSE1 = sSE[1].Split('-');
                startMile = (int)(float.Parse(sSE1[0].Trim()) * 1000);
                endMile = (int)(float.Parse(sSE1[1].Trim()) * 1000);
            }
            totalGongli = Math.Abs(startMile - endMile);


            List<StatisticsDataClass> dataList = new List<StatisticsDataClass>();

            try
            {
                int typeNum = 0;//无效区段类型的个数
                Dictionary<int, String> dicInvalidType = new Dictionary<int, String>();//无效区段类型

                using (OleDbConnection sqlconn = new OleDbConnection(CommonClass.sDBConnectionString))
                {
                    OleDbCommand sqlcom = new OleDbCommand("", sqlconn);
                    sqlconn.Open();

                    sqlcom.CommandText = "select i_no,i_name from 无效区段类型";

                    OleDbDataReader oledbReader = sqlcom.ExecuteReader();
                    while (oledbReader.Read())
                    {
                        dicInvalidType.Add(int.Parse(oledbReader.GetValue(0).ToString()), oledbReader.GetValue(1).ToString());
                    }
                    oledbReader.Close();
                    sqlconn.Close();
                }

                typeNum = dicInvalidType.Count;

                using (OleDbConnection sqlconn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + CommonClass.listDIC[0].sAddFile + ";Persist Security Info=True"))
                {
                    OleDbCommand sqlcom = new OleDbCommand("", sqlconn);
                    sqlconn.Open();
                    for (int i = 0; i < typeNum; i++)
                    {
                        sqlcom.CommandText = "select InvalidType,StartMile,EndMile from InvalidData where InvalidType=" + i.ToString();

                        OleDbDataReader oledbReader = sqlcom.ExecuteReader();

                        StatisticsDataClass statisticsDataCls = null;
                        while (oledbReader.Read())
                        {
                            if (statisticsDataCls == null)
                            {
                                statisticsDataCls = new StatisticsDataClass(totalCount, totalGongli);
                            }
                            statisticsDataCls.reasonType = dicInvalidType[int.Parse(oledbReader.GetValue(0).ToString())];//类型
                            statisticsDataCls.sumcount++;
                            startMile = (int)(float.Parse(oledbReader.GetValue(1).ToString()) * 1000);
                            endMile = (int)(float.Parse(oledbReader.GetValue(2).ToString()) * 1000);

                            statisticsDataCls.sumGongli = statisticsDataCls.sumGongli + Math.Abs(endMile - startMile);
                            
                        }
                        oledbReader.Close();

                        if (statisticsDataCls != null)
                        {
                            dataList.Add(statisticsDataCls);
                        }
                    }
                    sqlconn.Close();
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            statisticsForm = new InvalidDataStatisticsForm(dataList);
            statisticsForm.Owner = this;
            statisticsForm.TopLevel = true;
            statisticsForm.Show();
        }


    }

    public class StatisticsDataClass
    {
        public int totalCount;
        public int totalGongli;
        public StatisticsDataClass(int count,int gongli)
        {
            totalCount = count;
            totalGongli = gongli;
        }

        /// <summary>
        /// 原因类型
        /// </summary>
        public String reasonType;
        /// <summary>
        /// 同一类型的无效区段的个数
        /// </summary>
        public int sumcount = 0;
        /// <summary>
        /// 个数百分比
        /// </summary>
        public String countPercent
        {
            get
            {
                String value = String.Format("{0}%",(float)(sumcount)/totalCount*100);
                return value;
            }
        }
        /// <summary>
        /// 公里百分比
        /// </summary>
        public String gongliPercent
        {
            get
            {
                String value = String.Format("{0}%", (float)(sumGongli) / totalGongli * 100);
                return value;
            }
        }

        public int sumGongli = 0;
    }
}
