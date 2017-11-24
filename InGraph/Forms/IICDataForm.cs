using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using InGraph.Classes;

namespace InGraph.Forms
{
    delegate void SetEnabledCallbackThis(bool value);
    public partial class IICDataForm : Form
    {
        public IICDataForm()
        {
            InitializeComponent();
        }
        private void SetEnabledThis(bool value)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    SetEnabledCallbackThis d = new SetEnabledCallbackThis(SetEnabledThis);
                    this.Invoke(d, new object[] { value });
                }
                else
                {
                    this.Enabled = value;
                }
            }
            catch
            {
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.FileName = "";
            DialogResult dr= openFileDialog1.ShowDialog();
            if (dr == DialogResult.OK)
            {
                listBox1.Items.Add(openFileDialog1.FileName);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Enabled = false;
            Application.DoEvents();
            backgroundWorker1.RunWorkerAsync();


        }

        private void IICDataForm_Load(object sender, EventArgs e)
        {
            try
            {
                textBox1.Text = CommonClass.listDIC[0].sFilePath;
            }
            catch
            {

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItems.Count == 1)
            {
                listBox1.Items.Remove(listBox1.SelectedItem);
                listBox1.Invalidate(); ;
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            //创建所需要的表结构
            if (checkBox1.Checked)
            {
                try
                {
                    for (int i = 0; i < listBox1.Items.Count; i++)
                    {
                        CommonClass.wdp.CreateIICTable(listBox1.Items[i].ToString());
                    }
                }
                catch
                {

                }
            }
            //主层文件没有加载索引，弹出提示框--ygx20131230
            //if (!CommonClass.listDIC[0].bIndex)
            //{
            //    MessageBox.Show("主层文件没有加载索引文件！");
            //    SetEnabledThis(true);
            //    Application.DoEvents();

            //    return;
            //}
            //复制TQI---里程未校正
            //if (CommonClass.listDIC[0].listIC == null || CommonClass.listDIC[0].listIC.Count == 0)
            //{
            //    for (int i = 0; i < listBox1.Items.Count;i++ )
            //    {
            //        CommonClass.wdp.TQICopy(listBox1.Items[i].ToString(), CommonClass.listDIC[0].sKmInc, CommonClass.listDIC[0].listIDC);
            //    }

            //    MessageBox.Show("TQI拷贝成功！");
            //    SetEnabledThis(true);
            //    Application.DoEvents();

            //    return;
            //}
            //处理修正--里程校正下
            for (int i = 0; i < listBox1.Items.Count; i++)
            {

                CommonClass.wdp.ExceptionFix(CommonClass.listDIC[0].sFilePath, listBox1.Items[i].ToString(), CommonClass.listDIC[0].listIC,
                    CommonClass.listDIC[0].iSmaleRate, CommonClass.listDIC[0].iChannelNumber, CommonClass.listDIC[0].bEncrypt, CommonClass.listDIC[0].sKmInc, CommonClass.listETC);

                CommonClass.wdp.TQIFix(CommonClass.listDIC[0].sFilePath, listBox1.Items[i].ToString(), CommonClass.listDIC[0].listIC,
                    CommonClass.listDIC[0].iSmaleRate, CommonClass.listDIC[0].iChannelNumber, CommonClass.listDIC[0].bEncrypt, CommonClass.listDIC[0].sKmInc, CommonClass.listDIC[0].listIDC, CommonClass.listDIC[0].sTrain);

            }
            MessageBox.Show("修正成功！");
            SetEnabledThis(true);
            Application.DoEvents();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
