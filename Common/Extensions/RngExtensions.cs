using Godot;

public static class RngExtensions
{
    public static int i(this RandomNumberGenerator r) => (int)r.Randi();
    
    public static double d(this RandomNumberGenerator r) => (double)r.Randf();

    public static double RanddRange(this RandomNumberGenerator r, double from, double to) => (double)r.RandfRange((float)from, (float)to);

    public static double SpreadAngle(this RandomNumberGenerator r, double degrees) => r.RanddRange(Mathf.DegToRad(-degrees / 2.0), Mathf.DegToRad(degrees / 2.0));

    public static double SpreadAngleRadians(this RandomNumberGenerator r, double radians) => r.RanddRange(-radians / 2.0, radians / 2.0);

    public static double RandomAngle(this RandomNumberGenerator r) => r.RanddRange(0, Mathf.Tau);

    public static double RandomAngleCentered(this RandomNumberGenerator r) => r.RanddRange(0, Mathf.Tau) - Mathf.Tau / 2.0;

    public static bool Percent(this RandomNumberGenerator r, double percent) => r.RanddRange(0, 100) < percent;

    public static Vector2 RandomPointInRectangle(this RandomNumberGenerator r, Rect2 rect) =>
        new(r.RandfRange(rect.Position.X, rect.End.X), r.RandfRange(rect.Position.Y, rect.End.Y));
}