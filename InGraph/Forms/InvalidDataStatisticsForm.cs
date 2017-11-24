using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace InGraph.Forms
{
    public partial class InvalidDataStatisticsForm : Form
    {
        //private List<StatisticsDataClass> dataList;
        public InvalidDataStatisticsForm(List<StatisticsDataClass> dataList)
        {
            InitializeComponent();

            InitListview(dataList);
        }

        private void InitListview(List<StatisticsDataClass> dataList)
        {
            if (dataList == null || dataList.Count == 0)
            {
                return;
            }
            foreach (StatisticsDataClass cls in dataList)
            {
                ListViewItem lvi = new ListViewItem();
                lvi.Text = cls.reasonType;
                //lvi.SubItems.Add(cls.reasonType);
                lvi.SubItems.Add(cls.sumcount.ToString());
                lvi.SubItems.Add(cls.countPercent);
                lvi.SubItems.Add(cls.gongliPercent);

                listView1.Items.Add(lvi);
            }
        }
    }
}
