using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Data.OleDb;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Xml;
using System.IO.Compression;
using DataProcess;
using System.Text;
using System.Data.OracleClient;


#pragma warning disable 0618
namespace ControlsCenter
{
    public class ControlsForm : System.Windows.Forms.Form
    {
        private System.Windows.Forms.GroupBox grpSocket;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbSocketPort;
        private System.Windows.Forms.TextBox tbSocketClientsNum;
        private System.Windows.Forms.RichTextBox rtbSocketMsg;
        private System.Windows.Forms.Button btnSocketStart;

        //clients数组保存当前在线用户的Client对象
        internal static Hashtable clients = new Hashtable();

        //该服务器默认的监听的端口号
        private TcpListener listener;

        //服务器可以支持的最多的客户端的连接数
        static int MAX_NUM = 100;
        //default dat port serverip
        private string IPAddress1 = string.Empty;
        internal static string XMLVersion = string.Empty;
        internal static string LocalDataAddress = string.Empty;
        internal static string LocalBKDataAddress = string.Empty;
        internal static string LocalTMPDataAddress = string.Empty;
        internal static int sRunMode = 0; //0 地面，1 车载，2 其他
        internal static string ExportDataAddress = string.Empty;
        internal static string DBConnectString = string.Empty;
        internal static string DBMobileConnectString = string.Empty;
        internal static string DBOther = string.Empty;
        private Label label2;


        //开始服务的标志
        internal static bool SocketServiceFlag = false;

        public ControlsForm()
        {
            //
            // Windows 窗体设计器支持所必需的
            //
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            //
            // TODO: 在 InitializeComponent 调用后添加任何构造函数代码
            //
        }

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            try
            {
                base.Dispose(disposing);
            }
            catch
            {

            }
        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ControlsForm));
            this.grpSocket = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.rtbSocketMsg = new System.Windows.Forms.RichTextBox();
            this.btnSocketStart = new System.Windows.Forms.Button();
            this.tbSocketPort = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tbSocketClientsNum = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.grpSocket.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpSocket
            // 
            this.grpSocket.Controls.Add(this.label2);
            this.grpSocket.Controls.Add(this.rtbSocketMsg);
            this.grpSocket.Controls.Add(this.btnSocketStart);
            this.grpSocket.Controls.Add(this.tbSocketPort);
            this.grpSocket.Controls.Add(this.label1);
            this.grpSocket.Controls.Add(this.tbSocketClientsNum);
            this.grpSocket.Controls.Add(this.label3);
            this.grpSocket.Location = new System.Drawing.Point(12, 12);
            this.grpSocket.Name = "grpSocket";
            this.grpSocket.Size = new System.Drawing.Size(462, 233);
            this.grpSocket.TabIndex = 13;
            this.grpSocket.TabStop = false;
            this.grpSocket.Text = "监听请求";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(19, 39);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 14);
            this.label2.TabIndex = 20;
            this.label2.Text = "状态信息";
            // 
            // rtbSocketMsg
            // 
            this.rtbSocketMsg.Location = new System.Drawing.Point(21, 56);
            this.rtbSocketMsg.Name = "rtbSocketMsg";
            this.rtbSocketMsg.Size = new System.Drawing.Size(422, 164);
            this.rtbSocketMsg.TabIndex = 19;
            this.rtbSocketMsg.Text = "";
            // 
            // btnSocketStart
            // 
            this.btnSocketStart.Location = new System.Drawing.Point(356, 15);
            this.btnSocketStart.Name = "btnSocketStart";
            this.btnSocketStart.Size = new System.Drawing.Size(87, 26);
            this.btnSocketStart.TabIndex = 18;
            this.btnSocketStart.Text = "启动服务器";
            this.btnSocketStart.Click += new System.EventHandler(this.btnSocketStart_Click);
            // 
            // tbSocketPort
            // 
            this.tbSocketPort.Location = new System.Drawing.Point(303, 17);
            this.tbSocketPort.MaxLength = 5;
            this.tbSocketPort.Name = "tbSocketPort";
            this.tbSocketPort.Size = new System.Drawing.Size(47, 21);
            this.tbSocketPort.TabIndex = 17;
            this.tbSocketPort.Text = "1235";
            this.tbSocketPort.TextChanged += new System.EventHandler(this.tbSocketPort_TextChanged);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(250, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 23);
            this.label1.TabIndex = 16;
            this.label1.Text = "端口号：";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tbSocketClientsNum
            // 
            this.tbSocketClientsNum.Location = new System.Drawing.Point(126, 17);
            this.tbSocketClientsNum.Name = "tbSocketClientsNum";
            this.tbSocketClientsNum.ReadOnly = true;
            this.tbSocketClientsNum.Size = new System.Drawing.Size(120, 21);
            this.tbSocketClientsNum.TabIndex = 15;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(19, 22);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(117, 17);
            this.label3.TabIndex = 13;
            this.label3.Text = "当前客户端连接数：";
            // 
            // ControlsForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.ClientSize = new System.Drawing.Size(489, 253);
            this.Controls.Add(this.grpSocket);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "ControlsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "波形数据监控中心";
            this.TopMost = true;
            this.Closing += new System.ComponentModel.CancelEventHandler(this.ClientSeverForm_Closing);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ControlsForm_FormClosing);
            this.Load += new System.EventHandler(this.ControlsForm_Load);
            this.grpSocket.ResumeLayout(false);
            this.grpSocket.PerformLayout();
            this.ResumeLayout(false);

        }
        #endregion

        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.Run(new ControlsForm());
        }

        private int getValidPort(string port)
        {
            int lport;

            //测试端口号是否有效
            try
            {
                //是否为空
                if (port == "")
                {
                    throw new ArgumentException(
                        "端口号为空，不能启动服务器");
                }
                lport = System.Convert.ToInt32(port);
            }
            catch (Exception e)
            {
                //ArgumentException, 
                //FormatException, 
                //OverflowException
                Console.WriteLine("无效的端口号：" + e.ToString());
                this.rtbSocketMsg.AppendText("无效的端口号：" + e.ToString() + "\n");
                return -1;
            }
            return lport;
        }

        //当单击“Socket启动”按钮时，便开始监听指定的Socket端口
        private void btnSocketStart_Click(object sender, System.EventArgs e)
        {
            if (btnSocketStart.Text.Equals("启动服务器"))
            {
                int port = getValidPort(tbSocketPort.Text);
                if (port < 0)
                {
                    return;
                }

                try
                {
                    IPAddress ipAdd;
                    IPAddress.TryParse(IPAddress1, out ipAdd);
                    //创建服务器套接字
                    listener = new TcpListener(ipAdd, port);
                    //开始监听服务器端口
                    listener.Start();
                    this.rtbSocketMsg.AppendText("服务器已经启动，正在监听" +
                        IPAddress1 + " 端口号：" + this.tbSocketPort.Text + "\n");

                    //启动一个新的线程，执行方法this.StartSocketListen，
                    //以便在一个独立的进程中执行确认与客户端Socket连接的操作
                    ControlsForm.SocketServiceFlag = true;
                    Thread thread = new Thread(new ThreadStart(this.StartSocketListen));
                    thread.Start();

                }
                catch (Exception ex)
                {
                    this.rtbSocketMsg.AppendText(ex.Message.ToString() + "\n");
                }
                btnSocketStart.Text = "停止服务器";
            }
            else
            {
                listener.Stop();
                ControlsForm.SocketServiceFlag = false;
                this.btnSocketStart.Enabled = true;
                clients.Clear();
                this.rtbSocketMsg.AppendText("监听已关闭！\n");
                btnSocketStart.Text = "启动服务器";
            }

        }

        //在新的线程中的操作，它主要用于当接收到一个客户端请求时，确认与客户端的连接，
        //并且立刻启动一个新的线程来处理和该客户端的信息交互。
        private void StartSocketListen()
        {
            while (ControlsForm.SocketServiceFlag)
            {
                try
                {
                    //当接收到一个客户端请求时，确认与客户端的连接
                    if (listener.Pending())
                    {
                        Socket socket = listener.AcceptSocket();
                        uint dummy = 0;
                        byte[] inOptionValues = new byte[Marshal.SizeOf(dummy) * 3];
                        BitConverter.GetBytes((uint)1).CopyTo(inOptionValues, 0);//是否启用Keep-Alive
                        BitConverter.GetBytes((uint)5000).CopyTo(inOptionValues, Marshal.SizeOf(dummy));//多长时间开始第一次探测
                        BitConverter.GetBytes((uint)5000).CopyTo(inOptionValues, Marshal.SizeOf(dummy) * 2);//探测时间间隔
                        socket.IOControl(IOControlCode.KeepAliveValues, inOptionValues, null);
                        if (clients.Count >= MAX_NUM)
                        {
                            this.rtbSocketMsg.AppendText("已经达到了最大连接数：" +
                                MAX_NUM + "，拒绝新的连接\n");
                            socket.Close();
                        }
                        else
                        {
                            //启动一个新的线程，
                            //执行方法this.ServiceClient，处理用户相应的请求
                            Client client = new Client(this, socket);
                            client.CurrentSocket.SendBufferSize = 16384;
                            Thread clientService = new Thread(
                                new ThreadStart(client.ServiceClient));
                            clientService.IsBackground = true;
                            clientService.Start();
                        }
                    }
                    Thread.Sleep(200);
                }
                catch (Exception ex)
                {
                    this.rtbSocketMsg.AppendText(ex.Message.ToString() + "\n");
                }

                //更新状态
                //try
                //{
                //    using (OleDbConnection connection = new OleDbConnection(ControlsForm.DBMobileConnectString))
                //    {
                //        OleDbCommand command = new OleDbCommand("update tbl09_02ThreadState set CurrentState=0,ReportTime='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "',CurrentStateDesc='STATUS|RUN' where ThreadID=303");
                //        command.Connection = connection;
                //        connection.Open();
                //        command.ExecuteNonQuery();
                //        connection.Close();
                //    }

                //    Thread.Sleep(200);
                //}
                //catch
                //{

                //}
            }
        }

        private void tbSocketPort_TextChanged(object sender, System.EventArgs e)
        {
            this.btnSocketStart.Enabled = (this.tbSocketPort.Text != "");
        }


        private void btnSocketStop_Click(object sender, System.EventArgs e)
        {
            listener.Stop();
            ControlsForm.SocketServiceFlag = false;
            this.btnSocketStart.Enabled = true;
            clients.Clear();
            this.rtbSocketMsg.AppendText("监听已关闭！\n");
        }


        public void updateUI(string msg)
        {
            try
            {
                this.rtbSocketMsg.AppendText(msg + "\n");
                if (this.rtbSocketMsg.TextLength > (2 * 65536))
                {
                    this.rtbSocketMsg.Text = "";
                }
            }
            catch
            {

            }
        }

        public void updateUINoLine(string msg)
        {
            try
            {
                this.rtbSocketMsg.AppendText(msg + " ");
            }
            catch
            {

            }
        }
        public string GetDataFileAddress()
        {
            return LocalDataAddress;
        }

        private void ClientSeverForm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ControlsForm.SocketServiceFlag = false;
        }


        private void ControlsForm_Load(object sender, EventArgs e)
        {
            try
            {
                string sFilePath = Application.StartupPath;
                if (!sFilePath.EndsWith("\\"))
                {
                    sFilePath += "\\";
                }
                sFilePath += "config.xml";
                //读取xml配置文件
                XmlDocument xd = new XmlDocument();
                xd.Load(sFilePath);
                IPAddress1 = xd.DocumentElement["Config"].Attributes["server"].Value;
                tbSocketPort.Text = xd.DocumentElement["Config"].Attributes["port"].Value;
                DBConnectString = xd.DocumentElement["DBCenter"].Attributes["connectionstring"].Value;
                DBMobileConnectString = xd.DocumentElement["DBMobile"].Attributes["connectionstring"].Value;
                LocalDataAddress = xd.DocumentElement["DBDataPath"].InnerText;
                LocalBKDataAddress = xd.DocumentElement["DBBKDataPath"].InnerText;
                LocalTMPDataAddress = xd.DocumentElement["DBTMPDataPath"].InnerText;
                sRunMode = int.Parse(xd.DocumentElement["Config"].Attributes["mode"].Value);

                if (sRunMode == 1)
                { //插入状态
                    try
                    {
                        int iCount = 0;
                        using (OleDbConnection connection = new OleDbConnection(ControlsForm.DBMobileConnectString))
                        {
                            OleDbCommand command = new OleDbCommand("select count(*) from tbl09_02ThreadState where ThreadID=303");
                            command.Connection = connection;
                            connection.Open();
                            iCount = (int)command.ExecuteScalar();
                            connection.Close();
                        }

                        if (iCount == 0)
                        {
                            using (OleDbConnection connection = new OleDbConnection(ControlsForm.DBMobileConnectString))
                            {
                                OleDbCommand command = new OleDbCommand("insert into tbl09_02ThreadState values(?,?,?,?,?)");
                                command.Connection = connection;
                                connection.Open();
                                command.Parameters.Add("p1", OleDbType.SmallInt);
                                command.Parameters.Add("p2", OleDbType.DBTimeStamp);
                                command.Parameters.Add("p3", OleDbType.SmallInt);
                                command.Parameters.Add("p4", OleDbType.LongVarChar);
                                command.Parameters.Add("p5", OleDbType.LongVarChar);

                                command.Parameters[0].Value = 303;
                                command.Parameters[1].Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                command.Parameters[2].Value = 0;
                                command.Parameters[3].Value = "STATUS|RUN";
                                command.Parameters[4].Value = "info";

                                command.ExecuteNonQuery();
                                connection.Close();
                            }
                        }

                        Thread.Sleep(200);
                    }
                    catch
                    {
                    }
                }
                this.btnSocketStart_Click(sender, e);

            }
            catch
            {

            }
        }

        private void ControlsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (sRunMode == 1)
                {
                    //更新状态
                    try
                    {
                        using (OleDbConnection connection = new OleDbConnection(ControlsForm.DBMobileConnectString))
                        {
                            OleDbCommand command = new OleDbCommand("update tbl09_02ThreadState set CurrentState=1,ReportTime='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "',CurrentStateDesc='STATUS|OK' where ThreadID=303");
                            command.Connection = connection;
                            connection.Open();
                            command.ExecuteNonQuery();
                            connection.Close();
                        }

                        Thread.Sleep(200);
                    }
                    catch
                    {

                    }
                }
            }
            catch
            {

            }
            //关掉所有线程
            try
            {

                listener.Stop();
            }
            catch
            {

            }
        }

    }

    public class Client
    {
        private string name = string.Empty;
        private Socket currentSocket = null;
        private string ipAddress = string.Empty;
        private string sLocalDataAddress = string.Empty;

        private ControlsForm server;

        //保留当前连接的状态：
        //closed --> connected --> closed
        private string state = "closed";

        public Client(ControlsForm server, Socket clientSocket)
        {
            this.server = server;

            this.currentSocket = clientSocket;
            ipAddress = getRemoteIPAddress();
            sLocalDataAddress = server.GetDataFileAddress();
        }

        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }
        public Socket CurrentSocket
        {
            get
            {
                return currentSocket;
            }
            set
            {
                currentSocket = value;
            }
        }

        public string IpAddress
        {
            get
            {
                return ipAddress;
            }
        }
        private string GetConn(int iDT)
        {
            if (iDT == 0)
            {
                return System.Configuration.ConfigurationManager.ConnectionStrings["OraConnStringDJ"].ConnectionString;
            }
            else
            {
                return System.Configuration.ConfigurationManager.ConnectionStrings["OraConnStringGJ"].ConnectionString;
            }
        }
        private string GetBackupPath(int iDT)
        {
            string sFilePath = "";
            try
            {
                using (OracleConnection connection = new OracleConnection(GetConn(iDT)))
                {
                    try
                    {

                        OracleCommand command = new OracleCommand("select t.* from tbl00_system_config t where configname='BackupPath'");
                        command.Connection = connection;
                        connection.Open();
                        OracleDataReader oddr = command.ExecuteReader();

                        if (oddr.Read())
                        {
                            sFilePath = oddr[1].ToString();
                        }


                        oddr.Close();
                    }
                    catch (Exception ex)
                    {
                        //MessageBox.Show(ex.Message);
                    }
                    connection.Close();
                }



            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
            return sFilePath;
        }
        private string GetManagePath(int iDT)
        {
            string sFilePath = "";
            try
            {
                using (OracleConnection connection = new OracleConnection(GetConn(iDT)))
                {
                    try
                    {

                        OracleCommand command = new OracleCommand("select t.* from tbl00_system_config t where configname='ServerPath'");
                        command.Connection = connection;
                        connection.Open();
                        OracleDataReader oddr = command.ExecuteReader();

                        if (oddr.Read())
                        {
                            sFilePath = oddr[1].ToString();
                        }


                        oddr.Close();
                    }
                    catch (Exception ex)
                    {
                        //MessageBox.Show(ex.Message);
                    }
                    connection.Close();
                }



            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
            return sFilePath;
        }
        private void GEOBackup(string sPlanID, string sTaskID, string sRecordID, string sStrFiles, string sPath, string sBackupPath, int iDT)
        {
            //备份
            File.Copy(sStrFiles, sBackupPath + sPath + Path.GetFileName(sStrFiles), true);

            Application.DoEvents();
            using (OracleConnection connection = new OracleConnection(GetConn(iDT)))
            {
                try
                {
                    OracleCommand command = new OracleCommand("insert into tbl00_detect_data_backup values(SEQ_TBL00_DETECT_1.nextval,:RECORDID,:TASKID,:PLANID,:DATATYPE,:DATANAME,:DATAPATH,:BACKUPDATE,:BACKUPPERSON,:BACKUPSTATUS,:DES,:unitcode)");
                    command.Connection = connection;

                    connection.Open();
                    command.Parameters.AddWithValue("RECORDID", OleDbType.Integer);
                    command.Parameters.AddWithValue("TASKID", OleDbType.Integer);
                    command.Parameters.AddWithValue("PLANID", OleDbType.Integer);
                    command.Parameters.AddWithValue("DATATYPE", OleDbType.Integer);
                    command.Parameters.AddWithValue("DATANAME", OleDbType.LongVarChar);
                    command.Parameters.AddWithValue("DATAPATH", OleDbType.LongVarChar);
                    command.Parameters.AddWithValue("BACKUPDATE", OleDbType.Date);
                    command.Parameters.AddWithValue("BACKUPPERSON", OleDbType.LongVarChar);
                    command.Parameters.AddWithValue("BACKUPSTATUS", OleDbType.Integer);
                    command.Parameters.AddWithValue("DES", OleDbType.LongVarChar);
                    command.Parameters.AddWithValue("unitcode", OleDbType.LongVarChar);

                    command.Parameters[0].Value = sRecordID;
                    command.Parameters[1].Value = sTaskID;
                    command.Parameters[2].Value = sPlanID;
                    command.Parameters[3].Value = 2;
                    command.Parameters[4].Value = Path.GetFileName(sStrFiles);
                    command.Parameters[5].Value = sPath;
                    command.Parameters[6].Value = DateTime.Now;
                    command.Parameters[7].Value = "admin";
                    command.Parameters[8].Value = -1;
                    command.Parameters[9].Value = "";
                    command.Parameters[10].Value = "";

                    command.ExecuteNonQuery();

                }
                catch (Exception ex)
                {
                    //MessageBox.Show(ex.Message);
                }
                connection.Close();
            }
        }

        private void CITBackup(string sPlanID, string sTaskID, string sRecordID, string sStrFiles, string sPath, string sBackupPath, int iDT)
        {
            File.Copy(sStrFiles, sBackupPath + sPath + Path.GetFileName(sStrFiles), true);
            Application.DoEvents();
            using (OracleConnection connection = new OracleConnection(GetConn(iDT)))
            {
                try
                {
                    OracleCommand command = new OracleCommand("insert into tbl00_detect_data_backup values(SEQ_TBL00_DETECT_1.nextval,:RECORDID,:TASKID,:PLANID,:DATATYPE,:DATANAME,:DATAPATH,:BACKUPDATE,:BACKUPPERSON,:BACKUPSTATUS,:DES,:unitcode)");
                    command.Connection = connection;

                    connection.Open();
                    command.Parameters.AddWithValue("RECORDID", OleDbType.Integer);
                    command.Parameters.AddWithValue("TASKID", OleDbType.Integer);
                    command.Parameters.AddWithValue("PLANID", OleDbType.Integer);
                    command.Parameters.AddWithValue("DATATYPE", OleDbType.Integer);
                    command.Parameters.AddWithValue("DATANAME", OleDbType.LongVarChar);
                    command.Parameters.AddWithValue("DATAPATH", OleDbType.LongVarChar);
                    command.Parameters.AddWithValue("BACKUPDATE", OleDbType.Date);
                    command.Parameters.AddWithValue("BACKUPPERSON", OleDbType.LongVarChar);
                    command.Parameters.AddWithValue("BACKUPSTATUS", OleDbType.Integer);
                    command.Parameters.AddWithValue("DES", OleDbType.LongVarChar);
                    command.Parameters.AddWithValue("unitcode", OleDbType.LongVarChar);

                    command.Parameters[0].Value = sRecordID;
                    command.Parameters[1].Value = sTaskID;
                    command.Parameters[2].Value = sPlanID;
                    command.Parameters[3].Value = 0;
                    command.Parameters[4].Value = Path.GetFileName(sStrFiles);
                    command.Parameters[5].Value = sPath;
                    command.Parameters[6].Value = DateTime.Now;
                    command.Parameters[7].Value = "admin";
                    command.Parameters[8].Value = -1;
                    command.Parameters[9].Value = "";
                    command.Parameters[10].Value = "";

                    command.ExecuteNonQuery();

                }
                catch (Exception ex)
                {
                    //MessageBox.Show(ex.Message);
                }
                connection.Close();
            }
        }

        private void IICBackup(string sPlanID, string sTaskID, string sRecordID, string sStrFiles, string sPath, string sBackupPath, int iDT)
        {
            File.Copy(sStrFiles, sBackupPath + sPath + Path.GetFileName(sStrFiles), true);
            Application.DoEvents();
            using (OracleConnection connection = new OracleConnection(GetConn(iDT)))
            {
                try
                {
                    OracleCommand command = new OracleCommand("insert into tbl00_detect_data_backup values(SEQ_TBL00_DETECT_1.nextval,:RECORDID,:TASKID,:PLANID,:DATATYPE,:DATANAME,:DATAPATH,:BACKUPDATE,:BACKUPPERSON,:BACKUPSTATUS,:DES,:unitcode)");
                    command.Connection = connection;

                    connection.Open();
                    command.Parameters.AddWithValue("RECORDID", OleDbType.Integer);
                    command.Parameters.AddWithValue("TASKID", OleDbType.Integer);
                    command.Parameters.AddWithValue("PLANID", OleDbType.Integer);
                    command.Parameters.AddWithValue("DATATYPE", OleDbType.Integer);
                    command.Parameters.AddWithValue("DATANAME", OleDbType.LongVarChar);
                    command.Parameters.AddWithValue("DATAPATH", OleDbType.LongVarChar);
                    command.Parameters.AddWithValue("BACKUPDATE", OleDbType.Date);
                    command.Parameters.AddWithValue("BACKUPPERSON", OleDbType.LongVarChar);
                    command.Parameters.AddWithValue("BACKUPSTATUS", OleDbType.Integer);
                    command.Parameters.AddWithValue("DES", OleDbType.LongVarChar);
                    command.Parameters.AddWithValue("unitcode", OleDbType.LongVarChar);

                    command.Parameters[0].Value = sRecordID;
                    command.Parameters[1].Value = sTaskID;
                    command.Parameters[2].Value = sPlanID;
                    command.Parameters[3].Value = 1;
                    command.Parameters[4].Value = Path.GetFileName(sStrFiles);
                    command.Parameters[5].Value = sPath;
                    command.Parameters[6].Value = DateTime.Now;
                    command.Parameters[7].Value = "admin";
                    command.Parameters[8].Value = -1;
                    command.Parameters[9].Value = "";
                    command.Parameters[10].Value = "";

                    command.ExecuteNonQuery();

                }
                catch (Exception ex)
                {
                    //MessageBox.Show(ex.Message);
                }
                connection.Close();
            }
        }
        private int IICFix(string sFile, string sPath, string sID, string sBackupPath)
        {
            string sFullFile = sBackupPath + sPath + sFile;

            try
            {
                using (OleDbConnection sqlconn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + sFullFile + ";Persist Security Info=True"))
                {
                    string sqlCreate = "CREATE TABLE Detect_IIC_Info (" +
    "detecttrain varchar(255) NULL," +
    "detectdir varchar(255) NULL," +
    "detectmainperson varchar(255) NULL," +
    "detectdate varchar(255) NULL," +
    "linecode varchar(255) NULL," +
    "linedir varchar(255) NULL," +
    "traindir varchar(255) NULL," +
    "trainnumber varchar(255) NULL," +
    "planid varchar(255) NULL," +
    "taskid varchar(255) NULL," +
    "recordid varchar(255) NULL," +
    "backupid varchar(255) NULL," +
    "manageid varchar(255) NULL" +
    ");";
                    OleDbCommand sqlcom = new OleDbCommand(sqlCreate, sqlconn);
                    sqlconn.Open();
                    sqlcom.ExecuteNonQuery();
                    sqlconn.Close();
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show("创建自解释表失败:" + ex.Message);
                return -1;
            }



            try
            {
                using (OleDbConnection sqlconn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + sFullFile + ";Persist Security Info=True"))
                {
                    string sqlSQL = "insert into Detect_IIC_Info values(" + sID + ");";
                    OleDbCommand sqlcom = new OleDbCommand(sqlSQL, sqlconn);
                    sqlconn.Open();

                    sqlcom.ExecuteNonQuery();
                    sqlconn.Close();
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show("插入自解释表内容失败!:" + ex.Message);
                return -2;
            }


            return 0;
        }
        private string AutoCreateDirectory(string sTaskID, string sRecordID, string sManagePath, string sBackupPath, int iDT)
        {
            string sTrain = string.Empty;
            string sDate = string.Empty;
            string sLine = string.Empty;
            string sDir = string.Empty;
            string sYear = string.Empty;
            string sMonth = string.Empty;
            string sDay = string.Empty;
            using (OracleConnection connection = new OracleConnection(GetConn(iDT)))
            {
                try
                {
                    OracleCommand command = new OracleCommand("select t1.detecttrain,t2.detectdate,t2.linename,(select t1.name from tbl00_dic_line_dir t1 where t2.linedirection=t1.id) from tbl00_detect_task t1,pw_run_record t2 where  t1.id=" + sTaskID + " and t1.id=t2.taskid and t2.detectid=" + sRecordID);
                    command.Connection = connection;

                    connection.Open();
                    OracleDataReader oddr = command.ExecuteReader();
                    if (oddr.Read())
                    {
                        sTrain = oddr[0].ToString();
                        sDate = oddr[1].ToString();
                        sLine = oddr[2].ToString();
                        sDir = oddr[3].ToString();
                    }

                    oddr.Close();
                }
                catch (Exception ex)
                {
                    //MessageBox.Show(ex.Message);
                }
                connection.Close();
            }
            //分解日期
            sYear = sDate.Substring(0, 4);
            sMonth = sDate.Substring(5, 2);
            sDay = sDate.Substring(8, 2);
            string sBackPath = string.Empty;
            sBackPath = sTrain + "\\" + sLine + "\\" + sYear + "\\" + sMonth + "\\" + sDay + "\\" + sDir + "\\";
            //创建目录
            if (!Directory.Exists(sBackupPath + sBackPath))
            {
                Directory.CreateDirectory(sBackupPath + sBackPath);
            }
            return sBackPath;
        }
        private string getRemoteIPAddress()
        {
            return ((IPEndPoint)currentSocket.RemoteEndPoint).
                    Address.ToString();
        }

        //ServiceClient方法用于和客户端进行数据通信，包括接收客户端的请求，
        //根据不同的请求命令，执行相应的操作，并将处理结果返回到客户端
        public void ServiceClient()
        {
            string[] tokens = null;
            byte[] buff = new byte[8192];
            bool keepConnect = true;
            WaveformDataProcess dpc = new WaveformDataProcess();
            //各数据检测文件
            string gjfile = string.Empty;
            string dlxfile = string.Empty;
            string gwfile = string.Empty;

            //各数据通道数
            int gjtds = 50;
            int dlxtds = 27;
            int gwtds = 23;
            //增减里程
            string jclczj = "增";

            //通道比例
            float[] f1 = new float[gjtds];
            float[] f2 = new float[dlxtds];
            float[] f3 = new float[gwtds];
            //通道偏移
            float[] g1 = new float[gjtds];
            float[] g2 = new float[dlxtds];
            float[] g3 = new float[gwtds];
            ////

            //用循环来不断地与客户端进行交互，直到客户端发出“EXIT”命令，
            //将keepConnect置为false，退出循环，关闭连接，并中止当前线程
            while (keepConnect && ControlsForm.SocketServiceFlag)
            {
                tokens = null;
                try
                {
                    if (currentSocket == null ||
                        currentSocket.Available < 1)
                    {
                        Thread.Sleep(300);
                        continue;
                    }
                    //接收数据并存入buff数组中
                    int len = currentSocket.Receive(buff);
                    //将字符数组转化为字符串
                    //MessageBox.Show(len.ToString());
                    string clientCommand = System.Text.Encoding.Default.GetString(
                                                         buff, 0, len);
                    //MessageBox.Show(clientCommand);
                    //tokens[0]中保存了命令标志符（CONN、CHAT、PRIV、LIST或EXIT）
                    tokens = clientCommand.Split(new Char[] { '|' });

                    if (tokens == null)
                    {
                        Thread.Sleep(200);
                        continue;
                    }
                }
                catch (Exception e)
                {
                    server.updateUI("发生异常：" + e.ToString());
                }

                try
                {
                    if (tokens[0].Equals("CONN"))
                    {
                        //此时接收到的命令格式为：
                        //命令标志符（CONN）|发送者的用户名|，
                        //tokens[1]中保存了发送者的用户名
                        this.name = tokens[1];
                        if (ControlsForm.clients.Contains(this.name))
                        {
                            SendToClient(this, "ERR|User " + this.name + " 已经存在" + "|#");

                        }
                        else
                        {
                            Hashtable syncClients = Hashtable.Synchronized(
                                ControlsForm.clients);
                            syncClients.Add(this.name, this);

                            //更新状态
                            state = "connected";
                            SendToClient(this, "LOGINOK" + "|#");
                        }

                    }
                    else if (tokens[0].Equals("UPDATECONFIG"))
                    {   //更新配置文件信息
                        this.name = tokens[1];
                        server.updateUI(this.name + "请求更新XML配置文件\n");
                        server.updateUI("服务器配置文件版本号：" + ControlsForm.XMLVersion.ToString());
                        server.updateUI("客户端配置文件版本号：" + tokens[2]);
                        if (ControlsForm.XMLVersion.Equals(tokens[2]))
                        {
                            server.updateUI(this.name + " 没有最新的配置文件更新\n");
                            SendToClient(this, "OK" + "|没有最新的数据更新|#");
                        }
                        else
                        {
                            using (StreamReader sr = new StreamReader("appconfig.xml"))
                            {
                                SendToClient(this, "UPDATECONFIG|" + sr.ReadToEnd());
                            }
                            server.updateUI(this.name + " 正在接收最新的配置文件更新\n");
                        }


                    }
                    else if (tokens[0].Equals("LIST"))
                    {
                        if (state.Equals("connected"))
                        {
                            //更新当前客户端的状态
                            string msgUsers = "OK|成功!";
                            if (ControlsForm.clients.Contains(this.name))
                            {
                                string v = "否";
                                if (tokens[2] == "1")
                                    v = "是";
                                server.updateUI(this.name + "同步状态更改为 " + v + "组号" + tokens[3] + "\n");

                            }
                            SendToClient(this, msgUsers + "|#");
                        }
                        else
                        {
                            //send err to server
                            SendToClient(this, "ERR|state error，Please login first" + "|#");
                        }
                    }
                    else if (tokens[0] == "LISTDATABYID")//根据ID获取当前管理所有文件的列表
                    {
                        //此时接收到的命令的格式为：
                        //命令标志符（LISTDATA）
                        //获取所有可用的数据列表
                        using (OleDbConnection connection = new OleDbConnection(ControlsForm.DBConnectString))
                        {
                            OleDbCommand command = new OleDbCommand("select file_path from PW_WAVE_FILE t where detectid=" + tokens[1] + " order by detect_date desc");
                            command.Connection = connection;
                            //StringB str1 = string.Empty;
                            StringBuilder sb = new StringBuilder();
                            try
                            {
                                connection.Open();
                                OleDbDataReader oddr = command.ExecuteReader();
                                if (!oddr.HasRows)
                                {
                                    SendToClient(this, "LISTDATABYIDREV|0|#");
                                    server.updateUI(tokens[1] + "查看了数据！");
                                    continue;
                                }
                                if (oddr.Read())
                                {
                                    sb.Append(oddr.GetValue(0).ToString());

                                }
                                oddr.Close();
                                //sb.Remove(sb.Length - 1, 1);
                                SendToClient(this, "LISTDATABYIDREV|" + sb.ToString() + "|#");
                                server.updateUI(tokens[1] + "数据！");
                            }
                            catch (Exception ex)
                            {
                                SendToClient(this, "ERR|" + ex.Message + "|#");
                                server.updateUI(tokens[1] + "查看了数据！但是没有成功");
                                Thread.Sleep(200);
                            }
                            keepConnect = false;
                        }
                    }
                    else if (tokens[0] == "LISTDATA")//获取所有当前管理所有文件的列表
                    {
                        //此时接收到的命令的格式为：
                        //命令标志符（LISTDATA）
                        //获取所有可用的数据列表
                        string sLineName = tokens[2];
                        string sLineDir = tokens[3];
                        string sTrainNo = tokens[4];
                        string sTimePickerStart = tokens[5];
                        string sTimePickerEnd = tokens[6];
                        string sSQL = "";
                        if (ControlsForm.sRunMode == 0)
                        {
                            if (!string.IsNullOrEmpty(sLineName))
                            {
                                sSQL += (" and t.lineid='" + sLineName + "' ");
                            }
                            if (!string.IsNullOrEmpty(sLineDir))
                            {
                                sSQL += (" and t.linedirection='" + sLineDir + "' ");
                            }
                            if (!string.IsNullOrEmpty(sTrainNo))
                            {
                                sSQL += (" and t1.detecttrain='" + sTrainNo + "' ");
                            }
                            sSQL += (" and t.detectdate>='" + sTimePickerStart + "' and t.detectdate<='" + sTimePickerEnd + "' ");
                        }
                        switch (ControlsForm.sRunMode)
                        {
                            case 0:
                                using (OleDbConnection connection = new OleDbConnection(ControlsForm.DBConnectString))
                                {
                                    OleDbCommand command = new OleDbCommand("select getline_name(t.lineid) linename,t.lineid,getlinedirection(t.linedirection) " +
"linedirection,t.kminc mileage_change,t.detectdate detect_date, " +
"'00:00:00' detect_starttime,t1.detecttrain DETECT_TRAINID,t2.datapath||t2.dataname file_path " +
" from pw_run_record t,tbl00_detect_task t1,tbl00_detect_data_manage t2 where t.taskid=t1.id and t.detectid=t2.recordid " +
" and t.taskid=t2.taskid and t2.datatype=0 " + sSQL + " order by t.detectdate desc");
                                    command.Connection = connection;
                                    //StringB str1 = string.Empty;
                                    StringBuilder sb = new StringBuilder();
                                    try
                                    {
                                        connection.Open();
                                        OleDbDataReader oddr = command.ExecuteReader();
                                        if (!oddr.HasRows)
                                        {
                                            SendToClient(this, "LISTDATAREV|0|#");
                                            server.updateUI(tokens[1] + "查看了数据！");
                                            continue;
                                        }
                                        while (oddr.Read())
                                        {
                                            sb.Append(oddr.GetValue(0).ToString() + "~");
                                            sb.Append(oddr.GetValue(1).ToString() + "~");
                                            sb.Append(oddr.GetValue(2).ToString() + "~");
                                            sb.Append(oddr.GetValue(3).ToString() + "~");
                                            sb.Append(oddr.GetValue(4).ToString() + "~");
                                            sb.Append(oddr.GetValue(5).ToString() + "~");
                                            sb.Append(oddr.GetValue(6).ToString() + "~");
                                            string sFileName = oddr.GetValue(7).ToString();
                                            sb.Append(Path.GetFileName(sFileName) + "~");
                                            sb.Append("0~");
                                            sb.Append(Path.GetDirectoryName(sFileName) + "^");
                                        }
                                        oddr.Close();
                                        sb.Remove(sb.Length - 1, 1);
                                        SendToClient(this, "LISTDATAREV|" + sb.ToString() + "|#");
                                        server.updateUI(tokens[1] + "请求了波形数据！");
                                    }
                                    catch (Exception ex)
                                    {
                                        SendToClient(this, "ERR|" + ex.Message + "|#");
                                        server.updateUI(tokens[1] + "查看了数据！但是没有成功");
                                        Thread.Sleep(200);
                                    }
                                }
                                break;
                            case 1:
                                using (OleDbConnection connection = new OleDbConnection(ControlsForm.DBMobileConnectString))
                                {
                                    OleDbCommand command = new OleDbCommand("select LINENAME,LINEID,LINEDIRECTION," +
                                        "MILEAGE_CHANGE,DETECT_DATE,DETECT_STARTTIME,DETECT_TRAINID,FILE_PATH from tbl08_02FileManager t order by DETECT_DATE desc");
                                    command.Connection = connection;
                                    //StringB str1 = string.Empty;
                                    StringBuilder sb = new StringBuilder();
                                    try
                                    {
                                        connection.Open();
                                        OleDbDataReader oddr = command.ExecuteReader();
                                        if (!oddr.HasRows)
                                        {
                                            SendToClient(this, "LISTDATAREV|0|#");
                                            server.updateUI(tokens[1] + "查看了数据！");
                                            continue;
                                        }
                                        while (oddr.Read())
                                        {
                                            sb.Append(oddr.GetValue(0).ToString() + "~");
                                            sb.Append(oddr.GetValue(1).ToString() + "~");
                                            string sXB = oddr.GetValue(2).ToString();
                                            if (sXB.EndsWith("01"))
                                            {
                                                sb.Append("下~");
                                            }
                                            else if (sXB.EndsWith("02"))
                                            {
                                                sb.Append("上~");
                                            }
                                            else
                                            {
                                                sb.Append("单~");
                                            }
                                            string sZJ = oddr.GetValue(3).ToString();
                                            if (sZJ.EndsWith("0"))
                                            {
                                                sb.Append("增~");
                                            }
                                            else if (sZJ.EndsWith("1"))
                                            {
                                                sb.Append("减~");
                                            }

                                            sb.Append(oddr.GetDateTime(4).ToString("yyyy-MM-dd") + "~");
                                            sb.Append(oddr.GetValue(5).ToString() + "~");
                                            sb.Append(oddr.GetValue(6).ToString() + "~");
                                            string sFileName = oddr.GetValue(7).ToString();
                                            sb.Append(Path.GetFileName(sFileName) + "~");
                                            sb.Append("0~");
                                            sb.Append(Path.GetDirectoryName(sFileName) + "^");
                                        }
                                        oddr.Close();
                                        sb.Remove(sb.Length - 1, 1);
                                        SendToClient(this, "LISTDATAREV|" + sb.ToString() + "|#");
                                        server.updateUI(tokens[1] + "请求了波形数据！");
                                    }
                                    catch (Exception ex)
                                    {
                                        SendToClient(this, "ERR|" + ex.Message + "|#");
                                        server.updateUI(tokens[1] + "查看了数据！但是没有成功");
                                        Thread.Sleep(200);
                                    }
                                }
                                break;
                            case 2:

                                break;
                        }

                        keepConnect = false;
                    }
                    else if (tokens[0] == "LISTFILECHANNELS")//获取指定文件的文件头
                    {
                        try
                        {

                            //LocalDataAddress
                            string sFileName = sLocalDataAddress + tokens[1];
                            CITDataProcess cdp = new CITDataProcess();
                            cdp.QueryDataInfoHead(sFileName);
                            string status = cdp.QueryDataChannelInfoHead(sFileName);

                            SendToClient(this, "LISTFILECHANNELSREV|" + status + "|#");
                            server.updateUI(sFileName + "读取文件通道信息成功！");
                        }
                        catch (Exception ex)
                        {
                            SendToClient(this, "ERR|" + ex.Message + "|#");
                            server.updateUI(tokens[1] + "读取文件通道信息没有成功！");
                            Thread.Sleep(200);
                        }
                        keepConnect = false;
                    }
                    else if (tokens[0] == "LISTFILEHEAD")//获取指定文件的文件头
                    {
                        try
                        {

                            //LocalDataAddress
                            string sFileName = sLocalDataAddress + tokens[1];
                            CITDataProcess cdp = new CITDataProcess();
                            string status = cdp.QueryDataInfoHead(sFileName);

                            SendToClient(this, "LISTFILEHEADREV|" + status + "|#");
                            server.updateUI(sFileName + "读取文件头成功！");
                        }
                        catch (Exception ex)
                        {
                            SendToClient(this, "ERR|" + ex.Message + "|#");
                            server.updateUI(tokens[1] + "读取文件头没有成功！");
                            Thread.Sleep(200);
                        }
                        keepConnect = false;
                    }
                    else if (tokens[0] == "METERFIND")//获取文件里程定位
                    {
                        try
                        {

                            //LocalDataAddress
                            string sFileName = sLocalDataAddress + tokens[1];
                            WaveformDataProcess cdp = new WaveformDataProcess();
                            int status = cdp.AnalyseDataAutoFindCorLC(sFileName, long.Parse(tokens[2]), int.Parse(tokens[3]),
                                int.Parse(tokens[4]), int.Parse(tokens[5]), int.Parse(tokens[6]), bool.Parse(tokens[7]));

                            SendToClient(this, "METERFINDREV|" + status.ToString() + "|#");
                            server.updateUI(sFileName + "-" + status.ToString() + "查找里程定位成功！");
                        }
                        catch (Exception ex)
                        {
                            SendToClient(this, "ERR|" + ex.Message + "|#");
                            server.updateUI(tokens[1] + "查找里程定位没有成功！");
                            Thread.Sleep(200);
                        }
                        keepConnect = false;
                    }
                    else if (tokens[0] == "LISTCHANNELSCALE")//获取指定文件的通道比例等
                    {
                        try
                        {

                            //LocalDataAddress
                            string sFileName = sLocalDataAddress + tokens[1];
                            WaveformDataProcess wdp = new WaveformDataProcess();
                            float[] fScale = new float[int.Parse(tokens[2])];
                            float[] fOffset = new float[int.Parse(tokens[2])];
                            wdp.GetChannelScale(sFileName, int.Parse(tokens[2]), ref fScale, ref fOffset);
                            MemoryStream ms1 = new MemoryStream();
                            MemoryStream ms2 = new MemoryStream();
                            BinaryFormatter formatter = new BinaryFormatter();
                            formatter.Serialize(ms1, fScale);
                            formatter.Serialize(ms2, fOffset);

                            SendToClient(this, ms1.ToArray(), ms2.ToArray(), (byte)20);
                            server.updateUI(sFileName + "读取通道比例成功！");
                        }
                        catch (Exception ex)
                        {
                            SendToClient(this, "ERR|" + ex.Message + "|#");
                            server.updateUI(tokens[1] + "读取通道比例没有成功！");
                            Thread.Sleep(200);
                        }
                        keepConnect = false;
                    }
                    else if (tokens[0] == "LISTSTARTENDLENGTHINFO")//获取指定文件的头尾信息
                    {
                        try
                        {

                            //LocalDataAddress
                            string sFileName = sLocalDataAddress + tokens[1];
                            WaveformDataProcess wdp = new WaveformDataProcess();
                            long lStart = 0;
                            long lEnd = 0;
                            wdp.GetDataStartPositionEndPositionInfoIncludeIndex(ref lStart, ref lEnd, sFileName, int.Parse(tokens[5]), long.Parse(tokens[6]), long.Parse(tokens[7]), bool.Parse(tokens[8]));

                            MemoryStream ms1 = new MemoryStream();
                            MemoryStream ms2 = new MemoryStream();
                            BinaryFormatter formatter = new BinaryFormatter();
                            formatter.Serialize(ms1, lStart);
                            formatter.Serialize(ms2, lEnd);

                            SendToClient(this, ms1.ToArray(), ms2.ToArray(), (byte)20);
                            server.updateUI(sFileName + "读取头尾信息成功！");
                        }
                        catch (Exception ex)
                        {
                            SendToClient(this, "ERR|" + ex.Message + "|#");
                            server.updateUI(tokens[1] + "读取头尾信息没有成功！");
                            Thread.Sleep(200);
                        }
                        keepConnect = false;
                    }
                    else if (tokens[0] == "LISTGETDATA")//获取指定文件数据信息
                    {
                        try
                        {

                            //LocalDataAddress
                            string sFileName = sLocalDataAddress + tokens[1];
                            WaveformDataProcess wdp = new WaveformDataProcess();

                            List<IndexStaClass> listIndex = new List<IndexStaClass>();
                            List<int> listChannelIndex = new List<int>();
                            string[] sSplit = tokens[3].Split(new char[] { ',' });
                            for (int i = 0; i < sSplit.Length; i++)
                            {
                                listChannelIndex.Add(int.Parse(sSplit[i]));
                            }
                            float[][] p = new float[listChannelIndex.Count][];
                            List<WaveMeter> listWM = new List<WaveMeter>();
                            wdp.GetDataInfo(listChannelIndex, ref p, ref listWM, long.Parse(tokens[4]), sFileName, long.Parse(tokens[5]),
                                int.Parse(tokens[6]), int.Parse(tokens[7]), int.Parse(tokens[8]), long.Parse(tokens[9]), long.Parse(tokens[10]), listIndex, bool.Parse(tokens[11]),
                                tokens[12], bool.Parse(tokens[13]), bool.Parse(tokens[14]), false, 0);

                            MemoryStream ms1 = new MemoryStream();
                            MemoryStream ms2 = new MemoryStream();
                            BinaryFormatter formatter = new BinaryFormatter();
                            formatter.Serialize(ms1, p);
                            formatter.Serialize(ms2, listWM);

                            SendToClient(this, ms1.ToArray(), ms2.ToArray(), (byte)20);

                            server.updateUI(sFileName + "读取文件数据信息成功！长度" + (ms1.Length + ms2.Length).ToString());
                        }
                        catch (Exception ex)
                        {
                            SendToClient(this, "ERR|" + ex.Message + "|#");
                            server.updateUI(tokens[1] + "读取文件数据信息没有成功！");
                            Thread.Sleep(200);
                        }
                        keepConnect = false;
                    }
                    else if (tokens[0] == "DMIMP")//
                    {
                        try
                        {
                            if (int.Parse(tokens[1]) == 0)
                            {
                                UseCase.UseSampleDJ IICIMPInto = new UseCase.UseSampleDJ();
                                bool sResult = IICIMPInto.IICDataImp(int.Parse(tokens[2]));
                            }
                            else
                            {
                                UseCase.UseSampleGJ IICIMPInto = new UseCase.UseSampleGJ();
                                bool sResult = IICIMPInto.IICDataImp(int.Parse(tokens[2]));
                            }


                            SendToClient(this, "IIC数据导入成功!");
                        }
                        catch (Exception ex)
                        {
                            SendToClient(this, "ERR|" + ex.Message + "|#");
                            server.updateUI(tokens[1] + "出错!");
                            Thread.Sleep(200);
                        }
                        keepConnect = false;
                    }
                    else if (tokens[0] == "DMCOPYDATAIIC")//获取指定文件数据信息
                    {
                        try
                        {
                            string[] str = tokens[2].Split('`');
                            string sPath = str[0];
                            string sName = str[1];
                            string sSQL = str[2];
                            string sManagePath = GetManagePath(int.Parse(tokens[1]));
                            string sBackupPath = GetBackupPath(int.Parse(tokens[1]));
                            server.updateUI("确认目录和备份目录成功！");
                            int iResult = IICFix(sName, sPath, sSQL, sBackupPath);
                            string sDestPath = sManagePath + sPath + sName;
                            string sSourcePath = sBackupPath + sPath + sName;
                            server.updateUI("IICFix！");
                            Directory.CreateDirectory(sManagePath + sPath);
                            File.Copy(sSourcePath, sDestPath, true);

                            SendToClient(this, "IIC数据确认成功!");
                        }
                        catch (Exception ex)
                        {
                            SendToClient(this, "ERR|" + ex.Message + "|#");
                            server.updateUI(tokens[1] + "出错!" + ex.Message);
                            Thread.Sleep(200);
                        }
                        keepConnect = false;
                    }
                    else if (tokens[0] == "DMCOPYDATACIT")//获取指定文件数据信息CIT
                    {  /*
                        * 0号车下来的波形文件就是cit，其余的车都是geo，
                        * 因此拷贝cit，cit通道规整化，滤波等功能，
                        * 只是针对0号车的
                        */
                        try
                        {
                            string[] str = tokens[2].Split(',');
                            string sPath = str[0];
                            string sName = str[1];
                            string sManagePath = GetManagePath(int.Parse(tokens[1]));
                            string sBackupPath = GetBackupPath(int.Parse(tokens[1]));
                            string sDestPath = sManagePath + sPath + sName;
                            string sSourcePath = sBackupPath + sPath + sName;
                            Directory.CreateDirectory(sManagePath + sPath);
                            File.Copy(sSourcePath, sDestPath, true);
                            List<DataProcess.GEO2CITBind> listGEO2CIT = new List<GEO2CITBind>();
                            if (File.Exists("c:\\GEOConfig\\CIT001.csv"))
                            {
                                server.updateUI("CIT预处理成功1-2！!");
                                StreamReader sr = new StreamReader("c:\\GEOConfig\\CIT001.csv", Encoding.Default);
                                while (sr.Peek() != -1)
                                {
                                    string[] sSplit = sr.ReadLine().Split(new char[] { '=' });
                                    GEO2CITBind fa = new GEO2CITBind();
                                    fa.sCIT = sSplit[0];
                                    fa.sGEO = sSplit[1];
                                    fa.sChinese = sSplit[2];
                                    listGEO2CIT.Add(fa);
                                }
                                sr.Close();
                                server.updateUI("CIT预处理成功2！!" + listGEO2CIT.Count.ToString());
                            }
                            server.updateUI("CIT预处理成功1！!");
                            CITDataProcess cdp = new CITDataProcess();
                            cdp.ModifyCITHeader(sDestPath, str[2], str[3], listGEO2CIT);
                            server.updateUI(tokens[1] + "CIT处理成功！");
                            SendToClient(this, "CIT数据确认成功!");
                        }
                        catch (Exception ex)
                        {
                            SendToClient(this, "ERR|" + ex.Message + "|#");
                            server.updateUI(tokens[1] + "出错!");
                            Thread.Sleep(200);
                        }
                        keepConnect = false;
                    }
                    else if (tokens[0] == "DMCOPYDATAGEO")//获取指定文件数据信息GEO
                    {
                        try
                        {
                            string[] str = tokens[2].Split('`');
                            string sPath = str[0];
                            string sName = str[1];
                            string[] sPara = new string[7];
                            sPara[0] = str[2];
                            sPara[1] = str[3];
                            sPara[2] = str[4];
                            sPara[3] = str[5];
                            sPara[4] = str[6];
                            sPara[5] = str[7];
                            sPara[6] = str[8];
                            string sTrain = str[9];
                            string sManagePath = GetManagePath(int.Parse(tokens[1]));
                            string sBackupPath = GetBackupPath(int.Parse(tokens[1]));
                            string sDestPath = sManagePath + sPath + Path.GetFileNameWithoutExtension(sName) + ".cit";
                            string sSourcePath = sBackupPath + sPath + sName;
                            Directory.CreateDirectory(sManagePath + sPath);
                            List<DataProcess.GEO2CITBind> listGEO2CIT = new List<GEO2CITBind>();
                            server.updateUI("GEO预处理成功1！!" + sTrain);
                            if (File.Exists("c:\\GEOConfig\\" + sTrain + ".csv"))
                            {
                                server.updateUI("GEO预处理成功1-2！!");
                                StreamReader sr = new StreamReader("c:\\GEOConfig\\" + sTrain + ".csv", Encoding.Default);
                                while (sr.Peek() != -1)
                                {
                                    string[] sSplit = sr.ReadLine().Split(new char[] { '=' });
                                    GEO2CITBind fa = new GEO2CITBind();
                                    fa.sCIT = sSplit[0];
                                    fa.sGEO = sSplit[1];
                                    fa.sChinese = sSplit[2];
                                    listGEO2CIT.Add(fa);
                                }
                                sr.Close();
                                server.updateUI("GEO预处理成功2！!" + listGEO2CIT.Count.ToString());
                                GEODataProcess geodp = new GEODataProcess();
                                geodp.ConvertData(sSourcePath, sDestPath, sPara, 1, listGEO2CIT);
                                //补零
                                Application.DoEvents();
                                //geodp.ProcessFile(sDestPath);
                                //
                                SendToClient(this, "GEO数据确认成功!");
                            }
                            else
                            {
                                SendToClient(this, "12121");
                            }
                        }
                        catch (Exception ex)
                        {
                            SendToClient(this, "ERR|" + ex.Message + "|#");
                            server.updateUI(tokens[1] + "出错!");
                            Thread.Sleep(200);
                        }
                        keepConnect = false;
                    }
                    else if (tokens[0] == "DMADDDATA")//获取指定文件数据信息
                    {
                        try
                        {
                            //拆分接收到的序列
                            string[] str = tokens[2].Split('^');
                            string sPlanID = str[0];
                            string sTaskID = str[1];
                            string sRecordID = str[2];
                            string sStrFiles = str[3];    
                            string sManagePath = GetManagePath(int.Parse(tokens[1]));
                            server.updateUI(" GetManagePath传输的内容!" + sManagePath);
                            string sBackupPath = GetBackupPath(int.Parse(tokens[1]));
                            server.updateUI("GetBackupPath传输的内容!" + sBackupPath);
                            //创建相关目录
                            string sPath = AutoCreateDirectory(sTaskID, sRecordID, sManagePath, sBackupPath, int.Parse(tokens[1]));
                            server.updateUI("AutoCreateDirectory");
                            server.updateUI(sPath);
                            //数据打开
                            //IIC获取
                            server.updateUI("sStrFiles" + sStrFiles);
                            if (sStrFiles.Length > 0 && sStrFiles.ToLower().EndsWith(".iic"))
                            {
                                IICBackup(sPlanID, sTaskID, sRecordID, sStrFiles, sPath, sBackupPath, int.Parse(tokens[1]));
                            }

                            //cit获取
                            if (sStrFiles.Length > 0 && sStrFiles.ToLower().EndsWith(".cit"))
                            {
                                CITBackup(sPlanID, sTaskID, sRecordID, sStrFiles, sPath, sBackupPath, int.Parse(tokens[1]));
                            }

                            //geo获取
                            if (sStrFiles.Length > 0 && sStrFiles.ToLower().EndsWith(".geo"))
                            {
                                GEOBackup(sPlanID, sTaskID, sRecordID, sStrFiles, sPath, sBackupPath, int.Parse(tokens[1]));
                            }

                            server.updateUI(tokens[1] + "传输的内容!");

                            SendToClient(this, "导入检测数据成功!");
                        }
                        catch (Exception ex)
                        {
                            SendToClient(this, "ERR|" + ex.Message + "|#");
                            server.updateUI(tokens[1] + "出错!");
                            Thread.Sleep(200);
                        }
                        keepConnect = false;
                    }
                    else if (tokens[0] == "GETDATAMILEAGEINFO")//获取指定文件数据信息
                    {
                        try
                        {
                            //LocalDataAddress
                            string sFileName = sLocalDataAddress + tokens[1];
                            WaveformDataProcess wdp = new WaveformDataProcess();
                            int[] iMeter = wdp.GetDataMileageInfo(sFileName, int.Parse(tokens[2]), int.Parse(tokens[3]), bool.Parse(tokens[4]), new List<IndexStaClass>(), false, 0, 0,"");
                            MemoryStream ms1 = new MemoryStream();
                            BinaryFormatter formatter = new BinaryFormatter();
                            formatter.Serialize(ms1, iMeter);
                            SendToClient(this, ms1.ToArray(), (byte)40);
                            server.updateUI(sFileName + "读取文件数据信息成功！长度" + (ms1.Length).ToString());
                        }
                        catch (Exception ex)
                        {
                            SendToClient(this, "ERR|" + ex.Message + "|#");
                            server.updateUI(tokens[1] + "读取文件数据信息没有成功！");
                            Thread.Sleep(200);
                        }
                        keepConnect = false;
                    }
                    else if (tokens[0] == "STATUS")//波形状态汇报
                    {
                        try
                        {
                            server.updateUI("接收状态汇报请求,请求方:" + tokens[1]);

                            server.updateUI("查询到的运行状态为：RUN");

                            SendToClientFast(this, "STATUS|RUN");

                        }
                        catch (Exception ex)
                        {
                            server.updateUI("状态汇报错误:" + ex.Message);
                            SendToClientFast(this, "STATUS|OK");
                        }
                        Thread.Sleep(100);
                        keepConnect = false;
                    }
                    else if (tokens[0] == "GETWAVEALLDATAFILE")//获取指定文件全部里程文件信息，可用波形软件查看
                    {
                        try
                        {
                            server.updateUI("接收到获取文件整体指令");
                            for (int i = 0; i < tokens.Length; i++)
                            {
                                server.updateUINoLine(tokens[i]);
                            }
                            string sID = tokens[1];

                            //查找是否有该波形文件
                            string sPath = string.Empty;
                            using (OleDbConnection connection = new OleDbConnection(ControlsForm.DBConnectString))
                            {
                                OleDbCommand command = new OleDbCommand("select datapath||dataname from " +
                                    "tbl00_detect_data_manage t where id='" + sID + "'");
                                command.Connection = connection;
                                //StringB str1 = string.Empty;
                                StringBuilder sb = new StringBuilder();
                                try
                                {
                                    connection.Open();
                                    server.updateUI(command.CommandText);
                                    OleDbDataReader oddr = command.ExecuteReader();
                                    if (oddr.Read())
                                    {
                                        sPath = oddr.GetString(0);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    SendToClient(this, null, (byte)1);
                                    server.updateUI(ex.Message);
                                    Thread.Sleep(200);
                                    keepConnect = false;
                                    return;
                                }
                            }
                            string sFileName = sLocalDataAddress + sPath;

                            byte[] bResult = File.ReadAllBytes(sFileName);
                            if (bResult.Length > 0)
                            {
                                SendToClientAll(this, bResult, (byte)10);
                                server.updateUI("发送波形全部文件成功，大小" + bResult.Length.ToString());
                            }
                            else
                            {
                                SendToClient(this, null, (byte)2);
                                server.updateUI("ERR|202|#");
                                Thread.Sleep(200);
                            }


                        }
                        catch (Exception ex)
                        {
                            SendToClient(this, null, (byte)2);
                            server.updateUI(ex.Message);
                            Thread.Sleep(200);
                        }
                        keepConnect = false;
                    }
                    else if (tokens[0] == "GETWAVEDATAFILE")//获取指定文件的里程的文件片段，片段可用波形软件查看最新
                    {
                        try
                        {
                            server.updateUI("接收到获取文件片段指令");
                            for (int i = 0; i < tokens.Length; i++)
                            {
                                server.updateUINoLine(tokens[i]);
                            }
                            string sID = tokens[1];
                            float fStartMile = float.Parse(tokens[2]);
                            float fEndMile = float.Parse(tokens[3]);

                            //查找是否有该波形文件
                            string sPath = string.Empty;
                            using (OleDbConnection connection = new OleDbConnection(ControlsForm.DBConnectString))
                            {
                                OleDbCommand command = new OleDbCommand("select datapath||dataname from " +
                                    "tbl00_detect_data_manage t where id='" + sID + "'");
                                command.Connection = connection;
                                //StringB str1 = string.Empty;
                                StringBuilder sb = new StringBuilder();
                                try
                                {
                                    connection.Open();
                                    server.updateUI(command.CommandText);
                                    OleDbDataReader oddr = command.ExecuteReader();
                                    if (oddr.Read())
                                    {
                                        sPath = oddr.GetString(0);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    SendToClient(this, null, (byte)1);
                                    server.updateUI(ex.Message);
                                    Thread.Sleep(200);
                                    keepConnect = false;
                                    return;
                                }
                            }
                            string sFileName = sLocalDataAddress + sPath;
                            long lStartMile = (long)(fStartMile * 1000);
                            long lEndMile = (long)(fEndMile * 1000);
                            byte[] bResult = new byte[512];
                            CITDataProcess cdp = new CITDataProcess();
                            cdp.QueryDataInfoHead(sFileName);
                            cdp.QueryDataChannelInfoHead(sFileName);
                            server.updateUI(sFileName + "," + lStartMile.ToString() + "," + lEndMile.ToString());
                            int iResult = cdp.ExportData(sFileName, ref bResult, lStartMile, lEndMile);
                            if (iResult == 0)
                            {
                                SendToClient(this, bResult, (byte)10);
                                server.updateUI("发送波形片段文件成功，大小" + bResult.Length.ToString());
                            }
                            else
                            {
                                SendToClient(this, null, (byte)2);
                                server.updateUI("ERR|202|#");
                                Thread.Sleep(200);
                            }


                        }
                        catch (Exception ex)
                        {
                            SendToClient(this, null, (byte)2);
                            server.updateUI(ex.Message);
                            Thread.Sleep(200);
                        }
                        keepConnect = false;
                    }
                    else if (tokens[0] == "LISTGETDATAWAVEFILE")//获取指定文件的里程的文件片段，片段可用波形软件查看
                    {
                        try
                        {
                            server.updateUI("接收到获取文件片段指令");
                            for (int i = 0; i < tokens.Length; i++)
                            {
                                server.updateUINoLine(tokens[i]);
                            }
                            string[] str = tokens[1].Split(new char[] { ',' });
                            //串内容，lineid,xb,date,start,end
                            //查找是否有该波形文件
                            string sPath = "";
                            using (OleDbConnection connection = new OleDbConnection(ControlsForm.DBMobileConnectString))
                            {
                                OleDbCommand command = new OleDbCommand("select FILE_PATH from " +
                                    "tbl08_02FileManager t where LINEID='" + str[0] + "' and LINEDIRECTION=" + str[1] +
                                    " and DETECT_DATE='" + str[2] + "'");
                                command.Connection = connection;
                                //StringB str1 = string.Empty;
                                StringBuilder sb = new StringBuilder();
                                try
                                {
                                    connection.Open();
                                    server.updateUI(command.CommandText);
                                    OleDbDataReader oddr = command.ExecuteReader();
                                    if (oddr.Read())
                                    {
                                        sPath = oddr.GetString(0);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    SendToClient(this, "ERR|204|#");
                                    server.updateUI(ex.Message);
                                    Thread.Sleep(200);
                                    keepConnect = false;
                                    return;
                                }
                            }
                            string sFileName = sLocalDataAddress + sPath;
                            long lStartMile = (long)(double.Parse(str[3]) * 1000);
                            long lEndMile = (long)(double.Parse(str[4]) * 1000);
                            byte[] bResult = new byte[512];
                            CITDataProcess cdp = new CITDataProcess();
                            cdp.QueryDataInfoHead(sFileName);
                            cdp.QueryDataChannelInfoHead(sFileName);
                            server.updateUI(sFileName + "," + lStartMile.ToString() + "," + lEndMile.ToString());
                            int iResult = cdp.ExportData(sFileName, ref bResult, lStartMile, lEndMile);
                            if (iResult == 0)
                            {
                                SendToClient(this, bResult, (byte)10);
                                server.updateUI("发送波形片段文件成功，大小" + bResult.Length.ToString());
                            }
                            else
                            {
                                SendToClient(this, "ERR|202|#");
                                server.updateUI("ERR|202|#");
                                Thread.Sleep(200);
                            }


                        }
                        catch (Exception ex)
                        {
                            SendToClient(this, "ERR|201|#");
                            server.updateUI(ex.Message);
                            Thread.Sleep(200);
                        }
                        keepConnect = false;
                    }
                    else if (tokens[0] == "AUTOINSERTEXCEPTION")//根据ID获取大值信息并插入数据库
                    {
                        string sID = tokens[2];
                        string sLeftCount = tokens[3];
                        string sRightCount = tokens[4];
                        string sDetectid = "";
                        string sLineid = "";
                        string sLineDir = "";
                        int max_km;
                        int max_m;
                        string detect_date = "";
                        server.updateUI(tokens[2] + "连接请求获取大值,删除已有数据");

                        try
                        {
                            using (OleDbConnection connection = new OleDbConnection(ControlsForm.DBConnectString))
                            {
                                OleDbCommand command = new OleDbCommand("delete from tbl02_10pw_atta t where CRITICAL_EXCEPTION_NUM=" + sID);
                                command.Connection = connection;

                                connection.Open();
                                command.ExecuteNonQuery();
                                connection.Close();
                            }
                        }
                        catch(Exception ex)
                        {
                            server.updateUI(tokens[2] + "连接请求获取大值，查找数据"+ex.Message);
                        }
                        server.updateUI(tokens[2] + "连接请求获取大值，查找数据");
                        try
                        {
                            using (OleDbConnection connection = new OleDbConnection(ControlsForm.DBConnectString))
                            {
                                OleDbCommand command = new OleDbCommand("select detectid," +
                                    "lineid,linedirection,max_km,max_m,DETECT_DATE from TBL02_09GJ_CRITICALEXCEPTION t  where exception_num=" + sID);
                                command.Connection = connection;

                                connection.Open();
                                OleDbDataReader oddr = command.ExecuteReader();
                                if (oddr.Read())
                                {
                                    sDetectid = oddr.GetValue(0).ToString();
                                    sLineid = oddr.GetValue(1).ToString();
                                    sLineDir = oddr.GetValue(2).ToString();
                                    max_km = int.Parse(oddr.GetValue(3).ToString());
                                    max_m = (int)float.Parse(oddr.GetValue(4).ToString());
                                    detect_date = oddr.GetValue(5).ToString();
                                }
                                else
                                {
                                    throw new Exception("3434");
                                }
                                oddr.Close();
                                connection.Close();
                            }
                        }
                        catch(Exception ex)
                        {
                            SendToClientFast(this, "AUTOINSERTEXCEPTIONREV|false|没有找到大值信息");
                            server.updateUI(tokens[2] + "查看了数据！但是没有成功 "+ex.Message);
                            Thread.Sleep(200);
                            keepConnect = false;
                            break;
                        }
                        SendToClientFast(this, "AUTOINSERTEXCEPTIONREV|true|找到大值信息");
                        //select * from (select t1.detectid,t2.file_path from PW_RUN_RECORD t1,PW_WAVE_FILE t2 where t1.lineid='0008' and t1.linedirection='01' and t1.detectdate<='2010-10-15' and t1.detectid=t2.detectid order by t1.detectdate desc) where  rownum<=5
                        List<string> listDETECTID = new List<string>();
                        List<string> listWaveFile = new List<string>();
                        //Left
                        try
                        {
                            using (OleDbConnection connection = new OleDbConnection(ControlsForm.DBConnectString))
                            {
                                string sql = "select t.recordid,t.datapath||t.dataname from tbl00_detect_data_manage t where t.recordid in ( select t1.detectid from pw_run_record t1 " +
"where t1.lineid='" + sLineid + "' and t1.linedirection='" + sLineDir + "' and t1.detectdate<='" + detect_date + "'" +
" and t1.detectdate>'" + DateTime.Parse(detect_date).AddMonths(-3).ToString("yyyy-MM-dd") + "') and t.datatype=0";
                                OleDbCommand command = new OleDbCommand(sql);
                                command.Connection = connection;

                                connection.Open();
                                OleDbDataReader oddr = command.ExecuteReader();
                                while (oddr.Read())
                                {
                                    listDETECTID.Add(oddr.GetValue(0).ToString());
                                    listWaveFile.Add(sLocalDataAddress + oddr.GetValue(1).ToString());
                                }

                                oddr.Close();
                                connection.Close();
                            }
                        }
                        catch
                        {
                            SendToClientFast(this, "AUTOINSERTEXCEPTIONREV|false|找超限表出错！");
                            server.updateUI(tokens[2] + "查看了数据！但是没有成功");
                            Thread.Sleep(200);
                            keepConnect = false;
                            break;
                        }
                        //Right
                        try
                        {
                            using (OleDbConnection connection = new OleDbConnection(ControlsForm.DBConnectString))
                            {
                                string sql = "select t.recordid,t.datapath||t.dataname from tbl00_detect_data_manage t where t.recordid in ( select t1.detectid from pw_run_record t1 " +
"where t1.lineid='" + sLineid + "' and t1.linedirection='" + sLineDir + "' and t1.detectdate>'" + detect_date + "'" +
" and t1.detectdate<='" + DateTime.Parse(detect_date).AddMonths(2).ToString("yyyy-MM-dd") + "') and t.datatype=0";
                                OleDbCommand command = new OleDbCommand(sql);
                                command.Connection = connection;

                                connection.Open();
                                OleDbDataReader oddr = command.ExecuteReader();
                                while (oddr.Read())
                                {
                                    listDETECTID.Add(oddr.GetValue(0).ToString());
                                    listWaveFile.Add(sLocalDataAddress + oddr.GetValue(1).ToString());
                                }

                                oddr.Close();
                                connection.Close();
                            }
                        }
                        catch
                        {
                            SendToClientFast(this, "AUTOINSERTEXCEPTIONREV|false|找超限表出错！");
                            server.updateUI(tokens[2] + "查看了数据！但是没有成功");
                            Thread.Sleep(200);
                            keepConnect = false;
                            break;
                        }
                        //拼接里程

                        //获取文件里程范围
                        CITDataProcess cdp = new CITDataProcess();
                        for (int i = 0; i < listWaveFile.Count; i++)
                        {
                            cdp.QueryDataInfoHead(listWaveFile[i]);
                            cdp.QueryDataChannelInfoHead(listWaveFile[i]);
                            long lStart = 0;
                            long lEnd = 0;
                            switch (cdp.dhi.iKmInc)
                            {
                                case 0:
                                    if (max_km < 2)
                                    {
                                        lStart = 0;
                                        lEnd = (max_km + 2) * 1000;
                                    }
                                    else
                                    {
                                        lStart = (max_km - 2) * 1000;
                                        lEnd = (max_km + 2) * 1000;
                                    }
                                    break;
                                case 1:
                                    if (max_km < 2)
                                    {
                                        lEnd = 0;
                                        lStart = (max_km + 2) * 1000;
                                    }
                                    else
                                    {
                                        lEnd = (max_km - 2) * 1000;
                                        lStart = (max_km + 2) * 1000;
                                    }
                                    break;
                            }
                            byte[] bResult = new byte[512];
                            int iResult = cdp.ExportData(listWaveFile[i], ref bResult, lStart, lEnd);
                            if (iResult == 0)
                            {
                                server.updateUI(tokens[2] + listWaveFile[i] + "|ok");
                                server.updateUI(tokens[2] + listWaveFile[i] + "|" + lStart.ToString() + "-" + lEnd.ToString());
                                server.updateUI(tokens[2] + listWaveFile[i] + "|" + bResult.Length.ToString());
                                //插入数据库
                                try
                                {//select seq_tbl02_10pw_atta.nextval from dual
                                    string sUUID = "";
                                    using (OleDbConnection connection = new OleDbConnection(ControlsForm.DBConnectString))
                                    {
                                        string sql = "select seq_tbl02_10pw_atta.nextval from dual";
                                        OleDbCommand command = new OleDbCommand(sql);
                                        command.Connection = connection;
                                        connection.Open();

                                        OleDbDataReader oddr = command.ExecuteReader();
                                        if (oddr.Read())
                                        {
                                            sUUID = oddr.GetValue(0).ToString();
                                        }
                                        oddr.Close();

                                        connection.Close();
                                    }

                                    using (OleDbConnection connection = new OleDbConnection(ControlsForm.DBConnectString))
                                    {
                                        string sql = "insert into TBL02_10PW_ATTA values(" + sUUID + "," + sID + ",1,?,?,'Auto','',?,0)";
                                        OleDbCommand command = new OleDbCommand(sql);
                                        command.Connection = connection;
                                        connection.Open();
                                        command.Parameters.Add("P1", OleDbType.LongVarChar);
                                        command.Parameters.Add("P2", OleDbType.LongVarBinary);
                                        command.Parameters.Add("P3", OleDbType.LongVarChar);
                                        command.Parameters[0].Value = cdp.dhi.sTrackName + "-" + cdp.dhi.sDate + "-" +
                                            max_km.ToString() + "." + max_m.ToString();
                                        command.Parameters[1].Value = bResult;
                                        command.Parameters[2].Value = Path.GetFileName(listWaveFile[i]);
                                        command.ExecuteNonQuery();


                                        connection.Close();
                                    }
                                    server.updateUI(tokens[2] + listWaveFile[i] + "|成功");
                                }
                                catch (Exception ex)
                                {
                                    server.updateUI(tokens[2] + listWaveFile[i] + "|" + ex.Message + "|插入数据库失败");
                                    SendToClientFast(this, "AUTOINSERTEXCEPTIONREV|false|插入数据库失败");
                                    Thread.Sleep(200);
                                    keepConnect = false;
                                }

                            }
                            else
                            {
                                server.updateUI(tokens[2] + listWaveFile[i] + "|error");
                            }
                        }
                        SendToClientFast(this, "AUTOINSERTEXCEPTIONREV|OK");
                        Thread.Sleep(200);
                        keepConnect = false;
                        break;
                    }
                    else if (tokens[0] == "AUTOPOINTRETURN")
                    {
                        //if (state == "connected")
                        //{
                        //    try
                        //    {
                        //        server.updateUI(tokens[1] + "要求发送客户端数据最后一屏幕 ");
                        //        PointsClass pc = new PointsClass();
                        //        dpc.AnalyseDataLCGJ(ref pc, gjfile, long.Parse(tokens[8]), int.Parse(tokens[9]), gjjg, gjtds);
                        //        dpc.AnalyseDataLCDLX(ref pc, dlxfile, long.Parse(tokens[8]), int.Parse(tokens[9]), dlxjg, dlxtds);
                        //        dpc.AnalyseDataLCGW(ref pc, gwfile, long.Parse(tokens[8]), int.Parse(tokens[9]), gwjg, gwtds);
                        //        //传输各个子系统的数据
                        //        pc.kmzj = jclczj;
                        //        pc.gjxz = long.Parse(tokens[10]);
                        //        pc.dlxxz = long.Parse(tokens[11]);
                        //        pc.gwxz = long.Parse(tokens[12]);
                        //        pc.curScrollValue = int.Parse(tokens[8]);
                        //        dpc.AnalyseDataGJAURE(ref pc, gjfile, long.Parse(tokens[8]), int.Parse(tokens[9]), gjjg, gjtds, f1, g1);
                        //        dpc.AnalyseDataDLXAURE(ref pc, dlxfile, long.Parse(tokens[8]), int.Parse(tokens[9]), dlxjg, dlxtds, f2, g2);
                        //        dpc.AnalyseDataGWAURE(ref pc, gwfile, long.Parse(tokens[8]), int.Parse(tokens[9]), gwjg, gwtds, f3, g3);
                        //        switch (int.Parse(tokens[14]))
                        //        {
                        //            case 0:
                        //                if (pc.gjEndPos == 0)
                        //                {
                        //                    if (pc.dlxEndPos == 0)
                        //                    {
                        //                        pc.startPos = pc.gwStartPos;
                        //                        pc.endPos = pc.gwEndPos;
                        //                        pc.ddx = gwtds * 2;
                        //                    }
                        //                    else
                        //                    {
                        //                        pc.startPos = pc.dlxStartPos;
                        //                        pc.endPos = pc.dlxEndPos;
                        //                        pc.ddx = dlxtds * 2;
                        //                    }
                        //                }
                        //                else
                        //                {
                        //                    pc.startPos = pc.gjStartPos;
                        //                    pc.endPos = pc.gjEndPos;
                        //                    pc.ddx = gjtds * 2;
                        //                }
                        //                break;
                        //            case 1:
                        //                if (pc.dlxEndPos == 0)
                        //                {
                        //                    if (pc.gwEndPos == 0)
                        //                    {
                        //                        pc.startPos = pc.gjStartPos;
                        //                        pc.endPos = pc.gjEndPos;
                        //                        pc.ddx = gjtds * 2;
                        //                    }
                        //                    else
                        //                    {
                        //                        pc.startPos = pc.gwStartPos;
                        //                        pc.endPos = pc.gwEndPos;
                        //                        pc.ddx = gwtds * 2;
                        //                    }
                        //                }
                        //                else
                        //                {
                        //                    pc.startPos = pc.dlxStartPos;
                        //                    pc.endPos = pc.dlxEndPos;
                        //                    pc.ddx = dlxtds * 2;
                        //                }
                        //                break;
                        //            case 2:
                        //                if (pc.gwEndPos == 0)
                        //                {
                        //                    if (pc.gjEndPos == 0)
                        //                    {
                        //                        pc.startPos = pc.dlxStartPos;
                        //                        pc.endPos = pc.dlxEndPos;
                        //                        pc.ddx = dlxtds * 2;
                        //                    }
                        //                    else
                        //                    {
                        //                        pc.startPos = pc.gjStartPos;
                        //                        pc.endPos = pc.gjEndPos;
                        //                        pc.ddx = gjtds * 2;

                        //                    }
                        //                }
                        //                else
                        //                {
                        //                    pc.startPos = pc.gwStartPos;
                        //                    pc.endPos = pc.gwEndPos;
                        //                    pc.ddx = gwtds * 2;
                        //                }

                        //                break;
                        //        }

                        //        //tc.AnalyseData1(ref pc, "12", long.Parse(tokens[7]), int.Parse(tokens[8]));
                        //        //序列化对象并发送给客户端
                        //        using (MemoryStream ms = new MemoryStream())
                        //        {
                        //            BinaryFormatter formatter = new BinaryFormatter();
                        //            formatter.Serialize(ms, pc);
                        //            SendToClient(this, ms.ToArray(), (byte)23);

                        //        }
                        //    }
                        //    catch (Exception ex)
                        //    {
                        //        server.updateUI(tokens[1] + ex.Message);
                        //        Thread.Sleep(200);
                        //    }
                        //}
                        //else
                        //{
                        //    SendToClient(this, "ERR|state error，Please login first" + "|#");
                        //}


                    }
                    else if (tokens[0] == "AUTOCORPOINT")
                    {
                        //server.updateUI(tokens[1] + "要求发送客户端数据(自动对正)1 ");
                        //PointsClass pc = new PointsClass();
                        //long jzlc = 0;
                        //switch (int.Parse(tokens[13]))
                        //{
                        //    case 0:
                        //        jzlc = long.Parse(tokens[10]);
                        //        dpc.AnalyseDataAutoCorLCGW(ref pc, gwfile, long.Parse(tokens[8]), int.Parse(tokens[9]), gwjg, jzlc,gwtds);
                        //        dpc.AnalyseDataAutoCorLCDLX(ref pc, dlxfile, long.Parse(tokens[8]), int.Parse(tokens[9]), dlxjg, jzlc, dlxtds);
                        //        break;
                        //    case 1:
                        //        jzlc = long.Parse(tokens[11]);
                        //        dpc.AnalyseDataAutoCorLCGW(ref pc, gwfile, long.Parse(tokens[8]), int.Parse(tokens[9]), gwjg, jzlc, gwtds);
                        //        dpc.AnalyseDataAutoCorLCGJ(ref pc, gjfile, long.Parse(tokens[8]), int.Parse(tokens[9]), gjjg, jzlc, gjtds);
                        //        break;
                        //    case 2:
                        //        jzlc = long.Parse(tokens[12]);
                        //        dpc.AnalyseDataAutoCorLCGJ(ref pc, gjfile, long.Parse(tokens[8]), int.Parse(tokens[9]), gjjg, jzlc,gjtds);
                        //        dpc.AnalyseDataAutoCorLCDLX(ref pc, dlxfile, long.Parse(tokens[8]), int.Parse(tokens[9]), dlxjg, jzlc, dlxtds);
                        //        break;
                        //}

                        //dpc.AnalyseDataGJ(ref pc, gjfile, long.Parse(tokens[8]), int.Parse(tokens[9]), gjjg,gjtds,f1,g1);
                        //dpc.AnalyseDataDLX(ref pc, dlxfile, long.Parse(tokens[8]), int.Parse(tokens[9]), dlxjg,dlxtds,f2,g2);
                        //dpc.AnalyseDataGW(ref pc, gwfile, long.Parse(tokens[8]), int.Parse(tokens[9]), gwjg,gwtds,f3,g3);


                        ////序列化对象并发送给客户端
                        //using (MemoryStream ms = new MemoryStream())
                        //{
                        //    BinaryFormatter formatter = new BinaryFormatter();
                        //    formatter.Serialize(ms, pc);
                        //    SendToClient(this, ms.ToArray(), (byte)21);
                        //}
                    }
                    else if (tokens[0] == "CORPOINT")
                    {
                        //server.updateUI(tokens[1] + "要求发送客户端数据1 ");
                        //PointsClass pc = new PointsClass();
                        //dpc.AnalyseDataLCGJ(ref pc, gjfile, long.Parse(tokens[8]), int.Parse(tokens[9]), gjjg,gjtds);
                        //dpc.AnalyseDataLCDLX(ref pc, dlxfile, long.Parse(tokens[8]), int.Parse(tokens[9]), dlxjg,dlxtds);
                        //dpc.AnalyseDataLCGW(ref pc, gwfile, long.Parse(tokens[8]), int.Parse(tokens[9]), gwjg,gwtds);
                        //pc.gjxz = long.Parse(tokens[10]);
                        //pc.dlxxz = long.Parse(tokens[11]);
                        //pc.gwxz = long.Parse(tokens[12]);
                        //dpc.AnalyseDataGJ(ref pc, gjfile, long.Parse(tokens[8]), int.Parse(tokens[9]), gjjg,gjtds,f1,g1);
                        //dpc.AnalyseDataDLX(ref pc, dlxfile, long.Parse(tokens[8]), int.Parse(tokens[9]), dlxjg,dlxtds,f2,g2);
                        //dpc.AnalyseDataGW(ref pc, gwfile, long.Parse(tokens[8]), int.Parse(tokens[9]), gwjg,gwtds,f3,g3);


                        ////序列化对象并发送给客户端
                        //using (MemoryStream ms = new MemoryStream())
                        //{
                        //    BinaryFormatter formatter = new BinaryFormatter();
                        //    formatter.Serialize(ms, pc);
                        //    SendToClient(this, ms.ToArray(), (byte)21);
                        //}
                    }
                    else if (tokens[0] == "EXIT")
                    {
                        //此时接收到的命令的格式为：命令标志符（EXIT）|发送者的用户名
                        if (ControlsForm.clients.Contains(tokens[1]))
                        {
                            Client client = (Client)ControlsForm.clients[tokens[1]];

                            //将该用户对应的Client对象从clients中删除
                            Hashtable syncClients = Hashtable.Synchronized(
                                ControlsForm.clients);
                            syncClients.Remove(client.name);
                            server.updateUI(tokens[1] + "线程退出！");
                        }

                        //退出当前线程
                        break;
                    }
                    else if (tokens[0] == "EXPORT")
                    {
                        server.updateUI(tokens[1] + "导出数据!");

                        string[] ResultStr = new string[3];
                        int[] ResultInt = new int[3];
                        int[] ResultTDS = new int[3];
                        bool[] isExport = new bool[3];
                        isExport[0] = bool.Parse(tokens[5]);
                        isExport[1] = bool.Parse(tokens[6]);
                        isExport[2] = bool.Parse(tokens[7]);
                        server.updateUI(GetDatabaseRecord(ref ResultStr[0], ref  ResultStr[1], ref  ResultStr[2], tokens[8], tokens[9], tokens[10], tokens[11], ref ResultInt[0], ref ResultInt[1], ref ResultInt[2], ref ResultTDS[0], ref ResultTDS[1], ref ResultTDS[2], ref jclczj));
                        if (tokens[2].Equals("False"))
                        {

                        }
                        else
                        {
                            if (dpc.ExportData(ControlsForm.ExportDataAddress, long.Parse(tokens[3]), long.Parse(tokens[4]), isExport, ResultStr, ResultInt, ResultTDS, tokens[8], tokens[9], tokens[10], tokens[11], "", true))
                            { server.updateUI("导出原始数据成功!"); }
                            else { server.updateUI("导出原始数据失败!"); }
                        }

                        server.updateUI("当前导出数据线程退出！");
                        break;
                        //this.currentSocket.Close();
                        //keepConnect = false;
                    }
                    else if (tokens[0] == "GETAUTOPOINT")
                    {
                        try
                        {
                            List<byte[]> listBB = new List<byte[]>();
                            WaveformDataProcess wdp = new WaveformDataProcess();
                            List<AutoPointClass> listAutoPointClass = new List<AutoPointClass>();
                            //初始化数据
                            for (int iIndex = 1; iIndex < tokens.Length; iIndex++)
                            {
                                string[] strPara = tokens[iIndex].Split('~');
                                AutoPointClass apc = new AutoPointClass();
                                apc.sFileName = sLocalDataAddress + strPara[0];
                                apc.iDataType = int.Parse(strPara[1]);
                                apc.listChannel = new List<int>();
                                string[] sSplit = strPara[2].Split(new char[] { ',' });
                                for (int i = 0; i < sSplit.Length; i++)
                                {
                                    apc.listChannel.Add(int.Parse(sSplit[i]));
                                }
                                apc.lReviseValue = long.Parse(strPara[3]);
                                apc.XZoomIn = int.Parse(strPara[5]);
                                apc.iSmaleRate = int.Parse(strPara[6]);
                                apc.iChannelNumber = int.Parse(strPara[7]);
                                apc.lStartPosition = long.Parse(strPara[8]);
                                apc.lEndPosition = long.Parse(strPara[9]);
                                wdp.GetDataStartPositionEndPositionInfoIncludeIndex(ref apc.lStartPosition,
                                    ref apc.lEndPosition, apc.sFileName, apc.iChannelNumber, -1, -1, false);
                                apc.sKmInc = strPara[10];
                                apc.bEncrypt = bool.Parse(strPara[11]);
                                apc.lPos = long.Parse(strPara[8]) + long.Parse(strPara[4]) * apc.iChannelNumber * 2 * apc.iSmaleRate * (apc.XZoomIn / 10)
                                    + apc.lReviseValue * apc.iChannelNumber * 2;
                                listAutoPointClass.Add(apc);
                            }

                            while (true)
                            {
                                int iSum = 0;
                                listBB.Clear();
                                //获取多少可用
                                int iReadCount = 10000;
                                for (int i = 0; i < listAutoPointClass.Count; i++)
                                {
                                    wdp.GetDataStartPositionEndPositionInfoIncludeIndex(ref listAutoPointClass[i].lStartPosition,
                                    ref listAutoPointClass[i].lEndPosition, listAutoPointClass[i].sFileName, listAutoPointClass[i].iChannelNumber, -1, -1, false);
                                    long lContainPoint = (long)((listAutoPointClass[i].lEndPosition - listAutoPointClass[i].lPos) /
                                        (listAutoPointClass[i].iChannelNumber * 2.0) / listAutoPointClass[i].iSmaleRate);
                                    if (lContainPoint >= listAutoPointClass[i].XZoomIn)
                                    {
                                        lContainPoint = 200;
                                    }
                                    if (lContainPoint < iReadCount)
                                    {
                                        iReadCount = (int)lContainPoint;
                                    }
                                }

                                //获取数据
                                for (int iIndex = 0; iIndex < listAutoPointClass.Count; iIndex++)
                                {
                                    server.updateUI(listAutoPointClass[iIndex].sFileName + "要求发送客户端实时数据");
                                    float[][] p = new float[listAutoPointClass[iIndex].listChannel.Count][];
                                    List<WaveMeter> listWM = new List<WaveMeter>();
                                    int iResultCount = wdp.GetAutoDataInfo(listAutoPointClass[iIndex].listChannel, ref p, ref listWM, listAutoPointClass[iIndex].sFileName,
                                        ref listAutoPointClass[iIndex].lPos, listAutoPointClass[iIndex].XZoomIn, listAutoPointClass[iIndex].iSmaleRate, listAutoPointClass[iIndex].iChannelNumber,
                                    listAutoPointClass[iIndex].lStartPosition, listAutoPointClass[iIndex].lEndPosition, listAutoPointClass[iIndex].sKmInc, listAutoPointClass[iIndex].bEncrypt, iReadCount);
                                    MemoryStream ms1 = new MemoryStream();
                                    MemoryStream ms2 = new MemoryStream();
                                    BinaryFormatter formatter = new BinaryFormatter();
                                    formatter.Serialize(ms1, p);
                                    formatter.Serialize(ms2, listWM);
                                    listBB.Add(ms1.ToArray());
                                    listBB.Add(ms2.ToArray());
                                    listBB.Add(BitConverter.GetBytes(iResultCount));
                                }
                                for (int iIndex = 0; iIndex < listBB.Count; iIndex++)
                                {
                                    iSum += listBB[iIndex].Length;
                                }

                                if (iReadCount > 0)
                                {
                                    SendToClientAutoProcess(this, listBB, (byte)99, iSum);
                                    server.updateUI("自动更新数据发送成功！大小" + (iSum).ToString());
                                }
                                Thread.Sleep(50);

                            }
                        }
                        catch (Exception ex)
                        {
                            server.updateUI(tokens[1] + ex.Message);
                            Thread.Sleep(100);
                        }

                    }
                    else if (tokens[0] == "AUTOCALC")
                    {
                        server.updateUI(tokens[1] + "请求计算数据！数据库连接:" + ControlsForm.DBConnectString);
                        List<DataProcess.CalcSpace.QueryClass> listQuery = new List<DataProcess.CalcSpace.QueryClass>();
                        DataProcess.CalcSpace.CalcProcess cp = new DataProcess.CalcSpace.CalcProcess();
                        using (OleDbConnection connection = new OleDbConnection(ControlsForm.DBConnectString))
                        {
                            OleDbCommand command = new OleDbCommand("select t.id,t.recordid,t.taskid,t.planid,t.datapath||t.dataname,t1.rundirection,t2.detecttrain from tbl00_detect_data_manage t,pw_run_record t1,tbl00_detect_task t2 where t2.id=t.taskid and t2.id=t1.taskid and t.recordid=t1.detectid and t.datatype=0");
                            command.Connection = connection;
                            connection.Open();
                            OleDbDataReader oddr = command.ExecuteReader();

                            while (oddr.Read())
                            {
                                DataProcess.CalcSpace.QueryClass qc = new DataProcess.CalcSpace.QueryClass();
                                qc.backid = int.Parse(oddr.GetValue(0).ToString());
                                qc.recordid = int.Parse(oddr.GetValue(1).ToString());
                                qc.taskid = int.Parse(oddr.GetValue(2).ToString());
                                qc.planid = int.Parse(oddr.GetValue(3).ToString());
                                qc.sFilePath = sLocalDataAddress + oddr.GetValue(4).ToString();
                                qc.sRun = oddr.GetValue(5).ToString();
                                qc.sTrain = oddr.GetValue(6).ToString();
                                listQuery.Add(qc);
                            }
                            oddr.Close();
                            connection.Close();
                            server.updateUI(tokens[1] + "请求计算数据！数量:" + listQuery.Count);
                        }
                        //开始计算
                        using (OleDbConnection connection = new OleDbConnection(ControlsForm.DBConnectString))
                        {
                            OleDbCommand command = new OleDbCommand();
                            command.Connection = connection;
                            connection.Open();
                            for (int i = 0; i < listQuery.Count; i++)
                            {
                                server.updateUI(tokens[1] + "请求:" + i.ToString() + " " + listQuery[i].sFilePath);
                                //List<DataProcess.CalcSpace.CalcClass1> listCla1 = cp.ProcessCalculateDynamicCriteriaPeak(listQuery[i].sFilePath, listQuery[i].planid, listQuery[i].taskid, listQuery[i].recordid, listQuery[i].backid);
                                //for (int j = 0; j < listCla1.Count; j++)
                                //{
                                //    command.CommandText = "insert into tbl00_calc_data_log values(" + listCla1[j].planid.ToString() +
                                //        "," + listCla1[j].taskid.ToString() +
                                //        "," + listCla1[j].recordid.ToString() +
                                //        "," + listCla1[j].backid.ToString() +
                                //        "," + listCla1[j].startkm.ToString() +
                                //        "," + listCla1[j].startm.ToString() +
                                //        "," + listCla1[j].endkm.ToString() +
                                //        "," + listCla1[j].endm.ToString() +
                                //        "," + listCla1[j].value.ToString() +
                                //        ",'" + listCla1[j].des.ToString() +
                                //        "','" + listCla1[j].channel.ToString() +
                                //        "','" + listCla1[j].para.ToString() +
                                //        "')";
                                //    command.ExecuteNonQuery();
                                //    Application.DoEvents();
                                //}
                                //listCla1 = null;
                                //listCla1 = cp.ProcessCalculateDynamicCriteriaP2P(listQuery[i].sFilePath, listQuery[i].planid, listQuery[i].taskid, listQuery[i].recordid, listQuery[i].backid);
                                //for (int j = 0; j < listCla1.Count; j++)
                                //{
                                //    command.CommandText = "insert into tbl00_calc_data_log values(" + listCla1[j].planid.ToString() +
                                //        "," + listCla1[j].taskid.ToString() +
                                //        "," + listCla1[j].recordid.ToString() +
                                //        "," + listCla1[j].backid.ToString() +
                                //        "," + listCla1[j].startkm.ToString() +
                                //        "," + listCla1[j].startm.ToString() +
                                //        "," + listCla1[j].endkm.ToString() +
                                //        "," + listCla1[j].endm.ToString() +
                                //        "," + listCla1[j].value.ToString() +
                                //        ",'" + listCla1[j].des.ToString() +
                                //        "','" + listCla1[j].channel.ToString() +
                                //        "','" + listCla1[j].para.ToString() +
                                //        "')";
                                //    command.ExecuteNonQuery();
                                //    Application.DoEvents();
                                //}
                                //listCla1 = null;
                                //listCla1 = cp.ProcessCalculateComprehensiveCriteriaOnSwitch(listQuery[i].sFilePath, listQuery[i].planid, listQuery[i].taskid, listQuery[i].recordid, listQuery[i].backid);
                                //for (int j = 0; j < listCla1.Count; j++)
                                //{
                                //    command.CommandText = "insert into tbl00_calc_data_log values(" + listCla1[j].planid.ToString() +
                                //        "," + listCla1[j].taskid.ToString() +
                                //        "," + listCla1[j].recordid.ToString() +
                                //        "," + listCla1[j].backid.ToString() +
                                //        "," + listCla1[j].startkm.ToString() +
                                //        "," + listCla1[j].startm.ToString() +
                                //        "," + listCla1[j].endkm.ToString() +
                                //        "," + listCla1[j].endm.ToString() +
                                //        "," + listCla1[j].value.ToString() +
                                //        ",'" + listCla1[j].des.ToString() +
                                //        "','" + listCla1[j].channel.ToString() +
                                //        "','" + listCla1[j].para.ToString() +
                                //        "')";
                                //    command.ExecuteNonQuery();
                                //    Application.DoEvents();
                                //}
                                try
                                {
                                    List<DataProcess.CalcSpace.CalcClass1> listCla1 = null;
                                    listCla1 = cp.ProcessCalculateGEI(listQuery[i].sFilePath, listQuery[i].planid, listQuery[i].taskid, listQuery[i].recordid, listQuery[i].backid, listQuery[i].sRun, listQuery[i].sTrain);
                                    for (int j = 0; j < listCla1.Count; j++)
                                    {
                                        command.CommandText = "insert into tbl00_calc_data_log values(" + listCla1[j].planid.ToString() +
                                            "," + listCla1[j].taskid.ToString() +
                                            "," + listCla1[j].recordid.ToString() +
                                            "," + listCla1[j].backid.ToString() +
                                            "," + listCla1[j].startkm.ToString() +
                                            "," + listCla1[j].startm.ToString() +
                                            "," + listCla1[j].endkm.ToString() +
                                            "," + listCla1[j].endm.ToString() +
                                            "," + listCla1[j].value.ToString() +
                                            ",'" + listCla1[j].des.ToString() +
                                            "','" + listCla1[j].channel.ToString() +
                                            "','" + listCla1[j].para.ToString() +
                                            "')";
                                        command.ExecuteNonQuery();
                                        Application.DoEvents();
                                    }
                                }
                                catch
                                {

                                }
                            } connection.Close();
                        }
                        server.updateUI(tokens[1] + "计算数据完成！数量:" + listQuery.Count);
                    }
                    Thread.Sleep(100);
                }
                catch (Exception e)
                {
                    server.updateUI("发生异常：" + e.ToString());
                    Thread.Sleep(200);
                }
            }
        }
        //SendToClient()方法实现了向客户端发送命令请求的功能不用数据头
        public void SendToClientFast(Client client, string msg)
        {
            byte[] message = System.Text.Encoding.Default.GetBytes(
                    msg.ToCharArray());
            client.CurrentSocket.Send(message, message.Length, 0);
        }
        //SendToClient()方法实现了向客户端发送命令请求的功能
        public void SendToClient(Client client, string msg)
        {
            byte[] message = System.Text.Encoding.Default.GetBytes(
                    msg.ToCharArray());
            byte[] b1 = new byte[message.Length + 6];
            b1[0] = (byte)231;
            b1[1] = (byte)1;
            BitConverter.GetBytes(message.Length).CopyTo(b1, 2);
            message.CopyTo(b1, 6);
            client.CurrentSocket.Send(b1, b1.Length, 0);
        }
        //向客户端发送包含数据的内容
        public void SendToClientAll(Client client, byte[] b, byte i)
        {
            byte[] b1 = new byte[6];
            if (b == null)
            {

                b1[0] = (byte)231;
                b1[1] = i;
                BitConverter.GetBytes(0).CopyTo(b1, 2);
                client.CurrentSocket.Send(b1, b1.Length, 0);
                return;
            }
            b1 = new byte[6];
            b1[0] = (byte)231;
            b1[1] = i;
            BitConverter.GetBytes(b.Length).CopyTo(b1, 2);
            client.CurrentSocket.Send(b1, b1.Length, 0);

            for (int j = 0; j < b.Length; j += 10240)
            {
                int iLength = 10240;
                if ((b.Length - j) < 10240)
                {
                    iLength = b.Length - j;
                }
                client.CurrentSocket.Send(b, j, iLength, 0);
            }
        }
        //向客户端发送包含数据的内容
        public void SendToClient(Client client, byte[] b, byte i)
        {
            byte[] b1 = new byte[6];
            if (b == null)
            {

                b1[0] = (byte)231;
                b1[1] = i;
                BitConverter.GetBytes(0).CopyTo(b1, 2);
                client.CurrentSocket.Send(b1, b1.Length, 0);
                return;
            }
            b1 = new byte[b.Length + 6];
            b1[0] = (byte)231;
            b1[1] = i;
            BitConverter.GetBytes(b.Length).CopyTo(b1, 2);
            b.CopyTo(b1, 6);
            client.CurrentSocket.Send(b1, b1.Length, 0);
        }
        //
        public void SendToClientAutoProcess(Client client, List<byte[]> listBB, byte i, int iSum)
        {
            byte[] b1 = new byte[iSum + 6 + 4 * listBB.Count];
            b1[0] = (byte)231;
            b1[1] = i;
            BitConverter.GetBytes(iSum + 4 * listBB.Count).CopyTo(b1, 2);
            int iPos = 6;
            for (int iIndex = 0; iIndex < listBB.Count; iIndex++)
            {
                BitConverter.GetBytes(listBB[iIndex].Length).CopyTo(b1, iPos);
                iPos += 4;
                listBB[iIndex].CopyTo(b1, iPos);
                iPos += listBB[iIndex].Length;
            }
            client.CurrentSocket.Send(b1, b1.Length, 0);
        }
        public void SendToClient(Client client, byte[] bb1, byte[] bb2, byte i)
        {

            byte[] b1 = new byte[bb1.Length + bb2.Length + 6 + 4];
            b1[0] = (byte)231;
            b1[1] = i;
            BitConverter.GetBytes(bb1.Length + bb2.Length + 4).CopyTo(b1, 2);
            BitConverter.GetBytes(bb1.Length).CopyTo(b1, 6);
            bb1.CopyTo(b1, 10);
            bb2.CopyTo(b1, 6 + 4 + bb1.Length);
            client.CurrentSocket.Send(b1, b1.Length, 0);
        }
        public string GetDatabaseRecord(ref string gjfile, ref string dlxfile, ref string gwfile, string riqi, string xm, string xb, string jcxh, ref int gjjg, ref int dlxjg, ref int gwjg, ref int gjtds, ref int dlxtds, ref int gwtds, ref string jclczj)
        {
            string[] datatype = new string[3] { "轨检", "动力学", "弓网" };
            using (OleDbConnection connection = new OleDbConnection(ControlsForm.DBConnectString))
            {
                OleDbCommand command = new OleDbCommand();
                command.Connection = connection;
                try
                {
                    connection.Open();
                    for (int i = 0; i < 3; i++)
                    {
                        command.CommandText = "select jcyssjwj,jccyjg,jctds,jclczj from ZHJC_SJDY where jcrq='" +
                            riqi + "' and jcxm='" + xm +
                            "' and jcxb='" + xb + "'" + " and jclb='" + datatype[i] + "' and jcxh=" + jcxh;

                        OleDbDataReader oddr = command.ExecuteReader();
                        if (!oddr.HasRows)
                        {
                            oddr.Close();
                            continue;
                        }
                        while (oddr.Read())
                        {
                            switch (i)
                            {
                                case 0:
                                    gjfile = oddr.GetString(0);
                                    gjjg = oddr.GetInt16(1);
                                    gjtds = oddr.GetInt16(2);
                                    jclczj = oddr.GetString(3);
                                    break;
                                case 1:
                                    dlxfile = oddr.GetString(0);
                                    dlxjg = oddr.GetInt16(1);
                                    dlxtds = oddr.GetInt16(2);
                                    jclczj = oddr.GetString(3);
                                    break;
                                case 2:
                                    gwfile = oddr.GetString(0);
                                    gwjg = oddr.GetInt16(1);
                                    gwtds = oddr.GetInt16(2);
                                    jclczj = oddr.GetString(3);
                                    break;
                            }
                        }
                        oddr.Close();
                    }
                    connection.Close();

                    return "从数据库中查找完数据！";
                }
                catch (Exception ex)
                {
                    return "从数据库中查找完数据,但是没有成功!,原因" + ex.Message;
                }
            }
        }
    }
}
