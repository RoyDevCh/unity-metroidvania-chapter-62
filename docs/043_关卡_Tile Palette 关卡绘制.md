# Tile Palette 关卡绘制

> **关卡** | 第 43/192 节 | [[042_状态机_攻击方向|⬅️ 攻击方向]] | [[044_关卡_Tilemap 碰撞体|➡️ Tilemap 碰撞体]]

---

## 🎯 本节目标

使用 Tile Palette 和 Tilemap 系统高效绘制2D关卡地图。

---

## 🖥️ 操作步骤

### 步骤 1：创建 Tilemap

1. Hierarchy 右键 → 2D Object → Tilemap → Rectangular
2. 重命名为 `Ground`
3. 给 Ground Tilemap 添加 **Tilemap Collider 2D** 组件

![Tile Palette](../screenshots/043_a.jpg)

---

### 步骤 2：创建 Tile Palette

1. Window → 2D → Tile Palette
2. 点击 **Create New Palette** → 命名 `LevelPalette`
3. 保存到 `Assets/Tiles/Palette` 文件夹

![创建Palette](../screenshots/043_b.jpg)

---

### 步骤 3：填充 Tile 资源

1. 在 Project 窗口中选中关卡精灵图（Sprite Sheet）
2. 确认 Sprite Mode = Multiple，已切割
3. 将切割好的 Sprite 拖入 Tile Palette 窗口
4. 在 Palette 中排列成方便选取的布局

---

### 步骤 4：绘制关卡

1. 在 Tile Palette 窗口中选择 Active Tilemap = `Ground`
2. 使用画笔工具 (B) 在 Scene 视图中绘制地图
3. 快捷键：

| 快捷键 | 功能 |
|--------|------|
| **B** | 画笔 |
| **E** | 橡皮擦 |
| **G** | 填充 |
| **,/.** | 切换 Tile |

---

### 步骤 5：设置图层和碰撞

1. 创建多个 Tilemap 层：`Background`（背景）、`Ground`（地面）、`Foreground`（前景）
2. 设置 Rendering 的 Sort Order：背景 < 地面 < 前景
3. 只有 Ground 层需要 Tilemap Collider 2D

![绘制效果](../screenshots/043_c.jpg)

---

## 🎬 操作演示

![Tile Palette绘制](../gifs/043_Tile_Palette绘制关卡.gif)

---

## 📝 核心要点

```
Tilemap 绘制流程：
  1. 创建 Tilemap（每层一个：Background, Ground, Foreground）
  2. 创建 Tile Palette（把精灵拖进来）
  3. 用画笔工具绘制关卡
  4. 地面层加 Tilemap Collider 2D → 角色可以站上去

优化：
  多个 Tilemap 分别控制层级渲染
  使用 Composite Collider 2D 合并碰撞体，提升性能
```

---

<details>
<summary>📝 英文原版转录</summary>

来源文件：C:\Users\rjq51\Desktop\B站视频下载\transcripts\043_1 Tile Palette.txt

本节关键操作摘要：打开 Tile Palette，创建 Tile Palette 资源并整理生成文件，将城堡 Tile 拖入 Palette；用 Tilemap 绘制训练场平台、背景和关卡基础环境。

摘要根据转录关键句整理，用于定位视频操作；它不是逐字转录，也不替代 Unity 编辑器验证。
</details>

---

> 🏷️ `Tilemap` `Tile Palette` `Tilemap Collider 2D` `Composite Collider 2D` `Sort Order`
