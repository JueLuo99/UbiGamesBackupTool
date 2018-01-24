### UbiGamesBackupTool
### 育碧游戏存档备份工具

#### 适用群体
- 需要备份不支持Uplay云存档的单机游戏的玩家
- 无法正常通过Uplay云存档备份存档的玩家

#### 现有特性
- 自动探测Uplay安装路径
- 支持检测多个Uplay用户
- 支持选择备份任意数量的游戏

#### 计划特性
- 实时备份游戏
- 还原已经备份的游戏

#### 额外说明
- Uplay在注册表中显示的安装位置<br>
计算机\HKEY_LOCAL_MACHINE\SOFTWARE\Classes\uplay\Shell\Open\Command

- 用户的UID与用户名相对应文件<br>
%USERPROFILE%\AppData\Local\Ubisoft Game Launcher\users.dat

- 存档文件夹内的游戏ID与游戏名字关系是怎么得出的？<br>
我参照了这里：https://steamcn.com/t232432-1-1

- 《彩虹六号：围攻》《全境封锁》等网络游戏存档在服务器，备份的仅仅是在本地的按键设置（其实我也不知道他们存在本地的是啥）

#### 常见问题
为什么需要UAC权限？
用于读取Uplay的安装位置以了解存档位置（通过注册表），使用的是只读操作。


#### 更新日志

20180121 加入并优化了自动探测路径功能

20180120 最基本的备份功能
