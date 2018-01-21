namespace UbiGamesBackupTool
{
    partial class FormMain
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
            this.folderBrowserDialogBackupFrom = new System.Windows.Forms.FolderBrowserDialog();
            this.textBoxBackupFrom = new System.Windows.Forms.TextBox();
            this.buttonBackupFrom = new System.Windows.Forms.Button();
            this.folderBrowserDialogBackupTo = new System.Windows.Forms.FolderBrowserDialog();
            this.textBoxBackupTo = new System.Windows.Forms.TextBox();
            this.buttonBackupTo = new System.Windows.Forms.Button();
            this.labelTip1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // buttonBackup
            // 
            this.buttonBackup.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.buttonBackup.Location = new System.Drawing.Point(37, 153);
            this.buttonBackup.Name = "buttonBackup";
            this.buttonBackup.Size = new System.Drawing.Size(99, 52);
            this.buttonBackup.TabIndex = 0;
            this.buttonBackup.Text = "开始备份";
            this.buttonBackup.UseVisualStyleBackColor = true;
            this.buttonBackup.Click += new System.EventHandler(this.buttonBackup_Click);
            // 
            // textBoxBackupFrom
            // 
            this.textBoxBackupFrom.Location = new System.Drawing.Point(12, 26);
            this.textBoxBackupFrom.Name = "textBoxBackupFrom";
            this.textBoxBackupFrom.Size = new System.Drawing.Size(305, 21);
            this.textBoxBackupFrom.TabIndex = 1;
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
            // textBoxBackupTo
            // 
            this.textBoxBackupTo.Location = new System.Drawing.Point(12, 88);
            this.textBoxBackupTo.Name = "textBoxBackupTo";
            this.textBoxBackupTo.Size = new System.Drawing.Size(305, 21);
            this.textBoxBackupTo.TabIndex = 3;
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
            // labelTip1
            // 
            this.labelTip1.AutoSize = true;
            this.labelTip1.Location = new System.Drawing.Point(10, 60);
            this.labelTip1.Name = "labelTip1";
            this.labelTip1.Size = new System.Drawing.Size(101, 12);
            this.labelTip1.TabIndex = 5;
            this.labelTip1.Text = "自动检测到的路径";
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(506, 227);
            this.Controls.Add(this.labelTip1);
            this.Controls.Add(this.buttonBackupTo);
            this.Controls.Add(this.textBoxBackupTo);
            this.Controls.Add(this.buttonBackupFrom);
            this.Controls.Add(this.textBoxBackupFrom);
            this.Controls.Add(this.buttonBackup);
            this.Name = "FormMain";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonBackup;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialogBackupFrom;
        private System.Windows.Forms.TextBox textBoxBackupFrom;
        private System.Windows.Forms.Button buttonBackupFrom;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialogBackupTo;
        private System.Windows.Forms.TextBox textBoxBackupTo;
        private System.Windows.Forms.Button buttonBackupTo;
        private System.Windows.Forms.Label labelTip1;
    }
}

