namespace InGraph.Forms
{
    partial class AcclrtnRmsProcsForm
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
            this.btn_BuildExcel = new System.Windows.Forms.Button();
            this.ckb_merge = new System.Windows.Forms.CheckBox();
            this.ckb_All = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtRmsMean1 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtRmsMean2 = new System.Windows.Forms.TextBox();
            this.txtRmsMean3 = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxMaxin50M)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCalcRMS)).BeginInit();
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
            // btn_BuildExcel
            // 
            this.btn_BuildExcel.Location = new System.Drawing.Point(766, 367);
            this.btn_BuildExcel.Name = "btn_BuildExcel";
            this.btn_BuildExcel.Size = new System.Drawing.Size(100, 23);
            this.btn_BuildExcel.TabIndex = 3;
            this.btn_BuildExcel.Text = "生成csv";
            this.btn_BuildExcel.UseVisualStyleBackColor = true;
            this.btn_BuildExcel.Click += new System.EventHandler(this.btn_BuildExcel_Click);
            // 
            // ckb_merge
            // 
            this.ckb_merge.AutoSize = true;
            this.ckb_merge.Location = new System.Drawing.Point(648, 370);
            this.ckb_merge.Name = "ckb_merge";
            this.ckb_merge.Size = new System.Drawing.Size(90, 16);
            this.ckb_merge.TabIndex = 4;
            this.ckb_merge.Text = "cit合并计算";
            this.ckb_merge.UseVisualStyleBackColor = true;
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
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(98, 369);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(161, 12);
            this.label1.TabIndex = 6;
            this.label1.Text = "左轴垂移动有效值信号平均值";
            // 
            // txtRmsMean1
            // 
            this.txtRmsMean1.Location = new System.Drawing.Point(279, 363);
            this.txtRmsMean1.Name = "txtRmsMean1";
            this.txtRmsMean1.Size = new System.Drawing.Size(100, 21);
            this.txtRmsMean1.TabIndex = 7;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(99, 394);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(161, 12);
            this.label2.TabIndex = 8;
            this.label2.Text = "右轴垂移动有效值信号平均值";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(99, 417);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(161, 12);
            this.label3.TabIndex = 9;
            this.label3.Text = "左轴横移动有效值信号平均值";
            // 
            // txtRmsMean2
            // 
            this.txtRmsMean2.Location = new System.Drawing.Point(279, 390);
            this.txtRmsMean2.Name = "txtRmsMean2";
            this.txtRmsMean2.Size = new System.Drawing.Size(100, 21);
            this.txtRmsMean2.TabIndex = 10;
            // 
            // txtRmsMean3
            // 
            this.txtRmsMean3.Location = new System.Drawing.Point(279, 414);
            this.txtRmsMean3.Name = "txtRmsMean3";
            this.txtRmsMean3.Size = new System.Drawing.Size(100, 21);
            this.txtRmsMean3.TabIndex = 11;
            // 
            // AcclrtnRmsProcsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(881, 460);
            this.Controls.Add(this.txtRmsMean3);
            this.Controls.Add(this.txtRmsMean2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtRmsMean1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ckb_All);
            this.Controls.Add(this.ckb_merge);
            this.Controls.Add(this.btn_BuildExcel);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AcclrtnRmsProcsForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "有效值概率分布计算";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxMaxin50M)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCalcRMS)).EndInit();
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
        private System.Windows.Forms.Button btn_BuildExcel;
        private System.Windows.Forms.CheckBox ckb_merge;
        private System.Windows.Forms.CheckBox ckb_All;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtRmsMean1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtRmsMean2;
        private System.Windows.Forms.TextBox txtRmsMean3;
    }
}