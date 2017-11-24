using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using InGraph.Classes;

namespace InGraph
{
    public partial class FindForm : Form
    {
        public FindForm()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {

                this.Tag = textBox1.Text;

            }
            catch (ArgumentException ae)
            {
                MessageBox.Show(ae.Message);
            }
            catch (FormatException fe)
            {
                MessageBox.Show(fe.Message);
            }
            catch (OverflowException ofe)
            {
                MessageBox.Show(ofe.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            button2_Click(sender, e);
        }

        private void FindForm_Load(object sender, EventArgs e)
        {

        }
        private void Textbox_Check(object sender, EventArgs e)
        {
            TextBox tb = (TextBox)sender;
            String tbStr = tb.Text;

            if (String.IsNullOrEmpty(tbStr))
            {
                MessageBox.Show("里程不能为空！");
                return;
            }
            else
            {
                try
                {
                    float mile = float.Parse(tbStr);
                    if (mile < 0)
                    {
                        MessageBox.Show("里程数必须大于或等于零！");
                        return;
                    }
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show("请输入数字！");
                    return;
                }

            }

        }
    }
}
