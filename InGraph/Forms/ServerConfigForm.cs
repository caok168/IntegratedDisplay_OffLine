using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.IO;
using System.Windows.Forms;
using InGraph.Classes;
using System.Xml;
namespace InGraph
{
    public partial class ServerConfigForm : Form
    {
        public ServerConfigForm()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ServerConfigForm_Load(object sender, EventArgs e)
        {
            ServerIPTextBox.Text = CommonClass.ServerAddress;
            ServerPortTextBox.Text = CommonClass.ServerPort.ToString();
        

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //保存服务器地址
            try
            {
                CommonClass.ServerAddress = ServerIPTextBox.Text;
                CommonClass.ServerPort = int.Parse(ServerPortTextBox.Text);
                XmlDocument xd = new XmlDocument();
                xd.Load(CommonClass.AppConfigPath);
                xd.DocumentElement["ServerConfig"].Attributes["server"].Value = CommonClass.ServerAddress;
                xd.DocumentElement["ServerConfig"].Attributes["port"].Value = CommonClass.ServerPort.ToString();
                xd.Save(CommonClass.AppConfigPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            this.Close();
        }

    }
}
