using Microsoft.Win32;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UbiGamesBackupTool
{
    public partial class Form2 : Form
    {

        static string USERINFOLOCATION = System.Environment.GetEnvironmentVariable("USERPROFILE") + "\\AppData\\Local\\Ubisoft Game Launcher\\users.dat";
        static string UPLAYSAVEGAME = GetUplayPath().Split(new char[] { '"', })[1].Substring(0, GetUplayPath().LastIndexOf('\\')) + "\\savegames";
        static string USERICONLOCATION = GetUplayPath().Split(new char[] { '"', })[1].Substring(0, GetUplayPath().LastIndexOf('\\')) + "\\cache\\avatars";

        static Image gamepanelbackground = Properties.Resources.gamepanelbackground;

        private string SelectedUid { get; set; } = null;

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
                            //int unamestart = content.IndexOf('', uidend)+1;
                            int unamestart = content.IndexOf("=2", uidend) + 3;
                            int unameend = content.IndexOf(':', unamestart);
                            string username = content.Substring(unamestart, unameend - unamestart);
                            Console.WriteLine("UID:" + uid + "    UserName:" + username);
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
            Control.ControlCollection controls = flowLayoutPanel1.Controls;
            List<Dictionary<string, string>> userlist = GetAllUserInfo();
            foreach (Dictionary<string, string> userinfo in userlist)
            {
                //-----------------------------开始添加用户---------------------------

                PictureBox pictureBox = new PictureBox();
                flowLayoutPanel1.Controls.Add(pictureBox);
                string uid = userinfo["uid"];
                string uname = userinfo["username"];
                string imgpath = USERICONLOCATION + "\\" + uid + "_64.png";
                pictureBox.Image = OverDrawHeadImg(imgpath);
                pictureBox.Size = new Size(48,48);
                pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
                toolTip1.SetToolTip(pictureBox, uname);
                InitForm();
                //CheckGameSaveDirectory();
                InitGameListPanel();
            }
        }
        /// <summary>
        /// 初始化窗体
        /// </summary>
        public void InitForm()
        {
            SelectedUid = GetAllUserInfo()[0]["uid"];
            button1.Size = new Size(flowLayoutPanel1.Height, flowLayoutPanel1.Height);
            button2.Size = new Size(flowLayoutPanel1.Height, flowLayoutPanel1.Height);
            flowLayoutPanel1.Width = this.Width - button1.Width * 2;
            flowLayoutPanel1.Location = new Point(button1.Width, 0);
            button2.Location = new Point(this.Width - button2.Width, 0);
            flowLayoutPanel2.Location = new Point(flowLayoutPanel2.Location.X, button1.Location.Y + button1.Height);
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
        /// <summary>
        /// 重绘头像为圆形
        /// </summary>
        /// <param name="file">图片路径</param>
        /// <returns>圆形头像</returns>
        private Bitmap OverDrawHeadImg(string file)
        {
            using (Image i = new Bitmap(file))
            {
                Bitmap b = new Bitmap(i.Width, i.Height);
                using (Graphics g = Graphics.FromImage(b))
                {
                    g.SmoothingMode = SmoothingMode.HighQuality;
                    using (GraphicsPath p = new GraphicsPath(FillMode.Alternate))
                    {
                        p.AddEllipse(0, 0, i.Width, i.Height);
                        g.FillPath(new TextureBrush(i), p);
                    }
                    //g.FillEllipse(new TextureBrush(i), 0, 0, i.Width, i.Height);
                }
                return b;
            }
        }
        private void Button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        /// <summary>
        /// 检查并获取已存在的存档的游戏
        /// </summary>
        /// <returns>检测到的已存在存档的游戏列表</returns>
        public List<Game> CheckGameSaveDirectory()
        {
            Assembly assm = Assembly.GetExecutingAssembly();
            Stream gamejson = assm.GetManifestResourceStream("UbiGamesBackupTool.game.json");
            StreamReader stream = new StreamReader(gamejson);
            List<Game> gamelist = JsonConvert.DeserializeObject<List<Game>>(stream.ReadToEnd());
            List<Game> Ugamelist = new List<Game>();
            if (SelectedUid != null)
            {
                foreach (string str in GetGameSaveDirectory(SelectedUid))
                {
                    foreach (Game g in gamelist)
                    {
                        if (g.id.Equals(str))
                        {
                            Ugamelist.Add(g);
                            break;
                        }
                    }
                }
            }
            return Ugamelist;
        }
        /// <summary>
        /// 获取用户所有存档文件夹
        /// </summary>
        /// <param name="uid">用户UID</param>
        /// <returns>返回存档文件夹名称，不包括路径</returns>
        public String[] GetGameSaveDirectory(string uid)
        {
            String[] SaveDirectorys = Directory.GetDirectories(UPLAYSAVEGAME + "\\" + uid);
            for (int i = 0; i < SaveDirectorys.Length; i++)
            {
                SaveDirectorys[i] = SaveDirectorys[i].Substring(SaveDirectorys[i].LastIndexOf('\\') + 1, SaveDirectorys[i].Length - SaveDirectorys[i].LastIndexOf('\\') - 1);
            }
            return SaveDirectorys;
        }
        public void InitGameListPanel()
        {
            List<Game> Ugamelist = CheckGameSaveDirectory();
            foreach (Game g in Ugamelist)
            {
                //----------------------开始添加一个游戏在列表中---------------------
                FlowLayoutPanel panel = new FlowLayoutPanel();
                panel.FlowDirection = FlowDirection.TopDown;
                panel.AutoSize = true;
                PictureBox pictureBox = new PictureBox();
                Label label = new Label();
                panel.Controls.Add(pictureBox);
                panel.Controls.Add(label);
                flowLayoutPanel2.Controls.Add(panel);

                pictureBox.Image = gamepanelbackground;
                pictureBox.Size = new Size(340, 181);
                pictureBox.SizeMode = PictureBoxSizeMode.CenterImage;
                pictureBox.BackColor = Color.Black;


                label.AutoSize = true;
                label.Text = g.name;
                label.TextAlign = ContentAlignment.MiddleCenter;
                label.Dock = DockStyle.Bottom;
                label.BackColor = Color.Blue;
            }
        }
        public void ChangeUsered()
        {

        }
    }
}
