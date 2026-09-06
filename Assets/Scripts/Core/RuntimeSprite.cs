using UnityEngine;

public static class RuntimeSprite
{
    private static Sprite square;

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
}
