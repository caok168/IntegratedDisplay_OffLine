namespace InGraph.Forms
{
    partial class DataStatisticsForm
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.buttonExportCsv_fuzhi = new System.Windows.Forms.Button();
            this.labelCalSum = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.buttonOK_fuzhi = new System.Windows.Forms.Button();
            this.dataGridView_fuzhi = new System.Windows.Forms.DataGridView();
            this.ColumnCalItems = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_L_Prof = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_R_Prof = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_L_Align = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_R_Align = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_Gage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_CrossLevel = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_Twist = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.textBoxEndMile = new System.Windows.Forms.TextBox();
            this.textBoxStartMile = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label10 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_fuzhi)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(0, 61);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(5);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.buttonExportCsv_fuzhi);
            this.splitContainer1.Panel1.Controls.Add(this.labelCalSum);
            this.splitContainer1.Panel1.Controls.Add(this.label3);
            this.splitContainer1.Panel1.Controls.Add(this.buttonOK_fuzhi);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.dataGridView_fuzhi);
            this.splitContainer1.Size = new System.Drawing.Size(686, 183);
            this.splitContainer1.SplitterDistance = 42;
            this.splitContainer1.SplitterWidth = 7;
            this.splitContainer1.TabIndex = 0;
            // 
            // buttonExportCsv_fuzhi
            // 
            this.buttonExportCsv_fuzhi.Location = new System.Drawing.Point(557, 4);
            this.buttonExportCsv_fuzhi.Margin = new System.Windows.Forms.Padding(5);
            this.buttonExportCsv_fuzhi.Name = "buttonExportCsv_fuzhi";
            this.buttonExportCsv_fuzhi.Size = new System.Drawing.Size(125, 31);
            this.buttonExportCsv_fuzhi.TabIndex = 5;
            this.buttonExportCsv_fuzhi.Text = "导出excel";
            this.buttonExportCsv_fuzhi.UseVisualStyleBackColor = true;
            this.buttonExportCsv_fuzhi.Click += new System.EventHandler(this.buttonExportCsv_fuzhi_Click);
            // 
            // labelCalSum
            // 
            this.labelCalSum.AutoSize = true;
            this.labelCalSum.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelCalSum.ForeColor = System.Drawing.Color.Blue;
            this.labelCalSum.Location = new System.Drawing.Point(125, 9);
            this.labelCalSum.Name = "labelCalSum";
            this.labelCalSum.Size = new System.Drawing.Size(57, 23);
            this.labelCalSum.TabIndex = 4;
            this.labelCalSum.Text = "         ";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 10);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(106, 21);
            this.label3.TabIndex = 3;
            this.label3.Text = "标准差之和：";
            // 
            // buttonOK_fuzhi
            // 
            this.buttonOK_fuzhi.Location = new System.Drawing.Point(422, 5);
            this.buttonOK_fuzhi.Margin = new System.Windows.Forms.Padding(5);
            this.buttonOK_fuzhi.Name = "buttonOK_fuzhi";
            this.buttonOK_fuzhi.Size = new System.Drawing.Size(125, 31);
            this.buttonOK_fuzhi.TabIndex = 2;
            this.buttonOK_fuzhi.Text = "确定";
            this.buttonOK_fuzhi.UseVisualStyleBackColor = true;
            this.buttonOK_fuzhi.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // dataGridView_fuzhi
            // 
            this.dataGridView_fuzhi.AllowUserToAddRows = false;
            this.dataGridView_fuzhi.AllowUserToDeleteRows = false;
            this.dataGridView_fuzhi.AllowUserToResizeColumns = false;
            this.dataGridView_fuzhi.AllowUserToResizeRows = false;
            this.dataGridView_fuzhi.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_fuzhi.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnCalItems,
            this.Column_L_Prof,
            this.Column_R_Prof,
            this.Column_L_Align,
            this.Column_R_Align,
            this.Column_Gage,
            this.Column_CrossLevel,
            this.Column_Twist});
            this.dataGridView_fuzhi.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView_fuzhi.Location = new System.Drawing.Point(0, 0);
            this.dataGridView_fuzhi.Name = "dataGridView_fuzhi";
            this.dataGridView_fuzhi.ReadOnly = true;
            this.dataGridView_fuzhi.RowHeadersVisible = false;
            this.dataGridView_fuzhi.RowTemplate.Height = 23;
            this.dataGridView_fuzhi.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView_fuzhi.Size = new System.Drawing.Size(686, 134);
            this.dataGridView_fuzhi.TabIndex = 0;
            // 
            // ColumnCalItems
            // 
            this.ColumnCalItems.HeaderText = "统计项目";
            this.ColumnCalItems.Name = "ColumnCalItems";
            this.ColumnCalItems.ReadOnly = true;
            // 
            // Column_L_Prof
            // 
            this.Column_L_Prof.HeaderText = "左高低";
            this.Column_L_Prof.Name = "Column_L_Prof";
            this.Column_L_Prof.ReadOnly = true;
            this.Column_L_Prof.Width = 83;
            // 
            // Column_R_Prof
            // 
            this.Column_R_Prof.HeaderText = "右高低";
            this.Column_R_Prof.Name = "Column_R_Prof";
            this.Column_R_Prof.ReadOnly = true;
            this.Column_R_Prof.Width = 83;
            // 
            // Column_L_Align
            // 
            this.Column_L_Align.HeaderText = "左轨向";
            this.Column_L_Align.Name = "Column_L_Align";
            this.Column_L_Align.ReadOnly = true;
            this.Column_L_Align.Width = 83;
            // 
            // Column_R_Align
            // 
            this.Column_R_Align.HeaderText = "右轨向";
            this.Column_R_Align.Name = "Column_R_Align";
            this.Column_R_Align.ReadOnly = true;
            this.Column_R_Align.Width = 83;
            // 
            // Column_Gage
            // 
            this.Column_Gage.HeaderText = "轨距";
            this.Column_Gage.Name = "Column_Gage";
            this.Column_Gage.ReadOnly = true;
            this.Column_Gage.Width = 83;
            // 
            // Column_CrossLevel
            // 
            this.Column_CrossLevel.HeaderText = "水平";
            this.Column_CrossLevel.Name = "Column_CrossLevel";
            this.Column_CrossLevel.ReadOnly = true;
            this.Column_CrossLevel.Width = 83;
            // 
            // Column_Twist
            // 
            this.Column_Twist.HeaderText = "三角坑";
            this.Column_Twist.Name = "Column_Twist";
            this.Column_Twist.ReadOnly = true;
            this.Column_Twist.Width = 83;
            // 
            // textBoxEndMile
            // 
            this.textBoxEndMile.Location = new System.Drawing.Point(301, 8);
            this.textBoxEndMile.Margin = new System.Windows.Forms.Padding(5);
            this.textBoxEndMile.Name = "textBoxEndMile";
            this.textBoxEndMile.ReadOnly = true;
            this.textBoxEndMile.Size = new System.Drawing.Size(89, 29);
            this.textBoxEndMile.TabIndex = 1;
            // 
            // textBoxStartMile
            // 
            this.textBoxStartMile.Location = new System.Drawing.Point(74, 8);
            this.textBoxStartMile.Margin = new System.Windows.Forms.Padding(5);
            this.textBoxStartMile.Name = "textBoxStartMile";
            this.textBoxStartMile.ReadOnly = true;
            this.textBoxStartMile.Size = new System.Drawing.Size(89, 29);
            this.textBoxStartMile.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(233, 12);
            this.label2.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 21);
            this.label2.TabIndex = 0;
            this.label2.Text = "终点：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 11);
            this.label1.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 21);
            this.label1.TabIndex = 0;
            this.label1.Text = "起点：";
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label10);
            this.panel2.Controls.Add(this.label7);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.textBoxStartMile);
            this.panel2.Controls.Add(this.textBoxEndMile);
            this.panel2.Location = new System.Drawing.Point(101, 12);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(451, 42);
            this.panel2.TabIndex = 1;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(398, 12);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(42, 21);
            this.label10.TabIndex = 2;
            this.label10.Text = "公里";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(170, 12);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(42, 21);
            this.label7.TabIndex = 2;
            this.label7.Text = "公里";
            // 
            // splitter1
            // 
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitter1.Location = new System.Drawing.Point(0, 0);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(686, 61);
            this.splitter1.TabIndex = 4;
            this.splitter1.TabStop = false;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.HeaderText = "统计项目";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.HeaderText = "左高低";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.Width = 83;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.HeaderText = "右高低";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.Width = 83;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.HeaderText = "左轨向";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.Width = 83;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.HeaderText = "右轨向";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.Width = 83;
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.HeaderText = "轨距";
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            this.dataGridViewTextBoxColumn6.Width = 83;
            // 
            // dataGridViewTextBoxColumn7
            // 
            this.dataGridViewTextBoxColumn7.HeaderText = "水平";
            this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            this.dataGridViewTextBoxColumn7.Width = 83;
            // 
            // dataGridViewTextBoxColumn8
            // 
            this.dataGridViewTextBoxColumn8.HeaderText = "三角坑";
            this.dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
            this.dataGridViewTextBoxColumn8.Width = 83;
            // 
            // DataStatisticsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(686, 244);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.splitter1);
            this.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(5);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DataStatisticsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "DataStatisticsForm";
            this.Load += new System.EventHandler(this.DataStatisticsForm_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_fuzhi)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button buttonOK_fuzhi;
        protected internal System.Windows.Forms.TextBox textBoxEndMile;
        protected internal System.Windows.Forms.TextBox textBoxStartMile;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dataGridView_fuzhi;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Label labelCalSum;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnCalItems;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_L_Prof;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_R_Prof;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_L_Align;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_R_Align;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_Gage;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_CrossLevel;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_Twist;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.Button buttonExportCsv_fuzhi;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;
    }
}