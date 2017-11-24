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
using System.Threading;

namespace InGraph.Forms
{
    public partial class UnitCutPictureForm : Form
    {
        String excelName = null;

        public class UnitInfoClass
        {
            public String unitID;
            public String unitName;
            public String lineName;
            public String lineDir;
            public String unitStart;
            public String unitEnd;
            public String unitType;
            public String calcTime;
            public String level;
            public Byte[] pictureContent;
            public String pictureType;
        }

        List<UnitInfoClass> unitInfoClsList = new List<UnitInfoClass>();



        public UnitCutPictureForm()
        {
            InitializeComponent();
        }

        #region 返回Excel数据源
        /// <summary>
        /// 返回Excel数据源
        /// </summary>
        /// <param name="filename">文件路径</param>
        /// <returns></returns>
        static public DataTable ExcelToDataTable(string filename)
        {
            DataTable dt;
            string strCon = "Provider=Microsoft.Jet.OLEDB.4.0;Extended Properties=Excel 8.0;data source=" + filename;
            OleDbConnection myConn = new OleDbConnection(strCon);
            string strCom = "SELECT * FROM [单元信息得分列表0$]";
            myConn.Open();
            OleDbDataAdapter myCommand = new OleDbDataAdapter(strCom, myConn);
            dt = new DataTable();
            myCommand.Fill(dt);
            myConn.Close();
            return dt;
        }
        #endregion

        #region 把excel数据读取到List列表中
        /// <summary>
        /// 把excel数据读取到List列表中
        /// </summary>
        /// <param name="excelFilePath">excel文件路径</param>
        private void ReadUnitInfoIntoList(String excelFilePath)
        {
            DataTable dt = null;

            dt = ExcelToDataTable(excelFilePath);

            if (unitInfoClsList.Count > 0)
            {
                unitInfoClsList.Clear();
            }

            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count;i++ )
                {
                    UnitInfoClass unitInfoCls = new UnitInfoClass();

                    unitInfoCls.unitID = (String)(dt.Rows[i]["单元编号"]);
                    unitInfoCls.unitName = (String)(dt.Rows[i]["单元名称"]);
                    unitInfoCls.lineName = (String)(dt.Rows[i]["线路"]);
                    unitInfoCls.lineDir = (String)(dt.Rows[i]["行别"]);
                    unitInfoCls.unitStart = (String)(dt.Rows[i]["单元起始里程"]);
                    unitInfoCls.unitEnd = (String)(dt.Rows[i]["单元结束里程"]);
                    unitInfoCls.unitType = (String)(dt.Rows[i]["设备类型"]);
                    unitInfoCls.calcTime = (String)(dt.Rows[i]["计算时间"]);
                    unitInfoCls.level = (String)(dt.Rows[i]["级别"]);

                    unitInfoClsList.Add(unitInfoCls);
                }
            } 
            else
            {
            }
        }
        #endregion

        private void MeterFind(float f)
        {
            if (CommonClass.listDIC[0].listIC != null && CommonClass.listDIC[0].listIC.Count >0)
            {//里程已修正
                long findPos = CommonClass.wdp.GetNewIndexMeterPositon(CommonClass.listDIC[0].listIC, (long)(f * 1000), CommonClass.listDIC[0].iChannelNumber, CommonClass.listDIC[0].sKmInc, 0);
                MainForm.sMainform.MeterFind(findPos);
            }
            else
            {//里程未修正
                MainForm.sMainform.MeterFind(f);
            }
        }


        private void CutUnitImage(List<UnitInfoClass> uicList)
        {
            if (uicList == null || uicList.Count == 0)
            {
                return;
            }

            for (int i = 0; i < uicList.Count;i++ )
            {
                float startMile = 0;
                float endMile = 0;
                try
                {
                    startMile = float.Parse(uicList[i].unitStart);
                    endMile = float.Parse(uicList[i].unitEnd);                    
                }
                catch (System.Exception ex)
                {
                    //如果解析里程出错，则退出循环
                    MessageBox.Show(ex.Message + ex.Source + ex.StackTrace);
                    return;
                }
                
                //获取里程，调用截图，并把截取的图片保存在单元信息的对象里

                float finalMile = (startMile + endMile) / 2;
                //里程定位
                MeterFind(finalMile);
                //波形自动对齐
                MainForm.sMainform.AutoTranslation();
                Thread.Sleep(50);

                String msg = String.Format("单元编号：{0},单元名称：{1},起始里程：{2},结束里程：{3}", uicList[i].unitID, uicList[i].unitName, uicList[i].unitStart, uicList[i].unitEnd);
                float halfLen = (float)Math.Abs(startMile - endMile) / 2;

                Bitmap bmp = MainForm.sMainform.DrawingPoints(finalMile, msg, "", halfLen);

                MemoryStream memoryStream = new MemoryStream();
                bmp.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
                byte[] bmpBytes = memoryStream.ToArray();

                uicList[i].pictureContent = bmpBytes;
                uicList[i].pictureType = "png";
            }
        }

        private void InitDataGridView(List<UnitInfoClass> uicList)
        {
            if (uicList == null || uicList.Count == 0)
            {
                return;
            }

            dataGridView_UnitInfo.Rows.Clear();

            foreach (UnitInfoClass unitInfoCls in uicList)
            {
                DataGridViewRow dgvr = new DataGridViewRow();
                dgvr.CreateCells(dataGridView_UnitInfo);

                dgvr.Cells[0].Value = unitInfoCls.unitID;
                dgvr.Cells[1].Value = unitInfoCls.unitName;
                dgvr.Cells[2].Value = unitInfoCls.lineName;
                dgvr.Cells[3].Value = unitInfoCls.lineDir;
                dgvr.Cells[4].Value = unitInfoCls.unitStart;
                dgvr.Cells[5].Value = unitInfoCls.unitEnd;
                dgvr.Cells[6].Value = unitInfoCls.unitType;
                dgvr.Cells[7].Value = unitInfoCls.calcTime;
                dgvr.Cells[8].Value = unitInfoCls.level;


                dataGridView_UnitInfo.Rows.Add(dgvr);
            }
        }

        private void UnitInfoListToIdf(String idfFilePath)
        {
            CommonClass.wdp.CreateDB(idfFilePath);
            CommonClass.wdp.CreatTableUnitInfo(idfFilePath);

            try
            {
                using (OleDbConnection sqlconn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + idfFilePath + ";Persist Security Info=True"))
                {
                    sqlconn.Open();

                    OleDbCommand com = sqlconn.CreateCommand();
                    com.CommandText = "delete * from UnitInfo";
                    com.ExecuteNonQuery();

                    for (int i = 0; i < unitInfoClsList.Count; i++)
                    {
                        com = sqlconn.CreateCommand();
                        com.CommandText = "insert into UnitInfo values(@ID,@UnitID,@UnitName,@LineName,@LineDir,@UnitStart,@UnitEnd,@UnitType,@CalcTime,@UnitLevel,@PictureContent,@PictureType)";

                        com.Parameters.AddWithValue("@Id", i + 1);
                        com.Parameters.AddWithValue("@UnitID", unitInfoClsList[i].unitID);
                        com.Parameters.AddWithValue("@UnitName", unitInfoClsList[i].unitName);
                        com.Parameters.AddWithValue("@LineName", unitInfoClsList[i].lineName);
                        com.Parameters.AddWithValue("@LineDir", unitInfoClsList[i].lineDir);
                        com.Parameters.AddWithValue("@UnitStart", unitInfoClsList[i].unitStart);
                        com.Parameters.AddWithValue("@UnitEnd", unitInfoClsList[i].unitEnd);
                        com.Parameters.AddWithValue("@UnitType", unitInfoClsList[i].unitType);
                        com.Parameters.AddWithValue("@CalcTime", unitInfoClsList[i].calcTime);
                        com.Parameters.AddWithValue("@UnitLevel", unitInfoClsList[i].level);
                        com.Parameters.AddWithValue("@PictureContent", unitInfoClsList[i].pictureContent);
                        com.Parameters.AddWithValue("@PictureType", unitInfoClsList[i].pictureType);

                        com.ExecuteNonQuery();
                    }

                    sqlconn.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("新增单元信息异常:" + ex.Message);
                return;
            }
            return;

        }

        private void buttonSelectFile_Click(object sender, EventArgs e)
        {
            openFileDialog1.FileName = "";

            DialogResult dr = openFileDialog1.ShowDialog();
            if (dr == DialogResult.OK)
            {
                excelName = openFileDialog1.FileName;


                textBoxExcelPath.Text = excelName;
            }
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            try
            {
                ReadUnitInfoIntoList(excelName);

                CutUnitImage(unitInfoClsList);

                String idfFilePath = Path.Combine(Path.GetDirectoryName(excelName), "单元信息.idf");
                UnitInfoListToIdf(idfFilePath);

                InitDataGridView(unitInfoClsList);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


        }
        #region DataGridView自动在行头添加行号
        //DataGridView自动在行头添加行号
        /// <summary>
        /// DataGridView自动在行头添加行号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;
            Color color = dgv.RowHeadersDefaultCellStyle.ForeColor;
            if (dgv.Rows[e.RowIndex].Selected)
            {
                //color = dgv.RowHeadersDefaultCellStyle.SelectionForeColor;
            }
            else
            {
                color = dgv.RowHeadersDefaultCellStyle.ForeColor;
            }

            using (SolidBrush b = new SolidBrush(color))
            {
                e.Graphics.DrawString((e.RowIndex + 1).ToString(), e.InheritedRowStyle.Font, b, e.RowBounds.Location.X + 10, e.RowBounds.Location.Y + 6);
            }
        }
        #endregion

        private void buttonUnitPictureView_Click(object sender, EventArgs e)
        {
            if (dataGridView_UnitInfo.SelectedRows.Count != 0)
            {
                int m_UnitID = int.Parse((string)(dataGridView_UnitInfo.SelectedRows[0].Cells[0].Value));

                byte[] imageData = null;

                //try
                //{
                //    using (OleDbConnection sqlconn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + sIICFilePath + ";Persist Security Info=True"))
                //    {
                //        sqlconn.Open();
                //        OleDbCommand com = sqlconn.CreateCommand();
                //        com.CommandText = "select * from IICImages where ExceptionId = " + defectNum;

                //        OleDbDataReader dr = com.ExecuteReader();
                //        while (dr.Read())
                //        {
                //            imageData = (byte[])(dr.GetValue(2));
                //        }

                //        com.Dispose();
                //        sqlconn.Close();
                //    }
                //}
                //catch (Exception ex)
                //{
                //    MessageBox.Show("读取偏差图片异常:" + ex.Message);
                //}

                foreach(UnitInfoClass unitInfoCls in unitInfoClsList)
                {
                    int unitID = 0;
                    try
                    {
                        unitID = int.Parse(unitInfoCls.unitID);
                    }
                    catch (System.Exception ex)
                    {
                        MessageBox.Show(ex.Message + ex.Source);
                        return;
                    }

                    if (unitID == m_UnitID)
                    {
                        imageData = unitInfoCls.pictureContent;
                        break;
                    }
                    else
                    {
                        continue;
                    }

                }


                if (imageData == null)
                {
                    return;
                }

                //处理二进制字节数组
                MemoryStream ms = new MemoryStream(imageData);
                Image img = Image.FromStream(ms);

                Form picture = new Form();
                PictureBox pb = new PictureBox();
                pb.Image = img;
                pb.SizeMode = PictureBoxSizeMode.StretchImage;
                pb.Dock = DockStyle.Fill;

                pb.BackColor = Color.White;

                picture.Controls.Add(pb);
                picture.BackColor = Color.White;

                //屏幕宽度
                int width = SystemInformation.WorkingArea.Width;
                //屏幕高度（不包括任务栏）
                int height = SystemInformation.WorkingArea.Height;

                picture.Width = width / 3 * 2;
                picture.Height = height / 3 * 2;
                picture.StartPosition = FormStartPosition.CenterScreen;
                picture.TopLevel = true;
                picture.Text = "单元信息截图";
                picture.Show(this);
            }
        }

        private void buttonClosed_Click(object sender, EventArgs e)
        {
            this.Dispose();
            this.Close();
        }



    }
}
