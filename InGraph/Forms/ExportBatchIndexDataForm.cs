using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using DataProcess;

namespace InGraph.Forms
{
    /// <summary>
    /// 批量导出数据控件类
    /// </summary>
    public partial class ExportBatchIndexDataForm : Form
    {
        WaveformDataProcess wdp = new WaveformDataProcess();
        CITDataProcess citdp = new CITDataProcess();
        string[] listFile;
        public class cMile
        {
            public float fStartMile = 0f;
            public float fEndMile = 0f;
        }
        List<cMile> listMile;
        public ExportBatchIndexDataForm()
        {
            InitializeComponent();
        }

        private void buttonSeeDirectory_Click(object sender, EventArgs e)
        {
           DialogResult dr = folderBrowserDialog1.ShowDialog();
           if (dr == DialogResult.OK)
           {
               textBoxPath1.Text = folderBrowserDialog1.SelectedPath;
               listFile = Directory.GetFiles(textBoxPath1.Text, "*.cit", SearchOption.TopDirectoryOnly);

           }
           else
           {
               textBoxPath1.Text = "";
           }
        }

        private void buttonSelectFile_Click(object sender, EventArgs e)
        {
            DialogResult dr = openFileDialog1.ShowDialog();
            if (dr == DialogResult.OK)
            {
                textBoxMileFile1.Text = openFileDialog1.FileName;
                listMile = new List<cMile>();
                StreamReader sr = new StreamReader(textBoxMileFile1.Text);
                while (sr.Peek() != -1)
                {
                    string[] sStr = sr.ReadLine().Split(',');
                    cMile cM = new cMile();
                    cM.fStartMile = float.Parse(sStr[0]);
                    cM.fEndMile = float.Parse(sStr[1]);
                    listMile.Add(cM);
                }
                sr.Close();
            }
            else
            {
                textBoxMileFile1.Text = "";
            }
        }

        private void buttonBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonExportData_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxMileFile1.Text) || string.IsNullOrEmpty(textBoxPath1.Text)
                || checkedListBox1.CheckedItems.Count == 0)
            {
                MessageBox.Show("请补充完整！");
                return;
            }
            //获取选择通道
            string[] sTQIItem = new string[checkedListBox1.CheckedItems.Count];
            checkedListBox1.CheckedItems.CopyTo(sTQIItem, 0);
            //循环文件
            for (int i = 0; i < listFile.Length; i++)
            {
                string sIndexFile = listFile[i].TrimEnd('.', 'c', 'i', 't') + ".idf";
                if (!File.Exists(sIndexFile))
                {
                    continue;
                }
                List<IndexStaClass> listIC = wdp.GetDataIndexInfo(sIndexFile);
                if (listIC.Count == 0)
                {
                    continue;
                }
                //获取通道序号
                int[] sTQIItemIndex = new int[sTQIItem.Length];
                //获取通道信息
                citdp.QueryDataInfoHead(listFile[i]);
                citdp.QueryDataChannelInfoHead(listFile[i]);
                //给通道绑定序号
                for (int g = 0; g < sTQIItem.Length; g++)
                {

                    for (int h = 0; h < citdp.dciL.Count; h++)
                    {
                        if (sTQIItem[g].Equals(citdp.dciL[h].sNameEn))
                        {
                            sTQIItemIndex[g] = h;
                            break;
                        }
                    }
                }
                //处理数据
                string[] sPara=new string[2];
                StringBuilder sb1 = new StringBuilder();
                for (int k = 0; k < sTQIItemIndex.Length; k++)
                {
                    sb1.Append(sTQIItemIndex[k]+",");
                }
                sPara[0] = sb1.ToString().TrimEnd(',');
                StringBuilder sb2 = new StringBuilder();
                for (int k = 0; k < sTQIItem.Length; k++)
                {
                    sb2.Append(sTQIItem[k] + ",");
                }
                sPara[1] = sb2.ToString().TrimEnd(',');
                string sKmInc = "减";
                if (citdp.dhi.iKmInc == 0)
                {
                    sKmInc = "增";
                }
                bool bEncrypt = false;
                if (citdp.dhi.sDataVersion.StartsWith("3."))
                {
                    bEncrypt = true;
                }
                //循环里程
                for (int j = 0; j < listMile.Count; j++)
                {
                    try
                    {
                        wdp.ExportIndexDataOld(listFile[i], citdp.dhi.iChannelNumber,
                 sKmInc, bEncrypt, listMile[j].fStartMile,
                       listMile[j].fEndMile, sPara);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    Application.DoEvents();
                }
                Application.DoEvents();
            }
            if (listFile.Length == 0)
            {
                MessageBox.Show("选择的目录下(不包含子目录)没有cit文件！");
                return;
            }
            MessageBox.Show("导出成功！");
        }
    }
}
