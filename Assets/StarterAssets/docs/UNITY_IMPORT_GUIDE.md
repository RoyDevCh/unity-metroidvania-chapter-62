# Unity 起步美术与音效资源包

这些资源是为当前教程库生成的原创占位资源，用来解决“没有课程原素材也能先把项目跑起来”的问题。

## 导入 Unity

1. 将 `starter_assets/art` 拖入 Unity 项目的 `Assets/Graphics/Starter`。
2. 将 `starter_assets/audio` 拖入 Unity 项目的 `Assets/Audio/Starter`。
3. 对 PNG 执行以下导入设置：
   - `Texture Type`: `Sprite (2D and UI)`
   - `Filter Mode`: `Point (no filter)`
   - `Compression`: `None`
   - Sprite Sheet 使用 `Sprite Mode: Multiple`
4. 需要切片的图：
   - `character_player_sheet_48x32x6x6.png`: 单元格 192 x 128（已按 4 倍放大，对应原始 48 x 32）
   - `enemy_slime_sheet_40x32.png`: 单元格 160 x 128
   - `enemy_skeleton_sheet_40x32.png`: 单元格 160 x 128
   - `enemy_archer_sheet_40x32.png`: 单元格 160 x 128
   - `enemy_boss_deathbringer_sheet_64x56.png`: 单元格 256 x 224
   - `tileset_dark_dungeon_16x16.png`: 单元格 64 x 64
   - `fx_skill_effects_sheet_32x32.png`: 单元格 128 x 128
   - `ui_skill_item_icons_32x32.png`: 单元格 128 x 128

## 推荐映射

| 教程系统 | 可用资源 |
|----------|----------|
| 玩家动画 | `character_player_sheet_48x32x6x6.png` |
| 史莱姆 | `enemy_slime_sheet_40x32.png` |
| 骷髅 | `enemy_skeleton_sheet_40x32.png` |
| 弓箭手 | `enemy_archer_sheet_40x32.png` |
| Boss | `enemy_boss_deathbringer_sheet_64x56.png` |
| 地图 Tilemap | `tileset_dark_dungeon_16x16.png` |
| 飞剑/黑洞/水晶/雷击/命中特效 | `fx_skill_effects_sheet_32x32.png` |
| 技能、物品、UI 图标 | `ui_skill_item_icons_32x32.png` |
| HUD | `ui_hud_atlas.png` |
| 视差背景 | `background_layer_1_sky.png`, `background_layer_2_ruins.png`, `background_layer_3_foreground.png` |

## 音效建议

| 行为 | 音效 |
|------|------|
| 跳跃 | `player_jump.wav` |
| 冲刺 | `player_dash.wav` |
| 普攻三段 | `sword_swing_1.wav`, `sword_swing_2.wav`, `sword_swing_3.wav` |
| 受击 | `hit_light.wav`, `hit_heavy.wav`, `enemy_hurt.wav` |
| 死亡 | `enemy_die.wav` |
| 拾取 | `pickup_coin.wav`, `pickup_item.wav` |
| UI | `ui_click.wav`, `ui_back.wav` |
| 技能 | `skill_clone.wav`, `sword_throw.wav`, `skill_blackhole_start.wav`, `skill_crystal_place.wav`, `skill_crystal_explode.wav`, `skill_thunder.wav` |
| 存档/检查点 | `save_game.wav`, `checkpoint.wav` |
| 环境 | `ambience_dungeon_loop.wav` |

## 注意

这是一版学习用资源包，优先保证清晰、统一、可切片、可导入。后续如果你想把某个角色做成更完整的动画集，可以在这些文件基础上继续扩展。
