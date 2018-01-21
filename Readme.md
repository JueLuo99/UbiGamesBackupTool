### UbiGamesBackupTool
### 育碧游戏存档备份工具

#### 适用用户
- 需要备份不支持Uplay云存档的单机游戏的玩家
- 无法正常通过Uplay云存档备份存档的玩家

#### 现有特性
- 自动探测路径

#### 计划特性
- 支持检测多个Uplay用户
- 支持备份指定游戏

#### 说明
- Uplay在注册表中的位置<br>
计算机\HKEY_LOCAL_MACHINE\SOFTWARE\Classes\uplay\Shell\Open\Command

- 用户的UID与用户名相对应文件<br>
%USERPROFILE%\AppData\Local\Ubisoft Game Launcher\users.dat

#### 更新日志

20180121 加入并优化了自动探测路径功能

20180120 最基本的备份功能
