using Godot;

public partial class Projectile : CharacterBody2D
{
	[Signal]
	public delegate void DistanceWentOffEventHandler();
	[Signal]
	public delegate void EffectiveDistanceWentOffEventHandler();


	public Weapon sourceWeapon { get; private set; }
	public Character projectileOwner { get; private set; }
	public double baseHullDamage { get; private set; }
	public double baseDurabilityDamage { get; private set; }
	public double baseCritChance { get; private set; }
	public double baseCritDamage { get; private set; }
	public double baseWeakpointDamage { get; private set; }
	public double baseArmorPierce { get; private set; }
	public double baseMaxDistance { get; private set; }
	public double baseEffectiveDistance { get; private set; }
	
	private double timeFactor = 1f;
	
	private bool active = false;

	[Export(PropertyHint.NodeType, "HitboxComponent")]
	private HitboxComponent hitboxComponent;
	[Export(PropertyHint.NodeType, "VelocityComponent")]
	private VelocityComponent velocityComponent;
	[Export(PropertyHint.ResourceType, "ProjectileStatsData")]
	public ProjectileStatsData Stats;

	public int ricochetAmount;
	public double leftDistance;
	public double leftEffectiveDistance;
	
	public override void _Ready()
	{	
		leftDistance = baseMaxDistance;
		leftEffectiveDistance = baseEffectiveDistance;
		ricochetAmount = Stats.RicochetAmount;

		velocityComponent.KinematicCollision += _on_KinematicCollide;

		active = true;
	}

	/// <summary>
	/// A method for a proper applience of scalings and buffs on Hitbox Component.
	/// </summary>
	public virtual void ApplyDefaults()
	{
		double finalHullDamage = baseHullDamage;
		double finalDurabilityDamage = baseDurabilityDamage;
		double finalShieldDamage = baseHullDamage;
		if (Main.Rand.Percent(baseCritChance))
		{
			finalHullDamage *= baseCritDamage;
			finalShieldDamage *= baseCritDamage;
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		double calculatedDelta = delta * Global.GetCalculatedTimeFactor(timeFactor);
		Update(calculatedDelta);
		PostUpdate(calculatedDelta);
	}

	void Update(double delta)
	{
		velocityComponent.MoveWithCollision(this, delta);

		leftDistance -= Velocity.Length() * delta;
		leftEffectiveDistance -= Velocity.Length() * delta;

		Rotation = Velocity.Angle();
	}

	void PostUpdate(double delta)
	{
		if (IsQueuedForDeletion())
			CallDeferred(nameof(OnKill), leftDistance);
	}

	void _QueueKill(double leftDistance)
	{
		if (OnPreKill(leftDistance))
		{
			QueueFree();
		}
	}

	bool OnPreKill(double leftDistance)
	{
		if (leftDistance < 0) return true;

		// if (velocityComponent.IsColliding)
		// {
		// 	if (Main.Rand.Percent(Stats.RicochetChance) & ricochetAmount > 0)
		// 	{
		// 		--ricochetAmount;
		// 		velocityComponent.Bounce(this);
		// 		return false;
		// 	}
		// }

		return true;
	}

	protected virtual void OnKill(double leftDistance) {}



	void on_DistanceWentOff()
	{
		_QueueKill(leftDistance);
	}

	void on_EffectiveDistanceWentOff()
	{

	}

	void _on_KinematicCollide(Node2D body)
	{
		// GD.Print("[Projectile/KinematicCollide] Collided with ", body, " | ", ricochetAmount);
		if (velocityComponent.IsColliding)
		{
			if (Main.Rand.Percent(Stats.RicochetChance) & ricochetAmount > 0)
			{
				--ricochetAmount;
				velocityComponent.Bounce(this);
				return;
			}
			if (body is not Character & body is not Projectile)
				_QueueKill(leftDistance);
		}
		
	}



	public static void NewProjectileFromWeapon(Weapon weapon, PackedScene projectileScene, Character owner, Vector2 spawnPosition, Vector2 spawnVelocity)
	{
		Projectile projectile = projectileScene.Instantiate<Projectile>();
		projectile.GlobalPosition = spawnPosition;
		projectile.velocityComponent.Velocity = spawnVelocity;
		projectile.Rotation = projectile.velocityComponent.Velocity.Angle();

		projectile.sourceWeapon = weapon;
		projectile.projectileOwner = owner;

		projectile.baseHullDamage = weapon.Stats.HullDamage;
		projectile.baseDurabilityDamage = weapon.Stats.DurabilityDamage;
		projectile.baseCritChance = weapon.Stats.CritChance;
		projectile.baseCritDamage = weapon.Stats.CritDamage;
		projectile.baseMaxDistance = weapon.Stats.MaxDistance;
		projectile.baseEffectiveDistance = weapon.Stats.EffectiveDistance;

		Global.Main.AddProjectileToLevel(projectile);
		GameEvents.EmitNewProjectileFromWeapon(weapon, projectile, owner, spawnPosition, spawnVelocity);
	}
}
