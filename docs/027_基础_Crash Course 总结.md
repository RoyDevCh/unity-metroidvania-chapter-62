# Crash Course 总结

> **基础** | 第 27/192 节 | [[026_基础_敌人攻击|⬅️ 敌人攻击]] | [[028_状态机_什么是状态机|➡️ 什么是状态机]]

## 🎯 本节目标
回顾 Unity 基础速成阶段，明确接下来的 RPG 游戏开发目标，并为进入正式的第一章节做好心理和项目准备。

## 🖥️ 操作步骤

### 1. 阶段性回顾与心态建设
在正式开始制作 RPG 游戏之前，我们需要意识到 RPG 的开发规模远大于简单的平台跳跃（Platformer）或无尽跑酷（Endless Runner）。
- **核心挑战**：我们将构建一个灵活且可扩展的角色系统，包含多种技能、物品（Items）、属性（Stats）以及类似《黑暗之魂》（Souls-like）的机制。
- **学习路径**：虽然系统复杂，但教程将引导你一步步实现。不要被庞大的系统吓到，重点在于理解机制的拆解。

![回顾总结](../screenshots/027_new1.jpg)

### 2. 关于课程结构的“小秘密”
为了保证学习效率，本课程采取**“主干与基础分离”**的教学模式：
- **第 0 章节**：作为 Unity 速成课程，为零基础学习者提供基础知识（如 `float` 类型、方法调用等）。
- **后续章节**：将直接进入核心开发，不再重复讲解基础语法。这样可以避免经验丰富的开发者感到进度缓慢，同时确保初学者已有基础可循。

![教学模式](../screenshots/027_new2.jpg)

### 3. 项目清理与新起点
在进入下一章节之前，我们将采取“推倒重来”的策略，以确保项目结构的纯净和逻辑的连贯性。
- **操作建议**：你可以直接创建新项目，或者删除当前项目中的大部分内容。
- **可选保留**：如果你想保留之前制作的 `animations` 文件夹，可以将其移至新项目，但重新制作也仅需几分钟。
- **重点提醒**：之前关于 Skeleton 的攻击动画和伤害系统未完全完成，这是因为该部分的教学重点在于理解 **继承（Inheritance）** 概念，而非完成该 Demo。

![项目清理](../screenshots/027_new3.jpg)

## 💻 完整代码
*本节为课程过渡章节，无需编写代码。*

## 📝 核心要点
- **RPG 复杂度**：涉及角色扩展性、物品系统、属性系统及复杂机制。
- **教学逻辑**：基础知识在 Section 0 完成，后续章节直接进入实战，不重复讲解基础 API。
- **项目状态**：接下来的章节将从一个全新的项目开始，重点将转向 **状态机（State Machine）** 的构建。

<details>
<summary>英文转录 (English Transcription)</summary>

Hello guys, this is the end of the section zero we had here. Kind of we can call it I guess a crash course on Unity on things you need to know before we begin. I would say yeah and well yeah actually, it's a pretty good explanation. A couple of hours of tutorials as I think you need to know before we begin is something that explains it very good on what we're going to do.

But don't get scared, don't get discouraged. Remember we're about to create an RPG game which is kind of a big deal. You know like when you start the game development you create in a platformer or endless runner or maybe some top-down shooter that is really simple to create. Even here what we have here in our game it's basically we have a platformer right we have jumps and walk left and right and so on. But creating an RPG game is something bigger. It's not only about a character being flexible and scalable with multiple skills. There are also items, stats, maybe XP bar. We're not going to have XP bar we're going to have a souls like in Dark Souls but still in our lots of things, lots of mechanics and systems and I'll walk you through it don't worry.

It's not going to be that difficult as you might think or as I might draw it. It's going to be pretty simple to understand I just need some time and I want to say in the next section I'm going to it's kind of like we have a little secret between us. I'm going to act like nothing happened like there was no section zero. All right I'm not going to refer to anything...
</details>

> 🏷️ #Unity #RPG #CourseIntroduction #ProjectSetup
