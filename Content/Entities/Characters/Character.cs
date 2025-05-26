using System;
using System.ComponentModel.Design;
using Godot;


public partial class Character : CharacterBody2D
{
	[Signal]
	public delegate void CharacterShootEventHandler(Weapon weapon, int type);
	[Signal]
	public delegate void CharacterReloadEventHandler(Weapon weapon);
	

	private protected bool active = false;
	private protected double timeFactor = 1f;
	private protected double previousTimeFactor;


	[Export(PropertyHint.ResourceType, "CharacterStats")]
	public CharacterStatsData CharacterStats;
	[Export(PropertyHint.NodeType, "HealthComponent")]
	public HealthComponent healthComponent;
	[Export(PropertyHint.NodeType, "HurtboxComponent")]
	private HurtboxComponent hurtboxComponent;
	[Export(PropertyHint.NodeType, "VelocityComponent")]
	private VelocityComponent velocityComponent;


	public Timer DurabilityRegenWaitTime => GetNode<Timer>("Timers/DurabilityRegenWait");
	public Timer ShieldRegenWaitTime => GetNode<Timer>("Timers/ShieldRegenWait");
	public Inventory Inventory => GetNode<Inventory>("Inventory");

	public double CalculatedDelta { get; private set; }
	public virtual Vector2 Direction => Vector2.Zero;

	public bool IsMoving => Velocity.Length() > 0;


	public override void _Ready()
	{
		active = _Setup();
	}

	public virtual bool _Setup() => false;

	public override void _PhysicsProcess(double delta)
	{
		CalculatedDelta = delta * Global.GetCalculatedTimeFactor(timeFactor);
		UpdateVelocity(CalculatedDelta);

		CustomUpdate(CalculatedDelta);
	}

	protected virtual void UpdateVelocity(double delta)
	{
		velocityComponent.MaximizeVelocity(Direction);
		velocityComponent.Move(this);
	}

	protected virtual void CustomUpdate(double delta) {}

	void ManageShot(Weapon weapon, int type)
	{
		switch (type)
		{
			case 0: weapon?.ActionPrimary(); break;
			case 1: weapon?.ActionSecondary(); break;
		}
	}

	void ManageReload(Weapon weapon)
	{
		weapon?.ActionReload();
	}

	protected virtual void OnHit(HealthComponent.HullUpdate hullUpdate) {}

	protected virtual void OnDeath()
	{
		QueueFree();
	}
	
}
