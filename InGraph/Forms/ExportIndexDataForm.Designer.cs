namespace InGraph.Forms
{
    partial class ExportIndexDataForm
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
            this.PWDTextBox = new System.Windows.Forms.TextBox();
            this.ExportButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.StartMileTextBox = new System.Windows.Forms.TextBox();
            this.EndMileTextBox = new System.Windows.Forms.TextBox();
            this.checkedListBox1 = new System.Windows.Forms.CheckedListBox();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label1.Location = new System.Drawing.Point(462, 195);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 23);
            this.label1.TabIndex = 0;
            this.label1.Text = "导出密码";
            // 
            // PWDTextBox
            // 
            this.PWDTextBox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.PWDTextBox.Location = new System.Drawing.Point(527, 192);
            this.PWDTextBox.MaxLength = 11;
            this.PWDTextBox.Name = "PWDTextBox";
            this.PWDTextBox.PasswordChar = '*';
            this.PWDTextBox.Size = new System.Drawing.Size(95, 21);
            this.PWDTextBox.TabIndex = 1;
            // 
            // ExportButton
            // 
            this.ExportButton.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.ExportButton.Location = new System.Drawing.Point(464, 239);
            this.ExportButton.Name = "ExportButton";
            this.ExportButton.Size = new System.Drawing.Size(75, 23);
            this.ExportButton.TabIndex = 2;
            this.ExportButton.Text = "导出(&E)";
            this.ExportButton.UseVisualStyleBackColor = true;
            this.ExportButton.Click += new System.EventHandler(this.ExportButton_Click);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(462, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 23);
            this.label2.TabIndex = 3;
            this.label2.Text = "起始里程";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(462, 47);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 23);
            this.label3.TabIndex = 4;
            this.label3.Text = "终止里程";
            // 
            // StartMileTextBox
            // 
            this.StartMileTextBox.Location = new System.Drawing.Point(527, 6);
            this.StartMileTextBox.MaxLength = 10;
            this.StartMileTextBox.Name = "StartMileTextBox";
            this.StartMileTextBox.Size = new System.Drawing.Size(95, 21);
            this.StartMileTextBox.TabIndex = 5;
            // 
            // EndMileTextBox
            // 
            this.EndMileTextBox.Location = new System.Drawing.Point(527, 44);
            this.EndMileTextBox.MaxLength = 10;
            this.EndMileTextBox.Name = "EndMileTextBox";
            this.EndMileTextBox.Size = new System.Drawing.Size(95, 21);
            this.EndMileTextBox.TabIndex = 6;
            // 
            // checkedListBox1
            // 
            this.checkedListBox1.CheckOnClick = true;
            this.checkedListBox1.FormattingEnabled = true;
            this.checkedListBox1.Location = new System.Drawing.Point(12, 6);
            this.checkedListBox1.MultiColumn = true;
            this.checkedListBox1.Name = "checkedListBox1";
            this.checkedListBox1.ScrollAlwaysVisible = true;
            this.checkedListBox1.Size = new System.Drawing.Size(436, 260);
            this.checkedListBox1.TabIndex = 7;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(547, 239);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 8;
            this.button1.Text = "关闭(&C)";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // ExportIndexDataForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(217)))), ((int)(((byte)(222)))));
            this.ClientSize = new System.Drawing.Size(634, 274);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.checkedListBox1);
            this.Controls.Add(this.EndMileTextBox);
            this.Controls.Add(this.StartMileTextBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.ExportButton);
            this.Controls.Add(this.PWDTextBox);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ExportIndexDataForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "精确导出数据";
            this.Load += new System.EventHandler(this.ExportIndexDataForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox PWDTextBox;
        private System.Windows.Forms.Button ExportButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox StartMileTextBox;
        private System.Windows.Forms.TextBox EndMileTextBox;
        private System.Windows.Forms.CheckedListBox checkedListBox1;
        private System.Windows.Forms.Button button1;
    }
}