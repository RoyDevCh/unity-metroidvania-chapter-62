# 第 62 章测试记录

测试日期：2026-09-12

## 自动化检查

- Unity 2021.3.16f1 批处理构建：通过，日志包含 `Build Finished, Result: Success.`。
- `tools/run_chapter62_ui_test.py`：通过，四个独立场景均完成并生成截图证据；本次反击在第 1 次尝试取得 `Counter Result: Stunned`。
- 文档检查：通过，前 62 章文档代码块无未闭合围栏或冲突标记；45 个含 C# 代码的章节已加入“本章对应代码进度”。
- 测试脚本语法检查：通过，`python -m py_compile tools/run_chapter62_ui_test.py`。

## 黑盒运行结果

- 启动和窗口绑定：通过。脚本用 `pywinauto` 定位 `Chapter62CombatDemo`，用 `pyautogui` 发送按键。
- 移动、跳跃、冲刺：通过。证据：`test-evidence/movement_*.png`。
- 三段攻击、敌人掉血、受击和击退：通过。骷髅血量截图从 100HP 降到 75HP、50HP，第三次攻击后敌人消失；证据：`test-evidence/combat_*.png`。
- 敌人追击、攻击和玩家受伤：通过。HUD 能看到 Chase/Attack 状态，玩家 HP 会减少。
- 反击窗口和眩晕：通过。稳定时序是进入敌人附近后立即按 Q，约 0.10 秒后再次按 Q；证据 `counter_01_window.png` 和 `counter_02_result.png`，截图显示 `Stunned 100HP` 且玩家 HP 保持 100。
- 滑墙和蹬墙跳：通过。黑盒脚本在新启动进程中稳定显示 `PlayerWallSlideState`，随后发送跳跃输入并完成离墙移动；证据：`test-evidence/wall_*.png`。

## Computer Use 复测记录

- 目标窗口枚举：通过。Computer Use 能从返回的应用列表中唯一识别 `Chapter62CombatDemo`，不会把 UU 远程窗口当作游戏窗口。
- 历史故障：旧进程绑定期间曾出现窗口捕获错配、`failed to activate captured window`、`GetCursorPos failed: 拒绝访问 (0x80070005)` 和黑屏截图；这些截图不作为游戏证据。
- 当前结论：通过重新启动并绑定新的游戏进程，底层窗口枚举和真实游戏窗口读取已恢复；完整动作验收仍以本地 `pywinauto + pyautogui` 黑盒脚本为主。

## 2026-09-12 重启后复测

- 官方 `@oai/sky`：通过。重置后可以重新导入、枚举 Unity 和 Chapter62CombatDemo，绑定、激活、截图均成功；本轮没有出现 `SyntaxError: Unexpected token ':'` 或 `Transport closed`。
- Unity 编辑器：通过。重启后重新载入 `Chapter62_Combat`，Game 视图恢复为 1x，Play Mode 可以启动，画面能显示城堡场景、玩家和两个敌人。
- Windows 构建：通过。释放项目锁后使用 Unity 2021.3.16f1 批处理构建，日志包含 `Build Finished, Result: Success.`。
- 导入器修复：通过。`Chapter62PixelArtImporter` 使用 `TextureImporterSettings.spriteMeshType = SpriteMeshType.FullRect`，修复了 Sprite Tiling 警告；Unity 2021.3 API 编译通过。
- Computer Use 输入限制：`sky.press_key` 是瞬时按键，没有持续 `keyDown` 接口；移动系统使用持续的 `Horizontal` 轴，因此长距离移动仍使用本地黑盒脚本验证。官方 sky 已验证窗口操作和单次跳跃、攻击输入可以发送。
- 反击回归：通过。测试从干净进程启动，移动到真实接触距离后进入反击状态，并轮询游戏内 `Counter Result: Stunned` 横幅；第 1 次尝试成功。
- 身体接触取证：通过。玩家与敌人层碰撞已开启，敌人进入战斗的距离收紧到实际身体接触范围；HUD 增加 `Contact: Yes`、`HIT` 和 `Player Hit: Yes` 反馈。
- 测试取证：已移除固定截图坐标依赖，按实际截图尺寸裁剪 HUD，并在成功横幅出现后保存证据。

## 证据位置

测试截图默认写入 `test-evidence/`，该目录已加入 `.gitignore`，避免把机器相关截图和日志提交到仓库。需要复测时运行：

```powershell
python tools/run_chapter62_ui_test.py
```

## 边界

本次验证的是第 62 章最终整合工程的运行闭环。章节中标记为阶段代码的片段需要按前置章节接入 Unity 组件或父类，未声称每个中间阶段都拥有独立场景。Animator、Tilemap、Cinemachine 和正式菜单仍按对应章节保留为编辑器学习内容。
