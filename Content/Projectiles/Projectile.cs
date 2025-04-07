using System;
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
	public bool Crit { get; private set; }
	
	private double timeFactor = 1f;
	
	private bool active = false;

	[Export(PropertyHint.NodeType, "HitboxComponent")]
	public HitboxComponent hitboxComponent { get; private set; }
	[Export(PropertyHint.NodeType, "VelocityComponent")]
	public VelocityComponent velocityComponent { get; private set; }
	[Export(PropertyHint.ResourceType, "ProjectileStatsData")]
	public ProjectileStatsData Stats { get; private set; }

	public int ricochetAmount { get; private set; }
	public double remainedDistance { get; private set; }
	public double remainedEffectiveDistance { get; private set; }
	
	public override void _Ready()
	{	
		if (Stats == null) Free();
		remainedDistance = baseMaxDistance;
		remainedEffectiveDistance = baseEffectiveDistance;
		ricochetAmount = Stats.RicochetAmount;

		// velocityComponent.KinematicCollision += OnKinematicCollision;

		ApplyDefaults();

		active = true;
	}

	/// <summary>
	/// A method for a proper applience of scalings and buffs for Hitbox Component.
	/// </summary>
	protected virtual void ApplyDefaults()
	{
		Crit = Main.Rand.Percent(baseCritChance + 10);
		double rangeMultiplier = Main.Rand.RanddRange(sourceWeapon.Stats.DamageVariatyLowest, sourceWeapon.Stats.DamageVariatyHighest);
		hitboxComponent.HullDamage = baseHullDamage * Stats.DamageHullMultiplier * rangeMultiplier;
		hitboxComponent.DurabilityDamage = baseDurabilityDamage * Stats.DamageDurabilityMultiplier * rangeMultiplier;
		hitboxComponent.ShieldDamage = baseHullDamage * Stats.DamageShieldMultiplier * rangeMultiplier;
	}

	public override void _PhysicsProcess(double delta)
	{
		double calculatedDelta = delta * Global.GetCalculatedTimeFactor(timeFactor);
		PreUpdate(calculatedDelta);
		Update(calculatedDelta);
		PostUpdate(calculatedDelta);
	}


	void PreUpdate(double delta)
	{
		velocityComponent.MoveWithCollision(this, delta);

		remainedDistance -= Velocity.Length() * delta;
		remainedEffectiveDistance -= Velocity.Length() * delta;

		Rotation = Velocity.Angle();
	}

	protected virtual void Update(double delta) {}

	void PostUpdate(double delta)
	{
		if (IsQueuedForDeletion())
			CallDeferred(nameof(OnKill), remainedDistance);
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
		// if (leftDistance < 0) return true;

		return true;
	}

	protected virtual void OnKill(double leftDistance) {}



	protected virtual void OnDistanceWentOff()
	{
		_QueueKill(remainedDistance);
	}

	protected virtual void OnEffectiveDistanceWentOff() {}

	void OnHitboxAreaEntered(Area2D area)
	{
		OnHit(area);
		_QueueKill(remainedDistance);
	}

	protected virtual void OnHit(Area2D area) {}

	void OnKinematicCollision(Node2D body)
	{
		// GD.Print("[Projectile/KinematicCollide] Collided with ", body, " | ", ricochetAmount);
		if (velocityComponent.IsColliding)
		{
			// GD.Print($"{DateTime.Now:HH:mm:ss:fff} [Projectile] Столкновение ({Stats.RicochetChance})");
			if (Main.Rand.Percent(Stats.RicochetChance) & ricochetAmount > 0)
			{
				--ricochetAmount;
				velocityComponent.Bounce(this);
				// GD.Print($"{DateTime.Now:HH:mm:ss:fff} [Projectile] Рикошет");
				OnRichochet(body);
				return;
			}
			if (body is not Character & body is not Projectile)
				_QueueKill(remainedDistance);
		}
		
	}

	protected virtual void OnRichochet(Node2D body) {}


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
