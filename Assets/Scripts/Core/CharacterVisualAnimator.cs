using UnityEngine;

public class CharacterVisualAnimator : MonoBehaviour
{
    private Player player;
    private SpriteRenderer renderer;
    private string resourcePath;
    private int columns;
    private int rows;
    private int frameWidth;
    private float pixelsPerUnit;
    private float timer;
    private int frame;
    private string lastState;

    public void Configure(string path, int sheetColumns, int sheetRows, int cellWidth, float ppu)
    {
        resourcePath = path;
        columns = sheetColumns;
        rows = sheetRows;
        frameWidth = cellWidth;
        pixelsPerUnit = ppu;
    }

    private void Awake()
    {
        player = GetComponent<Player>();
        renderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (player == null || renderer == null) return;
        string state = player.StateLabel;
        int row = RowForState(state);
        if (state != lastState)
        {
            lastState = state;
            frame = 0;
            timer = 0f;
            renderer.sprite = RuntimeSprite.Frame(resourcePath, columns, rows, row * columns, pixelsPerUnit);
        }

        timer += Time.deltaTime;
        if (timer < 0.09f) return;
        timer -= 0.09f;
        frame = (frame + 1) % columns;
        renderer.sprite = RuntimeSprite.Frame(resourcePath, columns, rows, row * columns + frame, pixelsPerUnit);
    }

    private int RowForState(string state)
    {
        if (state.Contains("Attack")) return 3;
        if (state.Contains("Dash")) return 1;
        if (state.Contains("Wall")) return 11;
        if (state.Contains("Air")) return 2;
        if (state.Contains("Counter")) return 4;
        if (state.Contains("Move")) return 7;
        return 0;
    }
}
