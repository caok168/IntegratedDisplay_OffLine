namespace InGraph.Forms
{
    partial class UnitCutPictureForm
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
            this.textBoxExcelPath = new System.Windows.Forms.TextBox();
            this.buttonSelectFile = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.buttonOK = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.dataGridView_UnitInfo = new System.Windows.Forms.DataGridView();
            this.Column_UnitID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_UnitName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_LineName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_LineDir = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_startMile = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_EndMile = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_UnitType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_CalcTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_Level = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel3 = new System.Windows.Forms.Panel();
            this.buttonClosed = new System.Windows.Forms.Button();
            this.buttonUnitPictureView = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_UnitInfo)).BeginInit();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "单元信息文件：";
            // 
            // textBoxExcelPath
            // 
            this.textBoxExcelPath.Location = new System.Drawing.Point(102, 8);
            this.textBoxExcelPath.Name = "textBoxExcelPath";
            this.textBoxExcelPath.Size = new System.Drawing.Size(423, 21);
            this.textBoxExcelPath.TabIndex = 1;
            // 
            // buttonSelectFile
            // 
            this.buttonSelectFile.Location = new System.Drawing.Point(542, 6);
            this.buttonSelectFile.Name = "buttonSelectFile";
            this.buttonSelectFile.Size = new System.Drawing.Size(75, 23);
            this.buttonSelectFile.TabIndex = 2;
            this.buttonSelectFile.Text = "选择";
            this.buttonSelectFile.UseVisualStyleBackColor = true;
            this.buttonSelectFile.Click += new System.EventHandler(this.buttonSelectFile_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.buttonOK);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.buttonSelectFile);
            this.panel1.Controls.Add(this.textBoxExcelPath);
            this.panel1.Location = new System.Drawing.Point(5, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(711, 37);
            this.panel1.TabIndex = 3;
            // 
            // buttonOK
            // 
            this.buttonOK.Location = new System.Drawing.Point(623, 6);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 3;
            this.buttonOK.Text = "确定";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.dataGridView_UnitInfo);
            this.panel2.Location = new System.Drawing.Point(5, 55);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(711, 216);
            this.panel2.TabIndex = 4;
            // 
            // dataGridView_UnitInfo
            // 
            this.dataGridView_UnitInfo.AllowUserToAddRows = false;
            this.dataGridView_UnitInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_UnitInfo.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column_UnitID,
            this.Column_UnitName,
            this.Column_LineName,
            this.Column_LineDir,
            this.Column_startMile,
            this.Column_EndMile,
            this.Column_UnitType,
            this.Column_CalcTime,
            this.Column_Level});
            this.dataGridView_UnitInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView_UnitInfo.Location = new System.Drawing.Point(0, 0);
            this.dataGridView_UnitInfo.Name = "dataGridView_UnitInfo";
            this.dataGridView_UnitInfo.ReadOnly = true;
            this.dataGridView_UnitInfo.RowTemplate.Height = 23;
            this.dataGridView_UnitInfo.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView_UnitInfo.Size = new System.Drawing.Size(711, 216);
            this.dataGridView_UnitInfo.TabIndex = 0;
            this.dataGridView_UnitInfo.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dataGridView_RowPostPaint);
            // 
            // Column_UnitID
            // 
            this.Column_UnitID.HeaderText = "单元编号";
            this.Column_UnitID.Name = "Column_UnitID";
            this.Column_UnitID.ReadOnly = true;
            this.Column_UnitID.Width = 80;
            // 
            // Column_UnitName
            // 
            this.Column_UnitName.HeaderText = "单元名称";
            this.Column_UnitName.Name = "Column_UnitName";
            this.Column_UnitName.ReadOnly = true;
            this.Column_UnitName.Width = 160;
            // 
            // Column_LineName
            // 
            this.Column_LineName.HeaderText = "线路";
            this.Column_LineName.Name = "Column_LineName";
            this.Column_LineName.ReadOnly = true;
            this.Column_LineName.Width = 80;
            // 
            // Column_LineDir
            // 
            this.Column_LineDir.HeaderText = "行别";
            this.Column_LineDir.Name = "Column_LineDir";
            this.Column_LineDir.ReadOnly = true;
            this.Column_LineDir.Width = 50;
            // 
            // Column_startMile
            // 
            this.Column_startMile.HeaderText = "单元起止里程";
            this.Column_startMile.Name = "Column_startMile";
            this.Column_startMile.ReadOnly = true;
            this.Column_startMile.Width = 60;
            // 
            // Column_EndMile
            // 
            this.Column_EndMile.HeaderText = "单元结束里程";
            this.Column_EndMile.Name = "Column_EndMile";
            this.Column_EndMile.ReadOnly = true;
            this.Column_EndMile.Width = 60;
            // 
            // Column_UnitType
            // 
            this.Column_UnitType.HeaderText = "设备类型";
            this.Column_UnitType.Name = "Column_UnitType";
            this.Column_UnitType.ReadOnly = true;
            this.Column_UnitType.Width = 50;
            // 
            // Column_CalcTime
            // 
            this.Column_CalcTime.HeaderText = "计算时间";
            this.Column_CalcTime.Name = "Column_CalcTime";
            this.Column_CalcTime.ReadOnly = true;
            // 
            // Column_Level
            // 
            this.Column_Level.HeaderText = "级别";
            this.Column_Level.Name = "Column_Level";
            this.Column_Level.ReadOnly = true;
            this.Column_Level.Width = 30;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.buttonClosed);
            this.panel3.Controls.Add(this.buttonUnitPictureView);
            this.panel3.Location = new System.Drawing.Point(5, 277);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(711, 29);
            this.panel3.TabIndex = 5;
            // 
            // buttonClosed
            // 
            this.buttonClosed.Location = new System.Drawing.Point(623, 3);
            this.buttonClosed.Name = "buttonClosed";
            this.buttonClosed.Size = new System.Drawing.Size(75, 23);
            this.buttonClosed.TabIndex = 1;
            this.buttonClosed.Text = "关闭";
            this.buttonClosed.UseVisualStyleBackColor = true;
            this.buttonClosed.Click += new System.EventHandler(this.buttonClosed_Click);
            // 
            // buttonUnitPictureView
            // 
            this.buttonUnitPictureView.Location = new System.Drawing.Point(542, 3);
            this.buttonUnitPictureView.Name = "buttonUnitPictureView";
            this.buttonUnitPictureView.Size = new System.Drawing.Size(75, 23);
            this.buttonUnitPictureView.TabIndex = 0;
            this.buttonUnitPictureView.Text = "截图预览";
            this.buttonUnitPictureView.UseVisualStyleBackColor = true;
            this.buttonUnitPictureView.Click += new System.EventHandler(this.buttonUnitPictureView_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // UnitCutPictureForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(728, 311);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "UnitCutPictureForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "单元信息截图";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_UnitInfo)).EndInit();
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxExcelPath;
        private System.Windows.Forms.Button buttonSelectFile;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.DataGridView dataGridView_UnitInfo;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button buttonClosed;
        private System.Windows.Forms.Button buttonUnitPictureView;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_UnitID;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_UnitName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_LineName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_LineDir;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_startMile;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_EndMile;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_UnitType;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_CalcTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_Level;
    }
}