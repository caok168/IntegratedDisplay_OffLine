namespace InGraph
{
    partial class InvalidDataAddForm
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
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.textBoxStartPoint1 = new System.Windows.Forms.TextBox();
            this.textBoxStartMile1 = new System.Windows.Forms.TextBox();
            this.textBoxEndPoint = new System.Windows.Forms.TextBox();
            this.textBoxEndMile = new System.Windows.Forms.TextBox();
            this.textBoxMemo = new System.Windows.Forms.TextBox();
            this.buttonAdd = new System.Windows.Forms.Button();
            this.buttonHide = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(32, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 23);
            this.label1.TabIndex = 0;
            this.label1.Text = "起始点";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(32, 47);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 23);
            this.label2.TabIndex = 1;
            this.label2.Text = "起始里程";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(32, 70);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(100, 23);
            this.label3.TabIndex = 2;
            this.label3.Text = "终止点";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(32, 93);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(100, 23);
            this.label4.TabIndex = 3;
            this.label4.Text = "终止里程";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(32, 139);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(76, 23);
            this.label5.TabIndex = 4;
            this.label5.Text = "描述";
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(32, 116);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(76, 23);
            this.label6.TabIndex = 5;
            this.label6.Text = "类型";
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(114, 113);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 20);
            this.comboBox1.TabIndex = 6;
            // 
            // textBoxStartPoint1
            // 
            this.textBoxStartPoint1.Location = new System.Drawing.Point(114, 21);
            this.textBoxStartPoint1.Name = "textBoxStartPoint1";
            this.textBoxStartPoint1.ReadOnly = true;
            this.textBoxStartPoint1.Size = new System.Drawing.Size(121, 21);
            this.textBoxStartPoint1.TabIndex = 7;
            // 
            // textBoxStartMile1
            // 
            this.textBoxStartMile1.Location = new System.Drawing.Point(114, 44);
            this.textBoxStartMile1.Name = "textBoxStartMile1";
            this.textBoxStartMile1.ReadOnly = true;
            this.textBoxStartMile1.Size = new System.Drawing.Size(121, 21);
            this.textBoxStartMile1.TabIndex = 8;
            // 
            // textBoxEndPoint
            // 
            this.textBoxEndPoint.Location = new System.Drawing.Point(114, 67);
            this.textBoxEndPoint.Name = "textBoxEndPoint";
            this.textBoxEndPoint.ReadOnly = true;
            this.textBoxEndPoint.Size = new System.Drawing.Size(121, 21);
            this.textBoxEndPoint.TabIndex = 9;
            // 
            // textBoxEndMile
            // 
            this.textBoxEndMile.Location = new System.Drawing.Point(114, 90);
            this.textBoxEndMile.Name = "textBoxEndMile";
            this.textBoxEndMile.ReadOnly = true;
            this.textBoxEndMile.Size = new System.Drawing.Size(121, 21);
            this.textBoxEndMile.TabIndex = 10;
            // 
            // textBoxMemo
            // 
            this.textBoxMemo.Location = new System.Drawing.Point(114, 136);
            this.textBoxMemo.Multiline = true;
            this.textBoxMemo.Name = "textBoxMemo";
            this.textBoxMemo.Size = new System.Drawing.Size(121, 89);
            this.textBoxMemo.TabIndex = 11;
            // 
            // buttonAdd
            // 
            this.buttonAdd.Location = new System.Drawing.Point(34, 232);
            this.buttonAdd.Name = "buttonAdd";
            this.buttonAdd.Size = new System.Drawing.Size(75, 23);
            this.buttonAdd.TabIndex = 12;
            this.buttonAdd.Text = "添加";
            this.buttonAdd.UseVisualStyleBackColor = true;
            this.buttonAdd.Click += new System.EventHandler(this.buttonAdd_Click);
            // 
            // buttonHide
            // 
            this.buttonHide.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonHide.Location = new System.Drawing.Point(179, 232);
            this.buttonHide.Name = "buttonHide";
            this.buttonHide.Size = new System.Drawing.Size(75, 23);
            this.buttonHide.TabIndex = 13;
            this.buttonHide.Text = "关闭";
            this.buttonHide.UseVisualStyleBackColor = true;
            this.buttonHide.Click += new System.EventHandler(this.buttonHide_Click);
            // 
            // InvalidDataAddForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonHide;
            this.ClientSize = new System.Drawing.Size(280, 267);
            this.Controls.Add(this.buttonHide);
            this.Controls.Add(this.buttonAdd);
            this.Controls.Add(this.textBoxMemo);
            this.Controls.Add(this.textBoxEndMile);
            this.Controls.Add(this.textBoxEndPoint);
            this.Controls.Add(this.textBoxStartMile1);
            this.Controls.Add(this.textBoxStartPoint1);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "InvalidDataAddForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "无效数据添加窗口";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.InvalidDataAddForm_FormClosing);
            this.Load += new System.EventHandler(this.InvalidDataAddForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button buttonAdd;
        private System.Windows.Forms.Button buttonHide;
        protected internal System.Windows.Forms.ComboBox comboBox1;
        protected internal System.Windows.Forms.TextBox textBoxStartPoint1;
        protected internal System.Windows.Forms.TextBox textBoxStartMile1;
        protected internal System.Windows.Forms.TextBox textBoxEndPoint;
        protected internal System.Windows.Forms.TextBox textBoxEndMile;
        protected internal System.Windows.Forms.TextBox textBoxMemo;
    }
}