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
    public partial class ChannelsConfigDialog : Form
    {
        List<int> l;
        int iLayerID = -1;
        public ChannelsConfigDialog()
        {
            InitializeComponent();
        }

        private void CancelButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void SaveButton1_Click(object sender, EventArgs e)
        {
            //依次批量设置内容
            if (checkBox1.Checked)
            {
                foreach (int i in l)
                {
                    CommonClass.listDIC[iLayerID].listCC[i].ChineseName = comboBox1.Text;
                }
            }
            if (checkBox2.Checked)
            {
                foreach (int i in l)
                {
                    CommonClass.listDIC[iLayerID].listCC[i].NonChineseName = comboBox2.Text;
                }
            }
            if (checkBox4.Checked)
            {
                foreach (int i in l)
                {
                    CommonClass.listDIC[iLayerID].listCC[i].ZoomIn = float.Parse(comboBox3.Text);
                }
            }
            if (checkBox5.Checked)
            {
                foreach (int i in l)
                {
                    CommonClass.listDIC[iLayerID].listCC[i].Location = int.Parse(comboBox4.Text);
                }
            }
            if (checkBox7.Checked)
            {
                foreach (int i in l)
                {
                    CommonClass.listDIC[iLayerID].listCC[i].fLineWidth = float.Parse(comboBox6.Text);
                }
            }
 
            if (checkBox3.Checked)
            {
                foreach (int i in l)
                {
                    CommonClass.listDIC[iLayerID].listCC[i].Color=pictureBox1.BackColor.ToArgb();
                }
            }

            if (checkBox6.Checked)
            {
                foreach (int i in l)
                {
                    CommonClass.listDIC[iLayerID].listCC[i].MeaOffset = checkBox9.Checked;
                }
            }

            if (checkBox8.Checked)
            {
                foreach (int i in l)
                {
                    CommonClass.listDIC[iLayerID].listCC[i].Visible = checkBox10.Checked;
                }
            }
            CancelButton1_Click(sender, e);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.BackColor = colorDialog1.Color;
            }
            
        }

        private void ChannelsConfigDialog_Load(object sender, EventArgs e)
        {
            l = new List<int>();
            string sPar=this.Tag.ToString();
            string[] sSplit=sPar.Split(new char[] { ',' });
            iLayerID = int.Parse(sSplit[0]);
            for (int i = 1; i < sSplit.Length; i++)
            {
                l.Add(int.Parse(sSplit[i]));
                comboBox1.Items.Add(CommonClass.listDIC[iLayerID].listCC[int.Parse(sSplit[i])].ChineseName);
                comboBox2.Items.Add(CommonClass.listDIC[iLayerID].listCC[int.Parse(sSplit[i])].NonChineseName);
                comboBox3.Items.Add(CommonClass.listDIC[iLayerID].listCC[int.Parse(sSplit[i])].ZoomIn);
                comboBox4.Items.Add(CommonClass.listDIC[iLayerID].listCC[int.Parse(sSplit[i])].Location);
                comboBox6.Items.Add(CommonClass.listDIC[iLayerID].listCC[int.Parse(sSplit[i])].fLineWidth);

            }
        }

        private void checkBox8_CheckedChanged(object sender, EventArgs e)
        {
            GetCheckBoxState();
        }
        private void GetCheckBoxState()
        {
            bool b = false;
            for (int i = 1; i < 9; i++)
            {
                if (((CheckBox)this.Controls["checkBox" + i.ToString()]).Checked)
                {
                    b = true;
                    break;
                }
            }
            SaveButton1.Enabled = b;
        }
    }
}
