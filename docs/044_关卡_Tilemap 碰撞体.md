# Tilemap 碰撞体

> **关卡** | 第 44/192 节 | [[043_关卡_Tile Palette 关卡绘制|⬅️ Tile Palette 关卡绘制]] | [[045_关卡_Cinemachine 摄像机跟随|➡️ Cinemachine 摄像机跟随]]

## 🎯 本节目标

在已会用 Tile Palette 绘制关卡的基础上，为 **Ground / Background** 两层 Tilemap 配置 **Tilemap Collider 2D + Composite Collider 2D + Static Rigidbody2D**，画出可站立的训练场，并区分「能踩的平台」与「纯装饰背景墙」。

> ⚠️ 本节以编辑器操作为主，**不需要写代码**。

## 🖥️ 操作步骤

### 1. 创建 Ground 与 Background Tilemap

1. 删除旧的手搓 Platform（若有），本节要画训练场。
2. Hierarchy → 右键 → **2D Object → Tilemap → Rectangular**，命名为 `Ground`。
3. 将 `Ground` 的 **Layer** 设为 `Ground`。
4. 再创建一层 Rectangular Tilemap，命名为 `Background`。
5. 在 **Tilemap Renderer** 里将 Background 的 **Order in Layer** 设为 `-1`，使其绘制在 Ground 后面。

![场景与图层](../screenshots/044_1.jpg)

### 2. 添加碰撞体（Ground）

选中 **Ground**：

1. **Add Component** → `Tilemap Collider 2D`
2. **Add Component** → `Composite Collider 2D`（会自动加上 `Rigidbody2D`）
3. **Composite Collider 2D**：**Geometry Type** = `Polygons`
4. **Rigidbody2D**：**Body Type** = `Static`（不受重力、不会下落）
5. **Tilemap Collider 2D**：勾选 **Used By Composite**

![碰撞组件](../screenshots/044_2.jpg)

### 3. 用 Tile Palette 绘制训练场

1. 打开 **Window → 2D → Tile Palette**，**Active Tilemap** 选 `Ground` 画平台/地面/天花板。
2. 常用工具：**Brush (B)** 绘制、**Shift + 左键** 擦除、**Fill** 大面积填充、**Selection / Move** 微调整块。
3. 画墙或装饰时，把 **Active Tilemap** 切到 `Background`，这样墙体**没有碰撞**，角色不能站在墙上。
4. 快速搭一个「地板 + 天花板 + 入口墙」即可，不必精雕细琢——后面还会改关卡结构。

![绘制效果](../screenshots/044_3.jpg)

## 💻 完整代码

*本节为 Tilemap 与碰撞体配置，暂无 C# 代码。*

## 📝 核心要点

- **Ground**：带碰撞，角色可站立。
- **Background**：无碰撞（或不加 Collider），仅视觉。
- **Composite + Static RB**：合并 Tile 碰撞、性能更好，且平台固定不动。
- **Active Tilemap** 画错层会导致「墙能踩上去」——先切对图层再画。

<details>
<summary>英文转录 (English Transcription)</summary>

Hello guys, I deleted platforms because we're going to draw kind of a training ground in this section. In this video, I'm going to show you how to use tile palette to draw levels… We need to add tile map collider. We need to add composite collider. Composite collider should have geometry type polygons. Then we need to cover this open rigid body and choose body type static… In tile map collider, check used by composite. Create background tilemap, order in layer -1. When drawing walls, activate background tile map so they are not walkable… Don't spend too much time here; make it look like a training ground and move to the next video.

</details>

> 🏷️ #Unity2D #Tilemap #TilemapCollider2D #CompositeCollider2D #LevelDesign
