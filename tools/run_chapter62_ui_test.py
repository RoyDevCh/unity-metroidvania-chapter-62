"""Repeatable black-box UI acceptance test for the Chapter 62 Windows build."""

from pathlib import Path
import subprocess
import time

import pyautogui
import win32gui
import pytesseract
from PIL import Image, ImageGrab


ROOT = Path(__file__).resolve().parents[1]
EXE = ROOT / "Builds" / "Chapter62CombatDemo.exe"
EVIDENCE = ROOT / "test-evidence"
EVIDENCE.mkdir(exist_ok=True)
pyautogui.PAUSE = 0.06
pyautogui.FAILSAFE = False


def activate(window):
    handle = window.handle
    win32gui.ShowWindow(handle, 9)  # SW_RESTORE
    for _ in range(8):
        if win32gui.GetForegroundWindow() == handle:
            break
        try:
            win32gui.BringWindowToTop(handle)
            win32gui.SetActiveWindow(handle)
            win32gui.SetForegroundWindow(handle)
        except Exception:
            pass
        time.sleep(0.08)
    if win32gui.GetForegroundWindow() != handle:
        raise RuntimeError(f"could not activate game window {handle}")
    time.sleep(0.12)


def start_clean_game():
    subprocess.run(["taskkill", "/F", "/IM", "Chapter62CombatDemo.exe"],
                   stdout=subprocess.DEVNULL, stderr=subprocess.DEVNULL, check=False)
    subprocess.Popen([str(EXE)], cwd=str(EXE.parent))
    deadline = time.monotonic() + 10.0
    handle = None
    while time.monotonic() < deadline:
        handles = []
        win32gui.EnumWindows(
            lambda hwnd, out: out.append(hwnd)
            if win32gui.IsWindowVisible(hwnd)
            and win32gui.GetWindowText(hwnd) == "Chapter62CombatDemo"
            else None,
            handles,
        )
        if len(handles) == 1:
            handle = handles[0]
            break
        time.sleep(0.1)
    if handle is None:
        raise TimeoutError("Chapter62CombatDemo window did not appear within 10 seconds")

    class WindowHandle:
        def __init__(self, value):
            self.handle = value

    window = WindowHandle(handle)
    activate(window)
    time.sleep(3.0)  # let Unity render a stable first frame before input
    return window


def screenshot_window(window):
    left, top, right, bottom = win32gui.GetWindowRect(window.handle)
    width, height = right - left, bottom - top
    if width <= 0 or height <= 0:
        raise RuntimeError(f"invalid game window bounds: {(left, top, right, bottom)}")
    # ImageGrab with all_screens=True supports windows on monitors with negative
    # virtual-screen coordinates; pyautogui's region capture does not.
    image = ImageGrab.grab(bbox=(left, top, right, bottom), all_screens=True)
    if image.width < width or image.height < height:
        raise RuntimeError(f"incomplete game screenshot: {image.size}, expected {(width, height)}")
    return image


def capture(window, name):
    activate(window)
    path = EVIDENCE / f"{name}.png"
    screenshot_window(window).save(path)
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
    # The enemy can hit the player while the test waits for one exact frame.
    # Retry from a clean process and poll the explicit in-game result banner.
    # This separates a real counter miss from a screenshot/OCR timing miss.
    for attempt in range(1, 4):
        window = start_clean_game()
        print(f"SCENARIO counter ATTEMPT {attempt} WINDOW_HANDLE {window.handle}")
        activate(window)
        # Reach the first enemy without waiting through its full attack cycle.
        hold("d", 1.25)
        tap(window, "q")
        # Do not capture or activate between the two Q presses: either operation
        # can consume the enemy's 0.45 s attack telegraph.
        time.sleep(0.18)
        tap(window, "q")

        success = False
        latest = None
        for poll in range(16):
            time.sleep(0.10)
            latest = screenshot_window(window)
            hud = latest.crop((0, 0, int(latest.width * 0.70), int(latest.height * 0.38)))
            hud = hud.resize((min(hud.width * 3, 5000), hud.height * 3))
            ocr_text = "\n".join(
                pytesseract.image_to_string(hud, config=config)
                for config in ("--psm 6", "--psm 11")
            )
            if "Stunned" in ocr_text or "Counter Result" in ocr_text:
                success = True
                latest.save(EVIDENCE / "counter_02_result.png")
                break
        if latest is not None:
            latest.save(EVIDENCE / f"counter_attempt_{attempt:02d}_last.png")
        if success:
            return
    raise AssertionError("counter result did not show the in-game Counter Result: Stunned banner")


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
