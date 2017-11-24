using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using InGraph.Classes;
using System.Text;
using System.Windows.Forms;
using System.IO;

using InvalidDataProcessing;

namespace InGraph
{
    /// <summary>
    /// 层平移---控件类
    /// </summary>
    public partial class LayerTranslationForm : Form
    {
        public LayerTranslationForm()
        {
            InitializeComponent();
        }

        private void HisCorrectForm_Load(object sender, EventArgs e)
        {
          
        }

        private void HistoryReviserForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();

        }

        private void LayerTranslationForm_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {

                ShowList();
            }
        }

        public void ShowList()
        {
            LayerDataGridView1.Rows.Clear();
            for (int i = 0; i < CommonClass.listDIC.Count; i++)
            {
                object[] o =new object[7];
                o[0] = (i + 1).ToString();
                o[1] = CommonClass.listDIC[i].sTrackName + CommonClass.listDIC[i].sDir;
                o[2] = CommonClass.listDIC[i].sDate;
                o[3] = CommonClass.listDIC[i].sRunDir;
                o[4] = (CommonClass.listDIC[i].listMeter[0].GetMeter(CommonClass.listDIC[0].bIndex ? 1 : CommonClass.listDIC[i].fScale[1]) / 1000.0).ToString();
                o[5] = CommonClass.listDIC[i].sKmInc;
                o[6]=CommonClass.listDIC[i].sTime;
                LayerDataGridView1.Rows.Add(o);
                 
            }

        }
        //里程对齐
        private void toolStripButton1_Click(object sender, EventArgs e)
        {

            try
            {
                if (LayerDataGridView1.SelectedRows.Count != 1)
                {
                    return;
                }
                CommonClass.AutoDataReviseValue(int.Parse(LayerDataGridView1.SelectedRows[0].Cells[0].Value.ToString())-1);
            }
            catch
            {

            }
        }

        //波形自动对齐
        private void AutoTranslation(object sender, EventArgs e)
        {
            MainForm.sMainform.AutoTranslation();
        }

        //重置偏移
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            try
            {
                if (LayerDataGridView1.SelectedRows.Count != 1)
                {
                    return;
                }
                int iLayerID=int.Parse(LayerDataGridView1.SelectedRows[0].Cells[0].Value.ToString()) - 1;
                CommonClass.listDIC[iLayerID].lReviseValue = 0;
                MessageBox.Show("设置成功!");
            }
            catch
            {

            }
        }



        #region timer_Tick事件--函数6个
        private void timer1_Tick(object sender, EventArgs e)
        {
            buttonTLeft1_Click(sender, e);
        }        

        private void timer2_Tick(object sender, EventArgs e)
        {
            buttonTLeftEX1_Click(sender, e);
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            buttonTLeftCU1_Click(sender, e);
        }

        private void timer4_Tick(object sender, EventArgs e)
        {
            buttonTRightCU1_Click(sender, e);
        }

        private void timer5_Tick(object sender, EventArgs e)
        {
            buttonTRightEX1_Click(sender, e);
        }

        private void timer6_Tick(object sender, EventArgs e)
        {
            buttonTRight1_Click(sender, e);
        }
        #endregion

        

        #region button_Click事件--函数6个
        private void buttonTLeft1_Click(object sender, EventArgs e)
        {
            if (LayerDataGridView1.SelectedRows.Count != 1)
            {
                return;
            }
            CommonClass.SetLayerDataReviseValue(int.Parse(LayerDataGridView1.SelectedRows[0].Cells[0].Value.ToString()) - 1, 1);
    
        }

        private void buttonTLeftEX1_Click(object sender, EventArgs e)
        {
            if (LayerDataGridView1.SelectedRows.Count != 1)
            {
                return;
            }
            CommonClass.SetLayerDataReviseValue(int.Parse(LayerDataGridView1.SelectedRows[0].Cells[0].Value.ToString()) - 1, 20);
        }

        private void buttonTLeftCU1_Click(object sender, EventArgs e)
        {
            if (LayerDataGridView1.SelectedRows.Count != 1)
            {
                return;
            }
            CommonClass.SetLayerDataReviseValue(int.Parse(LayerDataGridView1.SelectedRows[0].Cells[0].Value.ToString()) - 1, (long)numericUpDownReviseValue1.Value);
        }

        private void buttonTRight1_Click(object sender, EventArgs e)
        {
            if (LayerDataGridView1.SelectedRows.Count != 1)
            {
                return;
            }
            CommonClass.SetLayerDataReviseValue(int.Parse(LayerDataGridView1.SelectedRows[0].Cells[0].Value.ToString()) - 1, -1);
        }

        private void buttonTRightEX1_Click(object sender, EventArgs e)
        {
            if (LayerDataGridView1.SelectedRows.Count != 1)
            {
                return;
            }
            CommonClass.SetLayerDataReviseValue(int.Parse(LayerDataGridView1.SelectedRows[0].Cells[0].Value.ToString()) - 1, -20);
        }

        private void buttonTRightCU1_Click(object sender, EventArgs e)
        {
            if (LayerDataGridView1.SelectedRows.Count != 1)
            {
                return;
            }
            CommonClass.SetLayerDataReviseValue(int.Parse(LayerDataGridView1.SelectedRows[0].Cells[0].Value.ToString()) - 1, (-1)*(long)numericUpDownReviseValue1.Value);
        }
        #endregion


        #region button_MouseDown事件--函数6个
        private void buttonTLeft1_MouseDown(object sender, MouseEventArgs e)
        {
            timer1.Enabled = true;
        }

        private void buttonTLeftEX1_MouseDown(object sender, MouseEventArgs e)
        {
            timer2.Enabled = true;
        }

        private void buttonTLeftCU1_MouseDown(object sender, MouseEventArgs e)
        {
            timer3.Enabled = true;
        }

        private void buttonTRightCU1_MouseDown(object sender, MouseEventArgs e)
        {
            timer4.Enabled = true;
        }

        private void buttonTRightEX1_MouseDown(object sender, MouseEventArgs e)
        {
            timer5.Enabled = true;
        }

        private void buttonTRight1_MouseDown(object sender, MouseEventArgs e)
        {
            timer6.Enabled = true;
        }
        #endregion


        #region button_MouseUp事件--函数6个
        private void buttonTLeft1_MouseUp(object sender, MouseEventArgs e)
        {
            timer1.Enabled = false;
        }

        private void buttonTLeftEX1_MouseUp(object sender, MouseEventArgs e)
        {
            timer2.Enabled = false;
        }

        private void buttonTLeftCU1_MouseUp(object sender, MouseEventArgs e)
        {
            timer3.Enabled = false;
        }

        private void buttonTRightCU1_MouseUp(object sender, MouseEventArgs e)
        {
            timer4.Enabled = false;
        }

        private void buttonTRightEX1_MouseUp(object sender, MouseEventArgs e)
        {
            timer5.Enabled = false;
        }

        private void buttonTRight1_MouseUp(object sender, MouseEventArgs e)
        {
            timer6.Enabled = false;
        }
        #endregion


        private void LayerDataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (LayerDataGridView1.SelectedRows.Count > 0)
            {
                if (LayerDataGridView1.SelectedRows[0].Index == 0)
                {
                    toolStripButton1.Enabled = false;
                    toolStripButton2.Enabled = false;
                    buttonTLeft1.Enabled = false;
                    buttonTLeftEX1.Enabled = false;
                    buttonTLeftCU1.Enabled = false;
                    numericUpDownReviseValue1.Enabled = false;
                    buttonTRightCU1.Enabled = false;
                    buttonTRightEX1.Enabled = false;
                    buttonTRight1.Enabled = false;
                }
                else
                {
                    toolStripButton1.Enabled = true;
                    toolStripButton2.Enabled = true;
                    buttonTLeft1.Enabled = true;
                    buttonTLeftEX1.Enabled = true;
                    buttonTLeftCU1.Enabled = true;
                    numericUpDownReviseValue1.Enabled = true;
                    buttonTRightCU1.Enabled = true;
                    buttonTRightEX1.Enabled = true;
                    buttonTRight1.Enabled = true;
                }
            }
        }


    }
}
