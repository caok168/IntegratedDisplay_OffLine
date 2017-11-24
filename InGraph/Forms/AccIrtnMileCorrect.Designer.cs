namespace InGraph.Forms
{
    partial class AccIrtnMileCorrect
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
            this.txt_fs = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txt_thresh_curve = new System.Windows.Forms.TextBox();
            this.btnProcess = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txt_CitFilePath = new System.Windows.Forms.TextBox();
            this.txt_CurveFilePath = new System.Windows.Forms.TextBox();
            this.txt_AbruptMileFilePath = new System.Windows.Forms.TextBox();
            this.btn_CitSelect = new System.Windows.Forms.Button();
            this.btn_CurveSelect = new System.Windows.Forms.Button();
            this.btn_AbruptMileSelect = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(61, 97);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "采样频率：";
            // 
            // txt_fs
            // 
            this.txt_fs.Location = new System.Drawing.Point(132, 91);
            this.txt_fs.Name = "txt_fs";
            this.txt_fs.Size = new System.Drawing.Size(100, 21);
            this.txt_fs.TabIndex = 1;
            this.txt_fs.Text = "4";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(262, 96);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "超高控制阈值：";
            // 
            // txt_thresh_curve
            // 
            this.txt_thresh_curve.Location = new System.Drawing.Point(355, 91);
            this.txt_thresh_curve.Name = "txt_thresh_curve";
            this.txt_thresh_curve.Size = new System.Drawing.Size(100, 21);
            this.txt_thresh_curve.TabIndex = 3;
            this.txt_thresh_curve.Text = "50.0";
            // 
            // btnProcess
            // 
            this.btnProcess.Location = new System.Drawing.Point(512, 92);
            this.btnProcess.Name = "btnProcess";
            this.btnProcess.Size = new System.Drawing.Size(75, 23);
            this.btnProcess.TabIndex = 4;
            this.btnProcess.Text = "开始处理";
            this.btnProcess.UseVisualStyleBackColor = true;
            this.btnProcess.Click += new System.EventHandler(this.btnProcess_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(19, 18);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(107, 12);
            this.label3.TabIndex = 6;
            this.label3.Text = "选择轨检cit文件：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 42);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(113, 12);
            this.label4.TabIndex = 7;
            this.label4.Text = "选择曲线台账模板：";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(25, 66);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(101, 12);
            this.label5.TabIndex = 8;
            this.label5.Text = "选择长短链模板：";
            // 
            // txt_CitFilePath
            // 
            this.txt_CitFilePath.Location = new System.Drawing.Point(132, 15);
            this.txt_CitFilePath.Name = "txt_CitFilePath";
            this.txt_CitFilePath.ReadOnly = true;
            this.txt_CitFilePath.Size = new System.Drawing.Size(356, 21);
            this.txt_CitFilePath.TabIndex = 9;
            // 
            // txt_CurveFilePath
            // 
            this.txt_CurveFilePath.Location = new System.Drawing.Point(132, 39);
            this.txt_CurveFilePath.Name = "txt_CurveFilePath";
            this.txt_CurveFilePath.ReadOnly = true;
            this.txt_CurveFilePath.Size = new System.Drawing.Size(356, 21);
            this.txt_CurveFilePath.TabIndex = 10;
            // 
            // txt_AbruptMileFilePath
            // 
            this.txt_AbruptMileFilePath.Location = new System.Drawing.Point(132, 63);
            this.txt_AbruptMileFilePath.Name = "txt_AbruptMileFilePath";
            this.txt_AbruptMileFilePath.ReadOnly = true;
            this.txt_AbruptMileFilePath.Size = new System.Drawing.Size(356, 21);
            this.txt_AbruptMileFilePath.TabIndex = 11;
            // 
            // btn_CitSelect
            // 
            this.btn_CitSelect.Location = new System.Drawing.Point(512, 13);
            this.btn_CitSelect.Name = "btn_CitSelect";
            this.btn_CitSelect.Size = new System.Drawing.Size(75, 23);
            this.btn_CitSelect.TabIndex = 12;
            this.btn_CitSelect.Text = "选  择";
            this.btn_CitSelect.UseVisualStyleBackColor = true;
            this.btn_CitSelect.Click += new System.EventHandler(this.btn_CitSelect_Click);
            // 
            // btn_CurveSelect
            // 
            this.btn_CurveSelect.Location = new System.Drawing.Point(512, 37);
            this.btn_CurveSelect.Name = "btn_CurveSelect";
            this.btn_CurveSelect.Size = new System.Drawing.Size(75, 23);
            this.btn_CurveSelect.TabIndex = 13;
            this.btn_CurveSelect.Text = "选  择";
            this.btn_CurveSelect.UseVisualStyleBackColor = true;
            this.btn_CurveSelect.Click += new System.EventHandler(this.btn_CurveSelect_Click);
            // 
            // btn_AbruptMileSelect
            // 
            this.btn_AbruptMileSelect.Location = new System.Drawing.Point(512, 61);
            this.btn_AbruptMileSelect.Name = "btn_AbruptMileSelect";
            this.btn_AbruptMileSelect.Size = new System.Drawing.Size(75, 23);
            this.btn_AbruptMileSelect.TabIndex = 14;
            this.btn_AbruptMileSelect.Text = "选  择";
            this.btn_AbruptMileSelect.UseVisualStyleBackColor = true;
            this.btn_AbruptMileSelect.Click += new System.EventHandler(this.btn_AbruptMileSelect_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // AccIrtnMileCorrect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(599, 349);
            this.Controls.Add(this.btn_AbruptMileSelect);
            this.Controls.Add(this.btn_CurveSelect);
            this.Controls.Add(this.btn_CitSelect);
            this.Controls.Add(this.txt_AbruptMileFilePath);
            this.Controls.Add(this.txt_CurveFilePath);
            this.Controls.Add(this.txt_CitFilePath);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnProcess);
            this.Controls.Add(this.txt_thresh_curve);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txt_fs);
            this.Controls.Add(this.label1);
            this.Name = "AccIrtnMileCorrect";
            this.ShowIcon = false;
            this.Text = "智能里程修正";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txt_fs;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txt_thresh_curve;
        private System.Windows.Forms.Button btnProcess;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txt_CitFilePath;
        private System.Windows.Forms.TextBox txt_CurveFilePath;
        private System.Windows.Forms.TextBox txt_AbruptMileFilePath;
        private System.Windows.Forms.Button btn_CitSelect;
        private System.Windows.Forms.Button btn_CurveSelect;
        private System.Windows.Forms.Button btn_AbruptMileSelect;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
    }
}