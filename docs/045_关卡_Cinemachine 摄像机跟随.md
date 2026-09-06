# Cinemachine 摄像机跟随

> **关卡** | 第 45/192 节 | [[044_关卡_Tilemap 碰撞体|⬅️ Tilemap 碰撞体]] | [[046_关卡_视差背景|➡️ 视差背景]]

---

## 🎯 本节目标

使用 Cinemachine 实现摄像机平滑跟随角色，包括边界限制。

---

## 🖥️ 操作步骤

### 步骤 1：安装 Cinemachine

1. Window → Package Manager
2. 搜索 `Cinemachine` → Install

---

### 步骤 2：创建虚拟摄像机

1. Top Menu → Cinemachine → Create 2D Camera
2. 自动生成 `CM vcam1` 对象

![Cinemachine](../screenshots/045_a.jpg)

---

### 步骤 3：绑定跟随目标

1. 选中 `CM vcam1` → Inspector
2. 将 **Player** 拖入 **Follow** 槽位
3. 设置 **Look Ahead Time** = 0.3（角色移动方向预判）

---

### 步骤 4：设置镜头参数

在 CinemachineVirtualCamera 组件中：

| 参数 | 推荐值 | 说明 |
|------|--------|------|
| **Lens Ortho Size** | 5-8 | 镜头大小，越大看越远 |
| **Damping X/Y** | 1-2 | 跟随延迟，值越大越平滑 |
| **Look Ahead Time** | 0.3 | 预判角色移动方向 |

![镜头设置](../screenshots/045_b.jpg)

---

### 步骤 5：添加边界限制（可选）

1. 使用 **Cinemachine Confiner**：给关卡区域添加 Polygon Collider 2D
2. 勾选 Is Trigger → 设为边界
3. 在 CM vcam1 → Add Extension → Cinemachine Confiner → 拖入边界 Collider

---

## 🎬 操作演示

![摄像机跟随](../gifs/045_Cinemachine摄像机跟随.gif)

---

## 📝 核心要点

```
Cinemachine 2D 摄像机：
  Follow → 跟随目标（Player）
  Damping → 平滑延迟（避免生硬跟随）
  Look Ahead → 预判方向（提前偏移）
  Confiner → 摄像机不超出关卡边界

对比手动跟随：
  旧：transform.position = player.position （生硬）
  新：Cinemachine 自带平滑插值 + 边界限制
```

---

<details>
<summary>📝 英文原版转录</summary>

来源文件：C:\Users\rjq51\Desktop\B站视频下载\transcripts\045_3 Camera.txt

本节关键操作摘要：安装 Cinemachine，创建/配置虚拟相机，把 Player 放入 Follow，调整正交尺寸、Damping 和镜头边界，最后选定本项目使用的相机设置。

摘要根据转录关键句整理，用于定位视频操作；它不是逐字转录，也不替代 Unity 编辑器验证。
</details>

---

> 🏷️ `Cinemachine` `Virtual Camera` `Damping` `Look Ahead` `Confiner` `Ortho Size`
