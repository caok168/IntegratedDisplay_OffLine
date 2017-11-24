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
    public partial class LayerForm : Form
    {
        public LayerForm()
        {
            InitializeComponent();
        }

        private void LayerForm_Load(object sender, EventArgs e)
        {
            LayerConfigDataGridView1.Rows.Clear();
            for (int i = 0; i < CommonClass.listDIC.Count; i++)
            {
                object[] o = new object[6];
                o[0] = CommonClass.listDIC[i].Name;
                o[1] = CommonClass.listDIC[i].bVisible;
                o[2] = CommonClass.listDIC[i].bMeterInfoVisible;
                o[3] = CommonClass.listDIC[i].bChannelsLabel;
                o[4] = CommonClass.listDIC[i].bLabel;
                o[5] = CommonClass.listDIC[i].bReverse;
                LayerConfigDataGridView1.Rows.Add(o);
            }

            if (LayerConfigDataGridView1.Rows.Count > 0)
            {
                LayerConfigDataGridView1.Rows[0].ReadOnly = true;
            }
        }

        private void SaveButton1_Click(object sender, EventArgs e)
        {
            if (LayerConfigDataGridView1.Rows.Count < 1)
            {
                return;
            }
            //为了使底层文件也可以反转，将下面的i=1，改为i=0
            for (int i = 1; i < CommonClass.listDIC.Count; i++)
            {
                CommonClass.listDIC[i].bVisible = bool.Parse(LayerConfigDataGridView1.Rows[i].Cells[1].Value.ToString());
                CommonClass.listDIC[i].bMeterInfoVisible = bool.Parse(LayerConfigDataGridView1.Rows[i].Cells[2].Value.ToString());
                CommonClass.listDIC[i].bChannelsLabel = bool.Parse(LayerConfigDataGridView1.Rows[i].Cells[3].Value.ToString());
                CommonClass.listDIC[i].bLabel = bool.Parse(LayerConfigDataGridView1.Rows[i].Cells[4].Value.ToString());
                CommonClass.listDIC[i].bReverse = bool.Parse(LayerConfigDataGridView1.Rows[i].Cells[5].Value.ToString());
            }
            this.Close();
        }

        private void CloseLayerButton1_Click(object sender, EventArgs e)
        {
            if (LayerConfigDataGridView1.Rows.Count < 1)
            {
                return;
            }

            for (int i = 1; i < LayerConfigDataGridView1.Rows.Count; i++)
            {
                if (LayerConfigDataGridView1.Rows[i].Selected)
                {
                    CommonClass.listDIC.RemoveAt(i);
                    break;
                }
            }
            this.Close();
        }
        private void LayerForm_Resize(object sender, EventArgs e)
        {
            try
            {
                LayerConfigDataGridView1.Height = this.ClientSize.Height - 100;
            }
            catch
            {
                this.Height = 200;
            }
        }
    }
}
