using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Xml;
using System.Windows.Forms;
using InGraph.Classes;
using System.IO;
using System.Drawing.Drawing2D;
using System.Net.Sockets;
using System.Reflection;
using System.Net;
using System.Threading;
using System.Diagnostics;
using System.Runtime.Serialization.Formatters.Binary;
using System.Drawing.Printing;
using System.Drawing.Imaging;
using System.Globalization;
using DataProcess;
using System.Data.OleDb;
using InGraph.Forms;
using Microsoft.Win32;
using InGraph.Classes.Layers;
using System.Collections;
using System.Text.RegularExpressions;
using InGraph.Properties;
using System.Runtime.InteropServices;
using System.Management;
using Microsoft.VisualBasic;

using InvalidDataProcessing;

using SuperDog;

namespace InGraph
{

    public partial class MainForm : Form
    {
        [DllImportAttribute("gdi32.dll")]
        private static extern bool BitBlt(
            IntPtr hdcDest, //目标设备的句柄 
            int nXDest, // 目标对象的左上角的X坐标 
            int nYDest, // 目标对象的左上角的X坐标 
            int nWidth, // 目标对象的矩形的宽度 
            int nHeight, // 目标对象的矩形的长度 
            IntPtr hdcSrc, // 源设备的句柄 
            int nXSrc, // 源对象的左上角的X坐标 
            int nYSrc, // 源对象的左上角的X坐标 
            int dwRop // 光栅的操作值 
            );
        public const int ROP_SrcCopy = 0xCC0020;
        //全局窗体 
        #region 层平移控件对象
        /// <summary>
        /// 层平移控件对象
        /// </summary>
        LayerTranslationForm fLayerTranslationForm;
        #endregion
        #region 台账信息显示控件对象
        /// <summary>
        /// 台账信息显示控件对象
        /// </summary>
        EquipmentInfoForm fEquipmentInfoForm;
        #endregion
        #region 加载iic文件控件对象
        /// <summary>
        /// 加载iic文件控件对象
        /// </summary>
        IICViewForm fIICViewForm;
        #endregion
        #region 无效信息查看---控件类对象
        /// <summary>
        /// 无效信息查看---控件类对象
        /// </summary>
        InvalidDataForm fInvalidDataForm;
        #endregion
        #region 新增无效信息---控件类对象
        /// <summary>
        /// 新增无效信息---控件类对象
        /// </summary>
        InvalidDataAddForm fInvalidDataAddForm;
        #endregion
        #region 索引设置控件类对象
        /// <summary>
        /// 索引设置控件类对象
        /// </summary>
        public IndexForm fViewIndexForm;
        #endregion
        #region 波形数据图层通道设置控件类对象
        /// <summary>
        /// 波形数据图层通道设置控件类对象
        /// </summary>
        ChannelsDialog dChannelsDialog;
        #endregion
        #region 自定义超限控件类对象---ygx--20140120
        /// <summary>
        /// 自定义超限控件类对象
        /// </summary>
        DIYDefectForm diyDefectForm;
        #endregion
        #region 微小变化查看控件对象---ygx--20140408
        ChangeDataViewForm changeDataViewForm;
        #endregion
        #region 单元截图控件对象---ygx--20151116
        UnitCutPictureForm unitCutPictureForm;
        #endregion


        //微小变化识别
        ChangeDetectionForm changeDetectionForm;
        //峰峰值指标
        FengFengzhiForm fengfengzhiForm;
        //连续多波指标
        MultiWaveForm multiwaveForm;
        //功率谱
        GongLvPuForm gonglvpuForm;


        //幅值统计控件类对象
        DataStatisticsForm dataStatisticsFrom ;
        //扣分统计控件类对象
        KoufenStatisticsForm koufenStatisticsForm ;
        //能量权系数控件类对象
        EnergyStatisticsForm energyStatisticsForm ;

        CaseEstablishedForm caseEstablishedForm;
        CaseViewForm caseViewForm;

        //坐标字体
        Pen pCoordinatesLinesMeter;
        Pen pCoordinatesLines;

        //主窗体事件
        public static MainForm sMainform;
        public StringFormat sfCenter;
        public StringFormat sfLeft;
        public StringFormat sfDB;

        //动态绑定控件响应操作所需要的3个变量
        delegate void SetTextCallbacklblServer(string text);
        delegate void SetTextCallbackthis(string text);
        delegate void SetEnabledCallbackMFHScrollBar(bool value);
        delegate void SetMaximumCallbackMFHScrollBar(int value);
        delegate void SetValueCallbackMFHScrollBar(int value);
        delegate void SetTextCallbacklblCurMeter(string text);
        //动态显示进度条--20140103--ygx
        delegate void SetInfoLabelCallBackThis(Boolean value);

        //

        //分析图显示内容编号
        public List<ChannelsDragClass> listAnalyseGraph;
        //

        private Pen ReticlePen;
        private Font fDBFont;
        private SolidBrush sbInvalidDataBG;

        private int iToolStripCount = 0;
        private int ChannelsDragYMargin = 0;
        private bool ChannelsDraging = false;
        private int ChannelsDragingID1 = -1;
        private int ChannelsDragingID2 = -1;
        private int ChannelsType = 0;
        private List<ChannelsDragClass> listChannelsDrag = new List<ChannelsDragClass>();

        private bool wasMove = false;
        private bool wasUnAreas = false;
        private bool wasCalAreas_fuzhi = false;//幅值统计
        private bool wasCalAreas_koufen = false;//扣分统计--20150312
        private bool wasCalAreas_energy = false;//能量权系数--20150313
        private bool wasMeterageLength = false;
        public bool wasIndex = false;
        private Point lastMovePoint = new Point(0, 0);
        private Point startMovePoint = new Point(0, 0);
        private Point lastMeterageLengthPoint = new Point(0, 0);
        private Point startMeterageLengthPoint = new Point(0, 0);
        //
        private int iSPointX = 0;
        private long iSPointXPos = 0;
        private int iSPointXKM = 0;
        private float iSPointXMeter = 0;

        //客户端的状态
        private static string CLOSED = "closed";
        private static string CONNECTED = "connected";
        private string state = CLOSED;
        //里程标签的位置
        private bool stopFlag;
        //allowNext
        private bool allowNext = true;

        //是否放大
        private bool bZoomInOut = false;
        private int iZoomInOutY = 0;
        private bool bZoomInOutMove = true;
        private int iXLeft = 0;
        private int iXRight = 0;
        private int iZoomInOutX = 0;

        //放大后的属性
        //private int iZoomIn
        //int iHeight = MainGraphicsPictureBox1.ClientSize.Height - CommonClass.CoordinatesHeight;
        //CommonClass.CoordinatesGrid = iHeight / 100;

        //Left里程
        private long leftLC = 0;


        private class NetRemote
        {
            public string iDetectID;
            public string dMeter;
        }
        NetRemote netRemoteInst;
        private class FileRemote
        {
            public string sFilePath;
        }
        FileRemote fileRemoteInst;
        public MainForm(string[] args)
        {
            InitializeComponent();
            //skinEngine1.SkinFile = "skin\\MP10.ssk";
            //打开绘图缓冲相关设置
            EnableDoubleBuffering();
            sMainform = this;
            this.Visible = false;
            try
            {
                if (args.Length == 2)
                {
                    netRemoteInst = new NetRemote();
                    netRemoteInst.iDetectID = args[0];
                    netRemoteInst.dMeter = args[1];
                }
                else if (args.Length == 1)
                {
                    fileRemoteInst=new FileRemote();
                    fileRemoteInst.sFilePath = args[0];
                }
                //Win32_Processor 
                List<string> listS = new List<string>();
                StringBuilder sbID;
                ManagementObjectSearcher mos = new ManagementObjectSearcher("select ProcessorId from " + "Win32_Processor");
                foreach (ManagementObject mo in mos.Get())
                {
                    foreach (PropertyData po in mo.Properties)
                    {
                        sbID = new StringBuilder();
                        sbID.Append(po.Value.ToString().Trim());
                        //while (sbID.Length < 32)
                        //{
                        //    sbID.Append("0");
                        //}
                        listS.Add(sbID.ToString());
                        break;
                    }

                }
                mos = new ManagementObjectSearcher("select SerialNumber from " + "Win32_BaseBoard");
                foreach (ManagementObject mo in mos.Get())
                {
                    foreach (PropertyData po in mo.Properties)
                    {
                        sbID = new StringBuilder();
                        sbID.Append(po.Value.ToString().Trim());
                        //while (sbID.Length < 32)
                        //{
                        //    sbID.Append("0");
                        //}
                        listS.Add(sbID.ToString());
                        break;
                    }

                }
                CommonClass.sLicense = listS[0] + listS[1];

                //StringXOR(listS);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }



        }
        private void StringXOR(List<string> list)
        {
            byte[] b1 = Encoding.UTF8.GetBytes(list[0]);
            byte[] b2 = Encoding.UTF8.GetBytes(list[1]);
            byte[] b = new byte[32];
            for (int i = 0; i < 32; i++)
            {
                b[i] = (byte)(b1[i] ^ b2[i]);
            }

            CommonClass.sLicense = Encoding.UTF8.GetString(b);


        }
        //启用 Form 上的双缓冲，并更新样式以反映所做的更改
        private void EnableDoubleBuffering()
        {
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.UpdateStyles();
        }


        

        

        /// <summary>
        /// 加载数据库
        /// </summary>
        private bool LoadPWMISDatabase()
        {
            try
            {
                if (CommonClass.listDBC.Count > 0 && CommonClass.listDBC[0].Name.Contains("PWMIS台帐"))
                {
                    MessageBox.Show("图形化台帐已经显示，您只能打开一次.");
                    return false;
                }
                //清除数据                
                DatabaseClass dc = new DatabaseClass();
                List<DBChannelsClass> listDBCC = new List<DBChannelsClass>();
                Application.DoEvents();
                int iShow=0;
                for (int i = 0; i < CommonClass.listPWMISTable.Count; i++)
                {
                    DBChannelsClass dbcc = new DBChannelsClass();

                    using (OleDbConnection sqlconn = new OleDbConnection(CommonClass.sDBConnectionString))
                    {
                        OleDbCommand sqlcom = new OleDbCommand("select * from " + CommonClass.listPWMISTable[i].table_name + " where 线编号='" +
                            CommonClass.listDIC[0].sLineCode +
                            "' and 行别='" + CommonClass.listDIC[0].sDir + "' and " + CommonClass.listPWMISTable[i].startMileage + " is not null", sqlconn);
                        sqlconn.Open();
                        OleDbDataReader oddr = sqlcom.ExecuteReader();
                        List<DBDataClass> listDBDC = new List<DBDataClass>();
                        while (oddr.Read())
                        {
                            DBDataClass dbdc = new DBDataClass();

                            dbdc.sDBType = CommonClass.listPWMISTable[i].name;

                            if (dbdc.sDBType.Contains("道岔"))
                            {
                                double jgjlch = double.Parse(oddr.GetValue(oddr.GetOrdinal(CommonClass.listPWMISTable[i].startMileage)).ToString());
                                double dchqch = double.Parse(oddr.GetValue(oddr.GetOrdinal("道岔全长")).ToString()) / 1000000;

                                dbdc.lStartMeter = (long)((jgjlch - dchqch) * 1000);
                                dbdc.lEndMeter = (long)((jgjlch + dchqch) * 1000);

                            }
                            else
                            {
                                dbdc.lStartMeter = (long)(double.Parse(oddr.GetValue(oddr.GetOrdinal(CommonClass.listPWMISTable[i].startMileage)).ToString()) * 1000);
                                if (CommonClass.listPWMISTable[i].endMileage.Length > 0)
                                {
                                    dbdc.lEndMeter = (long)(double.Parse(oddr.GetValue(oddr.GetOrdinal(CommonClass.listPWMISTable[i].endMileage)).ToString()) * 1000);
                                }
                            }


                            if (CommonClass.listDIC[0].sKmInc.Equals("减"))
                            {
                                long iChange = dbdc.lStartMeter;
                                dbdc.lStartMeter = dbdc.lEndMeter;
                                dbdc.lEndMeter = iChange;
                            }


                            if (dbdc.sDBType.Contains("曲线"))
                            {
                                dbdc.sText = oddr.GetValue(oddr.GetOrdinal("曲线方向")).ToString() + " R-" + oddr.GetValue(oddr.GetOrdinal(CommonClass.listPWMISTable[i].sText)).ToString();
                            }
                            else if (dbdc.sDBType.Contains("坡度"))
                            {
                                dbdc.sText = "坡度 "+oddr.GetValue(oddr.GetOrdinal(CommonClass.listPWMISTable[i].sText)).ToString();
                            }
                            else if (dbdc.sDBType.Contains("速度区段"))
                            {
                                dbdc.sText = oddr.GetValue(oddr.GetOrdinal(CommonClass.listPWMISTable[i].sText)).ToString()+"Km/h";
                            }
                            else if (dbdc.sDBType.Contains("道岔"))
                            {
                                String str = oddr.GetValue(oddr.GetOrdinal("辙叉号")).ToString() + "-" + oddr.GetValue(oddr.GetOrdinal("尖轨尖里程")).ToString();
                                dbdc.sText = "道岔";
                            }

                            listDBDC.Add(dbdc);
                        }
                        dbcc.Rect = new Rectangle(MainGraphicsPictureBox1.ClientSize.Width - CommonClass.ChannelsAreaWidth,
                                    3 + (i + 1) * CommonClass.ChannelsAreaHeight + (i + 1),
                                    CommonClass.ChannelsAreaWidth,
                                    CommonClass.ChannelsAreaHeight);
                        dbcc.Name = CommonClass.listPWMISTable[i].name;
                        dbcc.Visible = CommonClass.listPWMISTable[i].bGraph;
                        if(dbcc.Visible) iShow++;
                        dbcc.listDBDC = listDBDC;
                        sqlconn.Close();
                    }

                    listDBCC.Add(dbcc);
                }
                dc.listDBCC = listDBCC;
                CommonClass.listDBC.Insert(0, dc);
                CommonClass.listDBC[0].Name = "PWMIS台帐";
                CommonClass.listDBC[0].bVisible = CommonClass.PWMISGraph;
                CommonClass.SecondGraphicsAreaHeight = 40 * iShow;
                return true;
            }
            catch (Exception ex)
            {
                SystemInfoToolStripStatusLabel1.Text = ("台帐数据库_" + ex.Message);
                return false;
            }


        }



        /// <summary>
        /// 从config.xml文件中加载配置信息，初始化环境 
        /// </summary>
        /// <returns></returns>
        public void InitializeStartup()
        {
            try
            {
                XmlDocument xd = new XmlDocument();
                xd.Load(CommonClass.AppConfigPath);
                CommonClass.MeterageRadius = int.Parse(xd.DocumentElement["MeterageRadius"].InnerText);
                CommonClass.bInvalidDataShow = Boolean.Parse(xd.DocumentElement["InvalidDataShow"].InnerText);
                CommonClass.iAutoScroll = int.Parse(xd.DocumentElement["AutoScroll"].InnerText);
                CommonClass.XMLVersion = xd.DocumentElement["version"].InnerText;

                CommonClass.ServerAddress = xd.DocumentElement["ServerConfig"].Attributes["server"].Value;
                CommonClass.ServerPort = int.Parse(xd.DocumentElement["ServerConfig"].Attributes["port"].Value);

                //获取PWMIS数据库模式
                //Provider=Microsoft.Jet.OLEDB.4.0;Data Source=D:\InfoData.mdb;Persist Security Info=True
                CommonClass.sDBConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Application.StartupPath + "\\InnerDB.idf;Persist Security Info=True;Mode=Share Exclusive;Jet OLEDB:Database Password=iicdc;";
                //
                CommonClass.listETC = new List<WaveformDataProcess.ExceptionTypeClass>();

                using (OleDbConnection sqlconn = new OleDbConnection(CommonClass.sDBConnectionString))
                {
                    OleDbCommand sqlcom = new OleDbCommand("select EXCEPTIONEN,EXCEPTIONCN from Exceptiontype", sqlconn);
                    sqlconn.Open();
                    OleDbDataReader oldr = sqlcom.ExecuteReader();
                    while (oldr.Read())
                    {
                        DataProcess.WaveformDataProcess.ExceptionTypeClass etc = new WaveformDataProcess.ExceptionTypeClass();
                        etc.EXCEPTIONEN = oldr[0].ToString();
                        etc.EXCEPTIONCN = oldr[1].ToString();
                        CommonClass.listETC.Add(etc);
                    }
                    oldr.Close();
                    sqlconn.Close();
                }

                CommonClass.PWMISGraph = bool.Parse(xd.DocumentElement["pwmis"].Attributes["graph"].Value);
                CommonClass.listPWMISTable = new List<PWMISTableClass>();

                //PWMIS
                foreach (XmlNode xnl in xd.DocumentElement["pwmis"].ChildNodes)
                {
                    PWMISTableClass pwmistc = new PWMISTableClass();
                    pwmistc.name = xnl.InnerText;
                    pwmistc.table_name = xnl.Attributes["name"].Value;
                    pwmistc.startMileage = xnl.Attributes["para1"].Value;
                    pwmistc.endMileage = xnl.Attributes["para2"].Value;
                    pwmistc.bGraph = bool.Parse(xnl.Attributes["graph"].Value);
                    pwmistc.sText = xnl.Attributes["text1"].Value;
                    pwmistc.sDisplayField = xnl.Attributes["sDisplayField"].Value;
                    CommonClass.listPWMISTable.Add(pwmistc);
                }
                //默认配置文件
                foreach (XmlNode xnl in xd.DocumentElement["ConfigFiles"].ChildNodes)
                {
                    //xnl.InnerText;
                    string sName = xnl.Name.Replace("default", "");
                    CommonClass.sArrayConfigFile[int.Parse(sName) - 1] = xnl.InnerText;
                }
                //默认配置文件
                foreach (XmlNode xnl in xd.DocumentElement["HisLayer"].ChildNodes)
                {
                    //xnl.InnerText;
                    string sName = xnl.Name.Replace("L", "");
                    CommonClass.iHisLayerLabelColor[int.Parse(sName) - 1] = int.Parse(xnl.InnerText);
                }
                //
                CommonClass.sLastSelectPath = xd.DocumentElement["default"].ChildNodes[0].InnerText;
                CommonClass.sExportWaveFilePath = xd.DocumentElement["ExportData"].ChildNodes[0].InnerText;
                CommonClass.sExportWaveDataPath = xd.DocumentElement["ExportData"].ChildNodes[1].InnerText;//WaveSplitPath
                CommonClass.sWaveSplitPath = xd.DocumentElement["ExportData"].ChildNodes[2].InnerText;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n请检查config.xml文件，程序即将退出！");
                Application.Exit();
            }
            finally
            {

            }

        }

        /// <summary>
        /// 窗体滚轮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_MouseWheel(object sender, MouseEventArgs e)
        {
            if (!MainHScrollBar1.Enabled)
            {
                return;
            }
            ScrollEventArgs sea;
            if (e.Delta > 0)
            {
                if (MainHScrollBar1.Value - MainHScrollBar1.LargeChange < MainHScrollBar1.Minimum)
                {
                    return;
                }
                MainHScrollBar1.Value -= MainHScrollBar1.LargeChange;
                sea = new ScrollEventArgs(ScrollEventType.EndScroll,
                    MainHScrollBar1.Value);
            }
            else
            {
                if (MainHScrollBar1.Value + MainHScrollBar1.LargeChange > MainHScrollBar1.Maximum)
                {
                    return;
                }
                MainHScrollBar1.Value += MainHScrollBar1.LargeChange;
                sea = new ScrollEventArgs(ScrollEventType.EndScroll,
                    MainHScrollBar1.Value);
            }
            MFHScrollBar_Scroll(sender, sea);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            ShowForm sf = new ShowForm();
            sf.Show();
            notifyIcon1.Icon = this.Icon;
            notifyIcon1.Text = this.Text;
            notifyIcon1.Visible = true;
            //写入注册表程序路径
            Registry.CurrentUser.CreateSubKey("Software\\IntegratedDisplay\\");
            RegistryKey regKey = Registry.CurrentUser.OpenSubKey("Software\\IntegratedDisplay\\", true);
            regKey.SetValue("Path", Application.ExecutablePath, RegistryValueKind.String);
            //DateTime dt = DateTime.ParseExact("2008-6-27 9:27:15", "yyyy-M-dd H:mm:ss", new CultureInfo("zh-CN"));
            this.MouseWheel += new MouseEventHandler(MainForm_MouseWheel);

            try
            {
                string sPath = Environment.GetEnvironmentVariable("PATH").ToLower();
                //matlab compiler runtime
                Match m = Regex.Match(sPath, @"matlab compiler runtime\\\w+", RegexOptions.IgnoreCase | RegexOptions.Singleline);
                if (m.Success)
                {
                    //MessageBox.Show("MATLAB 运行时已安装,版本:" +
                    //    m.Value.ToString().Substring(24, m.Value.ToString().Length - 24) +
                    //    "\n您可以使用本软件所提供的MATLAB相关功能!");
                    CommonClass.bRunMatlab = true;
                }
                else
                {
                    CommonClass.bRunMatlab = false;
                }
            }
            catch
            {

            }

            try
            {

                //初始化窗体
                fLayerTranslationForm = new LayerTranslationForm();
                fLayerTranslationForm.Owner = this;
                fLayerTranslationForm.TopLevel = true;
                fEquipmentInfoForm = new EquipmentInfoForm();
                fEquipmentInfoForm.Owner = this;
                fEquipmentInfoForm.TopLevel = true;
                fIICViewForm = new IICViewForm();
                fIICViewForm.Owner = this;
                fIICViewForm.TopLevel = true;
                fViewIndexForm = new IndexForm();
                fViewIndexForm.Owner = this;
                fViewIndexForm.TopLevel = true;
                fInvalidDataForm = new InvalidDataForm();
                fInvalidDataForm.Owner = this;
                fInvalidDataForm.TopLevel = true;
                fInvalidDataAddForm = new InvalidDataAddForm();
                fInvalidDataAddForm.Owner = this;
                fInvalidDataAddForm.TopLevel = true;
                dChannelsDialog = new ChannelsDialog();

                dataStatisticsFrom = new DataStatisticsForm();
                dataStatisticsFrom.Owner = this;
                dataStatisticsFrom.TopLevel = true;
                koufenStatisticsForm = new KoufenStatisticsForm();
                koufenStatisticsForm.Owner = this;
                //koufenStatisticsForm.TopLevel = true;
                energyStatisticsForm = new EnergyStatisticsForm();
                energyStatisticsForm.Owner = this;
                energyStatisticsForm.TopLevel = true;




                listAnalyseGraph = new List<ChannelsDragClass>();

                //初始化常用信息
                sbInvalidDataBG = new SolidBrush(Color.Gainsboro);
                ReticlePen = new Pen(Color.Silver);
                //ReticlePen.DashStyle = DashStyle.Dash;
                ReticlePen.Width = 2;
                pCoordinatesLinesMeter = new Pen(Color.Black);
                pCoordinatesLinesMeter.Width = 1.0f;
                pCoordinatesLines = new Pen(Color.Silver);
                pCoordinatesLines.Width = 1.0f;
                sfCenter = new StringFormat();
                sfCenter.Alignment = StringAlignment.Center;
                sfCenter.LineAlignment = StringAlignment.Far;
                sfLeft = new StringFormat();
                sfLeft.Alignment = StringAlignment.Far;
                sfLeft.LineAlignment = StringAlignment.Near;
                sfDB = new StringFormat();
                sfDB.Alignment = StringAlignment.Center;
                sfDB.LineAlignment = StringAlignment.Center;
                fDBFont = new Font(CommonClass.sFontName, 10);
                Application.DoEvents();
                CommonClass.listDIC = new List<DataInfoClass>();
                CommonClass.listDBC = new List<DatabaseClass>();

                CommonClass.ScreenWidth = Screen.PrimaryScreen.Bounds.Width;
                CommonClass.ScreenHeight = Screen.PrimaryScreen.Bounds.Height;

                //加载服务器信息 
                CommonClass.AppConfigPath = Application.StartupPath + @"\config.xml";


                CommonClass.wdp = new WaveformDataProcess();
                CommonClass.cdp = new CITDataProcess();
                CommonClass.DCConfig = new List<ChannelsClass>();

                //CommonClass.DCLabelData = new List<Label>();
                //[200]原始大小，[400]2倍大小，[600]3倍大小，[800]4倍大小，[1000]5倍大小，[2000]10倍大小，[5000]25倍大小
                CommonClass.XZoomIn = 600;
                //初始化界面 
                InitializeStartup();
                this.MainForm_Resize(sender, e);
                CommonClass.ID = Environment.MachineName + Process.GetCurrentProcess().Id.ToString();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Application.ExitThread();
            }

            try
            {

                if (netRemoteInst != null)
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
                        string cmd = "LISTDATABYID|" + netRemoteInst.iDetectID;
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
                            msg = System.Text.Encoding.Default.GetString(bb, 0, bb.Length);
                            msg.Trim();

                            string[] tokens = msg.Split(new Char[] { '|' });
                            if (tokens[0] == "LISTDATABYIDREV")
                            {
                                string sFilePath = tokens[1];
                                DataInfoClass dic = new DataInfoClass();
                                dic.iAppMode = 1;
                                dic.sFilePath = sFilePath;
                                dic.sConfigFilePath = CommonClass.sArrayConfigFile[0];
                                dic.sAddFile = "";
                                dic.bIndex = false;
                                dic.iIndexID = -1;

                                SetLayerInfo(dic);
                                Application.DoEvents();
                                float f;
                                if (float.TryParse(netRemoteInst.dMeter, out f))
                                {
                                    if (f != 0)
                                    {
                                        MeterFind(f);
                                    }
                                }


                            }
                        }
                        else
                        {

                        }
                    }

                }
                else if (fileRemoteInst != null)
                {
                    DataInfoClass dic = new DataInfoClass();
                    dic.iAppMode = 0;
                    dic.sFilePath = fileRemoteInst.sFilePath;
                    dic.sConfigFilePath = CommonClass.sArrayConfigFile[0];
                    dic.sAddFile = "";
                    dic.bIndex = false;
                    dic.iIndexID = -1;

                    SetLayerInfo(dic);
                    Application.DoEvents();

                }

            }
            catch
            {
            }
            this.Enabled = false;
            // liyang: 去掉下面这个sleep.
            //Thread.Sleep(3000);
            this.Visible = true;
            sf.Close();
            this.Enabled = true;
        }

        //实时数据处理线程
        private void ServerAutoResponse()
        {
            //定义一个byte数组，用于接收从服务器端发送来的数据，
            //每次所能接收的数据包的最大长度为8192个字节
            if (!CommonClass.AutoNetworkStream.CanRead)
            {
                return;
            }
            while (true)
            {
                //try
                //{
                //从流中得到数据，并存入到buff字符数组中
                byte[] buff = new byte[6];
                int len;
                MemoryStream ms1 = new MemoryStream();

                len = CommonClass.AutoNetworkStream.Read(buff, 0, buff.Length);
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
                    realLength = CommonClass.AutoNetworkStream.Read(bb, sumLength, strlen);
                    strlen = strlen - realLength;
                    sumLength += realLength;
                    Thread.Sleep(10);
                } while (strlen > 0);


                //将字符数组转化为字符串
                if (buff[1] == 99)
                {
                    //拆分数组
                    int iLayer = 0;
                    for (int j = 0; j < bb.Length; )
                    {
                        int iSplit = BitConverter.ToInt32(bb, j);
                        j += 4;
                        MemoryStream bb1 = new MemoryStream(bb, j, iSplit);
                        BinaryFormatter formatter = new BinaryFormatter();
                        float[][] arrayPointF = (float[][])formatter.Deserialize(bb1);
                        j += iSplit;
                        int iSplit1 = BitConverter.ToInt32(bb, j);
                        j += 4;
                        bb1 = new MemoryStream(bb, j, iSplit1);
                        List<WaveMeter> listMeter = (List<WaveMeter>)formatter.Deserialize(bb1);
                        j += iSplit1;
                        int iSplit2 = BitConverter.ToInt32(bb, j);
                        j += 4;
                        int iResultCount = BitConverter.ToInt32(bb, j);
                        j += 4;

                        //处理公里
                        for (int i = 0; i < iResultCount; i++)
                        {
                            CommonClass.listDIC[iLayer].listMeter.RemoveAt(0);
                            CommonClass.listDIC[iLayer].listMeter.Add(listMeter[i]);
                        }
                        //处理数据数组
                        for (int n = 0; n < CommonClass.listDIC[iLayer].listChannelsVisible.Count; n++)
                        {
                            for (int m = 0; m < CommonClass.listDIC[iLayer].listCC.Count; m++)
                            {
                                if (CommonClass.listDIC[iLayer].listCC[m].Id == CommonClass.listDIC[iLayer].listChannelsVisible[n] && iResultCount > 0)
                                {
                                    for (int iMi = iResultCount; iMi < CommonClass.listDIC[iLayer].iSmaleRate * CommonClass.XZoomIn; iMi++)
                                    {
                                        CommonClass.listDIC[iLayer].listCC[m].Data[iMi - iResultCount] = CommonClass.listDIC[iLayer].listCC[m].Data[iMi];
                                    }

                                    for (int iMj = 0; iMj < iResultCount; iMj++)
                                    {
                                        CommonClass.listDIC[iLayer].listCC[m].Data[iMj + (CommonClass.listDIC[iLayer].iSmaleRate * CommonClass.XZoomIn) - iResultCount] =
                                            arrayPointF[n][iMj];
                                    }

                                    break;
                                }
                            }
                        }
                        iLayer++;
                    }
                }
                MainGraphicsPictureBox1.Invalidate();
                SecondGraphicsPictureBox1.Invalidate();
                Thread.Sleep(20);
                //}
                //catch(Exception ex)
                //{

                //    return;
                //}
                //finally
                //{
                //}

            }

        }

        private void SetTextthis(string text)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    SetTextCallbackthis d = new SetTextCallbackthis(SetTextthis);
                    this.Invoke(d, new object[] { text });
                }
                else
                {
                    this.Text = text;
                }
            }
            catch
            {
            }
        }
        private void SetEnabledMFHScrollBar(bool value)
        {
            try
            {
                if (this.MainHScrollBar1.InvokeRequired)
                {
                    SetEnabledCallbackMFHScrollBar d = new SetEnabledCallbackMFHScrollBar(SetEnabledMFHScrollBar);
                    this.Invoke(d, new object[] { value });
                }
                else
                {
                    this.MainHScrollBar1.Enabled = value;
                }
            }
            catch
            {
            }
        }
        private void SetMaximumMFHScrollBar(int value)
        {
            try
            {
                if (this.MainHScrollBar1.InvokeRequired)
                {
                    SetMaximumCallbackMFHScrollBar d = new SetMaximumCallbackMFHScrollBar(SetMaximumMFHScrollBar);
                    this.Invoke(d, new object[] { value });
                }
                else
                {
                    this.MainHScrollBar1.Maximum = value;
                }
            }
            catch
            {
            }
        }
        private void SetValueMFHScrollBar(int value)
        {
            try
            {
                if (this.MainHScrollBar1.InvokeRequired)
                {
                    SetValueCallbackMFHScrollBar d = new SetValueCallbackMFHScrollBar(SetValueMFHScrollBar);
                    this.Invoke(d, new object[] { value });
                }
                else
                {
                    this.MainHScrollBar1.Value = value;
                }
            }
            catch
            {
            }
        }
        //实时显示进度条--20140103--ygx
        private void SetInfoLabelThis(Boolean value)
        {
            try
            {
                if (this.InfoLabel2.InvokeRequired && this.pictureBox1.InvokeRequired)
                {
                    SetInfoLabelCallBackThis d = new SetInfoLabelCallBackThis(SetInfoLabelThis);
                    this.Invoke(d, new object[] { value });
                }
                else
                {
                    this.InfoLabel2.Visible = value;
                    this.pictureBox1.Visible = value;
                }
            }
            catch (System.Exception ex)
            {
            	
            }
        }

        /// <summary>
        /// 窗体 Resize
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_Resize(object sender, EventArgs e)
        {
            if (this.Width < 800 || this.Height < 600)
            {
                this.Width = 800;
                this.Height = 600;
            }
            int iPBT1Height = 0;
            int iPBB1Height = 0;
            //初始化4个基本控件的高度
            MainMenuStrip1.Height = CommonClass.CommonControlHeight;
            MainStatusStrip1.Height = CommonClass.CommonControlHeight;

            if (this.FormBorderStyle == FormBorderStyle.Sizable)
            {
                
                //波形下方拖动框
                MainHScrollBar1.Left = 1;
                MainHScrollBar1.Top = this.ClientSize.Height - MainHScrollBar1.Height - MainStatusStrip1.Height - iPBB1Height;
                MainHScrollBar1.Width = this.ClientSize.Width - 2;

                //软件主菜单
                MainMenuStrip1.Left = 1;
                MainMenuStrip1.Top = iPBT1Height + 1;
                MainMenuStrip1.Width = this.ClientSize.Width - 2;
                
                //波形下方状态框
                MainStatusStrip1.Left = 1;
                MainStatusStrip1.Top = this.ClientSize.Height - MainStatusStrip1.Height - iPBB1Height;
                MainStatusStrip1.Width = this.ClientSize.Width - 2;                
            }
            else
            {
                //MFTopTablePanelBox.Top = 1;
                MainHScrollBar1.Top = 1;
            }

            MainGraphicsPictureBox1.Left = 1;
            SecondGraphicsPictureBox1.Left = MainGraphicsPictureBox1.Left;


            MainGraphicsPictureBox1.Width = this.ClientSize.Width - 2;
            SecondGraphicsPictureBox1.Width = MainGraphicsPictureBox1.Width - CommonClass.ChannelsAreaWidth;
            RightLabel1.Width = this.ClientSize.Width - SecondGraphicsPictureBox1.Width - 2;
            RightLabel1.Left = SecondGraphicsPictureBox1.Width + 1;
            if (this.FormBorderStyle == FormBorderStyle.Sizable)
            {
                if (SecondGraphicsPictureBox1.Visible)
                {
                    SecondGraphicsPictureBox1.Top = CommonClass.CommonControlHeight * iToolStripCount + MainMenuStrip1.Height + iPBT1Height + 2;
                    RightLabel1.Top = SecondGraphicsPictureBox1.Top;
                    SecondGraphicsPictureBox1.Height = CommonClass.SecondGraphicsAreaHeight;
                    RightLabel1.Height = SecondGraphicsPictureBox1.Height;
                    MainGraphicsPictureBox1.Top = SecondGraphicsPictureBox1.Top + SecondGraphicsPictureBox1.Height;
                    MainGraphicsPictureBox1.Height = this.ClientSize.Height - MainHScrollBar1.Height - MainMenuStrip1.Height - CommonClass.CommonControlHeight * iToolStripCount - MainStatusStrip1.Height - SecondGraphicsPictureBox1.Height - 2 - iPBT1Height - iPBB1Height;
                }
                else
                {
                    MainGraphicsPictureBox1.Top = CommonClass.CommonControlHeight * iToolStripCount + MainMenuStrip1.Height + iPBT1Height + 2;
                    MainGraphicsPictureBox1.Height = this.ClientSize.Height - MainHScrollBar1.Height - MainMenuStrip1.Height - CommonClass.CommonControlHeight * iToolStripCount - MainStatusStrip1.Height - 2 - iPBT1Height - iPBB1Height;
                }
            }
            else
            {
                if (SecondGraphicsPictureBox1.Visible)
                {
                    SecondGraphicsPictureBox1.Top = 1;
                    SecondGraphicsPictureBox1.Height = CommonClass.SecondGraphicsAreaHeight;
                    MainGraphicsPictureBox1.Top = SecondGraphicsPictureBox1.Height + 1; ;
                    MainGraphicsPictureBox1.Height = this.ClientSize.Height - MainHScrollBar1.Height - MainMenuStrip1.Height - CommonClass.CommonControlHeight * iToolStripCount - SecondGraphicsPictureBox1.Height - 2;
                }
                else
                {
                    MainGraphicsPictureBox1.Top = 1;
                    MainGraphicsPictureBox1.Height = this.ClientSize.Height - MainHScrollBar1.Height - MainMenuStrip1.Height - CommonClass.CommonControlHeight * iToolStripCount - 2;
                }
            }
            //波形数据通道自动变化
            if (CommonClass.listDIC != null)
            {
                foreach (DataInfoClass dic in CommonClass.listDIC)
                {
                    foreach (ChannelsClass cc in dic.listCC)
                    {
                        cc.Rect = new Rectangle(MainGraphicsPictureBox1.ClientSize.Width - CommonClass.ChannelsAreaWidth,
                            cc.Rect.Top,
                            CommonClass.ChannelsAreaWidth,
                            CommonClass.ChannelsAreaHeight);
                    }

                }
            }
            //数据库通道自动变化
            if (CommonClass.listDBC != null)
            {
                foreach (DatabaseClass dbc in CommonClass.listDBC)
                {
                    foreach (DBChannelsClass dbcc in dbc.listDBCC)
                    {
                        dbcc.Rect = new Rectangle(MainGraphicsPictureBox1.ClientSize.Width - CommonClass.ChannelsAreaWidth,
                            dbcc.Rect.Top,
                            CommonClass.ChannelsAreaWidth,
                            CommonClass.ChannelsAreaHeight);
                    }

                }
            }

            MileageInfoLabel1.Left = this.ClientSize.Width / 2 - MileageInfoLabel1.Width / 2;
            MileageInfoLabel1.Top = this.ClientSize.Height / 2 - MileageInfoLabel1.Height / 2;

            ReCalcAxis();

        }
        private void ReCalcAxis()
        {
            CommonClass.CoordinatesYGrid = (MainGraphicsPictureBox1.ClientSize.Height - CommonClass.CoordinatesHeight) / 100;
            CommonClass.iLayerAreaWidth = MainGraphicsPictureBox1.ClientSize.Width - CommonClass.ChannelsAreaWidth;
            CommonClass.CoordinatesXGrid = CommonClass.iLayerAreaWidth / CommonClass.XZoomIn;

        }

        //绘制通道基线
        private Bitmap MoveChannelsBaselines()
        {
            //MainGraphicsPictureBox1
            Bitmap b1 = new Bitmap(MainGraphicsPictureBox1.ClientSize.Width, MainGraphicsPictureBox1.ClientSize.Height - CommonClass.CoordinatesHeight);
            b1.MakeTransparent(Color.White);
            Graphics g1 = Graphics.FromImage(b1);
            Pen p = new Pen(Color.LightSlateGray);
            p.DashStyle = DashStyle.Dash;
            p.DashPattern = new float[] { 10, 5 };
            StringFormat sf = new StringFormat();
            sf.LineAlignment = StringAlignment.Center;
            //绘制数据
            for (int i = 0; i < CommonClass.listDIC.Count; i++)
            {
                if (!CommonClass.listDIC[i].bVisible || !CommonClass.listDIC[i].bChannelsLabel)
                {
                    continue;
                }
                for (int j = 0; j < CommonClass.listDIC[i].listCC.Count; j++)
                {

                    //if (CommonClass.ChannelsLabel[i].Visible)
                    //{
                    if (!CommonClass.listDIC[i].listCC[j].Visible || CommonClass.listDIC[i].listCC[j].Data == null)
                    {
                        continue;
                    }

                    //CommonClass.listDIC[i].listCC[j].Rect = new Rectangle(MainGraphicsPictureBox1.ClientSize.Width - CommonClass.ChannelsAreaWidth,
                    //            3 + CommonClass.listDIC[i].listCC[j].Location,
                    //            CommonClass.ChannelsAreaWidth,
                    //            CommonClass.ChannelsAreaHeight);
                    int iPos = 0;
                    if (CommonClass.listDIC[i].listCC[j].Location < iZoomInOutY)
                    {
                        iPos = -100;
                    }
                    else
                    {
                        iPos = (CommonClass.listDIC[i].listCC[j].Location - iZoomInOutY) * (int)CommonClass.CoordinatesYGrid - CommonClass.ChannelsAreaHeight / 2;
                    }
                    CommonClass.listDIC[i].listCC[j].Rect = new Rectangle(CommonClass.listDIC[i].listCC[j].Rect.X, iPos,
                        CommonClass.listDIC[i].listCC[j].Rect.Width, CommonClass.listDIC[i].listCC[j].Rect.Height);
                    //g1.DrawRectangle(new Pen(Color.Gray), CommonClass.listDIC[i].listCC[j].Rect);
                    g1.DrawString(CommonClass.listDIC[i].listCC[j].ChineseName + " (" + CommonClass.listDIC[i].listCC[j].ZoomIn + CommonClass.listDIC[i].listCC[j].Units + ")",
                        new Font(CommonClass.sFontName, 11.0f, FontStyle.Regular),
                        new SolidBrush(Color.Black),
                        new RectangleF(CommonClass.listDIC[i].listCC[j].Rect.Left,
                            CommonClass.listDIC[i].listCC[j].Rect.Top,
                            CommonClass.listDIC[i].listCC[j].Rect.Width,
                            CommonClass.listDIC[i].listCC[j].Rect.Height / 2), sf);

                    g1.DrawLine(p, 
                        new Point(0, CommonClass.listDIC[i].listCC[j].Rect.Top + CommonClass.listDIC[i].listCC[j].Rect.Height / 2),
                        new Point(MainGraphicsPictureBox1.ClientSize.Width - CommonClass.ChannelsAreaWidth,
                        CommonClass.listDIC[i].listCC[j].Rect.Top + CommonClass.listDIC[i].listCC[j].Rect.Height / 2));


                    g1.DrawLine(new Pen(Color.FromArgb(CommonClass.listDIC[i].listCC[j].Color)), 
                        new Point(MainGraphicsPictureBox1.ClientSize.Width - CommonClass.ChannelsAreaWidth,
                        CommonClass.listDIC[i].listCC[j].Rect.Top + CommonClass.listDIC[i].listCC[j].Rect.Height / 2),
                        new Point(MainGraphicsPictureBox1.ClientSize.Width,
                        CommonClass.listDIC[i].listCC[j].Rect.Top + CommonClass.listDIC[i].listCC[j].Rect.Height / 2));
                    //}

                }
            }
            return b1;
        }

        private void MFPictureBox_Paint(object sender, PaintEventArgs e)
        {
            //绘制通道基线
            e.Graphics.Clear(Color.White);
            //无效数据标示
            if (CommonClass.bInvalidDataShow && CommonClass.listDIC.Count > 0 && CommonClass.listDIC[0].listIDC != null)
            {
                #region 无效数据
                //int iStartIndex = -1;
                //int iEndIndex = -1;
                for (int i = 0; i < CommonClass.listDIC[0].listIDC.Count; i++)
                {
                    int iStartIndex = -1;
                    int iEndIndex = -1;

                    if (long.Parse(CommonClass.listDIC[0].listIDC[i].sEndPoint) < CommonClass.listDIC[0].listMeter[0].lPosition)
                    { 
                        continue; 
                    }
                    if (long.Parse(CommonClass.listDIC[0].listIDC[i].sStartPoint) >
                        CommonClass.listDIC[0].listMeter[CommonClass.listDIC[0].listMeter.Count - 1].lPosition && CommonClass.listDIC[0].listMeter[CommonClass.listDIC[0].listMeter.Count - 1].lPosition != 0)
                    { 
                        continue; 
                    }
                    for (int j = 0; j < CommonClass.listDIC[0].listMeter.Count; j++)
                    {
                        if (CommonClass.listDIC[0].listIDC[i].
                            sStartPoint.Equals(CommonClass.listDIC[0].listMeter[j].lPosition.ToString()))
                        {
                            iStartIndex = j;
                            break;
                        }
                    }
                    for (int j = 0; j < CommonClass.listDIC[0].listMeter.Count; j++)
                    {
                        if (CommonClass.listDIC[0].listIDC[i].
                            sEndPoint.Equals(CommonClass.listDIC[0].listMeter[j].lPosition.ToString()))
                        {
                            iEndIndex = j;
                            break;
                        }
                    }

                    if (iStartIndex != -1 && iEndIndex == -1)
                    {
                        if (CommonClass.listDIC[0].listMeter[CommonClass.listDIC[0].listMeter.Count - 1].lPosition == 0)
                        {
                            iStartIndex = (int)((long.Parse(CommonClass.listDIC[0].listIDC[i].sStartPoint) - CommonClass.listDIC[0].listMeter[0].lPosition) / (2 * CommonClass.listDIC[0].iChannelNumber));
                            iEndIndex = (int)((long.Parse(CommonClass.listDIC[0].listIDC[i].sEndPoint) - CommonClass.listDIC[0].listMeter[0].lPosition) / (2 * CommonClass.listDIC[0].iChannelNumber));
                        } 
                        else
                        {
                            iEndIndex = CommonClass.listDIC[0].listMeter.Count; 
                        }
                        
                    }
                    if (iStartIndex == -1 && iEndIndex != -1)
                    { 
                        iStartIndex = 0; 
                    }

                    if (iStartIndex == -1 && iEndIndex == -1)
                    {
                        if (CommonClass.listDIC[0].listMeter[CommonClass.listDIC[0].listMeter.Count - 1].lPosition == 0)
                        {
                            iStartIndex = (int)((long.Parse(CommonClass.listDIC[0].listIDC[i].sStartPoint) - CommonClass.listDIC[0].listMeter[0].lPosition) / (2 * CommonClass.listDIC[0].iChannelNumber));
                            iEndIndex = (int)((long.Parse(CommonClass.listDIC[0].listIDC[i].sEndPoint) - CommonClass.listDIC[0].listMeter[0].lPosition) / (2 * CommonClass.listDIC[0].iChannelNumber));
                        }
                        else
                        {
                            if (long.Parse(CommonClass.listDIC[0].listIDC[i].sStartPoint) < CommonClass.listDIC[0].listMeter[0].lPosition
                                    && long.Parse(CommonClass.listDIC[0].listIDC[i].sEndPoint) > CommonClass.listDIC[0].listMeter[CommonClass.listDIC[0].listMeter.Count - 1].lPosition)
                            {
                                iStartIndex = 0;
                                iEndIndex = CommonClass.listDIC[0].listMeter.Count;
                            }
                        }


                    }


                    int vStart = iStartIndex * CommonClass.iLayerAreaWidth / CommonClass.listDIC[0].listMeter.Count;
                    int vEnd = iEndIndex * CommonClass.iLayerAreaWidth / CommonClass.listDIC[0].listMeter.Count;
                    e.Graphics.FillRectangle(sbInvalidDataBG, new Rectangle(vStart, CommonClass.CoordinatesHeight, vEnd - vStart,
                    MainGraphicsPictureBox1.ClientSize.Height - CommonClass.CoordinatesHeight));
                }
                #endregion
            }
            e.Graphics.DrawImage(MoveChannelsBaselines(), new Point(0, CommonClass.CoordinatesHeight));


            #region 测量线和断面线
            if (CommonClass.listDIC.Count > 0)
            {             //测量线
                float i = (CommonClass.iLayerAreaWidth) / 1.0f / (CommonClass.listDIC[0].listMeter.Count - 1);

                //测量线
                if (MeterageNoToolStripMenuItem1.Checked == false && MainGraphicsPictureBox1.Cursor == Cursors.Cross && 距离测量ToolStripMenuItem.Checked == false && 断面测量ToolStripMenuItem.Checked == false)
                { 
                    e.Graphics.DrawLine(ReticlePen,
                        MainGraphicsPictureBox1.PointToClient(new Point(Control.MousePosition.X - (int)(CommonClass.MeterageRadius * i * 4),0)).X, CommonClass.CoordinatesHeight,
                      MainGraphicsPictureBox1.PointToClient(new Point(Control.MousePosition.X - (int)(CommonClass.MeterageRadius * i * 4),0)).X, MainGraphicsPictureBox1.ClientSize.Height);

                    e.Graphics.DrawLine(ReticlePen, 
                        MainGraphicsPictureBox1.PointToClient(new Point(Control.MousePosition.X + (int)(CommonClass.MeterageRadius * i * 4),0)).X, CommonClass.CoordinatesHeight,
                      MainGraphicsPictureBox1.PointToClient(new Point(Control.MousePosition.X + (int)(CommonClass.MeterageRadius * i * 4),0)).X, MainGraphicsPictureBox1.ClientSize.Height);
                }
                //断面线
                if (MainGraphicsPictureBox1.Cursor == Cursors.Cross  && 断面测量ToolStripMenuItem.Checked == true)
                {
                    e.Graphics.DrawLine(ReticlePen,
                        MainGraphicsPictureBox1.PointToClient(new Point(Control.MousePosition.X,0)).X, CommonClass.CoordinatesHeight,
                        MainGraphicsPictureBox1.PointToClient(new Point(Control.MousePosition.X,0)).X, MainGraphicsPictureBox1.ClientSize.Height);


                    int k = (int)(Control.MousePosition.X / ((CommonClass.iLayerAreaWidth) / 1.0f / (CommonClass.listDIC[0].listMeter.Count - 1.9)));
                    if (k >= CommonClass.listDIC[0].listMeter.Count)
                    {
                        k = CommonClass.listDIC[0].listMeter.Count - 1;
                    }
                    Font f = new Font(CommonClass.sFontName, 10, FontStyle.Regular);
                    e.Graphics.DrawString("K" + CommonClass.listDIC[0].listMeter[k].Km + "+" + Math.Round((CommonClass.listDIC[0].listMeter[k].Meter / (CommonClass.listDIC[0].bIndex ? 1 : CommonClass.listDIC[0].fScale[1])), 2).ToString(), f, new SolidBrush(Color.Red), new PointF(Control.MousePosition.X - 10, 13));
                }
                DrawingPoints(sender, e);
            }
            #endregion

            #region 标注
            for (int k = 0; k < CommonClass.listDIC.Count; k++)
            {
                if (!CommonClass.listDIC[k].bLabel || CommonClass.listDIC[k].listLIC == null)
                {
                    continue;
                }
                for (int i = 0; i < CommonClass.listDIC[k].listLIC.Count; i++)
                {
                    for (int j = 0; j < CommonClass.listDIC[k].listMeter.Count; j++)
                    {
                        if (CommonClass.listDIC[k].listMeter[j].lPosition == long.Parse(CommonClass.listDIC[k].listLIC[i].sMileIndex))
                        {
                            CommonClass.listDIC[k].listLIC[i].r = new Rectangle((int)Math.Ceiling((j * (MainGraphicsPictureBox1.ClientSize.Width - CommonClass.ChannelsAreaWidth) / 1.0f / CommonClass.listDIC[k].listMeter.Count)),
                                   CommonClass.CoordinatesHeight - 15, 15, 15);
                            e.Graphics.DrawImage(Resources.LabelInfo1, CommonClass.listDIC[k].listLIC[i].r);

                        }
                    }
                }
            }
            #endregion

            #region 定位里程
            if (bDisPlayMeterFind)
            {
                int iMeterFindStart = 0;
                int iMeterFindEnd = 0;

                int sum = CommonClass.listDIC[0].listMeter.Count;

                for (int j = 0; j < CommonClass.listDIC[0].listMeter.Count; j++)
                {
                    if (CommonClass.listDIC[0].listMeter[j].lPosition == lStartIndexMF)
                    {
                        iMeterFindStart = j;
                        break;
                    }
                }
                if (iMeterFindType == 2)
                {
                    for (int j = 0; j < CommonClass.listDIC[0].listMeter.Count; j++)
                    {
                        if (CommonClass.listDIC[0].listMeter[j].lPosition == lEndIndexMF)
                        {
                            iMeterFindEnd = j;
                            break;
                        }
                    }
                }


                int vStart = (iMeterFindStart - sum / 200) * CommonClass.iLayerAreaWidth / sum;

                int len = 0;
                if (iMeterFindType == 1)
                {
                    len = CommonClass.iLayerAreaWidth / 100;
                }
                if (iMeterFindType == 2)
                {
                    int vEnd = (iMeterFindEnd + sum / 200) * CommonClass.iLayerAreaWidth / sum;
                    len = vEnd - vStart;
                }


                SolidBrush sbMeterFindBG = new SolidBrush(Color.FromArgb(100, 255, 125, 255));
                if (iMeterFindStart != 0)
                {
                    e.Graphics.FillRectangle(sbMeterFindBG, new Rectangle(vStart, CommonClass.CoordinatesHeight, len,
                        MainGraphicsPictureBox1.ClientSize.Height - CommonClass.CoordinatesHeight));
                }
            }
            #endregion
        }

        //绘制数据库点
        public void DrawingDBData(object sender, PaintEventArgs e)
        {

            Bitmap bmp = new Bitmap(SecondGraphicsPictureBox1.ClientSize.Width, SecondGraphicsPictureBox1.ClientSize.Height);
            Graphics g1 = Graphics.FromImage(bmp);
            int iHeight = 0;

            int dbccChannelAreaHeight = CommonClass.ChannelsAreaHeight;
            int dbccVisibleCount = 0;

            Pen pen_Line = new Pen(Color.Black, 2);
            Font font_String = new Font(CommonClass.sFontName, 10, FontStyle.Bold);
            SolidBrush brush_String = new SolidBrush(Color.Red);

            try
            {
                for (int i = 0; i < CommonClass.listDBC.Count; i++)
                {
                    if (!CommonClass.listDBC[i].bVisible)
                    {
                        continue;
                    }

                    dbccVisibleCount = 0;

                    for (int j = 0; j < CommonClass.listDBC[i].listDBCC.Count; j++)
                    {
                        if (CommonClass.listDBC[i].listDBCC[j].Visible && !(CommonClass.listDBC[i].listDBCC[j].listDBDC == null))
                        {
                            dbccVisibleCount += 1;

                            foreach (DBDataClass dbdc in CommonClass.listDBC[i].listDBCC[j].listDBDC)
                            {
                                Point p1 = new Point(-1, -1);
                                Point p2 = new Point(-1, -1);
                                if ((CommonClass.listDIC[0].listMeter[0].GetMeter((CommonClass.listDIC[0].bIndex ? 1 : CommonClass.listDIC[0].fScale[1])) <= dbdc.lStartMeter &&
                                    dbdc.lStartMeter < CommonClass.listDIC[0].listMeter[CommonClass.listDIC[0].listMeter.Count - 1].GetMeter((CommonClass.listDIC[0].bIndex ? 1 : CommonClass.listDIC[0].fScale[1])))
                                    || (CommonClass.listDIC[0].listMeter[0].GetMeter((CommonClass.listDIC[0].bIndex ? 1 : CommonClass.listDIC[0].fScale[1])) >= dbdc.lStartMeter &&
                                    dbdc.lStartMeter >= CommonClass.listDIC[0].listMeter[CommonClass.listDIC[0].listMeter.Count - 1].GetMeter((CommonClass.listDIC[0].bIndex ? 1 : CommonClass.listDIC[0].fScale[1]))))
                                {

                                    for (int g = 0; g < CommonClass.listDIC[0].listMeter.Count; g++)
                                    {
                                        if (CommonClass.listDIC[0].listMeter[g].GetMeter((CommonClass.listDIC[0].bIndex ? 1 : CommonClass.listDIC[0].fScale[1])) == dbdc.lStartMeter)
                                        {
                                            p1.X = g * (SecondGraphicsPictureBox1.ClientSize.Width) / CommonClass.listDIC[0].listMeter.Count;
                                            p1.Y = iHeight + (dbccVisibleCount - 1) * dbccChannelAreaHeight;
                                            p2.X = g * (SecondGraphicsPictureBox1.ClientSize.Width) / CommonClass.listDIC[0].listMeter.Count;
                                            //p2.Y = CommonClass.SecondGraphicsAreaHeight - iHeight * 4;
                                            p2.Y = p1.Y + dbccChannelAreaHeight; ;
                                            g1.DrawLine(pen_Line, p1.X, p1.Y, p2.X, p2.Y);


                                            g += 10;
                                            string sHeadText = (dbdc.lStartMeter / 1000.0f).ToString("f3");
                                            g1.DrawString(sHeadText, font_String, brush_String, new PointF(p1.X, p1.Y));
                                            //}
                                            break;
                                        }
                                    }
                                }

                                bool bTail = false;
                                bool bHead = false;
                                Point p3 = new Point(-1, -1);
                                Point p4 = new Point(-1, -1);
                                if (dbdc.lEndMeter.ToString().Length > 0 && (CommonClass.listDIC[0].listMeter[0].GetMeter((CommonClass.listDIC[0].bIndex ? 1 : CommonClass.listDIC[0].fScale[1])) < dbdc.lEndMeter &&
                                    dbdc.lEndMeter <= CommonClass.listDIC[0].listMeter[CommonClass.listDIC[0].listMeter.Count - 1].GetMeter((CommonClass.listDIC[0].bIndex ? 1 : CommonClass.listDIC[0].fScale[1])))
                                    || (CommonClass.listDIC[0].listMeter[0].GetMeter((CommonClass.listDIC[0].bIndex ? 1 : CommonClass.listDIC[0].fScale[1])) >= dbdc.lEndMeter &&
                                    dbdc.lEndMeter >= CommonClass.listDIC[0].listMeter[CommonClass.listDIC[0].listMeter.Count - 1].GetMeter((CommonClass.listDIC[0].bIndex ? 1 : CommonClass.listDIC[0].fScale[1]))))
                                {

                                    for (int k = 0; k < CommonClass.listDIC[0].listMeter.Count; k++)
                                    {

                                        if (CommonClass.listDIC[0].listMeter[k].GetMeter((CommonClass.listDIC[0].bIndex ? 1 : CommonClass.listDIC[0].fScale[1])) == dbdc.lEndMeter)
                                        {
                                            p3.X = k * (SecondGraphicsPictureBox1.ClientSize.Width) / CommonClass.listDIC[0].listMeter.Count;
                                            p3.Y = iHeight + (dbccVisibleCount - 1) * dbccChannelAreaHeight;
                                            p4.X = k * (SecondGraphicsPictureBox1.ClientSize.Width) / CommonClass.listDIC[0].listMeter.Count;
                                            p4.Y = p3.Y + dbccChannelAreaHeight;


                                            g1.DrawLine(pen_Line, p3.X, p3.Y, p4.X, p4.Y);
                                            //绘制曲线尾位置
                                            string sTailText = (dbdc.lEndMeter / 1000.0f).ToString("f3");
                                            g1.DrawString(sTailText, font_String, brush_String, new PointF(p3.X, p3.Y));
                                            if (p1.X != -1 && p1.Y != -1)
                                            {
                                                g1.DrawLine(pen_Line, p3.X, p3.Y, p1.X, p1.Y);
                                                g1.DrawLine(pen_Line, p4.X, p4.Y, p2.X, p2.Y);

                                                //绘制文字信息
                                                g1.DrawString(dbdc.sText, font_String, brush_String, new RectangleF(p1.X, p1.Y + iHeight, p3.X - p1.X, dbccChannelAreaHeight - iHeight * 4), sfDB);

                                            }
                                            else
                                            {
                                                bHead = true;

                                            }
                                            k += 10;
                                            bTail = true;
                                            break;
                                        }
                                    }
                                }



                                //有头没尾,后面填充
                                if (!bTail && p1.X != -1 && p1.Y != -1)
                                {
                                    g1.DrawLine(pen_Line, p1.X, p1.Y, bmp.Width, p1.Y);
                                    g1.DrawLine(pen_Line, p2.X, p2.Y, bmp.Width, p2.Y);
                                    g1.DrawString(dbdc.sText, font_String, brush_String, new RectangleF(p1.X, p1.Y, bmp.Width - p1.X, dbccChannelAreaHeight - iHeight * 4), sfDB);
                                }
                                else if (bHead && p3.X != -1 && p3.Y != -1)//有尾没头,前面补充
                                {
                                    g1.DrawLine(pen_Line, 0, p3.Y, p3.X, p3.Y);
                                    g1.DrawLine(pen_Line, 0, p4.Y, p4.X, p4.Y);
                                    g1.DrawString(dbdc.sText, font_String, brush_String,new RectangleF(0, p3.Y, p3.X, dbccChannelAreaHeight - iHeight * 4), sfDB);
                                }
                                else if (!bHead && !bTail)
                                {
                                    if ((CommonClass.listDIC[0].listMeter[0].GetMeter((CommonClass.listDIC[0].bIndex ? 1 : CommonClass.listDIC[0].fScale[1])) > dbdc.lStartMeter &&
                                      dbdc.lEndMeter > CommonClass.listDIC[0].listMeter[CommonClass.listDIC[0].listMeter.Count - 1].GetMeter((CommonClass.listDIC[0].bIndex ? 1 : CommonClass.listDIC[0].fScale[1])))
                                      || (CommonClass.listDIC[0].listMeter[0].GetMeter((CommonClass.listDIC[0].bIndex ? 1 : CommonClass.listDIC[0].fScale[1])) < dbdc.lStartMeter &&
                                      dbdc.lEndMeter < CommonClass.listDIC[0].listMeter[CommonClass.listDIC[0].listMeter.Count - 1].GetMeter((CommonClass.listDIC[0].bIndex ? 1 : CommonClass.listDIC[0].fScale[1]))))
                                    {
                                        g1.DrawLine(pen_Line, 0, (dbccVisibleCount - 1) * dbccChannelAreaHeight - iHeight, bmp.Width, (dbccVisibleCount - 1) * dbccChannelAreaHeight - iHeight);
                                        g1.DrawLine(pen_Line, 0, dbccVisibleCount * dbccChannelAreaHeight - iHeight, bmp.Width, dbccVisibleCount * dbccChannelAreaHeight - iHeight);

                                        g1.DrawString(dbdc.sText, font_String, brush_String, new RectangleF(0, (dbccVisibleCount - 1) * dbccChannelAreaHeight - iHeight, bmp.Width, CommonClass.ChannelsAreaHeight - iHeight * 2), sfDB);
                                    }

                                }


                            }
                        }
                    }
                }
            }
            catch
            {

            }
            e.Graphics.DrawImage(bmp, new Point(0, 0));
            
            g1.Dispose();
            e.Dispose();
        }

        #region 绘制数据点--事件响应函数
        /// <summary>
        /// 绘制数据点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void DrawingPoints(object sender, PaintEventArgs e)
        {
            //根据返回的数据画点
            Bitmap bmp = new Bitmap(MainGraphicsPictureBox1.ClientSize.Width, MainGraphicsPictureBox1.ClientSize.Height);
            Graphics g1 = Graphics.FromImage(bmp);

            //画点
            #region 画点
            for (int i = CommonClass.listDIC.Count - 1; i > -1; i--)
            {
                if (!CommonClass.listDIC[i].bVisible)
                {
                    continue;
                }
                for (int j = 0; j < CommonClass.listDIC[i].listCC.Count; j++)
                {
                    if (CommonClass.listDIC[i].listCC[j].Visible && !(CommonClass.listDIC[i].listCC[j].Data == null))
                    {
                        PointF[] p = new PointF[CommonClass.listDIC[i].listCC[j].Data.Length];
                        //int iCount = 0;
                        for (int k = 0; k < CommonClass.listDIC[i].listCC[j].Data.Length; k++)
                        {
                            p[k].Y = CommonClass.listDIC[i].listCC[j].Rect.Top + CommonClass.CoordinatesHeight +
                                CommonClass.listDIC[i].listCC[j].Rect.Height / 2 -
                                CommonClass.listDIC[i].listCC[j].Data[k] / CommonClass.listDIC[i].fScale[CommonClass.listDIC[i].listCC[j].Id] * (CommonClass.CoordinatesYGrid / CommonClass.listDIC[i].listCC[j].ZoomIn * (CommonClass.listDIC[i].listCC[j].bReverse ? -1 : 1));
                            p[k].X = k * CommonClass.iLayerAreaWidth / CommonClass.listDIC[i].listCC[j].Data.Length;
                            //p[k].X = CommonClass.CoordinatesXGrid / 1.0f / (CommonClass.listDIC[i].listCC[j].Data.Length / CommonClass.XZoomIn/1.0f) * (CommonClass.listDIC[i].listCC[j].Data[k].X);
                            //CommonClass.listDIC[i].listCC[j].Data[k].X
                        }

                        if (bZoomInOut)
                        {
                            int iLeft = (int)(iXLeft / (CommonClass.iLayerAreaWidth / 1.0f / CommonClass.listDIC[i].listCC[j].Data.Length));
                            int iRight = (int)(iXRight / (CommonClass.iLayerAreaWidth / 1.0f / CommonClass.listDIC[i].listCC[j].Data.Length));
                            int iCount = 0;
                            for (int k = 0; k < p.Length; k++)
                            {
                                if (p[k].X < iXLeft)
                                {
                                    p[k].X = -1;
                                }
                                else if (p[k].X > iXRight)
                                {
                                    p[k].X = CommonClass.iLayerAreaWidth;
                                }
                                else
                                {
                                    p[k].X = iCount * (CommonClass.iLayerAreaWidth / 1.0f / (iRight - iLeft));
                                    iCount++;
                                }
                            }

                        }
                        if (p != null)
                        {
                            g1.DrawLines(new Pen(Color.FromArgb(CommonClass.listDIC[i].listCC[j].Color), CommonClass.listDIC[i].listCC[j].fLineWidth), p);
                        }
                    }
                }
            }
            #endregion
            //绘制坐标
            #region 绘制坐标刻度
            Font f = new Font(CommonClass.sFontName, 10, FontStyle.Regular);
            //绘制默认坐标刻度
            for (int i = 0; i < 2; i++)
            {
                float v = 1.0f;
                v = v + i * 8 * (MainGraphicsPictureBox1.ClientSize.Width - CommonClass.ChannelsAreaWidth) / 8f - i * 3;
                float dSum = CommonClass.CoordinatesHeight;
                while (dSum < MainGraphicsPictureBox1.ClientSize.Height)
                {
                    dSum += CommonClass.CoordinatesYGrid;
                    g1.DrawLine(pCoordinatesLines, new PointF(v - 1, dSum), new PointF(v + 1, dSum));
                }
            }
            #endregion   
            //绘制右侧边框
            #region 绘制边框和里程
            g1.DrawLine(new Pen(Color.Black), CommonClass.iLayerAreaWidth, 0,
                CommonClass.iLayerAreaWidth, bmp.Height);
            //绘制顶部框
            g1.DrawLine(new Pen(Color.Black), 0, CommonClass.CoordinatesHeight,
                bmp.Width, CommonClass.CoordinatesHeight);

            //绘制当前连续里程
            if (CommonClass.listDIC.Count > 0 && CommonClass.listDIC[0].listMeter.Count > 0)
            {
                string sStartMeter = "";
                string sEndMeter = "";
                sStartMeter = "K" + CommonClass.listDIC[0].listMeter[0].Km.ToString() + "+" + (CommonClass.listDIC[0].listMeter[0].Meter / (CommonClass.listDIC[0].bIndex ? 1 : CommonClass.listDIC[0].fScale[1])).ToString("f3");
                sEndMeter = "K" + CommonClass.listDIC[0].listMeter[CommonClass.listDIC[0].listMeter.Count - 1].Km.ToString() + "+" + (CommonClass.listDIC[0].listMeter[CommonClass.listDIC[0].listMeter.Count - 1].Meter / (CommonClass.listDIC[0].bIndex ? 1 : CommonClass.listDIC[0].fScale[1])).ToString("f3");

                g1.DrawString(sStartMeter + "\n" + sEndMeter,f, new SolidBrush(Color.Black),new RectangleF(CommonClass.iLayerAreaWidth, 0,CommonClass.ChannelsAreaWidth, CommonClass.CoordinatesHeight), sfLeft);
            }
            //
            #endregion
            //绘制坐标
            #region 动态绘制坐标
            for (int k = 0; k < CommonClass.listDIC.Count; k++)
            {

                if (!CommonClass.listDIC[k].bMeterInfoVisible || !CommonClass.listDIC[k].bVisible)
                {
                    continue;
                }
                if (CommonClass.listDIC[k].bIndex)
                {
                    for (int i = 0; i < CommonClass.listDIC[k].listMeter.Count; i++)
                    {
                        if ((CommonClass.listDIC[k].listMeter[i].Meter % (CommonClass.XZoomIn == 2000 ? 500 : 100)) < 0.3 && CommonClass.listDIC[k].listMeter[i].GetMeter(1) != 0)
                        {
                            if (CommonClass.listDIC[k].listMeter[i].GetMeter(1) < 0)
                            {
                                continue;
                            }
                            int v = (int)Math.Ceiling(i * (MainGraphicsPictureBox1.ClientSize.Width - CommonClass.ChannelsAreaWidth) / 1.0 / CommonClass.listDIC[k].listMeter.Count);
                            g1.DrawString("K" + CommonClass.listDIC[k].listMeter[i].Km + "+" + (CommonClass.listDIC[k].listMeter[i].Meter).ToString("f0"), f, new SolidBrush(Color.FromArgb(CommonClass.iHisLayerLabelColor[k])), new PointF(v, 3));


                            if (CommonClass.listDIC[k].listMeter[i].GetMeter(1) % 1000 < 2 && CommonClass.listDIC[k].listMeter[i].GetMeter(1) != 0)
                            {

                                g1.DrawLine(pCoordinatesLinesMeter, new PointF(v, CommonClass.CoordinatesHeight), new PointF(v, MainGraphicsPictureBox1.Height));
                            }
                            else
                            {
                                g1.DrawLine(pCoordinatesLines, new PointF(v, CommonClass.CoordinatesHeight), new PointF(v, MainGraphicsPictureBox1.Height));
                            }
                            i = i + 10;
                        }

                    }
                }
                else
                {
                    for (int i = 0; i < CommonClass.listDIC[k].listMeter.Count; i++)
                    {
                        if ((CommonClass.listDIC[k].listMeter[i].Meter / CommonClass.listDIC[0].fScale[1] % (CommonClass.XZoomIn == 2000 ? 500 : 100)) == 0 && CommonClass.listDIC[k].listMeter[i].GetMeter(CommonClass.listDIC[k].fScale[1]) != 0)
                        {
                            if (CommonClass.listDIC[k].listMeter[i].GetMeter(CommonClass.listDIC[k].fScale[1]) < 0)
                            {
                                continue;
                            }
                            int v = (int)Math.Ceiling(i * (MainGraphicsPictureBox1.ClientSize.Width - CommonClass.ChannelsAreaWidth) / 1.0 / CommonClass.listDIC[k].listMeter.Count);
                            g1.DrawString("K" + CommonClass.listDIC[k].listMeter[i].Km + "+" + (CommonClass.listDIC[k].listMeter[i].Meter / CommonClass.listDIC[0].fScale[1]), f, new SolidBrush(Color.FromArgb(CommonClass.iHisLayerLabelColor[k])), new PointF(v, 3));


                            if (CommonClass.listDIC[k].listMeter[i].GetMeter(CommonClass.listDIC[k].fScale[1]) % 1000 == 0 && CommonClass.listDIC[k].listMeter[i].GetMeter(CommonClass.listDIC[k].fScale[1]) != 0)
                            {

                                g1.DrawLine(pCoordinatesLinesMeter, new PointF(v, CommonClass.CoordinatesHeight), new PointF(v, MainGraphicsPictureBox1.Height));
                            }
                            else
                            {
                                g1.DrawLine(pCoordinatesLines, new PointF(v, CommonClass.CoordinatesHeight), new PointF(v, MainGraphicsPictureBox1.Height));
                            }
                            i = i + 10;
                        }

                    }
                }
            }
            #endregion
            e.Graphics.DrawImage(bmp, new Point(0, 0));
            g1.Dispose();
            e.Dispose();
        }
        #endregion

        #region 绘制数据点--画截图
        /// <summary>
        /// 绘制数据点
        /// </summary>
        /// <param name="milePos">公里</param>
        /// <param name="msg">大值信息</param>
        /// <param name="channelName">通道名</param>
        /// <param name="halfLen">标识的半径</param>
        /// <returns>返回bmp图片</returns>
        public Bitmap DrawingPoints(float milePos, String msg,String channelName,float halfLen)
        {
            //根据返回的数据画点
            Bitmap bmp = new Bitmap((Int32)(MainGraphicsPictureBox1.ClientSize.Width), (Int32)(MainGraphicsPictureBox1.ClientSize.Height));
            Graphics g1 = Graphics.FromImage(bmp);

            const int MsgHeight = 30;//信息区高度：大值类型，大值数值，大值等级，位置等等。
            int publicHeight = MsgHeight + CommonClass.CoordinatesHeight;//信息区高度+坐标区高度


            //画点
            #region 画点
            for (int i = CommonClass.listDIC.Count - 1; i > -1; i--)
            {
                if (!CommonClass.listDIC[i].bVisible)
                {
                    continue;
                }
                for (int j = 0; j < CommonClass.listDIC[i].listCC.Count; j++)
                {
                    if (CommonClass.listDIC[i].listCC[j].Visible && !(CommonClass.listDIC[i].listCC[j].Data == null))
                    {
                        PointF[] p = new PointF[CommonClass.listDIC[i].listCC[j].Data.Length];
                        //int iCount = 0;
                        for (int k = 0; k < CommonClass.listDIC[i].listCC[j].Data.Length; k++)
                        {
                            p[k].Y = CommonClass.listDIC[i].listCC[j].Rect.Top + publicHeight +
                                CommonClass.listDIC[i].listCC[j].Rect.Height / 2 -
                                CommonClass.listDIC[i].listCC[j].Data[k] / CommonClass.listDIC[i].fScale[CommonClass.listDIC[i].listCC[j].Id] * (CommonClass.CoordinatesYGrid / CommonClass.listDIC[i].listCC[j].ZoomIn * (CommonClass.listDIC[i].listCC[j].bReverse ? -1 : 1));
                            p[k].X = k * CommonClass.iLayerAreaWidth / CommonClass.listDIC[i].listCC[j].Data.Length;
                            //p[k].X = CommonClass.CoordinatesXGrid / 1.0f / (CommonClass.listDIC[i].listCC[j].Data.Length / CommonClass.XZoomIn/1.0f) * (CommonClass.listDIC[i].listCC[j].Data[k].X);
                            //CommonClass.listDIC[i].listCC[j].Data[k].X
                        }

                        if (bZoomInOut)
                        {
                            int iLeft = (int)(iXLeft / (CommonClass.iLayerAreaWidth / 1.0f / CommonClass.listDIC[i].listCC[j].Data.Length));
                            int iRight = (int)(iXRight / (CommonClass.iLayerAreaWidth / 1.0f / CommonClass.listDIC[i].listCC[j].Data.Length));
                            int iCount = 0;
                            for (int k = 0; k < p.Length; k++)
                            {
                                if (p[k].X < iXLeft)
                                {
                                    p[k].X = -1;
                                }
                                else if (p[k].X > iXRight)
                                {
                                    p[k].X = CommonClass.iLayerAreaWidth;
                                }
                                else
                                {
                                    p[k].X = iCount * (CommonClass.iLayerAreaWidth / 1.0f / (iRight - iLeft));
                                    iCount++;
                                }
                            }
                            
                        }
                        if (p != null)
                        {
                            if (CommonClass.listDIC[i].listCC[j].NonChineseName == channelName)
                            {
                                g1.DrawLines(new Pen(Color.Red, 2), p);
                            } 
                            else
                            {
                                g1.DrawLines(new Pen(Color.FromArgb(CommonClass.listDIC[i].listCC[j].Color), CommonClass.listDIC[i].listCC[j].fLineWidth), p);
                            }                            
                        }
                    }
                }
            }
            #endregion
            //绘制坐标
            #region 绘制坐标刻度
            Font f = new Font(CommonClass.sFontName, 10, FontStyle.Regular);
            //绘制默认坐标刻度
            for (int i = 0; i < 2; i++)
            {
                float v = 1.0f;
                v = v + i * 8 * (MainGraphicsPictureBox1.ClientSize.Width - CommonClass.ChannelsAreaWidth) / 8f - i * 3;
                float dSum = publicHeight;
                while (dSum < MainGraphicsPictureBox1.ClientSize.Height)
                {
                    dSum += CommonClass.CoordinatesYGrid;
                    g1.DrawLine(pCoordinatesLines, new PointF(v - 1, dSum), new PointF(v + 1, dSum));
                }
            }
            #endregion

            //绘制大值信息
            Font f_Except = new Font(CommonClass.sFontName, 14, FontStyle.Regular);
            String m_MilePos = "K" + (int)(milePos) + "+" + Math.Round(milePos * 1000 % 1000,2);
            g1.DrawString(msg, f_Except, new SolidBrush(Color.Black), new PointF(CommonClass.iLayerAreaWidth / 10, 10));

            //绘制右侧边框
            #region 绘制边框和里程
            g1.DrawLine(new Pen(Color.Black), CommonClass.iLayerAreaWidth, 0,CommonClass.iLayerAreaWidth, bmp.Height);
            //绘制信息区的底部线 
            g1.DrawLine(new Pen(Color.Black), 0, MsgHeight,bmp.Width, MsgHeight);
            //绘制顶部框
            g1.DrawLine(new Pen(Color.Black), 0, publicHeight, bmp.Width, publicHeight);

            //绘制当前连续里程
            if (CommonClass.listDIC.Count > 0 && CommonClass.listDIC[0].listMeter.Count > 0)
            {
                string sStartMeter = "";
                string sEndMeter = "";
                sStartMeter = "K" + CommonClass.listDIC[0].listMeter[0].Km.ToString() + "+" + (CommonClass.listDIC[0].listMeter[0].Meter / (CommonClass.listDIC[0].bIndex ? 1 : CommonClass.listDIC[0].fScale[1])).ToString("f3");
                sEndMeter = "K" + CommonClass.listDIC[0].listMeter[CommonClass.listDIC[0].listMeter.Count - 1].Km.ToString() + "+" + (CommonClass.listDIC[0].listMeter[CommonClass.listDIC[0].listMeter.Count - 1].Meter / (CommonClass.listDIC[0].bIndex ? 1 : CommonClass.listDIC[0].fScale[1])).ToString("f3");

                //g1.DrawString(sStartMeter + "\n" + sEndMeter, f, new SolidBrush(Color.Black),
                //    new RectangleF(CommonClass.iLayerAreaWidth, MsgHeight, CommonClass.ChannelsAreaWidth, CommonClass.CoordinatesHeight), sfLeft);
                g1.DrawString(sStartMeter + "\n" + sEndMeter, f, new SolidBrush(Color.Black),
                    new PointF(CommonClass.iLayerAreaWidth, MsgHeight));
            }
            //
            #endregion
            //绘制坐标
            #region 动态绘制坐标
            for (int k = 0; k < CommonClass.listDIC.Count; k++)
            {

                if (!CommonClass.listDIC[k].bMeterInfoVisible || !CommonClass.listDIC[k].bVisible)
                {
                    continue;
                }
                if (CommonClass.listDIC[k].bIndex)
                {
                    for (int i = 0; i < CommonClass.listDIC[k].listMeter.Count; i++)
                    {
                        if ((CommonClass.listDIC[k].listMeter[i].Meter % (CommonClass.XZoomIn == 2000 ? 500 : 100)) < 0.3 && CommonClass.listDIC[k].listMeter[i].GetMeter(1) != 0)
                        {
                            if (CommonClass.listDIC[k].listMeter[i].GetMeter(1) < 0)
                            {
                                continue;
                            }
                            int v = (int)Math.Ceiling(i * (MainGraphicsPictureBox1.ClientSize.Width - CommonClass.ChannelsAreaWidth) / 1.0 / CommonClass.listDIC[k].listMeter.Count);
                            g1.DrawString("K" + CommonClass.listDIC[k].listMeter[i].Km + "+" + (CommonClass.listDIC[k].listMeter[i].Meter).ToString("f0"), f, new SolidBrush(Color.FromArgb(CommonClass.iHisLayerLabelColor[k])), new PointF(v, 3+MsgHeight+10));


                            if (CommonClass.listDIC[k].listMeter[i].GetMeter(1) % 1000 < 2 && CommonClass.listDIC[k].listMeter[i].GetMeter(1) != 0)
                            {

                                g1.DrawLine(pCoordinatesLinesMeter, new PointF(v, publicHeight), new PointF(v, MainGraphicsPictureBox1.Height));
                            }
                            else
                            {
                                g1.DrawLine(pCoordinatesLines, new PointF(v, publicHeight), new PointF(v, MainGraphicsPictureBox1.Height));
                            }
                            i = i + 10;
                        }

                    }
                }
                else
                {
                    for (int i = 0; i < CommonClass.listDIC[k].listMeter.Count; i++)
                    {
                        if ((CommonClass.listDIC[k].listMeter[i].Meter / CommonClass.listDIC[0].fScale[1] % (CommonClass.XZoomIn == 2000 ? 500 : 100)) == 0 && CommonClass.listDIC[k].listMeter[i].GetMeter(CommonClass.listDIC[k].fScale[1]) != 0)
                        {
                            if (CommonClass.listDIC[k].listMeter[i].GetMeter(CommonClass.listDIC[k].fScale[1]) < 0)
                            {
                                continue;
                            }
                            int v = (int)Math.Ceiling(i * (MainGraphicsPictureBox1.ClientSize.Width - CommonClass.ChannelsAreaWidth) / 1.0 / CommonClass.listDIC[k].listMeter.Count);
                            g1.DrawString("K" + CommonClass.listDIC[k].listMeter[i].Km + "+" + (CommonClass.listDIC[k].listMeter[i].Meter / CommonClass.listDIC[0].fScale[1]), f, new SolidBrush(Color.FromArgb(CommonClass.iHisLayerLabelColor[k])), new PointF(v, 3 + MsgHeight + 10));


                            if (CommonClass.listDIC[k].listMeter[i].GetMeter(CommonClass.listDIC[k].fScale[1]) % 1000 == 0 && CommonClass.listDIC[k].listMeter[i].GetMeter(CommonClass.listDIC[k].fScale[1]) != 0)
                            {

                                g1.DrawLine(pCoordinatesLinesMeter, new PointF(v, publicHeight), new PointF(v, MainGraphicsPictureBox1.Height));
                            }
                            else
                            {
                                g1.DrawLine(pCoordinatesLines, new PointF(v, publicHeight), new PointF(v, MainGraphicsPictureBox1.Height));
                            }
                            i = i + 10;
                        }

                    }
                }
            }
            #endregion
            //e.Graphics.DrawImage(bmp, new Point(0, 0));
            //g1.Dispose();
            //e.Dispose();

            //MainGraphicsPictureBox1
            //Bitmap b1 = new Bitmap(MainGraphicsPictureBox1.ClientSize.Width, MainGraphicsPictureBox1.ClientSize.Height - CommonClass.CoordinatesHeight);
            //b1.MakeTransparent(Color.White);
            //Graphics g1 = Graphics.FromImage(b1);
            #region 绘制右侧的通道名称
            Pen p1 = new Pen(Color.LightSlateGray);
            p1.DashStyle = DashStyle.Dash;
            p1.DashPattern = new float[] { 10, 5 };
            StringFormat sf = new StringFormat();
            sf.LineAlignment = StringAlignment.Center;
            //绘制数据
            for (int i = 0; i < CommonClass.listDIC.Count; i++)
            {
                if (!CommonClass.listDIC[i].bVisible || !CommonClass.listDIC[i].bChannelsLabel)
                {
                    continue;
                }
                for (int j = 0; j < CommonClass.listDIC[i].listCC.Count; j++)
                {

                    //if (CommonClass.ChannelsLabel[i].Visible)
                    //{
                    if (!CommonClass.listDIC[i].listCC[j].Visible || CommonClass.listDIC[i].listCC[j].Data == null)
                    {
                        continue;
                    }

                    //CommonClass.listDIC[i].listCC[j].Rect = new Rectangle(MainGraphicsPictureBox1.ClientSize.Width - CommonClass.ChannelsAreaWidth,
                    //            3 + CommonClass.listDIC[i].listCC[j].Location,
                    //            CommonClass.ChannelsAreaWidth,
                    //            CommonClass.ChannelsAreaHeight);
                    int iPos = 0;
                    if (CommonClass.listDIC[i].listCC[j].Location < iZoomInOutY)
                    {
                        iPos = -100;
                    }
                    else
                    {
                        iPos = (CommonClass.listDIC[i].listCC[j].Location - iZoomInOutY) * (int)CommonClass.CoordinatesYGrid - CommonClass.ChannelsAreaHeight / 2;
                    }
                    CommonClass.listDIC[i].listCC[j].Rect = new Rectangle(CommonClass.listDIC[i].listCC[j].Rect.X, iPos,
                        CommonClass.listDIC[i].listCC[j].Rect.Width, CommonClass.listDIC[i].listCC[j].Rect.Height);
                    //g1.DrawRectangle(new Pen(Color.Gray), CommonClass.listDIC[i].listCC[j].Rect);

                    SolidBrush solidBrushChannelName;
                    if (CommonClass.listDIC[i].listCC[j].NonChineseName == channelName)
                    {
                        solidBrushChannelName = new SolidBrush(Color.Red);
                    }
                    else
                    {
                        solidBrushChannelName = new SolidBrush(Color.Black);
                    }

                    g1.DrawString(CommonClass.listDIC[i].listCC[j].ChineseName + " (" + CommonClass.listDIC[i].listCC[j].ZoomIn + CommonClass.listDIC[i].listCC[j].Units + ")",
                        new Font(CommonClass.sFontName, 11.0f, FontStyle.Regular),
                        solidBrushChannelName,
                        new RectangleF(CommonClass.listDIC[i].listCC[j].Rect.Left,
                            CommonClass.listDIC[i].listCC[j].Rect.Top + publicHeight,
                            CommonClass.listDIC[i].listCC[j].Rect.Width,
                            CommonClass.listDIC[i].listCC[j].Rect.Height / 2), sf);

                    g1.DrawLine(p1,
                        new Point(0, CommonClass.listDIC[i].listCC[j].Rect.Top + CommonClass.listDIC[i].listCC[j].Rect.Height / 2 + publicHeight),
                        new Point(MainGraphicsPictureBox1.ClientSize.Width - CommonClass.ChannelsAreaWidth,
                        CommonClass.listDIC[i].listCC[j].Rect.Top + CommonClass.listDIC[i].listCC[j].Rect.Height / 2 + publicHeight));


                    g1.DrawLine(new Pen(Color.FromArgb(CommonClass.listDIC[i].listCC[j].Color)),
                        new Point(MainGraphicsPictureBox1.ClientSize.Width - CommonClass.ChannelsAreaWidth,
                        CommonClass.listDIC[i].listCC[j].Rect.Top + CommonClass.listDIC[i].listCC[j].Rect.Height / 2 + publicHeight),
                        new Point(MainGraphicsPictureBox1.ClientSize.Width,
                        CommonClass.listDIC[i].listCC[j].Rect.Top + CommonClass.listDIC[i].listCC[j].Rect.Height / 2 + publicHeight));
                    //}

                }
            }
            #endregion

            #region 定位里程

            int iMeterFindStart = 0;
            int iMeterFindEnd = 0;

            float fScale = CommonClass.listDIC[0].bIndex ? 1 : 4;

            int startMile = 0;
            int endMile = 0;
            if (CommonClass.listDIC[0].sKmInc == "增")
            {
                startMile = (int)((milePos - halfLen) * 1000);
                endMile = (int)((milePos + halfLen) * 1000);

                for (int j = 0; j < CommonClass.listDIC[0].listMeter.Count - 1; j++)
                {
                    if (CommonClass.listDIC[0].listMeter[j].GetMeter(fScale) <= startMile && startMile <= CommonClass.listDIC[0].listMeter[j + 1].GetMeter(fScale))
                    {
                        iMeterFindStart = j;
                        break;
                    }
                }
                for (int j = 0; j < CommonClass.listDIC[0].listMeter.Count - 1; j++)
                {
                    if (CommonClass.listDIC[0].listMeter[j].GetMeter(fScale) <= endMile && endMile <= CommonClass.listDIC[0].listMeter[j + 1].GetMeter(fScale))
                    {
                        iMeterFindEnd = j;
                        break;
                    }
                }
            }
            else
            {
                startMile = (int)((milePos + halfLen) * 1000);
                endMile = (int)((milePos - halfLen) * 1000);

                for (int j = 0; j < CommonClass.listDIC[0].listMeter.Count - 1; j++)
                {
                    if (CommonClass.listDIC[0].listMeter[j].GetMeter(fScale) >= startMile && startMile >= CommonClass.listDIC[0].listMeter[j + 1].GetMeter(fScale))
                    {
                        iMeterFindStart = j;
                        break;
                    }
                }
                for (int j = 0; j < CommonClass.listDIC[0].listMeter.Count - 1; j++)
                {
                    if (CommonClass.listDIC[0].listMeter[j].GetMeter(fScale) >= endMile && endMile >= CommonClass.listDIC[0].listMeter[j + 1].GetMeter(fScale))
                    {
                        iMeterFindEnd = j;
                        break;
                    }
                }
            }


            int sum = CommonClass.listDIC[0].listMeter.Count;

            //for (int j = 0; j < CommonClass.listDIC[0].listMeter.Count; j++)
            //{
            //    if (CommonClass.listDIC[0].listMeter[j].GetMeter(fScale) == lStartIndexMF)
            //    {
            //        iMeterFindStart = j;
            //        break;
            //    }
            //}
            //if (iMeterFindType == 2)
            //{
            //    for (int j = 0; j < CommonClass.listDIC[0].listMeter.Count; j++)
            //    {
            //        if (CommonClass.listDIC[0].listMeter[j].lPosition == lEndIndexMF)
            //        {
            //            iMeterFindEnd = j;
            //            break;
            //        }
            //    }
            //}

            int len = 0;
            int vStart = (iMeterFindStart) * CommonClass.iLayerAreaWidth / sum;
            int vEnd = (iMeterFindEnd) * CommonClass.iLayerAreaWidth / sum;
            len = vEnd - vStart;

            //if (iMeterFindType == 1)
            //{
            //    len = CommonClass.iLayerAreaWidth / 100;
            //}
            //if (iMeterFindType == 2)
            //{
            //    int vEnd = (iMeterFindEnd + sum / 200) * CommonClass.iLayerAreaWidth / sum;
            //    len = vEnd - vStart;
            //}


            SolidBrush sbMeterFindBG = new SolidBrush(Color.FromArgb(100, 255, 125, 255));
            if (iMeterFindStart != 0)
            {
                g1.FillRectangle(sbMeterFindBG, new Rectangle(vStart, publicHeight, len,
                    MainGraphicsPictureBox1.ClientSize.Height - publicHeight));
            }

            #endregion

            return bmp;            
        }
        #endregion

        //处理主绘图控件上的事件，根据绘图类型确定绘图点
        private void MFPictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (自动滚动ToolStripMenuItem.Checked)
            {
                自动滚动ToolStripMenuItem.Checked = false;
                自动滚动ToolStripMenuItem_Click(sender, e);
            }
            ////确定放大区域
            if (e.Button == MouseButtons.Right)
            {
                if (bZoomInOut)
                {
                    bZoomInOut = false;
                    bZoomInOutMove = true;
                    iZoomInOutY = 0;
                    ReCalcAxis();
                    MainGraphicsPictureBox1.Invalidate();
                }
            }
            if (区域放大ToolStripMenuItem.Checked && e.Button == MouseButtons.Left)
            {
                lastMovePoint.X = e.X;
                lastMovePoint.Y = e.Y;
                startMovePoint.X = e.X;
                startMovePoint.Y = e.Y;
                MainGraphicsPictureBox1.Capture = true;
                MainGraphicsPictureBox1.Refresh();
                wasMove = true;
                ControlPaint.DrawReversibleFrame(MainGraphicsPictureBox1.RectangleToScreen(GetNormalizedRectangle(startMovePoint, lastMovePoint)), Color.DarkRed, FrameStyle.Dashed);
            }
            if (距离测量ToolStripMenuItem.Checked && e.Button == MouseButtons.Left)
            {
                lastMeterageLengthPoint.X = e.X;
                lastMeterageLengthPoint.Y = e.Y;
                startMeterageLengthPoint.X = e.X;
                startMeterageLengthPoint.Y = e.Y;
                //MainGraphicsPictureBox1.Capture = true;
                //MainGraphicsPictureBox1.Refresh();
                wasMeterageLength = true;
                //ControlPaint.DrawReversibleLine(MainGraphicsPictureBox1.PointToScreen(startMeterageLengthPoint), MainGraphicsPictureBox1.PointToScreen(lastMeterageLengthPoint), Color.DarkRed);
            }


            //设置特征点
            if (wasIndex)
            {
                if (CommonClass.listDIC.Count == 0)
                {
                    return;
                }

                int i = ((int)(e.X / ((MainGraphicsPictureBox1.ClientSize.Width - CommonClass.ChannelsAreaWidth) / 1.0 / CommonClass.listDIC[0].listMeter.Count)));
                if (i >= CommonClass.listDIC[0].listMeter.Count)
                {
                    i = CommonClass.listDIC[0].listMeter.Count - 1;
                }

                using (IndexMainInfoAddForm iiaf = new IndexMainInfoAddForm(CommonClass.listDIC[0].listMeter[i].lPosition,
                    CommonClass.listDIC[0].listMeter[i].Km, CommonClass.listDIC[0].listMeter[i].Meter / (CommonClass.listDIC[0].bIndex ? 1 : CommonClass.listDIC[0].fScale[1])))
                {
                    DialogResult dr = iiaf.ShowDialog();
                    if (dr == DialogResult.OK && iiaf.Tag != null)
                    {
                        string sMeter = iiaf.Tag.ToString();
                        if (fViewIndexForm.Visible)
                        {
                            fViewIndexForm.SetIndexInfo(CommonClass.listDIC[0].listMeter[i].lPosition,
                    CommonClass.listDIC[0].listMeter[i].Km, CommonClass.listDIC[0].listMeter[i].Meter / (CommonClass.listDIC[0].bIndex ? 1 : CommonClass.listDIC[0].fScale[1]),
                    sMeter);
                        }
                    }

                }
            }
            else if (e.Button == MouseButtons.Left)
            {
                //数据对象
                for (int i = 0; i < CommonClass.listDIC.Count; i++)
                {
                    if (!CommonClass.listDIC[i].bVisible || bZoomInOutMove == false)
                    {
                        continue;
                    }
                    for (int j = 0; j < CommonClass.listDIC[i].listCC.Count; j++)
                    {
                        if (CommonClass.listDIC[i].listCC[j].Rect.Contains(e.X, e.Y - CommonClass.CoordinatesHeight) &&
                            CommonClass.listDIC[i].listCC[j].Visible)
                        {
                            string sName = CommonClass.listDIC[i].listCC[j].Name;
                            ChannelsDragYMargin = e.Y - CommonClass.listDIC[i].listCC[j].Rect.Y;
                            ChannelsDragingID1 = i;
                            ChannelsDragingID2 = j;
                            ChannelsDraging = true;
                            ChannelsType = 1;
                            //处理各种情况
                            if (同名通道拖动ToolStripMenuItem.Checked)
                            {
                                for (i = 0; i < CommonClass.listDIC.Count; i++)
                                {
                                    if (!CommonClass.listDIC[i].bVisible)
                                    {
                                        continue;
                                    }
                                    for (j = 0; j < CommonClass.listDIC[i].listCC.Count; j++)
                                    {
                                        if (CommonClass.listDIC[i].listCC[j].Name.Equals(sName) &&
                                            CommonClass.listDIC[i].listCC[j].Visible && (ChannelsDragingID1 != i))
                                        {
                                            ChannelsDragClass cd = new ChannelsDragClass();
                                            cd.ChannelsDragYMargin = e.Y - CommonClass.listDIC[i].listCC[j].Rect.Y;
                                            cd.ChannelsDragingID1 = i;
                                            cd.ChannelsDragingID2 = j;
                                            listChannelsDrag.Add(cd);
                                        }


                                    }

                                }
                            }
                            else if (同基线通道拖动ToolStripMenuItem.Checked)
                            {
                                for (i = 0; i < CommonClass.listDIC.Count; i++)
                                {
                                    if (!CommonClass.listDIC[i].bVisible)
                                    {
                                        continue;
                                    }
                                    for (j = 0; j < CommonClass.listDIC[i].listCC.Count; j++)
                                    {
                                        if (CommonClass.listDIC[i].listCC[j].Rect.Contains(e.X, e.Y - CommonClass.CoordinatesHeight) &&
                                            CommonClass.listDIC[i].listCC[j].Visible)
                                        {
                                            if ((ChannelsDragingID1 == i) && (ChannelsDragingID2 == j))
                                            {
                                                continue;
                                            }
                                            ChannelsDragClass cd = new ChannelsDragClass();
                                            cd.ChannelsDragYMargin = e.Y - CommonClass.listDIC[i].listCC[j].Rect.Y;
                                            cd.ChannelsDragingID1 = i;
                                            cd.ChannelsDragingID2 = j;
                                            listChannelsDrag.Add(cd);
                                        }


                                    }

                                }
                            }
                            return;
                        }
                    }

                }
            }
            else if (e.Button == MouseButtons.Right)
            {
                for (int i = 0; i < CommonClass.listDIC.Count; i++)
                {
                    if (!CommonClass.listDIC[i].bVisible)
                    {
                        continue;
                    }
                    for (int j = 0; j < CommonClass.listDIC[i].listCC.Count; j++)
                    {
                        if (CommonClass.listDIC[i].listCC[j].Rect.Contains(e.X, e.Y - CommonClass.CoordinatesHeight) &&
                            CommonClass.listDIC[i].listCC[j].Visible)
                        {
                            CallChannelsDialog(i, j);
                            break;
                        }
                    }

                }

            }

        }

        #region 调用控件--波形数据图层通道设置控件
        /// <summary>
        /// 调用控件--波形数据图层通道设置控件
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        private void CallChannelsDialog(int i, int j)
        {
            if (CommonClass.listDIC.Count > 0)
            {
                Point p = new Point(i, j);
                dChannelsDialog.Tag = p;
                dChannelsDialog.Owner = this;
                dChannelsDialog.TopLevel = true;

                dChannelsDialog.Show();
                dChannelsDialog.Activate();
            }
        }
        #endregion

        //处理主绘图控件上的事件，根据绘图类型更新指定的图形
        private void MainGraphicsPictureBox1_MouseMove(object sender, MouseEventArgs e)
        {

            if (区域放大ToolStripMenuItem.Checked && wasMove)//绘制矩形
            {
                Point point = new Point(e.X, e.Y);
                Point oldPoint = lastMovePoint;

                lastMovePoint.X = e.X;
                lastMovePoint.Y = e.Y;

                ControlPaint.DrawReversibleFrame(
                        MainGraphicsPictureBox1.RectangleToScreen(GetNormalizedRectangle(startMovePoint, oldPoint)),
                         Color.DarkRed, FrameStyle.Dashed);
                ControlPaint.DrawReversibleFrame(
                        MainGraphicsPictureBox1.RectangleToScreen(GetNormalizedRectangle(startMovePoint, point)),
                         Color.DarkRed, FrameStyle.Dashed);
            }

            if (距离测量ToolStripMenuItem.Checked && wasMeterageLength)//绘制线
            {
                Point point = new Point(e.X, e.Y);
                Point oldPoint = lastMeterageLengthPoint;

                lastMeterageLengthPoint.X = e.X;
                lastMeterageLengthPoint.Y = e.Y;
                ControlPaint.DrawReversibleLine(MainGraphicsPictureBox1.PointToScreen(startMeterageLengthPoint), MainGraphicsPictureBox1.PointToScreen(oldPoint), Color.DarkRed);
                ControlPaint.DrawReversibleLine(MainGraphicsPictureBox1.PointToScreen(startMeterageLengthPoint), MainGraphicsPictureBox1.PointToScreen(point), Color.DarkRed);

            }
            if (ChannelsDraging)
            {

                //if (e.Y % 3 == 0)
                //{
                switch (ChannelsType)
                {
                    case 1:
                        int iDY = (e.Y - ChannelsDragYMargin + CommonClass.ChannelsAreaHeight / 2);
                        if (iDY > 0 && iDY < MainGraphicsPictureBox1.ClientSize.Height - CommonClass.CoordinatesHeight)
                        {
                            int iIndex = iDY / (int)CommonClass.CoordinatesYGrid;
                            if (iIndex < 1)
                            {
                                iIndex = 1;
                            }
                            CommonClass.listDIC[ChannelsDragingID1].listCC[ChannelsDragingID2].Location = iIndex;

                        }

                        //判断其他可能
                        if (listChannelsDrag.Count > 0)
                        {
                            for (int i = 0; i < listChannelsDrag.Count; i++)
                            {
                                int iArrayDY = e.Y - listChannelsDrag[i].ChannelsDragYMargin + CommonClass.ChannelsAreaHeight / 2;
                                if (iArrayDY > 0 && iArrayDY < MainGraphicsPictureBox1.ClientSize.Height - CommonClass.CoordinatesHeight)
                                {
                                    int iIndex = iArrayDY / (int)CommonClass.CoordinatesYGrid;
                                    if (iIndex < 1)
                                    {
                                        iIndex = 1;
                                    }
                                    CommonClass.listDIC[listChannelsDrag[i].ChannelsDragingID1].listCC[listChannelsDrag[i].ChannelsDragingID2].Location = iIndex;

                                }
                            }
                        }

                        break;
                    case 2:
                        int iDBY = (e.Y - ChannelsDragYMargin + CommonClass.ChannelsAreaHeight / 2);
                        if (iDBY > 0 && iDBY < MainGraphicsPictureBox1.ClientSize.Height - CommonClass.CoordinatesHeight)
                        {
                            int iIndex = iDBY / (int)CommonClass.CoordinatesYGrid;
                            if (iIndex < 1)
                            {
                                iIndex = 1;
                            }
                            CommonClass.listDBC[ChannelsDragingID1].listDBCC[ChannelsDragingID2].Location = iIndex;

                        }
                        break;
                }
                MainGraphicsPictureBox1.Invalidate();
                SaveChannels();

            }
            //绘制数据测量时的鼠标十
            else if ((!MeterageNoToolStripMenuItem1.Checked) && 距离测量ToolStripMenuItem.Checked == false)
            {
                if (e.X > MainGraphicsPictureBox1.ClientRectangle.Width - CommonClass.ChannelsAreaWidth
                    || e.Y < CommonClass.CoordinatesHeight)
                {
                    MainGraphicsPictureBox1.Cursor = Cursors.Default;
                }
                else
                {
                    MainGraphicsPictureBox1.Cursor = Cursors.Cross;
                }
                MainGraphicsPictureBox1.Invalidate();
            }
            else if (MainGraphicsPictureBox1.Cursor == Cursors.Default)
            {

                for (int i = 0; i < CommonClass.listDIC.Count; i++)
                {
                    if (CommonClass.listDIC[i].listLIC != null)
                    {
                        for (int j = 0; j < CommonClass.listDIC[i].listLIC.Count; j++)
                        {
                            if (CommonClass.listDIC[i].listLIC[j].r.Contains(e.X, e.Y))
                            {
                                LabelInfoToolTip1.SetToolTip(MainGraphicsPictureBox1, CommonClass.listDIC[i].listLIC[j].sMemoText);
                                break;
                            }
                        }

                    }
                }
            }
        }

        private void SaveChannels()
        {
            try
            {
                if (CommonClass.listDIC.Count > 0)
                {
                    for (int i = 0; i < CommonClass.listDIC.Count; i++)
                    {
                        CommonClass.SaveChannelsConfig(i);
                    }
                }
            }
            catch
            {

            }
        }

        //处理主绘图控件上的事件，根据绘图类型擦除指定的图形
        private void MFPictureBox_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                if (区域放大ToolStripMenuItem.Checked && wasMove && e.Button == MouseButtons.Left)
                {
                    区域放大ToolStripMenuItem.Checked = false;
                    Rectangle r = GetNormalizedRectangle(startMovePoint, lastMovePoint);
                    int iHeight = this.MainGraphicsPictureBox1.ClientSize.Height - CommonClass.CoordinatesHeight;
                    if ((r.Height + r.Y) > iHeight)
                    {
                        r.Height = iHeight - r.Y;
                    }
                    int iCount = r.Height / (int)CommonClass.CoordinatesYGrid;

                    int iCorrd = iHeight / iCount;
                    bZoomInOut = true;
                    bZoomInOutMove = false;
                    iZoomInOutY = (r.Y - CommonClass.CoordinatesHeight) / (int)CommonClass.CoordinatesYGrid;
                    CommonClass.CoordinatesYGrid = iCorrd;
                    //处理X轴
                    iXLeft = r.X;
                    iXRight = r.Width + r.X;
                    if (iXLeft < 0)
                    {
                        iXLeft = 0;
                    }
                    if (iXRight > CommonClass.iLayerAreaWidth)
                    {
                        iXRight = CommonClass.iLayerAreaWidth - 1;
                    }

                }
            }
            catch
            {

            }
            //长度测量
            try
            {
                if (距离测量ToolStripMenuItem.Checked && wasMeterageLength && e.Button == MouseButtons.Left)
                {
                    int iSPointX = ((int)(startMeterageLengthPoint.X / (CommonClass.iLayerAreaWidth / 1.0 / CommonClass.listDIC[0].listMeter.Count)));

                    if (iSPointX >= CommonClass.listDIC[0].listMeter.Count)
                    {
                        iSPointX = CommonClass.listDIC[0].listMeter.Count - 1;
                    }
                    int iEPointX = ((int)(lastMeterageLengthPoint.X / (CommonClass.iLayerAreaWidth / 1.0 / CommonClass.listDIC[0].listMeter.Count)));

                    if (iEPointX >= CommonClass.listDIC[0].listMeter.Count)
                    {
                        iEPointX = CommonClass.listDIC[0].listMeter.Count - 1;
                    }
                    MessageBox.Show("从K" + CommonClass.listDIC[0].listMeter[iSPointX].Km.ToString()
                        + "+" + (CommonClass.listDIC[0].listMeter[iSPointX].Meter / (CommonClass.listDIC[0].bIndex ? 1 : CommonClass.listDIC[0].fScale[1])).ToString() + " \n到K"
                        + CommonClass.listDIC[0].listMeter[iEPointX].Km.ToString()
                        + "+" + (CommonClass.listDIC[0].listMeter[iEPointX].Meter / (CommonClass.listDIC[0].bIndex ? 1 : CommonClass.listDIC[0].fScale[1])).ToString() + "\n长度为:" +
                        Math.Abs((CommonClass.listDIC[0].listMeter[iSPointX].GetMeter((CommonClass.listDIC[0].bIndex ? 1 : CommonClass.listDIC[0].fScale[1])) -
                        CommonClass.listDIC[0].listMeter[iEPointX].GetMeter((CommonClass.listDIC[0].bIndex ? 1 : CommonClass.listDIC[0].fScale[1])))).ToString("f3") + "米\n包含采样点数:" +
                        (((CommonClass.listDIC[0].listMeter[iEPointX].lPosition - CommonClass.listDIC[0].listMeter[iSPointX].lPosition) / (CommonClass.listDIC[0].iChannelNumber * 2))).ToString(""));

                }
            }
            catch
            {

            }

            wasMove = false;
            wasMeterageLength = false;

            ChannelsDragYMargin = -1;
            ChannelsDragingID1 = -1;
            ChannelsDragingID2 = -1;
            ChannelsDraging = false;
            ChannelsType = 0;
            listChannelsDrag.Clear();

            MainGraphicsPictureBox1.Invalidate();
            if ((!MeterageNoToolStripMenuItem1.Checked) && MainGraphicsPictureBox1.Cursor == Cursors.Cross && e.Button == MouseButtons.Left && (!距离测量ToolStripMenuItem.Checked))//测量位置
            {
                //测量数据
                using (MeterageForm mf = new MeterageForm())
                {
                    int iWidth = MainGraphicsPictureBox1.ClientSize.Width - CommonClass.ChannelsAreaWidth;
                    Point p = new Point();
                    p.X = e.X;
                    p.Y = e.Y;
                    int iChecked = 0;
                    if (同通道名测量ToolStripMenuItem.Checked)
                    {
                        iChecked = 1;
                    }
                    else if (同基线测量ToolStripMenuItem.Checked)
                    {
                        iChecked = 2;
                    }
                    else if (同一层测量ToolStripMenuItem.Checked)
                    {
                        //同一层测量
                        iChecked = 3;
                    }
                    else
                    {
                        //断面线测量
                        iChecked = 4;
                    }
                    mf.Tag = iChecked.ToString() + "," + p.X.ToString() + "," + p.Y.ToString() + "," + iWidth.ToString();
                    mf.ShowDialog();
                }
            }
        }

        private Rectangle GetNormalizedRectangle(int x1, int y1, int x2, int y2)
        {
            if (x2 < x1)
            {
                x1 ^= x2;
                x2 ^= x1;
                x1 ^= x2;
            }

            if (y2 < y1)
            {
                y1 ^= y2;
                y2 ^= y1;
                y1 ^= y2;
            }

            return new Rectangle(x1, y1, x2 - x1, y2 - y1);
        }

        private Rectangle GetNormalizedRectangle(Point p1, Point p2)
        {
            return GetNormalizedRectangle(p1.X, p1.Y, p2.X, p2.Y);
        }

        private Rectangle GetNormalizedRectangle(Rectangle r)
        {
            return GetNormalizedRectangle(r.X, r.Y, r.X + r.Width, r.Y + r.Height);
        }

        

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            
            try
            {
                if (state == CONNECTED)
                {
                    string message = "EXIT|" + CommonClass.ID + "|";
                    //将字符串转化为字符数组
                    byte[] outbytes = System.Text.Encoding.Default.GetBytes(
                        message.ToCharArray());
                    CommonClass.MainNetworkStream.Write(outbytes, 0, outbytes.Length);

                    this.state = CLOSED;
                    this.stopFlag = true;
                }
            }
            catch
            {
                //MessageBox.Show(ex.Message);

                this.state = CLOSED;
                this.stopFlag = true;
            }
            //保存配置文件
            SaveChannels();
            try
            {
                XmlDocument xd = new XmlDocument();
                xd.Load(CommonClass.AppConfigPath);
                xd.DocumentElement["default"].ChildNodes[0].InnerText = CommonClass.sLastSelectPath;
                xd.Save(CommonClass.AppConfigPath);
            }
            catch
            {

            }
            try
            {

                CommonClass.MainNetworkStream.Close();
                CommonClass.MainTcpClient.Close();
                if (CommonClass.AutoNetworkStream != null)
                {
                    CommonClass.AutoNetworkStream.Close();
                    CommonClass.AutoThread.Abort();
                }
                CommonClass.MainThread.Abort();

            }
            catch
            {

            }
            finally
            {
                notifyIcon1.Visible = false;
                //Environment.Exit(Environment.ExitCode);
                e.Cancel = false;
            }

        }

        

        private bool GetDataCount()
        {
            if (CommonClass.listDIC.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        

        private void MFHScrollBar_Scroll(object sender, ScrollEventArgs e)
        {

            try
            {
                switch (e.Type)
                {
                        
                    case ScrollEventType.ThumbTrack:
                        try
                        {
                            //实时显示里程信息
                            MileageInfoLabel1.Visible = true;
                            if (CommonClass.DataMileageInfo != null)
                            {
                                MileageInfoLabel1.Text = (CommonClass.DataMileageInfo[MainHScrollBar1.Value * 4 * (CommonClass.XZoomIn / CommonClass.XZoomInDivisor)] / 1000.0).ToString("F03") + " - " +
                                    (CommonClass.DataMileageInfo[(MainHScrollBar1.Value * 4 * (CommonClass.XZoomIn / CommonClass.XZoomInDivisor)) + (CommonClass.XZoomIn) - 1] / 1000.0).ToString("F03");
                            }
                        }
                        catch
                        {

                        }
                        break;
                    case ScrollEventType.EndScroll:
                        MileageInfoLabel1.Visible = false;
                        for (int k = 0; k < CommonClass.listDIC.Count; k++)
                        {
                            List<int> listChannelsVisible = new List<int>();
                            StringBuilder sb = new StringBuilder();
                            for (int i = 0; i < CommonClass.listDIC[k].listCC.Count; i++)
                            {
                                if (CommonClass.listDIC[k].listCC[i].Visible)
                                {
                                    listChannelsVisible.Add(CommonClass.listDIC[k].listCC[i].Id);
                                    sb.Append(CommonClass.listDIC[k].listCC[i].Id.ToString() + ",");
                                }
                            }
                            sb.Remove(sb.Length - 1, 1);
                            float[][] arrayPointF = new float[listChannelsVisible.Count][];

                            for (int i = 0; i < listChannelsVisible.Count; i++)
                            {
                                arrayPointF[i] = new float[CommonClass.listDIC[k].iSmaleRate * CommonClass.XZoomIn];
                                //arrayPointF[i] = new PointF[800];
                            }


                            //
                            if (CommonClass.listDIC[k].iAppMode == 0)
                            {
                                if (CommonClass.listDIC[k].bIndex && k != 0)
                                {
                                    CommonClass.wdp.GetDataInfo(listChannelsVisible, ref arrayPointF, ref CommonClass.listDIC[k].listMeter, CommonClass.listDIC[k].lReviseValue,
                                    CommonClass.listDIC[k].sFilePath, CommonClass.listDIC[0].listMeter[0].GetMeter(1), CommonClass.XZoomIn,
                                    CommonClass.listDIC[k].iSmaleRate, CommonClass.listDIC[k].iChannelNumber, CommonClass.listDIC[k].lStartPosition, CommonClass.listDIC[k].lEndPosition, CommonClass.listDIC[k].listIC, CommonClass.listDIC[k].bIndex, CommonClass.listDIC[k].sKmInc, CommonClass.listDIC[k].bEncrypt, CommonClass.listDIC[k].bReverse, (CommonClass.listDIC[k].bIndex && k != 0) ? true : false, CommonClass.listDIC[k].iDataType);
                                }
                                else
                                {
                                    CommonClass.wdp.GetDataStartPositionEndPositionInfoIncludeIndex(ref CommonClass.listDIC[k].lStartPosition,
                                        ref CommonClass.listDIC[k].lEndPosition, CommonClass.listDIC[k].sFilePath,
                                        CommonClass.listDIC[k].iChannelNumber, CommonClass.listDIC[k].lStartPosition, CommonClass.listDIC[k].lEndPosition, CommonClass.listDIC[k].bIndex);
                                    CommonClass.wdp.GetDataInfo(listChannelsVisible, ref arrayPointF, ref CommonClass.listDIC[k].listMeter, CommonClass.listDIC[k].lReviseValue,
                                    CommonClass.listDIC[k].sFilePath, MainHScrollBar1.Value, CommonClass.XZoomIn,
                                    CommonClass.listDIC[k].iSmaleRate, CommonClass.listDIC[k].iChannelNumber, CommonClass.listDIC[k].lStartPosition, CommonClass.listDIC[k].lEndPosition, CommonClass.listDIC[k].listIC, CommonClass.listDIC[k].bIndex, CommonClass.listDIC[k].sKmInc, CommonClass.listDIC[k].bEncrypt, CommonClass.listDIC[k].bReverse, (CommonClass.listDIC[k].bIndex && k != 0) ? true : false, CommonClass.listDIC[k].iDataType);
                                }
                            }
                            else
                            {
                                CommonClass.NWDataStartPositionEndPositionInfoIncludeIndex(ref CommonClass.listDIC[k].lStartPosition, ref CommonClass.listDIC[k].lEndPosition, CommonClass.listDIC[k].sFilePath, 0, 0, CommonClass.listDIC[k].iSmaleRate, CommonClass.listDIC[k].iChannelNumber, CommonClass.listDIC[k].lStartPosition, CommonClass.listDIC[k].lEndPosition, CommonClass.listDIC[k].bIndex);
                                CommonClass.NWGetData(CommonClass.listDIC[k].iDataType, sb.ToString(), ref arrayPointF, ref CommonClass.listDIC[k].listMeter, CommonClass.listDIC[k].lReviseValue,
                            CommonClass.listDIC[k].sFilePath, MainHScrollBar1.Value, CommonClass.XZoomIn, CommonClass.listDIC[k].iSmaleRate, CommonClass.listDIC[k].iChannelNumber,
                            CommonClass.listDIC[k].lStartPosition, CommonClass.listDIC[k].lEndPosition, CommonClass.listDIC[k].listIC, CommonClass.listDIC[k].bIndex, CommonClass.listDIC[k].sKmInc, CommonClass.listDIC[k].bEncrypt, CommonClass.listDIC[k].bReverse);
                            }

                            SystemInfoToolStripStatusLabel1.Text = "读取数据完成";
                            for (int n = 0; n < listChannelsVisible.Count; n++)
                            {
                                for (int m = 0; m < CommonClass.listDIC[k].listCC.Count; m++)
                                {
                                    if (CommonClass.listDIC[k].listCC[m].Id == listChannelsVisible[n])
                                    {
                                        CommonClass.listDIC[k].listCC[m].Data = arrayPointF[n];
                                        break;
                                    }
                                }
                            }
                        }
                        MainGraphicsPictureBox1.Invalidate();
                        SecondGraphicsPictureBox1.Invalidate();

                        SetHScrollBarMaximum();

                        //设置提示
                        SetTipOnMenu();

                        //设置台帐位置
                        if (fEquipmentInfoForm.Visible && fEquipmentInfoForm.toolStripComboBox1.SelectedIndex == 0)
                        {

                            if (CommonClass.listDIC[0].sKmInc.Equals("增"))
                            {
                                fEquipmentInfoForm.StartMeterToolStripTextBox1.Text = ((CommonClass.listDIC[0].listMeter[0].GetMeter(CommonClass.listDIC[0].fScale[1]) - 10000) / 1000.0).ToString();
                                fEquipmentInfoForm.EndMeterToolStripTextBox1.Text = ((CommonClass.listDIC[0].listMeter[0].GetMeter(CommonClass.listDIC[0].fScale[1]) + 10000) / 1000.0).ToString();
                            }
                            else
                            {
                                fEquipmentInfoForm.EndMeterToolStripTextBox1.Text = ((CommonClass.listDIC[0].listMeter[0].GetMeter(CommonClass.listDIC[0].fScale[1]) + 10000) / 1000.0).ToString();
                                fEquipmentInfoForm.StartMeterToolStripTextBox1.Text = ((CommonClass.listDIC[0].listMeter[0].GetMeter(CommonClass.listDIC[0].fScale[1]) - 10000) / 1000.0).ToString();
                            }
                            fEquipmentInfoForm.SetData();
                        }
                        //设置联动

                        break;

                }
            }
            catch
            {
            }

        }

        public void SetTipOnMenu()
        {
            try
            {
                for (int i = 0; i < 已加载的数据图层ToolStripDropDownButton.DropDownItems.Count; i++)
                {
                    if(CommonClass.listDIC[i].bIndex)
                    {
                        已加载的数据图层ToolStripDropDownButton.DropDownItems[i].ForeColor = Color.Black;
                        已加载的数据图层ToolStripDropDownButton.DropDownItems[i].Text ="(里程已修正) ";
                    }
                    else
                    {
                        已加载的数据图层ToolStripDropDownButton.DropDownItems[i].ForeColor = Color.Red;
                        已加载的数据图层ToolStripDropDownButton.DropDownItems[i].Text ="(里程未修正) ";
                    }
                    
                    已加载的数据图层ToolStripDropDownButton.DropDownItems[i].Text += ( CommonClass.listDIC[i].Name + " " + (CommonClass.listDIC[i].listMeter[0].GetMeter(CommonClass.listDIC[i].bIndex ? 1 : CommonClass.listDIC[i].fScale[1]) / 1000.0).ToString());
                    fLayerTranslationForm.LayerDataGridView1.Rows[i].Cells[4].Value = (CommonClass.listDIC[i].listMeter[0].GetMeter(CommonClass.listDIC[i].bIndex ? 1 : CommonClass.listDIC[i].fScale[1]) / 1000.0).ToString();
                }
            }
            catch
            {

            }
        }

        

        private void ZoomInPicture_Click(object sender, EventArgs e)
        {
            MainGraphicsPictureBox1.Show();
            MainHScrollBar1.Enabled = true;
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {

            //主层移动快捷
            if (e.Alt && e.KeyCode == Keys.Left)
            {
                SetMainLayerDataValue(0, 0);
                return;
            }
            else if (e.Alt && e.KeyCode == Keys.Right)
            {
                SetMainLayerDataValue(1, 0);
                return;
            }
            else if (e.Control && e.KeyCode == Keys.Left)
            {
                SetMainLayerDataValue(0, 1);
                return;
            }
            else if (e.Control && e.KeyCode == Keys.Right)
            {
                SetMainLayerDataValue(1, 1);
                return;
            }
            //其他层移动快捷
            switch (e.KeyCode)
            {
                case Keys.D1:
                case Keys.D2:
                case Keys.D3:
                case Keys.D4:
                case Keys.D5:
                case Keys.D6:
                case Keys.D7:
                case Keys.D8:
                case Keys.D9:
                case Keys.NumPad1:
                case Keys.NumPad2:
                case Keys.NumPad3:
                case Keys.NumPad4:
                case Keys.NumPad5:
                case Keys.NumPad6:
                case Keys.NumPad7:
                case Keys.NumPad8:
                case Keys.NumPad9:

                    break;

            }


        }

        /// <summary>
        /// 设置主层移动的块捷键
        /// </summary>
        /// <param name="l"></param>
        /// 
        public void SetMainLayerDataValue(int iDir, int iValue)
        {
            if (!MainForm.sMainform.Enabled)
            {
                return;
            }
            ScrollEventArgs sea;
            int iCalcValue = 0;
            if (iValue == 0)
            {
                iCalcValue = MainHScrollBar1.SmallChange;
            }
            else
            {
                iCalcValue = MainHScrollBar1.LargeChange;
            }
            if (iDir == 0)
            {

                if (MainHScrollBar1.Value - iCalcValue < MainHScrollBar1.Minimum)
                {
                    return;
                }
                MainHScrollBar1.Value -= iCalcValue;
                sea = new ScrollEventArgs(ScrollEventType.EndScroll,
                   MainHScrollBar1.Value);
            }
            else
            {
                if (MainHScrollBar1.Value + iCalcValue > MainHScrollBar1.Maximum)
                {
                    return;
                }
                MainHScrollBar1.Value += iCalcValue;
                sea = new ScrollEventArgs(ScrollEventType.EndScroll,
                    MainHScrollBar1.Value);
            }
            object sender = new object();
            MFHScrollBar_Scroll(sender, sea);
        }

        private void printDocument1_PrintPage(object sender, PrintPageEventArgs e)
        {
            try
            {
                //PrintDocument pd = (PrintDocument)sender;

                Rectangle r = new Rectangle(0, 0, this.Width, this.Height - MainMenuStrip1.Height - MainHScrollBar1.Height + 1);
                //Bitmap b = new Bitmap(this.Width, this.Height - MainMenuStrip1.Height - MainHScrollBar1.Height + 1, PixelFormat.Format32bppArgb);
                //this.DrawToBitmap(b, r);
                //e.Graphics.DrawImage(b, e.Graphics.VisibleClipBounds);
                //e.HasMorePages = false;

                //e.Graphics.PageUnit = GraphicsUnit.Document;
                //e.Graphics.PageScale = 1000;
                //e.Graphics.DrawImage(memoryImage, r, 0, 0, 850, 350, GraphicsUnit.Millimeter); ;




                //e.Graphics.DrawImage(memoryImage, new Rectangle(0, 0, 850, 350));

                e.Graphics.DrawImage(memoryImage, new Rectangle(0, 15, memoryImage.Width * 59 / 100, memoryImage.Height * 81 / 100));

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        

        /// <summary>
        /// 针对一个或多个波形文件，初始化波形，从文件中第一个里程开始显示
        /// </summary>
        private void exeFind()
        {
            try
            {

                SetHScrollBarMaximum();
                for (int i = 0; i < CommonClass.listDIC.Count; i++)
                {
                    int iMeter;
                    if (CommonClass.listDIC[i].listMeter.Count < 1)
                    {
                        iMeter = 0;
                    }
                    else
                    {
                        //iMeter单位为米
                        iMeter = CommonClass.listDIC[i].listMeter[0].GetMeter(CommonClass.listDIC[i].bIndex ? 1 : CommonClass.listDIC[i].fScale[1]);
                    }
                    MeterFind(iMeter / 1000f);
                }

            }
            catch
            {

            }
        }


        /// <summary>
        /// 屏幕是否显示波形定位的位置---20141218-ygx
        /// </summary>
        private Boolean bDisPlayMeterFind = false;
        private long lStartIndexMF = 0;
        private long lEndIndexMF = 0;
        private int iMeterFindType = 0;//1--代表定位后画出固定长度的框框，2--代表定位后画出规定长度的框框

        #region 输入公里标(单位:公里)，计算出拖动条的值，同时移动波形
        /// <summary>
        /// 输入公里标(单位:公里)，计算出拖动条的值，同时移动波形
        /// </summary>
        /// <param name="f">公里标(单位:公里)</param>
        public void MeterFind(float f)
        {
            int i = 0;
            switch (CommonClass.listDIC[0].iAppMode)
            {
                case 0:
                    i = CommonClass.wdp.AnalyseDataAutoFindCorLC_New(CommonClass.listDIC[0].sFilePath, (long)(f * 1000), CommonClass.XZoomIn, CommonClass.listDIC[0].iSmaleRate, CommonClass.listDIC[0].iChannelNumber, 0, CommonClass.listDIC[0].bEncrypt,out lStartIndexMF);
                    break;
                case 1:
                    i = CommonClass.NWGetMeterFind(CommonClass.listDIC[0].sFilePath, (long)(f * 1000), CommonClass.XZoomIn, CommonClass.listDIC[0].iSmaleRate, CommonClass.listDIC[0].iChannelNumber, 0, CommonClass.listDIC[0].bEncrypt);
                    break;
            }
            if (i > 10)
            { 
                i -= 5;
            }
            else
            {
                i -= i / 2;
            }
            bDisPlayMeterFind = true;
            iMeterFindType = 1;
            SetValueMFHScrollBar(i);
            ScrollEventArgs sea = new ScrollEventArgs(ScrollEventType.EndScroll, i);
            MFHScrollBar_Scroll(new Object(), sea);
        }
        #endregion

        #region 输入公里标(单位:公里)，计算出拖动条的值，同时移动波形 ----20150317--ygx
        /// <summary>
        /// 输入公里标(单位:公里)，计算出拖动条的值，同时移动波形
        /// </summary>
        /// <param name="f">公里标(单位:公里)</param>
        public void MeterFind(float fStartMile,float fEndMile)
        {
            int i = 0;
            switch (CommonClass.listDIC[0].iAppMode)
            {
                case 0:
                    i = CommonClass.wdp.AnalyseDataAutoFindCorLC_New(CommonClass.listDIC[0].sFilePath, (long)(fStartMile * 1000), CommonClass.XZoomIn, CommonClass.listDIC[0].iSmaleRate, CommonClass.listDIC[0].iChannelNumber, 0, CommonClass.listDIC[0].bEncrypt, out lStartIndexMF);
                    break;
                case 1:
                    i = CommonClass.NWGetMeterFind(CommonClass.listDIC[0].sFilePath, (long)(fStartMile * 1000), CommonClass.XZoomIn, CommonClass.listDIC[0].iSmaleRate, CommonClass.listDIC[0].iChannelNumber, 0, CommonClass.listDIC[0].bEncrypt);
                    break;
            }
            if (i > 10)
            {
                i -= 5;
            }
            else
            {
                i -= i / 2;
            }

            int j = CommonClass.wdp.AnalyseDataAutoFindCorLC_New(CommonClass.listDIC[0].sFilePath, (long)(fEndMile * 1000), CommonClass.XZoomIn, CommonClass.listDIC[0].iSmaleRate, CommonClass.listDIC[0].iChannelNumber, 0, CommonClass.listDIC[0].bEncrypt, out lEndIndexMF);
            iMeterFindType = 2;

            bDisPlayMeterFind = true;
            SetValueMFHScrollBar(i);
            ScrollEventArgs sea = new ScrollEventArgs(ScrollEventType.EndScroll, i);
            MFHScrollBar_Scroll(new Object(), sea);
        }
        #endregion

        #region 输入文件指针，计算出拖动条的值，同时移动波形---精确定位--20140106--ygx
        /// <summary>
        /// 输入文件指针，计算出拖动条的值，同时移动波形---精确定位
        /// </summary>
        /// <param name="pos">文件指针</param>
        public void MeterFind(long pos)
        {
            int i = 0;
            switch (CommonClass.listDIC[0].iAppMode)
            {
                case 0:
                    i = CommonClass.wdp.AnalyseDataAutoFindCorLC(CommonClass.listDIC[0].sFilePath, pos, CommonClass.XZoomIn, CommonClass.listDIC[0].iSmaleRate, CommonClass.listDIC[0].iChannelNumber);
                    break;
                //这里暂时只考虑单机版本
                //case 1:
                //    i = CommonClass.NWGetMeterFind(CommonClass.listDIC[0].sFilePath, (long)(f * 1000), CommonClass.XZoomIn, CommonClass.listDIC[0].iSmaleRate, CommonClass.listDIC[0].iChannelNumber, 0, CommonClass.listDIC[0].bEncrypt);
                //    break;
            }
            if (i > 10)
            {
                i -= 5;
            }
            else
            {
                i -= i / 2;
            }
            lStartIndexMF = pos;
            bDisPlayMeterFind = true;
            iMeterFindType = 1;
            SetValueMFHScrollBar(i);
            ScrollEventArgs sea = new ScrollEventArgs(ScrollEventType.EndScroll, i);
            MFHScrollBar_Scroll(new Object(), sea);
        }
        #endregion

        #region 输入文件指针，计算出拖动条的值，同时移动波形---精确定位--20150317--ygx
        /// <summary>
        /// 输入文件指针，计算出拖动条的值，同时移动波形---精确定位
        /// </summary>
        /// <param name="pos">文件指针</param>
        public void MeterFind(long posStart,long posEnd)
        {
            int i = 0;
            switch (CommonClass.listDIC[0].iAppMode)
            {
                case 0:
                    i = CommonClass.wdp.AnalyseDataAutoFindCorLC(CommonClass.listDIC[0].sFilePath, posStart, CommonClass.XZoomIn, CommonClass.listDIC[0].iSmaleRate, CommonClass.listDIC[0].iChannelNumber);
                    break;
                //这里暂时只考虑单机版本
                //case 1:
                //    i = CommonClass.NWGetMeterFind(CommonClass.listDIC[0].sFilePath, (long)(f * 1000), CommonClass.XZoomIn, CommonClass.listDIC[0].iSmaleRate, CommonClass.listDIC[0].iChannelNumber, 0, CommonClass.listDIC[0].bEncrypt);
                //    break;
            }
            if (i > 10)
            {
                i -= 5;
            }
            else
            {
                i -= i / 2;
            }
            lStartIndexMF = posStart;
            lEndIndexMF = posEnd;
            bDisPlayMeterFind = true;
            iMeterFindType = 2;
            SetValueMFHScrollBar(i);
            ScrollEventArgs sea = new ScrollEventArgs(ScrollEventType.EndScroll, i);
            MFHScrollBar_Scroll(new Object(), sea);
        }
        #endregion

        //里程自动对齐
        #region 波形自动对齐--根据matlab算法，当有多个波形时，把第二层以上波形自动对齐第一层的波形（当前屏幕）--20151106--ygx
        /// <summary>
        /// 波形自动对齐--根据matlab算法，当有多个波形时，把第二层以上波形自动对齐第一层的波形（当前屏幕）
        /// </summary>
        public void AutoTranslation()
        {
            #region 波形1数据的提取
            //波形1数据的提取
            float[][] data_ww = new float[5][];

            //左高低
            for (int j = 0; j < CommonClass.listDIC[0].listCC.Count; j++)
            {
                if (CommonClass.listDIC[0].listCC[j].ChineseName == "左高低_中波" || CommonClass.listDIC[0].listCC[j].Name == "L_Prof_SC")
                {
                    data_ww[0] = CommonClass.listDIC[0].listCC[j].Data;
                    break;
                }
            }
            //右高低
            for (int j = 0; j < CommonClass.listDIC[0].listCC.Count; j++)
            {
                if (CommonClass.listDIC[0].listCC[j].ChineseName == "右高低_中波" || CommonClass.listDIC[0].listCC[j].Name == "R_Prof_SC")
                {
                    data_ww[1] = CommonClass.listDIC[0].listCC[j].Data;
                    break;
                }
            }
            //左轨向
            for (int j = 0; j < CommonClass.listDIC[0].listCC.Count; j++)
            {
                if (CommonClass.listDIC[0].listCC[j].ChineseName == "左轨向_中波" || CommonClass.listDIC[0].listCC[j].Name == "L_Align_SC")
                {
                    data_ww[2] = CommonClass.listDIC[0].listCC[j].Data;
                    break;
                }
            }
            //右轨向
            for (int j = 0; j < CommonClass.listDIC[0].listCC.Count; j++)
            {
                if (CommonClass.listDIC[0].listCC[j].ChineseName == "右轨向_中波" || CommonClass.listDIC[0].listCC[j].Name == "R_Align_SC")
                {
                    data_ww[3] = CommonClass.listDIC[0].listCC[j].Data;
                    break;
                }
            }
            //轨距
            for (int j = 0; j < CommonClass.listDIC[0].listCC.Count; j++)
            {
                if (CommonClass.listDIC[0].listCC[j].ChineseName == "轨距" || CommonClass.listDIC[0].listCC[j].Name == "Gage")
                {
                    data_ww[4] = CommonClass.listDIC[0].listCC[j].Data;
                    break;
                }
            }
            #endregion

            for (int i = 1; i < CommonClass.listDIC.Count; i++)
            {
                int citIndex = i;

                #region 波形2数据的提取
                //波形2数据的提取
                float[][] data_ww1 = new float[5][];

                //左高低
                for (int j = 0; j < CommonClass.listDIC[citIndex].listCC.Count; j++)
                {
                    if (CommonClass.listDIC[citIndex].listCC[j].ChineseName == "左高低_中波" || CommonClass.listDIC[citIndex].listCC[j].Name == "L_Prof_SC")
                    {
                        data_ww1[0] = CommonClass.listDIC[citIndex].listCC[j].Data;
                        break;
                    }
                }
                //右高低
                for (int j = 0; j < CommonClass.listDIC[citIndex].listCC.Count; j++)
                {
                    if (CommonClass.listDIC[citIndex].listCC[j].ChineseName == "右高低_中波" || CommonClass.listDIC[citIndex].listCC[j].Name == "R_Prof_SC")
                    {
                        data_ww1[1] = CommonClass.listDIC[citIndex].listCC[j].Data;
                        break;
                    }
                }
                //左轨向
                for (int j = 0; j < CommonClass.listDIC[citIndex].listCC.Count; j++)
                {
                    if (CommonClass.listDIC[citIndex].listCC[j].ChineseName == "左轨向_中波" || CommonClass.listDIC[citIndex].listCC[j].Name == "L_Align_SC")
                    {
                        data_ww1[2] = CommonClass.listDIC[citIndex].listCC[j].Data;
                        break;
                    }
                }
                //右轨向
                for (int j = 0; j < CommonClass.listDIC[citIndex].listCC.Count; j++)
                {
                    if ((CommonClass.listDIC[citIndex].listCC[j].ChineseName == "右轨向_中波") || (CommonClass.listDIC[citIndex].listCC[j].Name == "R_Align_SC"))
                    {
                        data_ww1[3] = CommonClass.listDIC[citIndex].listCC[j].Data;
                        break;
                    }
                }
                //轨距
                for (int j = 0; j < CommonClass.listDIC[citIndex].listCC.Count; j++)
                {
                    if (CommonClass.listDIC[citIndex].listCC[j].ChineseName == "轨距" || CommonClass.listDIC[citIndex].listCC[j].Name == "Gage")
                    {
                        data_ww1[4] = CommonClass.listDIC[citIndex].listCC[j].Data;
                        break;
                    }
                }
                #endregion

                #region 波形对齐
                try
                {
                    AutoTranslationClass autoTranslationCls = new AutoTranslationClass();
                    int reviseVal = autoTranslationCls.AutoTranslation(data_ww, data_ww1);

                    //if (LayerDataGridView1.SelectedRows.Count != 1)
                    //{
                    //    return;
                    //}
                    //CommonClass.SetLayerDataReviseValue(int.Parse(LayerDataGridView1.SelectedRows[0].Cells[0].Value.ToString()) - 1, reviseVal);

                    CommonClass.SetLayerDataReviseValue(citIndex, reviseVal);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                #endregion
            }

            //StringBuilder sb = new StringBuilder();
            //sb.Append("里程,公里,米数,左高低中波,右高低中波,左轨向中波,右轨向中波,轨距\n");
            //for (int i = 0; i < data_ww.GetLength(0);i++ )
            //{
            //    for (int j=0;j<data_ww.GetLength(1);j++)
            //    {
            //        sb.AppendFormat("{0},", data_ww[i,j]);
            //    }
            //    sb.Append("\n");
            //}
            //File.WriteAllText(@"D:\1.csv", sb.ToString(),Encoding.Default);

            //sb = new StringBuilder();
            //sb.Append("里程,公里,米数,左高低中波,右高低中波,左轨向中波,右轨向中波,轨距\n");
            //for (int i = 0; i < data_ww.GetLength(0); i++)
            //{
            //    for (int j = 0; j < data_ww.GetLength(1); j++)
            //    {
            //        sb.AppendFormat("{0},", data_ww1[i, j]);
            //    }
            //    sb.Append("\n");
            //}
            //File.WriteAllText(@"D:\2.csv", sb.ToString(), Encoding.Default);
        }
        #endregion

        private void SetLayerInfo(DataInfoClass dic)
        {
            /* 基本版的只能加载两次波形----20140304--严广学 */
            //其他版本的最多显示十个波形
            if (CommonClass.listDIC.Count >= 10)
            {
                return;
            }

            string status = "";
            if (dic.iAppMode == 0)
            {
                status = CommonClass.cdp.QueryDataInfoHead(dic.sFilePath);
            }
            else
            {
                status = CommonClass.NWGetFileHead(dic.sFilePath);
            }
            string[] sDataInfoHead = status.Split(new char[] { ',' });

            if (sDataInfoHead[0].Contains("0"))
            {
                // iDataType; sDataVersion; sTrackCode; sTrackName; iDir; sTrain; sDate; sTime; iRunDir;iKmInc; fkmFrom; fkmTo; iSmaleRate; iChannelNumber;

                //
                if (sDataInfoHead[2].StartsWith("3."))
                {
                    dic.bEncrypt = true;
                }
                dic.sDate = sDataInfoHead[7];
                dic.sTrackName = sDataInfoHead[4];
                dic.sTrackCode = sDataInfoHead[3];
                dic.sDir = sDataInfoHead[5];
                dic.sTime = sDataInfoHead[8];
                dic.sTrain = sDataInfoHead[6];
                dic.sKmInc = sDataInfoHead[10];
                dic.iSmaleRate = int.Parse(sDataInfoHead[13]);
                dic.iChannelNumber = int.Parse(sDataInfoHead[14]);
                dic.sRunDir = sDataInfoHead[9];
                dic.Name = dic.sTrackName + dic.sDir + " " + dic.sDate + " " + dic.sTime + " " + dic.sTrain
                    + " " + dic.sKmInc + " " + dic.sRunDir + " " + Path.GetFileNameWithoutExtension(dic.sFilePath);

                if (sDataInfoHead[1].Contains("轨检"))
                {
                    dic.iDataType = 1;
                }
                else if (sDataInfoHead[1].Contains("动力学"))
                {
                    dic.iDataType = 2;
                }
                else if (sDataInfoHead[1].Contains("弓网"))
                {
                    dic.iDataType = 3;
                    dic.bEncrypt = true;
                    //dic.byteEncrypt= CommonClass.wdp.GetEncryptByteArray(dic.sFilePath,dic.iChannelNumber);
                }

                if (dic.iSmaleRate < 0)
                {
                    dic.bSmaleRate = false;
                    //dic.iSmaleRate = Math.Abs(dic.iSmaleRate) / 100;
                    dic.iSmaleRate = 4;
                }

            }
            else
            {
                MessageBox.Show("获取文件失败!");
                return;
            }

            //获取线路编码
            try
            {
                //Share Exclusive
                using (OleDbConnection sqlconn = new OleDbConnection(CommonClass.sDBConnectionString))
                {

                    OleDbCommand sqlcom = new OleDbCommand("select 自定义线路编号 from 自定义线路代码表 where 自定义线路编号='" + dic.sTrackCode + "' and 线路名='" + dic.sTrackName + "'", sqlconn);
                    sqlconn.Open();
                    dic.sLineCode = (string)sqlcom.ExecuteScalar();
                    sqlconn.Close();
                }
            }
            catch
            {
                //MessageBox.Show(ex.Message);
            }


            //初始化通道配置
            if (dic.iAppMode == 0)
            {
                CommonClass.cdp.QueryDataChannelInfoHead(dic.sFilePath);
                dic.sChannelsName = new string[CommonClass.cdp.dciL.Count];
                for (int i = 0; i < CommonClass.cdp.dciL.Count; i++)
                {
                    dic.sChannelsName[i] = CommonClass.cdp.dciL[i].sNameEn;
                }
                if (dic.sConfigFilePath.Trim().Length < 3)
                {
                    dic.sConfigFilePath = CommonClass.cdp.CreateWaveXMLConfig(Path.GetFullPath(Application.ExecutablePath), 0);
                }
            }
            else
            {
                dic.sConfigFilePath = CommonClass.cdp.CreateWaveXMLConfig(Path.GetFullPath(Application.ExecutablePath), 0);
                string sReturn = CommonClass.NWGetFileChannels(dic.sFilePath);
                string[] sSplit = sReturn.Split(new char[] { ',' });
                if (sSplit[0].Contains("0"))
                {
                    dic.sChannelsName = new string[sSplit.Length - 1];
                    for (int i = 0; i < sSplit.Length - 1; i++)
                    {
                        dic.sChannelsName[i] = sSplit[i + 1];
                    }
                }
                else
                {
                    dic = null;
                    return;
                }

            }

            dic.listCC = CommonClass.LoadChannelsConfig(dic.sConfigFilePath, dic.sChannelsName);
            //MessageBox.Show(dic.listCC.Count.ToString());
            if (dic.listCC.Count == 0 || (dic.listCC.Count == 1 && dic.listCC[0].Visible == false))
            {
                dic.sConfigFilePath = CommonClass.cdp.CreateWaveXMLConfig(Path.GetFullPath(Application.ExecutablePath), 0);
                dic.listCC = CommonClass.LoadChannelsConfig(dic.sConfigFilePath, dic.sChannelsName);
                //dic = null;
                //MessageBox.Show("请打开系统配置对话框，删除配置文件设置中的第一条！");
                //return;
            }

            dic.listChannelsVisible = new List<int>();
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < dic.listCC.Count; i++)
            {

                dic.listCC[i].Rect = new Rectangle(MainGraphicsPictureBox1.ClientSize.Width - CommonClass.ChannelsAreaWidth,
                    CommonClass.CoordinatesHeight + 2 + dic.listCC[i].Location * (int)CommonClass.CoordinatesYGrid - CommonClass.ChannelsAreaHeight / 2,
                    CommonClass.ChannelsAreaWidth,
                    CommonClass.ChannelsAreaHeight);
                if (dic.listCC[i].Visible)
                {
                    dic.listChannelsVisible.Add(dic.listCC[i].Id);
                    sb.Append(dic.listCC[i].Id + ",");
                }
            }
            sb.Remove(sb.Length - 1, 1);

            dic.listMeter = new List<WaveMeter>(dic.iSmaleRate * CommonClass.XZoomIn);
            for (int i = 0; i < dic.iSmaleRate * CommonClass.XZoomIn; i++)
            {
                WaveMeter wm = new WaveMeter();
                dic.listMeter.Add(wm);
            }

            float[][] arrayPointF = new float[dic.listChannelsVisible.Count][];

            for (int i = 0; i < dic.listChannelsVisible.Count; i++)
            {
                arrayPointF[i] = new float[dic.iSmaleRate * CommonClass.XZoomIn];
            }

            dic.fScale = new float[dic.iChannelNumber];
            dic.fOffset = new float[dic.iChannelNumber];

            if (dic.iAppMode == 0)
            {
                CommonClass.wdp.GetChannelScale(dic.sFilePath, dic.iChannelNumber, ref dic.fScale, ref dic.fOffset);
            }
            else
            {
                CommonClass.NWGetChannelScale(dic.sFilePath, dic.iChannelNumber, ref dic.fScale, ref dic.fOffset);
            }
            //异常处理,针对波形数据存储不正确
            switch (dic.iDataType)
            {
                case 1:
                case 3:
                    dic.fScale[1] = 4;
                    break;
            }

            SystemInfoToolStripStatusLabel1.Text = "获取通道比例完成";

            long iSRI = -1;
            long iERI = -1;

            //处理索引和标注
            if (dic.iAppMode == 0)
            {
                if (File.Exists(dic.sAddFile))
                {
                    dic.listLIC = CommonClass.wdp.GetDataLabelInfo(dic.sAddFile);
                    dic.listIC = CommonClass.wdp.GetDataIndexInfo(dic.sAddFile);

                    //如果有需要，把invaliddata表新增一列：通道类型--20140106--ygx
                    CommonClass.wdp.InvalidDataOldToNew(dic.sAddFile);

                    dic.listIDC = CommonClass.wdp.InvalidDataList(dic.sAddFile);
                    if (dic.listIC.Count > 0 && dic.bIndex)
                    {
                        iSRI = dic.listIC[0].lStartPoint;
                        iERI = dic.listIC[dic.listIC.Count - 1].lEndPoint;

                    }
                }
            }
            else
            {
                //网络获取信息

            }

            if (dic.iAppMode == 0)
            {
                CommonClass.wdp.GetDataStartPositionEndPositionInfoIncludeIndex(ref dic.lStartPosition, ref dic.lEndPosition, dic.sFilePath,
                    dic.iChannelNumber, iSRI, iERI, dic.bIndex);
            }
            else
            {
                CommonClass.NWDataStartPositionEndPositionInfoIncludeIndex(ref dic.lStartPosition, ref dic.lEndPosition, dic.sFilePath, 0, 0, dic.iSmaleRate, dic.iChannelNumber, iSRI, iERI, dic.bIndex);
            }
            SystemInfoToolStripStatusLabel1.Text = "获取数据里程完成";

            if (dic.iAppMode == 0)
            {
                if (CommonClass.listDIC.Count > 0 && dic.bIndex)
                {   //CommonClass.listDIC.Count > 0时：第n次打开文件,n>=2
                    //且dic.bIndex == true时：有启用索引
                    CommonClass.wdp.GetDataInfo(dic.listChannelsVisible, ref arrayPointF, ref dic.listMeter, 0,
                    dic.sFilePath, CommonClass.listDIC[0].listMeter[0].GetMeter(1), CommonClass.XZoomIn, dic.iSmaleRate, dic.iChannelNumber,
                    dic.lStartPosition, dic.lEndPosition, dic.listIC, dic.bIndex, dic.sKmInc, dic.bEncrypt, dic.bReverse, true, dic.iDataType);
                }
                else
                {   //CommonClass.listDIC.Count == 0时：第一次打开文件
                    //dic.bIndex == false时：没有启用索引
                    CommonClass.wdp.GetDataInfo(dic.listChannelsVisible, ref arrayPointF, ref dic.listMeter, 0,
                    dic.sFilePath, MainHScrollBar1.Value, CommonClass.XZoomIn, dic.iSmaleRate, dic.iChannelNumber,
                    dic.lStartPosition, dic.lEndPosition, dic.listIC, dic.bIndex, dic.sKmInc, dic.bEncrypt, dic.bReverse, false, dic.iDataType);
                }

            }
            else
            {
                CommonClass.NWGetData(dic.iDataType, sb.ToString(), ref arrayPointF, ref dic.listMeter, 0,
                    dic.sFilePath, MainHScrollBar1.Value, CommonClass.XZoomIn, dic.iSmaleRate, dic.iChannelNumber,
                    dic.lStartPosition, dic.lEndPosition, dic.listIC, dic.bIndex, dic.sKmInc, dic.bEncrypt, dic.bReverse);
            }

            SystemInfoToolStripStatusLabel1.Text = "读取数据完成";

            for (int n = 0; n < dic.listChannelsVisible.Count; n++)
            {
                for (int m = 0; m < dic.listCC.Count; m++)
                {
                    if (dic.listCC[m].Id == dic.listChannelsVisible[n])
                    {
                        dic.listCC[m].Data = arrayPointF[n];
                        break;
                    }
                }
            }

            if (CommonClass.listDIC.Count > 0)
            {
                dic.bMeterInfoVisible = false;
            }
            else
            {
                dic.bMeterInfoVisible = true;
            }
            CommonClass.listDIC.Add(dic);
            //Application.DoEvents();
            if (CommonClass.listDIC.Count == 1)
            {
                while (GetDataInfoBackgroundWorker1.IsBusy)
                {
                    GetDataInfoBackgroundWorker1.CancelAsync();
                    Thread.Sleep(100);                    
                }
                GetDataInfoBackgroundWorker1.RunWorkerAsync(dic);
                if (MainGraphicsPictureBox1.Visible == false)
                { MainGraphicsPictureBox1.Visible = true; }
            }


            MainGraphicsPictureBox1.Invalidate();
            InitLayer();
        }

        

        private void InitLayer()
        {
            //初始化图层菜单项
            fLayerTranslationForm.ShowList();
            已加载的数据图层ToolStripDropDownButton.DropDownItems.Clear();
            关闭波形数据ToolStripMenuItem.DropDownItems.Clear();
            for (int i = 0; i < CommonClass.listDIC.Count; i++)
            {
                已加载的数据图层ToolStripDropDownButton.DropDownItems.Add(CommonClass.listDIC[i].Name);
                if (CommonClass.listDIC[i].bIndex)
                {
                    已加载的数据图层ToolStripDropDownButton.DropDownItems[i].ForeColor = Color.Black;
                    已加载的数据图层ToolStripDropDownButton.DropDownItems[i].Text = "(里程已修正) " + 已加载的数据图层ToolStripDropDownButton.DropDownItems[i].Text;
                }
                else
                {
                    已加载的数据图层ToolStripDropDownButton.DropDownItems[i].ForeColor = Color.Red;
                    已加载的数据图层ToolStripDropDownButton.DropDownItems[i].Text = "(里程未修正) " + 已加载的数据图层ToolStripDropDownButton.DropDownItems[i].Text;
                }
                ToolStripMenuItem tsmi = new ToolStripMenuItem();
                tsmi.Name = "CusMenuItem" + i.ToString();
                tsmi.Text = CommonClass.listDIC[i].Name;
                tsmi.Tag = i.ToString();
                tsmi.Click += new EventHandler(tsmi_Click);
                关闭波形数据ToolStripMenuItem.DropDownItems.Add(tsmi);
            }
        }

        #region 菜单响应事件__文件__关闭波形文件__具体波形文件的点击关闭事件
        /// <summary>
        /// 菜单响应事件__文件__关闭波形文件__具体波形文件的点击关闭事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void tsmi_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem t = (ToolStripMenuItem)sender;
            if (t.Tag.ToString().Equals("0"))
            {
                while (CommonClass.listDIC.Count > 0)
                {
                    CommonClass.listDIC.RemoveAt(CommonClass.listDIC.Count - 1);
                    图形化台账显示ToolStripMenuItem.Checked = false;
                    图形化台账显示ToolStripMenuItem_Click(sender, e);
                }
                if (CommonClass.listDBC.Count > 0)
                {
                    CommonClass.listDBC.RemoveAt(CommonClass.listDBC.Count - 1);
                }
                MainGraphicsPictureBox1.Invalidate();
                MainHScrollBar1.Value = 0;
                MainHScrollBar1.Enabled = false;
                MainGraphicsPictureBox1.Visible = false;
                fViewIndexForm.Visible = false;
                特征点修正PToolStripMenuItem.Checked = false;
                区域放大ToolStripMenuItem.Checked = false;
                自动滚动ToolStripMenuItem.Checked = false;

                同通道名测量ToolStripMenuItem.Checked = false;
                同基线测量ToolStripMenuItem.Checked = false;
                同一层测量ToolStripMenuItem.Checked = false;
                距离测量ToolStripMenuItem.Checked = false;
                MeterageNoToolStripMenuItem1.Checked = true;
            }
            else
            {
                CommonClass.listDIC.RemoveAt(int.Parse(t.Tag.ToString()));
                MainGraphicsPictureBox1.Invalidate();
            }
            InitLayer();
        }
        #endregion

        #region 设置波形下方拖动条的最大值
        /// <summary>
        /// 设置波形下方拖动条的最大值
        /// 拖动条的单位为：XZoomIn / 10
        /// 拖动条+1，波形移动XZoomIn / 10米
        /// </summary>
        private void SetHScrollBarMaximum()
        {
            try
            {
                int iLength = (int)((CommonClass.listDIC[0].lEndPosition -
                        CommonClass.listDIC[0].lStartPosition) / (CommonClass.listDIC[0].iChannelNumber * 2 * 4
                        ) / (CommonClass.XZoomIn / CommonClass.XZoomInDivisor));
                SetMaximumMFHScrollBar(iLength);
            }
            catch
            {

            }
        }
        #endregion

        private void AutoUpdateBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            long lPos = CommonClass.wdp.AnalyseDataLCGJAuto(0, 0, CommonClass.listDIC[0].sFilePath, CommonClass.XZoomIn, CommonClass.listDIC[0].iSmaleRate, CommonClass.listDIC[0].iChannelNumber);
            while (!AutoUpdateBackgroundWorker.CancellationPending)
            {
                for (int k = 0; k < CommonClass.listDIC.Count; k++)
                {
                    List<int> listChannelsVisible = new List<int>();
                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < CommonClass.listDIC[k].listCC.Count; i++)
                    {
                        if (CommonClass.listDIC[k].listCC[i].Visible)
                        {
                            listChannelsVisible.Add(CommonClass.listDIC[k].listCC[i].Id);
                            sb.Append(CommonClass.listDIC[k].listCC[i].Id.ToString() + ",");
                        }
                    }
                    sb.Remove(sb.Length - 1, 1);
                    float[][] arrayPointF = new float[listChannelsVisible.Count][];

                    for (int i = 0; i < listChannelsVisible.Count; i++)
                    {
                        arrayPointF[i] = new float[CommonClass.listDIC[k].iSmaleRate * CommonClass.XZoomIn];
                        //arrayPointF[i] = new PointF[800];
                    }
                    List<WaveMeter> listMeter = new List<WaveMeter>();
                    int iResult = 0;
                    iResult = CommonClass.wdp.GetAutoDataInfo(listChannelsVisible, ref arrayPointF, ref listMeter,
                    CommonClass.listDIC[k].sFilePath, ref lPos, CommonClass.XZoomIn,
                    CommonClass.listDIC[k].iSmaleRate, CommonClass.listDIC[k].iChannelNumber, CommonClass.listDIC[k].lStartPosition, CommonClass.listDIC[k].lEndPosition, CommonClass.listDIC[k].sKmInc, CommonClass.listDIC[k].bEncrypt, CommonClass.XZoomIn);

                    SystemInfoToolStripStatusLabel1.Text = "自动读取数据完成";

                    for (int n = 0; n < CommonClass.listDIC.Count; n++)
                    {
                        for (int i = 0; i < iResult; i++)
                        {
                            CommonClass.listDIC[k].listMeter.RemoveAt(0);
                            CommonClass.listDIC[k].listMeter.Add(listMeter[i]);
                        }
                    }
                    for (int n = 0; n < listChannelsVisible.Count; n++)
                    {
                        for (int m = 0; m < CommonClass.listDIC[k].listCC.Count; m++)
                        {
                            if (CommonClass.listDIC[k].listCC[m].Id == listChannelsVisible[n] && iResult > 0)
                            {
                                for (int iMi = iResult; iMi < CommonClass.listDIC[k].iSmaleRate * CommonClass.XZoomIn; iMi++)
                                {
                                    CommonClass.listDIC[k].listCC[m].Data[iMi - iResult] = CommonClass.listDIC[k].listCC[m].Data[iMi];
                                }
                                for (int iMj = 0; iMj < iResult; iMj++)
                                {
                                    CommonClass.listDIC[k].listCC[m].Data[iMj + (CommonClass.listDIC[k].iSmaleRate * CommonClass.XZoomIn) - iResult] =
                                        arrayPointF[n][iMj];
                                }
                                break;
                            }
                        }
                    }
                }
                MainGraphicsPictureBox1.Invalidate();
                SecondGraphicsPictureBox1.Invalidate();
                Thread.Sleep(30);
            }
        }

        private void StartRecordIndexToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (CommonClass.listDIC.Count == 0)
            {
                MessageBox.Show("请先打开波形文件");
                return;
            }
            if (!MeterageNoToolStripMenuItem1.Checked)
            {
                MessageBox.Show("请先关闭测量模式状态");
                return;
            }
            MainGraphicsPictureBox1.Cursor = Cursors.Cross;
        }

        private void EndRecordIndexToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            MainGraphicsPictureBox1.Cursor = Cursors.Default;
        }

        private void ClearRecordIndexToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("是否要清空已经记录的索引信息?", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2);
            if (dr == DialogResult.Yes)
            {
                CommonClass.listDIC[0].listIC.Clear();
            }
        }

        public void SetMFHScrollBar_Scroll(object sender, ScrollEventArgs sea)
        {
            MFHScrollBar_Scroll(sender, sea);
        }

        

        #region 判断是否已经有打开的波形文件
        /// <summary>
        /// 判断是否已经有打开的波形文件--true:已经打开至少一个波形文件；false：没有打开过波形文件
        /// </summary>
        /// <returns>true:已经打开至少一个波形文件；false：没有打开过波形文件</returns>
        private bool VInfo()
        {
            if (CommonClass.listDIC.Count < 1)
            {
                MessageBox.Show("请先打开一个波形文件");
                return false;
            }
            else
            {
                return true;
            }
        }
        #endregion

        #region 菜单响应事件__文件
        #region 菜单响应事件__文件__打开波形数据
        /// <summary>
        /// 菜单响应事件__文件__打开波形数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 打开波形数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Dog dog = new Dog();
            
            //DogStatus dogStatus = dog.Login(Vendor);
            //if (dogStatus == DogStatus.DogNotFound)
            //{
            //    MessageBox.Show("请插入超级狗");
            //    return;
            //}
            //if (dogStatus == DogStatus.KeyIdNotFound)
            //{
            //    MessageBox.Show("许可文件错误");
            //    return;
            //}
            
            using (OpenDataDialog odd = new OpenDataDialog())
            {

                DialogResult dr = odd.ShowDialog();
                if (dr == DialogResult.OK)
                {

                    //
                    List<OpenDataPackClass> listReturnData = (List<OpenDataPackClass>)odd.Tag;
                    //获取文件头信息
                    for (int iIndex = 0; iIndex < listReturnData.Count; iIndex++)
                    {
                        if (listReturnData[0].iType == 1)
                        {
                            DataInfoClass dic = new DataInfoClass();
                            dic.iAppMode = listReturnData[iIndex].iAppMode;
                            dic.sFilePath = listReturnData[iIndex].sFileName;
                            if (CommonClass.listDIC.Count > 0)
                            {
                                dic.sConfigFilePath = listReturnData[iIndex].sArrayConfigFile;
                                dic.bHis = false;
                                单通道拖动ToolStripMenuItem.Checked = true;
                                同名通道拖动ToolStripMenuItem.Checked = false;
                                同基线通道拖动ToolStripMenuItem.Checked = false;
                            }
                            else
                            {
                                dic.sConfigFilePath = listReturnData[iIndex].sArrayConfigFile;
                            }
                            dic.sAddFile = listReturnData[iIndex].sAddFileName;
                            dic.bIndex = listReturnData[iIndex].bIndex;
                            dic.iIndexID = listReturnData[iIndex].iIndexID;

                            SetLayerInfo(dic);


                        }
                        Application.DoEvents();
                    }
                }
            }
        }
        #endregion

        #region 菜单响应事件__文件__加载iic数据
        /// <summary>
        /// 菜单响应事件__文件__打开iic数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 加载IIC数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (VInfo())
            {
                fIICViewForm.Show();
            }
        }
        #endregion

        #region 菜单响应事件__文件__导出数据
        #region 菜单响应事件__文件__导出数据__截取波形片段
        /// <summary>
        /// 菜单响应事件__文件__导出数据__截取波形片段
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 截取波形片段ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (VInfo())
            {
                using (ExportDataForm edf = new ExportDataForm())
                {
                    edf.ShowDialog();
                }
            }
        }
        #endregion

        #region 菜单响应事件__文件__导出数据__导出当前屏数据
        /// <summary>
        /// 菜单响应事件__文件__导出数据__导出当前屏数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 导出当前屏数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (VInfo())
            {
                using (ExportWaveDataForm ewdf = new ExportWaveDataForm())
                {
                    ewdf.ShowDialog();
                }
            }
        }
        #endregion

        #region 菜单响应事件__文件__导出数据__精确数据导出
        /// <summary>
        /// 菜单响应事件__文件__导出数据__精确数据导出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 精确数据导出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (VInfo())
            {
                using (ExportIndexDataForm eidf = new ExportIndexDataForm())
                {
                    eidf.ShowDialog();
                }
            }
        }
        #endregion

        #region 菜单响应事件__文件__导出数据__波形数据分割
        /// <summary>
        /// 菜单响应事件__文件__导出数据__波形数据分割
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 波形数据分割ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (VInfo())
            {
                using (WaveSplitForm wsf = new WaveSplitForm())
                {
                    wsf.ShowDialog();
                }
            }
        }
        #endregion

        #region 菜单响应事件__文件__导出数据__批量精确数据导出
        /// <summary>
        /// 菜单响应事件__文件__导出数据__批量精确数据导出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 批量精确数据导出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (ExportBatchIndexDataForm ebidf = new ExportBatchIndexDataForm())
            {
                ebidf.ShowDialog();

            }
        }
        #endregion
        #endregion

        #region 菜单响应事件__文件__打印当前内容
        /// <summary>
        /// 菜单响应事件__文件__打印当前内容
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 打印PToolStripMenuItem_Click(object sender, EventArgs e)
        {

            try
            {
                CaptureScreen();

                this.FormBorderStyle = FormBorderStyle.None;
                this.MainHScrollBar1.Visible = false;
                this.MainMenuStrip1.Visible = false;
                this.MainStatusStrip1.Visible = false;
                this.MainForm_Resize(sender, e);
                if (pageSetupDialog1.ShowDialog() == DialogResult.OK)
                {
                    printPreviewDialog1.ShowDialog();
                }


                printDialog1.Document = printDocument1;

                if (printDialog1.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        printDocument1.Print();
                    }
                    catch (System.Exception ex)
                    {

                    }
                }

                //MainGraphicsPictureBox1.Image.Save(@"D:\1.jpg", ImageFormat.Jpeg);
                memoryImage.Save(@"J:\1.jpg", ImageFormat.Jpeg);
                MessageBox.Show("ok");


            }
            catch
            {

            }
            this.MainStatusStrip1.Visible = true;
            this.MainHScrollBar1.Visible = true;
            this.MainMenuStrip1.Visible = true;
            this.MainForm_Resize(sender, e);
            this.FormBorderStyle = FormBorderStyle.Sizable;

        }

        


        private Bitmap memoryImage;
        private void CaptureScreen() 
        {
            Graphics mygraphics = MainGraphicsPictureBox1.CreateGraphics();
            Size s = MainGraphicsPictureBox1.Size;
            memoryImage = new Bitmap(s.Width, s.Height);
            //memoryImage = new Bitmap(s.Width, s.Height, mygraphics);
            //Graphics memoryGraphics = Graphics.FromImage(memoryImage);
            //IntPtr dc1 = mygraphics.GetHdc();
            //IntPtr dc2 = memoryGraphics.GetHdc();
            //BitBlt(dc2, 0, 0, this.ClientRectangle.Width,
            //    this.ClientRectangle.Height, dc1, 0, 0, 13369376);
            //mygraphics.ReleaseHdc(dc1);
            //memoryGraphics.ReleaseHdc(dc2);

            Graphics memoryGraphics = Graphics.FromImage(memoryImage);
            memoryGraphics.CopyFromScreen(MainGraphicsPictureBox1.Location.X, MainMenuStrip1.Height+25, 0, 0, s);
            //memoryGraphics.PageUnit = GraphicsUnit.Document;

        }


        #endregion

        #region 菜单响应事件__文件__退出
        /// <summary>
        /// 菜单响应事件__文件__退出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 退出QToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion
        #endregion

        #region 菜单响应事件__视图
        #region 菜单响应事件__视图_显示范围选择
        #region 菜单响应事件__视图_显示范围选择__x轴原始大小(200米)
        /// <summary>
        /// 菜单响应事件__视图_显示范围选择__x轴原始大小(200米)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void x轴原始大小ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (XZoomIn200ToolStripMenuItem1.Checked == false)
                {
                    XZoomIn200ToolStripMenuItem1.Checked = true;
                    XZoomIn400ToolStripMenuItem1.Checked = false;
                    XZoomIn600ToolStripMenuItem1.Checked = false;
                    XZoomIn800ToolStripMenuItem1.Checked = false;
                    XZoomIn1000ToolStripMenuItem1.Checked = false;
                    XZoomIn2000ToolStripMenuItem1.Checked = false;
                    CommonClass.XZoomIn = 200;
                    exeFind();
                }
            }
            catch
            {

            }
        }
        #endregion

        #region 菜单响应事件__视图_显示范围选择__x轴2倍大小(400米)
        /// <summary>
        /// 菜单响应事件__视图_显示范围选择__x轴2倍大小(400米)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void x轴2倍大小toolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (XZoomIn400ToolStripMenuItem1.Checked == false)
                {
                    XZoomIn200ToolStripMenuItem1.Checked = false;
                    XZoomIn400ToolStripMenuItem1.Checked = true;
                    XZoomIn600ToolStripMenuItem1.Checked = false;
                    XZoomIn800ToolStripMenuItem1.Checked = false;
                    XZoomIn1000ToolStripMenuItem1.Checked = false;
                    XZoomIn2000ToolStripMenuItem1.Checked = false;

                    CommonClass.XZoomIn = 400;
                    exeFind();
                }
            }
            catch
            {

            }
        }
        #endregion

        #region 菜单响应事件__视图_显示范围选择__x轴3倍大小(600米)
        /// <summary>
        /// 菜单响应事件__视图_显示范围选择__x轴3倍大小(600米)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void x轴3倍大小ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (XZoomIn600ToolStripMenuItem1.Checked == false)
                {
                    XZoomIn200ToolStripMenuItem1.Checked = false;
                    XZoomIn400ToolStripMenuItem1.Checked = false;
                    XZoomIn600ToolStripMenuItem1.Checked = true;
                    XZoomIn800ToolStripMenuItem1.Checked = false;
                    XZoomIn1000ToolStripMenuItem1.Checked = false;
                    XZoomIn2000ToolStripMenuItem1.Checked = false;
                    CommonClass.XZoomIn = 600;
                    exeFind();
                }
            }
            catch
            {

            }
        }
        #endregion

        #region 菜单响应事件__视图_显示范围选择__x轴4倍大小(800米)
        /// <summary>
        /// 菜单响应事件__视图_显示范围选择__x轴4倍大小(800米)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void x轴4倍大小toolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (XZoomIn800ToolStripMenuItem1.Checked == false)
                {
                    XZoomIn200ToolStripMenuItem1.Checked = false;
                    XZoomIn400ToolStripMenuItem1.Checked = false;
                    XZoomIn600ToolStripMenuItem1.Checked = false;
                    XZoomIn800ToolStripMenuItem1.Checked = true;
                    XZoomIn1000ToolStripMenuItem1.Checked = false;
                    XZoomIn2000ToolStripMenuItem1.Checked = false;

                    CommonClass.XZoomIn = 800;
                    exeFind();
                }
            }
            catch
            {

            }
        }
        #endregion

        #region 菜单响应事件__视图_显示范围选择__x轴5倍大小(1公里)
        /// <summary>
        /// 菜单响应事件__视图_显示范围选择__x轴5倍大小(1公里)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void x轴5倍大小ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (XZoomIn1000ToolStripMenuItem1.Checked == false)
                {
                    XZoomIn200ToolStripMenuItem1.Checked = false;
                    XZoomIn400ToolStripMenuItem1.Checked = false;
                    XZoomIn600ToolStripMenuItem1.Checked = false;
                    XZoomIn800ToolStripMenuItem1.Checked = false;
                    XZoomIn1000ToolStripMenuItem1.Checked = true;
                    XZoomIn2000ToolStripMenuItem1.Checked = false;
                    CommonClass.XZoomIn = 1000;
                    exeFind();
                }
            }
            catch
            {
            }
        }
        #endregion

        #region 菜单响应事件__视图_显示范围选择__x轴10倍大小(2公里)
        /// <summary>
        /// 菜单响应事件__视图_显示范围选择__x轴10倍大小(2公里)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void x轴10倍大小ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (XZoomIn2000ToolStripMenuItem1.Checked == false)
                {
                    XZoomIn200ToolStripMenuItem1.Checked = false;
                    XZoomIn400ToolStripMenuItem1.Checked = false;
                    XZoomIn600ToolStripMenuItem1.Checked = false;
                    XZoomIn800ToolStripMenuItem1.Checked = false;
                    XZoomIn1000ToolStripMenuItem1.Checked = false;
                    XZoomIn2000ToolStripMenuItem1.Checked = true;
                    CommonClass.XZoomIn = 2000;
                    exeFind();
                }
            }
            catch
            {
            }
        }
        #endregion
        #endregion

        #region 菜单响应事件__视图_区域放大
        /// <summary>
        /// 菜单响应事件__视图_区域放大
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 区域放大ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (VInfo())
            {
                if (区域放大ToolStripMenuItem.Checked)
                {
                    区域放大ToolStripMenuItem.Checked = false;
                }
                else
                {
                    区域放大ToolStripMenuItem.Checked = true;
                }
            }
        }
        #endregion

        #region 菜单响应事件__视图_自动滚动
        /// <summary>
        /// 菜单响应事件__视图_自动滚动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 自动滚动ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!(CommonClass.listDIC.Count > 0))
            {
                MessageBox.Show("请先打开一个波形文件！");
                自动滚动ToolStripMenuItem.Checked = false;
                return;
            }
            if (自动滚动ToolStripMenuItem.Checked)
            {
                MainHScrollBar1.Enabled = false;
                AutoScrollbackgroundWorker1.RunWorkerAsync();

            }
            else
            {
                AutoScrollbackgroundWorker1.CancelAsync();
                MainHScrollBar1.Enabled = true;

            }

        }
        #endregion

        #endregion

        #region 菜单响应事件__功能
        #region 菜单响应事件__功能__查看里程特征点
        /// <summary>
        /// 菜单响应事件__功能__查看里程特征点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 查看里程特征点ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (VInfo())
            {
                fViewIndexForm.Show();
            }
        }
        #endregion

        #region 菜单响应事件__功能__查看分析标注_无效区段标注查看
        /// <summary>
        /// 菜单响应事件__功能__查看分析标注_无效区段标注查看
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 无效区段标注查看ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (VInfo())
            {
                fInvalidDataForm.Show();
            }
        }
        #endregion

        #region 菜单响应事件__功能__查看分析标注_普通标注查看
        /// <summary>
        /// 菜单响应事件__功能__查看分析标注_普通标注查看
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 普通标注查看ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (VInfo())
            {
                using (LabelInfoForm lif = new LabelInfoForm())
                {
                    lif.ShowDialog();
                }
            }
        }
        #endregion

        #region 菜单响应事件__功能__图层控制
        /// <summary>
        /// 菜单响应事件__功能__图层控制
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 图层控制ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (LayerForm lf = new LayerForm())
            {
                lf.ShowDialog();
            }
            ScrollEventArgs sea;
            sea = new ScrollEventArgs(ScrollEventType.EndScroll,
                    MainHScrollBar1.Value);
            SetMFHScrollBar_Scroll(sender, sea);
            //MainGraphicsPictureBox1.Invalidate();
        }
        #endregion

        #region 菜单响应事件__功能__层平移
        /// <summary>
        /// 菜单响应事件__功能__层平移
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 层平移ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (VInfo())
            {
                fLayerTranslationForm.Show();
            }
        }
        #endregion

        #region 菜单响应事件__功能__里程定位
        /// <summary>
        /// 菜单响应事件__功能__里程定位
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 里程定位ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //打开里程定位
            try
            {
                if (!VInfo())
                {
                    return;
                }
                using (FindForm ff = new FindForm())
                {
                    ff.ShowDialog();
                    ff.Hide();
                    Application.DoEvents();
                    //根据运行模式，获取里程坐标点---单位为km
                    float f = float.Parse(ff.Tag.ToString());
                    if (CommonClass.listDIC[0].bIndex)
                    {
                        long findPos = CommonClass.wdp.GetNewIndexMeterPositon(CommonClass.listDIC[0].listIC, (long)(f * 1000), CommonClass.listDIC[0].iChannelNumber, CommonClass.listDIC[0].sKmInc, 0);
                        MainForm.sMainform.MeterFind(findPos);
                        
                    } 
                    else
                    {
                        MeterFind(f);
                    }
                    
                }
            }
            catch
            {

            }
        }
        #endregion

        #region 菜单响应事件__功能__截图
        /// <summary>
        /// 菜单响应事件__功能__截图
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 截图ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Point p = this.PointToScreen(new Point(0, SystemInformation.MenuHeight));

            Image img = new Bitmap(this.Width - SystemInformation.Border3DSize.Width * 2,
                this.Height -
                SystemInformation.CaptionHeight - SystemInformation.MenuHeight);
            Graphics g = Graphics.FromImage(img);

            Size s = new System.Drawing.Size(this.Width - SystemInformation.Border3DSize.Width * 2,
                this.Height -
                SystemInformation.CaptionHeight - SystemInformation.MenuHeight);
            g.CopyFromScreen(p, new Point(0, 0), s);
            DialogResult dr = savePrintScreenFileDialog1.ShowDialog();
            if (dr == DialogResult.OK)
            { 
                img.Save(savePrintScreenFileDialog1.FileName, ImageFormat.Bmp);
                MessageBox.Show("保存成功！");
            }
            else
            {

            }
            g.Dispose();
            //MessageBox.Show("保存成功！");
        }
        #endregion

        #region 菜单响应事件__功能__iic修正
        /// <summary>
        /// 菜单响应事件__功能__iic修正
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void iIC修正ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //IIC修正数据
            if (CommonClass.listDIC.Count > 1)
            {
                MessageBox.Show("不要在打开多个波形文件的基础上执行此功能！");
                return;
            }
            else if (CommonClass.listDIC.Count == 0)
            {
                MessageBox.Show("请打开一个波形文件！");
                return;
            }
            //if (!CommonClass.listDIC[0].bIndex)
            //{
            //    MessageBox.Show("缺少idf索引文件！");
            //    return;
            //}

            //if (CommonClass.listDIC.Count == 1 && CommonClass.listDIC[0].bIndex)---新增了里程没有校正时，把tqi表转置，于是改为---ygx--20140113
            if (CommonClass.listDIC.Count == 1)
            {
                //处理
                using (IICDataForm iicdf = new IICDataForm())
                {
                    iicdf.ShowDialog();
                }
            }
        }
        #endregion
        #endregion

        #region 菜单响应事件__操作
        private void SetOperationMenuItemEnable(Boolean b)
        {
            特征点修正PToolStripMenuItem.Enabled = b;
        }
        #region 菜单响应事件__操作__设定特征点
        /// <summary>
        /// 菜单响应事件__操作__设定特征点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 特征点修正PToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (VInfo())
            {
                if (特征点修正PToolStripMenuItem.Checked)
                {
                    特征点修正PToolStripMenuItem.Checked = false;
                    MainGraphicsPictureBox1.Cursor = Cursors.Default;
                    MainForm.sMainform.wasIndex = false;
                    fViewIndexForm.Hide();
                    SetOperationMenuItemEnable(true);
                }
                else
                {
                    特征点修正PToolStripMenuItem.Checked = true;
                    MainGraphicsPictureBox1.Cursor = Cursors.Cross;
                    MainForm.sMainform.wasIndex = true;
                    fViewIndexForm.Show();

                    SetOperationMenuItemEnable(false);
                    特征点修正PToolStripMenuItem.Enabled = true;

                }
            }
        }
        #endregion
        #endregion

        #region 菜单响应事件___测量主菜单
        private void SetMeterageMenuEnable(Boolean b)
        {
            MeterageNoToolStripMenuItem1.Checked = b;
            同通道名测量ToolStripMenuItem.Checked = b;
            同基线测量ToolStripMenuItem.Checked = b;
            同一层测量ToolStripMenuItem.Checked = b;
            距离测量ToolStripMenuItem.Checked = b;
            断面测量ToolStripMenuItem.Checked = b;
        }
        #region 菜单响应事件___测量中的____无
        /// <summary>
        /// 菜单响应事件___测量中的____无
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MeterageNoToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            SetMeterageMenuEnable(false);
            MeterageNoToolStripMenuItem1.Checked = true;
            MainGraphicsPictureBox1.Cursor = Cursors.Default;
            MainGraphicsPictureBox1.Invalidate();
        }
        #endregion
        #region 菜单响应事件___同通道名测量
        /// <summary>
        /// 菜单响应事件___同通道名测量
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 同通道名测量ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetMeterageMenuEnable(false);
            同通道名测量ToolStripMenuItem.Checked = true;
            MainGraphicsPictureBox1.Cursor = Cursors.Cross;
            this.wasIndex = false;
        }
        #endregion
        #region 菜单响应事件___同基线测量
        /// <summary>
        /// 菜单响应事件___同基线测量
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 同基线测量ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetMeterageMenuEnable(false);
            同基线测量ToolStripMenuItem.Checked = true;
            MainGraphicsPictureBox1.Cursor = Cursors.Cross;
            this.wasIndex = false;
        }
        #endregion
        #region 菜单响应事件___同一层测量
        /// <summary>
        /// 菜单响应事件___同一层测量
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 同一层测量ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetMeterageMenuEnable(false);
            同一层测量ToolStripMenuItem.Checked = true;
            MainGraphicsPictureBox1.Cursor = Cursors.Cross;
            this.wasIndex = false;
        }
        #endregion
        #region 菜单响应事件___断面测量
        /// <summary>
        /// 菜单响应事件___断面测量
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 断面测量ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetMeterageMenuEnable(false);
            断面测量ToolStripMenuItem.Checked = true;
            MainGraphicsPictureBox1.Cursor = Cursors.Cross;
            this.wasIndex = false;
        }
        #endregion
        #region 菜单响应事件___距离测量
        /// <summary>
        /// 菜单响应事件___距离测量
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 距离测量ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetMeterageMenuEnable(false);
            距离测量ToolStripMenuItem.Checked = true;
            MainGraphicsPictureBox1.Cursor = Cursors.Cross;
            this.wasIndex = false;
        }
        #endregion
        #endregion

        #region 菜单响应事件__台账
        #region 菜单响应事件__台账__台账综合查询列表
        /// <summary>
        /// 菜单响应事件__台账__台账综合查询列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EquipmentInfoToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (VInfo())
            {
                fEquipmentInfoForm.Show();
            }
        }
        #endregion
        #region 菜单响应事件__台账__图形化台账显示
        /// <summary>
        /// 菜单响应事件__台账__图形化台账显示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 图形化台账显示ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (图形化台账显示ToolStripMenuItem.Checked)
            {
                //
                if (CommonClass.listDIC.Count > 0)
                {
                    bool b = LoadPWMISDatabase();
                    SecondGraphicsPictureBox1.Visible = true;
                    RightLabel1.Visible = true;
                    this.MainForm_Resize(sender, e);
                    SecondGraphicsPictureBox1.Invalidate();
                }
                MainGraphicsPictureBox1.Invalidate();
            }
            else
            {
                if (CommonClass.listDBC.Count > 0 && CommonClass.listDBC[0].Name.Contains("PWMIS台帐"))
                {
                    CommonClass.listDBC.RemoveAt(0);
                }
                SecondGraphicsPictureBox1.Visible = false;
                RightLabel1.Visible = false;
                this.MainForm_Resize(sender, e);
                MainGraphicsPictureBox1.Invalidate();

            }
        }
        #endregion
        #endregion

        #region 菜单响应事件__高级
        #region 菜单响应事件__高级__相关性修正
        /// <summary>
        /// 菜单响应事件__高级__相关性修正
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 相关性修正ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //IIC修正数据
            if (CommonClass.listDIC.Count > 1)
            {
                MessageBox.Show("不要在打开多个波形文件的基础上执行此功能");
                return;
            }
            else if (CommonClass.listDIC.Count == 0)
            {
                MessageBox.Show("请打开一个波形文件！");
                return;
            }
            if (!CommonClass.bRunMatlab)
            {
                MessageBox.Show("MATLAB 运行环境没有找到,您必须安装MATLAB 运行环境才能使用本软件所提供的MATLAB相关功能!");
                return;
            }
            //在主层文件的索引没有加载时，弹出提示框
            if (!CommonClass.listDIC[0].bIndex)
            {
                MessageBox.Show("请在打开文件时，加载索引！");
                return;
            }
            /*
             * 当里程较为精确时，则不需要进行里程校正，也就不需要做相关性修正 
             */
            if (CommonClass.listDIC.Count == 1 && CommonClass.listDIC[0].bIndex)
            {
                //处理
                using (CorrelationForm cf = new CorrelationForm())
                {
                    cf.ShowDialog();
                }
            }
        }
        #endregion
        #region 菜单响应事件__高级__无效数据滤除
        /// <summary>
        /// 菜单响应事件__高级__相关性修正
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 无效数据滤除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CommonClass.listDIC.Count > 1)
            {
                MessageBox.Show("当前打开的波形文件超过一个!");
                return;
            }
            else if (CommonClass.listDIC.Count == 0)
            {
                MessageBox.Show("请打开一个波形文件!");
                return;
            }
            if (!CommonClass.bRunMatlab)
            {
                MessageBox.Show("MATLAB 运行环境没有找到,您必须安装MATLAB 运行环境才能使用本软件所提供的MATLAB相关功能!");
                return;
            }
            if (CommonClass.listDIC.Count == 1)
            {
                InfoLabel2.Visible = true;
                pictureBox1.Visible = true;
                //后台处理
                InvalidDataProcessBackgroundWorker.RunWorkerAsync();

            }
        }
        #endregion
        #endregion

        #region 菜单响应事件__设置
        #region 菜单响应事件__设置__波形数据图层通道设置
        /// <summary>
        /// 菜单响应事件__设置__波形数据图层通道设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 数据通道CToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CallChannelsDialog(0, 0);
        }
        #endregion

        #region 菜单响应事件__设置__拖动方式设置---有三种
        #region 菜单响应事件__设置__拖动方式设置__单通道拖动
        /// <summary>
        /// 菜单响应事件__设置__拖动方式设置__单通道拖动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 单通道拖动ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            单通道拖动ToolStripMenuItem.Checked = true;
            同名通道拖动ToolStripMenuItem.Checked = false;
            同基线通道拖动ToolStripMenuItem.Checked = false;
        }
        #endregion
        #region 菜单响应事件__设置__拖动方式设置__同名通道拖动
        /// <summary>
        /// 菜单响应事件__设置__拖动方式设置__同名通道拖动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 同名通道拖动ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            单通道拖动ToolStripMenuItem.Checked = false;
            同名通道拖动ToolStripMenuItem.Checked = true;
            同基线通道拖动ToolStripMenuItem.Checked = false;
        }
        #endregion
        #region 菜单响应事件__设置__拖动方式设置__同基线通道拖动
        /// <summary>
        /// 菜单响应事件__设置__拖动方式设置__同基线通道拖动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 同基线通道拖动ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            单通道拖动ToolStripMenuItem.Checked = false;
            同名通道拖动ToolStripMenuItem.Checked = false;
            同基线通道拖动ToolStripMenuItem.Checked = true;
        }
        #endregion
        #endregion

        #region 菜单响应事件__设置__系统设置
        /// <summary>
        /// 菜单响应事件__设置__系统设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 服务器连接设置LToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (SystemConfigForm scf = new SystemConfigForm())
            {
                scf.ShowDialog();
            }
        }
        #endregion
        #endregion

        #region 菜单响应事件__帮助
        #region 菜单响应事件__帮助__授权信息
        /// <summary>
        /// 菜单响应事件__帮助__授权信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 授权信息ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (LicenseForm lf = new LicenseForm())
            {
                lf.ShowDialog();
            }
        }
        #endregion
        #region 菜单响应事件__帮助__关于
        /// <summary>
        /// 菜单响应事件__帮助__关于
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 关于AToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutForm af = new AboutForm();
            af.ShowDialog();
            af.Dispose();
        }
        #endregion
        #endregion

        private void MatlabVersionToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            string sPath = Environment.GetEnvironmentVariable("PATH").ToLower();
            //matlab compiler runtime
            Match m = Regex.Match(sPath, @"matlab compiler runtime\\\w+", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            if (m.Success)
            {
                MessageBox.Show("MATLAB 运行时已安装,版本:" +
                    m.Value.ToString().Substring(24, m.Value.ToString().Length - 24) +
                    "\n您可以使用本软件所提供的MATLAB相关功能!");
            }
            else
            {
                MessageBox.Show("MATLAB 运行环境没有找到,请先安装再使用本软件所提供的MATLAB相关功能!");
            }

        }        

        private void MainGraphicsPictureBox1_MouseLeave(object sender, EventArgs e)
        {
            this.MainGraphicsPictureBox1.Invalidate();
        }

        private void MainForm_Deactivate(object sender, EventArgs e)
        {
            wasMove = false;
            wasUnAreas = false;
        }


        

        private void GetDataInfoBackgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            if (!GetDataInfoBackgroundWorker1.CancellationPending)
            {
                DataInfoClass dic = (DataInfoClass)e.Argument;
                SetValueMFHScrollBar(0);
                SetHScrollBarMaximum();
                SetEnabledMFHScrollBar(true);
                Application.DoEvents();
                if (dic.iAppMode == 0)
                {
                    CommonClass.DataMileageInfo = CommonClass.wdp.GetDataMileageInfo(dic.sFilePath, dic.iChannelNumber, dic.iSmaleRate, dic.bEncrypt, dic.listIC, dic.bIndex, dic.lStartPosition, dic.lEndPosition, dic.sKmInc);
                }
                else
                {
                    CommonClass.DataMileageInfo = CommonClass.NWGetDataMileageInfo(dic.sFilePath, dic.iChannelNumber, dic.iSmaleRate, dic.bEncrypt);
                }
                GetDataInfoBackgroundWorker1.CancelAsync();
            }
        }

        private void SecondGraphicsPictureBox1_Paint(object sender, PaintEventArgs e)
        {
            if (CommonClass.listDBC.Count > 0)
            { DrawingDBData(sender, e); }
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (this.Visible)
            {
                this.Visible = false;
            }
            else
            {
                this.Visible = true;
                this.BringToFront();
            }
        }
 
        private void AutoScrollbackgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                while (!AutoScrollbackgroundWorker1.CancellationPending)
                {
                    MainHScrollBar1.Value++;
                    for (int k = 0; k < CommonClass.listDIC.Count; k++)
                    {
                        List<int> listChannelsVisible = new List<int>();
                        StringBuilder sb = new StringBuilder();
                        for (int i = 0; i < CommonClass.listDIC[k].listCC.Count; i++)
                        {
                            if (CommonClass.listDIC[k].listCC[i].Visible)
                            {
                                listChannelsVisible.Add(CommonClass.listDIC[k].listCC[i].Id);
                                sb.Append(CommonClass.listDIC[k].listCC[i].Id.ToString() + ",");
                            }
                        }
                        sb.Remove(sb.Length - 1, 1);
                        float[][] arrayPointF = new float[listChannelsVisible.Count][];

                        for (int i = 0; i < listChannelsVisible.Count; i++)
                        {
                            arrayPointF[i] = new float[CommonClass.listDIC[k].iSmaleRate * CommonClass.XZoomIn];
                            //arrayPointF[i] = new PointF[800];
                        }


                        //
                        if (CommonClass.listDIC[k].iAppMode == 0)
                        {
                            if (CommonClass.listDIC[k].bIndex && k != 0)
                            {
                                CommonClass.wdp.GetDataInfo(listChannelsVisible, ref arrayPointF, ref CommonClass.listDIC[k].listMeter, CommonClass.listDIC[k].lReviseValue,
                                CommonClass.listDIC[k].sFilePath, CommonClass.listDIC[0].listMeter[0].GetMeter(1), CommonClass.XZoomIn,
                                CommonClass.listDIC[k].iSmaleRate, CommonClass.listDIC[k].iChannelNumber, CommonClass.listDIC[k].lStartPosition, CommonClass.listDIC[k].lEndPosition, CommonClass.listDIC[k].listIC, CommonClass.listDIC[k].bIndex, CommonClass.listDIC[k].sKmInc, CommonClass.listDIC[k].bEncrypt, CommonClass.listDIC[k].bReverse, (CommonClass.listDIC[k].bIndex && k != 0) ? true : false, CommonClass.listDIC[k].iDataType);
                            }
                            else
                            {
                                CommonClass.wdp.GetDataStartPositionEndPositionInfoIncludeIndex(ref CommonClass.listDIC[k].lStartPosition,
                                    ref CommonClass.listDIC[k].lEndPosition, CommonClass.listDIC[k].sFilePath,
                                    CommonClass.listDIC[k].iChannelNumber, CommonClass.listDIC[k].lStartPosition, CommonClass.listDIC[k].lEndPosition, CommonClass.listDIC[k].bIndex);
                                CommonClass.wdp.GetDataInfo(listChannelsVisible, ref arrayPointF, ref CommonClass.listDIC[k].listMeter, CommonClass.listDIC[k].lReviseValue,
                                CommonClass.listDIC[k].sFilePath, MainHScrollBar1.Value, CommonClass.XZoomIn,
                                CommonClass.listDIC[k].iSmaleRate, CommonClass.listDIC[k].iChannelNumber, CommonClass.listDIC[k].lStartPosition, CommonClass.listDIC[k].lEndPosition, CommonClass.listDIC[k].listIC, CommonClass.listDIC[k].bIndex, CommonClass.listDIC[k].sKmInc, CommonClass.listDIC[k].bEncrypt, CommonClass.listDIC[k].bReverse, (CommonClass.listDIC[k].bIndex && k != 0) ? true : false, CommonClass.listDIC[k].iDataType);
                            }
                        }
                        else
                        {
                            CommonClass.NWDataStartPositionEndPositionInfoIncludeIndex(ref CommonClass.listDIC[k].lStartPosition, ref CommonClass.listDIC[k].lEndPosition, CommonClass.listDIC[k].sFilePath, 0, 0, CommonClass.listDIC[k].iSmaleRate, CommonClass.listDIC[k].iChannelNumber, CommonClass.listDIC[k].lStartPosition, CommonClass.listDIC[k].lEndPosition, CommonClass.listDIC[k].bIndex);
                            CommonClass.NWGetData(CommonClass.listDIC[k].iDataType, sb.ToString(), ref arrayPointF, ref CommonClass.listDIC[k].listMeter, CommonClass.listDIC[k].lReviseValue,
                        CommonClass.listDIC[k].sFilePath, MainHScrollBar1.Value, CommonClass.XZoomIn, CommonClass.listDIC[k].iSmaleRate, CommonClass.listDIC[k].iChannelNumber,
                        CommonClass.listDIC[k].lStartPosition, CommonClass.listDIC[k].lEndPosition, CommonClass.listDIC[k].listIC, CommonClass.listDIC[k].bIndex, CommonClass.listDIC[k].sKmInc, CommonClass.listDIC[k].bEncrypt, CommonClass.listDIC[k].bReverse);
                        }

                        SystemInfoToolStripStatusLabel1.Text = "读取数据完成";
                        for (int n = 0; n < listChannelsVisible.Count; n++)
                        {
                            for (int m = 0; m < CommonClass.listDIC[k].listCC.Count; m++)
                            {
                                if (CommonClass.listDIC[k].listCC[m].Id == listChannelsVisible[n])
                                {
                                    CommonClass.listDIC[k].listCC[m].Data = arrayPointF[n];
                                    break;
                                }
                            }
                        }
                    }
                    MainGraphicsPictureBox1.Invalidate();
                    SecondGraphicsPictureBox1.Invalidate();

                    SetHScrollBarMaximum();

                    //设置提示
                    SetTipOnMenu();

                    //设置台帐位置
                    if (fEquipmentInfoForm.Visible && fEquipmentInfoForm.toolStripComboBox1.SelectedIndex == 0)
                    {

                        if (CommonClass.listDIC[0].sKmInc.Equals("增"))
                        {
                            fEquipmentInfoForm.StartMeterToolStripTextBox1.Text = ((CommonClass.listDIC[0].listMeter[0].GetMeter(CommonClass.listDIC[0].fScale[1]) - 10000) / 1000.0).ToString();
                            fEquipmentInfoForm.EndMeterToolStripTextBox1.Text = ((CommonClass.listDIC[0].listMeter[0].GetMeter(CommonClass.listDIC[0].fScale[1]) + 10000) / 1000.0).ToString();
                        }
                        else
                        {
                            fEquipmentInfoForm.EndMeterToolStripTextBox1.Text = ((CommonClass.listDIC[0].listMeter[0].GetMeter(CommonClass.listDIC[0].fScale[1]) + 10000) / 1000.0).ToString();
                            fEquipmentInfoForm.StartMeterToolStripTextBox1.Text = ((CommonClass.listDIC[0].listMeter[0].GetMeter(CommonClass.listDIC[0].fScale[1]) - 10000) / 1000.0).ToString();
                        }
                        fEquipmentInfoForm.SetData();
                    }
                    //设置联动
                    Thread.Sleep(CommonClass.iAutoScroll);
                }
            }
            catch
            {

            }
        }

        private void 批处理过程ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                using (AcclrtnBatProcsForm abpf = new AcclrtnBatProcsForm())
                {
                    abpf.ShowDialog();
                    abpf.Dispose();

                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

        }

        private void 加速度分析ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (AcclrtnAnalysisForm aaf = new AcclrtnAnalysisForm())
            {
                aaf.ShowDialog();
            }

        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void 高级测量功能ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (VInfo())
            {
                using (SeniorMeterageForm smf = new SeniorMeterageForm())
                {
                    smf.ShowDialog();
                }
            }
        }

        private void 自定义超限ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CommonClass.listDIC.Count > 1)
            {
                MessageBox.Show("不要在打开多个波形文件的基础上执行此功能");
                return;
            }
            else if (CommonClass.listDIC.Count == 0)
            {
                MessageBox.Show("请打开一个波形文件！");
                return;
            }
            else
            {
                diyDefectForm = new DIYDefectForm();
                diyDefectForm.Owner = this;
                diyDefectForm.TopLevel = true;

                diyDefectForm.Show();
            }
        }

        private void InvalidDataProcessBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {  
            try
            {            //处理
                InvalidDataProcessing.DataProcessing IDP = new DataProcessing();
                //处理单通道数据
                //IDP.GetDataInfo(CommonClass.listDIC[0].sFilePath, CommonClass.listDIC[0].sAddFile);
                //处理多通道数据
                IDP.GetDataInfoMulti(CommonClass.listDIC[0].sFilePath, CommonClass.listDIC[0].sAddFile);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
                MessageBox.Show(ex.Source);
                MessageBox.Show(ex.StackTrace);

            }

        }

        private void InvalidDataProcessBackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            SetInfoLabelThis(false);
            MessageBox.Show("处理完成");

        }

        private void geo转citToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (WaveConvertForm wcf = new WaveConvertForm())
            {
                wcf.ShowDialog();
            }
        }

        private void 高频滤波ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (CitFilterForm cff = new CitFilterForm())
            {
                cff.ShowDialog();
            }
        }

        private void 变化智能识别ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                changeDetectionForm = new ChangeDetectionForm();
                changeDetectionForm.Owner = this;
                changeDetectionForm.TopLevel = true;

                changeDetectionForm.Show();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("微小变化识别 出错\n"+ex.Message+"\n"+ex.Source+"\n"+ex.StackTrace);
            }
        }

        private void 峰峰值指标ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //IIC修正数据
            if (CommonClass.listDIC.Count > 1)
            {
                MessageBox.Show("不要在打开多个波形文件的基础上执行此功能");
                return;
            }
            else if (CommonClass.listDIC.Count == 0)
            {
                MessageBox.Show("请打开一个波形文件！");
                return;
            }
            if (!CommonClass.bRunMatlab)
            {
                MessageBox.Show("MATLAB 运行环境没有找到,您必须安装MATLAB 运行环境才能使用本软件所提供的MATLAB相关功能!");
                return;
            }

            try
            {
                fengfengzhiForm = new FengFengzhiForm();
                fengfengzhiForm.Owner = this;
                fengfengzhiForm.TopLevel = true;

                fengfengzhiForm.Show();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("峰峰值指标  出错\n" + ex.Message + "\n" + ex.Source + "\n" + ex.StackTrace);
            }
        }

        private void 连续多波指标ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //IIC修正数据
            if (CommonClass.listDIC.Count > 1)
            {
                MessageBox.Show("不要在打开多个波形文件的基础上执行此功能");
                return;
            }
            else if (CommonClass.listDIC.Count == 0)
            {
                MessageBox.Show("请打开一个波形文件！");
                return;
            }
            if (!CommonClass.bRunMatlab)
            {
                MessageBox.Show("MATLAB 运行环境没有找到,您必须安装MATLAB 运行环境才能使用本软件所提供的MATLAB相关功能!");
                return;
            }

            try
            {
                multiwaveForm = new MultiWaveForm();
                multiwaveForm.Owner = this;
                multiwaveForm.TopLevel = true;

                multiwaveForm.Show();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("连续多波指标  出错\n" + ex.Message + "\n" + ex.Source + "\n" + ex.StackTrace);
            }
        }

        private void 功率谱ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //
            if (CommonClass.listDIC.Count > 1)
            {
                MessageBox.Show("不要在打开多个波形文件的基础上执行此功能");
                return;
            }
            else if (CommonClass.listDIC.Count == 0)
            {
                MessageBox.Show("请打开一个波形文件！");
                return;
            }
            if (!CommonClass.bRunMatlab)
            {
                MessageBox.Show("MATLAB 运行环境没有找到,您必须安装MATLAB 运行环境才能使用本软件所提供的MATLAB相关功能!");
                return;
            }

            try
            {
                gonglvpuForm = new GongLvPuForm();
                gonglvpuForm.Owner = this;
                gonglvpuForm.TopLevel = true;

                gonglvpuForm.Show();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("功率谱  出错\n" + ex.Message + "\n" + ex.Source + "\n" + ex.StackTrace);
            }
        }

        private void 有效值概率分布计算ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                using (AcclrtnRmsProcsForm arpf = new AcclrtnRmsProcsForm())
                {
                    arpf.ShowDialog();
                    arpf.Dispose();
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }

        private void 波磨ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                using (AccIrtnCorrugation ac = new AccIrtnCorrugation())
                {
                    ac.ShowDialog();
                    ac.Dispose();
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }

        private void 提取焊接接头ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                using (AccIrtnWeld ac = new AccIrtnWeld())
                {
                    ac.ShowDialog();
                    ac.Dispose();
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }


        private void 智能里程修正ToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            try
            {
                using (AccIrtnMileCorrect ac = new AccIrtnMileCorrect())
                {
                    ac.ShowDialog();
                    ac.Dispose();
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }

    }
}

