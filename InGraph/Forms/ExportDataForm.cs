using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.IO;
using InGraph.Classes;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net;

namespace InGraph
{
    /// <summary>
    /// 截取波形片段--控件类
    /// </summary>
    public partial class ExportDataForm : Form
    {
        long startMile;
        long endMile;
        Boolean isKmInc = true;//默认增里程

        public ExportDataForm()
        {
            InitializeComponent();
        }

        private void buttonReturn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonExport_Click(object sender, EventArgs e)
        {

            if (CommonClass.listDIC.Count < 1)
            {
                return;
            }
            switch (CommonClass.listDIC[0].iAppMode)
            {
                case 0:
                    //////
                    string[] ResultStr = new string[3];
                    int[] ResultInt = new int[3];

                    int[] ResultTDS = new int[3];
                    bool[] isExport = new bool[3];
                    isExport[0]=true;
                    isExport[1]=false;
                    isExport[2]=false;
                    ResultStr[0]=CommonClass.listDIC[0].sFilePath;
                    ResultInt[0] = CommonClass.listDIC[0].iSmaleRate;
                    ResultTDS[0]=CommonClass.listDIC[0].iChannelNumber;

                    if (!CommonClass.sExportWaveFilePath.EndsWith("\\"))
                    {
                        CommonClass.sExportWaveFilePath += "\\";
                    }

                    Boolean retVal1 = Textbox_Check(textBoxMileStart);
                    Boolean retVal2 = Textbox_Check(textBoxMileEnd);
                    if (!retVal1 || !retVal2)
                    {
                        return;
                    }


                    long startMile = (long)(float.Parse(textBoxMileStart.Text) * 1000);
                    long endMile = (long)(float.Parse(textBoxMileEnd.Text) * 1000);
                    if (startMile == endMile)
                    {
                        MessageBox.Show("起点公里标和终点公里标相同，请重新填写！");
                        return;
                    }
                    //if (isKmInc && (startMile > endMile))
                    //{
                    //    MessageBox.Show("增里程时：起始里程必须小于结束里程！");
                    //    return;
                    //}
                    //if (!isKmInc && (startMile > endMile))
                    //{
                    //    MessageBox.Show("减里程时：起始里程必须大于结束里程！");
                    //    return;
                    //}

                    Boolean retVal = CommonClass.wdp.ExportData(CommonClass.sExportWaveFilePath, startMile, endMile,
                        isExport, ResultStr, ResultInt, ResultTDS, CommonClass.listDIC[0].sDate, CommonClass.listDIC[0].sTrackName, CommonClass.listDIC[0].sDir, "1", "cit", CommonClass.listDIC[0].bEncrypt);

                    if (retVal)
                    {
                        MessageBox.Show("导出成功！");
                    } 
                    else
                    {
                        MessageBox.Show("导出失败！");
                    }
                    

                    break;
                case 1:
                    try
                    {
                        TcpClient tc;
                        NetworkStream Stream;
                        tc = new TcpClient();
                        //向指定的IP地址的服务器发出连接请求
                        IAsyncResult MyResult = tc.BeginConnect(IPAddress.Parse(CommonClass.ServerAddress),
                            CommonClass.ServerPort, null, null);       //采用非同步
                        MyResult.AsyncWaitHandle.WaitOne(3000, true);                                      //指定等候时间 
                        if (!MyResult.IsCompleted)
                        {
                            tc.Close();
                            MessageBox.Show("网络连接失败！");

                        }
                        if (tc.Connected)
                        {
                            //获得与服务器数据交互的流通道（NetworkStream)
                            Stream = tc.GetStream();
                            string cmd = "EXPORT|客户端请求|true|" + textBoxMileStart.Text + "|" + textBoxMileEnd.Text + "|false|false|false|" + CommonClass.listDIC[0].sDate + "|" + CommonClass.listDIC[0].sTrackName + "|" + CommonClass.listDIC[0].sDir + "|0";
                            //将字符串转化为字符数组
                            byte[] outbytes = System.Text.Encoding.Default.GetBytes(
                                cmd.ToCharArray());
                            if (Stream.CanWrite)
                            { Stream.Write(outbytes, 0, outbytes.Length); }
                            Stream.Close();
                            tc.Close();
                        }

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    finally
                    { this.Close(); }
                    break;
            }
            

        }

        private void ExportDataForm_Load(object sender, EventArgs e)
        {
            if (CommonClass.listDIC.Count == 0)
            {
                MessageBox.Show("请打开一个波形文件");
                return;
            }
            else if (CommonClass.listDIC.Count > 1)
            {
                MessageBox.Show("请不要在打开多个波形文件时执行此功能！");
                return;
            }
            else
            {
                if (CommonClass.listDIC[0].listIC.Count > 0 && CommonClass.listDIC[0].bIndex)
                {
                    startMile = (long)(float.Parse(CommonClass.listDIC[0].listIC[0].lStartMeter)*1000);
                    endMile = (long)(float.Parse(CommonClass.listDIC[0].listIC[CommonClass.listDIC[0].listIC.Count - 1].LEndMeter)*1000);
                    if (startMile > endMile)
                    {
                        isKmInc = false;//减里程
                    }

                    textBoxMileStart.Text = CommonClass.listDIC[0].listIC[0].lStartMeter.ToString();
                    textBoxMileEnd.Text = CommonClass.listDIC[0].listIC[CommonClass.listDIC[0].listIC.Count - 1].LEndMeter.ToString();
                }
                else
                {
                    //后去没有索引过后
                    string sLength = CommonClass.cdp.QueryDataMileageRange(CommonClass.listDIC[0].sFilePath, false, CommonClass.listDIC[0].bEncrypt);
                    string[] sSE = sLength.Split(',');
                    string[] sSE1 = sSE[1].Split('-');
                    textBoxMileStart.Text = sSE1[0].Trim();
                    textBoxMileEnd.Text = sSE1[1].Trim();
                }
            }
        }

        private Boolean Textbox_Check(TextBox tb)
        {
            String tbStr = tb.Text;

            if (String.IsNullOrEmpty(tbStr))
            {
                MessageBox.Show("里程不能为空！");
                return false;
            } 
            else
            {
                try
                {
                    float mile = float.Parse(tbStr);
                    if (mile < 0)
                    {
                        MessageBox.Show("里程数必须大于或等于零！");
                        return false;
                    }
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show("请输入数字！");
                    return false;
                }
                
            }

            return true;
            
        }
    }
}
