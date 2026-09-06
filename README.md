# 第 62 章：2D 战斗试炼场

这是一个面向学习的 Unity 2021.3 工程，对应教程第 008-062 章的最终可运行检查点。它把课程中的演进式脚本整理为一套能直接运行的状态机和战斗闭环。

## 运行

1. 用 Unity `2021.3.16f1` 打开本目录。
2. 打开 `Assets/Scenes/Chapter62_Combat.unity`。
3. 点击 Play。

场景会在运行时生成训练场、玩家和两个敌人，并加载 `Assets/Resources/Chapter62Art` 中的像素角色与背景素材，因此不依赖手工制作 Prefab 或 Animator 就能开始学习代码。

当前演示使用的是单帧像素素材，重点仍然是前 62 章的移动、状态机和战斗逻辑；动画剪辑、Tilemap 关卡和正式 UI 可以在此基础上继续制作。

## 操作

| 按键 | 动作 |
|---|---|
| A / D 或方向键 | 移动 |
| Space | 跳跃 |
| Left Shift | 冲刺 |
| J | 近战攻击 |
| Q，然后在敌人攻击动作中再按 Q | 进入反击并尝试弹反 |

## 代码阅读顺序

`Entity.cs` -> `PlayerStateMachine.cs` -> `Player.cs` -> `PlayerStates.cs` -> `Enemy.cs` -> `CombatHud.cs`。

`DemoBootstrap.cs` 只负责生成可运行的演示场景。教程中的 Animator、Tilemap、Cinemachine 和 Animation Event 配置仍可根据前 62 章文档逐步替换进去。

## 构建

Unity 菜单选择 `Build > Chapter 62 Windows Build`，输出到 `Builds/Chapter62CombatDemo.exe`。
