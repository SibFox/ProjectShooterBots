using Godot;

public static class MathfExt
{
    public static Vector2 Abs(Vector2 vector)
    {
        return new Vector2(Mathf.Abs(vector.X), Mathf.Abs(vector.Y));
    }
}