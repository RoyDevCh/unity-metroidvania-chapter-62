using UnityEngine;

public class DemoBootstrap : MonoBehaviour
{
    private void Awake()
    {
        Physics2D.IgnoreLayerCollision(9, 10, true);
        Camera camera = FindObjectOfType<Camera>();
        if (camera != null) camera.backgroundColor = new Color(0.035f, 0.045f, 0.09f);
        CreateBackground("Chapter62Art/background_layer_1_sky", 8, 0.98f);
        CreateBackground("Chapter62Art/background_layer_2_ruins", 7, 0.98f);
        CreateBackground("Chapter62Art/background_layer_3_foreground", 6, 0.98f);
        CreatePlayer(new Vector2(-8f, -1.4f));
        CreateEnemy(new Vector2(1.5f, -1.4f), "Chapter62Art/enemy_skeleton_sheet_40x32");
        CreateEnemy(new Vector2(8f, -1.4f), "Chapter62Art/enemy_slime_sheet_40x32");
        CreatePlatform(new Vector2(0f, -3f), new Vector2(26f, 1f), new Color(0.18f, 0.22f, 0.34f));
        CreatePlatform(new Vector2(-5f, 0f), new Vector2(4f, 0.55f), new Color(0.26f, 0.3f, 0.45f));
        CreatePlatform(new Vector2(4f, 1.1f), new Vector2(4f, 0.55f), new Color(0.26f, 0.3f, 0.45f));
        // 靠近出生点的练习墙，方便直接验证滑墙和蹬墙跳。
        CreatePlatform(new Vector2(-9.0f, 0.5f), new Vector2(0.6f, 7f), new Color(0.2f, 0.25f, 0.38f));
        CreatePlatform(new Vector2(-12.5f, 1f), new Vector2(0.7f, 8f), new Color(0.2f, 0.25f, 0.38f));
        CreatePlatform(new Vector2(12.5f, 1f), new Vector2(0.7f, 8f), new Color(0.2f, 0.25f, 0.38f));
        if (FindObjectOfType<CombatHud>() == null) new GameObject("Combat HUD").AddComponent<CombatHud>();
    }

    private GameObject CreateActor(string name, Vector2 position, Color color, float width, float height, Sprite sprite = null)
    {
        GameObject go = new GameObject(name);
        go.transform.position = position;
        SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
        sr.sprite = sprite == null ? RuntimeSprite.Square : sprite;
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
        // The supplied pixel sheets are exported at 4x (192x128 per logical 48x32 frame).
        GameObject go = CreateActor("Player", position, Color.white, 0.9f, 1.5f,
            RuntimeSprite.PixelActor("Chapter62Art/character_player_sheet_48x32x6x6", 192, 128, 0, 128f));
        go.layer = 9;
        Player player = go.AddComponent<Player>();
        Transform ground = new GameObject("GroundCheck").transform; ground.SetParent(go.transform); ground.localPosition = new Vector3(0f, -0.78f, 0f);
        Transform wall = new GameObject("WallCheck").transform; wall.SetParent(go.transform); wall.localPosition = new Vector3(0.52f, 0f, 0f);
        Transform attack = new GameObject("AttackCheck").transform; attack.SetParent(go.transform); attack.localPosition = new Vector3(0.9f, 0f, 0f);
        player.ConfigureRuntimeReferences(ground, wall, attack);
    }

    private void CreateEnemy(Vector2 position, string spritePath)
    {
        GameObject go = CreateActor("Enemy", position, Color.white, 0.95f, 1.3f,
            RuntimeSprite.PixelActor(spritePath, 160, 128, 0, 128f));
        go.layer = 10;
        go.AddComponent<Enemy>();
    }

    private void CreatePlatform(Vector2 position, Vector2 size, Color color)
    {
        GameObject go = CreateActor("Ground", position, color, size.x, size.y);
        go.layer = 8;
        go.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
    }

    private void CreateBackground(string spritePath, int sortingOrder, float scale)
    {
        GameObject go = new GameObject("Background_" + sortingOrder);
        go.transform.position = new Vector3(0f, 1.5f, 5f + (8 - sortingOrder));
        go.transform.localScale = Vector3.one * scale;
        SpriteRenderer renderer = go.AddComponent<SpriteRenderer>();
        renderer.sprite = RuntimeSprite.Background(spritePath);
        renderer.sortingOrder = sortingOrder - 20;
    }
}
