using Godot;
using System.Collections.Generic;
using System.Linq;

[Tool]
[GlobalClass]
public partial class VelocityComponent : Node2D
{
	[Signal]
	public delegate void KinematicCollisionEventHandler(Node2D body);

	[Export(PropertyHint.Range, "0, 1000, or_greater")]
	private float maxSpeed = 0;
	// [Export(PropertyHint.Range, "0, 1000, or_greater")]
	// private float accelerationCoefficient = 200;
	// [Export]
	// private bool debugMode;

	public Vector2 Velocity { get; set; }
	public Vector2? VelocityOverride { get; set; }
	public float SpeedMultiplier { get; set; } = 1f;
	public float SpeedPercentModifier => speedPercentModifiers.Values.Sum();
	public float AccelerationCoefficientMultiplier { get; set; } = 1f;

	//public float AccelerationCoefficient => accelerationCoefficient;
	public float CalculatedMaxSpeed => maxSpeed * (1f + SpeedPercentModifier) * SpeedMultiplier;
	public float SpeedPercent => Mathf.Min(Velocity.Length() / (CalculatedMaxSpeed > 0f ? CalculatedMaxSpeed : 1f), 1f);

	private Dictionary<string, float> speedPercentModifiers = [];

	private KinematicCollision2D currentCollision;
	private KinematicCollision2D previousCollision;
	public bool IsColliding => currentCollision != null;
	public bool IsPreviousCollision => currentCollision == previousCollision;

	public override void _Ready()
	{
	// 	SetProcess(false);
	// 	if (OS.IsDebugBuild() && debugMode)
	// 	{
	// 		(Owner as Node2D)?.Connect("draw", Callable.From(() => OnDebugDraw(Owner as Node2D)));
	// 	}
	// 	SetProcess(true);
	}

	public override void _Process(double delta)
	{
		(Owner as Node2D)?.QueueRedraw();
	}

	// public void AccelerateToVelocity(Vector2 velocity)
	// {
	// 	Velocity = Velocity.Lerp(velocity, 1f - Mathf.Exp(-accelerationCoefficient * AccelerationCoefficientMultiplier /* * (float)...*/));
	// }

	// public void AccelerateInDirection(Vector2 direction)
	// {
	// 	AccelerateToVelocity(direction * CalculatedMaxSpeed);
	// }

	// public void Decelerate()
	// {
	// 	AccelerateToVelocity(Vector2.Zero);
	// }

	public Vector2 GetMaxVelocity(Vector2 direction) => direction * CalculatedMaxSpeed;

	public void MaximizeVelocity(Vector2 direction) => Velocity = GetMaxVelocity(direction);

	public void Move(CharacterBody2D characterBody)
	{
		characterBody.Velocity = VelocityOverride ?? Velocity;
		characterBody.MoveAndSlide();
	}

	public void MoveWithCollision(CharacterBody2D characterBody, double delta)
	{
		characterBody.Velocity = VelocityOverride ?? Velocity;
		previousCollision = currentCollision;
		currentCollision = characterBody.MoveAndCollide(characterBody.Velocity * (float)delta);
		if (currentCollision != null)
		{
			//GD.Print("[VelocityComponent/MoveCollision] ", collision);
			EmitSignal(SignalName.KinematicCollision, currentCollision.GetCollider());
		}
			
	}

	public void Bounce(CharacterBody2D characterBody)
	{
		// GD.Print("[VelocityComponent/Bounce] Bounced off ", collision);
		Velocity = characterBody.Velocity.Bounce(currentCollision.GetNormal());
	}

	public void SetMaxSpeed(float newSpeed) => maxSpeed = newSpeed;

	public void AddSpeedPercentModifier(string name, float change)
	{
		speedPercentModifiers.TryGetValue(name, out var currentValue);
		currentValue += change;
		speedPercentModifiers[name] = currentValue;
	}

	public void SetSpeedPercentModifier(string name, float val)
	{
		speedPercentModifiers[name] = val;
	}

	public float GetSpeedPercentModifier(string name)
	{
		speedPercentModifiers.TryGetValue(name, out var currentValue);
		return currentValue;
	}

	public bool DeleteSpeedPercentModifier(string name)
	{
		return speedPercentModifiers.Remove(name);
	}

	// private void OnDebugDraw(Node2D owner)
	// {
	// 	owner.DrawLine(Vector2.Zero, Velocity, Colors.Cyan, 2f);
	// }
	
}
