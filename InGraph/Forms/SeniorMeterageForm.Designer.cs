namespace InGraph.Forms
{
    partial class SeniorMeterageForm
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
            this.textBoxConfigFile = new System.Windows.Forms.TextBox();
            this.buttonLoadConfigFile = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxResultFile = new System.Windows.Forms.TextBox();
            this.buttonResultFile = new System.Windows.Forms.Button();
            this.buttonExecute = new System.Windows.Forms.Button();
            this.buttonCancer = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(26, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "配置文件：";
            // 
            // textBoxConfigFile
            // 
            this.textBoxConfigFile.Location = new System.Drawing.Point(97, 28);
            this.textBoxConfigFile.Name = "textBoxConfigFile";
            this.textBoxConfigFile.Size = new System.Drawing.Size(481, 21);
            this.textBoxConfigFile.TabIndex = 1;
            this.textBoxConfigFile.TextChanged += new System.EventHandler(this.textBoxConfigFile_TextChanged);
            // 
            // buttonLoadConfigFile
            // 
            this.buttonLoadConfigFile.Location = new System.Drawing.Point(603, 26);
            this.buttonLoadConfigFile.Name = "buttonLoadConfigFile";
            this.buttonLoadConfigFile.Size = new System.Drawing.Size(75, 23);
            this.buttonLoadConfigFile.TabIndex = 2;
            this.buttonLoadConfigFile.Text = "加载";
            this.buttonLoadConfigFile.UseVisualStyleBackColor = true;
            this.buttonLoadConfigFile.Click += new System.EventHandler(this.buttonLoadConfigFile_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(26, 58);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "测量结果：";
            // 
            // textBoxResultFile
            // 
            this.textBoxResultFile.Location = new System.Drawing.Point(97, 55);
            this.textBoxResultFile.Name = "textBoxResultFile";
            this.textBoxResultFile.Size = new System.Drawing.Size(481, 21);
            this.textBoxResultFile.TabIndex = 4;
            // 
            // buttonResultFile
            // 
            this.buttonResultFile.Location = new System.Drawing.Point(603, 53);
            this.buttonResultFile.Name = "buttonResultFile";
            this.buttonResultFile.Size = new System.Drawing.Size(75, 23);
            this.buttonResultFile.TabIndex = 5;
            this.buttonResultFile.Text = "浏览";
            this.buttonResultFile.UseVisualStyleBackColor = true;
            this.buttonResultFile.Visible = false;
            // 
            // buttonExecute
            // 
            this.buttonExecute.Location = new System.Drawing.Point(503, 97);
            this.buttonExecute.Name = "buttonExecute";
            this.buttonExecute.Size = new System.Drawing.Size(75, 23);
            this.buttonExecute.TabIndex = 6;
            this.buttonExecute.Text = "执行";
            this.buttonExecute.UseVisualStyleBackColor = true;
            this.buttonExecute.Click += new System.EventHandler(this.buttonExecute_Click);
            // 
            // buttonCancer
            // 
            this.buttonCancer.Location = new System.Drawing.Point(603, 97);
            this.buttonCancer.Name = "buttonCancer";
            this.buttonCancer.Size = new System.Drawing.Size(75, 23);
            this.buttonCancer.TabIndex = 7;
            this.buttonCancer.Text = "取消";
            this.buttonCancer.UseVisualStyleBackColor = true;
            this.buttonCancer.Click += new System.EventHandler(this.buttonCancer_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 349);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.statusStrip1.Size = new System.Drawing.Size(705, 29);
            this.statusStrip1.TabIndex = 8;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.AutoSize = false;
            this.toolStripStatusLabel1.Font = new System.Drawing.Font("Microsoft YaHei UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.toolStripStatusLabel1.ForeColor = System.Drawing.Color.OrangeRed;
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(300, 24);
            this.toolStripStatusLabel1.Text = "启动完毕";
            // 
            // SeniorMeterageForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(705, 378);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.buttonCancer);
            this.Controls.Add(this.buttonExecute);
            this.Controls.Add(this.buttonResultFile);
            this.Controls.Add(this.textBoxResultFile);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.buttonLoadConfigFile);
            this.Controls.Add(this.textBoxConfigFile);
            this.Controls.Add(this.label1);
            this.Name = "SeniorMeterageForm";
            this.Text = "SeniorMeterageForm";
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxConfigFile;
        private System.Windows.Forms.Button buttonLoadConfigFile;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxResultFile;
        private System.Windows.Forms.Button buttonResultFile;
        private System.Windows.Forms.Button buttonExecute;
        private System.Windows.Forms.Button buttonCancer;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
    }
}