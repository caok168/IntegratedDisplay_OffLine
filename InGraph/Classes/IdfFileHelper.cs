using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Data.OleDb;
using CitFileProcess;

namespace InGraph.Classes
{
    public class IdfFileHelper
    {
        #region 创建数据库和表结构

        /// <summary>
        /// 动态创建数据库---idf文件
        /// </summary>
        /// <param name="sFilePath"></param>
        public void CreateDB(string idfFilePath)
        {
            string ConnectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + idfFilePath + ";Jet OLEDB:Engine Type=5";

            ADOX.Catalog ct = new ADOX.Catalog();
            try
            {
                if (File.Exists(idfFilePath))
                {
                    File.Delete(idfFilePath);
                }
                ct.Create(ConnectionString);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                MessageBox.Show(ex.StackTrace);
            }
            ct = null;
            Application.DoEvents();
        }

        /// <summary>
        /// 创建idf数据库里的表
        /// </summary>
        /// <param name="sFilePath"></param>
        public void CreateTable(string idfFilePath, String tableName)
        {

            try
            {
                using (OleDbConnection sqlconn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + idfFilePath + ";Persist Security Info=True"))
                {
                    string sqlCreate = "CREATE TABLE " + tableName + " (" +
                                                                        "Id integer identity primary key," +
                                                                        "KiloMeter varchar(255) NULL," +
                                                                        "Speed varchar(255) NULL," +
                                                                        "Segment_Peak varchar(255) NULL," +
                                                                        "valid integer NULL);";
                    OleDbCommand sqlcom = new OleDbCommand(sqlCreate, sqlconn);
                    sqlconn.Open();
                    sqlcom.ExecuteNonQuery();
                    sqlconn.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                MessageBox.Show(ex.StackTrace);
            }
        }

        /// <summary>
        /// 创建idf数据库里的表：cit文件信息表
        /// </summary>
        /// <param name="sFilePath"></param>
        public void CreateTableCitFileInfo(string idfFilePath)
        {
            String tableName = "CitFileInfo";

            try
            {
                using (OleDbConnection sqlconn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + idfFilePath + ";Persist Security Info=True"))
                {
                    string sqlCreate = "CREATE TABLE " + tableName + " (" +
                                                                        "Id integer identity primary key," +
                                                                        "LineName varchar(255) NULL," +
                                                                        "LineCode varchar(255) NULL," +
                                                                        "LineDir varchar(255) NULL," +
                                                                        "KmInc varchar(255) NULL," +
                                                                        "SDate date NULL," +
                                                                        "STime date NULL," +
                                                                        "Train varchar(255) NULL);";
                    OleDbCommand sqlcom = new OleDbCommand(sqlCreate, sqlconn);
                    sqlconn.Open();
                    sqlcom.ExecuteNonQuery();
                    sqlconn.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                MessageBox.Show(ex.StackTrace);
            }
        }

        #endregion

        /// <summary>
        /// 向idf文件CitFileInfo表中写入cit信息
        /// </summary>
        /// <param name="idfFilePath"></param>
        /// <param name="m_dhi"></param>
        public void WriteTableCitFileInfo(String idfFilePath, DataHeadInfo m_dhi)
        {
            InitDicKmInc();//初始化字典
            InitDicDir();

            try
            {
                using (OleDbConnection sqlconn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + idfFilePath + ";Persist Security Info=True"))
                {
                    sqlconn.Open();

                    // 临时修改开始，此处如果m_dhi.sTrackCode是空字符串（检测车软件没有配置好），sqlcom.ExecuteNonQuery()会出错，临时修改为：
                    if (m_dhi.sTrackCode.StartsWith("\0"))
                    {
                        m_dhi.sTrackCode = "0000"; // 这里的值统一设置为0000，对后面的计算不影响
                    }
                    // 临时修改完毕

                    string sSql = String.Format("insert into {0}(Id,LineName,LineCode,LineDir,KmInc,SDate,STime,Train) values( {1},'{2}','{3}','{4}','{5}','{6}','{7}','{8}');",
                                                "CitFileInfo",
                                                 1,
                                                m_dhi.sTrackName,
                                                m_dhi.sTrackCode,
                                                dicDir[m_dhi.iDir],
                                                dicKmInc[m_dhi.iKmInc],
                                                m_dhi.sDate,
                                                m_dhi.sTime,
                                                m_dhi.sTrain);

                    OleDbCommand sqlcom = new OleDbCommand(sSql, sqlconn);
                    sqlcom.ExecuteNonQuery();

                    sqlconn.Close();
                }
            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("写入{0}文件中的{1}异常", idfFilePath, "CitFileInfo");
                MessageBox.Show(sb.ToString() + "\n" + ex.Message);
                MessageBox.Show(ex.StackTrace);
            }
        }

        /// <summary>
        /// 读取CitFileInfo表中的信息
        /// </summary>
        /// <param name="idfFilePath"></param>
        /// <returns></returns>
        public String ReadTableCitFileInfo(String idfFilePath)
        {
            StringBuilder sb = new StringBuilder();

            try
            {

                using (OleDbConnection sqlconn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + idfFilePath + ";Persist Security Info=False"))
                {
                    OleDbCommand sqlcom = new OleDbCommand("select * from CitFileInfo", sqlconn);
                    sqlconn.Open();
                    OleDbDataReader sqloledr = sqlcom.ExecuteReader();
                    while (sqloledr.Read())
                    {
                        //sb.AppendFormat("{0},", sqloledr.GetValue(0).ToString());
                        sb.AppendFormat("{0},", sqloledr.GetValue(1).ToString());
                        sb.AppendFormat("{0},", sqloledr.GetValue(2).ToString());
                        sb.AppendFormat("{0},", sqloledr.GetValue(3).ToString());
                        sb.AppendFormat("{0},", sqloledr.GetValue(4).ToString());
                        sb.AppendFormat("{0},", DateTime.Parse(sqloledr.GetValue(5).ToString()).Date.ToShortDateString());
                        sb.AppendFormat("{0},", DateTime.Parse(sqloledr.GetValue(6).ToString()).TimeOfDay.ToString());
                        sb.AppendFormat("{0}", sqloledr.GetValue(7).ToString());
                    }
                    sqlconn.Close();
                }

            }
            catch (Exception ex)
            {
                return "1," + ex.Message;
            }

            return "0," + sb.ToString();
        }


        public void ReadTablePeak(String idfFilePath, String tableName,out List<double> mileData, out List<double> peakData, out List<double> speedData)
        {
            mileData = new List<double>();
            peakData = new List<double>();
            speedData = new List<double>();
            try
            {

                using (OleDbConnection sqlconn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + idfFilePath + ";Persist Security Info=False"))
                {
                    String sql = String.Format("select KiloMeter,Segment_Peak,Speed from {0} order by Id", tableName);
                    OleDbCommand sqlcom = new OleDbCommand(sql, sqlconn);
                    sqlconn.Open();
                    OleDbDataReader sqloledr = sqlcom.ExecuteReader();
                    while (sqloledr.Read())
                    {
                        double mile = double.Parse(sqloledr.GetValue(0).ToString());
                        double peak = double.Parse(sqloledr.GetValue(1).ToString());
                        double speed = double.Parse(sqloledr.GetValue(2).ToString());
                        mileData.Add(mile);
                        peakData.Add(peak);
                        speedData.Add(speed);
                    }
                    sqlconn.Close();
                }

            }
            catch (Exception ex)
            {
                mileData = null;
                peakData = null;
                speedData = null;
                return;
            }

            return;
        }


        public void InsertIntoSegmentPeakTable(string idfFilePath, String tableName, List<double> data_KM, List<double> data_SPEED, List<double> data_Peak)
        {
            try
            {
                using (OleDbConnection sqlconn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + idfFilePath + ";Persist Security Info=True"))
                {
                    sqlconn.Open();

                    for (int i = 0; i < data_KM.Count; i++)
                    {
                        string sSql = String.Format("insert into {0} (KiloMeter,Speed,Segment_Peak,Valid) values( '{1}','{2}','{3}',{4} )",
                                                    tableName,
                                                    data_KM[i].ToString(),
                                                    data_SPEED[i].ToString(),
                                                    data_Peak[i].ToString(),
                                                    1);

                        OleDbCommand sqlcom = new OleDbCommand(sSql, sqlconn);
                        sqlcom.ExecuteNonQuery();
                    }

                    sqlconn.Close();
                }
            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("生成{0}文件中的{1}异常", idfFilePath, tableName);
                MessageBox.Show(sb.ToString() + "\n" + ex.Message);
                MessageBox.Show(ex.StackTrace);
            }
        }



        private Dictionary<int, String> dicKmInc = null;
        private void InitDicKmInc()
        {
            if (dicKmInc == null || dicKmInc.Count == 0)
            {
                dicKmInc = new Dictionary<int, String>();
                dicKmInc.Add(0, "增");
                dicKmInc.Add(1, "减");
            }
        }

        private Dictionary<int, String> dicDir = null;
        private void InitDicDir()
        {
            if (dicDir == null || dicDir.Count == 0)
            {
                dicDir = new Dictionary<int, String>();
                dicDir.Add(1, "上");
                dicDir.Add(2, "下");
                dicDir.Add(3, "单");
            }
        }
    }
}
