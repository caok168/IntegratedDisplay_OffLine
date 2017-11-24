namespace InGraph.Forms
{
    partial class EnergyStatisticsForm
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
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.rb_LACC = new System.Windows.Forms.RadioButton();
            this.rb_VACC = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cb_Crosslevel = new System.Windows.Forms.CheckBox();
            this.cb_Short_Twist = new System.Windows.Forms.CheckBox();
            this.cb_Superelevation = new System.Windows.Forms.CheckBox();
            this.cb_R_Align_SC = new System.Windows.Forms.CheckBox();
            this.cb_L_Align_SC = new System.Windows.Forms.CheckBox();
            this.cb_R_Prof_SC = new System.Windows.Forms.CheckBox();
            this.cb_L_Prof_SC = new System.Windows.Forms.CheckBox();
            this.textBoxSpdBand = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.buttonAvg = new System.Windows.Forms.Button();
            this.labelSpeed = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.buttonExportCsv_EnergyWeight = new System.Windows.Forms.Button();
            this.button_EnergyWeight = new System.Windows.Forms.Button();
            this.labelEnergyWeight = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.dataGridView_EnergyWeight = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label10 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxStartMile = new System.Windows.Forms.TextBox();
            this.textBoxEndMile = new System.Windows.Forms.TextBox();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_EnergyWeight)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.splitContainer3);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.splitter1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(790, 482);
            this.panel1.TabIndex = 0;
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer3.Location = new System.Drawing.Point(0, 63);
            this.splitContainer3.Name = "splitContainer3";
            this.splitContainer3.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.groupBox2);
            this.splitContainer3.Panel1.Controls.Add(this.groupBox1);
            this.splitContainer3.Panel1.Controls.Add(this.textBoxSpdBand);
            this.splitContainer3.Panel1.Controls.Add(this.label8);
            this.splitContainer3.Panel1.Controls.Add(this.buttonAvg);
            this.splitContainer3.Panel1.Controls.Add(this.labelSpeed);
            this.splitContainer3.Panel1.Controls.Add(this.label6);
            this.splitContainer3.Panel1.Controls.Add(this.buttonExportCsv_EnergyWeight);
            this.splitContainer3.Panel1.Controls.Add(this.button_EnergyWeight);
            this.splitContainer3.Panel1.Controls.Add(this.labelEnergyWeight);
            this.splitContainer3.Panel1.Controls.Add(this.label4);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.dataGridView_EnergyWeight);
            this.splitContainer3.Size = new System.Drawing.Size(790, 419);
            this.splitContainer3.SplitterDistance = 180;
            this.splitContainer3.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.rb_LACC);
            this.groupBox2.Controls.Add(this.rb_VACC);
            this.groupBox2.Location = new System.Drawing.Point(578, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(200, 84);
            this.groupBox2.TabIndex = 9;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "动态响应";
            // 
            // rb_LACC
            // 
            this.rb_LACC.AutoSize = true;
            this.rb_LACC.Location = new System.Drawing.Point(46, 49);
            this.rb_LACC.Name = "rb_LACC";
            this.rb_LACC.Size = new System.Drawing.Size(140, 25);
            this.rb_LACC.TabIndex = 0;
            this.rb_LACC.Text = "车体横向加速度";
            this.rb_LACC.UseVisualStyleBackColor = true;
            this.rb_LACC.CheckedChanged += new System.EventHandler(this.radioButton_CheckedChange);
            // 
            // rb_VACC
            // 
            this.rb_VACC.AutoSize = true;
            this.rb_VACC.Location = new System.Drawing.Point(46, 24);
            this.rb_VACC.Name = "rb_VACC";
            this.rb_VACC.Size = new System.Drawing.Size(140, 25);
            this.rb_VACC.TabIndex = 0;
            this.rb_VACC.Text = "车体垂向加速度";
            this.rb_VACC.UseVisualStyleBackColor = true;
            this.rb_VACC.CheckedChanged += new System.EventHandler(this.radioButton_CheckedChange);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cb_Crosslevel);
            this.groupBox1.Controls.Add(this.cb_Short_Twist);
            this.groupBox1.Controls.Add(this.cb_Superelevation);
            this.groupBox1.Controls.Add(this.cb_R_Align_SC);
            this.groupBox1.Controls.Add(this.cb_L_Align_SC);
            this.groupBox1.Controls.Add(this.cb_R_Prof_SC);
            this.groupBox1.Controls.Add(this.cb_L_Prof_SC);
            this.groupBox1.Location = new System.Drawing.Point(12, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(560, 84);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "几何";
            // 
            // cb_Crosslevel
            // 
            this.cb_Crosslevel.AutoSize = true;
            this.cb_Crosslevel.Location = new System.Drawing.Point(432, 22);
            this.cb_Crosslevel.Name = "cb_Crosslevel";
            this.cb_Crosslevel.Size = new System.Drawing.Size(61, 25);
            this.cb_Crosslevel.TabIndex = 0;
            this.cb_Crosslevel.Text = "水平";
            this.cb_Crosslevel.UseVisualStyleBackColor = true;
            // 
            // cb_Short_Twist
            // 
            this.cb_Short_Twist.AutoSize = true;
            this.cb_Short_Twist.Location = new System.Drawing.Point(316, 53);
            this.cb_Short_Twist.Name = "cb_Short_Twist";
            this.cb_Short_Twist.Size = new System.Drawing.Size(77, 25);
            this.cb_Short_Twist.TabIndex = 1;
            this.cb_Short_Twist.Text = "三角坑";
            this.cb_Short_Twist.UseVisualStyleBackColor = true;
            // 
            // cb_Superelevation
            // 
            this.cb_Superelevation.AutoSize = true;
            this.cb_Superelevation.Location = new System.Drawing.Point(316, 22);
            this.cb_Superelevation.Name = "cb_Superelevation";
            this.cb_Superelevation.Size = new System.Drawing.Size(61, 25);
            this.cb_Superelevation.TabIndex = 0;
            this.cb_Superelevation.Text = "超高";
            this.cb_Superelevation.UseVisualStyleBackColor = true;
            // 
            // cb_R_Align_SC
            // 
            this.cb_R_Align_SC.AutoSize = true;
            this.cb_R_Align_SC.Location = new System.Drawing.Point(194, 52);
            this.cb_R_Align_SC.Name = "cb_R_Align_SC";
            this.cb_R_Align_SC.Size = new System.Drawing.Size(116, 25);
            this.cb_R_Align_SC.TabIndex = 1;
            this.cb_R_Align_SC.Text = "右轨向_中波";
            this.cb_R_Align_SC.UseVisualStyleBackColor = true;
            // 
            // cb_L_Align_SC
            // 
            this.cb_L_Align_SC.AutoSize = true;
            this.cb_L_Align_SC.Location = new System.Drawing.Point(194, 21);
            this.cb_L_Align_SC.Name = "cb_L_Align_SC";
            this.cb_L_Align_SC.Size = new System.Drawing.Size(116, 25);
            this.cb_L_Align_SC.TabIndex = 0;
            this.cb_L_Align_SC.Text = "左轨向_中波";
            this.cb_L_Align_SC.UseVisualStyleBackColor = true;
            // 
            // cb_R_Prof_SC
            // 
            this.cb_R_Prof_SC.AutoSize = true;
            this.cb_R_Prof_SC.Checked = true;
            this.cb_R_Prof_SC.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb_R_Prof_SC.Location = new System.Drawing.Point(70, 52);
            this.cb_R_Prof_SC.Name = "cb_R_Prof_SC";
            this.cb_R_Prof_SC.Size = new System.Drawing.Size(116, 25);
            this.cb_R_Prof_SC.TabIndex = 1;
            this.cb_R_Prof_SC.Text = "右高低_中波";
            this.cb_R_Prof_SC.UseVisualStyleBackColor = true;
            // 
            // cb_L_Prof_SC
            // 
            this.cb_L_Prof_SC.AutoSize = true;
            this.cb_L_Prof_SC.Checked = true;
            this.cb_L_Prof_SC.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb_L_Prof_SC.Location = new System.Drawing.Point(70, 21);
            this.cb_L_Prof_SC.Name = "cb_L_Prof_SC";
            this.cb_L_Prof_SC.Size = new System.Drawing.Size(116, 25);
            this.cb_L_Prof_SC.TabIndex = 0;
            this.cb_L_Prof_SC.Text = "左高低_中波";
            this.cb_L_Prof_SC.UseVisualStyleBackColor = true;
            // 
            // textBoxSpdBand
            // 
            this.textBoxSpdBand.Location = new System.Drawing.Point(410, 93);
            this.textBoxSpdBand.Name = "textBoxSpdBand";
            this.textBoxSpdBand.Size = new System.Drawing.Size(86, 29);
            this.textBoxSpdBand.TabIndex = 7;
            this.textBoxSpdBand.Text = "10";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(314, 96);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(90, 21);
            this.label8.TabIndex = 6;
            this.label8.Text = "速度带宽：";
            // 
            // buttonAvg
            // 
            this.buttonAvg.Location = new System.Drawing.Point(636, 125);
            this.buttonAvg.Name = "buttonAvg";
            this.buttonAvg.Size = new System.Drawing.Size(121, 32);
            this.buttonAvg.TabIndex = 5;
            this.buttonAvg.Text = "计算平均数";
            this.buttonAvg.UseVisualStyleBackColor = true;
            this.buttonAvg.Click += new System.EventHandler(this.buttonAvg_Click);
            // 
            // labelSpeed
            // 
            this.labelSpeed.AutoSize = true;
            this.labelSpeed.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelSpeed.Location = new System.Drawing.Point(223, 125);
            this.labelSpeed.Name = "labelSpeed";
            this.labelSpeed.Size = new System.Drawing.Size(72, 23);
            this.labelSpeed.TabIndex = 4;
            this.labelSpeed.Text = "            ";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(143, 125);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(74, 21);
            this.label6.TabIndex = 3;
            this.label6.Text = "速度级：";
            // 
            // buttonExportCsv_EnergyWeight
            // 
            this.buttonExportCsv_EnergyWeight.Location = new System.Drawing.Point(636, 90);
            this.buttonExportCsv_EnergyWeight.Name = "buttonExportCsv_EnergyWeight";
            this.buttonExportCsv_EnergyWeight.Size = new System.Drawing.Size(121, 32);
            this.buttonExportCsv_EnergyWeight.TabIndex = 2;
            this.buttonExportCsv_EnergyWeight.Text = "导出excel";
            this.buttonExportCsv_EnergyWeight.UseVisualStyleBackColor = true;
            this.buttonExportCsv_EnergyWeight.Click += new System.EventHandler(this.buttonExportCsv_EnergyWeight_Click);
            // 
            // button_EnergyWeight
            // 
            this.button_EnergyWeight.Location = new System.Drawing.Point(527, 90);
            this.button_EnergyWeight.Name = "button_EnergyWeight";
            this.button_EnergyWeight.Size = new System.Drawing.Size(103, 32);
            this.button_EnergyWeight.TabIndex = 2;
            this.button_EnergyWeight.Text = "确定";
            this.button_EnergyWeight.UseVisualStyleBackColor = true;
            this.button_EnergyWeight.Click += new System.EventHandler(this.button_EnergyWeight_Click);
            // 
            // labelEnergyWeight
            // 
            this.labelEnergyWeight.AutoSize = true;
            this.labelEnergyWeight.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelEnergyWeight.Location = new System.Drawing.Point(223, 96);
            this.labelEnergyWeight.Name = "labelEnergyWeight";
            this.labelEnergyWeight.Size = new System.Drawing.Size(72, 23);
            this.labelEnergyWeight.TabIndex = 1;
            this.labelEnergyWeight.Text = "            ";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(15, 96);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(202, 21);
            this.label4.TabIndex = 0;
            this.label4.Text = "能量权限系数的最大频率：";
            // 
            // dataGridView_EnergyWeight
            // 
            this.dataGridView_EnergyWeight.AllowUserToAddRows = false;
            this.dataGridView_EnergyWeight.AllowUserToDeleteRows = false;
            this.dataGridView_EnergyWeight.AllowUserToOrderColumns = true;
            this.dataGridView_EnergyWeight.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_EnergyWeight.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column3});
            this.dataGridView_EnergyWeight.Location = new System.Drawing.Point(0, 0);
            this.dataGridView_EnergyWeight.Name = "dataGridView_EnergyWeight";
            this.dataGridView_EnergyWeight.ReadOnly = true;
            this.dataGridView_EnergyWeight.RowHeadersVisible = false;
            this.dataGridView_EnergyWeight.RowTemplate.Height = 23;
            this.dataGridView_EnergyWeight.Size = new System.Drawing.Size(790, 235);
            this.dataGridView_EnergyWeight.TabIndex = 0;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "序号";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "放大系数";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            this.Column2.Width = 300;
            // 
            // Column3
            // 
            this.Column3.HeaderText = "能量权系数";
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            this.Column3.Width = 365;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label10);
            this.panel2.Controls.Add(this.label7);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.textBoxStartMile);
            this.panel2.Controls.Add(this.textBoxEndMile);
            this.panel2.Location = new System.Drawing.Point(182, 12);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(451, 42);
            this.panel2.TabIndex = 3;
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
            this.splitter1.Size = new System.Drawing.Size(790, 63);
            this.splitter1.TabIndex = 0;
            this.splitter1.TabStop = false;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.HeaderText = "序号";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.HeaderText = "放大系数";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.Width = 300;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.HeaderText = "能量权系数";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.Width = 365;
            // 
            // EnergyStatisticsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(790, 482);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("微软雅黑", 12F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EnergyStatisticsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "能量权系数";
            this.panel1.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel1.PerformLayout();
            this.splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_EnergyWeight)).EndInit();
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
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.TextBox textBoxSpdBand;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button buttonAvg;
        private System.Windows.Forms.Label labelSpeed;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button buttonExportCsv_EnergyWeight;
        private System.Windows.Forms.Button button_EnergyWeight;
        private System.Windows.Forms.Label labelEnergyWeight;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DataGridView dataGridView_EnergyWeight;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton rb_LACC;
        private System.Windows.Forms.RadioButton rb_VACC;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox cb_Crosslevel;
        private System.Windows.Forms.CheckBox cb_Short_Twist;
        private System.Windows.Forms.CheckBox cb_Superelevation;
        private System.Windows.Forms.CheckBox cb_R_Align_SC;
        private System.Windows.Forms.CheckBox cb_L_Align_SC;
        private System.Windows.Forms.CheckBox cb_R_Prof_SC;
        private System.Windows.Forms.CheckBox cb_L_Prof_SC;
    }
}