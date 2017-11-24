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
    public partial class LicenseForm : Form
    {
        public LicenseForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void LicenseForm_Load(object sender, EventArgs e)
        {
            textBox1.Text = CommonClass.sLicense;
            textBox3.Text = "2012年12月31日";

        }
    }
}
