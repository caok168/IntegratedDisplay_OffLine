using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using InGraph.Classes;
using System.Windows.Forms;
using System.Xml;

namespace InGraph
{
    public partial class MeterageConfigForm : Form
    {
        public MeterageConfigForm()
        {
            InitializeComponent();
            MeterageRadiusTextBox.Text = CommonClass.MeterageRadius.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void MeterageConfigForm_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            CommonClass.MeterageRadius = int.Parse(MeterageRadiusTextBox.Text);
            //保存测量半径
            try
            {
                XmlDocument xd = new XmlDocument();
                xd.Load(CommonClass.AppConfigPath);
                xd.DocumentElement["MeterageRadius"].InnerText = CommonClass.MeterageRadius.ToString();
                xd.Save(CommonClass.AppConfigPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            this.Close();
        }

        private void MeterageRadiusTextBox_Leave(object sender, EventArgs e)
        {
            try
            {
                int i = int.Parse(MeterageRadiusTextBox.Text);
                if (i == 0)
                {
                    MeterageRadiusTextBox.Text = "1";
                }
            }
            catch
            {
                MeterageRadiusTextBox.Text = "1";
            }
        }
    }
}
