namespace InGraph.Forms
{
    partial class GongLvPuForm
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
            this.panel2 = new System.Windows.Forms.Panel();
            this.label8 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.radioButton_CB_Vt = new System.Windows.Forms.RadioButton();
            this.label9 = new System.Windows.Forms.Label();
            this.radioButton_CB_Lt = new System.Windows.Forms.RadioButton();
            this.textBox_Fourier = new System.Windows.Forms.TextBox();
            this.radioButton_Fr_Vt = new System.Windows.Forms.RadioButton();
            this.textBox_Km_Start = new System.Windows.Forms.TextBox();
            this.radioButton_Fr_Lt = new System.Windows.Forms.RadioButton();
            this.label7 = new System.Windows.Forms.Label();
            this.textBox_Km_End = new System.Windows.Forms.TextBox();
            this.textBox_Time = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.button_OK = new System.Windows.Forms.Button();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.button_ExportExcel = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.Column_ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_channelName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_Fs = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_Fuzhipu = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.splitContainer1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(462, 427);
            this.panel1.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label8);
            this.panel2.Controls.Add(this.label11);
            this.panel2.Controls.Add(this.radioButton_CB_Vt);
            this.panel2.Controls.Add(this.label9);
            this.panel2.Controls.Add(this.radioButton_CB_Lt);
            this.panel2.Controls.Add(this.button_ExportExcel);
            this.panel2.Controls.Add(this.button_OK);
            this.panel2.Controls.Add(this.textBox_Fourier);
            this.panel2.Controls.Add(this.radioButton_Fr_Vt);
            this.panel2.Controls.Add(this.textBox_Km_Start);
            this.panel2.Controls.Add(this.radioButton_Fr_Lt);
            this.panel2.Controls.Add(this.label7);
            this.panel2.Controls.Add(this.textBox_Km_End);
            this.panel2.Controls.Add(this.textBox_Time);
            this.panel2.Controls.Add(this.label6);
            this.panel2.Controls.Add(this.label10);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(462, 132);
            this.panel2.TabIndex = 36;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(141, 10);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(65, 20);
            this.label8.TabIndex = 28;
            this.label8.Text = "结束里程";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(221, 38);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(32, 20);
            this.label11.TabIndex = 34;
            this.label11.Text = "KM";
            // 
            // radioButton_CB_Vt
            // 
            this.radioButton_CB_Vt.AutoSize = true;
            this.radioButton_CB_Vt.Location = new System.Drawing.Point(19, 71);
            this.radioButton_CB_Vt.Name = "radioButton_CB_Vt";
            this.radioButton_CB_Vt.Size = new System.Drawing.Size(69, 24);
            this.radioButton_CB_Vt.TabIndex = 16;
            this.radioButton_CB_Vt.Text = "车体垂";
            this.radioButton_CB_Vt.UseVisualStyleBackColor = true;
            this.radioButton_CB_Vt.CheckedChanged += new System.EventHandler(this.radioButton_CheckedChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(99, 38);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(32, 20);
            this.label9.TabIndex = 35;
            this.label9.Text = "KM";
            // 
            // radioButton_CB_Lt
            // 
            this.radioButton_CB_Lt.AutoSize = true;
            this.radioButton_CB_Lt.Location = new System.Drawing.Point(141, 71);
            this.radioButton_CB_Lt.Name = "radioButton_CB_Lt";
            this.radioButton_CB_Lt.Size = new System.Drawing.Size(69, 24);
            this.radioButton_CB_Lt.TabIndex = 17;
            this.radioButton_CB_Lt.Text = "车体横";
            this.radioButton_CB_Lt.UseVisualStyleBackColor = true;
            this.radioButton_CB_Lt.CheckedChanged += new System.EventHandler(this.radioButton_CheckedChanged);
            // 
            // textBox_Fourier
            // 
            this.textBox_Fourier.Location = new System.Drawing.Point(263, 35);
            this.textBox_Fourier.Name = "textBox_Fourier";
            this.textBox_Fourier.Size = new System.Drawing.Size(61, 26);
            this.textBox_Fourier.TabIndex = 31;
            this.textBox_Fourier.Text = "1024";
            this.textBox_Fourier.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // radioButton_Fr_Vt
            // 
            this.radioButton_Fr_Vt.AutoSize = true;
            this.radioButton_Fr_Vt.Location = new System.Drawing.Point(263, 71);
            this.radioButton_Fr_Vt.Name = "radioButton_Fr_Vt";
            this.radioButton_Fr_Vt.Size = new System.Drawing.Size(69, 24);
            this.radioButton_Fr_Vt.TabIndex = 14;
            this.radioButton_Fr_Vt.Text = "构架垂";
            this.radioButton_Fr_Vt.UseVisualStyleBackColor = true;
            this.radioButton_Fr_Vt.CheckedChanged += new System.EventHandler(this.radioButton_CheckedChanged);
            // 
            // textBox_Km_Start
            // 
            this.textBox_Km_Start.Location = new System.Drawing.Point(23, 35);
            this.textBox_Km_Start.Name = "textBox_Km_Start";
            this.textBox_Km_Start.Size = new System.Drawing.Size(75, 26);
            this.textBox_Km_Start.TabIndex = 31;
            this.textBox_Km_Start.Text = "0.01";
            this.textBox_Km_Start.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // radioButton_Fr_Lt
            // 
            this.radioButton_Fr_Lt.AutoSize = true;
            this.radioButton_Fr_Lt.Location = new System.Drawing.Point(385, 71);
            this.radioButton_Fr_Lt.Name = "radioButton_Fr_Lt";
            this.radioButton_Fr_Lt.Size = new System.Drawing.Size(69, 24);
            this.radioButton_Fr_Lt.TabIndex = 15;
            this.radioButton_Fr_Lt.Text = "构架横";
            this.radioButton_Fr_Lt.UseVisualStyleBackColor = true;
            this.radioButton_Fr_Lt.CheckedChanged += new System.EventHandler(this.radioButton_CheckedChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(259, 10);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(107, 20);
            this.label7.TabIndex = 29;
            this.label7.Text = "傅里叶变换窗长";
            // 
            // textBox_Km_End
            // 
            this.textBox_Km_End.Location = new System.Drawing.Point(145, 35);
            this.textBox_Km_End.Name = "textBox_Km_End";
            this.textBox_Km_End.Size = new System.Drawing.Size(75, 26);
            this.textBox_Km_End.TabIndex = 33;
            this.textBox_Km_End.Text = "20";
            this.textBox_Km_End.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox_Time
            // 
            this.textBox_Time.Location = new System.Drawing.Point(385, 35);
            this.textBox_Time.Name = "textBox_Time";
            this.textBox_Time.Size = new System.Drawing.Size(61, 26);
            this.textBox_Time.TabIndex = 33;
            this.textBox_Time.Text = "0.003";
            this.textBox_Time.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(381, 10);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(65, 20);
            this.label6.TabIndex = 28;
            this.label6.Text = "时间步长";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(19, 10);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(65, 20);
            this.label10.TabIndex = 29;
            this.label10.Text = "起始里程";
            // 
            // button_OK
            // 
            this.button_OK.Location = new System.Drawing.Point(243, 100);
            this.button_OK.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button_OK.Name = "button_OK";
            this.button_OK.Size = new System.Drawing.Size(100, 26);
            this.button_OK.TabIndex = 13;
            this.button_OK.Text = "计算";
            this.button_OK.UseVisualStyleBackColor = true;
            this.button_OK.Click += new System.EventHandler(this.button_OK_Click);
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.panel2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.dataGridView1);
            this.splitContainer1.Size = new System.Drawing.Size(462, 427);
            this.splitContainer1.SplitterDistance = 132;
            this.splitContainer1.TabIndex = 37;
            // 
            // button_ExportExcel
            // 
            this.button_ExportExcel.Location = new System.Drawing.Point(351, 100);
            this.button_ExportExcel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button_ExportExcel.Name = "button_ExportExcel";
            this.button_ExportExcel.Size = new System.Drawing.Size(100, 26);
            this.button_ExportExcel.TabIndex = 13;
            this.button_ExportExcel.Text = "导出excel";
            this.button_ExportExcel.UseVisualStyleBackColor = true;
            this.button_ExportExcel.Click += new System.EventHandler(this.button_ExportExcel_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column_ID,
            this.Column_channelName,
            this.Column_Fs,
            this.Column_Fuzhipu});
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(462, 291);
            this.dataGridView1.TabIndex = 0;
            // 
            // Column_ID
            // 
            this.Column_ID.HeaderText = "序号";
            this.Column_ID.Name = "Column_ID";
            this.Column_ID.ReadOnly = true;
            this.Column_ID.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // Column_channelName
            // 
            this.Column_channelName.HeaderText = "项目名称";
            this.Column_channelName.Name = "Column_channelName";
            this.Column_channelName.ReadOnly = true;
            this.Column_channelName.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // Column_Fs
            // 
            this.Column_Fs.HeaderText = "频率";
            this.Column_Fs.Name = "Column_Fs";
            this.Column_Fs.ReadOnly = true;
            this.Column_Fs.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // Column_Fuzhipu
            // 
            this.Column_Fuzhipu.HeaderText = "幅值谱";
            this.Column_Fuzhipu.Name = "Column_Fuzhipu";
            this.Column_Fuzhipu.ReadOnly = true;
            this.Column_Fuzhipu.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // GongLvPuForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(462, 427);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "GongLvPuForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "功率谱";
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox textBox_Fourier;
        private System.Windows.Forms.TextBox textBox_Km_Start;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textBox_Time;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBox_Km_End;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.RadioButton radioButton_Fr_Lt;
        private System.Windows.Forms.RadioButton radioButton_Fr_Vt;
        private System.Windows.Forms.RadioButton radioButton_CB_Lt;
        private System.Windows.Forms.RadioButton radioButton_CB_Vt;
        private System.Windows.Forms.Button button_OK;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button button_ExportExcel;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_channelName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_Fs;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_Fuzhipu;
    }
}