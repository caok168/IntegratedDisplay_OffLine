namespace InGraph.Forms
{
    partial class WaveConvertForm
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
            this.groupBoxGeoSelect = new System.Windows.Forms.GroupBox();
            this.textBoxGeoPath = new System.Windows.Forms.TextBox();
            this.btnGeoSelect = new System.Windows.Forms.Button();
            this.groupBoxCitHeadEditer = new System.Windows.Forms.GroupBox();
            this.textBoxEndPos = new System.Windows.Forms.TextBox();
            this.textBoxStartPos = new System.Windows.Forms.TextBox();
            this.dateTimePickerTime = new System.Windows.Forms.DateTimePicker();
            this.dateTimePickerData = new System.Windows.Forms.DateTimePicker();
            this.labelKmInc = new System.Windows.Forms.Label();
            this.labelRunDir = new System.Windows.Forms.Label();
            this.labelEndPos = new System.Windows.Forms.Label();
            this.labelStartPos = new System.Windows.Forms.Label();
            this.labelTrainCode = new System.Windows.Forms.Label();
            this.labelTime = new System.Windows.Forms.Label();
            this.labelDate = new System.Windows.Forms.Label();
            this.labelLineDir = new System.Windows.Forms.Label();
            this.labelLineName = new System.Windows.Forms.Label();
            this.comboBoxKmInc = new System.Windows.Forms.ComboBox();
            this.comboBoxRunDir = new System.Windows.Forms.ComboBox();
            this.comboBoxTrainCode = new System.Windows.Forms.ComboBox();
            this.comboBoxLineDir = new System.Windows.Forms.ComboBox();
            this.comboBoxLineName = new System.Windows.Forms.ComboBox();
            this.buttonConvertData = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.button1 = new System.Windows.Forms.Button();
            this.textBoxIICFilePath = new System.Windows.Forms.TextBox();
            this.panelIIC = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBoxGeoSelect.SuspendLayout();
            this.groupBoxCitHeadEditer.SuspendLayout();
            this.panelIIC.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxGeoSelect
            // 
            this.groupBoxGeoSelect.Controls.Add(this.textBoxGeoPath);
            this.groupBoxGeoSelect.Controls.Add(this.btnGeoSelect);
            this.groupBoxGeoSelect.Location = new System.Drawing.Point(13, 14);
            this.groupBoxGeoSelect.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBoxGeoSelect.Name = "groupBoxGeoSelect";
            this.groupBoxGeoSelect.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBoxGeoSelect.Size = new System.Drawing.Size(495, 104);
            this.groupBoxGeoSelect.TabIndex = 0;
            this.groupBoxGeoSelect.TabStop = false;
            this.groupBoxGeoSelect.Text = "选择GEO波形";
            // 
            // textBoxGeoPath
            // 
            this.textBoxGeoPath.Location = new System.Drawing.Point(8, 26);
            this.textBoxGeoPath.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.textBoxGeoPath.Multiline = true;
            this.textBoxGeoPath.Name = "textBoxGeoPath";
            this.textBoxGeoPath.ReadOnly = true;
            this.textBoxGeoPath.Size = new System.Drawing.Size(376, 68);
            this.textBoxGeoPath.TabIndex = 1;
            // 
            // btnGeoSelect
            // 
            this.btnGeoSelect.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnGeoSelect.Location = new System.Drawing.Point(404, 26);
            this.btnGeoSelect.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnGeoSelect.Name = "btnGeoSelect";
            this.btnGeoSelect.Size = new System.Drawing.Size(78, 68);
            this.btnGeoSelect.TabIndex = 0;
            this.btnGeoSelect.Text = "打开波形";
            this.btnGeoSelect.UseVisualStyleBackColor = true;
            this.btnGeoSelect.Click += new System.EventHandler(this.btnGeoSelect_Click);
            // 
            // groupBoxCitHeadEditer
            // 
            this.groupBoxCitHeadEditer.Controls.Add(this.textBoxEndPos);
            this.groupBoxCitHeadEditer.Controls.Add(this.textBoxStartPos);
            this.groupBoxCitHeadEditer.Controls.Add(this.dateTimePickerTime);
            this.groupBoxCitHeadEditer.Controls.Add(this.dateTimePickerData);
            this.groupBoxCitHeadEditer.Controls.Add(this.labelKmInc);
            this.groupBoxCitHeadEditer.Controls.Add(this.labelRunDir);
            this.groupBoxCitHeadEditer.Controls.Add(this.labelEndPos);
            this.groupBoxCitHeadEditer.Controls.Add(this.labelStartPos);
            this.groupBoxCitHeadEditer.Controls.Add(this.labelTrainCode);
            this.groupBoxCitHeadEditer.Controls.Add(this.labelTime);
            this.groupBoxCitHeadEditer.Controls.Add(this.labelDate);
            this.groupBoxCitHeadEditer.Controls.Add(this.labelLineDir);
            this.groupBoxCitHeadEditer.Controls.Add(this.labelLineName);
            this.groupBoxCitHeadEditer.Controls.Add(this.comboBoxKmInc);
            this.groupBoxCitHeadEditer.Controls.Add(this.comboBoxRunDir);
            this.groupBoxCitHeadEditer.Controls.Add(this.comboBoxTrainCode);
            this.groupBoxCitHeadEditer.Controls.Add(this.comboBoxLineDir);
            this.groupBoxCitHeadEditer.Controls.Add(this.comboBoxLineName);
            this.groupBoxCitHeadEditer.Location = new System.Drawing.Point(13, 128);
            this.groupBoxCitHeadEditer.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBoxCitHeadEditer.Name = "groupBoxCitHeadEditer";
            this.groupBoxCitHeadEditer.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBoxCitHeadEditer.Size = new System.Drawing.Size(495, 213);
            this.groupBoxCitHeadEditer.TabIndex = 1;
            this.groupBoxCitHeadEditer.TabStop = false;
            this.groupBoxCitHeadEditer.Text = "文件头编辑";
            // 
            // textBoxEndPos
            // 
            this.textBoxEndPos.Location = new System.Drawing.Point(360, 100);
            this.textBoxEndPos.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.textBoxEndPos.Name = "textBoxEndPos";
            this.textBoxEndPos.Size = new System.Drawing.Size(122, 26);
            this.textBoxEndPos.TabIndex = 6;
            this.textBoxEndPos.Text = "0";
            this.textBoxEndPos.Leave += new System.EventHandler(this.Textbox_Check);
            // 
            // textBoxStartPos
            // 
            this.textBoxStartPos.Location = new System.Drawing.Point(360, 64);
            this.textBoxStartPos.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.textBoxStartPos.Name = "textBoxStartPos";
            this.textBoxStartPos.Size = new System.Drawing.Size(122, 26);
            this.textBoxStartPos.TabIndex = 5;
            this.textBoxStartPos.Text = "0";
            this.textBoxStartPos.Leave += new System.EventHandler(this.Textbox_Check);
            // 
            // dateTimePickerTime
            // 
            this.dateTimePickerTime.CustomFormat = "HH:mm:ss";
            this.dateTimePickerTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePickerTime.Location = new System.Drawing.Point(123, 138);
            this.dateTimePickerTime.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.dateTimePickerTime.Name = "dateTimePickerTime";
            this.dateTimePickerTime.ShowUpDown = true;
            this.dateTimePickerTime.Size = new System.Drawing.Size(122, 26);
            this.dateTimePickerTime.TabIndex = 3;
            // 
            // dateTimePickerData
            // 
            this.dateTimePickerData.CustomFormat = "yyyy-MM-dd";
            this.dateTimePickerData.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePickerData.Location = new System.Drawing.Point(123, 102);
            this.dateTimePickerData.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.dateTimePickerData.Name = "dateTimePickerData";
            this.dateTimePickerData.ShowUpDown = true;
            this.dateTimePickerData.Size = new System.Drawing.Size(122, 26);
            this.dateTimePickerData.TabIndex = 2;
            // 
            // labelKmInc
            // 
            this.labelKmInc.AutoSize = true;
            this.labelKmInc.Location = new System.Drawing.Point(284, 177);
            this.labelKmInc.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelKmInc.Name = "labelKmInc";
            this.labelKmInc.Size = new System.Drawing.Size(65, 20);
            this.labelKmInc.TabIndex = 1;
            this.labelKmInc.Text = "增减里程";
            // 
            // labelRunDir
            // 
            this.labelRunDir.AutoSize = true;
            this.labelRunDir.Location = new System.Drawing.Point(284, 139);
            this.labelRunDir.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelRunDir.Name = "labelRunDir";
            this.labelRunDir.Size = new System.Drawing.Size(65, 20);
            this.labelRunDir.TabIndex = 1;
            this.labelRunDir.Text = "检测方向";
            // 
            // labelEndPos
            // 
            this.labelEndPos.AutoSize = true;
            this.labelEndPos.Location = new System.Drawing.Point(284, 103);
            this.labelEndPos.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelEndPos.Name = "labelEndPos";
            this.labelEndPos.Size = new System.Drawing.Size(65, 20);
            this.labelEndPos.TabIndex = 1;
            this.labelEndPos.Text = "结束里程";
            // 
            // labelStartPos
            // 
            this.labelStartPos.AutoSize = true;
            this.labelStartPos.Location = new System.Drawing.Point(284, 67);
            this.labelStartPos.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelStartPos.Name = "labelStartPos";
            this.labelStartPos.Size = new System.Drawing.Size(65, 20);
            this.labelStartPos.TabIndex = 1;
            this.labelStartPos.Text = "开始里程";
            // 
            // labelTrainCode
            // 
            this.labelTrainCode.AutoSize = true;
            this.labelTrainCode.Location = new System.Drawing.Point(284, 29);
            this.labelTrainCode.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelTrainCode.Name = "labelTrainCode";
            this.labelTrainCode.Size = new System.Drawing.Size(65, 20);
            this.labelTrainCode.TabIndex = 1;
            this.labelTrainCode.Text = "检测车号";
            // 
            // labelTime
            // 
            this.labelTime.AutoSize = true;
            this.labelTime.Location = new System.Drawing.Point(47, 143);
            this.labelTime.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelTime.Name = "labelTime";
            this.labelTime.Size = new System.Drawing.Size(65, 20);
            this.labelTime.TabIndex = 1;
            this.labelTime.Text = "检测时间";
            // 
            // labelDate
            // 
            this.labelDate.AutoSize = true;
            this.labelDate.Location = new System.Drawing.Point(47, 107);
            this.labelDate.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelDate.Name = "labelDate";
            this.labelDate.Size = new System.Drawing.Size(65, 20);
            this.labelDate.TabIndex = 1;
            this.labelDate.Text = "检测日期";
            // 
            // labelLineDir
            // 
            this.labelLineDir.AutoSize = true;
            this.labelLineDir.Location = new System.Drawing.Point(75, 67);
            this.labelLineDir.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelLineDir.Name = "labelLineDir";
            this.labelLineDir.Size = new System.Drawing.Size(37, 20);
            this.labelLineDir.TabIndex = 1;
            this.labelLineDir.Text = "行别";
            // 
            // labelLineName
            // 
            this.labelLineName.AutoSize = true;
            this.labelLineName.Location = new System.Drawing.Point(47, 29);
            this.labelLineName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelLineName.Name = "labelLineName";
            this.labelLineName.Size = new System.Drawing.Size(65, 20);
            this.labelLineName.TabIndex = 1;
            this.labelLineName.Text = "线路名称";
            // 
            // comboBoxKmInc
            // 
            this.comboBoxKmInc.FormattingEnabled = true;
            this.comboBoxKmInc.Items.AddRange(new object[] {
            "增",
            "减"});
            this.comboBoxKmInc.Location = new System.Drawing.Point(360, 174);
            this.comboBoxKmInc.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.comboBoxKmInc.Name = "comboBoxKmInc";
            this.comboBoxKmInc.Size = new System.Drawing.Size(122, 28);
            this.comboBoxKmInc.TabIndex = 8;
            this.comboBoxKmInc.SelectedIndexChanged += new System.EventHandler(this.comboBoxKmInc_SelectedIndexChanged);
            // 
            // comboBoxRunDir
            // 
            this.comboBoxRunDir.FormattingEnabled = true;
            this.comboBoxRunDir.Items.AddRange(new object[] {
            "正",
            "反"});
            this.comboBoxRunDir.Location = new System.Drawing.Point(360, 136);
            this.comboBoxRunDir.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.comboBoxRunDir.Name = "comboBoxRunDir";
            this.comboBoxRunDir.Size = new System.Drawing.Size(122, 28);
            this.comboBoxRunDir.TabIndex = 7;
            this.comboBoxRunDir.SelectedIndexChanged += new System.EventHandler(this.comboBoxRunDir_SelectedIndexChanged);
            // 
            // comboBoxTrainCode
            // 
            this.comboBoxTrainCode.FormattingEnabled = true;
            this.comboBoxTrainCode.Location = new System.Drawing.Point(360, 26);
            this.comboBoxTrainCode.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.comboBoxTrainCode.Name = "comboBoxTrainCode";
            this.comboBoxTrainCode.Size = new System.Drawing.Size(122, 28);
            this.comboBoxTrainCode.TabIndex = 4;
            // 
            // comboBoxLineDir
            // 
            this.comboBoxLineDir.FormattingEnabled = true;
            this.comboBoxLineDir.Items.AddRange(new object[] {
            "上",
            "下",
            "单"});
            this.comboBoxLineDir.Location = new System.Drawing.Point(123, 64);
            this.comboBoxLineDir.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.comboBoxLineDir.Name = "comboBoxLineDir";
            this.comboBoxLineDir.Size = new System.Drawing.Size(122, 28);
            this.comboBoxLineDir.TabIndex = 1;
            this.comboBoxLineDir.SelectedIndexChanged += new System.EventHandler(this.comboBoxLineDir_SelectedIndexChanged);
            // 
            // comboBoxLineName
            // 
            this.comboBoxLineName.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.comboBoxLineName.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.comboBoxLineName.FormattingEnabled = true;
            this.comboBoxLineName.Location = new System.Drawing.Point(123, 26);
            this.comboBoxLineName.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.comboBoxLineName.Name = "comboBoxLineName";
            this.comboBoxLineName.Size = new System.Drawing.Size(122, 28);
            this.comboBoxLineName.TabIndex = 0;
            this.comboBoxLineName.SelectedIndexChanged += new System.EventHandler(this.comboBoxLineName_SelectedIndexChanged);
            this.comboBoxLineName.TextChanged += new System.EventHandler(this.comboBoxLineName_TextChanged);
            // 
            // buttonConvertData
            // 
            this.buttonConvertData.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.buttonConvertData.Location = new System.Drawing.Point(373, 351);
            this.buttonConvertData.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.buttonConvertData.Name = "buttonConvertData";
            this.buttonConvertData.Size = new System.Drawing.Size(134, 64);
            this.buttonConvertData.TabIndex = 9;
            this.buttonConvertData.Text = "转换为标准CIT文件";
            this.buttonConvertData.UseVisualStyleBackColor = true;
            this.buttonConvertData.Click += new System.EventHandler(this.buttonConvertData_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.Filter = "GEO波形文件|*.geo";
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button1.Location = new System.Drawing.Point(272, 3);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 27);
            this.button1.TabIndex = 10;
            this.button1.Text = "浏览";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBoxIICFilePath
            // 
            this.textBoxIICFilePath.ForeColor = System.Drawing.Color.Blue;
            this.textBoxIICFilePath.Location = new System.Drawing.Point(9, 32);
            this.textBoxIICFilePath.Name = "textBoxIICFilePath";
            this.textBoxIICFilePath.Size = new System.Drawing.Size(338, 26);
            this.textBoxIICFilePath.TabIndex = 11;
            // 
            // panelIIC
            // 
            this.panelIIC.Controls.Add(this.label1);
            this.panelIIC.Controls.Add(this.button1);
            this.panelIIC.Controls.Add(this.textBoxIICFilePath);
            this.panelIIC.Enabled = false;
            this.panelIIC.Location = new System.Drawing.Point(12, 351);
            this.panelIIC.Name = "panelIIC";
            this.panelIIC.Size = new System.Drawing.Size(350, 62);
            this.panelIIC.TabIndex = 12;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 20);
            this.label1.TabIndex = 12;
            this.label1.Text = "IIC文件：";
            // 
            // WaveConvertForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(517, 419);
            this.Controls.Add(this.panelIIC);
            this.Controls.Add(this.buttonConvertData);
            this.Controls.Add(this.groupBoxCitHeadEditer);
            this.Controls.Add(this.groupBoxGeoSelect);
            this.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "WaveConvertForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "GEO波形转换";
            this.groupBoxGeoSelect.ResumeLayout(false);
            this.groupBoxGeoSelect.PerformLayout();
            this.groupBoxCitHeadEditer.ResumeLayout(false);
            this.groupBoxCitHeadEditer.PerformLayout();
            this.panelIIC.ResumeLayout(false);
            this.panelIIC.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxGeoSelect;
        private System.Windows.Forms.TextBox textBoxGeoPath;
        private System.Windows.Forms.Button btnGeoSelect;
        private System.Windows.Forms.GroupBox groupBoxCitHeadEditer;
        private System.Windows.Forms.Button buttonConvertData;
        private System.Windows.Forms.TextBox textBoxEndPos;
        private System.Windows.Forms.TextBox textBoxStartPos;
        private System.Windows.Forms.DateTimePicker dateTimePickerTime;
        private System.Windows.Forms.DateTimePicker dateTimePickerData;
        private System.Windows.Forms.Label labelKmInc;
        private System.Windows.Forms.Label labelRunDir;
        private System.Windows.Forms.Label labelEndPos;
        private System.Windows.Forms.Label labelStartPos;
        private System.Windows.Forms.Label labelTrainCode;
        private System.Windows.Forms.Label labelTime;
        private System.Windows.Forms.Label labelDate;
        private System.Windows.Forms.Label labelLineDir;
        private System.Windows.Forms.Label labelLineName;
        private System.Windows.Forms.ComboBox comboBoxKmInc;
        private System.Windows.Forms.ComboBox comboBoxRunDir;
        private System.Windows.Forms.ComboBox comboBoxTrainCode;
        private System.Windows.Forms.ComboBox comboBoxLineDir;
        private System.Windows.Forms.ComboBox comboBoxLineName;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBoxIICFilePath;
        private System.Windows.Forms.Panel panelIIC;
        private System.Windows.Forms.Label label1;
    }
}