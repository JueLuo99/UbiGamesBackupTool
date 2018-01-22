using Microsoft.Win32;
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
    public partial class Form2 : Form
    {

        static string USERINFOLOCATION = System.Environment.GetEnvironmentVariable("USERPROFILE") + "\\AppData\\Local\\Ubisoft Game Launcher\\users.dat";
        static string USERICONLOCATION = GetUplayPath().Split(new char[] { '"', })[1].Substring(0, GetUplayPath().LastIndexOf('\\')) + "\\cache\\avatars";

        bool beginMove = false;//初始化鼠标位置  
        int currentXPosition;
        int currentYPosition;

        public Form2()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 获取Uplay路径
        /// </summary>
        /// <returns></returns>
        private static string GetUplayPath()
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

        /// <summary>
        /// 获取所有用户信息
        /// </summary>
        /// <returns>返回所有用户的集合</returns>
        public List<Dictionary<string, string>> GetAllUserInfo()
        {
            List<Dictionary<string, string>> UserList = new List<Dictionary<string, string>>();
            try
            {
                using (Stream stream = new FileStream(USERINFOLOCATION, FileMode.Open, FileAccess.Read))
                {
                    using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                    {
                        string content = reader.ReadToEnd();
                        int UserCount = 0;
                        int pos = 0;
                        while ((pos = content.IndexOf('�', pos + 1)) != -1)
                        {
                            UserCount++;
                            //Console.WriteLine("第"+UserCount+"个用户，位置"+pos);
                            Dictionary<string, string> UserInfo = new Dictionary<string, string>();
                            int uidstart = content.IndexOf('$', pos) + 1;
                            int uidend = content.IndexOf('*', uidstart);
                            string uid = content.Substring(uidstart, uidend - uidstart);
                            int unamestart = content.IndexOf('', uidend)+1;
                            int unameend = content.IndexOf(':', unamestart);
                            string username = content.Substring(unamestart, unameend - unamestart);
                            //Console.WriteLine("UID:" + uid+"    UserName:"+username);
                            UserInfo.Add("uid", uid);
                            UserInfo.Add("username", username);
                            UserList.Add(UserInfo);
                        }

                    }
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }
            return UserList;
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            
            Control.ControlCollection controls =  flowLayoutPanel1.Controls;
            List<Dictionary<string, string>> userlist = GetAllUserInfo();
            foreach (Dictionary<string, string> userinfo in userlist) {
                FlowLayoutPanel panel = new FlowLayoutPanel();
                panel.FlowDirection = FlowDirection.TopDown;
                PictureBox pictureBox = new PictureBox();
                Label label = new Label();
                panel.Controls.Add(pictureBox);
                panel.Controls.Add(label);

                string uid=null;
                string uname = null;
                userinfo.TryGetValue("uid", out uid);
                userinfo.TryGetValue("username", out uname);
                string imgpath = USERICONLOCATION + "\\" + uid + "_64.png";

                pictureBox.Image = Image.FromFile(imgpath);
                pictureBox.Size = new Size(32, 32);
                pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
                pictureBox.Margin = new Padding(0, 0, 0, 0);
                pictureBox.Parent = panel;
                pictureBox.Dock = DockStyle.Fill;

                label.AutoSize = true;
                label.TextAlign = ContentAlignment.MiddleLeft;
                label.Padding = new Padding(0, 0, 0, 0);
                label.Margin = new Padding(0, 0, 0, 0);
                label.Text = uname;
                label.Dock = DockStyle.Bottom;
                label.Parent = panel;

                panel.Parent = flowLayoutPanel1;
                panel.AutoSize = true;
                panel.Dock = DockStyle.Fill;
                controls.Add(panel);
                InitForm();
            }
        }
        public void InitForm() {
            button1.Size = new Size(flowLayoutPanel1.Height, flowLayoutPanel1.Height);
            button2.Size = new Size(flowLayoutPanel1.Height, flowLayoutPanel1.Height);
            tableLayoutPanel1.Location = new Point(tableLayoutPanel1.Location.X,button1.Location.Y + button1.Height);
        }

        private void Form2_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                beginMove = true;
                currentXPosition = MousePosition.X;//鼠标的x坐标为当前窗体左上角x坐标  
                currentYPosition = MousePosition.Y;//鼠标的y坐标为当前窗体左上角y坐标  
            }
        }

        private void Form2_MouseMove(object sender, MouseEventArgs e)
        {
            if (beginMove)
            {
                this.Left += MousePosition.X - currentXPosition;//根据鼠标x坐标确定窗体的左边坐标x  
                this.Top += MousePosition.Y - currentYPosition;//根据鼠标的y坐标窗体的顶部，即Y坐标  
                currentXPosition = MousePosition.X;
                currentYPosition = MousePosition.Y;
            }
        }

        private void Form2_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                currentXPosition = 0; //设置初始状态  
                currentYPosition = 0;
                beginMove = false;
            }
        }

        private void FlowLayoutPanel1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                currentXPosition = 0; //设置初始状态  
                currentYPosition = 0;
                beginMove = false;
            }
        }

        private void FlowLayoutPanel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (beginMove)
            {
                this.Left += MousePosition.X - currentXPosition;//根据鼠标x坐标确定窗体的左边坐标x  
                this.Top += MousePosition.Y - currentYPosition;//根据鼠标的y坐标窗体的顶部，即Y坐标  
                currentXPosition = MousePosition.X;
                currentYPosition = MousePosition.Y;
            }
        }

        private void FlowLayoutPanel1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                beginMove = true;
                currentXPosition = MousePosition.X;//鼠标的x坐标为当前窗体左上角x坐标  
                currentYPosition = MousePosition.Y;//鼠标的y坐标为当前窗体左上角y坐标  
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
