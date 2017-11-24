using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using System.Windows.Forms;

namespace InGraph.Classes
{
    class Pair
    {
        public Pair(float from, float to)
        {
            mFrom = from;
            mTo = to;
        }
        public bool Contain(float mile)
        {
            if ( (mile == mFrom) ||
                 (mile == mTo)   ||
                 ((mile > mFrom) && (mile < mTo))
              )
            {
                return true;
            }

            return false;
        }
        private float mFrom, mTo;
    }

    class DaoChaMileRange
    {
        // liyang: linename 就是一个大excel表中的一个 sheet的名字。
        public DaoChaMileRange(String xlsfilename, String linename)
        {
            Valid = false;
            //mLineName = "AJHX"; 
            mLineName = linename;
            // liyang: 读取xls表中tab为linename的表：
            // liyang: 注意加 HDR=NO，否则第0行会被当成表头而不读取。
            string connstr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + xlsfilename + ";" + "Extended Properties=\"Excel 8.0; HDR=NO;IMEX=1;\"";
            string sql = "SELECT * FROM [" + mLineName + "$]";
            OleDbCommand sqlcom;
            using (OleDbConnection sqlconn = new OleDbConnection(connstr))
            {
                sqlcom = new OleDbCommand(sql, sqlconn);
                sqlconn.Open();
                try 
                {
                    OleDbDataReader sdr = sqlcom.ExecuteReader();
                     while (sdr.Read())
                     {
                          // liyang: 只关心 1，2，3列
                           string from =   sdr.GetValue(1).ToString() ;
                           string to =    sdr.GetValue(2).ToString() ;
                           string what =   sdr.GetValue(3).ToString() ;
                          // liyang: 3-道岔  2-曲线   1-直线
                          if ((what.Length!=0)&&(what.Equals("3"))&&(from.Length!=0)&&(to.Length!=0))
                          {
                              AddRange(float.Parse(from), float.Parse(to));
                          }
                     }
                     sdr.Close(); 
                     sdr.Dispose();    // ??   

                     Valid = true;
                }
                catch(OleDbException )
                {
                    string s = "数据获取失败：\n"+ xlsfilename + " 中没有 " + "\"" + linename + "\"" + " 标签页 ！";
                    MessageBox.Show(s);
                }
                finally
                {
                   sqlcom.Dispose(); // ??
                   sqlconn.Close();
                }
            }
        }
        private void AddRange(float from, float to)
        {
            Pair p = new Pair(from, to);
            mRanges.Add(p);
        }
        public bool IsDaoChao(float mile)
        {
            foreach (Pair p in mRanges)
            {
                if (p.Contain(mile))
                    return true;
            }
            return false ;
        }
        private List<Pair> mRanges = new List<Pair>();
        String mLineName = null;
         

        public bool Valid { get; set; } 
    }
}
