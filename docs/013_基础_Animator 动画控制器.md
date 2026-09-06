# Animator 动画控制器

> **基础** | 第 13/192 节 | [[012_基础_精灵图切割|⬅️ 精灵图切割]] | [[014_基础_代码重构与整理|➡️ 代码重构与整理]]

---

## 🎯 本节目标

为角色创建 Idle 和 Move 两个动画片段，并通过 Animator 控制器实现状态切换。

> **课程阶段核对**：视频会在子物体 Animator 上创建并指定 `Player_AC`，并在 Animation 窗口逐个检查 Clip 的采样率。本页的 `Awake() + GetComponentInChildren<Animator>()` 是等价整合写法；课程截图中的组件缓存位置是 `Start()`。因此截图可辅助确认控制器、Clip 和过渡，但不要把它当成 `Awake` 代码的直接证据。

---

## 🖥️ 操作步骤

### 步骤 1：创建 Animation Clip 与 Player_AC

1. 选中 Hierarchy 中的 **Player** 对象
2. 打开 Animation 窗口：Window → Animation → Animation（快捷键 `Ctrl+6`）
3. 点击 **Create** → 弹出保存对话框 → 命名 `playerIdle` → 保存到 `Assets/Player` 文件夹，并确认子物体 Animator 使用课程创建的 `Player_AC`

![Animation窗口](../screenshots/013_1.jpg)

4. 将 Idle 帧的 Sprite 依次拖入 Animation 时间线（每个帧约 0.1 秒）
5. 再点击 Animation 窗口左上角的clip名 → Create New Clip → 命名 `playerMove` → 拖入移动帧

---

### 步骤 2：设置 Animator Controller

1. Window → Animation → Animator 打开状态机
2. 可以看到两个状态节点：`playerIdle` 和 `playerMove`

![Animator状态机](../screenshots/013_3.jpg)

3. **Entry** 默认连接 `playerIdle`（橙色 = 默认状态）

---

### 步骤 3：添加切换参数

1. Animator 窗口左侧 **Parameters** 标签 → 点击 `+` → 选 **Bool** → 命名 `isMoving`
2. 这个布尔参数用于判断角色是否在移动（`true` = 移动，`false` = 静止）

---

### 步骤 4：设置状态过渡条件

1. **右键** `playerIdle` → Make Transition → 指向 `playerMove`
2. **右键** `playerMove` → Make Transition → 指向 `playerIdle`
3. 选中 Idle→Move 的 Transition：

![Transition设置](../screenshots/013_transition.jpg)

Inspector 中设置：

| 设置 | 值 | 含义 |
|------|-----|------|
| **Conditions** | `isMoving true` | 在移动 → 切换到移动动画 |
| **Has Exit Time** | ❌ 取消勾选 | 立即切换不等动画播完 |
| **Transition Duration** | 0 | 无过渡时间 |

4. 选中 Move→Idle 的 Transition：

| 设置 | 值 |
|------|-----|
| **Conditions** | `isMoving false` |
| **Has Exit Time** | ❌ |
| **Transition Duration** | 0 |

> ⚠️ 视频里特别强调：取消 **Has Exit Time**、Duration 设 0 是本课程绝大多数动画过渡的默认设置，只有少数情况才保留 Exit Time。

---

### 步骤 5：用代码控制动画

在 `Player.cs` 中获取 Animator 并更新参数：

```csharp
private Animator anim;
private bool isMoving;

void Awake()
{
    rb = GetComponent<Rigidbody2D>();
    anim = GetComponentInChildren<Animator>();
}

void Update()
{
    // ... 移动逻辑 ...

    // 水平速度不为 0，就说明角色正在移动
    isMoving = rb.velocity.x != 0;

    // 更新动画参数
    anim.SetBool("isMoving", isMoving);
}
```

> 💡 **注意**：Animator 组件不在 Player 本体上，而是在它的**子对象**（挂 SpriteRenderer 的那个）身上，所以要用 `GetComponentInChildren<Animator>()` 而不是 `GetComponent<Animator>()`。

---

## 💻 完整代码

> 这是为后续章节整理的完整版本。若逐步复现课程，先在 `Start()` 完成组件引用并在 Inspector/Animation 窗口完成 `Player_AC` 和采样率配置，再合并为这里的写法。

```csharp
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 8f;
    [SerializeField] private float jumpForce = 7f;

    private Rigidbody2D rb;
    private Animator anim;
    private float xInput;
    private bool isMoving;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        xInput = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(xInput * moveSpeed, rb.velocity.y);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        isMoving = rb.velocity.x != 0;
        anim.SetBool("isMoving", isMoving);
    }
}
```

---

## 🎬 操作演示

![创建动画](../gifs/013_创建动画.gif)

---

## 📝 核心要点

```
Animation Clip（动画片段）= 帧序列（playerIdle, playerMove）
Animator Controller（控制器）= 状态机（状态 + 过渡条件）
Parameter（参数）= 脚本→状态机的桥梁（isMoving）

代码控制流程：
  脚本 anim.SetBool("isMoving", true/false)
    → Animator 检查条件
      → 自动切换 Idle ↔ Move
```

---

<details>
<summary>📝 英文原版转录</summary>

来源文件：C:\Users\rjq51\Desktop\B站视频下载\transcripts\013_8 Animator.txt

本节关键操作摘要：在子 Animator 对象上创建 Player_AC，制作 Idle/Move Animation Clip，设置采样率和 Animator 状态；再用 isMoving 条件连接两个状态。

摘要根据转录关键句整理，用于定位视频操作；它不是逐字转录，也不替代 Unity 编辑器验证。
</details>

---

> 🏷️ `Animator` `Animation Clip` `Animator Controller` `Transition` `Parameter` `SetBool` `Has Exit Time`

## 本章对应代码进度

> 下面是完成本章后应得到的阶段代码快照。它与前面章节代码逐步衔接，后续章节会继续重构同一职责；第 62 章整合工程的最终实现见 `UnityProject/Assets/Scripts/`。

### 对应文件

`UnityProject/Assets/Scripts/Player/Player.cs`

```csharp
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 8f;
    [SerializeField] private float jumpForce = 7f;

    private Rigidbody2D rb;
    private Animator anim;
    private float xInput;
    private bool isMoving;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        xInput = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(xInput * moveSpeed, rb.velocity.y);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        isMoving = rb.velocity.x != 0;
        anim.SetBool("isMoving", isMoving);
    }
}
```

### 本章验证

- 阶段代码：已从本章代码示例整理并完成静态核对；片段依赖前置章节时，按本章说明接入对应组件或父类。
- 整合运行：第 62 章 Unity 工程已完成构建，并用本地 `pywinauto + pyautogui` 黑盒脚本验证移动、攻击、受击和反击流程；墙体部分以测试记录中的实际状态为准。
- 结论：本章新增能力在最终整合版中保留；中间阶段代码不宣称作为独立场景单独运行。
