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

namespace InGraph.Forms
{
    /// <summary>
    /// 波形数据分割--控件类
    /// </summary>
    public partial class WaveSplitForm : Form
    {
        public WaveSplitForm()
        {
            InitializeComponent();
        }

        private void WaveSplitForm_Load(object sender, EventArgs e)
        {
            comboBoxSplitUnit.SelectedIndex = 0;
        }

        private void comboBoxSplitUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            try
            {
                int iLen = 2;
                switch (comboBoxSplitUnit.SelectedIndex)
                {
                    case 0:
                        iLen = 2;
                        break;
                    case 1:
                        iLen = 5;
                        break;
                    case 2:
                        iLen = 7;
                        break;
                    case 3:
                        iLen = 9;
                        break;
                }

                using (OleDbConnection oledbc = new OleDbConnection(CommonClass.sDBConnectionString))
                {
                    String str = "select IIf(" + iLen.ToString() +
                           "=2,(select distinct bureauname from dw b where b.bureaucode=a.sunitid)," +
    "(select distinct unitname from dw b where b.unitid=a.sunitid)) as unitname,a.sstartmile,a.sendmile from " +
         "(SELECT left(unitid," + iLen.ToString() + ")  as sunitid,min(startmile) as sstartmile,max(endmile) as sendmile " +
    "FROM Gj where lineid='" + CommonClass.listDIC[0].sLineCode + "' and linedirname='" + CommonClass.listDIC[0].sDir + "' " +
    "group by left(unitid," + iLen.ToString() + ")) a";
                    OleDbCommand oledbcomm = new OleDbCommand("select IIf(" + iLen.ToString() +
                           "=2,(select distinct bureauname from dw b where b.bureaucode=a.sunitid)," +
    "(select distinct unitname from dw b where b.unitid=a.sunitid)) as unitname,a.sstartmile,a.sendmile from " +
         "(SELECT left(unitid," + iLen.ToString() + ")  as sunitid,min(startmile) as sstartmile,max(endmile) as sendmile " +
    "FROM Gj where lineid='" + CommonClass.listDIC[0].sLineCode + "' and linedirname='" + CommonClass.listDIC[0].sDir + "' " +
    "group by left(unitid," + iLen.ToString() + ")) a", oledbc);
                    oledbc.Open();
                    OleDbDataReader oledr = oledbcomm.ExecuteReader();
                    while (oledr.Read())
                    {
                        object[] o = new object[3];
                        o[0] = oledr.GetValue(0).ToString();
                        o[1] = oledr.GetValue(1).ToString();
                        o[2] = oledr.GetValue(2).ToString();
                        dataGridView1.Rows.Add(o);
                    }
                    oledr.Close();
                    oledbc.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void buttonSplitProcess_Click(object sender, EventArgs e)
        {
            this.Enabled = false;
            Application.DoEvents();
            //获取文件范围
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                try
                {
                    //创建目录
                    //string sDestPath = CommonClass.sWaveSplitPath + "\\" + dataGridView1.Rows[i].Cells[0].Value.ToString() + "\\";

                    double dStartMile = double.Parse(dataGridView1.Rows[i].Cells[1].Value.ToString());
                    double dEndMile = double.Parse(dataGridView1.Rows[i].Cells[2].Value.ToString());
                    if (CommonClass.listDIC[0].sKmInc.Equals("减"))
                    {
                        double dChange = 0;
                        dChange = dStartMile;
                        dStartMile = dEndMile;
                        dEndMile = dChange;
                    }
                    if (CommonClass.listDIC[0].sKmInc.Equals("增"))
                    {
                        if(dStartMile>=double.Parse(CommonClass.listDIC[0].listIC[CommonClass.listDIC[0].listIC.Count-1].LEndMeter))
                        {
                            continue;
                        }
                        if (dEndMile <= double.Parse(CommonClass.listDIC[0].listIC[0].lStartMeter))
                        {
                            continue;
                        }
                    }
                    else
                    {
                        if (dEndMile >= double.Parse(CommonClass.listDIC[0].listIC[0].lStartMeter))
                        {
                            continue;
                        }
                        if (dStartMile <= double.Parse(CommonClass.listDIC[0].listIC[CommonClass.listDIC[0].listIC.Count - 1].LEndMeter))
                        {
                            continue;
                        }
                    }
                    #region 获取索引范围
                    long lStartPostionCit = CommonClass.listDIC[0].listIC[0].lStartPoint;//采样点数据的起始指针
                    long lEndPostionCit = CommonClass.listDIC[0].listIC[CommonClass.listDIC[0].listIC.Count - 1].lEndPoint;//采样点数据的结束指针
                    long lStartPostion = lStartPostionCit;//分割数据的起始指针
                    long lEndPostion = lEndPostionCit;//分割数据的结束指针
                    int gjtds=CommonClass.listDIC[0].iChannelNumber;

                    int iStartIndex = 0;
                    int iEndIndex = CommonClass.listDIC[0].listIC.Count - 1;
                    //获取起始
                    for (int j = 0; j < CommonClass.listDIC[0].listIC.Count; j++)
                    {
                        double m_startMeter = double.Parse(CommonClass.listDIC[0].listIC[j].lStartMeter);
                        double m_endMeter = double.Parse(CommonClass.listDIC[0].listIC[j].LEndMeter);
                        long m_startPoint = CommonClass.listDIC[0].listIC[j].lStartPoint;
                        long m_endPoint = CommonClass.listDIC[0].listIC[j].lEndPoint;
                        double m_containsMeter = double.Parse(CommonClass.listDIC[0].listIC[j].lContainsMeter);
                        long m_containsPoint = CommonClass.listDIC[0].listIC[j].lContainsPoint;
                        double m_meterPerPoint = m_containsMeter / m_containsPoint;

                        if (CommonClass.listDIC[0].sKmInc.Equals("增"))
                        {
                            if (m_startMeter <= dStartMile && m_endMeter > dStartMile)
                            {
                                lStartPostion = m_startPoint + (long)((dStartMile - m_startMeter) / m_meterPerPoint) * gjtds * 2 - 4000 * gjtds * 2;
                                lStartPostion = (lStartPostion >= lStartPostionCit) ? lStartPostion : lStartPostionCit;
                                iStartIndex = j;
                                break;
                            }
                        }
                        else
                        {
                            if (m_startMeter >= dStartMile && m_endMeter < dStartMile)
                            {
                                lStartPostion = m_startPoint + (long)((m_startMeter - dStartMile) / m_meterPerPoint) * gjtds * 2 - 4000 * gjtds * 2;
                                lStartPostion = (lStartPostion >= lStartPostionCit) ? lStartPostion : lStartPostionCit;
                                iStartIndex = j;
                                break;
                            }
                        }
                    }
                    //获取终止
                    for (int j = 0; j < CommonClass.listDIC[0].listIC.Count; j++)
                    {
                        double m_startMeter = double.Parse(CommonClass.listDIC[0].listIC[j].lStartMeter);
                        double m_endMeter = double.Parse(CommonClass.listDIC[0].listIC[j].LEndMeter);
                        long m_startPoint = CommonClass.listDIC[0].listIC[j].lStartPoint;
                        long m_endPoint = CommonClass.listDIC[0].listIC[j].lEndPoint;
                        double m_containsMeter = double.Parse(CommonClass.listDIC[0].listIC[j].lContainsMeter);
                        long m_containsPoint = CommonClass.listDIC[0].listIC[j].lContainsPoint;
                        double m_meterPerPoint = m_containsMeter / m_containsPoint;

                        if (CommonClass.listDIC[0].sKmInc.Equals("增"))
                        {
                            if (m_startMeter < dEndMile && m_endMeter >= dEndMile)
                            {
                                lEndPostion = m_startPoint + (long)((dEndMile - m_startMeter) / m_meterPerPoint) * gjtds * 2 + 4000 * gjtds * 2;
                                lEndPostion = (lEndPostion <= lEndPostionCit) ? lEndPostion : lEndPostionCit;
                                iEndIndex = j;
                                break;
                            }
                        }
                        else
                        {
                            if (m_startMeter > dEndMile && m_endMeter <= dEndMile)
                            {
                                lEndPostion = m_startPoint + (long)((m_startMeter - dEndMile) / m_meterPerPoint) * gjtds * 2 + 4000 * gjtds * 2;
                                lEndPostion = (lEndPostion <= lEndPostionCit) ? lEndPostion : lEndPostionCit;
                                iEndIndex = j;
                                break;
                            }
                        }
                    }
                    #endregion

                    string sDestPath = Path.Combine(CommonClass.sWaveSplitPath, dataGridView1.Rows[i].Cells[0].Value.ToString());
                    if (!Directory.Exists(sDestPath))
                    {
                        Directory.CreateDirectory(sDestPath);
                    }

                    //分割波形
                    string sDestFile = sDestPath +"\\"+
                            Path.GetFileNameWithoutExtension(CommonClass.listDIC[0].sFilePath) + "_" +
                            dStartMile.ToString() + "-" + dEndMile.ToString() + ".cit";
                    long iFileHead = 0;
                    #region 生成新波形
                    using (FileStream fs = new FileStream(CommonClass.listDIC[0].sFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    {
                        using (FileStream fsWrite = new FileStream(sDestFile, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
                        {
                            using (BinaryReader br = new BinaryReader(fs, Encoding.Default))
                            {
                                using (BinaryWriter bw = new BinaryWriter(fsWrite, Encoding.Default))
                                {
                                    bw.Write(br.ReadBytes(120));
                                    bw.Write(br.ReadBytes(65 * CommonClass.listDIC[0].iChannelNumber));
                                    byte[] last4 = br.ReadBytes(4);
                                    bw.Write(last4);
                                    byte[] zh = br.ReadBytes(BitConverter.ToInt32(last4, 0));
                                    if (zh.Length != 0)
                                    {
                                        bw.Write(zh);
                                    }
                                    iFileHead = br.BaseStream.Position;
                                    br.BaseStream.Position = lStartPostion;

                                    while (br.BaseStream.Position < lEndPostion)
                                    {
                                        bw.Write(br.ReadBytes(CommonClass.listDIC[0].iChannelNumber * 2));
                                    }
                                    bw.Flush();
                                    bw.Close();
                                }
                                br.Close();
                            }
                            fsWrite.Close();
                        }
                        fs.Close();
                    }
                    #endregion
                    //创建索引库
                    string sIndexFile = sDestFile.Replace(".cit", ".idf");
                    CommonClass.wdp.CreateDB(sIndexFile);
                    CommonClass.wdp.CreateTable(sIndexFile);
                    #region 插入新索引
                    //using (OleDbConnection sqlconn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + sIndexFile + ";Persist Security Info=True"))
                    //{
                    //    string sSql = "";
                    //    OleDbCommand sqlcom = new OleDbCommand(sSql, sqlconn);
                    //    sqlconn.Open();
                    //    sqlcom.CommandText = "delete from IndexSta";
                    //    sqlcom.ExecuteNonQuery();
                    //    for (int k = iStartIndex; k <= iEndIndex; k++)
                    //    {
                    //        sqlcom.CommandText = "insert into IndexSta values(" + k.ToString()
                    //          + "," + CommonClass.listDIC[0].listIC[k].iIndexID.ToString() +
                    //          ",'" + iFileHead.ToString() + "','" +
                    //          CommonClass.listDIC[0].listIC[k].lStartMeter.ToString() +
                    //          "','" + (CommonClass.listDIC[0].listIC[k].lEndPoint -
                    //          CommonClass.listDIC[0].listIC[k].lStartPoint + iFileHead).ToString() + "','" +
                    //          CommonClass.listDIC[0].listIC[k].LEndMeter.ToString() + "','" +
                    //          CommonClass.listDIC[0].listIC[k].lContainsPoint.ToString() +
                    //          "','" + CommonClass.listDIC[0].listIC[k].lContainsMeter.ToString() +
                    //          "','" + CommonClass.listDIC[0].listIC[k].sType + "')";
                    //        sqlcom.ExecuteNonQuery();
                    //        iFileHead = (CommonClass.listDIC[0].listIC[k].lEndPoint -
                    //          CommonClass.listDIC[0].listIC[k].lStartPoint + iFileHead);
                    //    }

                    //    sqlconn.Close();
                    //}
                    #endregion

                    #region 插入新索引
                    using (OleDbConnection sqlconn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + sIndexFile + ";Persist Security Info=True"))
                    {
                        string sSql = "";
                        OleDbCommand sqlcom = new OleDbCommand(sSql, sqlconn);
                        sqlconn.Open();
                        sqlcom.CommandText = "delete from IndexSta";
                        sqlcom.ExecuteNonQuery();

                        long m_fileHead = iFileHead;

                        for (int j = 0; j < CommonClass.listDIC[0].listIC.Count; j++)
                        {
                            double m_startMeter = double.Parse(CommonClass.listDIC[0].listIC[j].lStartMeter);
                            double m_endMeter = double.Parse(CommonClass.listDIC[0].listIC[j].LEndMeter);
                            long m_startPoint = CommonClass.listDIC[0].listIC[j].lStartPoint;
                            long m_endPoint = CommonClass.listDIC[0].listIC[j].lEndPoint;
                            double m_containsMeter = double.Parse(CommonClass.listDIC[0].listIC[j].lContainsMeter);
                            long m_containsPoint = CommonClass.listDIC[0].listIC[j].lContainsPoint;
                            double m_meterPerPoint = m_containsMeter / m_containsPoint;

                            int id = 0;
                            int indexId = 0;
                            long startPosition = 0;
                            double startMeter = 0;
                            long endPosition = 0;
                            double endMeter = 0;
                            long containsPoint = 0;
                            double containsMeter = 0;
                            String indexType = null;

                            String sqlFormat = "insert into IndexSta values({0},{1},'{2}','{3}','{4}','{5}','{6}','{7}','{8}')";
                            String sql = String.Format(sqlFormat, id.ToString(), indexId.ToString(), startPosition.ToString(), startMeter.ToString(), endPosition.ToString(), endMeter.ToString(), containsPoint.ToString(), containsMeter.ToString(), indexType);

                            id = j;
                            indexId = CommonClass.listDIC[0].listIC[j].iIndexID;
                            

                            //第一段索引
                            if (m_startPoint <= lStartPostion && lStartPostion < m_endPoint)
                            {

                                startPosition = m_fileHead;
                                startMeter = Math.Round(m_startMeter + ((m_endMeter - m_startMeter) / m_containsPoint) * ((lStartPostion - m_startPoint)/(gjtds*2)),3);

                                //如果分割的波形都在同一段索引内
                                if (m_startPoint < lEndPostion && lEndPostion <= m_endPoint)
                                {
                                    endPosition = lEndPostion - lStartPostion + startPosition;
                                    endMeter = Math.Round(m_startMeter + ((m_endMeter - m_startMeter) / m_containsPoint) * ((lEndPostion - m_startPoint) / (gjtds * 2)), 3);

                                    containsPoint = (endPosition - startPosition) / (gjtds * 2);
                                    containsMeter = Math.Abs(startMeter - endMeter);
                                    indexType = CommonClass.listDIC[0].listIC[j].sType;

                                    sql = String.Format(sqlFormat, id.ToString(), indexId.ToString(), startPosition.ToString(), startMeter.ToString(), endPosition.ToString(), endMeter.ToString(), containsPoint.ToString(), containsMeter.ToString(), indexType);
                                    sqlcom.CommandText = sql;
                                    sqlcom.ExecuteNonQuery();
                                    m_fileHead = endPosition;

                                    break;
                                }

                                endPosition = m_endPoint - lStartPostion + startPosition;
                                endMeter = m_endMeter;

                                containsPoint = (endPosition - startPosition) / (gjtds * 2);
                                containsMeter = Math.Abs(startMeter - endMeter);
                                indexType = CommonClass.listDIC[0].listIC[j].sType;

                                sql = String.Format(sqlFormat, id.ToString(), indexId.ToString(), startPosition.ToString(), startMeter.ToString(), endPosition.ToString(), endMeter.ToString(), containsPoint.ToString(), containsMeter.ToString(), indexType);
                                sqlcom.CommandText = sql;
                                sqlcom.ExecuteNonQuery();
                                m_fileHead = endPosition;

                            }
                            //中间段索引
                            if ((lStartPostion < m_startPoint && lStartPostion < m_endPoint) && (lEndPostion > m_startPoint && lEndPostion > m_endPoint))
                            {
                                startPosition = m_fileHead; ;
                                startMeter = m_startMeter;
                                endPosition = m_endPoint - m_startPoint + startPosition;
                                endMeter = m_endMeter;

                                containsPoint = (endPosition - startPosition) / (gjtds * 2);
                                containsMeter = Math.Abs(startMeter - endMeter);
                                indexType = CommonClass.listDIC[0].listIC[j].sType;

                                sql = String.Format(sqlFormat, id.ToString(), indexId.ToString(), startPosition.ToString(), startMeter.ToString(), endPosition.ToString(), endMeter.ToString(), containsPoint.ToString(), containsMeter.ToString(), indexType);
                                sqlcom.CommandText = sql;
                                sqlcom.ExecuteNonQuery();
                                m_fileHead = endPosition;
                            }
                            //最后一段索引
                            if (m_startPoint < lEndPostion && lEndPostion <= m_endPoint)
                            {
                                startPosition = m_fileHead;
                                startMeter = m_startMeter;
                                endPosition = lEndPostion - m_startPoint + startPosition;
                                endMeter = Math.Round(m_startMeter + ((m_endMeter - m_startMeter) / m_containsPoint) * ((lEndPostion - m_startPoint) / (gjtds * 2)), 3);

                                containsPoint = (endPosition - startPosition) / (gjtds * 2);
                                containsMeter = Math.Abs(startMeter - endMeter);
                                indexType = CommonClass.listDIC[0].listIC[j].sType;

                                sql = String.Format(sqlFormat, id.ToString(), indexId.ToString(), startPosition.ToString(), startMeter.ToString(), endPosition.ToString(), endMeter.ToString(), containsPoint.ToString(), containsMeter.ToString(), indexType);
                                sqlcom.CommandText = sql;
                                sqlcom.ExecuteNonQuery();
                                m_fileHead = endPosition;
                            }



                        }

                        sqlconn.Close();
                    }
                    #endregion

                    #region 插入无效区段
                    using (OleDbConnection sqlconn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + sIndexFile + ";Persist Security Info=True"))
                    {
                        string sSql = "";
                        OleDbCommand sqlcom = new OleDbCommand(sSql, sqlconn);
                        sqlconn.Open();
                        sqlcom.CommandText = "delete from InvalidData";
                        sqlcom.ExecuteNonQuery();
                        for (int j = 0; j < CommonClass.listDIC[0].listIDC.Count; j++)
                        {
                            long m_startPoint = long.Parse(CommonClass.listDIC[0].listIDC[j].sStartPoint);
                            long m_endPoint = long.Parse(CommonClass.listDIC[0].listIDC[j].sEndPoint);
                            double m_startMeter = double.Parse(CommonClass.listDIC[0].listIDC[j].sStartMile);
                            double m_endMeter = double.Parse(CommonClass.listDIC[0].listIDC[j].sEndMile);
                            int m_invalidType = CommonClass.listDIC[0].listIDC[j].iType;
                            string m_memoText = CommonClass.listDIC[0].listIDC[j].sMemoText;
                            int m_isShow = CommonClass.listDIC[0].listIDC[j].iIsShow;
                            string m_channelType = CommonClass.listDIC[0].listIDC[j].ChannelType;

                            int id = 0;
                            long startPosition = 0;
                            long endPosition = 0;
                            double startMeter = 0;
                            double endMeter = 0;
                            int invalidType = m_invalidType;
                            string memoText = m_memoText;
                            int isShow = m_isShow;
                            String channelType = m_channelType;

                            String sqlFormat = "insert into InvalidData values({0},'{1}','{2}','{3}','{4}',{5},'{6}',{7},'{8}')";
                            String sql = String.Format(sqlFormat, id.ToString(), startPosition.ToString(), endPosition.ToString(), startMeter.ToString(), endMeter.ToString(), invalidType.ToString(), memoText, isShow.ToString(), channelType);

                            id = j;


                            //第一种情形：无效区段完全在分割的波形内
                            if (m_startPoint >= lStartPostion && m_endPoint <= lEndPostion)
                            {

                                startPosition = iFileHead + m_startPoint - lStartPostion;
                                startMeter = m_startMeter;
                                endPosition = m_endPoint - m_startPoint + startPosition;
                                endMeter = m_endMeter;

                                sql = String.Format(sqlFormat, id.ToString(), startPosition.ToString(), endPosition.ToString(), startMeter.ToString(), endMeter.ToString(), invalidType.ToString(), memoText, isShow.ToString(), channelType);
                                sqlcom.CommandText = sql;
                                sqlcom.ExecuteNonQuery();
                            }
                            //第二种情形：无效区段的跨越分割波形的起始部分
                            if (m_startPoint < lStartPostion && lStartPostion < m_endPoint)
                            {
                                startPosition = iFileHead; ;
                                startMeter = m_startMeter + 0.25 * (lStartPostion - m_startPoint) / (gjtds * 2);
                                endPosition = m_endPoint - lStartPostion + startPosition;
                                endMeter = m_endMeter;

                                sql = String.Format(sqlFormat, id.ToString(), startPosition.ToString(), endPosition.ToString(), startMeter.ToString(), endMeter.ToString(), invalidType.ToString(), memoText, isShow.ToString(), channelType);
                                sqlcom.CommandText = sql;
                                sqlcom.ExecuteNonQuery();
                            }
                            //第三种情形：无效区段的跨越分割波形的结束部分
                            if (m_startPoint < lEndPostion && lEndPostion < m_endPoint)
                            {
                                startPosition = iFileHead + m_startPoint - lStartPostion;
                                startMeter = m_startMeter;
                                endPosition = m_endPoint - m_startPoint + startPosition;
                                endMeter = m_startMeter + 0.25 * (lEndPostion - m_startPoint) / (gjtds * 2);

                                sql = String.Format(sqlFormat, id.ToString(), startPosition.ToString(), endPosition.ToString(), startMeter.ToString(), endMeter.ToString(), invalidType.ToString(), memoText, isShow.ToString(), channelType);
                                sqlcom.CommandText = sql;
                                sqlcom.ExecuteNonQuery();
                            }
                        }

                        sqlconn.Close();
                    }
                    #endregion
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            MessageBox.Show("分割成功!");
            Application.DoEvents();
            this.Enabled = true;
        }
    }
}
