using UnityEngine;

public class CombatHud : MonoBehaviour
{
    private GUIStyle title;
    private GUIStyle body;
    private Player player;

    private void Start()
    {
        player = FindObjectOfType<Player>();
        Debug.Log("Chapter62: HUD started, player=" + (player == null ? "null" : player.name));
    }

    private void EnsureStyles()
    {
        if (title == null) title = new GUIStyle(GUI.skin.label) { fontSize = 22, fontStyle = FontStyle.Bold };
        if (body == null) body = new GUIStyle(GUI.skin.label) { fontSize = 16 };
    }

    private void OnGUI()
    {
        if (player == null) return;
        EnsureStyles();
        GUI.color = Color.white;
        GUI.Label(new Rect(18, 15, 500, 32), "第 62 章战斗试炼场", title);
        GUI.Label(new Rect(18, 52, 700, 28), "A/D 移动   Space 跳跃   Shift 冲刺   J 攻击   Q 进入弹反，再按 Q 触发招架", body);
        GUI.Label(new Rect(18, 82, 300, 28), "HP  " + Mathf.CeilToInt(player.Health) + " / " + Mathf.CeilToInt(player.MaxHealth), body);
        GUI.Label(new Rect(18, 110, 500, 28), "Player State: " + player.StateLabel, body);
        if (player.WasDamagedRecently)
            GUI.Label(new Rect(18, 138, 500, 28), "Player Hit: Yes", body);
        if (player.CounterSucceeded)
            GUI.Label(new Rect(18, 166, 500, 28), "Counter Result: Stunned", body);
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        for (int i = 0; i < enemies.Length; i++)
        {
            Vector3 screen = Camera.main.WorldToScreenPoint(enemies[i].transform.position + Vector3.up * 1.1f);
            string contact = enemies[i].AttackConnectedRecently ? "  HIT" : "";
            string touching = enemies[i].PlayerTouching ? "  Contact: Yes" : "";
            GUI.Label(new Rect(screen.x - 35f, Screen.height - screen.y, 260, 24), enemies[i].StateLabel + contact + touching + "  " + Mathf.CeilToInt(enemies[i].Health) + "HP", body);
        }
    }
}
