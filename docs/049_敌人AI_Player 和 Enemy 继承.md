# Player 和 Enemy 继承

> **敌人AI** | 第 49/192 节 | [[048_敌人AI_敌人状态机|⬅️ 敌人状态机]] | [[050_敌人AI_敌人待机与巡逻|➡️ 敌人待机与巡逻]]

## 🎯 本节目标
通过创建基类 `Entity` 并利用 C# 的继承（Inheritance）机制，将玩家（Player）和敌人（Enemy）共用的功能（如碰撞检测、翻转逻辑、组件引用）统一管理，消除重复代码并提高可维护性。

## 🖥️ 操作步骤

### 1. 创建基类 Entity
首先，我们需要创建一个所有生物（玩家、敌人等）都继承的父类。

- 在 Project 窗口创建名为 `Entity` 的 C# 脚本。
- 定义基础的生命周期方法，使用 `virtual` 关键字以便子类可以重写（Override）。

```csharp
using UnityEngine;

public class Entity : MonoBehaviour
{
    protected virtual void Awake() { }
    protected virtual void Start() { }
    protected virtual void Update() { }
}
```
**解释：** `virtual` 允许子类通过 `override` 重新定义这些方法的具体行为；访问修饰符用 `protected`，供继承链内部使用。

### 2. 设置 Player 继承 Entity
修改 `Player` 脚本，使其不再直接继承 `MonoBehaviour`，而是继承 `Entity`。

- 将 `public class Player : MonoBehaviour` 改为 `public class Player : Entity`。
- 将 `Awake`, `Start`, `Update` 的访问修饰符改为 `protected override`。

![继承设置](../screenshots/049_new1.jpg)

```csharp
public class Player : Entity
{
    protected override void Awake()
    {
        base.Awake(); // 调用父类方法（可选）
        // Player 特有初始化
    }

    protected override void Start()
    {
        base.Start();
        // Player 特有启动逻辑
    }

    protected override void Update()
    {
        base.Update();
        // Player 特有更新逻辑
    }
}
```
**解释：** `protected` 确保方法在子类中可见，但不对外公开；`override` 表示我们要覆盖父类的默认实现。

### 3. 迁移共用变量与碰撞检测逻辑
将玩家脚本中敌人也会用到的碰撞检测信息（Ground Check、Wall Check）移动到 `Entity` 中。

- 将碰撞检测相关的字段从 `Player` 剪切到 `Entity`，访问修饰符从 `private` 改为 `protected`，以便子类直接访问。
- 地面与墙壁检测改用射线检测（Raycast）：墙壁检测方向要乘上 `facingDir`，保证角色朝哪边就检测哪边。
- 补充 `OnDrawGizmos`，在 Scene 视图中画出检测射线，方便调整检测距离。

![迁移变量](../screenshots/049_new2.jpg)

```csharp
// 在 Entity 类中。组件在 Awake 缓存，保证派生类能在 Awake 创建状态对象时使用。
[Header("Collision info")]
[SerializeField] protected Transform groundCheck;
[SerializeField] protected float groundCheckDistance;
[SerializeField] protected Transform wallCheck;
[SerializeField] protected float wallCheckDistance;
[SerializeField] protected LayerMask whatIsGround;

public virtual bool IsGroundDetected() => groundCheck != null &&
    Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);

public virtual bool IsWallDetected() => wallCheck != null &&
    Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, wallCheckDistance, whatIsGround);

private void OnDrawGizmos()
{
    Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
    Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance * facingDir, wallCheck.position.y));
}
```
**解释：** `groundCheck`、`wallCheck` 空物体挂在角色预制体下，`whatIsGround` 在 Inspector 中选择 `Ground` 层。`wallCheck` 后面敌人战斗状态（第 51 节）也会用到。

### 4. 迁移翻转（Flip）、速度控制与组件引用
将所有生物共有的组件引用（Rigidbody2D、Animator）、翻转逻辑与速度控制方法移至基类。

- 组件引用改为属性形式 `public Animator anim { get; private set; }`，在 `Awake` 中通过 `GetComponentInChildren<Animator>()` 获取，保证状态对象初始化时引用已经存在。
- 翻转除了原有的 `facingRight` 布尔值外，新增 `public int facingDir { get; private set; } = 1` 记录朝向，供射线检测与后续攻击方向使用。
- 新增 `FlipController(float _xInput)`：根据水平输入自动决定是否调用 `Flip()`。

![组件迁移](../screenshots/049_new3.jpg)

```csharp
// 在 Entity 类中
public Animator anim { get; private set; }
public Rigidbody2D rb { get; private set; }

public int facingDir { get; private set; } = 1;
protected bool facingRight = true;

protected virtual void Awake()
{
    anim = GetComponentInChildren<Animator>();
    rb = GetComponent<Rigidbody2D>();
}

public void Flip()
{
    facingDir = facingDir * -1;
    facingRight = !facingRight;
    transform.Rotate(0, 180, 0);
}

public void FlipController(float _xInput)
{
    if (_xInput > 0 && !facingRight)
        Flip();
    else if (_xInput < 0 && facingRight)
        Flip();
}

#region Velocity
public void SetVelocity(float _xVelocity, float _yVelocity)
{
    rb.velocity = new Vector2(_xVelocity, _yVelocity);
    FlipController(_xVelocity);
}

public void SetZeroVelocity() => rb.velocity = Vector2.zero;

// 兼容早期章节名称；新代码统一使用 SetZeroVelocity。
public void ZeroVelocity() => SetZeroVelocity();
#endregion
```

## 💻 完整代码

### Entity.cs (基类)
```csharp
using UnityEngine;

public class Entity : MonoBehaviour
{
    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }

    #region Collision info

    [Header("Collision info")]
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected float groundCheckDistance;
    [SerializeField] protected Transform wallCheck;
    [SerializeField] protected float wallCheckDistance;
    [SerializeField] protected LayerMask whatIsGround;

    public int facingDir { get; private set; } = 1;
    protected bool facingRight = true;

    #endregion

    protected virtual void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    protected virtual void Start()
    {
        // 预留给需要在组件缓存后执行的派生类初始化。
    }

    protected virtual void Update()
    {

    }

    #region Collision

    public virtual bool IsGroundDetected() => groundCheck != null &&
        Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);

    public virtual bool IsWallDetected() => wallCheck != null &&
        Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, wallCheckDistance, whatIsGround);

    private void OnDrawGizmos()
    {
        if (groundCheck == null || wallCheck == null)
            return;

        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance * facingDir, wallCheck.position.y));
    }

    #endregion

    #region Flip

    public void Flip()
    {
        facingDir = facingDir * -1;
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
    }

    public void FlipController(float _xInput)
    {
        if (_xInput > 0 && !facingRight)
            Flip();
        else if (_xInput < 0 && facingRight)
            Flip();
    }

    #endregion

    #region Velocity

    public void SetVelocity(float _xVelocity, float _yVelocity)
    {
        rb.velocity = new Vector2(_xVelocity, _yVelocity);
        FlipController(_xVelocity);
    }

    public void SetZeroVelocity() => rb.velocity = Vector2.zero;

    // 兼容早期章节名称；新代码统一使用 SetZeroVelocity。
    public void ZeroVelocity() => SetZeroVelocity();

    #endregion
}
```
**说明**：后续章节还会继续向 `Entity` 添加攻击检测点 `attackCheck`、受击 `Damage()`、击退协程 `HitKnockback()` 等成员，本节先完成以上公共部分。

### Enemy.cs（统一敌人基类）
```csharp
using UnityEngine;

public class Enemy : Entity
{
    [Header("Detection info")]
    [SerializeField] protected LayerMask whatIsPlayer;
    public EnemyStateMachine stateMachine { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        stateMachine = new EnemyStateMachine();
    }

    protected override void Update()
    {
        stateMachine?.currentState?.Update();
    }

    public void AnimationTrigger()
    {
        stateMachine?.currentState?.AnimationFinishTrigger();
    }

    public virtual RaycastHit2D IsPlayerDetected()
    {
        if (wallCheck == null)
            return default;

        return Physics2D.Raycast(wallCheck.position,
            Vector2.right * facingDir, 50f, whatIsPlayer);
    }
}
```

> 后续章节只扩展这个 `Enemy`，不要再次声明 `Enemy : MonoBehaviour`。`EnemySkeleton`、`EnemySlime` 和 `Enemy_Archer` 都直接继承 `Enemy`。

### Player.cs (子类)
```csharp
using UnityEngine;

public class Player : Entity
{
    protected override void Awake()
    {
        base.Awake(); // 必须调用，以执行 Entity 中的初始化逻辑
    }

    protected override void Update()
    {
        base.Update();

        // 现在可以直接使用父类的 IsGroundDetected()、IsWallDetected() 和 SetVelocity()
        if (IsWallDetected())
            Debug.Log("I am touching the wall!");
    }
}
```

记得把 `Player` 和后续 `Enemy` 物体上的 `groundCheck`、`wallCheck` 子物体与 `whatIsGround` 图层在 Inspector 中拖好赋值。

## 📝 核心要点
- **Inheritance (继承)**：允许 `Player` 和 `Enemy` 共享 `Entity` 的所有 `protected` 和 `public` 成员。
- **Virtual vs Override**：父类方法用 `virtual` 声明，子类用 `override` 实现具体功能。
- **Protected 关键字**：比 `private` 宽松，允许子类访问，但防止外部类随意修改。
- **base 关键字**：用于在子类中调用父类已被重写的方法（如 `base.Awake()`）。
- **facingDir 与 facingRight 双记录**：`int` 用于数学计算（射线方向、速度方向），`bool` 用于判断是否需要翻转，两者在 `Flip()` 中同步更新。

<details>
<summary>英文转录 (English Transcription)</summary>

Hello guys, in this video we're gonna use inheritance to share the same code for player and the enemy. We're gonna make one script that will have all of their data like flip functions, ground detection and wall detection.

I'll create a new script called `Entity`. I'm going to create `public virtual void Awake`, `Start`, and `Update`. Then, I'll make the `Player` inherit from `Entity`. I'll change the methods to `protected override`.

The core goal is to move shared content from the player script to `Entity`. I'll start with the collision part, changing `private` to `protected`. I'll also move the flip logic and component references (Rigidbody2D, Animator) into the `Entity` class so that both the player and enemy can utilize them without duplicating code.
</details>

> 🏷️ #Unity #CSharp #Inheritance #Refactoring #GameArchitecture

## 本章对应代码进度

> 下面是完成本章后应得到的阶段代码快照。它与前面章节代码逐步衔接，后续章节会继续重构同一职责；第 62 章整合工程的最终实现见 `UnityProject/Assets/Scripts/`。

### 对应文件

`UnityProject/Assets/Scripts/Enemy/Enemy.cs / PlayerStates.cs`

```csharp
using UnityEngine;

public class Enemy : Entity
{
    [Header("Detection info")]
    [SerializeField] protected LayerMask whatIsPlayer;
    public EnemyStateMachine stateMachine { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        stateMachine = new EnemyStateMachine();
    }

    protected override void Update()
    {
        stateMachine?.currentState?.Update();
    }

    public void AnimationTrigger()
    {
        stateMachine?.currentState?.AnimationFinishTrigger();
    }

    public virtual RaycastHit2D IsPlayerDetected()
    {
        if (wallCheck == null)
            return default;

        return Physics2D.Raycast(wallCheck.position,
            Vector2.right * facingDir, 50f, whatIsPlayer);
    }
}
```

### 本章验证

- 阶段代码：已从本章代码示例整理并完成静态核对；片段依赖前置章节时，按本章说明接入对应组件或父类。
- 整合运行：第 62 章 Unity 工程已完成构建，并用本地 `pywinauto + pyautogui` 黑盒脚本验证移动、攻击、受击和反击流程；墙体部分以测试记录中的实际状态为准。
- 结论：本章新增能力在最终整合版中保留；中间阶段代码不宣称作为独立场景单独运行。
