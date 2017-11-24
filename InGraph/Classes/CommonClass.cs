using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using InGraph.Classes;
using System.Net.Sockets;
using System.Threading;
using DataProcess;
using System.Xml;
using System.IO;
using System.Globalization;
using System.Data;
using System.Runtime.Serialization.Formatters.Binary;
using System.Data.OleDb;

namespace InGraph.Classes
{
    /// <summary>
    /// 静态类：存放全局变量和方法
    /// </summary>
    static class CommonClass
    {
        #region 回放速度
        /// <summary>
        /// 回放速度
        /// </summary>
        public static int iAutoScroll = 150;
        #endregion

        #region 全局字体
        /// <summary>
        /// 全局字体
        /// </summary>
        public static string sFontName = "宋体";
        #endregion

        #region 是否能运行matlab相关
        /// <summary>
        /// 是否能运行matlab相关
        /// </summary>
        public static bool bRunMatlab = false;
        #endregion

        //数据处理函数
        public static WaveformDataProcess wdp;
        public static CITDataProcess cdp;


        #region 序列号
        /// <summary>
        ///序列号 
        /// </summary>
        public static string sLicense="";
        #endregion

        #region 使用截止时间
        /// <summary>
        /// 使用截止时间
        /// </summary>
        public static DateTime sLicenseDate=new DateTime(); 
        #endregion

        #region 默认配置文件数组
        /// <summary>
        /// 默认配置文件数组
        /// </summary>
        public static string[] sArrayConfigFile = new string[10];
        #endregion

        #region 默认历史层颜色
        /// <summary>
        /// 默认历史层颜色
        /// </summary>
        public static int[] iHisLayerLabelColor = new int[10];
        #endregion

        #region 是否显示无效标注层
        /// <summary>
        /// 是否显示无效标注层
        /// </summary>
        public static bool bInvalidDataShow = false;
        #endregion

        #region 当前打开文件所有里程数据
        /// <summary>
        /// 当前打开文件所有里程数据
        /// </summary>
        public static int[] DataMileageInfo;
        #endregion

        #region 常用控件的高度
        /// <summary>
        /// 常用控件的高度
        /// </summary>
        public static readonly int CommonControlHeight = 25;
        #endregion

        public static int SecondGraphicsAreaHeight = 40;


        #region 坐标区的高度
        /// <summary>
        /// 坐标区的高度
        /// </summary>
        public static readonly int CoordinatesHeight = 30;
        #endregion

        #region 绘图区宽
        /// <summary>
        /// 绘图区宽
        /// </summary>
        public static int iLayerAreaWidth = 0;
        #endregion

        #region 绘制坐标格子--Y
        /// <summary>
        /// 绘制坐标格子--Y
        /// </summary>
        public static int CoordinatesYGrid = 0;
        #endregion

        #region 绘制坐标格子--X
        /// <summary>
        /// 绘制坐标格子--X
        /// </summary>
        public static int CoordinatesXGrid = 0;
        #endregion

        #region 通道信息显示的宽度（波形的右边）
        /// <summary>
        /// 通道信息显示的宽度（波形的右边）
        /// </summary>
        public static readonly int ChannelsAreaWidth = 160;
        #endregion

        #region 通道信息显示的高度（波形的右边）
        /// <summary>
        /// 通道信息显示的高度（波形的右边）
        /// </summary>
        public static readonly int ChannelsAreaHeight = 40;
        #endregion

        #region 数据库运行方式 0本地，1CS 超限
        /// <summary>
        /// 数据库运行方式 0本地，1CS 超限
        /// </summary>
        public static int DB2RunningMode = -1;
        #endregion

        #region 上次打开的文件目录
        /// <summary>
        /// 上次打开的文件目录
        /// </summary>
        public static string sLastSelectPath = "";
        #endregion

        #region 导出数据的路径设置
        #region 截取波形片段的存放路径
        /// <summary>
        /// 截取波形片段的存放路径
        /// </summary>
        public static string sExportWaveFilePath = "";
        #endregion
        #region 导出当前屏数据的存放路径
        /// <summary>
        /// 导出当前屏数据的存放路径
        /// </summary>
        public static string sExportWaveDataPath = "";
        #endregion
        #region 波形数据分割的存放路径
        /// <summary>
        /// 波形数据分割的存放路径
        /// </summary>
        public static string sWaveSplitPath = "";
        #endregion
        #endregion


        #region 数据库连接对象
        /// <summary>
        /// 数据库连接对象
        /// </summary>
        public static string sDBConnectionString = string.Empty;
        #endregion

        #region 台帐图形化显示
        /// <summary>
        /// 台帐图形化显示
        /// </summary>
        public static bool PWMISGraph = false;
        #endregion

        #region 设备台帐信息，对应于Inner.idf文件中对应的数据表----来源于Config.xml配置文件
        /// <summary>
        /// 设备台帐信息，对应于Inner.idf文件中对应的数据表----来源于Config.xml配置文件
        /// ----来源于Config.xml配置文件，总共有5类
        /// ----曲线,坡度,道岔,长短链,速度区段
        /// </summary>
        public static List<PWMISTableClass> listPWMISTable;
        #endregion

        #region 偏差类型
        /// <summary>
        /// 偏差类型
        /// </summary>
        public static List<DataProcess.WaveformDataProcess.ExceptionTypeClass> listETC;
        #endregion

        #region 滚动条对应数据类型
        /// <summary>
        /// 滚动条对应数据类型
        /// </summary>
        public static int hscrolldatatype = 0;
        #endregion

        #region 服务器路径
        /// <summary>
        /// 服务器路径
        /// </summary> 
        public static string ServerPath = string.Empty;
        #endregion

        #region APP配置路径
        /// <summary>
        /// APP配置路径
        /// </summary>
        public static string AppConfigPath = string.Empty;
        #endregion

        #region 服务器地址
        /// <summary>
        /// 服务器地址
        /// </summary> 
        public static string ServerAddress = string.Empty;
        #endregion

        #region 服务器端口号
        /// <summary>
        /// 服务器端口号
        /// </summary> 
        public static int ServerPort = 0;
        #endregion

        #region XML版本号
        /// <summary>
        /// XML版本号
        /// </summary>
        public static string XMLVersion = string.Empty;
        #endregion

        ////动态通道Label数值
        //public static List<Label> DCLabelData;

        #region 动态通道config---没有使用
        /// <summary>
        /// 动态通道config---没有使用
        /// </summary> 
        public static List<ChannelsClass> DCConfig;
        #endregion

        #region 动态数据信息--存放一个或多个波形文件当前显示的部分(xZoomIn)
        /// <summary>
        /// 动态数据信息--存放一个或多个波形文件当前显示的部分(xZoomIn)
        /// </summary>
        public static List<DataInfoClass> listDIC;
        #endregion

        #region 图形化显示的台账信息----每个cit文件所属线路的台账信息
        /// <summary>
        /// 图形化显示的台账信息----每个cit文件所属线路的台账信息
        /// </summary>
        public static List<DatabaseClass> listDBC;
        #endregion

        #region 屏幕分辨率W
        /// <summary>
        /// 屏幕分辨率W
        /// </summary>
        public static int ScreenWidth;
        #endregion

        #region 屏幕分辨率H
        /// <summary>
        /// 屏幕分辨率H
        /// </summary>
        public static int ScreenHeight;
        #endregion

        #region  X轴放大倍数
        /// <summary>
        /// X轴放大倍数
        /// </summary>
        public static int XZoomIn=600;
        #endregion

        #region  X轴放大倍数的除数--把x轴平均分，每次移动的单位--ygx--20140702
        /// <summary>
        /// X轴放大倍数的除数--把x轴平均分，每次移动的单位--ygx--20140702
        /// </summary>
        public static int XZoomInDivisor = 10;
        #endregion

        #region 最后选择的通道
        /// <summary>
        /// 最后选择的通道
        /// </summary>
        public static string LastSelectChannel = string.Empty;
        #endregion

        #region 当前起始里程
        /// <summary>
        /// 当前起始里程
        /// </summary>
        public static float CurStartMileage=0.0f;
        #endregion

        #region 当前终止里程
        /// <summary>
        /// 当前终止里程
        /// </summary>
        public static float CurEndMileage=0.0f;
        #endregion

        #region 测量的半径
        /// <summary>
        /// 测量的半径
        /// </summary>
        public static int MeterageRadius = 0;
        #endregion

        //实时数据
        public static TcpClient AutoTcpClient;
        public static Thread AutoThread;
        public static NetworkStream AutoNetworkStream;

        #region 当前scroll的value
        /// <summary>
        /// 当前scroll的value
        /// </summary>
        public static int CurScrollValue;
        #endregion

        #region 与服务器的连接
        /// <summary>
        /// 与服务器的连接
        /// </summary>
        public static TcpClient MainTcpClient;
        #endregion

        #region 与服务器数据交互的流通道
        /// <summary>
        /// 与服务器数据交互的流通道
        /// </summary>
        public static NetworkStream MainNetworkStream;
        #endregion

        #region 主通信线程
        /// <summary>
        /// 主通信线程
        /// </summary>
        public static Thread MainThread;
        #endregion

        public static string ID = string.Empty;

        #region 最后选择的内容
        /// <summary>
        /// 最后选择的内容
        /// </summary>
        public static string sLastSelectText = string.Empty;
        #endregion

        #region 同步编号
        /// <summary>
        /// 同步编号
        /// </summary>
        public static int tbxh = 0;
        #endregion


        //动态数据组装

        #region 设置层数据偏移
        /// <summary>
        /// 设置层数据偏移
        /// </summary>
        /// <param name="l"></param>
        public static void SetLayerDataReviseValue(int iCheckedIndex,long l)
        {
            if (iCheckedIndex == 0)
            {
                return;
            }
            CommonClass.listDIC[iCheckedIndex].lReviseValue = CommonClass.listDIC[iCheckedIndex].lReviseValue + l;

            List<int> listChannelsVisible = new List<int>();
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < CommonClass.listDIC[iCheckedIndex].listCC.Count; i++)
            {
                if (CommonClass.listDIC[iCheckedIndex].listCC[i].Visible)
                {
                    listChannelsVisible.Add(CommonClass.listDIC[iCheckedIndex].listCC[i].Id);
                    sb.Append(CommonClass.listDIC[iCheckedIndex].listCC[i].Id.ToString() + ",");
                }
            }
            sb.Remove(sb.Length - 1, 1);
            float[][] arrayPointF = new float[listChannelsVisible.Count][];

            for (int i = 0; i < listChannelsVisible.Count; i++)
            {
                arrayPointF[i] = new float[800];
            }
            if (CommonClass.listDIC[iCheckedIndex].iAppMode == 0)
            {
                if (CommonClass.listDIC[iCheckedIndex].bIndex && iCheckedIndex != 0)
                {
                    CommonClass.wdp.GetDataInfo(listChannelsVisible, ref arrayPointF, ref CommonClass.listDIC[iCheckedIndex].listMeter, CommonClass.listDIC[iCheckedIndex].lReviseValue,
                    CommonClass.listDIC[iCheckedIndex].sFilePath, CommonClass.listDIC[0].listMeter[0].GetMeter(1), CommonClass.XZoomIn,
                    CommonClass.listDIC[iCheckedIndex].iSmaleRate, CommonClass.listDIC[iCheckedIndex].iChannelNumber, CommonClass.listDIC[iCheckedIndex].lStartPosition, CommonClass.listDIC[iCheckedIndex].lEndPosition, CommonClass.listDIC[iCheckedIndex].listIC, CommonClass.listDIC[iCheckedIndex].bIndex, CommonClass.listDIC[iCheckedIndex].sKmInc, CommonClass.listDIC[iCheckedIndex].bEncrypt, CommonClass.listDIC[iCheckedIndex].bReverse, true, CommonClass.listDIC[iCheckedIndex].iDataType);
                }
                else
                {
                    CommonClass.wdp.GetDataStartPositionEndPositionInfoIncludeIndex(ref CommonClass.listDIC[iCheckedIndex].lStartPosition,
                        ref CommonClass.listDIC[iCheckedIndex].lEndPosition, CommonClass.listDIC[iCheckedIndex].sFilePath,
                        CommonClass.listDIC[iCheckedIndex].iChannelNumber, -1, -1, CommonClass.listDIC[iCheckedIndex].bIndex);
                    //SystemInfoToolStripStatusLabel1.Text = "获取数据里程完成";
                    //CommonClass.listDIC[0].listMeter[0].GetMeter(1)
                    CommonClass.wdp.GetDataInfo(listChannelsVisible, ref arrayPointF, ref CommonClass.listDIC[iCheckedIndex].listMeter, CommonClass.listDIC[iCheckedIndex].lReviseValue,
                        CommonClass.listDIC[iCheckedIndex].sFilePath, MainForm.sMainform.MainHScrollBar1.Value, CommonClass.XZoomIn,
                        CommonClass.listDIC[iCheckedIndex].iSmaleRate, CommonClass.listDIC[iCheckedIndex].iChannelNumber, CommonClass.listDIC[iCheckedIndex].lStartPosition, CommonClass.listDIC[iCheckedIndex].lEndPosition, CommonClass.listDIC[iCheckedIndex].listIC, CommonClass.listDIC[iCheckedIndex].bIndex, CommonClass.listDIC[iCheckedIndex].sKmInc, CommonClass.listDIC[iCheckedIndex].bEncrypt, CommonClass.listDIC[iCheckedIndex].bReverse, false, CommonClass.listDIC[iCheckedIndex].iDataType);
                    //SystemInfoToolStripStatusLabel1.Text = "读取数据完成";
                }

            }
            else
            {

                CommonClass.wdp.GetDataStartPositionEndPositionInfoIncludeIndex(ref CommonClass.listDIC[iCheckedIndex].lStartPosition, ref CommonClass.listDIC[iCheckedIndex].lEndPosition, CommonClass.listDIC[iCheckedIndex].sFilePath, CommonClass.listDIC[iCheckedIndex].iChannelNumber, -1, -1, CommonClass.listDIC[iCheckedIndex].bIndex);
                CommonClass.NWGetData(CommonClass.listDIC[iCheckedIndex].iDataType, sb.ToString(), ref arrayPointF, ref CommonClass.listDIC[iCheckedIndex].listMeter, CommonClass.listDIC[iCheckedIndex].lReviseValue,
                    CommonClass.listDIC[iCheckedIndex].sFilePath, MainForm.sMainform.MainHScrollBar1.Value, CommonClass.XZoomIn, CommonClass.listDIC[iCheckedIndex].iSmaleRate, CommonClass.listDIC[iCheckedIndex].iChannelNumber,
                    CommonClass.listDIC[iCheckedIndex].lStartPosition, CommonClass.listDIC[iCheckedIndex].lEndPosition, CommonClass.listDIC[iCheckedIndex].listIC, CommonClass.listDIC[iCheckedIndex].bIndex, CommonClass.listDIC[iCheckedIndex].sKmInc, CommonClass.listDIC[iCheckedIndex].bEncrypt, CommonClass.listDIC[iCheckedIndex].bReverse);
            }
            for (int n = 0; n < listChannelsVisible.Count; n++)
            {
                for (int m = 0; m < CommonClass.listDIC[iCheckedIndex].listCC.Count; m++)
                {
                    if (CommonClass.listDIC[iCheckedIndex].listCC[m].Id == listChannelsVisible[n])
                    {
                        CommonClass.listDIC[iCheckedIndex].listCC[m].Data = arrayPointF[n];
                        break;
                    }
                }
            }


            //刷新
            MainForm.sMainform.MainGraphicsPictureBox1.Invalidate();
            MainForm.sMainform.SetTipOnMenu();
        }
        #endregion

        #region 里程对齐
        /// <summary>
        /// 里程对齐
        /// </summary>
        /// <param name="iCheckedIndex"></param>
        /// <param name="l"></param>
        public static void AutoDataReviseValue(int iCheckedIndex)
        {
            if (CommonClass.listDIC[0].bIndex)//有索引
            {
                MessageBox.Show("里程修正状态下不用进行里程对齐");
                return;
            }
            //没有索引的条件下--里程对齐
            switch (CommonClass.listDIC[0].iAppMode)
            {
                case 0:
                    //AnalyseDataAutoFindCorLC
                    /*
                     * 跟踪了CommonClass.listDic的赋值过程，在有索引的条件下Meter的单位为米，没有索引时Meter为采样值
                     * 觉得这里的Meter，属于没有索引，应该是采样值。
                     * 
                     * 修复bug
                     */
                    //long currentMeter = CommonClass.listDIC[0].listMeter[0].Km * 1000 + (int)CommonClass.listDIC[0].listMeter[0].Meter;//对此处的单位有疑问--严广学
                    long currentMeter = CommonClass.listDIC[0].listMeter[0].GetMeter(CommonClass.listDIC[0].bIndex ? 1 : CommonClass.listDIC[0].fScale[1]);
                    int iP=wdp.AutoFindCorLC(CommonClass.listDIC[iCheckedIndex].sFilePath, currentMeter, CommonClass.XZoomIn,
                        CommonClass.listDIC[iCheckedIndex].iSmaleRate, CommonClass.listDIC[iCheckedIndex].iChannelNumber, 0,MainForm.sMainform.MainHScrollBar1.Value);
                    if (iP == 0)
                    {
                        MessageBox.Show("里程没有对齐");
                        return;
                    }
                    CommonClass.listDIC[iCheckedIndex].lReviseValue = iP;

                    List<int> listChannelsVisible = new List<int>();
                    for (int i = 0; i < CommonClass.listDIC[iCheckedIndex].listCC.Count; i++)
                    {
                        if (CommonClass.listDIC[iCheckedIndex].listCC[i].Visible)
                        {
                            listChannelsVisible.Add(CommonClass.listDIC[iCheckedIndex].listCC[i].Id);
                        }
                    }
                    float[][] arrayPointF = new float[listChannelsVisible.Count][];

                    for (int i = 0; i < listChannelsVisible.Count; i++)
                    {
                        arrayPointF[i] = new float[800];
                    }
                    CommonClass.wdp.GetDataStartPositionEndPositionInfoIncludeIndex(ref CommonClass.listDIC[iCheckedIndex].lStartPosition,
                        ref CommonClass.listDIC[iCheckedIndex].lEndPosition, CommonClass.listDIC[iCheckedIndex].sFilePath,
                        CommonClass.listDIC[iCheckedIndex].iChannelNumber, -1, -1, CommonClass.listDIC[iCheckedIndex].bIndex);
                    //SystemInfoToolStripStatusLabel1.Text = "获取数据里程完成";
                    CommonClass.wdp.GetDataInfo( listChannelsVisible, ref arrayPointF, ref CommonClass.listDIC[iCheckedIndex].listMeter, CommonClass.listDIC[iCheckedIndex].lReviseValue,
                        CommonClass.listDIC[iCheckedIndex].sFilePath, MainForm.sMainform.MainHScrollBar1.Value, CommonClass.XZoomIn,
                        CommonClass.listDIC[iCheckedIndex].iSmaleRate, CommonClass.listDIC[iCheckedIndex].iChannelNumber, CommonClass.listDIC[iCheckedIndex].lStartPosition, CommonClass.listDIC[iCheckedIndex].lEndPosition, CommonClass.listDIC[iCheckedIndex].listIC, CommonClass.listDIC[iCheckedIndex].bIndex, CommonClass.listDIC[iCheckedIndex].sKmInc, CommonClass.listDIC[iCheckedIndex].bEncrypt, CommonClass.listDIC[iCheckedIndex].bReverse, false, CommonClass.listDIC[iCheckedIndex].iDataType);
                    //SystemInfoToolStripStatusLabel1.Text = "读取数据完成";

                    for (int n = 0; n < listChannelsVisible.Count; n++)
                    {
                        for (int m = 0; m < CommonClass.listDIC[iCheckedIndex].listCC.Count; m++)
                        {
                            if (CommonClass.listDIC[iCheckedIndex].listCC[m].Id == listChannelsVisible[n])
                            {
                                CommonClass.listDIC[iCheckedIndex].listCC[m].Data = arrayPointF[n];
                                break;
                            }
                        }
                    }
                    MainForm.sMainform.MainGraphicsPictureBox1.Invalidate();
                    
                    break;
            }
        }
        #endregion

        #region 加载通道配置
        /// <summary>
        /// 加载通道配置
        /// </summary>
        public static List<ChannelsClass> LoadChannelsConfig(string sFileName,string[] sChannelsName)
        {
            List<ChannelsClass> l;
            try
            {

                XmlDocument xd = new XmlDocument();
                xd.Load(sFileName);

                XmlNodeList xnl = xd.DocumentElement["channels"].ChildNodes;
                l = new List<ChannelsClass>(xnl.Count);
                for (int i = 0; i < xnl.Count; i++)
                {
                    string sName = xnl[i].Attributes["name"].InnerText;

                    for (int j = 0; j < sChannelsName.Length; j++)
                    {
                        if (sChannelsName[j].ToLower().Equals(sName.ToLower()))
                        {
                            ChannelsClass cc = new ChannelsClass();
                            cc.Id = j;
                            cc.Name = sName;
                            cc.NonChineseName = xnl[i].Attributes["non-chinese_name"].InnerText;
                            cc.ChineseName = xnl[i].Attributes["chinese_name"].InnerText;
                            cc.Color = int.Parse(xnl[i].Attributes["color"].InnerText, NumberStyles.AllowHexSpecifier);
                            cc.Visible = bool.Parse(xnl[i].Attributes["visible"].InnerText);
                            cc.ZoomIn = float.Parse(xnl[i].Attributes["zoomin"].InnerText);
                            cc.Units = xnl[i].Attributes["units"].InnerText;
                            cc.MeaOffset = bool.Parse(xnl[i].Attributes["mea-offset"].InnerText);
                            cc.Location = int.Parse(xnl[i].Attributes["location"].InnerText);
                            cc.fLineWidth = float.Parse(xnl[i].Attributes["line_width"].InnerText);
                            l.Add(cc);
                            break;
                        }
                    }

                  
                }
                l.TrimExcess();
                return l;
            }
            catch (Exception ex)
            {
                MessageBox.Show("通道配置文件加载错误，请检查！");
            }
            return new List<ChannelsClass>();

        }
        #endregion

        #region 从InnerDB数据库上获取cit文件上具体线路的长短链(只适用于一个cit文件)
        /// <summary>
        /// 从InnerDB数据库上获取cit文件上具体线路的长短链(只适用于一个cit文件)
        /// </summary>
        /// <returns>长短链信息</returns>
        public static List<CDLClass> GetCDL()
        {
            List<CDLClass> listCDL = new List<CDLClass>();
            try
            {
                using (OleDbConnection sqlconn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Application.StartupPath + "\\InnerDB.idf;Persist Security Info=True;Mode=Share Exclusive;Jet OLEDB:Database Password=iicdc;"))
                {
                    string sSql = "";
                    if (CommonClass.listDIC[0].sKmInc.Contains("减"))
                    {
                        sSql = "desc";
                    }

                    OleDbCommand sqlcom = new OleDbCommand(" select * from 长短链 where 线编号='" + CommonClass.listDIC[0].sTrackCode + "' and 行别='" + CommonClass.listDIC[0].sDir + "' order by 公里 " + sSql, sqlconn);
                    sqlconn.Open();
                    OleDbDataReader sdr = sqlcom.ExecuteReader();
                    while (sdr.Read())
                    {
                        CDLClass cdcl = new CDLClass();
                        cdcl.dKM = (float)sdr.GetValue(2);
                        cdcl.iMeter = (int)sdr.GetValue(3);
                        cdcl.sType = sdr.GetValue(4).ToString();
                        listCDL.Add(cdcl);
                    }
                    sdr.Close();
                    sqlconn.Close();
                }
            }
            catch
            {

            }
            return listCDL;
        }
        #endregion

        #region 重新加载通道配置
        /// <summary>
        /// 重新加载通道配置
        /// 参数：
        ///     string sFileName 通道配置文件
        ///     int iSelectedIndex 图层文件
        /// </summary>
        public static bool ReLoadChannelsConfig(string sFileName,int iSelectedIndex)
        {
            
            try
            {
                List<ChannelsClass> channelsClsList = LoadChannelsConfig(sFileName,CommonClass.listDIC[iSelectedIndex].sChannelsName);
                CommonClass.listDIC[iSelectedIndex].listCC = channelsClsList;

                CommonClass.listDIC[iSelectedIndex].sConfigFilePath = sFileName;
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " 请检查通道配置文件！");
                return false;
            }
        }
        #endregion

        public static void SaveChannelsConfig(int iSelectedIndex)
        {
            try
            {
                XmlDocument xd = new XmlDocument();
                xd.Load(CommonClass.listDIC[iSelectedIndex].sConfigFilePath);
                XmlNodeList xnl = xd.DocumentElement["channels"].ChildNodes;
                for (int i = 0; i < xnl.Count; i++)
                {

                    //保存配置文件
                    string name = xnl[i].Attributes["name"].InnerText.ToLower();
                    for (int j = 0; j < CommonClass.listDIC[iSelectedIndex].listCC.Count; j++)
                    {
                        if (CommonClass.listDIC[iSelectedIndex].listCC[j].Name.ToLower().Equals(name))
                        {
                            xnl[i].Attributes["non-chinese_name"].Value = CommonClass.listDIC[iSelectedIndex].listCC[j].NonChineseName;
                            xnl[i].Attributes["chinese_name"].Value = CommonClass.listDIC[iSelectedIndex].listCC[j].ChineseName;
                            xnl[i].Attributes["color"].Value = CommonClass.listDIC[iSelectedIndex].listCC[j].Color.ToString("x");
                            xnl[i].Attributes["visible"].Value = CommonClass.listDIC[iSelectedIndex].listCC[j].Visible.ToString();
                            xnl[i].Attributes["zoomin"].Value = CommonClass.listDIC[iSelectedIndex].listCC[j].ZoomIn.ToString();
                            xnl[i].Attributes["mea-offset"].Value = CommonClass.listDIC[iSelectedIndex].listCC[j].MeaOffset.ToString();
                            xnl[i].Attributes["line_width"].Value = CommonClass.listDIC[iSelectedIndex].listCC[j].fLineWidth.ToString();
                            xnl[i].Attributes["location"].Value = CommonClass.listDIC[iSelectedIndex].listCC[j].Location.ToString();
                            break;
                        }
                    }
                }

                xd.Save(CommonClass.listDIC[iSelectedIndex].sConfigFilePath);
            }
            catch
            {

            }
        }
        #region 接口函数---网络版使用
        //网络获取里程定位
        public static int NWGetMeterFind(string sFileName, long mi, int jvli, int cyjg, int gjtds, int datatype,bool bEncrypt)
        {
            try
            {
                CommonClass.MainTcpClient = new TcpClient();
                //向指定的IP地址的服务器发出连接请求
                IAsyncResult MyResult = CommonClass.MainTcpClient.BeginConnect(CommonClass.ServerAddress,
                    CommonClass.ServerPort, null, null);       //采用非同步
                MyResult.AsyncWaitHandle.WaitOne(200, true);                                      //指定等候时间 
                if (CommonClass.MainTcpClient.Connected)
                {
                    //获得与服务器数据交互的流通道（NetworkStream)
                    CommonClass.MainNetworkStream = CommonClass.MainTcpClient.GetStream();

                    //向服务器发送“CONN”请求命令，
                    //此命令的格式与服务器端的定义的格式一致，
                    //命令格式为：命令标志符（CONN）|发送者的用户名|
                    string cmd = "METERFIND|" + sFileName + "|" + mi.ToString() + "|" + jvli.ToString() + "|" + cyjg.ToString() + "|" + gjtds.ToString() + "|" + datatype.ToString() + "|" + bEncrypt.ToString();
                    //将字符串转化为字符数组
                    byte[] outbytes = System.Text.Encoding.Default.GetBytes(
                        cmd.ToCharArray());
                    CommonClass.MainNetworkStream.Write(outbytes, 0, outbytes.Length);

                    if (CommonClass.MainNetworkStream.CanRead)
                    {
                        //读取数据
                        byte[] buff = new byte[6];
                        string msg;
                        int len;
                        MemoryStream ms1 = new MemoryStream();

                        len = CommonClass.MainNetworkStream.Read(buff, 0, buff.Length);
                        if (len < 1)
                        {
                            Thread.Sleep(50);
                            return 0;
                        }
                        if (!(len == 6) || !(buff[0] == 231))
                        {
                            return 0;
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
                        if (tokens[0] == "METERFINDREV")
                        {
                            //此时从服务器返回的消息格式：
                            //命令标志符（JOIN）|刚刚登入的用户名|
                            return int.Parse(tokens[1]);

                        }
                        else
                        {
                            return 0;
                        }


                    }
                    else
                    {
                        return 0;
                    }
                }
                else
                {
                    throw new Exception("网络错误");
                }
            }
            catch
            {
                return 0;
            }
        }

        //网络获取文件通道信息
        public static string NWGetFileChannels(string sFilePath)
        {
            try
            {
                CommonClass.MainTcpClient = new TcpClient();
                //向指定的IP地址的服务器发出连接请求
                IAsyncResult MyResult = CommonClass.MainTcpClient.BeginConnect(CommonClass.ServerAddress,
                    CommonClass.ServerPort, null, null);       //采用非同步
                MyResult.AsyncWaitHandle.WaitOne(200, true);                                      //指定等候时间 
                if (CommonClass.MainTcpClient.Connected)
                {
                    //获得与服务器数据交互的流通道（NetworkStream)
                    CommonClass.MainNetworkStream = CommonClass.MainTcpClient.GetStream();

                    //向服务器发送“CONN”请求命令，
                    //此命令的格式与服务器端的定义的格式一致，
                    //命令格式为：命令标志符（CONN）|发送者的用户名|
                    string cmd = "LISTFILECHANNELS|" + sFilePath;
                    //将字符串转化为字符数组
                    byte[] outbytes = System.Text.Encoding.Default.GetBytes(
                        cmd.ToCharArray());
                    CommonClass.MainNetworkStream.Write(outbytes, 0, outbytes.Length);

                    if (CommonClass.MainNetworkStream.CanRead)
                    {
                        //读取数据
                        byte[] buff = new byte[6];
                        string msg;
                        int len;
                        MemoryStream ms1 = new MemoryStream();

                        len = CommonClass.MainNetworkStream.Read(buff, 0, buff.Length);
                        if (len < 1)
                        {
                            Thread.Sleep(50);
                            return "1,错误!";
                        }
                        if (!(len == 6) || !(buff[0] == 231))
                        {
                            return "1,错误!";
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
                        if (tokens[0] == "LISTFILECHANNELSREV")
                        {
                            //此时从服务器返回的消息格式：
                            //命令标志符（JOIN）|刚刚登入的用户名|
                            return tokens[1];

                        }
                        else
                        {
                            return "1,错误!";
                        }


                    }
                    else
                    {
                        return "1,错误!";
                    }
                }
                else
                {
                    throw new Exception("网络错误");
                }
            }
            catch
            {
                return "1,错误!";
            }
        }
        public static DataSet Query(string sQueryString)
        {
            using (OleDbConnection connection = new OleDbConnection(CommonClass.sDBConnectionString))
            {
                DataSet ds = new DataSet();
                try
                {
                    connection.Open();
                    OleDbDataAdapter command = new OleDbDataAdapter(sQueryString, connection);
                    command.Fill(ds, "ds");
                    connection.Close();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                return ds;
            }
        }
        //网络获取文件头
        public static string NWGetFileHead(string sFilePath)
        {
            try
            {
                CommonClass.MainTcpClient = new TcpClient();
                //向指定的IP地址的服务器发出连接请求
                IAsyncResult MyResult = CommonClass.MainTcpClient.BeginConnect(CommonClass.ServerAddress,
                    CommonClass.ServerPort, null, null);       //采用非同步
                MyResult.AsyncWaitHandle.WaitOne(200, true);                                      //指定等候时间 
                if (CommonClass.MainTcpClient.Connected)
                {
                    //获得与服务器数据交互的流通道（NetworkStream)
                    CommonClass.MainNetworkStream = CommonClass.MainTcpClient.GetStream();

                    //向服务器发送“CONN”请求命令，
                    //此命令的格式与服务器端的定义的格式一致，
                    //命令格式为：命令标志符（CONN）|发送者的用户名|
                    string cmd = "LISTFILEHEAD|" + sFilePath;
                    //将字符串转化为字符数组
                    byte[] outbytes = System.Text.Encoding.Default.GetBytes(
                        cmd.ToCharArray());
                    CommonClass.MainNetworkStream.Write(outbytes, 0, outbytes.Length);

                    if (CommonClass.MainNetworkStream.CanRead)
                    {
                        //读取数据
                        byte[] buff = new byte[6];
                        string msg;
                        int len;
                        MemoryStream ms1 = new MemoryStream();

                        len = CommonClass.MainNetworkStream.Read(buff, 0, buff.Length);
                        if (len < 1)
                        {
                            Thread.Sleep(50);
                            return "1,错误!";
                        }
                        if (!(len == 6) || !(buff[0] == 231))
                        {
                            return "1,错误!";
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
                        if (tokens[0] == "LISTFILEHEADREV")
                        {
                            //此时从服务器返回的消息格式：
                            //命令标志符（JOIN）|刚刚登入的用户名|
                            return tokens[1];

                        }
                        else
                        {
                            return "1,错误!";
                        }


                    }
                    else
                    {
                        return "1,错误!";
                    }
                }
                else
                {
                    throw new Exception("网络错误");
                }
            }
            catch
            {
                return "1,错误!";
            }
        }
        //网络获取文件头
        public static void NWGetChannelScale(string sFilePath,int iChannelNumber, ref float[] fScale, ref  float[] fOffset)
        {
            try
            {
                CommonClass.MainTcpClient = new TcpClient();
                //向指定的IP地址的服务器发出连接请求
                IAsyncResult MyResult = CommonClass.MainTcpClient.BeginConnect(CommonClass.ServerAddress,
                    CommonClass.ServerPort, null, null);       //采用非同步
                MyResult.AsyncWaitHandle.WaitOne(200, true);                                      //指定等候时间 
                if (CommonClass.MainTcpClient.Connected)
                {
                    //获得与服务器数据交互的流通道（NetworkStream)
                    CommonClass.MainNetworkStream = CommonClass.MainTcpClient.GetStream();

                    //向服务器发送“CONN”请求命令，
                    //此命令的格式与服务器端的定义的格式一致，
                    //命令格式为：命令标志符（CONN）|发送者的用户名|
                    string cmd = "LISTCHANNELSCALE|" + sFilePath + "|" + iChannelNumber.ToString();
                    //将字符串转化为字符数组
                    byte[] outbytes = System.Text.Encoding.Default.GetBytes(
                        cmd.ToCharArray());
                    CommonClass.MainNetworkStream.Write(outbytes, 0, outbytes.Length);

                    if (CommonClass.MainNetworkStream.CanRead)
                    {
                        //读取数据
                        byte[] buff = new byte[6];
                        int len;
                        MemoryStream ms1 = new MemoryStream();

                        len = CommonClass.MainNetworkStream.Read(buff, 0, buff.Length);
                        if (len < 1)
                        {
                            Thread.Sleep(50);
                           
                        }
                        if (!(len == 6) || !(buff[0] == 231))
                        {
                           
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
                        if (buff[1] == 20)
                        {
                            //拆分数组
                            int iSplit=BitConverter.ToInt32(bb, 0);
                            MemoryStream bb1=new MemoryStream(bb,4,iSplit+4);
                            BinaryFormatter formatter = new BinaryFormatter();
                            fScale=(float[])formatter.Deserialize(bb1);
                            bb1 = new MemoryStream(bb, iSplit+4, bb.Length - iSplit-4);
                            fOffset = (float[])formatter.Deserialize(bb1);

 
                        }


                    }
                    else
                    {
        
                    }
                }
                else
                {
                    throw new Exception("网络错误");
                }
            }
            catch
            {

            }
        }

        //网络获取文件头尾
        public static void NWDataStartPositionEndPositionInfoIncludeIndex(ref long lStartPosition, ref long lEndPosition, string FileName, long mi, int jvli, int cyjg, int gjtds, long lFixStartPosition, long lFixEndPosition, bool bIndex)
        {
            try
            {
                CommonClass.MainTcpClient = new TcpClient();
                //向指定的IP地址的服务器发出连接请求
                IAsyncResult MyResult = CommonClass.MainTcpClient.BeginConnect(CommonClass.ServerAddress,
                    CommonClass.ServerPort, null, null);       //采用非同步
                MyResult.AsyncWaitHandle.WaitOne(200, true);                                      //指定等候时间 
                if (CommonClass.MainTcpClient.Connected)
                {
                    //获得与服务器数据交互的流通道（NetworkStream)
                    CommonClass.MainNetworkStream = CommonClass.MainTcpClient.GetStream();

                    //向服务器发送“CONN”请求命令，
                    //此命令的格式与服务器端的定义的格式一致，
                    //命令格式为：命令标志符（CONN）|发送者的用户名|
                    string cmd = "LISTSTARTENDLENGTHINFO|" + FileName + "|" + mi.ToString() + "|" + jvli.ToString() + "|" + cyjg.ToString() + "|" + gjtds.ToString() + "|" + lFixStartPosition.ToString() + "|" + lFixEndPosition.ToString() + "|" + bIndex.ToString();
                    //将字符串转化为字符数组
                    byte[] outbytes = System.Text.Encoding.Default.GetBytes(
                        cmd.ToCharArray());
                    CommonClass.MainNetworkStream.Write(outbytes, 0, outbytes.Length);

                    if (CommonClass.MainNetworkStream.CanRead)
                    {
                        //读取数据
                        byte[] buff = new byte[6];
                        int len;
                        MemoryStream ms1 = new MemoryStream();

                        len = CommonClass.MainNetworkStream.Read(buff, 0, buff.Length);
                        if (len < 1)
                        {
                            Thread.Sleep(50);

                        }
                        if (!(len == 6) || !(buff[0] == 231))
                        {

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
                        if (buff[1] == 20)
                        {
                            //拆分数组
                            int iSplit = BitConverter.ToInt32(bb, 0);
                            MemoryStream bb1 = new MemoryStream(bb, 4, iSplit + 4);
                            BinaryFormatter formatter = new BinaryFormatter();
                            lStartPosition = (long)formatter.Deserialize(bb1);
                            bb1 = new MemoryStream(bb, iSplit + 4, bb.Length - iSplit - 4);
                            lEndPosition = (long)formatter.Deserialize(bb1);


                        }


                    }
                    else
                    {

                    }
                }
                else
                {
                    throw new Exception("网络错误");
                }
            }
            catch
            {

            }
            MainNetworkStream.Close();
            MainTcpClient.Close();
        }

        //网络获取数据
        public static void NWGetData(int iDataType, string sListVisable, ref float[][] arrayPointF, ref List<WaveMeter> listMeter, long lReviseValue, string FileName, long mi, int jvli, float iSmaleRate, int iChannelNumber, long lStartPosition, long lEndLength, List<IndexStaClass> listIC, bool bIndex, string sKmInc, bool bEncrypt, bool bReverse)
        {
            try
            {
                CommonClass.MainTcpClient = new TcpClient();
                //向指定的IP地址的服务器发出连接请求
                IAsyncResult MyResult = CommonClass.MainTcpClient.BeginConnect(CommonClass.ServerAddress,
                    CommonClass.ServerPort, null, null);       //采用非同步
                MyResult.AsyncWaitHandle.WaitOne(200, true);                                      //指定等候时间 
                if (CommonClass.MainTcpClient.Connected)
                {
                    //获得与服务器数据交互的流通道（NetworkStream)
                    CommonClass.MainNetworkStream = CommonClass.MainTcpClient.GetStream();

                    //向服务器发送“CONN”请求命令，
                    //此命令的格式与服务器端的定义的格式一致，
                    //命令格式为：命令标志符（CONN）|发送者的用户名|
                    string cmd = "LISTGETDATA|" + FileName + "|" + iDataType.ToString() + "|" + sListVisable.ToString() + "|" + lReviseValue.ToString() + "|" + mi.ToString() + "|" + jvli.ToString() + "|" + iSmaleRate.ToString() + "|" + iChannelNumber.ToString() + "|" + lStartPosition.ToString() + "|" + lEndLength.ToString() + "|" + bIndex.ToString() + "|" + sKmInc.ToString() + "|" + bEncrypt.ToString() + "|" + bReverse.ToString();
                    //将字符串转化为字符数组
                    byte[] outbytes = System.Text.Encoding.Default.GetBytes(
                        cmd.ToCharArray());
                    CommonClass.MainNetworkStream.Write(outbytes, 0, outbytes.Length);

                    if (CommonClass.MainNetworkStream.CanRead)
                    {
                        //读取数据
                        byte[] buff = new byte[6];
                        int len;
                        MemoryStream ms1 = new MemoryStream();

                        len = CommonClass.MainNetworkStream.Read(buff, 0, buff.Length);
                        if (len < 1)
                        {
                            Thread.Sleep(50);

                        }
                        if (!(len == 6) || !(buff[0] == 231))
                        {

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
                        if (buff[1] == 20)
                        {
                            //拆分数组
                            int iSplit = BitConverter.ToInt32(bb, 0);
                            MemoryStream bb1 = new MemoryStream(bb, 4, iSplit + 4);
                            BinaryFormatter formatter = new BinaryFormatter();
                            arrayPointF = (float[][])formatter.Deserialize(bb1);
                            bb1 = new MemoryStream(bb, iSplit + 4, bb.Length - iSplit - 4);
                            listMeter = (List<WaveMeter>)formatter.Deserialize(bb1);


                        }


                    }
                    else
                    {

                    }
                }
                else
                {
                    throw new Exception("网络错误");
                }
            }
            catch
            {

            }
            MainNetworkStream.Close();
            MainTcpClient.Close();
        }
        #endregion

        #region 网络获取里程范围
        /// <summary>
        /// 网络获取里程范围
        /// </summary>
        /// <param name="FileName">文件名</param>
        /// <param name="tds"></param>
        /// <param name="cyjg"></param>
        /// <param name="bEncrypt">是否加密</param>
        /// <returns></returns>
        public static int[] NWGetDataMileageInfo(string FileName, int tds, int cyjg, bool bEncrypt)
        {
            try
            {
                CommonClass.MainTcpClient = new TcpClient();
                //向指定的IP地址的服务器发出连接请求
                IAsyncResult MyResult = CommonClass.MainTcpClient.BeginConnect(CommonClass.ServerAddress,
                    CommonClass.ServerPort, null, null);       //采用非同步
                MyResult.AsyncWaitHandle.WaitOne(200, true);                                      //指定等候时间 
                if (CommonClass.MainTcpClient.Connected)
                {
                    //获得与服务器数据交互的流通道（NetworkStream)
                    CommonClass.MainNetworkStream = CommonClass.MainTcpClient.GetStream();

                    //向服务器发送“CONN”请求命令，
                    //此命令的格式与服务器端的定义的格式一致，
                    //命令格式为：命令标志符（CONN）|发送者的用户名|
                    string cmd = "GETDATAMILEAGEINFO|" + FileName + "|" + tds.ToString() + "|" + cyjg.ToString() + "|" + bEncrypt.ToString();
                    //将字符串转化为字符数组
                    byte[] outbytes = System.Text.Encoding.Default.GetBytes(
                        cmd.ToCharArray());
                    CommonClass.MainNetworkStream.Write(outbytes, 0, outbytes.Length);

                    if (CommonClass.MainNetworkStream.CanRead)
                    {
                        //读取数据
                        byte[] buff = new byte[6];
                        int len;
                        MemoryStream ms1 = new MemoryStream();

                        len = CommonClass.MainNetworkStream.Read(buff, 0, buff.Length);
                        if (len < 1)
                        {
                            Thread.Sleep(50);

                        }
                        if (!(len == 6) || !(buff[0] == 231))
                        {

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
                        if (buff[1] == 40)
                        {
                            //拆分数组
                            MemoryStream bb1 = new MemoryStream(bb);
                            BinaryFormatter formatter = new BinaryFormatter();
                            return (int[])formatter.Deserialize(bb1);


                        }
                        else
                        {
                            throw new Exception("网络错误");
                        }


                    }
                    else
                    {
                        throw new Exception("网络错误");
                    }
                }
                else
                {
                    throw new Exception("网络错误");
                }
            }
            catch
            {
                return new int[800];
            }
            
        }
        #endregion

    }
}
