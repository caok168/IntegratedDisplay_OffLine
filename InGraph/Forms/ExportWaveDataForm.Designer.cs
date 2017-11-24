namespace InGraph.Forms
{
    partial class ExportWaveDataForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExportWaveDataForm));
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxMileRange = new System.Windows.Forms.TextBox();
            this.checkedListBox1 = new System.Windows.Forms.CheckedListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioButtonAllLayersWithShowingChannel = new System.Windows.Forms.RadioButton();
            this.buttonExportData = new System.Windows.Forms.Button();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonSelectAll = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonSelectNone = new System.Windows.Forms.ToolStripButton();
            this.label3 = new System.Windows.Forms.Label();
            this.PasswordTextBox1 = new System.Windows.Forms.TextBox();
            this.buttonClose = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(12, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(136, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "主层的显示里程范围:";
            // 
            // textBoxMileRange
            // 
            this.textBoxMileRange.Location = new System.Drawing.Point(14, 48);
            this.textBoxMileRange.Name = "textBoxMileRange";
            this.textBoxMileRange.ReadOnly = true;
            this.textBoxMileRange.Size = new System.Drawing.Size(415, 21);
            this.textBoxMileRange.TabIndex = 1;
            this.textBoxMileRange.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // checkedListBox1
            // 
            this.checkedListBox1.CheckOnClick = true;
            this.checkedListBox1.FormattingEnabled = true;
            this.checkedListBox1.HorizontalScrollbar = true;
            this.checkedListBox1.Location = new System.Drawing.Point(14, 90);
            this.checkedListBox1.Name = "checkedListBox1";
            this.checkedListBox1.ScrollAlwaysVisible = true;
            this.checkedListBox1.Size = new System.Drawing.Size(415, 116);
            this.checkedListBox1.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(12, 72);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(136, 15);
            this.label2.TabIndex = 3;
            this.label2.Text = "请选择要导出的数据层:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioButtonAllLayersWithShowingChannel);
            this.groupBox1.Location = new System.Drawing.Point(14, 212);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(415, 45);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "导出模式";
            // 
            // radioButtonAllLayersWithShowingChannel
            // 
            this.radioButtonAllLayersWithShowingChannel.Checked = true;
            this.radioButtonAllLayersWithShowingChannel.Location = new System.Drawing.Point(128, 15);
            this.radioButtonAllLayersWithShowingChannel.Name = "radioButtonAllLayersWithShowingChannel";
            this.radioButtonAllLayersWithShowingChannel.Size = new System.Drawing.Size(162, 24);
            this.radioButtonAllLayersWithShowingChannel.TabIndex = 0;
            this.radioButtonAllLayersWithShowingChannel.TabStop = true;
            this.radioButtonAllLayersWithShowingChannel.Text = "导出每层所显示的通道";
            this.radioButtonAllLayersWithShowingChannel.UseVisualStyleBackColor = true;
            // 
            // buttonExportData
            // 
            this.buttonExportData.Location = new System.Drawing.Point(266, 263);
            this.buttonExportData.Name = "buttonExportData";
            this.buttonExportData.Size = new System.Drawing.Size(75, 23);
            this.buttonExportData.TabIndex = 5;
            this.buttonExportData.Text = "导出(&E)";
            this.buttonExportData.UseVisualStyleBackColor = true;
            this.buttonExportData.Click += new System.EventHandler(this.buttonExportData_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonSelectAll,
            this.toolStripButtonSelectNone});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(441, 25);
            this.toolStrip1.TabIndex = 6;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButtonSelectAll
            // 
            this.toolStripButtonSelectAll.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonSelectAll.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonSelectAll.Image")));
            this.toolStripButtonSelectAll.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSelectAll.Name = "toolStripButtonSelectAll";
            this.toolStripButtonSelectAll.Size = new System.Drawing.Size(35, 22);
            this.toolStripButtonSelectAll.Text = "全选";
            this.toolStripButtonSelectAll.Click += new System.EventHandler(this.toolStripButtonSelectAll_Click);
            // 
            // toolStripButtonSelectNone
            // 
            this.toolStripButtonSelectNone.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonSelectNone.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonSelectNone.Image")));
            this.toolStripButtonSelectNone.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSelectNone.Name = "toolStripButtonSelectNone";
            this.toolStripButtonSelectNone.Size = new System.Drawing.Size(47, 22);
            this.toolStripButtonSelectNone.Text = "全不选";
            this.toolStripButtonSelectNone.Click += new System.EventHandler(this.toolStripButtonSelectNone_Click);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(12, 268);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(89, 23);
            this.label3.TabIndex = 7;
            this.label3.Text = "请输入验证码";
            // 
            // PasswordTextBox1
            // 
            this.PasswordTextBox1.Location = new System.Drawing.Point(95, 265);
            this.PasswordTextBox1.MaxLength = 11;
            this.PasswordTextBox1.Name = "PasswordTextBox1";
            this.PasswordTextBox1.PasswordChar = '*';
            this.PasswordTextBox1.Size = new System.Drawing.Size(81, 21);
            this.PasswordTextBox1.TabIndex = 8;
            // 
            // buttonClose
            // 
            this.buttonClose.Location = new System.Drawing.Point(354, 263);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(75, 23);
            this.buttonClose.TabIndex = 9;
            this.buttonClose.Text = "关闭(&C)";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // ExportWaveDataForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(217)))), ((int)(((byte)(222)))));
            this.ClientSize = new System.Drawing.Size(441, 293);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.PasswordTextBox1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.buttonExportData);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.checkedListBox1);
            this.Controls.Add(this.textBoxMileRange);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ExportWaveDataForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "导出当前屏的波形数据";
            this.Load += new System.EventHandler(this.ExportWaveDataForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxMileRange;
        private System.Windows.Forms.CheckedListBox checkedListBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radioButtonAllLayersWithShowingChannel;
        private System.Windows.Forms.Button buttonExportData;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButtonSelectAll;
        private System.Windows.Forms.ToolStripButton toolStripButtonSelectNone;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox PasswordTextBox1;
        private System.Windows.Forms.Button buttonClose;
    }
}