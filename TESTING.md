# 测试记录

测试日期：2026-09-07

以下检查已通过：

- Unity 2021.3.16f1 自带 Mono C# 编译器已编译 `Assets/Scripts` 中全部运行时代码，无错误、无警告。
- Unity 2021.3.16f1 自带 Mono C# 编译器已编译 `Assets/Editor/Chapter62Build.cs`，无错误、无警告。
- 默认 Build Settings 指向 `Assets/Scenes/Chapter62_Combat.unity`。
- `DemoBootstrap` 的场景脚本 GUID 与其 `.meta` 文件一致。
- Input Manager 包含 `Horizontal`、`Jump` 和 `Fire1`；TagManager 已配置 `Ground`、`Player`、`Enemy`、`Attack` 层。
- Unity 2021.3.16f1 批处理导入工程并成功执行 Windows 构建，日志记录 `Build Finished, Result: Success.`。
- 构建后的 `Builds/Chapter62CombatDemo.exe` 已启动，进程正常响应，窗口标题为 `Chapter62CombatDemo`。
- 已通过底层 Windows Computer Use bridge 发现、绑定并激活 `Chapter62CombatDemo` 窗口，成功读取 Unity 游戏画面截图。
- 已完成运行时窗口和游戏画面截图验收；截图显示训练场窗口可启动并正常响应。
- 已修复演示场景使用纯色方块的问题：玩家、骷髅、史莱姆和三层背景现在从 `Assets/Resources/Chapter62Art` 加载像素素材。
- 已重新执行 Unity Windows 构建；像素素材导入器和运行时素材加载编译通过，日志记录 `Build Finished, Result: Success.`。
- 运行时日志曾发现 `CombatHud.Start()` 在 `Start` 阶段访问 `GUI.skin` 的 Unity 异常，已改为在 `OnGUI` 内延迟创建 GUI 样式，并重新构建。

## 当前逐项验收状态

- 启动、窗口响应、截图：通过。
- HUD 无运行时 GUI 异常：已修复，待在新构建窗口中再次截图确认。
- A/D 移动、Space 跳跃、Shift 冲刺：待 Computer Use 按键操作验证。
- J 三段攻击、敌人掉血、受击闪白、击退、死亡：待 Computer Use 按键操作验证。
- 敌人攻击、玩家受伤、Q 反击、敌人眩晕：待 Computer Use 按键操作验证。
- 滑墙、蹬墙跳：待 Computer Use 按键操作验证。

本轮 Computer Use 在重新绑定新构建窗口时返回 `SyntaxError: Unexpected token ':'`，因此不能把尚未完成的按键验收写成“全部通过”。恢复原生窗口桥接后，应从启动截图开始重新执行上述待验证项目。

在已激活的 Unity 2021.3 LTS 编辑器中打开 `Assets/Scenes/Chapter62_Combat.unity` 后，可按 `CHAPTER_062_IMPLEMENTATION.md` 的五步验收动作重复测试；构建菜单 `Build > Chapter 62 Windows Build` 已由批处理构建验证通过。当前角色使用单帧像素素材，动画剪辑、Tilemap 关卡和正式菜单仍未纳入第 62 章终点。
