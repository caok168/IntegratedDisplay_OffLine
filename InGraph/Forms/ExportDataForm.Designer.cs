namespace InGraph
{
    partial class ExportDataForm
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
            this.buttonReturn = new System.Windows.Forms.Button();
            this.buttonExport = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxMileStart = new System.Windows.Forms.TextBox();
            this.textBoxMileEnd = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // buttonReturn
            // 
            this.buttonReturn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonReturn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonReturn.Location = new System.Drawing.Point(184, 92);
            this.buttonReturn.Name = "buttonReturn";
            this.buttonReturn.Size = new System.Drawing.Size(106, 25);
            this.buttonReturn.TabIndex = 0;
            this.buttonReturn.Text = "返回(&R)";
            this.buttonReturn.UseVisualStyleBackColor = true;
            // 
            // buttonExport
            // 
            this.buttonExport.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonExport.Location = new System.Drawing.Point(26, 92);
            this.buttonExport.Name = "buttonExport";
            this.buttonExport.Size = new System.Drawing.Size(106, 26);
            this.buttonExport.TabIndex = 1;
            this.buttonExport.Text = "导出(&E)";
            this.buttonExport.UseVisualStyleBackColor = true;
            this.buttonExport.Click += new System.EventHandler(this.buttonExport_Click);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(12, 29);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(143, 12);
            this.label2.TabIndex = 5;
            this.label2.Text = "起始里程(Km.Meter)";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxMileStart
            // 
            this.textBoxMileStart.Location = new System.Drawing.Point(159, 26);
            this.textBoxMileStart.Name = "textBoxMileStart";
            this.textBoxMileStart.Size = new System.Drawing.Size(79, 21);
            this.textBoxMileStart.TabIndex = 6;
            this.textBoxMileStart.Text = "0";
            // 
            // textBoxMileEnd
            // 
            this.textBoxMileEnd.Location = new System.Drawing.Point(159, 55);
            this.textBoxMileEnd.Name = "textBoxMileEnd";
            this.textBoxMileEnd.Size = new System.Drawing.Size(79, 21);
            this.textBoxMileEnd.TabIndex = 8;
            this.textBoxMileEnd.Text = "0";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(14, 58);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(141, 12);
            this.label3.TabIndex = 7;
            this.label3.Text = "终止里程(Km.Meter)";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ExportDataForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(212)))), ((int)(((byte)(217)))), ((int)(((byte)(222)))));
            this.CancelButton = this.buttonReturn;
            this.ClientSize = new System.Drawing.Size(316, 131);
            this.Controls.Add(this.textBoxMileEnd);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBoxMileStart);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.buttonExport);
            this.Controls.Add(this.buttonReturn);
            this.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ExportDataForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "导出波形数据 ";
            this.Load += new System.EventHandler(this.ExportDataForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonReturn;
        private System.Windows.Forms.Button buttonExport;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxMileStart;
        private System.Windows.Forms.TextBox textBoxMileEnd;
        private System.Windows.Forms.Label label3;
    }
}