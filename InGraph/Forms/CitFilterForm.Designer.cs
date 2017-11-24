namespace InGraph.Forms
{
    partial class CitFilterForm
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
            this.groupBoxGeoSelect = new System.Windows.Forms.GroupBox();
            this.textBoxCitPath = new System.Windows.Forms.TextBox();
            this.btnGeoSelect = new System.Windows.Forms.Button();
            this.buttonCitFilter = new System.Windows.Forms.Button();
            this.labelTrainCode = new System.Windows.Forms.Label();
            this.labelLineName = new System.Windows.Forms.Label();
            this.comboBoxTrainCode = new System.Windows.Forms.ComboBox();
            this.comboBoxLineName = new System.Windows.Forms.ComboBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.panel1 = new System.Windows.Forms.Panel();
            this.checkBoxIs0haoche = new System.Windows.Forms.CheckBox();
            this.groupBoxGeoSelect.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxGeoSelect
            // 
            this.groupBoxGeoSelect.Controls.Add(this.textBoxCitPath);
            this.groupBoxGeoSelect.Controls.Add(this.btnGeoSelect);
            this.groupBoxGeoSelect.Location = new System.Drawing.Point(9, 13);
            this.groupBoxGeoSelect.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.groupBoxGeoSelect.Name = "groupBoxGeoSelect";
            this.groupBoxGeoSelect.Padding = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.groupBoxGeoSelect.Size = new System.Drawing.Size(495, 105);
            this.groupBoxGeoSelect.TabIndex = 1;
            this.groupBoxGeoSelect.TabStop = false;
            this.groupBoxGeoSelect.Text = "选择CIT波形";
            // 
            // textBoxCitPath
            // 
            this.textBoxCitPath.Location = new System.Drawing.Point(6, 26);
            this.textBoxCitPath.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.textBoxCitPath.Multiline = true;
            this.textBoxCitPath.Name = "textBoxCitPath";
            this.textBoxCitPath.ReadOnly = true;
            this.textBoxCitPath.Size = new System.Drawing.Size(376, 68);
            this.textBoxCitPath.TabIndex = 1;
            // 
            // btnGeoSelect
            // 
            this.btnGeoSelect.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnGeoSelect.Location = new System.Drawing.Point(406, 26);
            this.btnGeoSelect.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.btnGeoSelect.Name = "btnGeoSelect";
            this.btnGeoSelect.Size = new System.Drawing.Size(78, 68);
            this.btnGeoSelect.TabIndex = 0;
            this.btnGeoSelect.Text = "打开波形";
            this.btnGeoSelect.UseVisualStyleBackColor = true;
            this.btnGeoSelect.Click += new System.EventHandler(this.btnGeoSelect_Click);
            // 
            // buttonCitFilter
            // 
            this.buttonCitFilter.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.buttonCitFilter.Location = new System.Drawing.Point(369, 173);
            this.buttonCitFilter.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.buttonCitFilter.Name = "buttonCitFilter";
            this.buttonCitFilter.Size = new System.Drawing.Size(135, 38);
            this.buttonCitFilter.TabIndex = 6;
            this.buttonCitFilter.Text = "CIT滤波";
            this.buttonCitFilter.UseVisualStyleBackColor = true;
            this.buttonCitFilter.Click += new System.EventHandler(this.buttonCitFilter_Click);
            // 
            // labelTrainCode
            // 
            this.labelTrainCode.AutoSize = true;
            this.labelTrainCode.Location = new System.Drawing.Point(14, 9);
            this.labelTrainCode.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelTrainCode.Name = "labelTrainCode";
            this.labelTrainCode.Size = new System.Drawing.Size(65, 20);
            this.labelTrainCode.TabIndex = 1;
            this.labelTrainCode.Text = "检测车号";
            this.labelTrainCode.Visible = false;
            // 
            // labelLineName
            // 
            this.labelLineName.AutoSize = true;
            this.labelLineName.Location = new System.Drawing.Point(289, 9);
            this.labelLineName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelLineName.Name = "labelLineName";
            this.labelLineName.Size = new System.Drawing.Size(65, 20);
            this.labelLineName.TabIndex = 1;
            this.labelLineName.Text = "线路名称";
            // 
            // comboBoxTrainCode
            // 
            this.comboBoxTrainCode.FormattingEnabled = true;
            this.comboBoxTrainCode.Items.AddRange(new object[] {
            "CIT001"});
            this.comboBoxTrainCode.Location = new System.Drawing.Point(90, 6);
            this.comboBoxTrainCode.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.comboBoxTrainCode.Name = "comboBoxTrainCode";
            this.comboBoxTrainCode.Size = new System.Drawing.Size(122, 28);
            this.comboBoxTrainCode.TabIndex = 0;
            this.comboBoxTrainCode.Visible = false;
            // 
            // comboBoxLineName
            // 
            this.comboBoxLineName.Enabled = false;
            this.comboBoxLineName.FormattingEnabled = true;
            this.comboBoxLineName.Location = new System.Drawing.Point(365, 6);
            this.comboBoxLineName.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.comboBoxLineName.Name = "comboBoxLineName";
            this.comboBoxLineName.Size = new System.Drawing.Size(122, 28);
            this.comboBoxLineName.TabIndex = 0;
            this.comboBoxLineName.SelectedIndexChanged += new System.EventHandler(this.comboBoxLineName_SelectedIndexChanged);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.Filter = "CIT波形文件|*.cit";
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.checkBoxIs0haoche);
            this.panel1.Controls.Add(this.labelLineName);
            this.panel1.Controls.Add(this.comboBoxLineName);
            this.panel1.Controls.Add(this.labelTrainCode);
            this.panel1.Controls.Add(this.comboBoxTrainCode);
            this.panel1.Location = new System.Drawing.Point(14, 127);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(490, 38);
            this.panel1.TabIndex = 7;
            // 
            // checkBoxIs0haoche
            // 
            this.checkBoxIs0haoche.AutoSize = true;
            this.checkBoxIs0haoche.Location = new System.Drawing.Point(225, 8);
            this.checkBoxIs0haoche.Name = "checkBoxIs0haoche";
            this.checkBoxIs0haoche.Size = new System.Drawing.Size(64, 24);
            this.checkBoxIs0haoche.TabIndex = 2;
            this.checkBoxIs0haoche.Text = "0号车";
            this.checkBoxIs0haoche.UseVisualStyleBackColor = true;
            this.checkBoxIs0haoche.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // CitFilterForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(514, 214);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.buttonCitFilter);
            this.Controls.Add(this.groupBoxGeoSelect);
            this.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CitFilterForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "高频滤波";
            this.groupBoxGeoSelect.ResumeLayout(false);
            this.groupBoxGeoSelect.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxGeoSelect;
        private System.Windows.Forms.TextBox textBoxCitPath;
        private System.Windows.Forms.Button btnGeoSelect;
        private System.Windows.Forms.Button buttonCitFilter;
        private System.Windows.Forms.Label labelTrainCode;
        private System.Windows.Forms.Label labelLineName;
        private System.Windows.Forms.ComboBox comboBoxTrainCode;
        private System.Windows.Forms.ComboBox comboBoxLineName;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.CheckBox checkBoxIs0haoche;
    }
}