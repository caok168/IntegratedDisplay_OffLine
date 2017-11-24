namespace InvalidDataProcessing
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.PathTextBox1 = new System.Windows.Forms.TextBox();
            this.BroButton1 = new System.Windows.Forms.Button();
            this.FilesListView1 = new System.Windows.Forms.ListView();
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader8 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader10 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader11 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader9 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.OpenButton1 = new System.Windows.Forms.Button();
            this.InfoLabel1 = new System.Windows.Forms.Label();
            this.IncludeSubFolderCheckBox1 = new System.Windows.Forms.CheckBox();
            this.checkedListBox1 = new System.Windows.Forms.CheckedListBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.InfoLabel2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // PathTextBox1
            // 
            this.PathTextBox1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.PathTextBox1.Location = new System.Drawing.Point(32, 19);
            this.PathTextBox1.Name = "PathTextBox1";
            this.PathTextBox1.ReadOnly = true;
            this.PathTextBox1.Size = new System.Drawing.Size(525, 21);
            this.PathTextBox1.TabIndex = 20;
            // 
            // BroButton1
            // 
            this.BroButton1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.BroButton1.Location = new System.Drawing.Point(573, 18);
            this.BroButton1.Name = "BroButton1";
            this.BroButton1.Size = new System.Drawing.Size(81, 21);
            this.BroButton1.TabIndex = 21;
            this.BroButton1.Text = "浏览文件夹";
            this.BroButton1.UseVisualStyleBackColor = true;
            this.BroButton1.Click += new System.EventHandler(this.BroButton1_Click);
            // 
            // FilesListView1
            // 
            this.FilesListView1.CheckBoxes = true;
            this.FilesListView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader4,
            this.columnHeader5,
            this.columnHeader6,
            this.columnHeader8,
            this.columnHeader7,
            this.columnHeader3,
            this.columnHeader10,
            this.columnHeader11,
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader9});
            this.FilesListView1.FullRowSelect = true;
            this.FilesListView1.GridLines = true;
            this.FilesListView1.Location = new System.Drawing.Point(11, 67);
            this.FilesListView1.MultiSelect = false;
            this.FilesListView1.Name = "FilesListView1";
            this.FilesListView1.Size = new System.Drawing.Size(722, 468);
            this.FilesListView1.TabIndex = 36;
            this.FilesListView1.UseCompatibleStateImageBehavior = false;
            this.FilesListView1.View = System.Windows.Forms.View.Details;
            this.FilesListView1.SelectedIndexChanged += new System.EventHandler(this.ListView_SelectedIndexChanged);
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "线路名";
            this.columnHeader4.Width = 78;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "线路代码";
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "行别";
            this.columnHeader6.Width = 50;
            // 
            // columnHeader8
            // 
            this.columnHeader8.Text = "增减里程";
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "检测日期";
            this.columnHeader7.Width = 100;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "检测时间";
            this.columnHeader3.Width = 100;
            // 
            // columnHeader10
            // 
            this.columnHeader10.Text = "检测车号";
            this.columnHeader10.Width = 78;
            // 
            // columnHeader11
            // 
            this.columnHeader11.Text = "通道数";
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "原始文件名";
            this.columnHeader1.Width = 236;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "大小(B)";
            this.columnHeader2.Width = 100;
            // 
            // columnHeader9
            // 
            this.columnHeader9.Text = "原始路径";
            this.columnHeader9.Width = 300;
            // 
            // OpenButton1
            // 
            this.OpenButton1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.OpenButton1.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OpenButton1.Location = new System.Drawing.Point(761, 19);
            this.OpenButton1.Name = "OpenButton1";
            this.OpenButton1.Size = new System.Drawing.Size(81, 21);
            this.OpenButton1.TabIndex = 37;
            this.OpenButton1.Text = "GO";
            this.OpenButton1.UseVisualStyleBackColor = true;
            this.OpenButton1.Click += new System.EventHandler(this.OpenButton1_Click);
            // 
            // InfoLabel1
            // 
            this.InfoLabel1.Location = new System.Drawing.Point(257, 203);
            this.InfoLabel1.Name = "InfoLabel1";
            this.InfoLabel1.Size = new System.Drawing.Size(194, 23);
            this.InfoLabel1.TabIndex = 38;
            this.InfoLabel1.Text = "正在获取,请稍候...";
            this.InfoLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.InfoLabel1.Visible = false;
            // 
            // IncludeSubFolderCheckBox1
            // 
            this.IncludeSubFolderCheckBox1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.IncludeSubFolderCheckBox1.Location = new System.Drawing.Point(665, 19);
            this.IncludeSubFolderCheckBox1.Name = "IncludeSubFolderCheckBox1";
            this.IncludeSubFolderCheckBox1.Size = new System.Drawing.Size(85, 24);
            this.IncludeSubFolderCheckBox1.TabIndex = 39;
            this.IncludeSubFolderCheckBox1.Text = "包括子目录";
            this.IncludeSubFolderCheckBox1.UseVisualStyleBackColor = true;
            // 
            // checkedListBox1
            // 
            this.checkedListBox1.CheckOnClick = true;
            this.checkedListBox1.FormattingEnabled = true;
            this.checkedListBox1.Location = new System.Drawing.Point(740, 65);
            this.checkedListBox1.Name = "checkedListBox1";
            this.checkedListBox1.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.checkedListBox1.Size = new System.Drawing.Size(236, 468);
            this.checkedListBox1.TabIndex = 40;
            // 
            // checkBox1
            // 
            this.checkBox1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.checkBox1.Location = new System.Drawing.Point(871, 8);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(85, 24);
            this.checkBox1.TabIndex = 41;
            this.checkBox1.Text = "全选";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // checkBox2
            // 
            this.checkBox2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.checkBox2.Location = new System.Drawing.Point(871, 30);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(85, 24);
            this.checkBox2.TabIndex = 42;
            this.checkBox2.Text = "反选";
            this.checkBox2.UseVisualStyleBackColor = true;
            this.checkBox2.CheckedChanged += new System.EventHandler(this.checkBox2_CheckedChanged);
            // 
            // InfoLabel2
            // 
            this.InfoLabel2.Location = new System.Drawing.Point(396, 261);
            this.InfoLabel2.Name = "InfoLabel2";
            this.InfoLabel2.Size = new System.Drawing.Size(226, 64);
            this.InfoLabel2.TabIndex = 43;
            this.InfoLabel2.Text = "正在写入idf";
            this.InfoLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.InfoLabel2.Visible = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(987, 545);
            this.Controls.Add(this.InfoLabel2);
            this.Controls.Add(this.checkBox2);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.checkedListBox1);
            this.Controls.Add(this.IncludeSubFolderCheckBox1);
            this.Controls.Add(this.InfoLabel1);
            this.Controls.Add(this.OpenButton1);
            this.Controls.Add(this.FilesListView1);
            this.Controls.Add(this.PathTextBox1);
            this.Controls.Add(this.BroButton1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox PathTextBox1;
        private System.Windows.Forms.Button BroButton1;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
        private System.Windows.Forms.ListView FilesListView1;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.ColumnHeader columnHeader8;
        private System.Windows.Forms.ColumnHeader columnHeader7;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader10;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader9;
        private System.Windows.Forms.Button OpenButton1;
        private System.Windows.Forms.Label InfoLabel1;
        private System.Windows.Forms.CheckBox IncludeSubFolderCheckBox1;
        private System.Windows.Forms.CheckedListBox checkedListBox1;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.ColumnHeader columnHeader11;
        private System.Windows.Forms.Label InfoLabel2;
    }
}

