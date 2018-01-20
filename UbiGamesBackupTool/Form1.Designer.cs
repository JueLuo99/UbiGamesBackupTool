namespace UbiGamesBackupTool
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.buttonBackup = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.buttonBackupFrom = new System.Windows.Forms.Button();
            this.folderBrowserDialog2 = new System.Windows.Forms.FolderBrowserDialog();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.buttonBackupTo = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonBackup
            // 
            this.buttonBackup.Location = new System.Drawing.Point(79, 154);
            this.buttonBackup.Name = "buttonBackup";
            this.buttonBackup.Size = new System.Drawing.Size(99, 52);
            this.buttonBackup.TabIndex = 0;
            this.buttonBackup.Text = "Backup";
            this.buttonBackup.UseVisualStyleBackColor = true;
            this.buttonBackup.Click += new System.EventHandler(this.buttonBackup_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(12, 26);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(305, 21);
            this.textBox1.TabIndex = 1;
            // 
            // buttonBackupFrom
            // 
            this.buttonBackupFrom.Location = new System.Drawing.Point(340, 26);
            this.buttonBackupFrom.Name = "buttonBackupFrom";
            this.buttonBackupFrom.Size = new System.Drawing.Size(73, 23);
            this.buttonBackupFrom.TabIndex = 2;
            this.buttonBackupFrom.Text = "备份自...";
            this.buttonBackupFrom.UseVisualStyleBackColor = true;
            this.buttonBackupFrom.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(12, 88);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(305, 21);
            this.textBox2.TabIndex = 3;
            // 
            // buttonBackupTo
            // 
            this.buttonBackupTo.Location = new System.Drawing.Point(340, 88);
            this.buttonBackupTo.Name = "buttonBackupTo";
            this.buttonBackupTo.Size = new System.Drawing.Size(75, 23);
            this.buttonBackupTo.TabIndex = 4;
            this.buttonBackupTo.Text = "备份到...";
            this.buttonBackupTo.UseVisualStyleBackColor = true;
            this.buttonBackupTo.Click += new System.EventHandler(this.buttonBackupTo_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(506, 227);
            this.Controls.Add(this.buttonBackupTo);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.buttonBackupFrom);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.buttonBackup);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonBackup;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button buttonBackupFrom;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog2;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Button buttonBackupTo;
    }
}

