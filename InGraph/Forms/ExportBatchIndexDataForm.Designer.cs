namespace InGraph.Forms
{
    partial class ExportBatchIndexDataForm
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
            this.buttonExportData = new System.Windows.Forms.Button();
            this.buttonBack = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.checkedListBox1 = new System.Windows.Forms.CheckedListBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.textBoxMileFile1 = new System.Windows.Forms.TextBox();
            this.buttonSelectFile = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.buttonSeeDirectory = new System.Windows.Forms.Button();
            this.textBoxPath1 = new System.Windows.Forms.TextBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonExportData
            // 
            this.buttonExportData.Location = new System.Drawing.Point(472, 301);
            this.buttonExportData.Name = "buttonExportData";
            this.buttonExportData.Size = new System.Drawing.Size(75, 23);
            this.buttonExportData.TabIndex = 0;
            this.buttonExportData.Text = "导出(&E)";
            this.buttonExportData.UseVisualStyleBackColor = true;
            this.buttonExportData.Click += new System.EventHandler(this.buttonExportData_Click);
            // 
            // buttonBack
            // 
            this.buttonBack.Location = new System.Drawing.Point(588, 301);
            this.buttonBack.Name = "buttonBack";
            this.buttonBack.Size = new System.Drawing.Size(75, 23);
            this.buttonBack.TabIndex = 1;
            this.buttonBack.Text = "返回(&B)";
            this.buttonBack.UseVisualStyleBackColor = true;
            this.buttonBack.Click += new System.EventHandler(this.buttonBack_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.checkedListBox1);
            this.groupBox1.Location = new System.Drawing.Point(12, 139);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(672, 156);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "导出通道选择";
            // 
            // checkedListBox1
            // 
            this.checkedListBox1.CheckOnClick = true;
            this.checkedListBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkedListBox1.FormattingEnabled = true;
            this.checkedListBox1.Items.AddRange(new object[] {
            "L_Prof_SC",
            "R_Prof_SC",
            "L_Align_SC",
            "R_Align_SC",
            "Gage",
            "Superelevation",
            "Crosslevel",
            "Short_Twist",
            "Curvature",
            "LACC",
            "VACC",
            "Speed",
            "ALD",
            "L_Prof_SC_70",
            "R_Prof_SC_70",
            "L_Align_SC_70",
            "R_Align_SC_70",
            "L_Prof_SC_120",
            "R_Prof_SC_120",
            "L_Align_SC_120",
            "R_Align_SC_120",
            "Gage_Rate",
            "Curvature_Rate",
            "Lacc_Rate",
            "Gage_L",
            "Gage_R",
            "G_LIrregular",
            "G_RIrregular"});
            this.checkedListBox1.Location = new System.Drawing.Point(3, 17);
            this.checkedListBox1.MultiColumn = true;
            this.checkedListBox1.Name = "checkedListBox1";
            this.checkedListBox1.ScrollAlwaysVisible = true;
            this.checkedListBox1.Size = new System.Drawing.Size(666, 136);
            this.checkedListBox1.TabIndex = 3;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.textBoxMileFile1);
            this.groupBox2.Controls.Add(this.buttonSelectFile);
            this.groupBox2.Location = new System.Drawing.Point(12, 72);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(672, 61);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "导出里程范围";
            // 
            // textBoxMileFile1
            // 
            this.textBoxMileFile1.Location = new System.Drawing.Point(17, 22);
            this.textBoxMileFile1.Name = "textBoxMileFile1";
            this.textBoxMileFile1.Size = new System.Drawing.Size(540, 21);
            this.textBoxMileFile1.TabIndex = 3;
            // 
            // buttonSelectFile
            // 
            this.buttonSelectFile.Location = new System.Drawing.Point(576, 20);
            this.buttonSelectFile.Name = "buttonSelectFile";
            this.buttonSelectFile.Size = new System.Drawing.Size(75, 23);
            this.buttonSelectFile.TabIndex = 2;
            this.buttonSelectFile.Text = "文件选择";
            this.buttonSelectFile.UseVisualStyleBackColor = true;
            this.buttonSelectFile.Click += new System.EventHandler(this.buttonSelectFile_Click);
            // 
            // folderBrowserDialog1
            // 
            this.folderBrowserDialog1.RootFolder = System.Environment.SpecialFolder.MyComputer;
            this.folderBrowserDialog1.ShowNewFolderButton = false;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.buttonSeeDirectory);
            this.groupBox3.Controls.Add(this.textBoxPath1);
            this.groupBox3.Location = new System.Drawing.Point(12, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(672, 54);
            this.groupBox3.TabIndex = 5;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "待处理数据目录选择";
            // 
            // buttonSeeDirectory
            // 
            this.buttonSeeDirectory.Location = new System.Drawing.Point(576, 19);
            this.buttonSeeDirectory.Name = "buttonSeeDirectory";
            this.buttonSeeDirectory.Size = new System.Drawing.Size(75, 23);
            this.buttonSeeDirectory.TabIndex = 1;
            this.buttonSeeDirectory.Text = "目录浏览";
            this.buttonSeeDirectory.UseVisualStyleBackColor = true;
            this.buttonSeeDirectory.Click += new System.EventHandler(this.buttonSeeDirectory_Click);
            // 
            // textBoxPath1
            // 
            this.textBoxPath1.Location = new System.Drawing.Point(17, 21);
            this.textBoxPath1.Name = "textBoxPath1";
            this.textBoxPath1.Size = new System.Drawing.Size(540, 21);
            this.textBoxPath1.TabIndex = 0;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.Filter = "txt 文件|*.txt";
            // 
            // ExportBatchIndexDataForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(696, 336);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.buttonBack);
            this.Controls.Add(this.buttonExportData);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ExportBatchIndexDataForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "批量导出索引后数据";
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonExportData;
        private System.Windows.Forms.Button buttonBack;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckedListBox checkedListBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button buttonSeeDirectory;
        private System.Windows.Forms.TextBox textBoxPath1;
        private System.Windows.Forms.Button buttonSelectFile;
        private System.Windows.Forms.TextBox textBoxMileFile1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
    }
}