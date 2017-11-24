using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
//using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using DataProcess;

using MathWorks.MATLAB.NET.Arrays;
using MathWorks.MATLAB.NET.Utility;
using PreProcessing;

namespace InvalidDataProcessing
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void BroButton1_Click(object sender, EventArgs e)
        {
            folderBrowserDialog.SelectedPath = CommonClassIDP.sLastSelectPath;
            DialogResult dr = folderBrowserDialog.ShowDialog();
            if (dr == DialogResult.OK)
            {
                CommonClassIDP.sLastSelectPath = folderBrowserDialog.SelectedPath;
                GetDataPre();
            }
        }

        private void GetDataPre()
        {

            FilesListView1.Enabled = false;
            InfoLabel1.Visible = true;
            Application.DoEvents();
            GetData();
            Application.DoEvents();
            FilesListView1.Enabled = true;
            InfoLabel1.Visible = false;
        }

        private void GetData()
        {
            PathTextBox1.Text = "";
            PathTextBox1.Text = CommonClassIDP.sLastSelectPath;
            FilesListView1.Items.Clear();
            string[] sFiles;
            try
            {
                sFiles = Directory.GetFiles(PathTextBox1.Text, "*.cit", IncludeSubFolderCheckBox1.Checked ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
            }
            catch
            {
                MessageBox.Show("获取所有文件出错，请检查子目录");
                return;
            }
            FilesListView1.BeginUpdate();
            foreach (string v in sFiles)
            {

                string status = CommonClassIDP.cdp.QueryDataInfoHead(v);
                string[] sDataInfoHead = status.Split(new char[] { ',' });
                //iDataType; sDataVersion; sTrackCode; sTrackName; iDir; 
                //sTrain; sDate; sTime; iRunDir;iKmInc; 
                //fkmFrom; fkmTo; iSmaleRate; iChannelNumber;
                if (sDataInfoHead[0].Contains("0"))/**/
                {
                    FilesListView1.Items.Add(sDataInfoHead[4]);
                    FilesListView1.Items[FilesListView1.Items.Count - 1].Name = v;

                    FilesListView1.Items[FilesListView1.Items.Count - 1].SubItems.Add(sDataInfoHead[3]);
                    FilesListView1.Items[FilesListView1.Items.Count - 1].SubItems.Add(sDataInfoHead[5]);
                    FilesListView1.Items[FilesListView1.Items.Count - 1].SubItems.Add(sDataInfoHead[10]);
                    FilesListView1.Items[FilesListView1.Items.Count - 1].SubItems.Add(sDataInfoHead[7]);
                    FilesListView1.Items[FilesListView1.Items.Count - 1].SubItems.Add(sDataInfoHead[8]);
                    FilesListView1.Items[FilesListView1.Items.Count - 1].SubItems.Add(sDataInfoHead[6]);
                    FilesListView1.Items[FilesListView1.Items.Count - 1].SubItems.Add(sDataInfoHead[14]);
                    FilesListView1.Items[FilesListView1.Items.Count - 1].SubItems.Add(Path.GetFileName(v));
                    FilesListView1.Items[FilesListView1.Items.Count - 1].SubItems.Add((new FileInfo(v)).Length.ToString());
                    FilesListView1.Items[FilesListView1.Items.Count - 1].SubItems.Add(Path.GetDirectoryName(v));
                }
                Application.DoEvents();

            }
            FilesListView1.EndUpdate();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            CommonClassIDP.cdp = new CITDataProcess();
            CommonClassIDP.wdp = new WaveformDataProcess();
            CommonClassIDP.ppc = new PreProcessClass();
        }

        private void OpenButton1_Click(object sender, EventArgs e)
        {
            if (FilesListView1.CheckedIndices.Count < 1 || FilesListView1.CheckedIndices.Count > 10)
            {
                return;
            }

            try
            {
                string sFileName = "";
                string sAddFileName = "";
                for (int i = 0; i < FilesListView1.CheckedItems.Count; i++)
                {
                    if (FilesListView1.CheckedItems[i].SubItems[10].Text.EndsWith("\\"))
                    {
                        sFileName = FilesListView1.CheckedItems[i].SubItems[10].Text + FilesListView1.CheckedItems[i].SubItems[8].Text;
                        sAddFileName = FilesListView1.CheckedItems[i].SubItems[10].Text + Path.GetFileNameWithoutExtension(FilesListView1.CheckedItems[i].SubItems[8].Text) + ".idf";
                    }
                    else
                    {
                        sFileName = FilesListView1.CheckedItems[i].SubItems[10].Text + "\\" + FilesListView1.CheckedItems[i].SubItems[8].Text;
                        sAddFileName = FilesListView1.CheckedItems[i].SubItems[10].Text + "\\" + Path.GetFileNameWithoutExtension(FilesListView1.CheckedItems[i].SubItems[8].Text) + ".idf";
                    }
                    GetDataInfo(sFileName, sAddFileName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        public class PointIDX
        {
            public double s = 0;
            public double e = 0;
        }
        #region 获取数据信息
        public bool GetDataInfo(string FileName, string sAddFileName)
        {
            string EnName = "";
            string CnName = "速度";
            try
            {
                //根据信息读取里程及数据信息
                CommonClassIDP.cdp.QueryDataInfoHead(FileName);
                double[] tt = CommonClassIDP.cdp.GetSingleChannelData(FileName, GetChannelNumberByChannelName(FileName, "Km", "公里"));
                double[] wvelo = CommonClassIDP.cdp.GetSingleChannelData(FileName, GetChannelNumberByChannelName(FileName, "Speed", "速度"));
                double[] Short_Twist = CommonClassIDP.cdp.GetSingleChannelData(FileName, GetChannelNumberByChannelName(FileName, "Short_Twist", "三角坑"));
                double[] L_Prof_SC = CommonClassIDP.cdp.GetSingleChannelData(FileName, GetChannelNumberByChannelName(FileName, "L_Prof_SC", "左高低_中波"));
                double[] R_Prof_SC = CommonClassIDP.cdp.GetSingleChannelData(FileName, GetChannelNumberByChannelName(FileName, "R_Prof_SC", "右高低_中波"));
                double[] L_Align_SC = CommonClassIDP.cdp.GetSingleChannelData(FileName, GetChannelNumberByChannelName(FileName, "L_Align_SC", "左轨向_中波"));
                double[] R_Align_SC = CommonClassIDP.cdp.GetSingleChannelData(FileName, GetChannelNumberByChannelName(FileName, "R_Align_SC", "右轨向_中波"));
                double[] Gage = CommonClassIDP.cdp.GetSingleChannelData(FileName, GetChannelNumberByChannelName(FileName, "Gage", "轨距"));
                double[] Crosslevel = CommonClassIDP.cdp.GetSingleChannelData(FileName, GetChannelNumberByChannelName(FileName, "Crosslevel", "水平"));
                //调用刘博士的算法
                preProcess(tt, Short_Twist, wvelo, FileName, sAddFileName, "Short_Twist");
                preProcess(tt, L_Prof_SC, wvelo, FileName, sAddFileName, "L_Prof_SC");
                preProcess(tt, R_Prof_SC, wvelo, FileName, sAddFileName, "R_Prof_SC");
                preProcess(tt, L_Align_SC, wvelo, FileName, sAddFileName, "L_Align_SC");
                preProcess(tt, R_Align_SC, wvelo, FileName, sAddFileName, "R_Align_SC");
                preProcess(tt, Gage, wvelo, FileName, sAddFileName, "Gage");
                preProcess(tt, Crosslevel, wvelo, FileName, sAddFileName, "Crosslevel");

                MessageBox.Show("处理完成");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
            }
            return true;
        }
        #endregion

        public bool preProcess(double[] tt, double[] wx, double[] wvelo, string FileName, string sAddFileName, string swx)
        {
            try
            {
                List<PointIDX> Lidx = new List<PointIDX>();
                //调用刘博士的算法，得到索引数组idx
                int oneTimeLength = 1000000; //一次处理的点数
                
                for (int i = 0; i < tt.Length; i += oneTimeLength)
                {
                    int remain = 0;
                    int index = (i / oneTimeLength) * oneTimeLength;
                    remain = tt.Length - oneTimeLength * (i / oneTimeLength + 1);
                    int ThisTimeLength = remain > 0 ? oneTimeLength : (remain += oneTimeLength);
                    double[] tmpCh1 = new double[ThisTimeLength];
                    double[] tmpCh2 = new double[ThisTimeLength];
                    double[] tmpCh3 = new double[ThisTimeLength];

                    for (int j = 0; j < ThisTimeLength; j++)
                    {
                        tmpCh1[j] = tt[index + j];
                        tmpCh2[j] = wx[index + j];
                        tmpCh3[j] = wvelo[index + j];
                    }

                    MWNumericArray dtt = new MWNumericArray(tmpCh1);
                    MWNumericArray dwx = new MWNumericArray(tmpCh2);
                    MWNumericArray dwvelo = new MWNumericArray(tmpCh3);
                    //调用算法
                    MWNumericArray resultArrayAB = (MWNumericArray)CommonClassIDP.ppc.sub_identify_abnormal_point(dtt, dwx, dwvelo);
                    double[,] result = (double[,])resultArrayAB.ToArray();
                    if (result.GetLength(1) == 0) continue;
                    Lidx.Clear();
                    for (int m = 0; m < result.GetLength(1); m++)
                    {
                        PointIDX pi = new PointIDX();
                        pi.s = result[0, m] + index;
                        pi.e = result[1, m] + index;
                        Lidx.Add(pi);
                    }

                    InfoLabel2.Visible = true;
                    int k = 0;
                    //按对处理索引数组
                    List<PointIDX>.Enumerator listCredentials = Lidx.GetEnumerator();
                    while (listCredentials.MoveNext())
                    {
                        InfoLabel2.Text = swx +": i=" + i + " ,length=" + tt.Length + "\n正在写入第" + k + "条，共" + Lidx.Count + "条";
                        InfoLabel2.Refresh();
                        k++;
                        //根据索引值获取对应的文件指针。
                        double sPox = CommonClassIDP.cdp.GetPosByIdx(FileName, listCredentials.Current.s);
                        double ePox = CommonClassIDP.cdp.GetPosByIdx(FileName, listCredentials.Current.e);
                        //根据文件指针，获取里程信息。
                        double smile = CommonClassIDP.cdp.GetMileByPos(sAddFileName, FileName, sPox);
                        double emile = CommonClassIDP.cdp.GetMileByPos(sAddFileName, FileName, ePox);
                        //调用InvalidDataInsertInto，写idf文件。
                        CommonClassIDP.wdp.InvalidDataInsertInto(sAddFileName, sPox.ToString(), ePox.ToString(), toKM(smile), toKM(emile), 6, "无效滤除算法", swx);
                    }
                }
                InfoLabel2.Visible = false;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
            return true;
        }

        public string toKM(double m)
        {
            string meter = "";
            string[] ms = m.ToString().Split(new char[] {'.'});
            string km = ms[0];
            long lm = long.Parse(ms[1]);
            if (lm > 1000)
            {
                meter = lm.ToString().Substring(0, 3) + "." + lm.ToString().Substring(3, lm.ToString().Length - 3);
            }
            else
            {
                meter = lm.ToString();
            }
            if (meter.Length > 8)
                return "K" + km + "+" + meter.Substring(0, 8);
            else
                return "K" + km + "+" + meter;
        }
        public double[,] test(double[] a)
        {
            double[,] idx = new double[2,2];
            idx[0,0] = 100000;
            idx[1,0] = 200000;
            idx[0, 1] = 300000;
            idx[1, 1] = 400000;

            return idx;
        }

        private int GetChannelNumberByChannelName(string FileName, string EnName, string CnName)
        {
            List<CITDataProcess.DataChannelInfo> dci = CommonClassIDP.cdp.GetDataChannelInfoHead(FileName);
            int ChannelNumber = -1;
            for (int i = 0; i < dci.Count; i++)
            {
                if ((dci[i].sNameEn.Equals(EnName) && (EnName != "")) || (dci[i].sNameCh.Equals(CnName)) && (CnName != ""))
                {
                    ChannelNumber = i + 1;
                    break;
                }
            }
            return ChannelNumber;
        }

        private void ListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if(FilesListView1.SelectedIndices != null && FilesListView1.SelectedIndices.Count > 0)
            //{
            //    ListView.SelectedIndexCollection c = FilesListView1.SelectedIndices;
            //    MessageBox.Show(FilesListView1.Items[c[0]].SubItems[2].Text);
            //}
            checkedListBox1.Items.Clear();
            if (FilesListView1.SelectedItems.Count == 1)
            {
                //string ChannelNames = CommonClass.cdp.QueryDataChannelInfoHead(FilesListView1.SelectedItems[0].Name);
                List<CITDataProcess.DataChannelInfo> dci = CommonClassIDP.cdp.GetDataChannelInfoHead(FilesListView1.SelectedItems[0].Name);
                //string [] ChannelName = ChannelNames.Split(new char[] { ',' });
                for(int i = 0; i < dci.Count; i ++)
                {
                    checkedListBox1.Items.Add(dci[i].sNameCh + "(" + dci[i].sNameEn + ")");
                }
            }
            return;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                for (int j = 0; j < checkedListBox1.Items.Count; j++)
                    checkedListBox1.SetItemChecked(j, true);
            }
            else
            {
                for (int j = 0; j < checkedListBox1.Items.Count; j++)
                    checkedListBox1.SetItemChecked(j, false);
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            for (int j = 0; j < checkedListBox1.Items.Count; j++)
            {
                if (checkedListBox1.GetItemChecked(j))
                {
                    checkedListBox1.SetItemChecked(j, false);
                }
                else
                {
                    checkedListBox1.SetItemChecked(j, true);
                }
            }
        }
    }
}
