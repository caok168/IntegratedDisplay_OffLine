using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using InGraph.Classes;
using System.IO;

namespace InGraph.Forms
{
    /// <summary>
    /// 导出当前屏幕--控件类
    /// </summary>
    public partial class ExportWaveDataForm : Form
    {
        public ExportWaveDataForm()
        {
            InitializeComponent();
        }

        private void ExportWaveDataForm_Load(object sender, EventArgs e)
        {
            
            textBoxMileRange.Text = "KM"+CommonClass.listDIC[0].listMeter[0].Km.ToString()+
                "+" + (CommonClass.listDIC[0].listMeter[0].Meter / CommonClass.listDIC[0].fScale[1]).ToString() + "M" +
                 " - KM" + 
                 CommonClass.listDIC[0].listMeter[CommonClass.listDIC[0].listMeter.Count-1].Km.ToString() +
                "+" + (CommonClass.listDIC[0].listMeter[CommonClass.listDIC[0].listMeter.Count - 1].Meter / CommonClass.listDIC[0].fScale[1]).ToString() + "M";
            for (int i = 0; i < CommonClass.listDIC.Count; i++)
            {
                checkedListBox1.Items.Add(CommonClass.listDIC[i].Name);
            }
        }

        private void buttonExportData_Click(object sender, EventArgs e)
        {
            if (PasswordTextBox1.Text.Equals("19491001"))
            {
                //开始导出
                this.Enabled = false;
                Application.DoEvents();
                try
                {
                    for (int i = 0; i < checkedListBox1.Items.Count; i++)
                    {
                        if (checkedListBox1.GetItemChecked(i))
                        {
                            ExportData(i);
                        }
                    }

                    Application.DoEvents();
                    this.Enabled = true;
                    MessageBox.Show("导出成功！");
                }
                catch(Exception ex)
                {
                    Application.DoEvents();
                    this.Enabled = true;
                    MessageBox.Show("导出失败:" + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("请检查验证码是否正确!");
            }
        }

        private void ExportData(int iIndex)
        {
            if (!CommonClass.sExportWaveDataPath.EndsWith("\\"))
            {
                CommonClass.sExportWaveDataPath += "\\";
            }
            StreamWriter sw = new StreamWriter(CommonClass.sExportWaveDataPath+
                Path.GetFileNameWithoutExtension(CommonClass.listDIC[iIndex].sFilePath)+"_"+textBoxMileRange.Text+".txt",false,Encoding.Default);
            sw.AutoFlush = true;

            //写字段名
            StringBuilder sbName = new StringBuilder();
            sbName.Append("KM,Meter");
            for (int i = 0; i < CommonClass.listDIC[iIndex].listCC.Count; i++)
            {
                if (CommonClass.listDIC[iIndex].listCC[i].Visible)
                {
                    sbName.Append("," + CommonClass.listDIC[iIndex].listCC[i].ChineseName);
                }
            }
            sw.WriteLine(sbName.ToString());

            //写数据
            for (int j = 0; j < CommonClass.listDIC[0].listMeter.Count; j++)
            {
                StringBuilder sbData = new StringBuilder();
                sbData.Append(CommonClass.listDIC[0].listMeter[j].Km.ToString()+","+
                    (CommonClass.listDIC[0].listMeter[j].Meter / (CommonClass.listDIC[0].bIndex?1:CommonClass.listDIC[0].fScale[1])).ToString());
                for (int i = 0; i < CommonClass.listDIC[iIndex].listCC.Count; i++)
                {
                    if (CommonClass.listDIC[iIndex].listCC[i].Visible)
                    {
                        sbData.Append("," + (CommonClass.listDIC[iIndex].listCC[i].Data[j]
                            / CommonClass.listDIC[iIndex].fScale[CommonClass.listDIC[iIndex].listCC[i].Id]).ToString());
                    }
                }
                sw.WriteLine(sbData.ToString());
            }

            sw.Close();
        }

        private void toolStripButtonSelectAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                checkedListBox1.SetItemChecked(i, true);
            }
        }

        private void toolStripButtonSelectNone_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                checkedListBox1.SetItemChecked(i, false);
            }
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
