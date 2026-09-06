using UnityEngine;

public class CombatHud : MonoBehaviour
{
    private GUIStyle title;
    private GUIStyle body;
    private Player player;

    private void Start()
    {
        player = FindObjectOfType<Player>();
        title = new GUIStyle(GUI.skin.label) { fontSize = 22, fontStyle = FontStyle.Bold };
        body = new GUIStyle(GUI.skin.label) { fontSize = 16 };
    }

    private void OnGUI()
    {
        if (player == null) return;
        GUI.color = Color.white;
        GUI.Label(new Rect(18, 15, 500, 32), "第 62 章战斗试炼场", title);
        GUI.Label(new Rect(18, 52, 700, 28), "A/D 移动   Space 跳跃   Shift 冲刺   J 攻击   Q 进入弹反，再按 Q 触发招架", body);
        GUI.Label(new Rect(18, 82, 300, 28), "HP  " + Mathf.CeilToInt(player.Health) + " / " + Mathf.CeilToInt(player.MaxHealth), body);
        GUI.Label(new Rect(18, 110, 500, 28), "Player State: " + player.StateLabel, body);
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        for (int i = 0; i < enemies.Length; i++)
        {
            Vector3 screen = Camera.main.WorldToScreenPoint(enemies[i].transform.position + Vector3.up * 1.1f);
            GUI.Label(new Rect(screen.x - 35f, Screen.height - screen.y, 160, 24), enemies[i].StateLabel + "  " + Mathf.CeilToInt(enemies[i].Health) + "HP", body);
        }
    }
}
