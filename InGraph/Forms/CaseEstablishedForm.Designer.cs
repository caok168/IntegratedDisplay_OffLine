namespace InGraph.Forms
{
    partial class CaseEstablishedForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxStartMile = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxEndMile = new System.Windows.Forms.TextBox();
            this.comboBoxAnalysisType = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.comboBoxManageType = new System.Windows.Forms.ComboBox();
            this.richTextBoxMemoInfo = new System.Windows.Forms.RichTextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.comboBoxSecFlag = new System.Windows.Forms.ComboBox();
            this.buttonSaveCase = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.label5 = new System.Windows.Forms.Label();
            this.labelAttachAdd = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "起点里程：";
            // 
            // textBoxStartMile
            // 
            this.textBoxStartMile.Location = new System.Drawing.Point(83, 18);
            this.textBoxStartMile.Name = "textBoxStartMile";
            this.textBoxStartMile.Size = new System.Drawing.Size(100, 21);
            this.textBoxStartMile.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(189, 21);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "终点里程：";
            // 
            // textBoxEndMile
            // 
            this.textBoxEndMile.Location = new System.Drawing.Point(260, 18);
            this.textBoxEndMile.Name = "textBoxEndMile";
            this.textBoxEndMile.Size = new System.Drawing.Size(100, 21);
            this.textBoxEndMile.TabIndex = 1;
            // 
            // comboBoxAnalysisType
            // 
            this.comboBoxAnalysisType.FormattingEnabled = true;
            this.comboBoxAnalysisType.Items.AddRange(new object[] {
            "科研",
            "维修"});
            this.comboBoxAnalysisType.Location = new System.Drawing.Point(83, 48);
            this.comboBoxAnalysisType.Name = "comboBoxAnalysisType";
            this.comboBoxAnalysisType.Size = new System.Drawing.Size(100, 20);
            this.comboBoxAnalysisType.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 51);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 0;
            this.label3.Text = "分析类别：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(189, 51);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 12);
            this.label4.TabIndex = 0;
            this.label4.Text = "管理类别：";
            this.label4.Visible = false;
            // 
            // comboBoxManageType
            // 
            this.comboBoxManageType.FormattingEnabled = true;
            this.comboBoxManageType.Items.AddRange(new object[] {
            "复合",
            "闭环管理"});
            this.comboBoxManageType.Location = new System.Drawing.Point(260, 48);
            this.comboBoxManageType.Name = "comboBoxManageType";
            this.comboBoxManageType.Size = new System.Drawing.Size(100, 20);
            this.comboBoxManageType.TabIndex = 2;
            this.comboBoxManageType.Visible = false;
            // 
            // richTextBoxMemoInfo
            // 
            this.richTextBoxMemoInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxMemoInfo.Location = new System.Drawing.Point(3, 17);
            this.richTextBoxMemoInfo.Name = "richTextBoxMemoInfo";
            this.richTextBoxMemoInfo.Size = new System.Drawing.Size(340, 52);
            this.richTextBoxMemoInfo.TabIndex = 3;
            this.richTextBoxMemoInfo.Text = "";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(247, 231);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(65, 12);
            this.label7.TabIndex = 0;
            this.label7.Text = "区段标志：";
            this.label7.Visible = false;
            // 
            // comboBoxSecFlag
            // 
            this.comboBoxSecFlag.FormattingEnabled = true;
            this.comboBoxSecFlag.Items.AddRange(new object[] {
            "单点",
            "区段"});
            this.comboBoxSecFlag.Location = new System.Drawing.Point(318, 228);
            this.comboBoxSecFlag.Name = "comboBoxSecFlag";
            this.comboBoxSecFlag.Size = new System.Drawing.Size(36, 20);
            this.comboBoxSecFlag.TabIndex = 2;
            this.comboBoxSecFlag.Visible = false;
            // 
            // buttonSaveCase
            // 
            this.buttonSaveCase.Location = new System.Drawing.Point(14, 225);
            this.buttonSaveCase.Name = "buttonSaveCase";
            this.buttonSaveCase.Size = new System.Drawing.Size(75, 23);
            this.buttonSaveCase.TabIndex = 4;
            this.buttonSaveCase.Text = "保存";
            this.buttonSaveCase.UseVisualStyleBackColor = true;
            this.buttonSaveCase.Click += new System.EventHandler(this.buttonSaveCase_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.label5.Location = new System.Drawing.Point(117, 226);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(69, 26);
            this.label5.TabIndex = 5;
            this.label5.Text = "附件：";
            // 
            // labelAttachAdd
            // 
            this.labelAttachAdd.AutoSize = true;
            this.labelAttachAdd.Cursor = System.Windows.Forms.Cursors.Hand;
            this.labelAttachAdd.Enabled = false;
            this.labelAttachAdd.Font = new System.Drawing.Font("微软雅黑", 14.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelAttachAdd.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.labelAttachAdd.Location = new System.Drawing.Point(192, 226);
            this.labelAttachAdd.Name = "labelAttachAdd";
            this.labelAttachAdd.Size = new System.Drawing.Size(24, 26);
            this.labelAttachAdd.TabIndex = 5;
            this.labelAttachAdd.Text = "0";
            this.labelAttachAdd.Click += new System.EventHandler(this.labelAttachAdd_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.richTextBoxMemoInfo);
            this.groupBox1.Location = new System.Drawing.Point(14, 70);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(346, 72);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "案例分析";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.richTextBox1);
            this.groupBox2.Location = new System.Drawing.Point(14, 148);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(346, 72);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "标签";
            // 
            // richTextBox1
            // 
            this.richTextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox1.Location = new System.Drawing.Point(3, 17);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(340, 52);
            this.richTextBox1.TabIndex = 3;
            this.richTextBox1.Text = "";
            // 
            // CaseEstablishedForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(366, 254);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.labelAttachAdd);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.buttonSaveCase);
            this.Controls.Add(this.comboBoxSecFlag);
            this.Controls.Add(this.comboBoxManageType);
            this.Controls.Add(this.comboBoxAnalysisType);
            this.Controls.Add(this.textBoxEndMile);
            this.Controls.Add(this.textBoxStartMile);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CaseEstablishedForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "新增案例";
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxStartMile;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxEndMile;
        private System.Windows.Forms.ComboBox comboBoxAnalysisType;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox comboBoxManageType;
        private System.Windows.Forms.RichTextBox richTextBoxMemoInfo;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox comboBoxSecFlag;
        private System.Windows.Forms.Button buttonSaveCase;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label labelAttachAdd;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RichTextBox richTextBox1;
    }
}