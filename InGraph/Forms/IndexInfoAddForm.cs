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
    /// <summary>
    /// 索引标定显示框
    /// </summary>
    public partial class IndexMainInfoAddForm : Form
    {
        #region 构造函数
        /// <summary>
        /// 初始化索引标定显示框中的文件指针记录号和文件原始里程文本框
        /// </summary>
        /// <param name="lPostion">文件指针记录号</param>
        /// <param name="iKM">公里标(单位为公里)</param>
        /// <param name="iMeter">偏移量(单位为米)</param>
        public IndexMainInfoAddForm(long lPostion,int iKM,float iMeter)
        {
            InitializeComponent();
            textBox1.Text = lPostion.ToString();
            textBox2.Text = (iKM+(iMeter/1000.0f)).ToString();
            textBox3.Text = CommonClass.sLastSelectText;
            label4.Text = "KM" + iKM.ToString() + "+" + iMeter.ToString();
        }
        #endregion

        #region 事件响应函数
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void IndexInfoAddForm_Load(object sender, EventArgs e)
        {

        }

        #region 保存按钮的Click时间响应函数
        /// <summary>
        /// 保存按钮的Click时间响应函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            Boolean retVal = Textbox_Check(textBox3);
            if (!retVal)
            {
                return;
            }

            if (textBox3.Text.Length < 1)
            {
                button2_Click(sender,e);
            }
            this.Tag = textBox3.Text;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
        #endregion

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
        #endregion
    }
}
