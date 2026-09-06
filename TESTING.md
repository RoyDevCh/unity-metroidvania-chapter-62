# 第 62 章测试记录

测试日期：2026-09-07

## 自动化检查

- Unity 2021.3.16f1 批处理构建：通过，日志包含 `Build Finished, Result: Success.`。
- `tools/run_chapter62_ui_test.py`：通过，四个独立场景均完成并生成截图证据。
- 文档检查：通过，前 62 章文档代码块无未闭合围栏或冲突标记；45 个含 C# 代码的章节已加入“本章对应代码进度”。
- 测试脚本语法检查：通过，`python -m py_compile tools/run_chapter62_ui_test.py`。

## 黑盒运行结果

- 启动和窗口绑定：通过。脚本用 `pywinauto` 定位 `Chapter62CombatDemo`，用 `pyautogui` 发送按键。
- 移动、跳跃、冲刺：通过。证据：`test-evidence/movement_*.png`。
- 三段攻击、敌人掉血、受击和击退：通过。骷髅血量截图从 100HP 降到 75HP、50HP，第三次攻击后敌人消失；证据：`test-evidence/combat_*.png`。
- 敌人追击、攻击和玩家受伤：通过。HUD 能看到 Chase/Attack 状态，玩家 HP 会减少。
- 反击窗口和眩晕：通过。稳定时序是进入敌人附近后立即按 Q，约 0.10 秒后再次按 Q；证据 `counter_01_window.png` 和 `counter_02_result.png`，截图显示 `Stunned 100HP` 且玩家 HP 保持 100。
- 滑墙：部分通过。最新完整运行截图稳定显示 `PlayerWallSlideState`；蹬墙跳输入在当前黑盒时序下仍有一次留在滑墙状态，暂记为待优化。证据：`test-evidence/wall_*.png`。

## 证据位置

测试截图默认写入 `test-evidence/`，该目录已加入 `.gitignore`，避免把机器相关截图和日志提交到仓库。需要复测时运行：

```powershell
python tools/run_chapter62_ui_test.py
```

## 边界

本次验证的是第 62 章最终整合工程的运行闭环。章节中标记为阶段代码的片段需要按前置章节接入 Unity 组件或父类，未声称每个中间阶段都拥有独立场景。Animator、Tilemap、Cinemachine 和正式菜单仍按对应章节保留为编辑器学习内容。
