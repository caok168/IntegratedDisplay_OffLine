namespace InGraph.Forms
{
    partial class DIYDefectForm
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
            this.buttonOK = new System.Windows.Forms.Button();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column10 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column11 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.buttonExportExcel = new System.Windows.Forms.Button();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.buttonSelectIIC = new System.Windows.Forms.Button();
            this.buttonExportIIC = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.labelIICName = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.panel_export = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.checkBoxLevel4 = new System.Windows.Forms.CheckBox();
            this.checkBoxLevel3 = new System.Windows.Forms.CheckBox();
            this.checkBoxLevel2 = new System.Windows.Forms.CheckBox();
            this.checkBoxLevel1 = new System.Windows.Forms.CheckBox();
            this.panel7 = new System.Windows.Forms.Panel();
            this.checkBox_VACC = new System.Windows.Forms.CheckBox();
            this.checkBox_LACC = new System.Windows.Forms.CheckBox();
            this.checkBox_NarrowGage = new System.Windows.Forms.CheckBox();
            this.checkBox_WideGage = new System.Windows.Forms.CheckBox();
            this.checkBox_Short_Twist = new System.Windows.Forms.CheckBox();
            this.checkBox_Align_SC = new System.Windows.Forms.CheckBox();
            this.checkBox_Align_SC_120 = new System.Windows.Forms.CheckBox();
            this.checkBox_CrossLevel = new System.Windows.Forms.CheckBox();
            this.checkBox_Prof_SC_120 = new System.Windows.Forms.CheckBox();
            this.checkBox_Align_SC_70 = new System.Windows.Forms.CheckBox();
            this.checkBox_Prof_SC_70 = new System.Windows.Forms.CheckBox();
            this.checkBox_Prof_SC = new System.Windows.Forms.CheckBox();
            this.groupBox_select = new System.Windows.Forms.GroupBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.radioButton_jubiao = new System.Windows.Forms.RadioButton();
            this.radioButton_bubiao = new System.Windows.Forms.RadioButton();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.panel_export.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel7.SuspendLayout();
            this.groupBox_select.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonOK
            // 
            this.buttonOK.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.buttonOK.Location = new System.Drawing.Point(606, 1);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(87, 29);
            this.buttonOK.TabIndex = 4;
            this.buttonOK.Text = "确  定";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dataGridView1);
            this.groupBox1.Location = new System.Drawing.Point(12, 59);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(710, 367);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "自定义超限列表";
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToOrderColumns = true;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.ColumnHeadersHeight = 45;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column3,
            this.Column4,
            this.Column5,
            this.Column6,
            this.Column7,
            this.Column8,
            this.Column9,
            this.Column10,
            this.Column11});
            this.dataGridView1.ContextMenuStrip = this.contextMenuStrip1;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(3, 22);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(704, 342);
            this.dataGridView1.TabIndex = 24;
            this.dataGridView1.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellDoubleClick);
            // 
            // Column1
            // 
            this.Column1.HeaderText = "ID";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Width = 35;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "类型";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            // 
            // Column3
            // 
            this.Column3.HeaderText = "里程(KM)";
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            this.Column3.Width = 90;
            // 
            // Column4
            // 
            this.Column4.HeaderText = "长度(M)";
            this.Column4.Name = "Column4";
            this.Column4.ReadOnly = true;
            this.Column4.Width = 50;
            // 
            // Column5
            // 
            this.Column5.HeaderText = "超限等级";
            this.Column5.Name = "Column5";
            this.Column5.ReadOnly = true;
            this.Column5.Width = 70;
            // 
            // Column6
            // 
            this.Column6.HeaderText = "限速(km/h)";
            this.Column6.Name = "Column6";
            this.Column6.ReadOnly = true;
            this.Column6.Width = 50;
            // 
            // Column7
            // 
            this.Column7.HeaderText = "速度(km/h)";
            this.Column7.Name = "Column7";
            this.Column7.ReadOnly = true;
            this.Column7.Width = 50;
            // 
            // Column8
            // 
            this.Column8.HeaderText = "行业标准";
            this.Column8.Name = "Column8";
            this.Column8.ReadOnly = true;
            this.Column8.Width = 70;
            // 
            // Column9
            // 
            this.Column9.HeaderText = "铁路局标准";
            this.Column9.Name = "Column9";
            this.Column9.ReadOnly = true;
            this.Column9.Width = 60;
            // 
            // Column10
            // 
            this.Column10.HeaderText = "超限值";
            this.Column10.Name = "Column10";
            this.Column10.ReadOnly = true;
            this.Column10.Width = 65;
            // 
            // Column11
            // 
            this.Column11.HeaderText = "有效性";
            this.Column11.Name = "Column11";
            this.Column11.ReadOnly = true;
            this.Column11.Width = 65;
            // 
            // buttonExportExcel
            // 
            this.buttonExportExcel.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.buttonExportExcel.Location = new System.Drawing.Point(4, 3);
            this.buttonExportExcel.Name = "buttonExportExcel";
            this.buttonExportExcel.Size = new System.Drawing.Size(132, 29);
            this.buttonExportExcel.TabIndex = 7;
            this.buttonExportExcel.Text = "导出Excel";
            this.buttonExportExcel.UseVisualStyleBackColor = true;
            this.buttonExportExcel.Click += new System.EventHandler(this.buttonExportExcel_Click);
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // buttonSelectIIC
            // 
            this.buttonSelectIIC.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.buttonSelectIIC.Location = new System.Drawing.Point(159, 2);
            this.buttonSelectIIC.Name = "buttonSelectIIC";
            this.buttonSelectIIC.Size = new System.Drawing.Size(75, 29);
            this.buttonSelectIIC.TabIndex = 15;
            this.buttonSelectIIC.Text = "选择IIC";
            this.buttonSelectIIC.UseVisualStyleBackColor = true;
            this.buttonSelectIIC.Click += new System.EventHandler(this.buttonSelectIIC_Click);
            // 
            // buttonExportIIC
            // 
            this.buttonExportIIC.Enabled = false;
            this.buttonExportIIC.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.buttonExportIIC.Location = new System.Drawing.Point(630, 3);
            this.buttonExportIIC.Name = "buttonExportIIC";
            this.buttonExportIIC.Size = new System.Drawing.Size(75, 30);
            this.buttonExportIIC.TabIndex = 16;
            this.buttonExportIIC.Text = "导出IIC";
            this.buttonExportIIC.UseVisualStyleBackColor = true;
            this.buttonExportIIC.Click += new System.EventHandler(this.buttonExportIIC_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label8.Location = new System.Drawing.Point(236, 8);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(26, 21);
            this.label8.TabIndex = 17;
            this.label8.Text = "：";
            // 
            // labelIICName
            // 
            this.labelIICName.AutoSize = true;
            this.labelIICName.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelIICName.Location = new System.Drawing.Point(265, 7);
            this.labelIICName.Name = "labelIICName";
            this.labelIICName.Size = new System.Drawing.Size(74, 21);
            this.labelIICName.TabIndex = 18;
            this.labelIICName.Text = "iic文件名";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.Filter = "iic文件|*.iic|所有文件|*.*";
            // 
            // panel_export
            // 
            this.panel_export.Controls.Add(this.buttonExportExcel);
            this.panel_export.Controls.Add(this.buttonSelectIIC);
            this.panel_export.Controls.Add(this.buttonExportIIC);
            this.panel_export.Controls.Add(this.labelIICName);
            this.panel_export.Controls.Add(this.label8);
            this.panel_export.Enabled = false;
            this.panel_export.Location = new System.Drawing.Point(15, 430);
            this.panel_export.Name = "panel_export";
            this.panel_export.Size = new System.Drawing.Size(710, 37);
            this.panel_export.TabIndex = 20;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.checkBoxLevel4);
            this.panel5.Controls.Add(this.checkBoxLevel3);
            this.panel5.Controls.Add(this.checkBoxLevel2);
            this.panel5.Controls.Add(this.checkBoxLevel1);
            this.panel5.Location = new System.Drawing.Point(1, 17);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(698, 31);
            this.panel5.TabIndex = 24;
            this.panel5.Visible = false;
            // 
            // checkBoxLevel4
            // 
            this.checkBoxLevel4.AutoSize = true;
            this.checkBoxLevel4.Location = new System.Drawing.Point(415, 3);
            this.checkBoxLevel4.Name = "checkBoxLevel4";
            this.checkBoxLevel4.Size = new System.Drawing.Size(56, 24);
            this.checkBoxLevel4.TabIndex = 25;
            this.checkBoxLevel4.Tag = "4";
            this.checkBoxLevel4.Text = "四级";
            this.checkBoxLevel4.UseVisualStyleBackColor = true;
            this.checkBoxLevel4.Visible = false;
            // 
            // checkBoxLevel3
            // 
            this.checkBoxLevel3.AutoSize = true;
            this.checkBoxLevel3.Location = new System.Drawing.Point(267, 4);
            this.checkBoxLevel3.Name = "checkBoxLevel3";
            this.checkBoxLevel3.Size = new System.Drawing.Size(56, 24);
            this.checkBoxLevel3.TabIndex = 2;
            this.checkBoxLevel3.Tag = "3";
            this.checkBoxLevel3.Text = "三级";
            this.checkBoxLevel3.UseVisualStyleBackColor = true;
            this.checkBoxLevel3.Visible = false;
            // 
            // checkBoxLevel2
            // 
            this.checkBoxLevel2.AutoSize = true;
            this.checkBoxLevel2.Location = new System.Drawing.Point(127, 3);
            this.checkBoxLevel2.Name = "checkBoxLevel2";
            this.checkBoxLevel2.Size = new System.Drawing.Size(56, 24);
            this.checkBoxLevel2.TabIndex = 1;
            this.checkBoxLevel2.Tag = "2";
            this.checkBoxLevel2.Text = "二级";
            this.checkBoxLevel2.UseVisualStyleBackColor = true;
            this.checkBoxLevel2.Visible = false;
            // 
            // checkBoxLevel1
            // 
            this.checkBoxLevel1.AutoSize = true;
            this.checkBoxLevel1.Checked = true;
            this.checkBoxLevel1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxLevel1.Location = new System.Drawing.Point(3, 3);
            this.checkBoxLevel1.Name = "checkBoxLevel1";
            this.checkBoxLevel1.Size = new System.Drawing.Size(56, 24);
            this.checkBoxLevel1.TabIndex = 0;
            this.checkBoxLevel1.Tag = "1";
            this.checkBoxLevel1.Text = "一级";
            this.checkBoxLevel1.UseVisualStyleBackColor = true;
            this.checkBoxLevel1.Visible = false;
            // 
            // panel7
            // 
            this.panel7.Controls.Add(this.panel5);
            this.panel7.Controls.Add(this.checkBox_VACC);
            this.panel7.Controls.Add(this.checkBox_LACC);
            this.panel7.Controls.Add(this.checkBox_NarrowGage);
            this.panel7.Controls.Add(this.checkBox_WideGage);
            this.panel7.Controls.Add(this.checkBox_Short_Twist);
            this.panel7.Controls.Add(this.checkBox_Align_SC);
            this.panel7.Controls.Add(this.checkBox_Align_SC_120);
            this.panel7.Controls.Add(this.checkBox_CrossLevel);
            this.panel7.Controls.Add(this.checkBox_Prof_SC_120);
            this.panel7.Controls.Add(this.checkBox_Align_SC_70);
            this.panel7.Controls.Add(this.checkBox_Prof_SC_70);
            this.panel7.Controls.Add(this.checkBox_Prof_SC);
            this.panel7.Location = new System.Drawing.Point(6, 18);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(647, 29);
            this.panel7.TabIndex = 24;
            this.panel7.Visible = false;
            // 
            // checkBox_VACC
            // 
            this.checkBox_VACC.AutoSize = true;
            this.checkBox_VACC.Checked = true;
            this.checkBox_VACC.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_VACC.Location = new System.Drawing.Point(609, 21);
            this.checkBox_VACC.Name = "checkBox_VACC";
            this.checkBox_VACC.Size = new System.Drawing.Size(84, 24);
            this.checkBox_VACC.TabIndex = 25;
            this.checkBox_VACC.Tag = "VACC";
            this.checkBox_VACC.Text = "车体垂加";
            this.checkBox_VACC.UseVisualStyleBackColor = true;
            // 
            // checkBox_LACC
            // 
            this.checkBox_LACC.AutoSize = true;
            this.checkBox_LACC.Checked = true;
            this.checkBox_LACC.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_LACC.Location = new System.Drawing.Point(609, 3);
            this.checkBox_LACC.Name = "checkBox_LACC";
            this.checkBox_LACC.Size = new System.Drawing.Size(84, 24);
            this.checkBox_LACC.TabIndex = 2;
            this.checkBox_LACC.Tag = "LACC";
            this.checkBox_LACC.Text = "车体横加";
            this.checkBox_LACC.UseVisualStyleBackColor = true;
            // 
            // checkBox_NarrowGage
            // 
            this.checkBox_NarrowGage.AutoSize = true;
            this.checkBox_NarrowGage.Checked = true;
            this.checkBox_NarrowGage.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_NarrowGage.Location = new System.Drawing.Point(415, 21);
            this.checkBox_NarrowGage.Name = "checkBox_NarrowGage";
            this.checkBox_NarrowGage.Size = new System.Drawing.Size(70, 24);
            this.checkBox_NarrowGage.TabIndex = 25;
            this.checkBox_NarrowGage.Tag = "NarrowGage";
            this.checkBox_NarrowGage.Text = "小轨距";
            this.checkBox_NarrowGage.UseVisualStyleBackColor = true;
            // 
            // checkBox_WideGage
            // 
            this.checkBox_WideGage.AutoSize = true;
            this.checkBox_WideGage.Checked = true;
            this.checkBox_WideGage.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_WideGage.Location = new System.Drawing.Point(415, 3);
            this.checkBox_WideGage.Name = "checkBox_WideGage";
            this.checkBox_WideGage.Size = new System.Drawing.Size(70, 24);
            this.checkBox_WideGage.TabIndex = 2;
            this.checkBox_WideGage.Tag = "WideGage";
            this.checkBox_WideGage.Text = "大轨距";
            this.checkBox_WideGage.UseVisualStyleBackColor = true;
            // 
            // checkBox_Short_Twist
            // 
            this.checkBox_Short_Twist.AutoSize = true;
            this.checkBox_Short_Twist.Checked = true;
            this.checkBox_Short_Twist.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_Short_Twist.Location = new System.Drawing.Point(519, 21);
            this.checkBox_Short_Twist.Name = "checkBox_Short_Twist";
            this.checkBox_Short_Twist.Size = new System.Drawing.Size(70, 24);
            this.checkBox_Short_Twist.TabIndex = 1;
            this.checkBox_Short_Twist.Tag = "Short_Twist";
            this.checkBox_Short_Twist.Text = "三角坑";
            this.checkBox_Short_Twist.UseVisualStyleBackColor = true;
            // 
            // checkBox_Align_SC
            // 
            this.checkBox_Align_SC.AutoSize = true;
            this.checkBox_Align_SC.Checked = true;
            this.checkBox_Align_SC.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_Align_SC.Location = new System.Drawing.Point(3, 24);
            this.checkBox_Align_SC.Name = "checkBox_Align_SC";
            this.checkBox_Align_SC.Size = new System.Drawing.Size(90, 24);
            this.checkBox_Align_SC.TabIndex = 25;
            this.checkBox_Align_SC.Tag = "Align_SC";
            this.checkBox_Align_SC.Text = "轨向_中波";
            this.checkBox_Align_SC.UseVisualStyleBackColor = true;
            // 
            // checkBox_Align_SC_120
            // 
            this.checkBox_Align_SC_120.AutoSize = true;
            this.checkBox_Align_SC_120.Checked = true;
            this.checkBox_Align_SC_120.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_Align_SC_120.Location = new System.Drawing.Point(267, 21);
            this.checkBox_Align_SC_120.Name = "checkBox_Align_SC_120";
            this.checkBox_Align_SC_120.Size = new System.Drawing.Size(114, 24);
            this.checkBox_Align_SC_120.TabIndex = 1;
            this.checkBox_Align_SC_120.Tag = "Align_SC_120";
            this.checkBox_Align_SC_120.Text = "轨向_120长波";
            this.checkBox_Align_SC_120.UseVisualStyleBackColor = true;
            // 
            // checkBox_CrossLevel
            // 
            this.checkBox_CrossLevel.AutoSize = true;
            this.checkBox_CrossLevel.Checked = true;
            this.checkBox_CrossLevel.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_CrossLevel.Location = new System.Drawing.Point(519, 3);
            this.checkBox_CrossLevel.Name = "checkBox_CrossLevel";
            this.checkBox_CrossLevel.Size = new System.Drawing.Size(56, 24);
            this.checkBox_CrossLevel.TabIndex = 0;
            this.checkBox_CrossLevel.Tag = "CrossLevel";
            this.checkBox_CrossLevel.Text = "水平";
            this.checkBox_CrossLevel.UseVisualStyleBackColor = true;
            // 
            // checkBox_Prof_SC_120
            // 
            this.checkBox_Prof_SC_120.AutoSize = true;
            this.checkBox_Prof_SC_120.Checked = true;
            this.checkBox_Prof_SC_120.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_Prof_SC_120.Location = new System.Drawing.Point(267, 3);
            this.checkBox_Prof_SC_120.Name = "checkBox_Prof_SC_120";
            this.checkBox_Prof_SC_120.Size = new System.Drawing.Size(114, 24);
            this.checkBox_Prof_SC_120.TabIndex = 2;
            this.checkBox_Prof_SC_120.Tag = "Prof_SC_120";
            this.checkBox_Prof_SC_120.Text = "高低_120长波";
            this.checkBox_Prof_SC_120.UseVisualStyleBackColor = true;
            // 
            // checkBox_Align_SC_70
            // 
            this.checkBox_Align_SC_70.AutoSize = true;
            this.checkBox_Align_SC_70.Checked = true;
            this.checkBox_Align_SC_70.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_Align_SC_70.Location = new System.Drawing.Point(127, 21);
            this.checkBox_Align_SC_70.Name = "checkBox_Align_SC_70";
            this.checkBox_Align_SC_70.Size = new System.Drawing.Size(106, 24);
            this.checkBox_Align_SC_70.TabIndex = 0;
            this.checkBox_Align_SC_70.Tag = "Align_SC_70";
            this.checkBox_Align_SC_70.Text = "轨向_70长波";
            this.checkBox_Align_SC_70.UseVisualStyleBackColor = true;
            // 
            // checkBox_Prof_SC_70
            // 
            this.checkBox_Prof_SC_70.AutoSize = true;
            this.checkBox_Prof_SC_70.Checked = true;
            this.checkBox_Prof_SC_70.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_Prof_SC_70.Location = new System.Drawing.Point(127, 3);
            this.checkBox_Prof_SC_70.Name = "checkBox_Prof_SC_70";
            this.checkBox_Prof_SC_70.Size = new System.Drawing.Size(106, 24);
            this.checkBox_Prof_SC_70.TabIndex = 1;
            this.checkBox_Prof_SC_70.Tag = "Prof_SC_70";
            this.checkBox_Prof_SC_70.Text = "高低_70长波";
            this.checkBox_Prof_SC_70.UseVisualStyleBackColor = true;
            // 
            // checkBox_Prof_SC
            // 
            this.checkBox_Prof_SC.AutoSize = true;
            this.checkBox_Prof_SC.Checked = true;
            this.checkBox_Prof_SC.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_Prof_SC.Location = new System.Drawing.Point(3, 3);
            this.checkBox_Prof_SC.Name = "checkBox_Prof_SC";
            this.checkBox_Prof_SC.Size = new System.Drawing.Size(90, 24);
            this.checkBox_Prof_SC.TabIndex = 0;
            this.checkBox_Prof_SC.Tag = "Prof_SC";
            this.checkBox_Prof_SC.Text = "高低_中波";
            this.checkBox_Prof_SC.UseVisualStyleBackColor = true;
            // 
            // groupBox_select
            // 
            this.groupBox_select.Controls.Add(this.panel3);
            this.groupBox_select.Controls.Add(this.panel7);
            this.groupBox_select.Location = new System.Drawing.Point(12, 7);
            this.groupBox_select.Name = "groupBox_select";
            this.groupBox_select.Size = new System.Drawing.Size(710, 52);
            this.groupBox_select.TabIndex = 25;
            this.groupBox_select.TabStop = false;
            this.groupBox_select.Text = "项目选择";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.radioButton_jubiao);
            this.panel3.Controls.Add(this.buttonOK);
            this.panel3.Controls.Add(this.radioButton_bubiao);
            this.panel3.Location = new System.Drawing.Point(7, 18);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(698, 31);
            this.panel3.TabIndex = 29;
            // 
            // radioButton_jubiao
            // 
            this.radioButton_jubiao.AutoSize = true;
            this.radioButton_jubiao.Location = new System.Drawing.Point(125, 3);
            this.radioButton_jubiao.Name = "radioButton_jubiao";
            this.radioButton_jubiao.Size = new System.Drawing.Size(55, 24);
            this.radioButton_jubiao.TabIndex = 20;
            this.radioButton_jubiao.Text = "局标";
            this.radioButton_jubiao.UseVisualStyleBackColor = true;
            // 
            // radioButton_bubiao
            // 
            this.radioButton_bubiao.AutoSize = true;
            this.radioButton_bubiao.Checked = true;
            this.radioButton_bubiao.Location = new System.Drawing.Point(3, 3);
            this.radioButton_bubiao.Name = "radioButton_bubiao";
            this.radioButton_bubiao.Size = new System.Drawing.Size(55, 24);
            this.radioButton_bubiao.TabIndex = 20;
            this.radioButton_bubiao.TabStop = true;
            this.radioButton_bubiao.Text = "部标";
            this.radioButton_bubiao.UseVisualStyleBackColor = true;
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(0, 0);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(3, 467);
            this.splitter1.TabIndex = 28;
            this.splitter1.TabStop = false;
            // 
            // DIYDefectForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(725, 467);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.groupBox_select);
            this.Controls.Add(this.panel_export);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(5);
            this.MaximumSize = new System.Drawing.Size(741, 591);
            this.Name = "DIYDefectForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.DIYDefectForm_Load);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.panel_export.ResumeLayout(false);
            this.panel_export.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.panel7.ResumeLayout(false);
            this.panel7.PerformLayout();
            this.groupBox_select.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button buttonExportExcel;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Button buttonSelectIIC;
        private System.Windows.Forms.Button buttonExportIIC;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label labelIICName;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Panel panel_export;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.CheckBox checkBoxLevel4;
        private System.Windows.Forms.CheckBox checkBoxLevel3;
        private System.Windows.Forms.CheckBox checkBoxLevel2;
        private System.Windows.Forms.CheckBox checkBoxLevel1;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.CheckBox checkBox_Align_SC;
        private System.Windows.Forms.CheckBox checkBox_Prof_SC_120;
        private System.Windows.Forms.CheckBox checkBox_Prof_SC_70;
        private System.Windows.Forms.CheckBox checkBox_Prof_SC;
        private System.Windows.Forms.CheckBox checkBox_VACC;
        private System.Windows.Forms.CheckBox checkBox_LACC;
        private System.Windows.Forms.CheckBox checkBox_NarrowGage;
        private System.Windows.Forms.CheckBox checkBox_WideGage;
        private System.Windows.Forms.CheckBox checkBox_Short_Twist;
        private System.Windows.Forms.CheckBox checkBox_Align_SC_120;
        private System.Windows.Forms.CheckBox checkBox_CrossLevel;
        private System.Windows.Forms.CheckBox checkBox_Align_SC_70;
        private System.Windows.Forms.GroupBox groupBox_select;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column6;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column7;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column8;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column9;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column10;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column11;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.RadioButton radioButton_jubiao;
        private System.Windows.Forms.RadioButton radioButton_bubiao;
    }
}