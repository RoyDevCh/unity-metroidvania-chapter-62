# Animator 配合状态机

> **状态机** | 第 30/192 节 | [[029_状态机_创建有限状态机|⬅️ 创建有限状态机]] | [[031_状态机_状态机驱动的移动|➡️ 状态机驱动的移动]]

---

## 🎯 本节目标

将 Animator 的状态切换从"手动设条件"改为"由代码状态机驱动"，让 Animator 成为状态机的可视化表现。

---

## 🖥️ 操作步骤

### 步骤 1：Animator 参数精简

只保留 Bool 类型参数，每个状态对应一个：

| 参数名 | 类型 | 对应状态 |
|--------|------|---------|
| `Idle` | Bool | 待机 |
| `Move` | Bool | 移动 |
| `Jump` | Bool | 跳跃 |
| `Air` | Bool | 空中 |
| `Dash` | Bool | 冲刺 |

---

### 步骤 2：状态机的 Enter/Exit 自动控制动画

```csharp
// PlayerState 基类已实现：
public override void Enter()
{
    player.anim.SetBool(animBoolName, true);   // 进入状态 → Bool = true
}

public override void Exit()
{
    player.anim.SetBool(animBoolName, false);  // 离开状态 → Bool = false
}
```

![Animator配置](../screenshots/030_a.jpg)

---

### 步骤 3：Animator 过渡条件

每个 Bool → 对应状态动画：

- `Any State` → `playerIdle`：Condition `Idle == true`
- `Any State` → `playerMove`：Condition `Move == true`
- `Any State` → `playerJump`：Condition `Jump == true`
- `Any State` → `playerAir`：Condition `Air == true`
- `Any State` → `playerDash`：Condition `Dash == true`

**所有 Transition 设置：**
- Has Exit Time: ❌
- Transition Duration: 0
- Can Transition To Self: ❌

![Transition设置](../screenshots/030_b.jpg)

---

### 步骤 4：验证流程

```
代码状态机切换：
  ChangeState(idleState) → idleState.Enter() → anim.SetBool("Idle", true)
  ↓
  Animator 检测到 Idle=true → 从 Any State 切换到 playerIdle 动画
```

---

## 🎬 操作演示

![Animator配合状态机](../gifs/030_Animator配合状态机.gif)

---

## 📝 核心要点

```
旧模式：脚本中大量 if/else + anim.SetBool 控制动画
新模式：状态机 Enter/Exit 自动处理 anim 参数

关键转变：
  以前：在 Update 中判断条件手动设参数
  现在：ChangeState() 自动触发 Enter/Exit → 动画自动跟随

Animator 只做"显示"，逻辑全在代码状态机中
```

---

<details>
<summary>📝 英文原版转录</summary>

来源文件：C:\Users\rjq51\Desktop\B站视频下载\transcripts\030_3 Setup Animator with State Machine.txt

本节关键操作摘要：导入课程图形，创建 Player 的 Animator 子对象和控制器，建立状态参数并让脚本状态机与 Animator Bool 同步。

摘要根据转录关键句整理，用于定位视频操作；它不是逐字转录，也不替代 Unity 编辑器验证。
</details>

---

> 🏷️ `Animator` `SetBool` `Any State` `Has Exit Time` `Transition Duration`

## 本章对应代码进度

> 下面是完成本章后应得到的阶段代码快照。它与前面章节代码逐步衔接，后续章节会继续重构同一职责；第 62 章整合工程的最终实现见 `UnityProject/Assets/Scripts/`。

### 对应文件

`UnityProject/Assets/Scripts/Player/PlayerStates.cs`

```csharp
// PlayerState 基类已实现：
public override void Enter()
{
    player.anim.SetBool(animBoolName, true);   // 进入状态 → Bool = true
}

public override void Exit()
{
    player.anim.SetBool(animBoolName, false);  // 离开状态 → Bool = false
}
```

### 本章验证

- 阶段代码：已从本章代码示例整理并完成静态核对；片段依赖前置章节时，按本章说明接入对应组件或父类。
- 整合运行：第 62 章 Unity 工程已完成构建，并用本地 `pywinauto + pyautogui` 黑盒脚本验证移动、攻击、受击和反击流程；墙体部分以测试记录中的实际状态为准。
- 结论：本章新增能力在最终整合版中保留；中间阶段代码不宣称作为独立场景单独运行。
