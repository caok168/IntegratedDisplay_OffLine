using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using InGraph.Classes;
using System.Windows.Forms;
using System.Reflection;
using System.IO;

namespace InGraph
{
    public partial class MeterageForm : Form
    {
        public MeterageForm()
        {
            InitializeComponent();
        }

        private void ReturnButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void MeterageForm_Load(object sender, EventArgs e)
        {
            
            MeterageDataGridView1.Rows.Clear();

          try
            {  
                string sStr = this.Tag.ToString();
                string[] sSplit = sStr.Split(new char[] { ',' });
                Point p = new Point();
                p.X = int.Parse(sSplit[1]);
                int iDPointX = p.X;
                p.Y = int.Parse(sSplit[2]);
                int iWidth = int.Parse(sSplit[3]);
                int iChecked = int.Parse(sSplit[0]);
                if (p.X < 0)
                {
                    p.X = 0;
                }
                switch (iChecked)
                {
                        //name
                    case 1:
                        for (int i = 0; i < CommonClass.listDIC.Count; i++)
                        {
                            for (int j = 0; j < CommonClass.listDIC[i].listCC.Count; j++)
                            {
                                if (CommonClass.listDIC[i].listCC[j].Visible &&
                                    CommonClass.listDIC[i].listCC[j].Rect.Contains(CommonClass.listDIC[i].listCC[j].Rect.X + 1, p.Y - CommonClass.CoordinatesHeight))
                                {
                                    //string sName = CommonClass.listDIC[i].listCC[j].Name;
                                    //for (int k = 0; k < CommonClass.listDIC.Count; k++)
                                    //{
                                    //    for (int g = 0; g < CommonClass.listDIC[k].listCC.Count; g++)
                                    //    {
                                    //        if (CommonClass.listDIC[k].listCC[g].Visible && CommonClass.listDIC[k].listCC[g].Name.Equals(sName))
                                    //        {
                                    iDPointX = ((int)(p.X / (iWidth / 1.0 / CommonClass.listDIC[i].listMeter.Count)));

                                    if (iDPointX >= CommonClass.listDIC[i].listMeter.Count)
                                    {
                                        iDPointX = CommonClass.listDIC[i].listMeter.Count - 1;
                                    }
                                    iDPointX = GetMeterArea(iDPointX, i, j);

                                    object[] o = new object[6];
                                    o[0] = i + 1;
                                    o[1] = CommonClass.listDIC[i].sDate;
                                    o[2] = CommonClass.listDIC[i].sDir;
                                    o[3] = CommonClass.listDIC[i].listCC[j].ChineseName;
                                    o[4] = "K" + CommonClass.listDIC[i].listMeter[iDPointX].Km.ToString() + "+" + (Math.Round(CommonClass.listDIC[i].listMeter[iDPointX].Meter / (CommonClass.listDIC[i].bIndex ? 1 : CommonClass.listDIC[i].fScale[1]), 2)).ToString();

                                    if (CommonClass.listDIC[i].listCC[j].MeaOffset)
                                    {
                                        o[5] = CommonClass.listDIC[i].listCC[j].Data[iDPointX] / CommonClass.listDIC[i].fScale[CommonClass.listDIC[i].listCC[j].Id] + CommonClass.listDIC[i].fOffset[CommonClass.listDIC[i].listCC[j].Id];
                                    }
                                    else
                                    {
                                        o[5] = CommonClass.listDIC[i].listCC[j].Data[iDPointX] / CommonClass.listDIC[i].fScale[CommonClass.listDIC[i].listCC[j].Id];
                                    }

                                    MeterageDataGridView1.Rows.Add(o);

                                                
                                    //        }
                                    //    }
                                    //}
                                    //return;
                                }
                            }
                        }
                        break;
                        //baseline
                    case 2:
                        for (int i = 0; i < CommonClass.listDIC.Count; i++)
                        {
                            for (int j = 0; j < CommonClass.listDIC[i].listCC.Count; j++)
                            {
                                if (CommonClass.listDIC[i].listCC[j].Visible &&
                                    CommonClass.listDIC[i].listCC[j].Rect.Contains(CommonClass.listDIC[i].listCC[j].Rect.X + 1, p.Y - CommonClass.CoordinatesHeight))
                                {
                                    iDPointX = ((int)(p.X / (iWidth / 1.0 / CommonClass.listDIC[i].listMeter.Count)));
                                    if (iDPointX >= CommonClass.listDIC[i].listMeter.Count)
                                    {
                                        iDPointX = CommonClass.listDIC[i].listMeter.Count - 1;
                                    }
                                    iDPointX = GetMeterArea(iDPointX, i, j);

                                    object[] o = new object[6];
                                    o[0] = i + 1;
                                    o[1] = CommonClass.listDIC[i].sDate;
                                    o[2] = CommonClass.listDIC[i].sDir;

                                    o[3] = CommonClass.listDIC[i].listCC[j].ChineseName;

                                    o[4] = "K" + CommonClass.listDIC[i].listMeter[iDPointX].Km.ToString() + "+" + (Math.Round(CommonClass.listDIC[i].listMeter[iDPointX].Meter / (CommonClass.listDIC[i].bIndex ? 1 : CommonClass.listDIC[i].fScale[1]),2)).ToString();

                                    if (CommonClass.listDIC[i].listCC[j].MeaOffset)
                                    {
                                        o[5] = CommonClass.listDIC[i].listCC[j].Data[iDPointX] / CommonClass.listDIC[i].fScale[CommonClass.listDIC[i].listCC[j].Id] + CommonClass.listDIC[i].fOffset[CommonClass.listDIC[i].listCC[j].Id];
                                    } 
                                    else
                                    {
                                        o[5] = CommonClass.listDIC[i].listCC[j].Data[iDPointX] / CommonClass.listDIC[i].fScale[CommonClass.listDIC[i].listCC[j].Id];
                                    }

                                    
                                    MeterageDataGridView1.Rows.Add(o);                 
                                }
                            }
                        }
                        break;
                        //layer
                    case 3:
                        for (int i = 0; i < CommonClass.listDIC.Count; i++)
                        {

                            for (int k = 0; k < CommonClass.listDIC[i].listCC.Count; k++)
                            {
                                if (CommonClass.listDIC[i].listCC[k].Visible)
                                {
                                    iDPointX = ((int)(p.X / (iWidth / 1.0 / CommonClass.listDIC[i].listMeter.Count)));
                                    if (iDPointX >= CommonClass.listDIC[i].listMeter.Count)
                                    {
                                        iDPointX = CommonClass.listDIC[i].listMeter.Count - 1;
                                    }
                                    iDPointX = GetMeterArea(iDPointX, i, k);

                                    object[] o = new object[6];
                                    o[0] = i + 1;
                                    o[1] = CommonClass.listDIC[i].sDate;
                                    o[2] = CommonClass.listDIC[i].sDir;

                                    o[3] = CommonClass.listDIC[i].listCC[k].ChineseName;

                                    o[4] = "K" + CommonClass.listDIC[i].listMeter[iDPointX].Km.ToString() + "+" + (Math.Round(CommonClass.listDIC[i].listMeter[iDPointX].Meter / (CommonClass.listDIC[i].bIndex ? 1 : CommonClass.listDIC[i].fScale[1]), 2)).ToString();

                                    if (CommonClass.listDIC[i].listCC[k].MeaOffset)
                                    {
                                        o[5] = CommonClass.listDIC[i].listCC[k].Data[iDPointX] / CommonClass.listDIC[i].fScale[CommonClass.listDIC[i].listCC[k].Id] + CommonClass.listDIC[i].fOffset[CommonClass.listDIC[i].listCC[k].Id];
                                    }
                                    else
                                    {
                                        o[5] = CommonClass.listDIC[i].listCC[k].Data[iDPointX] / CommonClass.listDIC[i].fScale[CommonClass.listDIC[i].listCC[k].Id];
                                    }



                                    MeterageDataGridView1.Rows.Add(o);

                                }
                            }
                        }
                        break;
                        //断面线测量
                    case 4:
                        for (int i = 0; i < CommonClass.listDIC.Count; i++)
                        {
                            for (int k = 0; k < CommonClass.listDIC[i].listCC.Count; k++)
                            {
                                if (CommonClass.listDIC[i].listCC[k].Visible)
                                {
                                    iDPointX = ((int)(p.X / (iWidth / 1.0 / (CommonClass.listDIC[i].listMeter.Count - 1))));
                                    if (iDPointX >= CommonClass.listDIC[i].listMeter.Count)
                                    {
                                        iDPointX = CommonClass.listDIC[i].listMeter.Count - 1;
                                    }

                                    object[] o = new object[6];
                                    o[0] = i + 1;
                                    o[1] = CommonClass.listDIC[i].sDate;
                                    o[2] = CommonClass.listDIC[i].sDir;
                                    o[3] = CommonClass.listDIC[i].listCC[k].ChineseName;

                                    o[4] = "K" + CommonClass.listDIC[i].listMeter[iDPointX].Km.ToString() + "+" + (Math.Round(CommonClass.listDIC[i].listMeter[iDPointX].Meter / (CommonClass.listDIC[i].bIndex ? 1 : CommonClass.listDIC[i].fScale[1]), 2)).ToString();

                                    if (CommonClass.listDIC[i].listCC[k].MeaOffset)
                                    {
                                        o[5] = CommonClass.listDIC[i].listCC[k].Data[iDPointX] / CommonClass.listDIC[i].fScale[CommonClass.listDIC[i].listCC[k].Id] + CommonClass.listDIC[i].fOffset[CommonClass.listDIC[i].listCC[k].Id];
                                    }
                                    else
                                    {
                                        o[5] = CommonClass.listDIC[i].listCC[k].Data[iDPointX] / CommonClass.listDIC[i].fScale[CommonClass.listDIC[i].listCC[k].Id];
                                    }


                                    MeterageDataGridView1.Rows.Add(o);

                                }
                            }
                        }
                        break;
                }
            }
          catch (Exception ex)
          {
              MessageBox.Show("里程不存在,请重新生成波形通道配置文件!\n错误描述" + ex.Message);
              this.Close();
          }
        }
        private int GetMeterArea(int iDPointX,int k,int g)
        {
            int i = iDPointX - CommonClass.MeterageRadius*4;
            int j = iDPointX + CommonClass.MeterageRadius*4;
            if (i < 0)
            {
                i = 0;
            }
            if (j >= CommonClass.listDIC[k].listMeter.Count)
            {
                j = CommonClass.listDIC[k].listMeter.Count - 1;
            }
            float iValue = 0f;
            for (; i < j; i++)
            {
                if (Math.Abs(CommonClass.listDIC[k].listCC[g].Data[i]) > iValue)
                {
                    iValue = Math.Abs(CommonClass.listDIC[k].listCC[g].Data[i]);
                    iDPointX = i;
                }
            }
                // iDPointX = ((int)(p.X / (iWidth / 1.0 /CommonClass.listDIC[k].Meter.Length)));
            return iDPointX;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            String citPath = Path.GetDirectoryName(CommonClass.listDIC[0].sFilePath);
            String meterageDataFilePath = Path.Combine(citPath, "测量结果.csv");

            ExportDataFromDataGridView(MeterageDataGridView1, meterageDataFilePath);
        }

        private void ExportDataFromDataGridView(DataGridView dgv, String csvFilePath)
        {
            try
            {
                StreamWriter sw = new StreamWriter(csvFilePath, false, Encoding.Default);
                StringBuilder sb = new StringBuilder();
                sb.Append("序号");
                for (int i = 0; i < dgv.Columns.Count; i++)
                {
                    sb.Append("," + dgv.Columns[i].HeaderText);
                }
                sw.WriteLine(sb.ToString());
                sw.AutoFlush = true;
                for (int i = 0; i < dgv.Rows.Count; i++)
                {
                    sb = new StringBuilder();
                    sb.Append((i + 1).ToString());
                    for (int j = 0; j < dgv.Rows[i].Cells.Count; j++)
                    {
                        sb.Append("," + dgv.Rows[i].Cells[j].Value.ToString());
                    }
                    sw.WriteLine(sb.ToString());
                }

                sw.Close();
                MessageBox.Show("导出成功！");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}