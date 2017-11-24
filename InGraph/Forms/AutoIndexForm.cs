using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using DataProcess;
using System.Data.OleDb;


namespace InGraph.Forms
{
    public partial class AutoIndexForm : Form
    {
        private String citFilePath = "";

        private List<AutoIndexClass> autoIndexClsList=new List<AutoIndexClass>();

        CITDataProcess cdp = new CITDataProcess();
        WaveformDataProcess wdp = new WaveformDataProcess();


        public AutoIndexForm()
        {
            InitializeComponent();
        }

        private void ReadCIT(String citFilePath)
        {
            if (numericUpDown1.Value <= 0)
            {
                MessageBox.Show("容许跳变值为 0");
                return;
            }

            try
            {
                autoIndexClsList.Clear();
                dataGridView1.Rows.Clear();

                CITDataProcess.DataHeadInfo m_dhi = cdp.GetDataInfoHead(citFilePath);
                List<CITDataProcess.DataChannelInfo> m_dcil = cdp.GetDataChannelInfoHeadNew(citFilePath);


                FileStream fs = new FileStream(citFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                BinaryReader br = new BinaryReader(fs, Encoding.Default);
                br.BaseStream.Position = 0;

                br.ReadBytes(120);


                br.ReadBytes(65 * m_dhi.iChannelNumber);
                br.ReadBytes(BitConverter.ToInt32(br.ReadBytes(4), 0));
                int iChannelNumberSize = m_dhi.iChannelNumber * 2;
                byte[] b = new byte[iChannelNumberSize];

                long milePos = 0;
                int km_pre = 0;
                int meter_pre = 0;
                int km_currrent = 0;
                int meter_current = 0;
                int meter_between = 0;
                int km_index = 0;
                int meter_index = 2;



                long iArray = (br.BaseStream.Length - br.BaseStream.Position) / iChannelNumberSize;

                for (int i = 0; i < iArray; i++)
                {
                    milePos = br.BaseStream.Position;

                    b = br.ReadBytes(iChannelNumberSize);
                    if (m_dhi.sDataVersion.StartsWith("3."))
                    {
                        b = CITDataProcess.ByteXORByte(b);
                    }

                    if (i == 0)
                    {
                        //km_pre = (int)(BitConverter.ToInt16(b, km_index) / m_dcil[km_index].fScale + m_dcil[km_index].fOffset);
                        //meter_pre = (int)(BitConverter.ToInt16(b, meter_index) / m_dcil[meter_index].fScale + m_dcil[meter_index].fOffset);

                        km_pre = (int)(BitConverter.ToInt16(b, km_index));
                        meter_pre = (int)(BitConverter.ToInt16(b, meter_index));
                    }
                    else
                    {
                        //km_currrent = (int)(BitConverter.ToInt16(b, km_index) / m_dcil[km_index].fScale + m_dcil[km_index].fOffset);
                        //meter_current = (int)(BitConverter.ToInt16(b, meter_index) / m_dcil[meter_index].fScale + m_dcil[meter_index].fOffset);

                        km_currrent = (int)(BitConverter.ToInt16(b, km_index));
                        meter_current = (int)(BitConverter.ToInt16(b, meter_index));

                        meter_between = (km_currrent - km_pre) * 4000 + (meter_current - meter_pre);

                        if (Math.Abs(meter_between) > numericUpDown1.Value)
                        {
                            AutoIndexClass autoIndexCls = new AutoIndexClass();
                            autoIndexCls.milePos = milePos;
                            autoIndexCls.km_current = km_currrent;
                            autoIndexCls.meter_current = meter_current;
                            autoIndexCls.km_pre = km_pre;
                            autoIndexCls.meter_pre = meter_pre;
                            autoIndexCls.meter_between = meter_between;

                            autoIndexClsList.Add(autoIndexCls);
                        }

                        km_pre = km_currrent;
                        meter_pre = meter_current;

                    }

                }

                br.Close();
                fs.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void Display(List<AutoIndexClass> autoIndexClsList)
        {
            if (autoIndexClsList == null || autoIndexClsList.Count == 0)
            {
                return;
            }

            int i = 0;
            dataGridView1.Rows.Clear();
            foreach (AutoIndexClass autoIndexCls in autoIndexClsList)
            {
                DataGridViewRow dgvr = new DataGridViewRow();
                dgvr.CreateCells(dataGridView1);
                dgvr.Cells[0].Value = ++i;
                dgvr.Cells[1].Value = autoIndexCls.milePos;
                dgvr.Cells[2].Value = autoIndexCls.km_current;
                dgvr.Cells[3].Value = autoIndexCls.meter_current;
                dgvr.Cells[4].Value = autoIndexCls.km_pre;
                dgvr.Cells[5].Value = autoIndexCls.meter_pre;
                dgvr.Cells[6].Value = autoIndexCls.meter_between;

                dataGridView1.Rows.Add(dgvr);
            }

            Boolean isKmInc = true; // 是否是增里程
            if (autoIndexClsList.Count < 2)
            {
                return;
            }
            if ((int)(dataGridView1.Rows[0].Cells[2].Value) > (int)(dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[2].Value))
            {
                isKmInc = false;
            }

            for (int j = 1; j < dataGridView1.Rows.Count;j++ )
            {
                int km_pre = (int)(dataGridView1.Rows[j-1].Cells[2].Value);
                int meter_pre = (int)(dataGridView1.Rows[j-1].Cells[3].Value);
                int km_current = (int)(dataGridView1.Rows[j].Cells[2].Value);
                int meter_current = (int)(dataGridView1.Rows[j].Cells[3].Value);

                int point_between = (km_current - km_pre) * 4000 + (meter_current - meter_pre);
                
                //增里程时，里程突然变小
                if (point_between <= 0 && isKmInc == true)
                {
                    dataGridView1.Rows[j].DefaultCellStyle.BackColor = Color.DeepSkyBlue;
                    dataGridView1.Rows[j].DefaultCellStyle.ForeColor = Color.White;
                }
                //减里程时，里程突然变大
                if (point_between >= 0 && isKmInc == false)
                {
                    dataGridView1.Rows[j].DefaultCellStyle.BackColor = Color.DeepSkyBlue;
                    dataGridView1.Rows[j].DefaultCellStyle.ForeColor = Color.White;
                }
            }
        }
        private void WriteIdf(String idfFilePath)
        {
            //long m_RecordNumber = 0;

            //try
            //{
            //    using (OleDbConnection sqlconn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + idfFilePath + ";Persist Security Info=True"))
            //    {

            //        String sSQL = "select max(Id) from IndexOri";
            //        OleDbCommand sqlcom = new OleDbCommand(sSQL, sqlconn);

            //        sqlconn.Open();
            //        OleDbDataReader sdr = sqlcom.ExecuteReader();

            //        while (sdr.Read())
            //        {
            //            m_RecordNumber = long.Parse(sdr.GetValue(0).ToString());
            //        }
            //        sdr.Close();
            //        sqlconn.Close();
            //    }
            //}
            //catch (System.Exception ex)
            //{
            //    MessageBox.Show("查询IndexOri表中的最大值出错：" + "\n" + ex.Source);
            //    return;
            //}

            wdp.deleteLayerIndexInfo(idfFilePath);

            try
            {
                using (OleDbConnection sqlconn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + idfFilePath + ";Persist Security Info=True"))
                {
                    sqlconn.Open();

                    for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    {
                        double mile = (int)dataGridView1.Rows[i].Cells[2].Value + (int)(dataGridView1.Rows[i].Cells[3].Value) / 4000f;
                        string sSql = String.Format("insert into {0} (Id,IndexId,IndexPoint,IndexMeter) values( {1},{2},'{3}','{4}')",
                                                    "IndexOri",
                                                    (i + 1),
                                                    0,
                                                    dataGridView1.Rows[i].Cells[1].Value,
                                                    mile
                                                    );

                        //string sSql = String.Format("insert into {0} (Id,IndexId,IndexPoint,IndexMeter) values( {1},{2},'{3}','{4}')",
                        //    "IndexOri",
                        //    (i + 1),
                        //    0,
                        //    (int)(dataGridView1.Rows[i].Cells[1].Value),
                        //    mile
                        //    );

                        OleDbCommand sqlcom = new OleDbCommand(sSql, sqlconn);
                        sqlcom.ExecuteNonQuery();
                    }

                    sqlconn.Close();

                    MessageBox.Show("共写入" + dataGridView1.Rows.Count+"条记录");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                MessageBox.Show("插入IndexOri表出错");
            }
        }



        private void AutoIndexForm_Load(object sender, EventArgs e)
        {

        }

        private void buttonSelectCIT_Click(object sender, EventArgs e)
        {
            DialogResult dr = openFileDialog1.ShowDialog();
            if (dr == DialogResult.OK)
            {
                citFilePath = openFileDialog1.FileName.ToLower();

                toolStripStatusLabel2.Text = citFilePath;
                toolStripStatusLabel2.Width = statusStrip1.Width - toolStripStatusLabel1.Width;

                buttonReadCIT.Enabled = true;
                buttonWriteInf.Enabled = true;

            }
            else
            {
                citFilePath = "";
            }
        }

        private void buttonReadCIT_Click(object sender, EventArgs e)
        {
            ReadCIT(citFilePath);
            Display(autoIndexClsList);
        }

        private void buttonQuit_Click(object sender, EventArgs e)
        {
            this.Dispose();
            this.Close();
        }

        private void buttonWriteInf_Click(object sender, EventArgs e)
        {
            String idfFileName = Path.GetFileNameWithoutExtension(citFilePath) + ".idf";

            String idfFilePath = Path.Combine(Path.GetDirectoryName(citFilePath),idfFileName);

            if (!File.Exists(idfFilePath))
            {
                MessageBox.Show("找不到波形附加文件！");
                return;
            }

            WriteIdf(idfFilePath);
        }
    }

    public class AutoIndexClass
    {
        public long milePos;
        public int km_current;
        public int meter_current;
        public int km_pre;
        public int meter_pre;
        public int meter_between;

        public float MileCurrent
        {
            get
            {
                return km_current + meter_current / 4;
            }            
        }
    }
}
