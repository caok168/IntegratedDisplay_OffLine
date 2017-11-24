namespace InGraph.Forms
{
    partial class AcclrtnAnalysisForm
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.buttonFolder = new System.Windows.Forms.Button();
            this.textBoxFolder = new System.Windows.Forms.TextBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.comboBoxFilePowerSpectrun = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonCreatePowerSpectrum = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.buttonThresholdSet = new System.Windows.Forms.Button();
            this.comboBoxThreshold = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBoxFileAcclrtn = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.buttonCompareOK = new System.Windows.Forms.Button();
            this.buttonCompareFileBrowser = new System.Windows.Forms.Button();
            this.textBoxCompareFileName = new System.Windows.Forms.TextBox();
            this.checkBoxCompare = new System.Windows.Forms.CheckBox();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.buttonDaoChaMileImport = new System.Windows.Forms.Button();
            this.textBoxDaoChaPeakLine = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.buttonReShow = new System.Windows.Forms.Button();
            this.buttonTPI = new System.Windows.Forms.Button();
            this.buttonRms = new System.Windows.Forms.Button();
            this.textBoxPeakLine = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.buttonExport = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.acclrtnAnalysisFormBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.panel1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.tabPage5.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.acclrtnAnalysisFormBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.buttonFolder);
            this.panel1.Controls.Add(this.textBoxFolder);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(990, 35);
            this.panel1.TabIndex = 0;
            // 
            // buttonFolder
            // 
            this.buttonFolder.Location = new System.Drawing.Point(873, 5);
            this.buttonFolder.Name = "buttonFolder";
            this.buttonFolder.Size = new System.Drawing.Size(88, 23);
            this.buttonFolder.TabIndex = 2;
            this.buttonFolder.Text = "选择文件夹";
            this.buttonFolder.UseVisualStyleBackColor = true;
            this.buttonFolder.Click += new System.EventHandler(this.buttonFolder_Click);
            // 
            // textBoxFolder
            // 
            this.textBoxFolder.Location = new System.Drawing.Point(11, 7);
            this.textBoxFolder.Name = "textBoxFolder";
            this.textBoxFolder.ReadOnly = true;
            this.textBoxFolder.Size = new System.Drawing.Size(856, 21);
            this.textBoxFolder.TabIndex = 1;
            this.textBoxFolder.TextChanged += new System.EventHandler(this.textBoxFolder_TextChanged);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(12, 53);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(990, 583);
            this.tabControl1.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.comboBoxFilePowerSpectrun);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.buttonCreatePowerSpectrum);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(982, 557);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "加速度功率谱";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // comboBoxFilePowerSpectrun
            // 
            this.comboBoxFilePowerSpectrun.FormattingEnabled = true;
            this.comboBoxFilePowerSpectrun.Location = new System.Drawing.Point(77, 13);
            this.comboBoxFilePowerSpectrun.Name = "comboBoxFilePowerSpectrun";
            this.comboBoxFilePowerSpectrun.Size = new System.Drawing.Size(787, 20);
            this.comboBoxFilePowerSpectrun.TabIndex = 4;
            this.comboBoxFilePowerSpectrun.Click += new System.EventHandler(this.comboBoxFilePowerSpectrun_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "文件选择：";
            // 
            // buttonCreatePowerSpectrum
            // 
            this.buttonCreatePowerSpectrum.Location = new System.Drawing.Point(870, 11);
            this.buttonCreatePowerSpectrum.Name = "buttonCreatePowerSpectrum";
            this.buttonCreatePowerSpectrum.Size = new System.Drawing.Size(88, 23);
            this.buttonCreatePowerSpectrum.TabIndex = 2;
            this.buttonCreatePowerSpectrum.Text = "生成功率谱图";
            this.buttonCreatePowerSpectrum.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.buttonThresholdSet);
            this.tabPage2.Controls.Add(this.comboBoxThreshold);
            this.tabPage2.Controls.Add(this.label3);
            this.tabPage2.Controls.Add(this.comboBoxFileAcclrtn);
            this.tabPage2.Controls.Add(this.label2);
            this.tabPage2.Controls.Add(this.tabControl2);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(982, 557);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "加速度有效值";
            this.tabPage2.UseVisualStyleBackColor = true;
            this.tabPage2.Enter += new System.EventHandler(this.tabPage2_Enter);
            // 
            // buttonThresholdSet
            // 
            this.buttonThresholdSet.Location = new System.Drawing.Point(692, 11);
            this.buttonThresholdSet.Name = "buttonThresholdSet";
            this.buttonThresholdSet.Size = new System.Drawing.Size(110, 23);
            this.buttonThresholdSet.TabIndex = 5;
            this.buttonThresholdSet.Text = "阀值设置";
            this.buttonThresholdSet.UseVisualStyleBackColor = true;
            this.buttonThresholdSet.Visible = false;
            this.buttonThresholdSet.Click += new System.EventHandler(this.buttonThresholdSet_Click);
            // 
            // comboBoxThreshold
            // 
            this.comboBoxThreshold.FormattingEnabled = true;
            this.comboBoxThreshold.Location = new System.Drawing.Point(510, 13);
            this.comboBoxThreshold.Name = "comboBoxThreshold";
            this.comboBoxThreshold.Size = new System.Drawing.Size(162, 20);
            this.comboBoxThreshold.TabIndex = 4;
            this.comboBoxThreshold.Visible = false;
            this.comboBoxThreshold.SelectedIndexChanged += new System.EventHandler(this.comboBoxThreshold_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(427, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 3;
            this.label3.Text = "检测车号：";
            this.label3.Visible = false;
            // 
            // comboBoxFileAcclrtn
            // 
            this.comboBoxFileAcclrtn.FormattingEnabled = true;
            this.comboBoxFileAcclrtn.Items.AddRange(new object[] {
            "11",
            "33",
            "84"});
            this.comboBoxFileAcclrtn.Location = new System.Drawing.Point(98, 13);
            this.comboBoxFileAcclrtn.Name = "comboBoxFileAcclrtn";
            this.comboBoxFileAcclrtn.Size = new System.Drawing.Size(323, 20);
            this.comboBoxFileAcclrtn.TabIndex = 2;
            this.comboBoxFileAcclrtn.SelectedIndexChanged += new System.EventHandler(this.comboBoxFileAcclrtn_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "加速度断面：";
            // 
            // tabControl2
            // 
            this.tabControl2.Controls.Add(this.tabPage5);
            this.tabControl2.Controls.Add(this.tabPage4);
            this.tabControl2.Location = new System.Drawing.Point(6, 39);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(970, 512);
            this.tabControl2.TabIndex = 0;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.buttonCompareOK);
            this.tabPage4.Controls.Add(this.buttonCompareFileBrowser);
            this.tabPage4.Controls.Add(this.textBoxCompareFileName);
            this.tabPage4.Controls.Add(this.checkBoxCompare);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(962, 486);
            this.tabPage4.TabIndex = 0;
            this.tabPage4.Text = "有效值分布图";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // buttonCompareOK
            // 
            this.buttonCompareOK.Enabled = false;
            this.buttonCompareOK.Location = new System.Drawing.Point(800, 418);
            this.buttonCompareOK.Name = "buttonCompareOK";
            this.buttonCompareOK.Size = new System.Drawing.Size(75, 23);
            this.buttonCompareOK.TabIndex = 4;
            this.buttonCompareOK.Text = "确定";
            this.buttonCompareOK.UseVisualStyleBackColor = true;
            // 
            // buttonCompareFileBrowser
            // 
            this.buttonCompareFileBrowser.Enabled = false;
            this.buttonCompareFileBrowser.Location = new System.Drawing.Point(719, 418);
            this.buttonCompareFileBrowser.Name = "buttonCompareFileBrowser";
            this.buttonCompareFileBrowser.Size = new System.Drawing.Size(75, 23);
            this.buttonCompareFileBrowser.TabIndex = 3;
            this.buttonCompareFileBrowser.Text = "浏览";
            this.buttonCompareFileBrowser.UseVisualStyleBackColor = true;
            // 
            // textBoxCompareFileName
            // 
            this.textBoxCompareFileName.Enabled = false;
            this.textBoxCompareFileName.Location = new System.Drawing.Point(22, 420);
            this.textBoxCompareFileName.Name = "textBoxCompareFileName";
            this.textBoxCompareFileName.Size = new System.Drawing.Size(691, 21);
            this.textBoxCompareFileName.TabIndex = 2;
            // 
            // checkBoxCompare
            // 
            this.checkBoxCompare.AutoSize = true;
            this.checkBoxCompare.Location = new System.Drawing.Point(22, 398);
            this.checkBoxCompare.Name = "checkBoxCompare";
            this.checkBoxCompare.Size = new System.Drawing.Size(96, 16);
            this.checkBoxCompare.TabIndex = 1;
            this.checkBoxCompare.Text = "分布比较功能";
            this.checkBoxCompare.UseVisualStyleBackColor = true;
            this.checkBoxCompare.CheckedChanged += new System.EventHandler(this.checkBoxCompare_CheckedChanged);
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.buttonDaoChaMileImport);
            this.tabPage5.Controls.Add(this.textBoxDaoChaPeakLine);
            this.tabPage5.Controls.Add(this.label5);
            this.tabPage5.Controls.Add(this.buttonReShow);
            this.tabPage5.Controls.Add(this.buttonTPI);
            this.tabPage5.Controls.Add(this.buttonRms);
            this.tabPage5.Controls.Add(this.textBoxPeakLine);
            this.tabPage5.Controls.Add(this.label4);
            this.tabPage5.Controls.Add(this.buttonExport);
            this.tabPage5.Controls.Add(this.groupBox1);
            this.tabPage5.Location = new System.Drawing.Point(4, 22);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage5.Size = new System.Drawing.Size(962, 486);
            this.tabPage5.TabIndex = 1;
            this.tabPage5.Text = "50米区段大值分布";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // buttonDaoChaMileImport
            // 
            this.buttonDaoChaMileImport.Location = new System.Drawing.Point(824, 443);
            this.buttonDaoChaMileImport.Name = "buttonDaoChaMileImport";
            this.buttonDaoChaMileImport.Size = new System.Drawing.Size(87, 23);
            this.buttonDaoChaMileImport.TabIndex = 12;
            this.buttonDaoChaMileImport.Text = "道岔位置导入";
            this.buttonDaoChaMileImport.UseVisualStyleBackColor = true;
            this.buttonDaoChaMileImport.Click += new System.EventHandler(this.buttonDaoChaMileImport_Click);
            // 
            // textBoxDaoChaPeakLine
            // 
            this.textBoxDaoChaPeakLine.Location = new System.Drawing.Point(636, 445);
            this.textBoxDaoChaPeakLine.Name = "textBoxDaoChaPeakLine";
            this.textBoxDaoChaPeakLine.Size = new System.Drawing.Size(100, 21);
            this.textBoxDaoChaPeakLine.TabIndex = 11;
            this.textBoxDaoChaPeakLine.Text = "6";
            this.textBoxDaoChaPeakLine.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBoxDaoChaPeakLine.Leave += new System.EventHandler(this.textBoxDaoChaPeakLine_TextChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(505, 448);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(89, 12);
            this.label5.TabIndex = 10;
            this.label5.Text = "道岔超限阈值：";
            // 
            // buttonReShow
            // 
            this.buttonReShow.Location = new System.Drawing.Point(178, 437);
            this.buttonReShow.Name = "buttonReShow";
            this.buttonReShow.Size = new System.Drawing.Size(101, 23);
            this.buttonReShow.TabIndex = 6;
            this.buttonReShow.Text = "重绘散点图";
            this.buttonReShow.UseVisualStyleBackColor = true;
            this.buttonReShow.Click += new System.EventHandler(this.buttonReShow_Click);
            // 
            // buttonTPI
            // 
            this.buttonTPI.Location = new System.Drawing.Point(235, 408);
            this.buttonTPI.Name = "buttonTPI";
            this.buttonTPI.Size = new System.Drawing.Size(106, 23);
            this.buttonTPI.TabIndex = 9;
            this.buttonTPI.Text = "轨道冲击指数";
            this.buttonTPI.UseVisualStyleBackColor = true;
            this.buttonTPI.Click += new System.EventHandler(this.buttonTPI_Click);
            // 
            // buttonRms
            // 
            this.buttonRms.Location = new System.Drawing.Point(113, 408);
            this.buttonRms.Name = "buttonRms";
            this.buttonRms.Size = new System.Drawing.Size(106, 23);
            this.buttonRms.TabIndex = 9;
            this.buttonRms.Text = "加速度有效值";
            this.buttonRms.UseVisualStyleBackColor = true;
            this.buttonRms.Click += new System.EventHandler(this.buttonRMS_Click);
            // 
            // textBoxPeakLine
            // 
            this.textBoxPeakLine.Location = new System.Drawing.Point(636, 408);
            this.textBoxPeakLine.Name = "textBoxPeakLine";
            this.textBoxPeakLine.Size = new System.Drawing.Size(100, 21);
            this.textBoxPeakLine.TabIndex = 8;
            this.textBoxPeakLine.Text = "4";
            this.textBoxPeakLine.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBoxPeakLine.Leave += new System.EventHandler(this.textBoxPeakLine_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(505, 413);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(125, 12);
            this.label4.TabIndex = 7;
            this.label4.Text = "直线或曲线超限阈值：";
            // 
            // buttonExport
            // 
            this.buttonExport.Location = new System.Drawing.Point(836, 408);
            this.buttonExport.Name = "buttonExport";
            this.buttonExport.Size = new System.Drawing.Size(75, 23);
            this.buttonExport.TabIndex = 6;
            this.buttonExport.Text = "超限导出";
            this.buttonExport.UseVisualStyleBackColor = true;
            this.buttonExport.Click += new System.EventHandler(this.buttonExport_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dataGridView1);
            this.groupBox1.Location = new System.Drawing.Point(504, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(452, 379);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "超标值列表";
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column3,
            this.Column4,
            this.Column5,
            this.Column6,
            this.Column7});
            this.dataGridView1.ContextMenuStrip = this.contextMenuStrip2;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(3, 17);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(446, 359);
            this.dataGridView1.TabIndex = 0;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "Id";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Width = 42;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "里程";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            this.Column2.Width = 70;
            // 
            // Column3
            // 
            this.Column3.HeaderText = "速度";
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            this.Column3.Width = 70;
            // 
            // Column4
            // 
            this.Column4.HeaderText = "50米区段大值";
            this.Column4.Name = "Column4";
            this.Column4.ReadOnly = true;
            this.Column4.Width = 102;
            // 
            // Column5
            // 
            this.Column5.HeaderText = "轨道冲击指数";
            this.Column5.Name = "Column5";
            this.Column5.ReadOnly = true;
            this.Column5.Width = 80;
            // 
            // Column6
            // 
            this.Column6.HeaderText = "有效性";
            this.Column6.Name = "Column6";
            this.Column6.ReadOnly = true;
            this.Column6.Width = 70;
            // 
            // Column7
            // 
            this.Column7.HeaderText = "是否道岔";
            this.Column7.Name = "Column7";
            // 
            // contextMenuStrip2
            // 
            this.contextMenuStrip2.Name = "contextMenuStrip2";
            this.contextMenuStrip2.ShowCheckMargin = true;
            this.contextMenuStrip2.ShowImageMargin = false;
            this.contextMenuStrip2.Size = new System.Drawing.Size(61, 4);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // acclrtnAnalysisFormBindingSource
            // 
            this.acclrtnAnalysisFormBindingSource.DataSource = typeof(InGraph.Forms.AcclrtnAnalysisForm);
            // 
            // AcclrtnAnalysisForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1014, 648);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AcclrtnAnalysisForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "轴箱加速度分析";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabControl2.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            this.tabPage5.ResumeLayout(false);
            this.tabPage5.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.acclrtnAnalysisFormBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button buttonFolder;
        private System.Windows.Forms.TextBox textBoxFolder;
        private System.Windows.Forms.ComboBox comboBoxFilePowerSpectrun;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonCreatePowerSpectrum;
        private System.Windows.Forms.Button buttonReShow;
        private System.Windows.Forms.Button buttonThresholdSet;
        private System.Windows.Forms.ComboBox comboBoxThreshold;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboBoxFileAcclrtn;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.TabPage tabPage5;
        private Steema.TeeChart.TChart tChart1;
        private System.Windows.Forms.Button buttonCompareOK;
        private System.Windows.Forms.Button buttonCompareFileBrowser;
        private System.Windows.Forms.TextBox textBoxCompareFileName;
        private System.Windows.Forms.CheckBox checkBoxCompare;
        private Steema.TeeChart.TChart tChart2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip2;
        private System.Windows.Forms.Button buttonExport;
        private System.Windows.Forms.TextBox textBoxPeakLine;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button buttonRms;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.BindingSource acclrtnAnalysisFormBindingSource;
        private System.Windows.Forms.Button buttonTPI;
        private System.Windows.Forms.TextBox textBoxDaoChaPeakLine;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column6;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column7;
        private System.Windows.Forms.Button buttonDaoChaMileImport;
    }
}