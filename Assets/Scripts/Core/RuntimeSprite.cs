using UnityEngine;

public static class RuntimeSprite
{
    private static Sprite square;
    private static readonly System.Collections.Generic.Dictionary<string, Sprite> cache =
        new System.Collections.Generic.Dictionary<string, Sprite>();

    public static Sprite Square
    {
        get
        {
            if (square == null)
            {
                Texture2D texture = new Texture2D(1, 1, TextureFormat.RGBA32, false);
                texture.name = "RuntimeSquare";
                texture.SetPixel(0, 0, Color.white);
                texture.Apply();
                square = Sprite.Create(texture, new Rect(0, 0, 1, 1), new Vector2(0.5f, 0.5f), 1f);
                square.name = "RuntimeSquareSprite";
            }
            return square;
        }
    }

    public static Sprite PixelActor(string resourcePath, int cellWidth, int cellHeight, int rowFromTop, float pixelsPerUnit)
    {
        string key = resourcePath + ":" + cellWidth + ":" + cellHeight + ":" + rowFromTop + ":" + pixelsPerUnit;
        Sprite cached;
        if (cache.TryGetValue(key, out cached)) return cached;

        Texture2D texture = Resources.Load<Texture2D>(resourcePath);
        if (texture == null || texture.width < cellWidth || texture.height < cellHeight)
            return Square;

        int y = texture.height - ((rowFromTop + 1) * cellHeight);
        if (y < 0) return Square;
        Sprite sprite = Sprite.Create(texture, new Rect(0f, y, cellWidth, cellHeight),
            new Vector2(0.5f, 0.5f), pixelsPerUnit);
        sprite.name = resourcePath + "_Frame0";
        cache[key] = sprite;
        return sprite;
    }

    public static Sprite Background(string resourcePath)
    {
        string key = "background:" + resourcePath;
        Sprite cached;
        if (cache.TryGetValue(key, out cached)) return cached;

        Texture2D texture = Resources.Load<Texture2D>(resourcePath);
        if (texture == null) return Square;
        Sprite sprite = Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height),
            new Vector2(0.5f, 0.5f), 30f);
        sprite.name = resourcePath;
        cache[key] = sprite;
        return sprite;
    }

    public static Sprite Single(string resourcePath, float pixelsPerUnit)
    {
        string key = "single:" + resourcePath + ":" + pixelsPerUnit;
        Sprite cached;
        if (cache.TryGetValue(key, out cached)) return cached;
        Texture2D texture = Resources.Load<Texture2D>(resourcePath);
        if (texture == null) return Square;
        Sprite sprite = Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height),
            new Vector2(0.5f, 0.5f), pixelsPerUnit);
        sprite.name = resourcePath;
        cache[key] = sprite;
        return sprite;
    }

    public static Sprite Frame(string resourcePath, int columns, int rows, int frame, float pixelsPerUnit)
    {
        string key = "frame:" + resourcePath + ":" + columns + ":" + rows + ":" + frame + ":" + pixelsPerUnit;
        Sprite cached;
        if (cache.TryGetValue(key, out cached)) return cached;

        Texture2D texture = Resources.Load<Texture2D>(resourcePath);
        if (texture == null || columns < 1 || rows < 1) return Square;
        int clampedFrame = Mathf.Max(0, frame) % (columns * rows);
        int cellWidth = texture.width / columns;
        int cellHeight = texture.height / rows;
        if (cellWidth < 1 || cellHeight < 1) return Square;
        int x = (clampedFrame % columns) * cellWidth;
        int y = texture.height - ((clampedFrame / columns + 1) * cellHeight);
        Sprite sprite = Sprite.Create(texture, new Rect(x, y, cellWidth, cellHeight),
            new Vector2(0.5f, 0.5f), pixelsPerUnit);
        sprite.name = resourcePath + "_Frame" + clampedFrame;
        cache[key] = sprite;
        return sprite;
    }

    public static Sprite StripFrame(string resourcePath, int frame, int frameWidth, int frameHeight, float pixelsPerUnit)
    {
        string key = "strip:" + resourcePath + ":" + frame + ":" + frameWidth + ":" + frameHeight + ":" + pixelsPerUnit;
        Sprite cached;
        if (cache.TryGetValue(key, out cached)) return cached;

        Texture2D texture = Resources.Load<Texture2D>(resourcePath);
        if (texture == null || frameWidth < 1 || frameHeight < 1) return Square;
        int columns = texture.width / frameWidth;
        if (columns < 1 || texture.height < frameHeight) return Square;
        int clampedFrame = Mathf.Max(0, frame) % columns;
        int x = clampedFrame * frameWidth;
        int y = texture.height - frameHeight;
        Sprite sprite = Sprite.Create(texture, new Rect(x, y, frameWidth, frameHeight),
            new Vector2(0.5f, 0.5f), pixelsPerUnit);
        sprite.name = resourcePath + "_Frame" + clampedFrame;
        cache[key] = sprite;
        return sprite;
    }
}
