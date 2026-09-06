"""Repeatable black-box UI acceptance test for the Chapter 62 Windows build."""

from pathlib import Path
import subprocess
import time

import pyautogui
import win32gui
import pytesseract
from pywinauto import Desktop
from PIL import Image


ROOT = Path(__file__).resolve().parents[1]
EXE = ROOT / "Builds" / "Chapter62CombatDemo.exe"
EVIDENCE = ROOT / "test-evidence"
EVIDENCE.mkdir(exist_ok=True)
pyautogui.PAUSE = 0.06
pyautogui.FAILSAFE = False


def activate(window):
    window.restore()
    window.set_focus()
    if win32gui.GetForegroundWindow() != window.handle:
        win32gui.SetForegroundWindow(window.handle)
    time.sleep(0.12)


def start_clean_game():
    subprocess.run(["taskkill", "/F", "/IM", "Chapter62CombatDemo.exe"],
                   stdout=subprocess.DEVNULL, stderr=subprocess.DEVNULL, check=False)
    subprocess.Popen([str(EXE)], cwd=str(EXE.parent))
    time.sleep(4.0)
    window = Desktop(backend="win32").window(title="Chapter62CombatDemo")
    window.wait("exists enabled visible", timeout=10)
    activate(window)
    return window


def capture(window, name):
    activate(window)
    path = EVIDENCE / f"{name}.png"
    window.capture_as_image().save(path)
    print(f"SCREENSHOT {name} {path}")


def hold(key, seconds):
    pyautogui.keyDown(key)
    time.sleep(seconds)
    pyautogui.keyUp(key)


def tap(window, key):
    activate(window)
    pyautogui.press(key)


def movement_scenario():
    window = start_clean_game()
    print(f"SCENARIO movement WINDOW_HANDLE {window.handle}")
    capture(window, "movement_00_initial")
    activate(window)
    hold("d", 0.45)
    capture(window, "movement_01_right")
    tap(window, "space")
    time.sleep(0.14)
    capture(window, "movement_02_jump_airborne")
    time.sleep(0.75)
    capture(window, "movement_03_jump_landed")
    activate(window)
    hold("d", 0.16)
    tap(window, "shift")
    time.sleep(0.08)
    capture(window, "movement_04_dash_active")
    time.sleep(0.72)
    capture(window, "movement_05_dash_recovered")


def combat_scenario():
    window = start_clean_game()
    print(f"SCENARIO combat WINDOW_HANDLE {window.handle}")
    capture(window, "combat_00_initial")
    activate(window)
    hold("d", 0.92)
    time.sleep(0.1)
    capture(window, "combat_01_in_range")
    for index in range(1, 4):
        tap(window, "j")
        time.sleep(0.16)
        capture(window, f"combat_02_attack_{index}_active")
        time.sleep(0.24)
        capture(window, f"combat_03_attack_{index}_resolved")


def counter_scenario():
    window = start_clean_game()
    print(f"SCENARIO counter WINDOW_HANDLE {window.handle}")
    activate(window)
    hold("d", 0.92)
    # 先进入反击状态，再在敌人攻击前摇期间第二次按 Q。
    tap(window, "q")
    # 记录窗口截图，但不调用带额外等待的通用 capture，给黑盒观察留出完整窗口。
    window.capture_as_image().save(EVIDENCE / "counter_01_window.png")
    # 攻击前摇是 0.45 秒；短时序能在反击窗口内稳定命中招架。
    time.sleep(0.12)
    tap(window, "q")
    time.sleep(0.12)
    capture(window, "counter_02_result")
    result = Image.open(EVIDENCE / "counter_02_result.png")
    enemy_area = result.crop((700, 950, 2560, 1150)).resize((3720, 400))
    if "Stunned" not in pytesseract.image_to_string(enemy_area, config="--psm 6"):
        raise AssertionError("counter result did not show Stunned in the enemy HUD")


def wall_scenario():
    window = start_clean_game()
    print(f"SCENARIO wall WINDOW_HANDLE {window.handle}")
    activate(window)
    # 出生点左侧的练习墙在 x=-9.0，向左走到墙边再跳起贴墙。
    hold("a", 0.12)
    tap(window, "space")
    activate(window)
    hold("a", 0.38)
    time.sleep(0.08)
    capture(window, "wall_01_slide")
    activate(window)
    hold("space", 0.10)
    time.sleep(0.08)
    capture(window, "wall_02_wall_jump")

if __name__ == "__main__":
    movement_scenario()
    combat_scenario()
    counter_scenario()
    wall_scenario()
    print("RESULT COMPLETE")
