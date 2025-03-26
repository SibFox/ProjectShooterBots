using Godot;
using System;

[Tool]
[GlobalClass]
public partial class HurtboxComponent : Area2D
{
    [Signal]
    public delegate void HitByProjectileEventHandler(ProjectileHitContext projectileHitContext);

    [Export(PropertyHint.NodeType, "HealthComponent")]
    private HealthComponent healthComponent;
    private StatusReceiverComponent statusReceiver;

    [Export]
    private PackedScene impactScene;
    [Export]
    private bool detectOnly;

    // public override string[] _GetConfigurationWarnings()
    // {
    //     if (Owner is CharacterBody2D body && (body.CollisionLayer & 1) == 1)
    //     {
    //         return new string[] { "Owner CharacterBody2D has "}
    //     }
    // }

    public override void _Ready()
    {
        //Connect("area_entered", new Callable(this, nameof(OnAreaEntered)));
    }

    public bool CanAcceptProjectileCollision() => healthComponent?.HasHullRemaining ?? true;

    public void HandleProjectileCollision(Projectile projectile)
    {
        GameEvents.EmitEntityCollision(new GameEvents.EntityCollisionEvent()
        {
            Entity = Owner as Node2D,
            WeaponStats = (Weapon)projectile.sourceWeapon.Duplicate(),
            Projectile = projectile,
            Tree = GetTree()
        });

        double hullDamage = 0;
        double durabilityDamage = 0;
        if (!detectOnly)
        {
            hullDamage = projectile.sourceWeapon.Stats.HullDamage;
            durabilityDamage = projectile.sourceWeapon.Stats.DurabilityDamage;
            DealDamageWithModifiers(ref hullDamage, ref durabilityDamage, Main.Rand.Percent(projectile.baseCritChance));
        }

        
        //Эффект от соприкосновения снаряда. Типа крови или искр.
        var impact = impactScene?.Instantiate();
        if (impact != null)
        {
            // Global.Main.Effects.AddImpact(impact);
            // impact.GlobalPosition = projectile.GlobalPosition;
            // impact.Rotation = (-projectile.Direction).Angle();
        }
        

        EmitSignal(SignalName.HitByProjectile, new ProjectileHitContext
        {
            Projectile = projectile,
            ModifiedHullDamage = hullDamage,
            ModifiedDurabilityDamage = durabilityDamage
        });
    }

    public void DealDamageWithModifiers(ref double hullDamage, ref double durabilityDamage, bool crit)
    {
        double[] finalDamage = statusReceiver?.ApplyDamageModifiers(ref hullDamage, ref durabilityDamage, crit, healthComponent.HasShieldRemaining) ?? 
                                                                                                        new [] { hullDamage, durabilityDamage };
        healthComponent?.Damage(finalDamage[0], finalDamage[1]);
    }


    /*public void OnAreaEntered(Area2D area)
    {
        if (area is HitboxComponent hitboxComponent)
        {
            if (!detectOnly)
            {
                DealDamageWithModifiers(hitboxComponent.Damage);
            }
            EmitSignal(SignalName.HitByHitbox, hitboxComponent);
        }
    }*/

    public partial class ProjectileHitContext : RefCounted
	{
        public Projectile Projectile;
		public double ModifiedHullDamage;
        public double ModifiedDurabilityDamage;
	}

}
