using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using InGraph.Classes;
using System.IO;
using DataProcess;

namespace InGraph.Forms
{
    /// <summary>
    /// 精确导出---控件类
    /// </summary>
    public partial class ExportIndexDataForm : Form
    {
        private Boolean isKmInc = true;//增里程
        public ExportIndexDataForm()
        {
            InitializeComponent();
        }

        private void ExportIndexDataForm_Load(object sender, EventArgs e)
        {
            if (CommonClass.listDIC.Count == 0)
            {
                MessageBox.Show("请打开一个波形文件");
                return;
            }
            else if (CommonClass.listDIC.Count > 1)
            {
                MessageBox.Show("请不要在打开多个波形文件时执行此功能！");
                return;
            } 
            else
            {
                if (CommonClass.listDIC[0].listIC.Count > 0 && CommonClass.listDIC[0].bIndex)
                {
                    StartMileTextBox.Text = CommonClass.listDIC[0].listIC[0].lStartMeter.ToString();
                    EndMileTextBox.Text = CommonClass.listDIC[0].listIC[CommonClass.listDIC[0].listIC.Count - 1].LEndMeter.ToString();
                }
                else
                {
                    //后去没有索引过后
                    string sLength = CommonClass.cdp.QueryDataMileageRange(CommonClass.listDIC[0].sFilePath, false, CommonClass.listDIC[0].bEncrypt);
                    string[] sSE = sLength.Split(',');
                    string[] sSE1 = sSE[1].Split('-');
                    StartMileTextBox.Text = sSE1[0].Trim();
                    EndMileTextBox.Text = sSE1[1].Trim();
                }

                float startMile = float.Parse(StartMileTextBox.Text);
                float endMile = float.Parse(EndMileTextBox.Text);
                if (startMile > endMile)
                {
                    isKmInc = false;//减里程
                }
            }

            string sChannel = CommonClass.cdp.QueryDataChannelInfoHead(CommonClass.listDIC[0].sFilePath);
            string[] sSEChannel = sChannel.Split(',');
            for (int i = 1; i < sSEChannel.Length; i++)
            {
                checkedListBox1.Items.Add(sSEChannel[i]);
            }
        }

        #region 事件响应函数---精确数据导出
        private void ExportButton_Click(object sender, EventArgs e)
        {
            //处理
            StringBuilder sb1 = new StringBuilder();
            StringBuilder sb2 = new StringBuilder();
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                if (checkedListBox1.GetItemChecked(i))
                {
                    sb1.Append(i.ToString() + ",");
                    sb2.Append(checkedListBox1.Items[i].ToString() + ",");
                }

            }
            
            if (sb1.Length == 0)
            {
                MessageBox.Show("请选择需要导出的通道！");
                return;
            }

            float startMile = 0; ;
            float endMile = 0;

            try
            {
                startMile = float.Parse(StartMileTextBox.Text);
                endMile = float.Parse(EndMileTextBox.Text);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("请输入数字！");
                return;
            }

            if (startMile < 0 || endMile < 0)
            {
                MessageBox.Show("起始里程和结束里程不能为负值！");
                return;
            }

            //if (isKmInc == true)
            //{
            //    if (startMile >= endMile)
            //    {
            //        MessageBox.Show("增里程时，起始里程必须小于结束里程！");
            //        return;
            //    }
            //} 
            //else
            //{
            //    if (startMile <= endMile)
            //    {
            //        MessageBox.Show("减里程时，起始里程必须大于结束里程！");
            //        return;
            //    }
            //}

            if (!PWDTextBox.Text.Equals("19491001"))
            {
                MessageBox.Show("请输入正确的密码!");
                return;
            }

            string[] sPara = new string[2];
            sPara[0] = sb1.ToString().TrimEnd(',');
            sPara[1] = sb2.ToString().TrimEnd(',');
            Application.DoEvents();
            if (CommonClass.listDIC[0].bIndex)
            {
                CommonClass.wdp.ExportIndexDataOld(CommonClass.listDIC[0].sFilePath, CommonClass.listDIC[0].iChannelNumber,
                  CommonClass.listDIC[0].sKmInc, CommonClass.listDIC[0].bEncrypt, float.Parse(StartMileTextBox.Text),
                        float.Parse(EndMileTextBox.Text), sPara);
            }
            else
            {
                CommonClass.cdp.ExportData(CommonClass.listDIC[0].sFilePath, CommonClass.listDIC[0].sFilePath + "_" +
                    StartMileTextBox.Text + "-" + EndMileTextBox.Text
                    + ".csv"
                        , (int)(float.Parse(StartMileTextBox.Text) * 1000),
                        (int)(float.Parse(EndMileTextBox.Text) * 1000), sPara, new List<AreaClass>(), CommonClass.listDIC[0].bEncrypt);
            }
            MessageBox.Show("导出成功！");
        }
        #endregion

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
