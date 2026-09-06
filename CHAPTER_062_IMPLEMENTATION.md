# 前 62 章实现说明

本工程是教程前 62 章的一个最终整合检查点，并非把每一课的临时脚本同时放进项目。课程前半段会反复重构同一功能；项目保留的是第 62 章时能运行、可继续扩展的版本，原始逐章过程仍在 vault 的 `docs/` 中。

- 008-018：`Entity`、`Player`、Rigidbody2D、Collider2D、水平移动、跳跃、落地/墙体检测和翻转。
- 019-022：`PlayerDashState` 的持续时间与冷却；`PlayerAttackState` 的攻击帧判定及三段连击计数。
- 023-027：`Entity` 是玩家和敌人的共同基类，封装生命、朝向、受击闪烁、击退和死亡。
- 028-042：`PlayerStateMachine`、`PlayerState` 和七个具体状态驱动站立、移动、空中、冲刺、滑墙、蹬墙跳、攻击和反击。
- 043-047：`DemoBootstrap` 在 Play Mode 生成平台训练场。学习 Tile Palette、Tilemap Collider、Cinemachine 和视差时，可按文档将这些运行时平台替换为编辑器资源；核心控制器不需要改。
- 048-053：`Enemy` 把巡逻、追击、战斗、攻击、脱战和眩晕状态以紧凑版本实现，便于先理解战斗闭环，再按章节拆成独立 EnemyState 类。
- 054-059：`Player.PerformAttack` 通过 `Physics2D.OverlapCircleAll` 做攻击检测；`Entity.Damage` 集中处理伤害、闪白和击退，并防止状态更新覆盖击退速度。
- 060-062：敌人攻击前摇期间 `CanBeStunned()` 返回 true。玩家首次 Q 进入 `PlayerCounterAttackState`，在窗口期再次 Q 调用 `TryCounter()`，命中则进入 `Enemy.Stun()`。

## 验收动作

1. 打开 `Assets/Scenes/Chapter62_Combat.unity` 并 Play。
2. 从左向右移动，确认跳跃、冲刺和平台落地正常。
3. 靠近敌人按 J 三次，确认敌人生命从 100 下降，受击时短暂变红并被击退。
4. 等敌人 HUD 显示 `Attack`，按 Q 后再按 Q，确认状态切为 `Stunned`。
5. 移向左右边墙，在下落中贴墙，确认下降变慢；此时按 Space，确认角色跳离墙面。

第 063 章之后的 Skill Manager、克隆和飞剑未加入本工程，以保持终点严格停在第 62 章。
