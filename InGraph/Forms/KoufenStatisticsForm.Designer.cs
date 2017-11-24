namespace InGraph.Forms
{
    partial class KoufenStatisticsForm
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.buttonExportCsv_koufen = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.labelSeveritySum = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.buttonSelectIIC = new System.Windows.Forms.Button();
            this.buttonOK_koufen = new System.Windows.Forms.Button();
            this.dataGridView_koufen = new System.Windows.Forms.DataGridView();
            this.ColumnCalItems = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_Prof_kf = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_Align_kf = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_GAUGE_kf = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_CrossLevel_kf = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_Twist_kf = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_LAT_ACCEL_kf = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_VERT_ACCEL_kf = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_Prof_70M_kf = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_Align_70M_kf = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_CURVATURE_RATE_kf = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_GAUGE_RATE_kf = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_LAT_ACCEL_RATE_kf = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_IRREGULAR_kf = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_LATACCEL_NOCUR_kf = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label10 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxStartMile = new System.Windows.Forms.TextBox();
            this.textBoxEndMile = new System.Windows.Forms.TextBox();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_koufen)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.splitContainer2);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.splitter1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(770, 327);
            this.panel1.TabIndex = 0;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer2.Location = new System.Drawing.Point(0, 54);
            this.splitContainer2.Margin = new System.Windows.Forms.Padding(5);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.buttonExportCsv_koufen);
            this.splitContainer2.Panel1.Controls.Add(this.textBox1);
            this.splitContainer2.Panel1.Controls.Add(this.labelSeveritySum);
            this.splitContainer2.Panel1.Controls.Add(this.label5);
            this.splitContainer2.Panel1.Controls.Add(this.buttonSelectIIC);
            this.splitContainer2.Panel1.Controls.Add(this.buttonOK_koufen);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.dataGridView_koufen);
            this.splitContainer2.Size = new System.Drawing.Size(770, 273);
            this.splitContainer2.SplitterDistance = 76;
            this.splitContainer2.SplitterWidth = 7;
            this.splitContainer2.TabIndex = 0;
            // 
            // buttonExportCsv_koufen
            // 
            this.buttonExportCsv_koufen.Location = new System.Drawing.Point(639, 40);
            this.buttonExportCsv_koufen.Margin = new System.Windows.Forms.Padding(5);
            this.buttonExportCsv_koufen.Name = "buttonExportCsv_koufen";
            this.buttonExportCsv_koufen.Size = new System.Drawing.Size(125, 31);
            this.buttonExportCsv_koufen.TabIndex = 6;
            this.buttonExportCsv_koufen.Text = "导出excel";
            this.buttonExportCsv_koufen.UseVisualStyleBackColor = true;
            this.buttonExportCsv_koufen.Click += new System.EventHandler(this.buttonExportCsv_koufen_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(19, 8);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(607, 29);
            this.textBox1.TabIndex = 5;
            // 
            // labelSeveritySum
            // 
            this.labelSeveritySum.AutoSize = true;
            this.labelSeveritySum.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelSeveritySum.ForeColor = System.Drawing.Color.Blue;
            this.labelSeveritySum.Location = new System.Drawing.Point(125, 45);
            this.labelSeveritySum.Name = "labelSeveritySum";
            this.labelSeveritySum.Size = new System.Drawing.Size(57, 23);
            this.labelSeveritySum.TabIndex = 4;
            this.labelSeveritySum.Text = "         ";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(45, 47);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(74, 21);
            this.label5.TabIndex = 3;
            this.label5.Text = "总扣分：";
            // 
            // buttonSelectIIC
            // 
            this.buttonSelectIIC.Location = new System.Drawing.Point(639, 6);
            this.buttonSelectIIC.Margin = new System.Windows.Forms.Padding(5);
            this.buttonSelectIIC.Name = "buttonSelectIIC";
            this.buttonSelectIIC.Size = new System.Drawing.Size(125, 31);
            this.buttonSelectIIC.TabIndex = 2;
            this.buttonSelectIIC.Text = "选择IIC";
            this.buttonSelectIIC.UseVisualStyleBackColor = true;
            this.buttonSelectIIC.Click += new System.EventHandler(this.buttonSelectIIC_Click);
            // 
            // buttonOK_koufen
            // 
            this.buttonOK_koufen.Location = new System.Drawing.Point(501, 40);
            this.buttonOK_koufen.Margin = new System.Windows.Forms.Padding(5);
            this.buttonOK_koufen.Name = "buttonOK_koufen";
            this.buttonOK_koufen.Size = new System.Drawing.Size(125, 31);
            this.buttonOK_koufen.TabIndex = 2;
            this.buttonOK_koufen.Text = "确定";
            this.buttonOK_koufen.UseVisualStyleBackColor = true;
            this.buttonOK_koufen.Click += new System.EventHandler(this.buttonOK_koufen_Click);
            // 
            // dataGridView_koufen
            // 
            this.dataGridView_koufen.AllowUserToAddRows = false;
            this.dataGridView_koufen.AllowUserToDeleteRows = false;
            this.dataGridView_koufen.AllowUserToResizeColumns = false;
            this.dataGridView_koufen.AllowUserToResizeRows = false;
            this.dataGridView_koufen.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_koufen.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnCalItems,
            this.Column_Prof_kf,
            this.Column_Align_kf,
            this.Column_GAUGE_kf,
            this.Column_CrossLevel_kf,
            this.Column_Twist_kf,
            this.Column_LAT_ACCEL_kf,
            this.Column_VERT_ACCEL_kf,
            this.Column_Prof_70M_kf,
            this.Column_Align_70M_kf,
            this.Column_CURVATURE_RATE_kf,
            this.Column_GAUGE_RATE_kf,
            this.Column_LAT_ACCEL_RATE_kf,
            this.Column_IRREGULAR_kf,
            this.Column_LATACCEL_NOCUR_kf});
            this.dataGridView_koufen.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView_koufen.Location = new System.Drawing.Point(0, 0);
            this.dataGridView_koufen.Name = "dataGridView_koufen";
            this.dataGridView_koufen.ReadOnly = true;
            this.dataGridView_koufen.RowHeadersVisible = false;
            this.dataGridView_koufen.RowTemplate.Height = 23;
            this.dataGridView_koufen.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView_koufen.Size = new System.Drawing.Size(770, 190);
            this.dataGridView_koufen.TabIndex = 0;
            // 
            // ColumnCalItems
            // 
            this.ColumnCalItems.HeaderText = "统计项目";
            this.ColumnCalItems.Name = "ColumnCalItems";
            this.ColumnCalItems.ReadOnly = true;
            // 
            // Column_Prof_kf
            // 
            this.Column_Prof_kf.HeaderText = "高低";
            this.Column_Prof_kf.Name = "Column_Prof_kf";
            this.Column_Prof_kf.ReadOnly = true;
            this.Column_Prof_kf.Width = 67;
            // 
            // Column_Align_kf
            // 
            this.Column_Align_kf.HeaderText = "轨向";
            this.Column_Align_kf.Name = "Column_Align_kf";
            this.Column_Align_kf.ReadOnly = true;
            this.Column_Align_kf.Width = 67;
            // 
            // Column_GAUGE_kf
            // 
            this.Column_GAUGE_kf.HeaderText = "轨距";
            this.Column_GAUGE_kf.Name = "Column_GAUGE_kf";
            this.Column_GAUGE_kf.ReadOnly = true;
            this.Column_GAUGE_kf.Width = 67;
            // 
            // Column_CrossLevel_kf
            // 
            this.Column_CrossLevel_kf.HeaderText = "水平";
            this.Column_CrossLevel_kf.Name = "Column_CrossLevel_kf";
            this.Column_CrossLevel_kf.ReadOnly = true;
            this.Column_CrossLevel_kf.Width = 67;
            // 
            // Column_Twist_kf
            // 
            this.Column_Twist_kf.HeaderText = "三角坑";
            this.Column_Twist_kf.Name = "Column_Twist_kf";
            this.Column_Twist_kf.ReadOnly = true;
            this.Column_Twist_kf.Width = 83;
            // 
            // Column_LAT_ACCEL_kf
            // 
            this.Column_LAT_ACCEL_kf.HeaderText = "水加";
            this.Column_LAT_ACCEL_kf.Name = "Column_LAT_ACCEL_kf";
            this.Column_LAT_ACCEL_kf.ReadOnly = true;
            this.Column_LAT_ACCEL_kf.Width = 67;
            // 
            // Column_VERT_ACCEL_kf
            // 
            this.Column_VERT_ACCEL_kf.HeaderText = "垂加";
            this.Column_VERT_ACCEL_kf.Name = "Column_VERT_ACCEL_kf";
            this.Column_VERT_ACCEL_kf.ReadOnly = true;
            this.Column_VERT_ACCEL_kf.Width = 67;
            // 
            // Column_Prof_70M_kf
            // 
            this.Column_Prof_70M_kf.HeaderText = "70米高低";
            this.Column_Prof_70M_kf.Name = "Column_Prof_70M_kf";
            this.Column_Prof_70M_kf.ReadOnly = true;
            this.Column_Prof_70M_kf.Width = 83;
            // 
            // Column_Align_70M_kf
            // 
            this.Column_Align_70M_kf.HeaderText = "70米轨向";
            this.Column_Align_70M_kf.Name = "Column_Align_70M_kf";
            this.Column_Align_70M_kf.ReadOnly = true;
            this.Column_Align_70M_kf.Width = 83;
            // 
            // Column_CURVATURE_RATE_kf
            // 
            this.Column_CURVATURE_RATE_kf.HeaderText = "曲率变化率";
            this.Column_CURVATURE_RATE_kf.Name = "Column_CURVATURE_RATE_kf";
            this.Column_CURVATURE_RATE_kf.ReadOnly = true;
            this.Column_CURVATURE_RATE_kf.Width = 83;
            // 
            // Column_GAUGE_RATE_kf
            // 
            this.Column_GAUGE_RATE_kf.HeaderText = "轨距变化率";
            this.Column_GAUGE_RATE_kf.Name = "Column_GAUGE_RATE_kf";
            this.Column_GAUGE_RATE_kf.ReadOnly = true;
            this.Column_GAUGE_RATE_kf.Width = 83;
            // 
            // Column_LAT_ACCEL_RATE_kf
            // 
            this.Column_LAT_ACCEL_RATE_kf.HeaderText = "横加变化率";
            this.Column_LAT_ACCEL_RATE_kf.Name = "Column_LAT_ACCEL_RATE_kf";
            this.Column_LAT_ACCEL_RATE_kf.ReadOnly = true;
            this.Column_LAT_ACCEL_RATE_kf.Width = 83;
            // 
            // Column_IRREGULAR_kf
            // 
            this.Column_IRREGULAR_kf.HeaderText = "复合不平顺";
            this.Column_IRREGULAR_kf.Name = "Column_IRREGULAR_kf";
            this.Column_IRREGULAR_kf.ReadOnly = true;
            this.Column_IRREGULAR_kf.Width = 83;
            // 
            // Column_LATACCEL_NOCUR_kf
            // 
            this.Column_LATACCEL_NOCUR_kf.HeaderText = "带通车体横加";
            this.Column_LATACCEL_NOCUR_kf.Name = "Column_LATACCEL_NOCUR_kf";
            this.Column_LATACCEL_NOCUR_kf.ReadOnly = true;
            this.Column_LATACCEL_NOCUR_kf.Width = 83;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label10);
            this.panel2.Controls.Add(this.label7);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.textBoxStartMile);
            this.panel2.Controls.Add(this.textBoxEndMile);
            this.panel2.Location = new System.Drawing.Point(128, 6);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(451, 42);
            this.panel2.TabIndex = 2;
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
            // textBoxStartMile
            // 
            this.textBoxStartMile.Location = new System.Drawing.Point(74, 8);
            this.textBoxStartMile.Margin = new System.Windows.Forms.Padding(5);
            this.textBoxStartMile.Name = "textBoxStartMile";
            this.textBoxStartMile.ReadOnly = true;
            this.textBoxStartMile.Size = new System.Drawing.Size(89, 29);
            this.textBoxStartMile.TabIndex = 1;
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
            // splitter1
            // 
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitter1.Location = new System.Drawing.Point(0, 0);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(770, 54);
            this.splitter1.TabIndex = 0;
            this.splitter1.TabStop = false;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.Filter = "IIC 文件|*.iic|所有文件|*.*";
            // 
            // KoufenStatisticsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(770, 327);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "KoufenStatisticsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "扣分统计";
            this.panel1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel1.PerformLayout();
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_koufen)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        protected internal System.Windows.Forms.TextBox textBoxStartMile;
        protected internal System.Windows.Forms.TextBox textBoxEndMile;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.Button buttonExportCsv_koufen;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label labelSeveritySum;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button buttonSelectIIC;
        private System.Windows.Forms.Button buttonOK_koufen;
        private System.Windows.Forms.DataGridView dataGridView_koufen;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnCalItems;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_Prof_kf;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_Align_kf;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_GAUGE_kf;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_CrossLevel_kf;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_Twist_kf;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_LAT_ACCEL_kf;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_VERT_ACCEL_kf;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_Prof_70M_kf;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_Align_70M_kf;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_CURVATURE_RATE_kf;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_GAUGE_RATE_kf;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_LAT_ACCEL_RATE_kf;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_IRREGULAR_kf;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_LATACCEL_NOCUR_kf;

    }
}