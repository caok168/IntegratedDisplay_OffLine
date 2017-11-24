using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using InGraph.Classes;
using System.Collections;
using System.Globalization;
using System.Net.Sockets;
using System.Threading;
using System.Data.OleDb;

namespace InGraph.Forms
{
    public partial class OpenDataDialog : Form
    {
        delegate void SetTextCallbackFilesListView1(string[] text);
        delegate void SetEnabledCallbackFilesListView1(bool text);
        delegate void SetVisibleCallbackInfoLabel1(bool text);
        delegate void SetUpdateCallbackFilesListView1(bool text);

        private bool bStartup = false;
        public OpenDataDialog()
        {
            InitializeComponent();

            columnHeader_Date.Name = "检测日期";
            columnHeader_Directory.Name = "原始路径";
            columnHeader_FileName.Name = "原始文件名";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.SelectedPath = CommonClass.sLastSelectPath;
            DialogResult dr = folderBrowserDialog1.ShowDialog();
            if (dr == DialogResult.OK)
            {
                CommonClass.sLastSelectPath = folderBrowserDialog1.SelectedPath;
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
            //单机版的新增了一列：方向。因此这里要把这列删除
            if (comboBox1.SelectedIndex == 1)
            {
                if (FilesListView1.Columns.Contains(columnHeader11))
                {
                    FilesListView1.Columns.Remove(columnHeader11);
                }
                else
                {
                }

            }
            else
            {
                if (FilesListView1.Columns.Contains(columnHeader11))
                {
                }
                else
                {
                    FilesListView1.Columns.Insert(3, columnHeader11);
                }

            }

            PathTextBox1.Text = "";
            PathTextBox1.Text = CommonClass.sLastSelectPath;
            FilesListView1.Items.Clear();
            string[] sFiles;
            try
            {
                sFiles = Directory.GetFiles(PathTextBox1.Text, "*.cit", IncludeSubFolderCheckBox1.Checked ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
            }
            catch
            {
                //MessageBox.Show("获取所有文件出错，请检查子目录");
                return;
            }
            FilesListView1.BeginUpdate();
            foreach (string v in sFiles)
            {
                
                string status = CommonClass.cdp.QueryDataInfoHead(v);
                string[] sDataInfoHead = status.Split(new char[] { ',' });
                // iDataType; sDataVersion; sTrackCode; sTrackName; iDir; 
                //sTrain; sDate; sTime; iRunDir;iKmInc; 
                //fkmFrom; fkmTo; iSmaleRate; iChannelNumber;
                if (sDataInfoHead[0].Contains("0"))
                {
                    FilesListView1.Items.Add(sDataInfoHead[4]);
                    
                    FilesListView1.Items[FilesListView1.Items.Count - 1].SubItems.Add(sDataInfoHead[3]);
                    FilesListView1.Items[FilesListView1.Items.Count - 1].SubItems.Add(sDataInfoHead[5]);
                    FilesListView1.Items[FilesListView1.Items.Count - 1].SubItems.Add(sDataInfoHead[9]);
                    FilesListView1.Items[FilesListView1.Items.Count - 1].SubItems.Add(sDataInfoHead[10]);
                    FilesListView1.Items[FilesListView1.Items.Count - 1].SubItems.Add(sDataInfoHead[7]);
                    FilesListView1.Items[FilesListView1.Items.Count - 1].SubItems.Add(sDataInfoHead[8]);
                    FilesListView1.Items[FilesListView1.Items.Count - 1].SubItems.Add(sDataInfoHead[6]);
                    FilesListView1.Items[FilesListView1.Items.Count - 1].SubItems.Add(Path.GetFileName(v));
                    FilesListView1.Items[FilesListView1.Items.Count - 1].SubItems.Add((new FileInfo(v)).Length.ToString());
                    FilesListView1.Items[FilesListView1.Items.Count - 1].SubItems.Add(Path.GetDirectoryName(v));
                }
                Application.DoEvents();
                
            }
            FilesListView1.EndUpdate();
        }

        private void OpenDataDialog_Load(object sender, EventArgs e)
        {
            panel1.Location = new Point(95, 6);
            panel2.Location = new Point(95, 6);
            this.FilesListView1.ListViewItemSorter = new ListViewColumnSorter();
            this.FilesListView1.ColumnClick += new ColumnClickEventHandler(ListViewHelper.ListView_ColumnClick);
            this.OpenDataDialog_Resize(sender, e);
            this.Tag = new string[2] {"","" };
            comboBox1.SelectedIndex = 0;
            if (CommonClass.sLastSelectPath.Length > 0)
            {
                GetData();
            }
            //CommonClass.sDBConnectionString
            try
            {

                DataSet ds = CommonClass.Query("select 自定义线路编号,线路名 from 自定义线路代码表 order by 自定义线路编号");
                comboBoxLineName.DataSource=ds.Tables[0].DefaultView;
                comboBoxLineName.ValueMember = "自定义线路编号";
                comboBoxLineName.DisplayMember = "线路名";
                ds = CommonClass.Query("select linedirid,linedirname from 行别 order by linedirid");
                comboBoxLineDir.DataSource = ds.Tables[0].DefaultView;
                comboBoxLineDir.ValueMember = "linedirid";
                comboBoxLineDir.DisplayMember = "linedirname";
                ds = CommonClass.Query("select trainno,trainname from 车 order by trainno");
                comboBoxTrainNo.DataSource = ds.Tables[0].DefaultView;
                comboBoxTrainNo.ValueMember = "trainno";
                comboBoxTrainNo.DisplayMember = "trainname";
                dateTimePickerEnd.Value = DateTime.Now;
            }
            catch
            {

            }
            bStartup = true;
        }

        private void OpenButton1_Click(object sender, EventArgs e)
        {
            List<OpenDataPackClass> listODPC = new List<OpenDataPackClass>();
            if (FilesListView1.CheckedIndices.Count < 1 || FilesListView1.CheckedIndices.Count > 10)
            {
                this.Tag = listODPC;
                return;
            }

            //try
            //{

            for (int i = 0; i < FilesListView1.CheckedItems.Count; i++)
            {
                int index_Date = FilesListView1.Columns["检测日期"].Index;
                int index_Directory = FilesListView1.Columns["原始路径"].Index;
                int index_FileName = FilesListView1.Columns["原始文件名"].Index;

                OpenDataPackClass odpc = new OpenDataPackClass();
                odpc.iType = 1;
                odpc.sDate = FilesListView1.CheckedItems[i].SubItems[index_Date].Text;
                if (FilesListView1.CheckedItems[i].SubItems[index_Directory].Text.EndsWith("\\"))
                {
                    odpc.sFileName = FilesListView1.CheckedItems[i].SubItems[index_Directory].Text + FilesListView1.CheckedItems[i].SubItems[index_FileName].Text;
                    odpc.sAddFileName = FilesListView1.CheckedItems[i].SubItems[index_Directory].Text + Path.GetFileNameWithoutExtension(FilesListView1.CheckedItems[i].SubItems[index_FileName].Text) + ".idf";
                }
                else
                {
                    odpc.sFileName = FilesListView1.CheckedItems[i].SubItems[index_Directory].Text + "\\" + FilesListView1.CheckedItems[i].SubItems[index_FileName].Text;
                    odpc.sAddFileName = FilesListView1.CheckedItems[i].SubItems[index_Directory].Text + "\\" + Path.GetFileNameWithoutExtension(FilesListView1.CheckedItems[i].SubItems[index_FileName].Text) + ".idf";
                }
                if (comboBox1.SelectedIndex == 1)
                {
                    odpc.iAppMode = 1;
                }
                if (odpc.iAppMode == 0)//本地模式
                {
                    if (!File.Exists(odpc.sAddFileName))
                    {
                        CommonClass.wdp.CreateDB(odpc.sAddFileName);
                        CommonClass.wdp.CreateTable(odpc.sAddFileName);
                        //CommonClass.wdp.GetLostData(odpc.sFileName, odpc.sAddFileName);
                        LoadIndexCheckBox1.Checked = false;
                    }
                    else if (LoadIndexCheckBox1.Checked)
                    {
                        odpc.iIndexID = 1;
                        odpc.bIndex = true;
                    }
                }
                else
                {

                }
                if ((CommonClass.listDIC.Count + i) > 9)
                {

                }
                else
                {
                    odpc.sArrayConfigFile = CommonClass.sArrayConfigFile[CommonClass.listDIC.Count + i];
                    if (!File.Exists(odpc.sArrayConfigFile))
                    {
                        odpc.sArrayConfigFile = "";
                    }
                }


                listODPC.Add(odpc);
            }

            //}
            //catch(Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}

            listODPC = Sort(listODPC);
            this.Tag = listODPC;
        }

        private void OpenDataDialog_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Escape:
                    this.Close();
                    break;

            }
        }

        private void OpenDataDialog_Resize(object sender, EventArgs e)
        {

            InfoLabel1.Left = this.ClientSize.Width / 2 - InfoLabel1.Width / 2;
            InfoLabel1.Top = this.ClientSize.Height / 2 - InfoLabel1.Height / 2;

        }

        private void FilesListView1_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                //双击保持状态
                if (e.Clicks > 1)
                {
                    ListViewItem lvi = FilesListView1.GetItemAt(e.X, e.Y);
                    if (lvi == null)
                        return;
                    lvi.Checked = !lvi.Checked;
                }

                //单击选中
                if (FilesListView1.GetItemAt(e.X, e.Y).Checked)
                {
                    FilesListView1.GetItemAt(e.X, e.Y).Checked = false;
                }
                else
                {
                    FilesListView1.GetItemAt(e.X, e.Y).Checked = true;
                }

            }
            catch
            {

            }
        }

        private List<OpenDataPackClass> Sort(List<OpenDataPackClass> listODPC)
        {

            for (int i = 0; i < listODPC.Count-1; i++)
            {
                for (int j = i+1; j < listODPC.Count; j++)
                {
                    // DateTime.ParseExact("2008-6-27 9:27:15", "yyyy-M-dd H:mm:ss", new CultureInfo("zh-CN"));
                    if (DateTime.ParseExact(listODPC[i].sDate,"yyyy-MM-dd",new CultureInfo("zh-CN")).CompareTo(DateTime.ParseExact(listODPC[j].sDate,"yyyy-MM-dd",new CultureInfo("zh-CN")))<0)
                    {
                        OpenDataPackClass odpc = new OpenDataPackClass();
                        odpc = listODPC[j];
                        listODPC[j] = listODPC[i];
                        listODPC[i] = odpc;
                    }
                }
            }

            return listODPC;
        }

        private void ListData()
        {
            if (!bStartup)
                return;
            FilesListView1.Items.Clear();

            //单机版的新增了一列：方向。因此这里要把这列删除
            if (comboBox1.SelectedIndex == 1)
            {
                if (FilesListView1.Columns.Contains(columnHeader11))
                {
                    FilesListView1.Columns.Remove(columnHeader11);
                } 
                else
                {
                }
                
            } 
            else
            {
                if (FilesListView1.Columns.Contains(columnHeader11))
                {
                }
                else
                {
                    FilesListView1.Columns.Insert(3, columnHeader11);
                }
                
            }

            try
            {
                FilesListView1.Enabled = false;
                InfoLabel1.Visible = true;
                Application.DoEvents();
                //SystemInfoToolStripStatusLabel1.Text=("正在登入网络...");
                //创建一个客户端套接字，它是Login的一个公共属性，
                //将被传递给ChatClient窗体
                CommonClass.MainTcpClient = new TcpClient();
                //向指定的IP地址的服务器发出连接请求
                IAsyncResult MyResult = CommonClass.MainTcpClient.BeginConnect(CommonClass.ServerAddress,
                    CommonClass.ServerPort, null, null);       //采用非同步
                MyResult.AsyncWaitHandle.WaitOne(100, true);                                      //指定等候时间 
                if (!MyResult.IsCompleted)
                {
                    CommonClass.MainTcpClient.Close();
                    //SystemInfoToolStripStatusLabel1.Text = ("网络连接失败");
                    MessageBox.Show("网络连接失败！");
                    comboBox1.SelectedIndex = 0;
                }
                else if (CommonClass.MainTcpClient.Connected)
                {
                    //获得与服务器数据交互的流通道（NetworkStream)
                    CommonClass.MainNetworkStream = CommonClass.MainTcpClient.GetStream();

                    //启动一个新的线程，执行方法this.ServerResponse()，
                    //以便来响应从服务器发回的信息
                    CommonClass.MainThread = new Thread(new ThreadStart(this.ServerResponse));
                    CommonClass.MainThread.Start();

                    //向服务器发送“CONN”请求命令，
                    //此命令的格式与服务器端的定义的格式一致，
                    //命令格式为：命令标志符（CONN）|发送者的用户名|
                    string cmd = "LISTDATA|" + CommonClass.ID + "|" + 
                        comboBoxLineName.SelectedValue.ToString() + "|" +
                        comboBoxLineDir.SelectedValue.ToString() + "|" +
                        comboBoxTrainNo.SelectedValue.ToString() + "|" +
                        dateTimePickerStart.Value.ToString("yyyy-MM-dd") + "|" +
                        dateTimePickerEnd.Value.ToString("yyyy-MM-dd");
                    //将字符串转化为字符数组
                    byte[] outbytes = System.Text.Encoding.Default.GetBytes(
                        cmd.ToCharArray());
                    CommonClass.MainNetworkStream.Write(outbytes, 0, outbytes.Length);

                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                comboBox1.SelectedIndex = 0;
                FilesListView1.Enabled = true;
                InfoLabel1.Visible = false;
                Application.DoEvents();
            }

        }
        private void SetTextFilesListView1(string[] value)
        {
            try
            {
                if (this.FilesListView1.InvokeRequired)
                {
                    SetTextCallbackFilesListView1 d = new SetTextCallbackFilesListView1(SetTextFilesListView1);
                    this.Invoke(d, new object[] { value });
                }
                else
                {
                    ListViewItem lvi = new ListViewItem(value);
                    this.FilesListView1.Items.Add(lvi);
                }
            }
            catch
            {
            }
        }

        private void SetEnabledFilesListView1(bool text)
        {
            try
            {
                if (this.FilesListView1.InvokeRequired)
                {
                    SetEnabledCallbackFilesListView1 d = new SetEnabledCallbackFilesListView1(SetEnabledFilesListView1);
                    this.Invoke(d, new object[] { text });
                }
                else
                {
                    this.FilesListView1.Enabled = text;
                }
            }
            catch
            {
            }
        }

        private void SetUpdateFilesListView1(bool text)
        {
            try
            {
                if (this.FilesListView1.InvokeRequired)
                {
                    SetUpdateCallbackFilesListView1 d = new SetUpdateCallbackFilesListView1(SetUpdateFilesListView1);
                    this.Invoke(d, new object[] { text });
                }
                else
                {
                    if (text)
                    {
                        this.FilesListView1.BeginUpdate();
                    }
                    else
                    {
                        this.FilesListView1.EndUpdate();
                    }
                }
            }
            catch
            {
            }
        }

        private void SetVisibleInfoLabel1(bool text)
        {
            try
            {
                if (this.InfoLabel1.InvokeRequired)
                {
                    SetVisibleCallbackInfoLabel1 d = new SetVisibleCallbackInfoLabel1(SetVisibleInfoLabel1);
                    this.Invoke(d, new object[] { text });
                }
                else
                {
                    this.InfoLabel1.Visible = text;
                }
            }
            catch
            {
            }
        }


        private void ServerResponse()
        {
            //定义一个byte数组，用于接收从服务器端发送来的数据，
            //每次所能接收的数据包的最大长度为6个字节
            byte[] buff = new byte[6];
            string msg;
            int len;
            MemoryStream ms1 = new MemoryStream();
            if (!CommonClass.MainNetworkStream.CanRead)
            {
                return;
            }

            bool stopFlag = false;
            while (!stopFlag)
            {
                try
                {
                    //从流中得到数据，并存入到buff字符数组中
                    len = CommonClass.MainNetworkStream.Read(buff, 0, buff.Length);
                    if (len < 1)
                    {
                        Thread.Sleep(50);
                        continue;
                    }
                    if (!(len == 6) || !(buff[0] == 231))
                    {
                        continue;
                    }
                    int strlen = BitConverter.ToInt32(buff, 2);
                    byte[] bb = new byte[strlen];
                    int realLength = 0;
                    int sumLength = 0;
                    do
                    {
                        realLength = CommonClass.MainNetworkStream.Read(bb, sumLength, strlen);
                        strlen = strlen - realLength;
                        sumLength += realLength;
                        Thread.Sleep(10);
                    } while (strlen > 0);
                    //将字符数组转化为字符串
                    msg = System.Text.Encoding.Default.GetString(bb, 0, bb.Length);
                    msg.Trim();

                    string[] tokens = msg.Split(new Char[] { '|' });
                    if (tokens[0] == "LISTDATAREV" && tokens[1]!="0")
                    {
                        //此时从服务器返回的消息格式：
                        //命令标志符（JOIN）|刚刚登入的用户名|
                        string[] sPra1=tokens[1].Split(new char[] { '^' });
                        SetUpdateFilesListView1(true);
                        foreach (string s in sPra1)
                        {
                            string[] sPra2= s.Split(new char[]{'~'});
                            SetTextFilesListView1(sPra2);
                        }
                        SetUpdateFilesListView1(false);
                     
                        
                    }
                    else
                    {
                        //如果从服务器返回的其他消息格式，
                        //则在ListBox控件中直接显示
                        //                        SystemInfoToolStripStatusLabel1.Text=(msg);
                    }
                    Thread.Sleep(200);
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    Thread.Sleep(200);
                }
                stopFlag = true;
            }
            SetEnabledFilesListView1(true);
            SetVisibleInfoLabel1(false);
            //关闭连接


        }

        private void FilesListView1_EnabledChanged(object sender, EventArgs e)
        {
            if (FilesListView1.Enabled)
            {
                //FilesListView1.Items[FilesListView1.Items.Count - 1].UseItemStyleForSubItems = true;
                //if (sDataInfoHead[5].Equals("上"))
                //{
                //    FilesListView1.Items[FilesListView1.Items.Count - 1].BackColor = Color.LightBlue;
                //}
                //else if (sDataInfoHead[5].Equals("下"))
                //{
                //    FilesListView1.Items[FilesListView1.Items.Count - 1].BackColor = Color.LightCyan;
                //}

                for (int i = 0; i < FilesListView1.Items.Count; i++)
                {
                    FilesListView1.Items[i].UseItemStyleForSubItems = true;
                    try
                    {
                        if (FilesListView1.Items[i].SubItems[2].Text.EndsWith("下"))
                        {
                            FilesListView1.Items[i].BackColor = Color.LightCyan;
                        }
                        else
                        {
                            FilesListView1.Items[i].BackColor = Color.LightBlue;
                        }
                    }
                    catch
                    {
                    
                    }
                }
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (comboBox1.SelectedIndex==1)
            {
                panel2.Visible = true;
                panel1.Visible = false;
                //LoadIndexCheckBox1.Enabled = false;
                ListData();
            }
            else
            {
                FilesListView1.Items.Clear();
                panel2.Visible = false;
                panel1.Visible = true;
                //LoadIndexCheckBox1.Enabled = true;
                //ListData();
                GetDataPre();
                //FilesListView1.Items.Clear();
            }
        }

        private void PathTextBox1_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    CommonClass.sLastSelectPath = PathTextBox1.Text;
                    FilesListView1.Enabled = false;
                    InfoLabel1.Visible = true;
                    Application.DoEvents();
                    GetData();
                    Application.DoEvents();
                    FilesListView1.Enabled = true;
                    InfoLabel1.Visible = false;
                    break;
            }
        }

        private void comboBoxLineName_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListData();
        }

        private void comboBoxLineDir_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListData();
        }

        private void comboBoxTrainNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListData();
        }

        private void dateTimePickerStart_ValueChanged(object sender, EventArgs e)
        {
            ListData();
        }

        private void dateTimePickerEnd_ValueChanged(object sender, EventArgs e)
        {
            ListData();
        }

        private void IncludeSubFolderCheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            GetDataPre();
        }

    }
}
