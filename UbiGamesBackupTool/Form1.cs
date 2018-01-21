using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UbiGamesBackupTool
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }




        private void CopyDirectory(string srcdir, string desdir)
        {
            string folderName = srcdir.Substring(srcdir.LastIndexOf("\\") + 1);

            string desfolderdir = desdir + "\\" + folderName;

            if (desdir.LastIndexOf("\\") == (desdir.Length - 1))
            {
                desfolderdir = desdir + folderName;
            }
            string[] filenames = Directory.GetFileSystemEntries(srcdir);

            foreach (string file in filenames)// 遍历所有的文件和目录
            {
                if (Directory.Exists(file))// 先当作目录处理 如果存在这个目录就递归Copy该目录下面的文件
                {

                    string currentdir = desfolderdir + "\\" + file.Substring(file.LastIndexOf("\\") + 1);
                    if (!Directory.Exists(currentdir))
                    {
                        Directory.CreateDirectory(currentdir);
                    }

                    CopyDirectory(file, desfolderdir);
                }

                else // 否则直接copy文件
                {
                    string srcfileName = file.Substring(file.LastIndexOf("\\") + 1);

                    srcfileName = desfolderdir + "\\" + srcfileName;


                    if (!Directory.Exists(desfolderdir))
                    {
                        Directory.CreateDirectory(desfolderdir);
                    }


                    File.Copy(file, srcfileName, true);
                }
            } 
        }



        private void buttonBackup_Click(object sender, EventArgs e)
        {
            CopyDirectory(textBoxBackupFrom.Text,textBoxBackupTo.Text);
            MessageBox.Show("备份完成！");
        }




        private void button1_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialogBackupFrom.ShowDialog() == DialogResult.OK)
            {
                textBoxBackupFrom.Text = folderBrowserDialogBackupFrom.SelectedPath;
            }
        }




        private void buttonBackupTo_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialogBackupTo.ShowDialog() == DialogResult.OK)
            {
                textBoxBackupTo.Text = folderBrowserDialogBackupTo.SelectedPath;
            }
        }




        private void Form1_Load(object sender, EventArgs e)
        {
            if (Directory.Exists(@"C:\Program Files (x86)\Ubisoft\Ubisoft Game Launcher\savegames")) textBoxBackupFrom.Text = @"C:\Program Files (x86)\Ubisoft\Ubisoft Game Launcher\savegames";
            if (Directory.Exists(@"D:\Program Files (x86)\Ubisoft\Ubisoft Game Launcher\savegames")) textBoxBackupFrom.Text = @"D:\Program Files (x86)\Ubisoft\Ubisoft Game Launcher\savegames";
            if (Directory.Exists(@"C:\Program Files\Ubisoft\Ubisoft Game Launcher\savegames")) textBoxBackupFrom.Text = @"C:\Program Files\Ubisoft\Ubisoft Game Launcher\savegames";
            if (Directory.Exists(@"D:\Program Files\Ubisoft\Ubisoft Game Launcher\savegames")) textBoxBackupFrom.Text = @"D:\Program Files\Ubisoft\Ubisoft Game Launcher\savegames";
        }
    }
}
