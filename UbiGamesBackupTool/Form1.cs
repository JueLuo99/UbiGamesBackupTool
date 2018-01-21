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
using Microsoft.Win32;

namespace UbiGamesBackupTool
{
    public partial class FormMain : Form
    {
        public FormMain()
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




        private string GetUplayPath()
        {
            try
            {
                string strKeyName = string.Empty;
                string softPath = @"SOFTWARE\Classes\uplay\Shell\Open\Command";
                RegistryKey regKey = Registry.LocalMachine;
                RegistryKey regSubKey = regKey.OpenSubKey(softPath, false);

                object objResult = regSubKey.GetValue(strKeyName);
                RegistryValueKind regValueKind = regSubKey.GetValueKind(strKeyName);
                if (regValueKind == Microsoft.Win32.RegistryValueKind.String)
                {
                    return objResult.ToString();
                }
            }
            catch
            {
               return "自动获取Uplay安装路径失败,请尝试手动选择";
            }
            return null;
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            labelTip1.Text = "正在尝试自动探测路径...";
            textBoxBackupFrom.Text = GetUplayPath().Split(new char[] { '"', })[1];
            textBoxBackupFrom.Text = (textBoxBackupFrom.Text.Substring(0, textBoxBackupFrom.Text.LastIndexOf('\\'))) + "\\savegames"; 
            //这里已经到达存档位置，接下来遇到的就是单个/多个用户的文件夹
            if (textBoxBackupFrom.Text != "") labelTip1.Text = "已自动探测到存档路径";
        }
    }
}
