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
        static string UPLAYINSTALLLOCATION = GetUplayPath().Split(new char[] { '"', })[1].Substring(0, GetUplayPath().LastIndexOf('\\'));
        static string UPLAYSAVEGAME = UPLAYINSTALLLOCATION + "\\savegames";
        static string USERICONLOCATION = UPLAYINSTALLLOCATION + "\\cache\\avatars";
        static string GAMELOGOCACHE = UPLAYINSTALLLOCATION + "\\cache\\assets";

        static Image gamepanelbackground = Properties.Resources.gamepanelbackground;
        static char DirChar = '-';
        static Color GamePictureBoxSelectedColor = Color.FromArgb(192, 192, 192);
        static Color GamePictureBoxSelectedBackColor = Color.FromArgb(108, 108, 108);
        static Color GameNameLabelBackColor = Color.FromArgb(208, 208, 208);

        private UserInfo SelectUser { get; set; } = null;

        private enum ToolMode
        {
            Backup, Restore
        }
        ToolMode toolMode = ToolMode.Backup;
        bool beginMove = false;//初始化鼠标位置  
        int currentXPosition;
        int currentYPosition;
        List<string> SelectGameList = new List<string>();

        public Form2()
        {
            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);
            InitializeComponent();
        }
        private Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            string dllName = args.Name.Contains(",") ? args.Name.Substring(0, args.Name.IndexOf(',')) : args.Name.Replace(".dll", "");
            dllName = dllName.Replace(".", "_");
            if (dllName.EndsWith("_resources")) return null;
            ResourceManager rm = new System.Resources.ResourceManager(GetType().Namespace + ".Properties.Resources", System.Reflection.Assembly.GetExecutingAssembly());
            byte[] bytes = (byte[])rm.GetObject(dllName);
            return Assembly.Load(bytes);
        }

        /// <summary>
        /// 获取Uplay路径
        /// </summary>
        /// <returns>Uplay路径</returns>
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
        public static List<UserInfo> GetBackupAllUserInfo()
        {
            List<UserInfo> UserList = new List<UserInfo>();
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
                            UserInfo user = new UserInfo();
                            int uidstart = content.IndexOf('$', pos) + 1;
                            int uidend = content.IndexOf('*', uidstart);
                            string uid = content.Substring(uidstart, uidend - uidstart);
                            //int unamestart = content.IndexOf('', uidend)+1;
                            int unamestart = content.IndexOf("=2", uidend) + 3;
                            int unameend = content.IndexOf(':', unamestart);
                            string username = content.Substring(unamestart, unameend - unamestart);
                            user.UID = uid;
                            user.UNAME = username;
                            user.USERSAVEGAME = UPLAYSAVEGAME + Path.DirectorySeparatorChar + uid;
                            UserList.Add(user);
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
        /// <summary>
        /// 获取需要还原存档的用户列表
        /// </summary>
        /// <param name="path">备份路径</param>
        /// <returns>用户列表</returns>
        public static List<UserInfo> GetReStoreUserList(string path)
        {
            List<UserInfo> UserList = new List<UserInfo>();
            string[] userpath = Directory.GetDirectories(path);
            foreach (string dir in userpath)
            {
                //string[] file = Directory.GetFiles(dir, "*.json");
                //if (file.Length!=1) {
                //    continue;
                //}
                string userjson = dir + Path.DirectorySeparatorChar + "userinfo.json";
                if (File.Exists(userjson))
                {
                    using (Stream stream = new FileStream(userjson, FileMode.Open))
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            UserInfo user = JsonConvert.DeserializeObject<UserInfo>(reader.ReadToEnd());
                            user.USERSAVEGAME = dir;
                            UserList.Add(user);
                        }
                    }
                }
                else
                {
                    continue;
                }
            }
            return UserList;
        }
        private void Form2_Load(object sender, EventArgs e)
        {
            InitForm();
            InitUserList(UPLAYSAVEGAME);
        }
        public void InitUserList(String path)
        {
            Control.ControlCollection controls = flowLayoutPanel1.Controls;
            List<UserInfo> userlist = null;
            SelectGameList = new List<string>();
            switch (toolMode)
            {
                case ToolMode.Backup:
                    userlist = GetBackupAllUserInfo();
                    break;
                case ToolMode.Restore:
                    userlist = GetReStoreUserList(path);
                    break;
            }
            foreach (UserInfo userinfo in userlist)
            {
                //-----------------------------开始添加用户---------------------------

                Button button = new Button();
                button.Margin = new Padding(0, 0, 0, 0);
                flowLayoutPanel1.Controls.Add(button);
                string imgpath = USERICONLOCATION + "\\" + userinfo.UID + "_64.png";
                button.BackgroundImage = OverDrawHeadImg(imgpath);
                button.Size = new Size(40, 40);
                button.BackgroundImageLayout = ImageLayout.Zoom;
                button.FlatStyle = FlatStyle.Flat;
                button.FlatAppearance.BorderSize = 0;
                button.Tag = userinfo;
                button.Click += new EventHandler(this.UserButtonClick);
                toolTip1.SetToolTip(button, userinfo.UNAME);
                //CheckGameSaveDirectory();
            }
            SelectUser = userlist[0];
            InitGameListPanel();
        }
        /// <summary>
        /// 切换用户
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserButtonClick(object sender, EventArgs e)
        {
            SelectUser = (UserInfo)((Button)sender).Tag;
            SelectGameList = new List<string>();
            //flowLayoutPanel2.Controls.Clear();
            tableLayoutPanel1.Controls.Clear();
            InitGameListPanel();
            GC.Collect();
        }

        /// <summary>
        /// 初始化窗体
        /// </summary>
        public void InitForm()
        {
            button1.Size = new Size(flowLayoutPanel1.Height, flowLayoutPanel1.Height);
            button2.Size = new Size(flowLayoutPanel1.Height, flowLayoutPanel1.Height);
            flowLayoutPanel1.Width = this.Width - button1.Width * 2;
            flowLayoutPanel1.Location = new Point(button1.Width, 0);
            button2.Location = new Point(this.Width - button2.Width, 0);
            //flowLayoutPanel2.Location = new Point(flowLayoutPanel2.Location.X, button1.Location.Y + button1.Height);
            //flowLayoutPanel2.Height = this.Height - flowLayoutPanel2.Location.Y;
            //panel1.Location = new Point(panel1.Location.X, flowLayoutPanel2.Location.Y);
            tableLayoutPanel1.Location = new Point(tableLayoutPanel1.Location.X, button1.Location.Y + button1.Height);
            tableLayoutPanel1.Height = this.Height - tableLayoutPanel1.Location.Y;
            panel1.Location = new Point(panel1.Location.X, tableLayoutPanel1.Location.Y);
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
        /// 获取支持的游戏列表
        /// </summary>
        /// <returns></returns>
        public List<Game> GetSupportGame()
        {
            Assembly assm = Assembly.GetExecutingAssembly();
            Stream gamejson = assm.GetManifestResourceStream("UbiGamesBackupTool.game.json");
            StreamReader stream = new StreamReader(gamejson);
            List<Game> gamelist = JsonConvert.DeserializeObject<List<Game>>(stream.ReadToEnd());
            gamejson.Close();
            stream.Close();
            return gamelist;
        }

        /// <summary>
        /// 检查并获取已存在的存档的游戏
        /// </summary>
        /// <returns>检测到的已存在存档的游戏列表</returns>
        public List<Game> CheckGameSaveDirectory()
        {
            List<Game> gamelist = GetSupportGame();
            List<Game> Ugamelist = new List<Game>();
            if (SelectUser != null)
            {
                foreach (string str in GetGameSaveDirectory())
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
        public String[] GetGameSaveDirectory()
        {
            if (Directory.Exists(SelectUser.USERSAVEGAME))
            {
                String[] SaveDirectorys = Directory.GetDirectories(SelectUser.USERSAVEGAME);
                for (int i = 0; i < SaveDirectorys.Length; i++)
                {
                    SaveDirectorys[i] = SaveDirectorys[i].Substring(SaveDirectorys[i].LastIndexOf('\\') + 1, SaveDirectorys[i].Length - SaveDirectorys[i].LastIndexOf('\\') - 1);
                }
                return SaveDirectorys;
            }
            return new string[0];
        }
        /// <summary>
        /// 初始化游戏列表
        /// </summary>
        public void InitGameListPanel()
        {
            tableLayoutPanel1.ColumnStyles.Clear();
            tableLayoutPanel1.RowStyles.Clear();
            tableLayoutPanel1.Controls.Clear();
            List<Game> Ugamelist = CheckGameSaveDirectory();
            foreach (Game g in Ugamelist)
            {
                //----------------------开始添加一个游戏在列表中---------------------
                FlowLayoutPanel panel = new FlowLayoutPanel();
                panel.FlowDirection = FlowDirection.TopDown;
                //panel.AutoSize = true;

                PictureBox pictureBox = new PictureBox();
                Label label = new Label();
                panel.Controls.Add(pictureBox);
                panel.Controls.Add(label);
                tableLayoutPanel1.Controls.Add(panel);

                if (File.Exists(GAMELOGOCACHE + "\\" + g.img))
                    pictureBox.BackgroundImage = Image.FromFile(GAMELOGOCACHE + "\\" + g.img);
                else
                    pictureBox.BackgroundImage = gamepanelbackground;
                pictureBox.Size = new Size(340, 181);
                pictureBox.BackgroundImageLayout = ImageLayout.Zoom;
                pictureBox.BackColor = GamePictureBoxSelectedColor;
                pictureBox.Margin = new Padding(0, 0, 0, 0);
                pictureBox.Click += new EventHandler(this.GamePanelClicked);
                pictureBox.Tag = g.id;

                CheckBox checkBox = new CheckBox();
                pictureBox.Controls.Add(checkBox);
                checkBox.AutoSize = true;
                checkBox.Checked = false;
                checkBox.Enabled = false;
                //checkBox.CheckedChanged += new EventHandler(GamePanelChecked);


                label.AutoSize = true;
                label.Margin = new Padding(0, 0, 0, 0);
                label.Padding = new Padding(0, 2, 0, 2);
                label.Text = g.name;
                label.TextAlign = ContentAlignment.MiddleCenter;
                label.Dock = DockStyle.Bottom;
                label.BackColor = GameNameLabelBackColor;

                panel.Size = new Size(pictureBox.Width, pictureBox.Height + label.Height);
                panel.Anchor = AnchorStyles.None;
            }
            GC.Collect();
        }

        //private void GamePanelChecked(object sender, EventArgs e)
        //{
        //    PictureBox pictureBox = (PictureBox)((CheckBox)sender).Parent;
        //    string gid = pictureBox.Tag.ToString();
        //    if (SelectGameList.Contains(gid))
        //    {
        //        SelectGameList.Remove(gid);
        //        Console.WriteLine("取消选中:" + gid);
        //    }
        //    else
        //    {
        //        SelectGameList.Add(gid);
        //        Console.WriteLine("选中:" + gid);
        //    }
        //}
        /// <summary>
        /// 游戏列表中 pictureBox的点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GamePanelClicked(object sender, EventArgs e)
        {
            PictureBox pictureBox = (PictureBox)sender;
            string gid = pictureBox.Tag.ToString();
            if (SelectGameList.Contains(gid))
            {
                SelectGameList.Remove(gid);
                pictureBox.BackColor = GamePictureBoxSelectedColor;
                ((CheckBox)pictureBox.Controls[0]).Checked = false;
            }
            else
            {
                SelectGameList.Add(gid);
                pictureBox.BackColor = GamePictureBoxSelectedBackColor;
                ((CheckBox)pictureBox.Controls[0]).Checked = true;
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form settingform = new SettingForm();
            settingform.Show();
            settingform.Location = this.Location;
        }

        private void BackupSaveGame(object sender, EventArgs e)
        {
            if (SelectGameList.Count > 0)
            {
                if (DialogResult.OK == MessageBox.Show("确定要备份这些游戏？", "确定", MessageBoxButtons.OKCancel, MessageBoxIcon.Question))
                {
                    if (folderBrowserDialogBackupTo.ShowDialog() == DialogResult.OK)
                    {
                        SelectUser.BackupTime = string.Format("{0:yyyy-MM-dd HH.mm.ss}", DateTime.Now);
                        String backupPath = folderBrowserDialogBackupTo.SelectedPath + Path.DirectorySeparatorChar + SelectUser.UNAME + DirChar + SelectUser.BackupTime;

                        foreach (string path in SelectGameList)
                        {
                            CopyDirectory(UPLAYSAVEGAME + Path.DirectorySeparatorChar + SelectUser.UID + Path.DirectorySeparatorChar + path, backupPath);
                        }
                        OutBackupInfo(backupPath);
                        MessageBox.Show("备份完成！");
                        GC.Collect();
                    }
                }
            }
            else
            {
                MessageBox.Show("还未选择游戏！");
            }
        }
        /// <summary>
        /// 复制存档目录
        /// </summary>
        /// <param name="srcdir">游戏存档源文件夹</param>
        /// <param name="desdir">目标存档文件夹</param>
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
        /// <summary>
        /// 输出备份信息
        /// </summary>
        /// <param name="path"></param>
        public void OutBackupInfo(string path)
        {
            using (Stream stream = new FileStream(path + Path.DirectorySeparatorChar + "userinfo.json", FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.WriteLine(JsonConvert.SerializeObject(SelectUser));
                    writer.Flush();
                }
            }
        }
        /// <summary>
        /// 检查游戏存档版本
        /// </summary>
        /// <param name="srcdir"></param>
        /// <param name="gid"></param>
        public void CheckGameVersion(string srcdir, string gid)
        {

        }
        /// <summary>
        /// 切换备份或还原
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChangeMode(object sender, EventArgs e)
        {

            if (this.toolMode == ToolMode.Backup)
            {
                if (folderBrowserDialogBackupTo.ShowDialog() == DialogResult.OK)
                {
                    button3.Visible = false;
                    button5.Visible = true;
                    toolMode = ToolMode.Restore;

                    flowLayoutPanel1.Controls.Clear();

                    InitUserList(folderBrowserDialogBackupTo.SelectedPath);
                    InitGameListPanel();
                }
            }
            else
            {
                button3.Visible = true;
                button5.Visible = false;
                toolMode = ToolMode.Backup;

                flowLayoutPanel1.Controls.Clear();

                InitUserList(UPLAYSAVEGAME);
                InitGameListPanel();
            }
        }

        private void ReStoreSaveGame(object sender, EventArgs e)
        {
            if (SelectGameList.Count > 0)
            {
                if (DialogResult.OK == MessageBox.Show("确定要还原这些游戏？", "确定", MessageBoxButtons.OKCancel, MessageBoxIcon.Question))
                {
                        String ReStorePath = UPLAYSAVEGAME+Path.DirectorySeparatorChar+SelectUser.UID;

                        foreach (string GID in SelectGameList)
                        {
                            CopyDirectory(SelectUser.USERSAVEGAME+Path.DirectorySeparatorChar+GID, ReStorePath);
                        }
                        MessageBox.Show("还原完成！");
                        GC.Collect();
                }
            }
            else
            {
                MessageBox.Show("还未选择存档！");
            }
        }
    }
}
