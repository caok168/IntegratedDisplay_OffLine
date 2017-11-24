namespace InGraph.Forms
{
    partial class AccIrtnCorrugation
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.textBoxFileDirectory = new System.Windows.Forms.TextBox();
            this.buttonFolderBrowser = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.pictureBoxMaxin50M = new System.Windows.Forms.PictureBox();
            this.pictureBoxCalcRMS = new System.Windows.Forms.PictureBox();
            this.listViewFile = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader8 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader9 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader10 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.backgroundWorkerMaxIn50M = new System.ComponentModel.BackgroundWorker();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.backgroundWorkerBat = new System.ComponentModel.BackgroundWorker();
            this.ckb_All = new System.Windows.Forms.CheckBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.txt_hj_FilterFreq_H = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txt_hj_FilterFreq_L = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txt_hj_len_win_imp = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.txt_bm_Wavelen_H = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txt_bm_Wavelen_L = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txt_bm_len_win = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txt_fs = new System.Windows.Forms.TextBox();
            this.txt_len_downsample = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txt_len_merge = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.btn_CalcRms = new System.Windows.Forms.Button();
            this.txtBM_Avg = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.txt_thresh_tii = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.btnCorruAvg = new System.Windows.Forms.Button();
            this.btnBMInfo = new System.Windows.Forms.Button();
            this.btnBMWave = new System.Windows.Forms.Button();
            this.txtAvg1 = new System.Windows.Forms.TextBox();
            this.txtAvg2 = new System.Windows.Forms.TextBox();
            this.txtAvg3 = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxMaxin50M)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCalcRMS)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textBoxFileDirectory);
            this.groupBox1.Controls.Add(this.buttonFolderBrowser);
            this.groupBox1.Location = new System.Drawing.Point(12, 5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(860, 45);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "路径选择";
            // 
            // textBoxFileDirectory
            // 
            this.textBoxFileDirectory.Location = new System.Drawing.Point(6, 17);
            this.textBoxFileDirectory.Name = "textBoxFileDirectory";
            this.textBoxFileDirectory.Size = new System.Drawing.Size(658, 21);
            this.textBoxFileDirectory.TabIndex = 2;
            // 
            // buttonFolderBrowser
            // 
            this.buttonFolderBrowser.Location = new System.Drawing.Point(670, 15);
            this.buttonFolderBrowser.Name = "buttonFolderBrowser";
            this.buttonFolderBrowser.Size = new System.Drawing.Size(75, 23);
            this.buttonFolderBrowser.TabIndex = 0;
            this.buttonFolderBrowser.Text = "浏览";
            this.buttonFolderBrowser.UseVisualStyleBackColor = true;
            this.buttonFolderBrowser.Click += new System.EventHandler(this.buttonFolderBrowser_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.pictureBoxMaxin50M);
            this.groupBox2.Controls.Add(this.pictureBoxCalcRMS);
            this.groupBox2.Controls.Add(this.listViewFile);
            this.groupBox2.Location = new System.Drawing.Point(12, 53);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(860, 292);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "文件列表";
            // 
            // pictureBoxMaxin50M
            // 
            this.pictureBoxMaxin50M.Image = global::InGraph.Properties.Resources.FILECOPY_16;
            this.pictureBoxMaxin50M.Location = new System.Drawing.Point(282, 198);
            this.pictureBoxMaxin50M.Name = "pictureBoxMaxin50M";
            this.pictureBoxMaxin50M.Size = new System.Drawing.Size(309, 69);
            this.pictureBoxMaxin50M.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxMaxin50M.TabIndex = 2;
            this.pictureBoxMaxin50M.TabStop = false;
            this.pictureBoxMaxin50M.Visible = false;
            // 
            // pictureBoxCalcRMS
            // 
            this.pictureBoxCalcRMS.BackColor = System.Drawing.SystemColors.Control;
            this.pictureBoxCalcRMS.Image = global::InGraph.Properties.Resources.jdt;
            this.pictureBoxCalcRMS.Location = new System.Drawing.Point(218, 231);
            this.pictureBoxCalcRMS.Name = "pictureBoxCalcRMS";
            this.pictureBoxCalcRMS.Size = new System.Drawing.Size(411, 36);
            this.pictureBoxCalcRMS.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxCalcRMS.TabIndex = 1;
            this.pictureBoxCalcRMS.TabStop = false;
            this.pictureBoxCalcRMS.Visible = false;
            // 
            // listViewFile
            // 
            this.listViewFile.CheckBoxes = true;
            this.listViewFile.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5,
            this.columnHeader6,
            this.columnHeader7,
            this.columnHeader8,
            this.columnHeader9,
            this.columnHeader10});
            this.listViewFile.FullRowSelect = true;
            this.listViewFile.GridLines = true;
            this.listViewFile.Location = new System.Drawing.Point(6, 20);
            this.listViewFile.MultiSelect = false;
            this.listViewFile.Name = "listViewFile";
            this.listViewFile.Size = new System.Drawing.Size(848, 264);
            this.listViewFile.TabIndex = 0;
            this.listViewFile.UseCompatibleStateImageBehavior = false;
            this.listViewFile.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "线路名";
            this.columnHeader1.Width = 78;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "线路代码";
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "行别";
            this.columnHeader3.Width = 50;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "增减里程";
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "检测日期";
            this.columnHeader5.Width = 100;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "检测时间";
            this.columnHeader6.Width = 100;
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "检测车号";
            this.columnHeader7.Width = 78;
            // 
            // columnHeader8
            // 
            this.columnHeader8.Text = "原始文件名";
            this.columnHeader8.Width = 236;
            // 
            // columnHeader9
            // 
            this.columnHeader9.Text = "大小(B)";
            this.columnHeader9.Width = 100;
            // 
            // columnHeader10
            // 
            this.columnHeader10.Text = "原始路径";
            this.columnHeader10.Width = 300;
            // 
            // timer1
            // 
            this.timer1.Interval = 30000;
            // 
            // ckb_All
            // 
            this.ckb_All.AutoSize = true;
            this.ckb_All.Location = new System.Drawing.Point(18, 366);
            this.ckb_All.Name = "ckb_All";
            this.ckb_All.Size = new System.Drawing.Size(54, 16);
            this.ckb_All.TabIndex = 5;
            this.ckb_All.Text = "全 选";
            this.ckb_All.UseVisualStyleBackColor = true;
            this.ckb_All.CheckedChanged += new System.EventHandler(this.ckb_All_CheckedChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.txt_hj_FilterFreq_H);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.txt_hj_FilterFreq_L);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.txt_hj_len_win_imp);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Location = new System.Drawing.Point(18, 403);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(266, 111);
            this.groupBox3.TabIndex = 6;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "计算焊接接头";
            // 
            // txt_hj_FilterFreq_H
            // 
            this.txt_hj_FilterFreq_H.Location = new System.Drawing.Point(107, 74);
            this.txt_hj_FilterFreq_H.Name = "txt_hj_FilterFreq_H";
            this.txt_hj_FilterFreq_H.Size = new System.Drawing.Size(100, 21);
            this.txt_hj_FilterFreq_H.TabIndex = 17;
            this.txt_hj_FilterFreq_H.Text = "500";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(10, 79);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(89, 12);
            this.label5.TabIndex = 16;
            this.label5.Text = "上限滤波频率：";
            // 
            // txt_hj_FilterFreq_L
            // 
            this.txt_hj_FilterFreq_L.Location = new System.Drawing.Point(107, 47);
            this.txt_hj_FilterFreq_L.Name = "txt_hj_FilterFreq_L";
            this.txt_hj_FilterFreq_L.Size = new System.Drawing.Size(100, 21);
            this.txt_hj_FilterFreq_L.TabIndex = 15;
            this.txt_hj_FilterFreq_L.Text = "20";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(10, 52);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(89, 12);
            this.label4.TabIndex = 14;
            this.label4.Text = "下限滤波频率：";
            // 
            // txt_hj_len_win_imp
            // 
            this.txt_hj_len_win_imp.Location = new System.Drawing.Point(107, 20);
            this.txt_hj_len_win_imp.Name = "txt_hj_len_win_imp";
            this.txt_hj_len_win_imp.Size = new System.Drawing.Size(100, 21);
            this.txt_hj_len_win_imp.TabIndex = 13;
            this.txt_hj_len_win_imp.Text = "60";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 25);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 12);
            this.label3.TabIndex = 12;
            this.label3.Text = "有效值窗长：";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.txt_bm_Wavelen_H);
            this.groupBox4.Controls.Add(this.label6);
            this.groupBox4.Controls.Add(this.txt_bm_Wavelen_L);
            this.groupBox4.Controls.Add(this.label7);
            this.groupBox4.Controls.Add(this.txt_bm_len_win);
            this.groupBox4.Controls.Add(this.label8);
            this.groupBox4.Location = new System.Drawing.Point(310, 403);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(253, 111);
            this.groupBox4.TabIndex = 7;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "计算波磨";
            // 
            // txt_bm_Wavelen_H
            // 
            this.txt_bm_Wavelen_H.Location = new System.Drawing.Point(107, 72);
            this.txt_bm_Wavelen_H.Name = "txt_bm_Wavelen_H";
            this.txt_bm_Wavelen_H.Size = new System.Drawing.Size(100, 21);
            this.txt_bm_Wavelen_H.TabIndex = 23;
            this.txt_bm_Wavelen_H.Text = "0.15";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(10, 77);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(89, 12);
            this.label6.TabIndex = 22;
            this.label6.Text = "上限滤波波长：";
            // 
            // txt_bm_Wavelen_L
            // 
            this.txt_bm_Wavelen_L.Location = new System.Drawing.Point(107, 45);
            this.txt_bm_Wavelen_L.Name = "txt_bm_Wavelen_L";
            this.txt_bm_Wavelen_L.Size = new System.Drawing.Size(100, 21);
            this.txt_bm_Wavelen_L.TabIndex = 21;
            this.txt_bm_Wavelen_L.Text = "0.04";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(10, 50);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(89, 12);
            this.label7.TabIndex = 20;
            this.label7.Text = "下限滤波波长：";
            // 
            // txt_bm_len_win
            // 
            this.txt_bm_len_win.Location = new System.Drawing.Point(107, 18);
            this.txt_bm_len_win.Name = "txt_bm_len_win";
            this.txt_bm_len_win.Size = new System.Drawing.Size(100, 21);
            this.txt_bm_len_win.TabIndex = 19;
            this.txt_bm_len_win.Text = "1000";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(10, 23);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(77, 12);
            this.label8.TabIndex = 18;
            this.label8.Text = "有效值窗长：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(97, 368);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 8;
            this.label1.Text = "采样频率：";
            // 
            // txt_fs
            // 
            this.txt_fs.Location = new System.Drawing.Point(168, 363);
            this.txt_fs.Name = "txt_fs";
            this.txt_fs.Size = new System.Drawing.Size(100, 21);
            this.txt_fs.TabIndex = 9;
            this.txt_fs.Text = "2000";
            // 
            // txt_len_downsample
            // 
            this.txt_len_downsample.Location = new System.Drawing.Point(364, 364);
            this.txt_len_downsample.Name = "txt_len_downsample";
            this.txt_len_downsample.Size = new System.Drawing.Size(100, 21);
            this.txt_len_downsample.TabIndex = 11;
            this.txt_len_downsample.Text = "5";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(286, 369);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 12);
            this.label2.TabIndex = 10;
            this.label2.Text = "重采样间隔：";
            // 
            // txt_len_merge
            // 
            this.txt_len_merge.Location = new System.Drawing.Point(543, 363);
            this.txt_len_merge.Name = "txt_len_merge";
            this.txt_len_merge.Size = new System.Drawing.Size(100, 21);
            this.txt_len_merge.TabIndex = 13;
            this.txt_len_merge.Text = "1000";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(478, 368);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(65, 12);
            this.label9.TabIndex = 12;
            this.label9.Text = "段的长度：";
            // 
            // btn_CalcRms
            // 
            this.btn_CalcRms.Location = new System.Drawing.Point(649, 361);
            this.btn_CalcRms.Name = "btn_CalcRms";
            this.btn_CalcRms.Size = new System.Drawing.Size(75, 23);
            this.btn_CalcRms.TabIndex = 14;
            this.btn_CalcRms.Text = "计算有效值";
            this.btn_CalcRms.UseVisualStyleBackColor = true;
            this.btn_CalcRms.Click += new System.EventHandler(this.btn_CalcRms_Click);
            // 
            // txtBM_Avg
            // 
            this.txtBM_Avg.Location = new System.Drawing.Point(681, 481);
            this.txtBM_Avg.Name = "txtBM_Avg";
            this.txtBM_Avg.Size = new System.Drawing.Size(100, 21);
            this.txtBM_Avg.TabIndex = 17;
            this.txtBM_Avg.Text = "1.5";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(579, 485);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(101, 12);
            this.label10.TabIndex = 16;
            this.label10.Text = "波磨指数平均值：";
            // 
            // txt_thresh_tii
            // 
            this.txt_thresh_tii.Location = new System.Drawing.Point(681, 455);
            this.txt_thresh_tii.Name = "txt_thresh_tii";
            this.txt_thresh_tii.Size = new System.Drawing.Size(100, 21);
            this.txt_thresh_tii.TabIndex = 19;
            this.txt_thresh_tii.Text = "4.0";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(579, 459);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(89, 12);
            this.label11.TabIndex = 18;
            this.label11.Text = "波磨指数阈值：";
            // 
            // btnCorruAvg
            // 
            this.btnCorruAvg.Location = new System.Drawing.Point(730, 361);
            this.btnCorruAvg.Name = "btnCorruAvg";
            this.btnCorruAvg.Size = new System.Drawing.Size(95, 23);
            this.btnCorruAvg.TabIndex = 20;
            this.btnCorruAvg.Text = "计算指数平均值";
            this.btnCorruAvg.UseVisualStyleBackColor = true;
            this.btnCorruAvg.Click += new System.EventHandler(this.btnCorruAvg_Click);
            // 
            // btnBMInfo
            // 
            this.btnBMInfo.Location = new System.Drawing.Point(649, 390);
            this.btnBMInfo.Name = "btnBMInfo";
            this.btnBMInfo.Size = new System.Drawing.Size(75, 23);
            this.btnBMInfo.TabIndex = 21;
            this.btnBMInfo.Text = "波磨信息";
            this.btnBMInfo.UseVisualStyleBackColor = true;
            this.btnBMInfo.Click += new System.EventHandler(this.btnBMInfo_Click);
            // 
            // btnBMWave
            // 
            this.btnBMWave.Location = new System.Drawing.Point(649, 419);
            this.btnBMWave.Name = "btnBMWave";
            this.btnBMWave.Size = new System.Drawing.Size(75, 23);
            this.btnBMWave.TabIndex = 22;
            this.btnBMWave.Text = "波磨波形";
            this.btnBMWave.UseVisualStyleBackColor = true;
            this.btnBMWave.Click += new System.EventHandler(this.btnBMWave_Click);
            // 
            // txtAvg1
            // 
            this.txtAvg1.Location = new System.Drawing.Point(936, 360);
            this.txtAvg1.Name = "txtAvg1";
            this.txtAvg1.Size = new System.Drawing.Size(100, 21);
            this.txtAvg1.TabIndex = 23;
            // 
            // txtAvg2
            // 
            this.txtAvg2.Location = new System.Drawing.Point(936, 387);
            this.txtAvg2.Name = "txtAvg2";
            this.txtAvg2.Size = new System.Drawing.Size(100, 21);
            this.txtAvg2.TabIndex = 24;
            // 
            // txtAvg3
            // 
            this.txtAvg3.Location = new System.Drawing.Point(936, 414);
            this.txtAvg3.Name = "txtAvg3";
            this.txtAvg3.Size = new System.Drawing.Size(100, 21);
            this.txtAvg3.TabIndex = 25;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(839, 367);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(95, 12);
            this.label12.TabIndex = 26;
            this.label12.Text = "轴箱左垂平均值:";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(839, 391);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(95, 12);
            this.label13.TabIndex = 27;
            this.label13.Text = "轴箱右垂平均值:";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(839, 418);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(95, 12);
            this.label14.TabIndex = 28;
            this.label14.Text = "轴箱左横平均值:";
            // 
            // AccIrtnCorrugation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1048, 537);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.txtAvg3);
            this.Controls.Add(this.txtAvg2);
            this.Controls.Add(this.txtAvg1);
            this.Controls.Add(this.btnBMWave);
            this.Controls.Add(this.btnBMInfo);
            this.Controls.Add(this.btnCorruAvg);
            this.Controls.Add(this.txt_thresh_tii);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.txtBM_Avg);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.btn_CalcRms);
            this.Controls.Add(this.txt_len_merge);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.txt_len_downsample);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txt_fs);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.ckb_All);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AccIrtnCorrugation";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "波磨计算";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxMaxin50M)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCalcRMS)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        /// <summary>
        /// 按钮--浏览文件夹
        /// </summary>
        private System.Windows.Forms.Button buttonFolderBrowser;
        /// <summary>
        /// 文本框--文件夹路径
        /// </summary>
        private System.Windows.Forms.TextBox textBoxFileDirectory;
        private System.Windows.Forms.GroupBox groupBox2;
        /// <summary>
        /// 文件列表
        /// </summary>
        private System.Windows.Forms.ListView listViewFile;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.ColumnHeader columnHeader7;
        private System.Windows.Forms.ColumnHeader columnHeader8;
        private System.Windows.Forms.ColumnHeader columnHeader9;
        private System.Windows.Forms.ColumnHeader columnHeader10;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.ComponentModel.BackgroundWorker backgroundWorkerMaxIn50M;
        private System.Windows.Forms.PictureBox pictureBoxCalcRMS;
        private System.Windows.Forms.PictureBox pictureBoxMaxin50M;
        private System.Windows.Forms.Timer timer1;
        private System.ComponentModel.BackgroundWorker backgroundWorkerBat;
        private System.Windows.Forms.CheckBox ckb_All;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.TextBox txt_hj_FilterFreq_H;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txt_hj_FilterFreq_L;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txt_hj_len_win_imp;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txt_fs;
        private System.Windows.Forms.TextBox txt_len_downsample;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txt_bm_Wavelen_H;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txt_bm_Wavelen_L;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txt_bm_len_win;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txt_len_merge;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button btn_CalcRms;
        private System.Windows.Forms.TextBox txtBM_Avg;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txt_thresh_tii;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button btnCorruAvg;
        private System.Windows.Forms.Button btnBMInfo;
        private System.Windows.Forms.Button btnBMWave;
        private System.Windows.Forms.TextBox txtAvg1;
        private System.Windows.Forms.TextBox txtAvg2;
        private System.Windows.Forms.TextBox txtAvg3;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
    }
}