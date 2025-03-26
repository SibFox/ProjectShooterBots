using System.Linq;
using Godot;
using Godot.Collections;

[GlobalClass]
public partial class BetterRng : RandomNumberGenerator
{
    public readonly Array<Vector2> CARDINAL_DIRECTIONS = new() { new(1, 0), new(0, 1), new(-1, 0), new(0, -1) };
    public readonly Array<Vector2> DIAGONAL_DIRECTIONS = new() { new(1, 1), new(1, -1), new(-1, -1), new(-1, 1) };
    public readonly string ascii = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
    public readonly string numbers = "0123456789";

    // public Vector2 RandomDirection(bool diagonals = false, bool zero = false)
    // {
    //     Array<Vector2> dirs = new();
    //     dirs.AddRange(CARDINAL_DIRECTIONS);
    //     if (diagonals) dirs.AddRange(DIAGONAL_DIRECTIONS);
    //     if (zero) dirs.Add(Vector2.Zero);
    //     return Choose<Vector2>(dirs);
    // }

    public int i() => (int)Randi();
    
    public double d() => (double)Randf();

    public double RanddRange(double from, double to) => (double)RandfRange((float)from, (float)to);

    public double SpreadAngle(double degrees) => RanddRange(Mathf.DegToRad(-degrees / 2.0), Mathf.DegToRad(degrees / 2.0));

    public double SpreadAngleRadians(double radians) => RanddRange(-radians / 2.0, radians / 2.0);

    public double RandomAngle() => RanddRange(0, Mathf.Tau);

    public double RandomAngleCentered() => RanddRange(0, Mathf.Tau) - Mathf.Tau / 2.0;
    // public Array Choose<T>(Array array)
    // {
    //     return (Array)array[i() % array.Count - 1];
    // }
}