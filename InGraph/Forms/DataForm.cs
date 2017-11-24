using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Text;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace InGraph
{
    public partial class DataForm : Form
    {
        public DataForm()
        {
            InitializeComponent();
        }

        private void Cancel_Button_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void DataForm_Load(object sender, EventArgs e)
        {
            string[] sp1 = this.Tag.ToString().Split('`');
            foreach (string sp2 in sp1)
            {
                string[] s = sp2.Split('~');
                DataListView.Items.Add(new ListViewItem(new string[] { s[0], s[1], s[2], s[3], s[4],s[5] }));

            }
          
        }

        private void Open_Button_Click(object sender, EventArgs e)
        {
            if (DataListView.SelectedItems.Count != 0)
            {
                this.Tag = "";
                this.Tag = DataListView.SelectedItems[0].SubItems[0].Text + "`" + DataListView.SelectedItems[0].SubItems[1].Text + "`" + DataListView.SelectedItems[0].SubItems[2].Text + "`" + DataListView.SelectedItems[0].SubItems[3].Text + "`" + DataListView.SelectedItems[0].SubItems[4].Text + "`" + DataListView.SelectedItems[0].SubItems[5].Text;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void DataListView_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Open_Button_Click(sender, e);
        }
    }
}
