namespace InGraph.Forms
{
    partial class OpenDataDialog
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
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.label7 = new System.Windows.Forms.Label();
            this.dateTimePickerEnd = new System.Windows.Forms.DateTimePicker();
            this.dateTimePickerStart = new System.Windows.Forms.DateTimePicker();
            this.label6 = new System.Windows.Forms.Label();
            this.comboBoxTrainNo = new System.Windows.Forms.ComboBox();
            this.comboBoxLineDir = new System.Windows.Forms.ComboBox();
            this.comboBoxLineName = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.IncludeSubFolderCheckBox1 = new System.Windows.Forms.CheckBox();
            this.BroButton1 = new System.Windows.Forms.Button();
            this.PathTextBox1 = new System.Windows.Forms.TextBox();
            this.OpenButton1 = new System.Windows.Forms.Button();
            this.LoadIndexCheckBox1 = new System.Windows.Forms.CheckBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.InfoLabel1 = new System.Windows.Forms.Label();
            this.FilesListView1 = new System.Windows.Forms.ListView();
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader11 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader8 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader_Date = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader_Time = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader10 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader_FileName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader_Directory = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // folderBrowserDialog1
            // 
            this.folderBrowserDialog1.ShowNewFolderButton = false;
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(620, 8);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(24, 21);
            this.label7.TabIndex = 43;
            this.label7.Text = "到";
            // 
            // dateTimePickerEnd
            // 
            this.dateTimePickerEnd.CustomFormat = "yyyy-MM-dd";
            this.dateTimePickerEnd.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePickerEnd.Location = new System.Drawing.Point(648, 5);
            this.dateTimePickerEnd.Name = "dateTimePickerEnd";
            this.dateTimePickerEnd.ShowUpDown = true;
            this.dateTimePickerEnd.Size = new System.Drawing.Size(87, 21);
            this.dateTimePickerEnd.TabIndex = 42;
            this.dateTimePickerEnd.ValueChanged += new System.EventHandler(this.dateTimePickerEnd_ValueChanged);
            // 
            // dateTimePickerStart
            // 
            this.dateTimePickerStart.CustomFormat = "yyyy-MM-dd";
            this.dateTimePickerStart.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePickerStart.Location = new System.Drawing.Point(525, 5);
            this.dateTimePickerStart.Name = "dateTimePickerStart";
            this.dateTimePickerStart.ShowUpDown = true;
            this.dateTimePickerStart.Size = new System.Drawing.Size(87, 21);
            this.dateTimePickerStart.TabIndex = 41;
            this.dateTimePickerStart.Value = new System.DateTime(2010, 1, 1, 0, 0, 0, 0);
            this.dateTimePickerStart.ValueChanged += new System.EventHandler(this.dateTimePickerStart_ValueChanged);
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(482, 8);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(37, 21);
            this.label6.TabIndex = 40;
            this.label6.Text = "日期";
            // 
            // comboBoxTrainNo
            // 
            this.comboBoxTrainNo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxTrainNo.FormattingEnabled = true;
            this.comboBoxTrainNo.Location = new System.Drawing.Point(309, 5);
            this.comboBoxTrainNo.Name = "comboBoxTrainNo";
            this.comboBoxTrainNo.Size = new System.Drawing.Size(151, 20);
            this.comboBoxTrainNo.TabIndex = 39;
            this.comboBoxTrainNo.SelectedIndexChanged += new System.EventHandler(this.comboBoxTrainNo_SelectedIndexChanged);
            // 
            // comboBoxLineDir
            // 
            this.comboBoxLineDir.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxLineDir.FormattingEnabled = true;
            this.comboBoxLineDir.Location = new System.Drawing.Point(186, 5);
            this.comboBoxLineDir.Name = "comboBoxLineDir";
            this.comboBoxLineDir.Size = new System.Drawing.Size(57, 20);
            this.comboBoxLineDir.TabIndex = 38;
            this.comboBoxLineDir.SelectedIndexChanged += new System.EventHandler(this.comboBoxLineDir_SelectedIndexChanged);
            // 
            // comboBoxLineName
            // 
            this.comboBoxLineName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxLineName.FormattingEnabled = true;
            this.comboBoxLineName.Location = new System.Drawing.Point(49, 5);
            this.comboBoxLineName.Name = "comboBoxLineName";
            this.comboBoxLineName.Size = new System.Drawing.Size(82, 20);
            this.comboBoxLineName.TabIndex = 37;
            this.comboBoxLineName.SelectedIndexChanged += new System.EventHandler(this.comboBoxLineName_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(260, 8);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(43, 21);
            this.label5.TabIndex = 36;
            this.label5.Text = "车号";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(137, 8);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(43, 21);
            this.label4.TabIndex = 35;
            this.label4.Text = "行别";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(3, 8);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(43, 21);
            this.label3.TabIndex = 34;
            this.label3.Text = "线名";
            // 
            // IncludeSubFolderCheckBox1
            // 
            this.IncludeSubFolderCheckBox1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.IncludeSubFolderCheckBox1.Location = new System.Drawing.Point(650, 3);
            this.IncludeSubFolderCheckBox1.Name = "IncludeSubFolderCheckBox1";
            this.IncludeSubFolderCheckBox1.Size = new System.Drawing.Size(85, 24);
            this.IncludeSubFolderCheckBox1.TabIndex = 20;
            this.IncludeSubFolderCheckBox1.Text = "包括子目录";
            this.IncludeSubFolderCheckBox1.UseVisualStyleBackColor = true;
            this.IncludeSubFolderCheckBox1.CheckedChanged += new System.EventHandler(this.IncludeSubFolderCheckBox1_CheckedChanged);
            // 
            // BroButton1
            // 
            this.BroButton1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.BroButton1.Location = new System.Drawing.Point(569, 3);
            this.BroButton1.Name = "BroButton1";
            this.BroButton1.Size = new System.Drawing.Size(75, 23);
            this.BroButton1.TabIndex = 19;
            this.BroButton1.Text = "浏览(&B)";
            this.BroButton1.UseVisualStyleBackColor = true;
            this.BroButton1.Click += new System.EventHandler(this.button1_Click);
            // 
            // PathTextBox1
            // 
            this.PathTextBox1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.PathTextBox1.Location = new System.Drawing.Point(3, 5);
            this.PathTextBox1.Name = "PathTextBox1";
            this.PathTextBox1.ReadOnly = true;
            this.PathTextBox1.Size = new System.Drawing.Size(560, 21);
            this.PathTextBox1.TabIndex = 18;
            this.PathTextBox1.KeyUp += new System.Windows.Forms.KeyEventHandler(this.PathTextBox1_KeyUp);
            // 
            // OpenButton1
            // 
            this.OpenButton1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.OpenButton1.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OpenButton1.Location = new System.Drawing.Point(764, 434);
            this.OpenButton1.Name = "OpenButton1";
            this.OpenButton1.Size = new System.Drawing.Size(75, 23);
            this.OpenButton1.TabIndex = 23;
            this.OpenButton1.Text = "打开(&O)";
            this.OpenButton1.UseVisualStyleBackColor = true;
            this.OpenButton1.Click += new System.EventHandler(this.OpenButton1_Click);
            // 
            // LoadIndexCheckBox1
            // 
            this.LoadIndexCheckBox1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.LoadIndexCheckBox1.Location = new System.Drawing.Point(644, 434);
            this.LoadIndexCheckBox1.Name = "LoadIndexCheckBox1";
            this.LoadIndexCheckBox1.Size = new System.Drawing.Size(104, 24);
            this.LoadIndexCheckBox1.TabIndex = 29;
            this.LoadIndexCheckBox1.Text = "加载波形索引";
            this.LoadIndexCheckBox1.UseVisualStyleBackColor = true;
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.Enabled = false;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "单机模式",
            "网络模式"});
            this.comboBox1.Location = new System.Drawing.Point(12, 13);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(75, 20);
            this.comboBox1.TabIndex = 33;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // InfoLabel1
            // 
            this.InfoLabel1.Location = new System.Drawing.Point(265, 243);
            this.InfoLabel1.Name = "InfoLabel1";
            this.InfoLabel1.Size = new System.Drawing.Size(194, 23);
            this.InfoLabel1.TabIndex = 34;
            this.InfoLabel1.Text = "正在获取,请稍候...";
            this.InfoLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.InfoLabel1.Visible = false;
            // 
            // FilesListView1
            // 
            this.FilesListView1.CheckBoxes = true;
            this.FilesListView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader4,
            this.columnHeader5,
            this.columnHeader6,
            this.columnHeader11,
            this.columnHeader8,
            this.columnHeader_Date,
            this.columnHeader_Time,
            this.columnHeader10,
            this.columnHeader_FileName,
            this.columnHeader2,
            this.columnHeader_Directory});
            this.FilesListView1.FullRowSelect = true;
            this.FilesListView1.GridLines = true;
            this.FilesListView1.Location = new System.Drawing.Point(3, 43);
            this.FilesListView1.MultiSelect = false;
            this.FilesListView1.Name = "FilesListView1";
            this.FilesListView1.Size = new System.Drawing.Size(844, 385);
            this.FilesListView1.TabIndex = 35;
            this.FilesListView1.UseCompatibleStateImageBehavior = false;
            this.FilesListView1.View = System.Windows.Forms.View.Details;
            this.FilesListView1.EnabledChanged += new System.EventHandler(this.FilesListView1_EnabledChanged);
            this.FilesListView1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.FilesListView1_MouseDown);
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "线路名";
            this.columnHeader4.Width = 78;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "线路编号";
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "行别";
            this.columnHeader6.Width = 50;
            // 
            // columnHeader11
            // 
            this.columnHeader11.Text = "方向";
            this.columnHeader11.Width = 50;
            // 
            // columnHeader8
            // 
            this.columnHeader8.Text = "增减里程";
            // 
            // columnHeader_Date
            // 
            this.columnHeader_Date.Text = "检测日期";
            this.columnHeader_Date.Width = 75;
            // 
            // columnHeader_Time
            // 
            this.columnHeader_Time.Text = "检测时间";
            this.columnHeader_Time.Width = 75;
            // 
            // columnHeader10
            // 
            this.columnHeader10.Text = "检测车号";
            this.columnHeader10.Width = 78;
            // 
            // columnHeader_FileName
            // 
            this.columnHeader_FileName.Text = "原始文件名";
            this.columnHeader_FileName.Width = 236;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "大小(B)";
            this.columnHeader2.Width = 100;
            // 
            // columnHeader_Directory
            // 
            this.columnHeader_Directory.Text = "原始路径";
            this.columnHeader_Directory.Width = 300;
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.PathTextBox1);
            this.panel1.Controls.Add(this.BroButton1);
            this.panel1.Controls.Add(this.IncludeSubFolderCheckBox1);
            this.panel1.Location = new System.Drawing.Point(95, 6);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(744, 31);
            this.panel1.TabIndex = 44;
            this.panel1.Visible = false;
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.comboBoxLineName);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.dateTimePickerEnd);
            this.panel2.Controls.Add(this.comboBoxLineDir);
            this.panel2.Controls.Add(this.label7);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.dateTimePickerStart);
            this.panel2.Controls.Add(this.comboBoxTrainNo);
            this.panel2.Controls.Add(this.label6);
            this.panel2.Location = new System.Drawing.Point(103, 113);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(744, 31);
            this.panel2.TabIndex = 45;
            this.panel2.Visible = false;
            // 
            // OpenDataDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(850, 462);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.InfoLabel1);
            this.Controls.Add(this.FilesListView1);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.LoadIndexCheckBox1);
            this.Controls.Add(this.OpenButton1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OpenDataDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "打开数据";
            this.Load += new System.EventHandler(this.OpenDataDialog_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OpenDataDialog_KeyDown);
            this.Resize += new System.EventHandler(this.OpenDataDialog_Resize);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.CheckBox IncludeSubFolderCheckBox1;
        private System.Windows.Forms.Button BroButton1;
        private System.Windows.Forms.TextBox PathTextBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox comboBoxLineName;
        private System.Windows.Forms.ComboBox comboBoxTrainNo;
        private System.Windows.Forms.ComboBox comboBoxLineDir;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.DateTimePicker dateTimePickerStart;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.DateTimePicker dateTimePickerEnd;
        private System.Windows.Forms.Button OpenButton1;
        private System.Windows.Forms.CheckBox LoadIndexCheckBox1;
        private System.Windows.Forms.ComboBox comboBox1;
        /// <summary>
        /// 正在获取，请稍后
        /// </summary>
        private System.Windows.Forms.Label InfoLabel1;
        private System.Windows.Forms.ListView FilesListView1;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.ColumnHeader columnHeader8;
        private System.Windows.Forms.ColumnHeader columnHeader_Date;
        private System.Windows.Forms.ColumnHeader columnHeader_Time;
        private System.Windows.Forms.ColumnHeader columnHeader10;
        private System.Windows.Forms.ColumnHeader columnHeader_FileName;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader_Directory;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ColumnHeader columnHeader11;
    }
}