using UnityEngine;

public class DemoBootstrap : MonoBehaviour
{
    private void Awake()
    {
        Physics2D.IgnoreLayerCollision(9, 10, true);
        Camera camera = FindObjectOfType<Camera>();
        if (camera != null) camera.backgroundColor = new Color(0.035f, 0.045f, 0.09f);
        CreatePlayer(new Vector2(-8f, -1.4f));
        CreateEnemy(new Vector2(1.5f, -1.4f));
        CreateEnemy(new Vector2(8f, -1.4f));
        CreatePlatform(new Vector2(0f, -3f), new Vector2(26f, 1f), new Color(0.18f, 0.22f, 0.34f));
        CreatePlatform(new Vector2(-5f, 0f), new Vector2(4f, 0.55f), new Color(0.26f, 0.3f, 0.45f));
        CreatePlatform(new Vector2(4f, 1.1f), new Vector2(4f, 0.55f), new Color(0.26f, 0.3f, 0.45f));
        CreatePlatform(new Vector2(-12.5f, 1f), new Vector2(0.7f, 8f), new Color(0.2f, 0.25f, 0.38f));
        CreatePlatform(new Vector2(12.5f, 1f), new Vector2(0.7f, 8f), new Color(0.2f, 0.25f, 0.38f));
        if (FindObjectOfType<CombatHud>() == null) new GameObject("Combat HUD").AddComponent<CombatHud>();
    }

    private GameObject CreateActor(string name, Vector2 position, Color color, float width, float height)
    {
        GameObject go = new GameObject(name);
        go.transform.position = position;
        SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
        sr.sprite = RuntimeSprite.Square;
        sr.color = color;
        go.transform.localScale = new Vector3(width, height, 1f);
        Rigidbody2D rb = go.AddComponent<Rigidbody2D>();
        rb.gravityScale = 3f;
        rb.freezeRotation = true;
        go.AddComponent<BoxCollider2D>();
        return go;
    }

    private void CreatePlayer(Vector2 position)
    {
        GameObject go = CreateActor("Player", position, new Color(0.3f, 0.85f, 1f), 0.9f, 1.5f);
        go.layer = 9;
        Player player = go.AddComponent<Player>();
        Transform ground = new GameObject("GroundCheck").transform; ground.SetParent(go.transform); ground.localPosition = new Vector3(0f, -0.78f, 0f);
        Transform wall = new GameObject("WallCheck").transform; wall.SetParent(go.transform); wall.localPosition = new Vector3(0.52f, 0f, 0f);
        Transform attack = new GameObject("AttackCheck").transform; attack.SetParent(go.transform); attack.localPosition = new Vector3(0.9f, 0f, 0f);
        player.ConfigureRuntimeReferences(ground, wall, attack);
    }

    private void CreateEnemy(Vector2 position)
    {
        GameObject go = CreateActor("Enemy", position, new Color(1f, 0.35f, 0.45f), 0.95f, 1.3f);
        go.layer = 10;
        go.AddComponent<Enemy>();
    }

    private void CreatePlatform(Vector2 position, Vector2 size, Color color)
    {
        GameObject go = CreateActor("Ground", position, color, size.x, size.y);
        go.layer = 8;
        go.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
    }
}
