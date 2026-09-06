# Input 与第一个脚本

> **基础** | 第 9/192 节 | [[008_基础_碰撞体与刚体|⬅️ 碰撞体与刚体]] | [[010_基础_移动与跳跃|➡️ 移动与跳跃]]

---

## 🎯 本节目标

创建第一个 C# 脚本，学会用 `Debug.Log` 调试，掌握 `Input` 类检测键盘输入。

---

## 🖥️ 操作步骤

### 步骤 1：创建脚本

1. Project 窗口 → 右键 → Create → C# Script → 命名 `Player`
2. 双击打开 Visual Studio，看到自动生成的模板：

![脚本模板](../screenshots/009_template.jpg)

```csharp
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Start 在第一帧之前调用一次
    void Start()
    {

    }

    // Update 每一帧调用一次
    void Update()
    {

    }
}
```

3. 把脚本拖到 Hierarchy 中的 Player 对象上

---

### 步骤 2：用 Debug.Log 验证生命周期

```csharp
void Start()
{
    Debug.Log("Start was called");   // 只打印一次
}

void Update()
{
    Debug.Log("Update is called");   // 每帧打印
}
```

点 Play，观察 Console 窗口：

![Console输出](../screenshots/009_console.jpg)

> 🔑 **Start()** = 游戏开始时调用一次；**Update()** = 每帧调用（60fps = 每秒60次）

---

### 步骤 3：检测按键输入

```csharp
void Update()
{
    // 按住空格 → 持续触发（适合蓄力、移动）
    if (Input.GetKey(KeyCode.Space))
    {
        Debug.Log("You holding the space button!");
    }

    // 按下空格瞬间 → 只触发一次（适合跳跃、攻击）
    if (Input.GetKeyDown(KeyCode.Space))
    {
        Debug.Log("Jump");
    }
}
```

![输入检测代码](../screenshots/009_input.jpg)

---

### 步骤 4：理解 GetKey vs GetKeyDown vs GetKeyUp

| 方法 | 触发时机 | 典型用途 |
|------|---------|---------|
| `Input.GetKey()` | 按住期间每帧 | 移动、蓄力 |
| `Input.GetKeyDown()` | 按下瞬间一帧 | 跳跃、攻击 |
| `Input.GetKeyUp()` | 松开瞬间一帧 | 蓄力释放 |

---

### 步骤 5：使用虚拟按钮（推荐）

除了 `KeyCode.Space` 这种硬编码，Unity 还支持虚拟按钮：

```csharp
if (Input.GetButtonDown("Jump"))    // 默认 = Space
{
    Debug.Log("Jump!");
}

float xInput = Input.GetAxisRaw("Horizontal");  // -1 / 0 / 1
```

> 💡 虚拟按钮的好处：玩家可以在 Edit → Project Settings → Input Manager 中自定义按键映射，不用改代码。

![GetButtonDown](../screenshots/009_getkey.jpg)

---

## 💻 完整代码

```csharp
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    void Start()
    {
        Debug.Log("Start was called");
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            Debug.Log("Holding Space!");
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Jump!");
        }
    }
}
```

---

<details>
<summary>📝 英文原版转录</summary>

转录源：`C:\Users\rjq51\Desktop\B站视频下载\transcripts\009_4 Input and First script.txt`

视频明确选择旧 Input Manager（Legacy Input）作为本课程输入方案。它先把 `Player` 脚本挂到 Player 对象，借 `Debug.Log` 对比 `Start()` 只在首帧前执行一次、`Update()` 每帧执行；若 Visual Studio 没有 Unity 语法关联，视频建议在 Unity Preferences 的 External Tools 选择编辑器并 Regenerate Project Files。

随后依次演示 `Input.GetKeyDown`、`GetKey`、`GetKeyUp`，以及 `Input.GetButtonDown("Jump")`。`Jump`、`Horizontal` 等名称来自 Edit -> Project Settings -> Input Manager；最后将 `Input.GetAxisRaw("Horizontal")` 的结果存入 `public float xInput`，为下一节移动做准备。

这是一份按课程操作整理的摘要，不是逐字转录，也不能替代实际 Unity 项目的 Input Handling 设置和 Play Mode 验证。

</details>

---

> 🏷️ `MonoBehaviour` `Start()` `Update()` `Debug.Log` `Input.GetKey` `Input.GetKeyDown` `Input.GetAxisRaw` `Input.GetButtonDown`

## 本章对应代码进度

> 下面是完成本章后应得到的阶段代码快照。它与前面章节代码逐步衔接，后续章节会继续重构同一职责；第 62 章整合工程的最终实现见 `UnityProject/Assets/Scripts/`。

### 对应文件

`UnityProject/Assets/Scripts/Player/Player.cs`

```csharp
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    void Start()
    {
        Debug.Log("Start was called");
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            Debug.Log("Holding Space!");
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Jump!");
        }
    }
}
```

### 本章验证

- 阶段代码：已从本章代码示例整理并完成静态核对；片段依赖前置章节时，按本章说明接入对应组件或父类。
- 整合运行：第 62 章 Unity 工程已完成构建，并用本地 `pywinauto + pyautogui` 黑盒脚本验证移动、攻击、受击和反击流程；墙体部分以测试记录中的实际状态为准。
- 结论：本章新增能力在最终整合版中保留；中间阶段代码不宣称作为独立场景单独运行。
