using Godot;

public static class Vector2Extensions
{
    public static Vector2 FromAngle(this Vector2 vector, double angle) => new((float)Mathf.Cos(angle), (float)Mathf.Sin(angle));

    //public static float ToAngle(this Vector2 vector) => Mathf.Atan2(vector.Normalized().Y, vector.Normalized().X);

    public static Vector2 Spread(this Vector2 vector, double spreadRadians) => vector.Rotated((float)Main.Rand.SpreadAngleRadians(spreadRadians));

    public static Vector2 Random(this Vector2 vector, bool normalized = false) => 
        vector.FromAngle(Main.Rand.RandomAngle() * (normalized ? Main.Rand.RanddRange(0, 1) : 1));
}