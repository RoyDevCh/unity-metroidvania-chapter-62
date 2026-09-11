using UnityEngine;

public class DemoBootstrap : MonoBehaviour
{
    private void Awake()
    {
        // Player and enemies must physically meet so attack contact is visible
        // and can be verified independently from the damage calculation.
        Physics2D.IgnoreLayerCollision(9, 10, false);
        Camera camera = FindObjectOfType<Camera>();
        if (camera != null) camera.backgroundColor = new Color(0.035f, 0.045f, 0.09f);
        CreateBackground("Chapter62Art/course_background_layer_1", -30, 2f, new Vector3(0f, 1.5f, 5f));
        CreateBackground("Chapter62Art/course_background_layer_2", -20, 2f, new Vector3(0f, 0.65f, 4f));
        CreateBackground("Chapter62Art/course_background_layer_3", -10, 2f, new Vector3(0f, -2.05f, 3f));
        CreatePlayer(new Vector2(-8f, -1.4f));
        CreateEnemy(new Vector2(1.5f, -1.4f));
        CreateEnemy(new Vector2(8f, -1.4f));
        CreatePlatform(new Vector2(0f, -3f), new Vector2(26f, 1f), "Chapter62Art/course_floor_tile_1", 32f);
        CreatePlatform(new Vector2(-5f, 0f), new Vector2(4f, 0.55f), "Chapter62Art/course_platform_1", 32f);
        CreatePlatform(new Vector2(4f, 1.1f), new Vector2(4f, 0.55f), "Chapter62Art/course_platform_1", 32f);
        // 靠近出生点的练习墙，方便直接验证滑墙和蹬墙跳。
        CreatePlatform(new Vector2(-9.0f, 0.5f), new Vector2(0.6f, 7f), "Chapter62Art/course_brick_1", 32f);
        CreatePlatform(new Vector2(-12.5f, 1f), new Vector2(0.7f, 8f), "Chapter62Art/course_brick_1", 32f);
        CreatePlatform(new Vector2(12.5f, 1f), new Vector2(0.7f, 8f), "Chapter62Art/course_brick_1", 32f);
        CreateDecor("Chapter62Art/course_candelabrum_1", new Vector2(-5.7f, -1.35f), 1.6f, 24);
        CreateDecor("Chapter62Art/course_candelabrum_1", new Vector2(5.7f, -1.35f), 1.6f, 24);
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
            RuntimeSprite.Frame("Chapter62Art/course_warrior_sheet", 6, 17, 0, 64f));
        go.layer = 9;
        Player player = go.AddComponent<Player>();
        Transform ground = new GameObject("GroundCheck").transform; ground.SetParent(go.transform); ground.localPosition = new Vector3(0f, -0.78f, 0f);
        Transform wall = new GameObject("WallCheck").transform; wall.SetParent(go.transform); wall.localPosition = new Vector3(0.52f, 0f, 0f);
        Transform attack = new GameObject("AttackCheck").transform; attack.SetParent(go.transform); attack.localPosition = new Vector3(0.9f, 0f, 0f);
        player.ConfigureRuntimeReferences(ground, wall, attack);
        go.GetComponent<BoxCollider2D>().size = new Vector2(0.52f / 0.9f, 1.18f / 1.5f);
        CharacterVisualAnimator visual = go.AddComponent<CharacterVisualAnimator>();
        visual.Configure("Chapter62Art/course_warrior_sheet", 6, 17, 44, 64f);
    }

    private void CreateEnemy(Vector2 position)
    {
        GameObject go = CreateActor("Enemy", position, Color.white, 1.4f, 1.75f,
            RuntimeSprite.StripFrame("Chapter62Art/course_skeleton_idle", 0, 24, 32, 64f));
        go.layer = 10;
        Enemy enemy = go.AddComponent<Enemy>();
        enemy.ConfigureVisuals("Idle", "Chapter62Art/course_skeleton_idle", 11, 24, 32, 8f, 64f);
        enemy.ConfigureVisuals("Move", "Chapter62Art/course_skeleton_walk", 13, 22, 33, 10f, 64f);
        enemy.ConfigureVisuals("Attack", "Chapter62Art/course_skeleton_attack", 18, 43, 37, 18f, 64f);
        enemy.ConfigureVisuals("Stunned", "Chapter62Art/course_skeleton_hit", 8, 30, 32, 12f, 64f);
        enemy.ConfigureVisuals("Dead", "Chapter62Art/course_skeleton_dead", 15, 33, 32, 16f, 64f);
        go.GetComponent<BoxCollider2D>().size = new Vector2(0.52f / 1.4f, 0.95f / 1.75f);
    }

    private void CreatePlatform(Vector2 position, Vector2 size, string spritePath, float pixelsPerUnit)
    {
        GameObject go = new GameObject("Ground");
        go.transform.position = position;
        SpriteRenderer renderer = go.AddComponent<SpriteRenderer>();
        renderer.sprite = RuntimeSprite.Single(spritePath, pixelsPerUnit);
        renderer.drawMode = SpriteDrawMode.Tiled;
        renderer.size = size;
        go.layer = 8;
        Rigidbody2D rb = go.AddComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Static;
        BoxCollider2D collider = go.AddComponent<BoxCollider2D>();
        collider.size = size;
    }

    private void CreateBackground(string spritePath, int sortingOrder, float scale, Vector3 position)
    {
        GameObject go = new GameObject("Background_" + sortingOrder);
        go.transform.position = position;
        go.transform.localScale = Vector3.one * scale;
        SpriteRenderer renderer = go.AddComponent<SpriteRenderer>();
        renderer.sprite = RuntimeSprite.Background(spritePath);
        renderer.sortingOrder = sortingOrder;
    }

    private void CreateDecor(string spritePath, Vector2 position, float scale, int sortingOrder)
    {
        GameObject go = new GameObject("Decor_Candelabrum");
        go.transform.position = position;
        go.transform.localScale = Vector3.one * scale;
        SpriteRenderer renderer = go.AddComponent<SpriteRenderer>();
        renderer.sprite = RuntimeSprite.Single(spritePath, 128f);
        renderer.sortingOrder = sortingOrder;
    }
}
