using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using InGraph.Classes;
using System.Globalization;
using System.Xml;
using System.IO;
using Microsoft.VisualBasic;
using InGraph.Forms;


namespace InGraph
{
    /// <summary>
    /// 波形数据图层通道设置控件类
    /// </summary>
    public partial class ChannelsDialog : Form
    {
        private int iLayerID = 0;
        private int iChannlesID = 0;
        //private int iLastSelectIndex = -1;
        /// <summary>
        /// 初始化设定
        /// </summary>
        public ChannelsDialog()
        {
            InitializeComponent();
           
        }

        #region 内部函数----没有使用
        private void OKButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ColorChoiceButton_Click(object sender, EventArgs e)
        {
            DialogResult dr = ColorChoiceDialog.ShowDialog();
        }
        #endregion



        #region 事件响应函数---Combobox的SelectedIndex发生改变时发生
        /// <summary>
        /// 事件响应函数---Combobox的SelectedIndex发生改变时发生
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LayerComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetChannelsData();
        }
        #endregion

        #region 事件响应函数---控件加载
        /// <summary>
        /// 事件响应函数---控件加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChannelsConfigDialog_Load(object sender, EventArgs e)
        {
            SaveButton1.Enabled = false;
            for (int i = 0; i < ChannelsConfigDataGridView1.Columns.Count; i++)
            {
                ChannelsConfigDataGridView1.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            ChannelsConfigDataGridView1.ClearSelection();
            if (ChannelsConfigDataGridView1.Rows.Count > 0)
            { ChannelsConfigDataGridView1.Rows[iChannlesID].Selected = true; }
            if (LayerComboBox1.Items.Count > 0)
            {
                SaveAsButton1.Enabled = true;
                OpenButton1.Enabled = true;
            }
            ChannelsDialog_Resize(sender, e);
        }
        #endregion

        #region 事件响应函数---双击单元格中的任意位置时发生
        /// <summary>
        /// 事件响应函数---双击单元格中的任意位置时发生
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChannelsConfigDataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.ColumnIndex==5 && e.RowIndex>-1)
            {
                ColorChoiceDialog.Color=
                    ChannelsConfigDataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor;
                DialogResult dr= ColorChoiceDialog.ShowDialog();
                if (dr == DialogResult.OK)
                {
                    ChannelsConfigDataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor =
                       ColorChoiceDialog.Color;
                    //Color.FromArgb(CommonClass.listDIC[LayerComboBox1.SelectedIndex].listCC[i].Color);
                    ChannelsConfigDataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = "";
                }
                
            }
        }
        #endregion

        #region 事件响应函数---单元格的值改变时发生
        /// <summary>
        /// 事件响应函数---单元格的值改变时发生
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChannelsConfigDataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            SaveButton1.Enabled = true;
        }
        #endregion

        #region 事件响应函数---单击单元格中的任意位置时发生
        /// <summary>
        /// 事件响应函数---单击单元格中的任意位置时发生
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChannelsConfigDataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && ChannelsConfigDataGridView1.SelectedRows.Count > 0)
            {
                BatchModify();
            }
        }
        #endregion
        private void BatchModify()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(LayerComboBox1.SelectedIndex.ToString());
            sb.Append(",");
            foreach (DataGridViewRow dgvr in ChannelsConfigDataGridView1.SelectedRows)
            {
                sb.Append(dgvr.Index.ToString());
                sb.Append(",");
            }
            sb.Remove(sb.Length - 1, 1);
            using (ChannelsConfigDialog ccd = new ChannelsConfigDialog())
            {
                ccd.Tag = sb.ToString();
                DialogResult dr = ccd.ShowDialog();
                if (dr == DialogResult.OK)
                {
                    GetChannelsData();
                    SaveButton1.Enabled = true;
                }

            }
        }

        #region 事件响应函数---改变控件可见性时发生
        /// <summary>
        /// 事件响应函数---改变控件可见性时发生
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChannelsDialog_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                Point p = (Point)this.Tag;
                //初始化数据设定
                LayerComboBox1.Items.Clear();
                for (int i = 0; i < CommonClass.listDIC.Count; i++)
                {
                    LayerComboBox1.Items.Add(CommonClass.listDIC[i].Name);
                }
                iLayerID = p.X;
                iChannlesID = p.Y;
                if (LayerComboBox1.Items.Count > 0)
                {
                    LayerComboBox1.SelectedIndex = iLayerID;
                    //iLastSelectIndex = 0;
                    SaveAsButton1.Enabled = true;
                    OpenButton1.Enabled = true;
                    if (ChannelsConfigDataGridView1.Rows.Count > iChannlesID)
                    { ChannelsConfigDataGridView1.Rows[iChannlesID].Selected = true; }
                }
            }
        }
        #endregion

        #region 调整控件大小响应事件
        /// <summary>
        /// 调整控件大小响应事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChannelsDialog_Resize(object sender, EventArgs e)
        {
            ChannelsConfigDataGridView1.Location = new Point(3, 58);
            ChannelsConfigDataGridView1.Height = this.ClientSize.Height - 100;
            ChannelsConfigDataGridView1.Width = this.ClientSize.Width - 3;


            ConfigLabel.Width = this.ClientSize.Width - ConfigLabel.Left - 20;
            LayerComboBox1.Width = this.ClientSize.Width - LayerComboBox1.Left - 20;
        }
        #endregion

        #region 控件关闭事件
        /// <summary>
        /// 控件关闭事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChannelsDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Tag = new Point();
            this.Hide();
        }
        #endregion





        #region 按钮单击响应事件---应用保存
        /// <summary>
        /// 按钮单击响应事件---应用保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion

        #region 按钮单击响应事件---应用保存
        /// <summary>
        /// 按钮单击响应事件---应用保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveButton1_Click(object sender, EventArgs e)
        {
            SaveChannelSetToConfigFile();

            SaveButton1.Enabled = false;
            MainForm.sMainform.MainGraphicsPictureBox1.Invalidate();
        }
        #endregion

        #region 把通道设置保存在所选层的系统配置变量ListCC中，然后把系统配置变量写入到所选层的配置文件中。
        /// <summary>
        /// 把通道设置保存在所选层的系统配置变量ListCC中，然后把系统配置变量写入到所选层的配置文件中。
        /// </summary>
        private void SaveChannelSetToConfigFile()
        {
            if (ChannelsConfigDataGridView1.Rows.Count < 1)
            {
                return;
            }

            //验证输入的比例，基线位置，线宽等值的合法性
            StringBuilder sb=new StringBuilder();
            for (int i = 0; i < CommonClass.listDIC[LayerComboBox1.SelectedIndex].listCC.Count; i++)
            {
                String channelName = CommonClass.listDIC[LayerComboBox1.SelectedIndex].listCC[i].ChineseName;
                try
                {
                    float zoomIn = float.Parse(ChannelsConfigDataGridView1.Rows[i].Cells[7].Value.ToString());
                    //if (zoomIn <= 0)
                    //{
                    //    sb.AppendLine(String.Format("{0}- 通道比例必须大于0", channelName));
                    //}
                }
                catch (System.Exception ex)
                {
                    sb.AppendLine(String.Format("{0}- 通道比例格式错误（必须是数字）", channelName));
                }

                try
                {
                    int location = int.Parse(ChannelsConfigDataGridView1.Rows[i].Cells[8].Value.ToString());
                }
                catch (System.Exception ex)
                {
                    sb.AppendLine(String.Format("{0}- 基线位置格式错误（必须是整数）", channelName));
                }                

                
                try
                {
                    float fLineWidth = float.Parse(ChannelsConfigDataGridView1.Rows[i].Cells[10].Value.ToString());
                    if (fLineWidth <= 0)
                    {
                        sb.AppendLine(String.Format("{0}-  线宽必须大于0", channelName));
                    }
                }
                catch (System.Exception ex)
                {
                    sb.AppendLine(String.Format("{0}- 线宽格式错误（必须是大于0的数字）", channelName));
                }
            }
            if (!String.IsNullOrEmpty(sb.ToString()))
            {
                MessageBox.Show(sb.ToString());
                return;
            }



            //保存配置到系统
            for (int i = 0; i < CommonClass.listDIC[LayerComboBox1.SelectedIndex].listCC.Count; i++)
            {
                CommonClass.listDIC[LayerComboBox1.SelectedIndex].listCC[i].ChineseName =
                    ChannelsConfigDataGridView1.Rows[i].Cells[3].Value.ToString();
                CommonClass.listDIC[LayerComboBox1.SelectedIndex].listCC[i].NonChineseName =
                    ChannelsConfigDataGridView1.Rows[i].Cells[4].Value.ToString();
                CommonClass.listDIC[LayerComboBox1.SelectedIndex].listCC[i].Color =
                    ChannelsConfigDataGridView1.Rows[i].Cells[5].Style.BackColor.ToArgb();
                CommonClass.listDIC[LayerComboBox1.SelectedIndex].listCC[i].ZoomIn =
                    float.Parse(ChannelsConfigDataGridView1.Rows[i].Cells[7].Value.ToString());
                CommonClass.listDIC[LayerComboBox1.SelectedIndex].listCC[i].Location =
                    int.Parse(ChannelsConfigDataGridView1.Rows[i].Cells[8].Value.ToString());
                CommonClass.listDIC[LayerComboBox1.SelectedIndex].listCC[i].MeaOffset =
                    bool.Parse(ChannelsConfigDataGridView1.Rows[i].Cells[9].Value.ToString());
                CommonClass.listDIC[LayerComboBox1.SelectedIndex].listCC[i].fLineWidth =
                    float.Parse(ChannelsConfigDataGridView1.Rows[i].Cells[10].Value.ToString());
                CommonClass.listDIC[LayerComboBox1.SelectedIndex].listCC[i].bReverse =
                    bool.Parse(ChannelsConfigDataGridView1.Rows[i].Cells[11].Value.ToString());
                CommonClass.listDIC[LayerComboBox1.SelectedIndex].listCC[i].Visible =
                    bool.Parse(ChannelsConfigDataGridView1.Rows[i].Cells[6].Value.ToString());
                //dic.listCC[i].Rect = new Rectangle(MainGraphicsPictureBox1.ClientSize.Width - CommonClass.ChannelsAreaWidth,
                //                3 + dic.listCC[i].Location,
                //                CommonClass.ChannelsAreaWidth,
                //                CommonClass.ChannelsAreaHeight);
                CommonClass.listDIC[LayerComboBox1.SelectedIndex].listCC[i].Rect =
                    new Rectangle(CommonClass.listDIC[LayerComboBox1.SelectedIndex].listCC[i].Rect.X,
                        CommonClass.listDIC[LayerComboBox1.SelectedIndex].listCC[i].Location,
                        CommonClass.listDIC[LayerComboBox1.SelectedIndex].listCC[i].Rect.Width,
                        CommonClass.listDIC[LayerComboBox1.SelectedIndex].listCC[i].Rect.Height);
            }
            //保存配置到文件
            CommonClass.SaveChannelsConfig(LayerComboBox1.SelectedIndex);
        }
        #endregion

        #region 按钮单击响应事件---当前通道配置文件另存为
        /// <summary>
        /// 按钮单击响应事件---当前通道配置文件另存为
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveAsButton1_Click(object sender, EventArgs e)
        {
            try
            {
                SaveAsFileDialog1.FileName = "";
                DialogResult dr = SaveAsFileDialog1.ShowDialog();
                if (dr == DialogResult.OK)
                {
                    File.Copy(CommonClass.listDIC[LayerComboBox1.SelectedIndex].sConfigFilePath, SaveAsFileDialog1.FileName, true);
                    //把所选层的配置文件设置为新的
                    CommonClass.listDIC[LayerComboBox1.SelectedIndex].sConfigFilePath = SaveAsFileDialog1.FileName;
                    //保存配置到文件
                    SaveChannelSetToConfigFile();
                    //CommonClass.SaveChannelsConfig(LayerComboBox1.SelectedIndex);
                    //CommonClass.cdp.QueryDataChannelInfoHead(CommonClass.listDIC[LayerComboBox1.SelectedIndex].sFilePath);
                    bool bReturn = CommonClass.ReLoadChannelsConfig(SaveAsFileDialog1.FileName, LayerComboBox1.SelectedIndex);
                    if (bReturn)
                    {

                        GetChannelsData();
                        SaveButton1.Enabled = true;
                    }

                    //CommonClass.listDIC[LayerComboBox1.SelectedIndex].sConfigFilePath = SaveAsFileDialog1.FileName;
                    MessageBox.Show("保存成功！");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            QuickSetMFHScrollBar_Scroll();
        }
        #endregion

        #region 按钮单击响应事件---重新加载一个通道配置文件
        /// <summary>
        /// 按钮单击响应事件---重新加载一个通道配置文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenButton1_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog1.FileName = "";
                DialogResult dr = OpenFileDialog1.ShowDialog();
                if (dr == DialogResult.OK)
                {
                    bool bReturn = CommonClass.ReLoadChannelsConfig(OpenFileDialog1.FileName, LayerComboBox1.SelectedIndex);
                    if (bReturn)
                    {

                        GetChannelsData();
                        SaveButton1.Enabled = true;
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            QuickSetMFHScrollBar_Scroll();
        }
        #endregion     

        #region 按钮单击响应事件---设置成为当前文件的默认配置文件
        /// <summary>
        /// 按钮单击事件---生成默认配置文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonSaveAsDefault_Click(object sender, EventArgs e)
        {
            try
            {
                //SaveAsFileDialog1.FileName = "";
                //DialogResult dr = SaveAsFileDialog1.ShowDialog();
                //if (dr == DialogResult.OK)
                //{
                //    CommonClass.cdp.QueryDataChannelInfoHead(CommonClass.listDIC[LayerComboBox1.SelectedIndex].sFilePath);
                //    CommonClass.cdp.CreateWaveXMLConfig(SaveAsFileDialog1.FileName, 1);
                //    bool bReturn = CommonClass.ReLoadChannelsConfig(SaveAsFileDialog1.FileName, LayerComboBox1.SelectedIndex);
                //    if (bReturn)
                //    {

                //        GetChannelsData();
                //        SaveButton1.Enabled = true;
                //    }
                //    //CommonClass.listDIC[LayerComboBox1.SelectedIndex].sConfigFilePath = SaveAsFileDialog1.FileName;
                //    MessageBox.Show("保存成功！");
                //}


                //MessageBox.Show("shezhichenggong");
                CommonClass.sArrayConfigFile[LayerComboBox1.SelectedIndex] = CommonClass.listDIC[LayerComboBox1.SelectedIndex].sConfigFilePath;

                XmlDocument xd = new XmlDocument();
                xd.Load(CommonClass.AppConfigPath);

                for (int i = 0; i < xd.DocumentElement["ConfigFiles"].ChildNodes.Count; i++)
                {
                    string sName = xd.DocumentElement["ConfigFiles"].ChildNodes[i].Name;
                    string sId = sName.Substring(7, sName.Length - 7);
                    int id = int.Parse(sId) - 1;
                    if (id == LayerComboBox1.SelectedIndex)
                    {
                        xd.DocumentElement["ConfigFiles"].ChildNodes[i].InnerText = CommonClass.sArrayConfigFile[LayerComboBox1.SelectedIndex];
                    }

                }

                xd.Save(CommonClass.AppConfigPath);

                //DialogResult dr = MessageBox.Show("设置成功！");
                //if (dr==DialogResult.OK)
                //{
                //    return;
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            //QuickSetMFHScrollBar_Scroll();
            //MessageBox.Show("设置成功！");
        }
        #endregion        

        #region 按钮单击响应事件---自动排列
        /// <summary>
        /// 按钮单击响应事件---自动排列
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonChannelAutoArange_Click(object sender, EventArgs e)
        {

            if (ChannelsConfigDataGridView1.Rows.Count < 1)
            {
                return;
            }

            int iIndex = 0;
            for (int i = 0; i < ChannelsConfigDataGridView1.Rows.Count; i++)
            {
                if (bool.Parse(ChannelsConfigDataGridView1.Rows[i].Cells["Column6"].Value.ToString()))
                {
                    iIndex++;
                }
            }

            if (iIndex < 1)
            {
                return;
            }
            int iGrid = 100 / (iIndex + 1);
            int iSum = iGrid;
            for (int i = 0; i < ChannelsConfigDataGridView1.Rows.Count; i++)
            {
                if (bool.Parse(ChannelsConfigDataGridView1.Rows[i].Cells["Column6"].Value.ToString()))
                {
                    //Column10
                    ChannelsConfigDataGridView1.Rows[i].Cells["Column10"].Value = iSum.ToString();
                    iSum += iGrid;
                }
            }

        }
        #endregion



        #region 调用主控件的函数SetMFHScrollBar_Scroll，把新的通道配置应用到当前波形显示
        /// <summary>
        /// 调用主控件的函数SetMFHScrollBar_Scroll，把新的通道配置应用到当前波形显示
        /// </summary>
        private void QuickSetMFHScrollBar_Scroll()
        {
            try
            {
                ScrollEventArgs sea;
                sea = new ScrollEventArgs(ScrollEventType.EndScroll,
                        MainForm.sMainform.MainHScrollBar1.Value);
                MainForm.sMainform.SetMFHScrollBar_Scroll(new object(), sea);
            }
            catch
            {

            }
        }
        #endregion

        //获取层数据信息
        private void GetChannelsData()
        {
            ChannelsConfigDataGridView1.Rows.Clear();

            for (int i = 0; i < CommonClass.listDIC[LayerComboBox1.SelectedIndex].listCC.Count; i++)
            {
                object[] o = new object[12];
                o[0] = CommonClass.listDIC[LayerComboBox1.SelectedIndex].listCC[i].Id;
                o[1] = CommonClass.listDIC[LayerComboBox1.SelectedIndex].listCC[i].Name;
                o[2] = CommonClass.listDIC[LayerComboBox1.SelectedIndex].listCC[i].Units;
                o[3] = CommonClass.listDIC[LayerComboBox1.SelectedIndex].listCC[i].ChineseName;
                o[4] = CommonClass.listDIC[LayerComboBox1.SelectedIndex].listCC[i].NonChineseName;
                o[5] = Color.FromArgb(CommonClass.listDIC[LayerComboBox1.SelectedIndex].listCC[i].Color);
                o[6] = CommonClass.listDIC[LayerComboBox1.SelectedIndex].listCC[i].Visible;
                o[7] = CommonClass.listDIC[LayerComboBox1.SelectedIndex].listCC[i].ZoomIn;
                o[8] = CommonClass.listDIC[LayerComboBox1.SelectedIndex].listCC[i].Location;
                o[9] = CommonClass.listDIC[LayerComboBox1.SelectedIndex].listCC[i].MeaOffset;
                o[10] = CommonClass.listDIC[LayerComboBox1.SelectedIndex].listCC[i].fLineWidth;
                o[11] = CommonClass.listDIC[LayerComboBox1.SelectedIndex].listCC[i].bReverse;
                ChannelsConfigDataGridView1.Rows.Add(o);
                ChannelsConfigDataGridView1.Rows[ChannelsConfigDataGridView1.Rows.Count - 1].Cells[0].Style.BackColor = Color.DarkGray;
                ChannelsConfigDataGridView1.Rows[ChannelsConfigDataGridView1.Rows.Count - 1].Cells[1].Style.BackColor = Color.DarkGray;
                ChannelsConfigDataGridView1.Rows[ChannelsConfigDataGridView1.Rows.Count - 1].Cells[2].Style.BackColor = Color.DarkGray;
                ChannelsConfigDataGridView1.Rows[ChannelsConfigDataGridView1.Rows.Count - 1].Cells[5].Style.BackColor =
                    Color.FromArgb(CommonClass.listDIC[LayerComboBox1.SelectedIndex].listCC[i].Color);
                ChannelsConfigDataGridView1.Rows[ChannelsConfigDataGridView1.Rows.Count - 1].Cells[5].Value = "";

                //设置通道拖拽区
                CommonClass.listDIC[LayerComboBox1.SelectedIndex].listCC[i].Rect =
                    new Rectangle(MainForm.sMainform.MainGraphicsPictureBox1.ClientSize.Width - CommonClass.ChannelsAreaWidth,
                        CommonClass.listDIC[LayerComboBox1.SelectedIndex].listCC[i].Location,
                        CommonClass.ChannelsAreaWidth,
                        CommonClass.ChannelsAreaHeight);

                ConfigLabel.Text = Path.GetFileName(CommonClass.listDIC[LayerComboBox1.SelectedIndex].sConfigFilePath);
            }



        }

        private void buttonNewDefaultConfig_Click(object sender, EventArgs e)
        {
            //SaveAsFileDialog1.FileName = "";
            //DialogResult dr = SaveAsFileDialog1.ShowDialog();

            String configFilePath = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory ,"默认配置文件.xml");

            //if (dr == DialogResult.OK)
            //{
                CommonClass.cdp.QueryDataChannelInfoHead(CommonClass.listDIC[LayerComboBox1.SelectedIndex].sFilePath);
                CommonClass.cdp.CreateWaveXMLConfig(configFilePath, 1);
                bool bReturn = CommonClass.ReLoadChannelsConfig(configFilePath, LayerComboBox1.SelectedIndex);
                if (bReturn)
                {

                    GetChannelsData();
                    SaveButton1.Enabled = true;
                }
                //CommonClass.listDIC[LayerComboBox1.SelectedIndex].sConfigFilePath = SaveAsFileDialog1.FileName;
                //MessageBox.Show("保存成功！");

                QuickSetMFHScrollBar_Scroll();
            //}


        }

    }
}
