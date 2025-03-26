using Godot;

[GlobalClass]
public partial class Main : Node
{
	[Export]
	public bool Debug;

	public static RandomNumberGenerator Rand => new();

	public static GameEvents Events => new();

	public static double GlobalTimeFactor { get; set; } = 1f;

	public Level ActiveLevel => GetNode("Level Handler").GetChild(0) as Level;

	public override void _Ready()
	{
		Global.Main = this;
	}


	public void AddProjectileToLevel(Projectile projectile)
	{
		ActiveLevel.Projectiles.AddChild(projectile);
		// GD.Print($"[Main/AddProjectileToLevel] {projectile}");
	}

    public override void _UnhandledInput(InputEvent @event)
    {
		if (Debug)
		{
			if (@event.IsActionPressed("debug_action_f1"))
			{
				ActiveLevel.QueueFree();
				GetNode("Level Handler").AddChild(GD.Load<PackedScene>("res://Content/Levels/LevelVR1/LevelVR1.tscn").Instantiate<Level>());
			}			
		}
    }

}
