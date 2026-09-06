# 测试记录

测试日期：2026-09-07

以下检查已通过：

- Unity 2021.3.16f1 自带 Mono C# 编译器已编译 `Assets/Scripts` 中全部运行时代码，无错误、无警告。
- Unity 2021.3.16f1 自带 Mono C# 编译器已编译 `Assets/Editor/Chapter62Build.cs`，无错误、无警告。
- 默认 Build Settings 指向 `Assets/Scenes/Chapter62_Combat.unity`。
- `DemoBootstrap` 的场景脚本 GUID 与其 `.meta` 文件一致。
- Input Manager 包含 `Horizontal`、`Jump` 和 `Fire1`；TagManager 已配置 `Ground`、`Player`、`Enemy`、`Attack` 层。

未能在本机完成的检查：

- Unity batchmode 在导入脚本前报告没有有效编辑器许可证，因此无法自动执行 Play Mode 或 Windows Build。日志为 `BatchMode: Unity has not been activated with a valid License`。

在已激活的 Unity 2021.3 LTS 编辑器中打开 `Assets/Scenes/Chapter62_Combat.unity` 后，按 `CHAPTER_062_IMPLEMENTATION.md` 的五步验收动作进行 Play Mode 验证，再运行 `Build > Chapter 62 Windows Build` 即可完成最终运行时和构建确认。
