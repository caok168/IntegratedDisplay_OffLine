namespace InGraph.Forms
{
    partial class CorrelationForm
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
            this.buttonClear = new System.Windows.Forms.Button();
            this.buttonFileDelete = new System.Windows.Forms.Button();
            this.buttonFileAdd = new System.Windows.Forms.Button();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.buttonWaveFix = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.textBox目标数据点 = new System.Windows.Forms.TextBox();
            this.textBox原始数据点 = new System.Windows.Forms.TextBox();
            this.textBox右高低门阀值 = new System.Windows.Forms.TextBox();
            this.textBox左高低门阀值 = new System.Windows.Forms.TextBox();
            this.textBox轨距门阀值 = new System.Windows.Forms.TextBox();
            this.textBox超高门阀值 = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonBack = new System.Windows.Forms.Button();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonClear
            // 
            this.buttonClear.Location = new System.Drawing.Point(397, 117);
            this.buttonClear.Name = "buttonClear";
            this.buttonClear.Size = new System.Drawing.Size(75, 23);
            this.buttonClear.TabIndex = 11;
            this.buttonClear.Text = "清空";
            this.buttonClear.UseVisualStyleBackColor = true;
            this.buttonClear.Click += new System.EventHandler(this.buttonClear_Click);
            // 
            // buttonFileDelete
            // 
            this.buttonFileDelete.Location = new System.Drawing.Point(397, 88);
            this.buttonFileDelete.Name = "buttonFileDelete";
            this.buttonFileDelete.Size = new System.Drawing.Size(75, 23);
            this.buttonFileDelete.TabIndex = 10;
            this.buttonFileDelete.Text = "删除文件";
            this.buttonFileDelete.UseVisualStyleBackColor = true;
            this.buttonFileDelete.Click += new System.EventHandler(this.buttonFileDelete_Click);
            // 
            // buttonFileAdd
            // 
            this.buttonFileAdd.Location = new System.Drawing.Point(397, 59);
            this.buttonFileAdd.Name = "buttonFileAdd";
            this.buttonFileAdd.Size = new System.Drawing.Size(75, 23);
            this.buttonFileAdd.TabIndex = 9;
            this.buttonFileAdd.Text = "添加文件";
            this.buttonFileAdd.UseVisualStyleBackColor = true;
            this.buttonFileAdd.Click += new System.EventHandler(this.buttonFileAdd_Click);
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 12;
            this.listBox1.Location = new System.Drawing.Point(14, 36);
            this.listBox1.Name = "listBox1";
            this.listBox1.ScrollAlwaysVisible = true;
            this.listBox1.Size = new System.Drawing.Size(377, 124);
            this.listBox1.TabIndex = 8;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(12, 18);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(139, 15);
            this.label2.TabIndex = 7;
            this.label2.Text = "要修正的波形文件列表";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.Filter = "CIT文件|*.cit";
            // 
            // buttonWaveFix
            // 
            this.buttonWaveFix.Location = new System.Drawing.Point(62, 173);
            this.buttonWaveFix.Name = "buttonWaveFix";
            this.buttonWaveFix.Size = new System.Drawing.Size(75, 23);
            this.buttonWaveFix.TabIndex = 12;
            this.buttonWaveFix.Text = "修正(&P)";
            this.buttonWaveFix.UseVisualStyleBackColor = true;
            this.buttonWaveFix.Click += new System.EventHandler(this.buttonWaveFix_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textBox目标数据点);
            this.groupBox1.Controls.Add(this.textBox原始数据点);
            this.groupBox1.Controls.Add(this.textBox右高低门阀值);
            this.groupBox1.Controls.Add(this.textBox左高低门阀值);
            this.groupBox1.Controls.Add(this.textBox轨距门阀值);
            this.groupBox1.Controls.Add(this.textBox超高门阀值);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(478, 27);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(192, 172);
            this.groupBox1.TabIndex = 13;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "参数配置";
            // 
            // textBox目标数据点
            // 
            this.textBox目标数据点.Location = new System.Drawing.Point(112, 148);
            this.textBox目标数据点.Name = "textBox目标数据点";
            this.textBox目标数据点.Size = new System.Drawing.Size(64, 21);
            this.textBox目标数据点.TabIndex = 15;
            this.textBox目标数据点.Text = "8000";
            // 
            // textBox原始数据点
            // 
            this.textBox原始数据点.Location = new System.Drawing.Point(112, 123);
            this.textBox原始数据点.Name = "textBox原始数据点";
            this.textBox原始数据点.Size = new System.Drawing.Size(64, 21);
            this.textBox原始数据点.TabIndex = 14;
            this.textBox原始数据点.Text = "400";
            // 
            // textBox右高低门阀值
            // 
            this.textBox右高低门阀值.Location = new System.Drawing.Point(113, 100);
            this.textBox右高低门阀值.Name = "textBox右高低门阀值";
            this.textBox右高低门阀值.Size = new System.Drawing.Size(64, 21);
            this.textBox右高低门阀值.TabIndex = 13;
            this.textBox右高低门阀值.Text = "0.8";
            // 
            // textBox左高低门阀值
            // 
            this.textBox左高低门阀值.Location = new System.Drawing.Point(113, 73);
            this.textBox左高低门阀值.Name = "textBox左高低门阀值";
            this.textBox左高低门阀值.Size = new System.Drawing.Size(64, 21);
            this.textBox左高低门阀值.TabIndex = 12;
            this.textBox左高低门阀值.Text = "0.8";
            // 
            // textBox轨距门阀值
            // 
            this.textBox轨距门阀值.Location = new System.Drawing.Point(113, 46);
            this.textBox轨距门阀值.Name = "textBox轨距门阀值";
            this.textBox轨距门阀值.Size = new System.Drawing.Size(64, 21);
            this.textBox轨距门阀值.TabIndex = 11;
            this.textBox轨距门阀值.Text = "0.8";
            // 
            // textBox超高门阀值
            // 
            this.textBox超高门阀值.Location = new System.Drawing.Point(113, 21);
            this.textBox超高门阀值.Name = "textBox超高门阀值";
            this.textBox超高门阀值.Size = new System.Drawing.Size(64, 21);
            this.textBox超高门阀值.TabIndex = 10;
            this.textBox超高门阀值.Text = "0.8";
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(6, 121);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(100, 23);
            this.label7.TabIndex = 9;
            this.label7.Text = "原始数据点";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(6, 146);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(100, 23);
            this.label6.TabIndex = 8;
            this.label6.Text = "目标数据点";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(6, 98);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(100, 23);
            this.label5.TabIndex = 3;
            this.label5.Text = "右高低门阚值";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(6, 71);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(100, 23);
            this.label4.TabIndex = 2;
            this.label4.Text = "左高低门阚值";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(6, 19);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(100, 23);
            this.label3.TabIndex = 1;
            this.label3.Text = "超高门阚值";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(6, 44);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 23);
            this.label1.TabIndex = 0;
            this.label1.Text = "轨距门阚值";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // buttonBack
            // 
            this.buttonBack.Location = new System.Drawing.Point(276, 173);
            this.buttonBack.Name = "buttonBack";
            this.buttonBack.Size = new System.Drawing.Size(75, 23);
            this.buttonBack.TabIndex = 14;
            this.buttonBack.Text = "返回(&B)";
            this.buttonBack.UseVisualStyleBackColor = true;
            this.buttonBack.Click += new System.EventHandler(this.buttonBack_Click);
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // CorrelationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(680, 211);
            this.Controls.Add(this.buttonBack);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.buttonWaveFix);
            this.Controls.Add(this.buttonClear);
            this.Controls.Add(this.buttonFileDelete);
            this.Controls.Add(this.buttonFileAdd);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.label2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CorrelationForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "里程智能校正";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonClear;
        private System.Windows.Forms.Button buttonFileDelete;
        private System.Windows.Forms.Button buttonFileAdd;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button buttonWaveFix;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textBox超高门阀值;
        private System.Windows.Forms.TextBox textBox目标数据点;
        private System.Windows.Forms.TextBox textBox原始数据点;
        private System.Windows.Forms.TextBox textBox右高低门阀值;
        private System.Windows.Forms.TextBox textBox左高低门阀值;
        private System.Windows.Forms.TextBox textBox轨距门阀值;
        private System.Windows.Forms.Button buttonBack;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;

    }
}