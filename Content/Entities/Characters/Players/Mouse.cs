using Godot;

public partial class Mouse : Node
{
	Player Player => Owner as Player; //GetParent<Player>()
	Marker2D MousePosition => GetNode<Marker2D>("MousePosition");
	Node2D MouseTarget => GetNode<Node2D>("MouseTarget");
	float length;
	float lengthPlayerToTarget;
	[Export]
	float maxTargetDistance = 700;

	public override void _PhysicsProcess(double delta)
	{
		MouseTarget.Position = MousePosition.Position = MousePosition.GetGlobalMousePosition();
		length = MathfExt.Abs(Player.GlobalPosition - MousePosition.Position).Length();

		if (length > maxTargetDistance)
			MouseTarget.Position = Vector2.FromAngle(Player.Rotation) * maxTargetDistance + Player.Position;

		lengthPlayerToTarget = MathfExt.Abs(Player.GlobalPosition - MouseTarget.Position).Length();
	}
}
