using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using InGraph.Classes;
using DataProcess.CalcSpace;
using DataProcess;

namespace InGraph.Forms
{
    /// <summary>
    /// 利用相关性修正波形里程---控件类
    /// </summary>
    public partial class CorrelationForm : Form
    {
        public CorrelationForm()
        {
            InitializeComponent();
        }

        #region 事件响应函数--利用相关性修正波形里程
        /// <summary>
        /// 事件响应函数--利用相关性修正波形里程--输出的是被修正的cit文件的索引idf文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonWaveFix_Click(object sender, EventArgs e)
        {
            Boolean args_0 = Textbox_Check(textBox超高门阀值);
            Boolean args_1 = Textbox_Check(textBox轨距门阀值);
            Boolean args_2 = Textbox_Check(textBox左高低门阀值);
            Boolean args_3 = Textbox_Check(textBox右高低门阀值);
            Boolean args_4 = Textbox_Check(textBox原始数据点);
            Boolean args_5 = Textbox_Check(textBox目标数据点);

            if (!(args_5 && args_4 && args_3 && args_2 && args_1 && args_0))
            {
                return ;
            }
            
            this.Enabled = false;
            Application.DoEvents();
            //CalcProcess cp = new CalcProcess();
            ////处理
            //for (int i = 0; i < listBox1.Items.Count; i++)
            //{
            //    //被修正的原始文件--cit
            //    String citFilePath = listBox1.Items[i].ToString();
            //    CITDataProcess.DataHeadInfo dhi = CommonClass.cdp.GetDataInfoHead(citFilePath);

            //    //根据相关性，修正cit文件的索引
            //    List<IndexOriClass> listIOC = cp.CorrelationCalc(CommonClass.listDIC[0].sFilePath, citFilePath,
            //        CommonClass.wdp.GetLayerIndexInfo(CommonClass.listDIC[0].sAddFile, "0", CommonClass.listDIC[0].sKmInc),
            //        CommonClass.listDIC[0].iSmaleRate, CommonClass.listDIC[0].iChannelNumber, CommonClass.listDIC[0].bEncrypt, CommonClass.listDIC[0].sKmInc
            //        , float.Parse(textBox超高门阀值.Text), float.Parse(textBox轨距门阀值.Text), float.Parse(textBox左高低门阀值.Text)
            //        , float.Parse(textBox右高低门阀值.Text), int.Parse(textBox原始数据点.Text), int.Parse(textBox目标数据点.Text), dhi.iChannelNumber);

            //    if (listIOC.Count > 0)
            //    {
            //        CommonClass.wdp.CreateDB(citFilePath.Replace(".cit", ".idf"));
            //        //删除
            //        CommonClass.wdp.deleteLayerIndexInfo(citFilePath.Replace(".cit", ".idf"));
            //        //重新保存索引库
            //        for (int j = 0; j < listIOC.Count; j++)
            //        {
            //            CommonClass.wdp.InsertLayerAllIndexInfo(citFilePath.Replace(".cit", ".idf"),
            //                listIOC[j].iId.ToString()
            //                , listIOC[j].iIndexId.ToString(),
            //                listIOC[j].IndexPoint.ToString(),
            //                listIOC[j].IndexMeter.ToString());
            //        }

            //        //创建计算后的索引库
            //        List<CDLClass> listCDL = CommonClass.GetCDL();
            //        //根据标定的里程特征点信息创建索引文件idf
            //        CommonClass.wdp.CreateIndexInfo(citFilePath, citFilePath.Replace(".cit", ".idf"),
            //            "1", listCDL, CommonClass.listDIC[0].sKmInc, dhi.iChannelNumber, CommonClass.listDIC[0].iSmaleRate);
            //    }
            //    else
            //    {
            //        MessageBox.Show("CommonClass.listDIC[0]的文件索引为空，没有执行修正！");
            //        this.Enabled = true;
            //        Application.DoEvents();
            //        return;
            //    }
            //}

            //this.Enabled = true;
            //Application.DoEvents();
            //MessageBox.Show("修正成功！");

            backgroundWorker1.RunWorkerAsync();
        }
        #endregion

        private void buttonClear_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
        }

        private void buttonFileAdd_Click(object sender, EventArgs e)
        {
            openFileDialog1.FileName = "";
            DialogResult dr = openFileDialog1.ShowDialog();
            if (dr == DialogResult.OK)
            {
                listBox1.Items.Add(openFileDialog1.FileName);
            }
        }

        private void buttonBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonFileDelete_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItems.Count == 1)
            {
                listBox1.Items.Remove(listBox1.SelectedItem);
                listBox1.Invalidate(); ;
            }
        }

        private Boolean Textbox_Check(TextBox tb)
        {
            String tbStr = tb.Text;

            if (String.IsNullOrEmpty(tbStr))
            {
                MessageBox.Show("参数不能为空！");
                return false;
            }
            else
            {
                try
                {
                    float mile = float.Parse(tbStr);
                    if (mile < 0)
                    {
                        MessageBox.Show("参数必须大于零！");
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

        private void Process()
        {
            CalcProcess cp = new CalcProcess();
            //处理
            for (int i = 0; i < listBox1.Items.Count; i++)
            {
                //被修正的原始文件--cit
                String citFilePath = listBox1.Items[i].ToString();
                CITDataProcess.DataHeadInfo dhi = CommonClass.cdp.GetDataInfoHead(citFilePath);

                //根据相关性，修正cit文件的索引
                List<IndexOriClass> listIOC = cp.CorrelationCalc(CommonClass.listDIC[0].sFilePath, citFilePath,
                    CommonClass.wdp.GetLayerIndexInfo(CommonClass.listDIC[0].sAddFile, "0", CommonClass.listDIC[0].sKmInc),
                    CommonClass.listDIC[0].iSmaleRate, CommonClass.listDIC[0].iChannelNumber, CommonClass.listDIC[0].bEncrypt, CommonClass.listDIC[0].sKmInc
                    , float.Parse(textBox超高门阀值.Text), float.Parse(textBox轨距门阀值.Text), float.Parse(textBox左高低门阀值.Text)
                    , float.Parse(textBox右高低门阀值.Text), int.Parse(textBox原始数据点.Text), int.Parse(textBox目标数据点.Text), dhi.iChannelNumber);

                if (listIOC.Count > 0)
                {
                    String idfFilePath = citFilePath.Replace(".cit", ".idf");
                    CommonClass.wdp.CreateDB(idfFilePath);
                    CommonClass.wdp.CreateTable(idfFilePath);
                    //删除
                    CommonClass.wdp.deleteLayerIndexInfo(idfFilePath);
                    //重新保存索引库
                    for (int j = 0; j < listIOC.Count; j++)
                    {
                        CommonClass.wdp.InsertLayerAllIndexInfo(idfFilePath,
                            listIOC[j].iId.ToString()
                            , listIOC[j].iIndexId.ToString(),
                            listIOC[j].IndexPoint.ToString(),
                            listIOC[j].IndexMeter.ToString());
                    }

                    //创建计算后的索引库
                    List<CDLClass> listCDL = CommonClass.GetCDL();
                    //根据标定的里程特征点信息创建索引文件idf
                    CommonClass.wdp.CreateIndexInfo(citFilePath, idfFilePath,
                        "1", listCDL, CommonClass.listDIC[0].sKmInc, dhi.iChannelNumber, CommonClass.listDIC[0].iSmaleRate);
                }
                else
                {
                    MessageBox.Show("CommonClass.listDIC[0]的文件索引为空，没有执行修正！");
                    this.Enabled = true;
                    Application.DoEvents();
                    return;
                }
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            Process();
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Enabled = true;
            Application.DoEvents();
            MessageBox.Show("修正成功！");
        }

    }
}
