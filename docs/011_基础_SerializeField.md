# SerializeField

> **基础** | 第 11/192 节 | [[010_基础_移动与跳跃|⬅️ 移动与跳跃]] | [[012_基础_精灵图切割|➡️ 精灵图切割]]

---

## 🎯 本节目标

理解 `[SerializeField]` 的用法——让 `private` 变量也能在 Inspector 中显示，兼顾封装性和便利性。

---

## 🖥️ 操作步骤

### 步骤 1：回顾上节代码

上节我们写了移动和跳跃，变量声明如下：

```csharp
public Rigidbody2D rb;
public float moveSpeed;
public float jumpForce;
private float xInput;
```

> 问题：`moveSpeed` 和 `jumpForce` 用 `public` 仅仅是为了在 Inspector 中看到它们，但这也意味着**其他脚本也能随便改它们**，不安全。

---

### 步骤 2：改用 [SerializeField]

把不需要外部访问的 `public` 改成 `[SerializeField] private`：

```csharp
[SerializeField] private float moveSpeed = 8f;
[SerializeField] private float jumpForce = 12f;
private float xInput;
```

![代码修改](../screenshots/011_code.jpg)

---

### 步骤 3：在 Inspector 中验证

选中 Player → Inspector 中仍然能看到 `moveSpeed` 和 `jumpForce` 的输入框，说明 `[SerializeField]` 生效了：

![Inspector显示](../screenshots/011_inspector.jpg)

---

## 📝 三种变量可见性对比

| 声明方式 | Inspector 可见 | 其他脚本可访问 | 用途 |
|---------|:---:|:---:|------|
| `public float x;` | ✅ | ✅ | 需要外部读写的值 |
| `[SerializeField] private float x;` | ✅ | ❌ | 只需自己调试，不让外部改 |
| `private float x;` | ❌ | ❌ | 内部临时变量 |

> 💡 **最佳实践**：默认用 `[SerializeField] private`，只在确认需要外部访问时才用 `public`。

---

## 💻 完整代码

```csharp
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 8f;
    [SerializeField] private float jumpForce = 12f;

    private Rigidbody2D rb;
    private float xInput;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        xInput = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(xInput * moveSpeed, rb.velocity.y);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }
}
```

---

<details>
<summary>📝 英文原版转录</summary>

来源文件：C:\Users\rjq51\Desktop\B站视频下载\transcripts\011_6 Serializefield.txt

本节关键操作摘要：将 moveSpeed、jumpForce 和 Rigidbody2D 等原本为 public 的字段改为 private；需要在 Inspector 调整时加 SerializeField，理解封装与 Inspector 暴露的区别。

摘要根据转录关键句整理，用于定位视频操作；它不是逐字转录，也不替代 Unity 编辑器验证。
</details>

---

> 🏷️ `[SerializeField]` `public` `private` `Inspector` `封装性`

## 本章对应代码进度

> 下面是完成本章后应得到的阶段代码快照。它与前面章节代码逐步衔接，后续章节会继续重构同一职责；第 62 章整合工程的最终实现见 `UnityProject/Assets/Scripts/`。

### 对应文件

`UnityProject/Assets/Scripts/Player/Player.cs`

```csharp
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 8f;
    [SerializeField] private float jumpForce = 12f;

    private Rigidbody2D rb;
    private float xInput;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        xInput = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(xInput * moveSpeed, rb.velocity.y);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }
}
```

### 本章验证

- 阶段代码：已从本章代码示例整理并完成静态核对；片段依赖前置章节时，按本章说明接入对应组件或父类。
- 整合运行：第 62 章 Unity 工程已完成构建，并用本地 `pywinauto + pyautogui` 黑盒脚本验证移动、攻击、受击和反击流程；墙体部分以测试记录中的实际状态为准。
- 结论：本章新增能力在最终整合版中保留；中间阶段代码不宣称作为独立场景单独运行。
