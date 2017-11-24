using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace InGraph.Forms
{
    public partial class TeeChartConfigForm : Form
    {
        public TeeChartConfigForm()
        {
            InitializeComponent();
        }

        public void InitData(object o)
        {
            TeeChartConfigClass tccc = (TeeChartConfigClass)o;

            textBoxTitle.Text = tccc.teeChartTitle;
            labelFontTitle.Text = tccc.teeChartTitleFont.Name + "," + tccc.teeChartTitleFont.Size;

            textBoxAxesLeft.Text = tccc.axesNameLeft;
            labelFontAxesLeft.Text = tccc.axesFontLeft.Name + "," + tccc.axesFontLeft.Size;
            numericUpDownAxesLeftMax.Value = (decimal)(tccc.axesMaxLeft);
            numericUpDownAxesLeftMin.Value = (decimal)(tccc.axesMinLeft);


            textBoxAxesBottom.Text = tccc.axesNameBottom;
            labelFontAxesBottom.Text = tccc.axesFontBottom.Name + "," + tccc.axesFontBottom.Size;
            numericUpDownAxesBottomMax.Value = (decimal)(tccc.axesMaxBottom);
            numericUpDownAxesBottomMin.Value = (decimal)(tccc.axesMinBottom);

        }

        private Font GetFont(String fontLabel)
        {
            Font font = null;
            if (!String.IsNullOrEmpty(fontLabel))
            {
                String[] str = fontLabel.Split(',');
                font = new Font(str[0], float.Parse(str[1]));
            }

            return font;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult dr = fontDialog1.ShowDialog();

            labelFontTitle.Text = fontDialog1.Font.Name + "," + fontDialog1.Font.SizeInPoints;
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            TeeChartConfigClass tc = new TeeChartConfigClass();
            tc.teeChartTitle = textBoxTitle.Text;
            tc.teeChartTitleFont = GetFont(labelFontTitle.Text);

            tc.axesNameLeft = textBoxAxesLeft.Text;
            tc.axesFontLeft = GetFont(labelFontAxesLeft.Text);
            tc.axesMaxLeft = (int)(numericUpDownAxesLeftMax.Value);
            tc.axesMinLeft = (int)(numericUpDownAxesLeftMin.Value);

            tc.axesNameBottom = textBoxAxesBottom.Text;
            tc.axesFontBottom = GetFont(labelFontAxesBottom.Text);
            tc.axesMaxBottom = (int)(numericUpDownAxesBottomMax.Value);
            tc.axesMinBottom = (int)(numericUpDownAxesBottomMin.Value);

            this.Tag = tc;

            this.Close();
        }

        private void buttonAxesLeft_Click(object sender, EventArgs e)
        {
            DialogResult dr = fontDialog1.ShowDialog();

            labelFontAxesLeft.Text = fontDialog1.Font.Name + "," + fontDialog1.Font.SizeInPoints;
        }

        private void buttonAxesBottom_Click(object sender, EventArgs e)
        {
            DialogResult dr = fontDialog1.ShowDialog();

            labelFontAxesBottom.Text = fontDialog1.Font.Name + "," + fontDialog1.Font.SizeInPoints;
        }

    }

    public class TeeChartConfigClass
    {
        public String teeChartTitle;
        public Font teeChartTitleFont;

        public String axesNameLeft;
        public Font axesFontLeft;
        public int axesMaxLeft;
        public int axesMinLeft;
        //public Boolean axesLeftAuto;

        public String axesNameBottom;
        public Font axesFontBottom;
        public int axesMaxBottom;
        public int axesMinBottom;
        //public Boolean axesBottomAuto;
    }
}
