using Godot;
using System;

[Tool]
[GlobalClass]
public partial class HurtboxComponent : Area2D
{
    [Signal]
    public delegate void HitByProjectileEventHandler(ProjectileHitContext projectileHitContext);
    [Signal]
    public delegate void HitByHitboxEventHandler(HitboxComponent hitboxComponent);

    [Export(PropertyHint.NodeType, "HealthComponent")]
    private HealthComponent healthComponent;
    [Export(PropertyHint.NodeType, "StatusReceiverComponent")]
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

    // public override void _Ready()
    // {
    //     Connect("area_entered", new Callable(this, nameof(OnAreaEntered)));
    // }

    public bool CanAcceptProjectileCollision() => healthComponent?.HasHullRemaining ?? true;

    public void HandleProjectileCollision(Projectile projectile)
    {
        GameEvents.EmitEntityCollision(new GameEvents.EntityCollisionEvent()
        {
            Entity = Owner as Node2D,
            WeaponStats = (Weapon)projectile.sourceWeapon.Duplicate(),
            Projectile = (Projectile)projectile.Duplicate(),
            Tree = GetTree()
        });

        double hullDamage = 0;
        double durabilityDamage = 0;
        double shieldDamage = 0;
        if (!detectOnly)
        {
            hullDamage = projectile.hitboxComponent.HullDamage;
            durabilityDamage = projectile.hitboxComponent.DurabilityDamage;
            shieldDamage = projectile.hitboxComponent.ShieldDamage;
            DealDamageWithModifiers(ref hullDamage, ref durabilityDamage, ref shieldDamage, projectile.Crit ? projectile.baseCritDamage : 0);
        }

        
        //Эффект от соприкосновения снаряда. Типа крови или искр.
        var impact = impactScene?.Instantiate();
        if (impact != null)
        {
            // impact.GlobalPosition = projectile.GlobalPosition;
            // impact.Rotation = (-projectile.Direction).Angle();
            // Global.Main.ActiveLevel?.AddImpact(impact)
        }
        

        EmitSignal(SignalName.HitByProjectile, new ProjectileHitContext
        {
            Projectile = projectile,
            ModifiedHullDamage = hullDamage,
            ModifiedDurabilityDamage = durabilityDamage,
            ModifiedShieldDamage = shieldDamage
        });
    }

    public void DealDamageWithModifiers(ref double hullDamage, ref double durabilityDamage, ref double shieldDamage, double critMultiplier)
    {
        statusReceiver?.ApplyDamageModifiers(ref hullDamage, ref durabilityDamage, ref shieldDamage, critMultiplier);
        healthComponent?.Damage(hullDamage, durabilityDamage, shieldDamage);
    }


    public void OnAreaEntered(Area2D area)
    {
        if (area is HitboxComponent hitboxComponent)
        {
            if (!detectOnly)
            {
                if (hitboxComponent.Owner is Projectile projectile)
                {
                    HandleProjectileCollision(projectile);
                    return;
                }
                
                double hullDamage = hitboxComponent.HullDamage, durabilityDamage = hitboxComponent.DurabilityDamage, shieldDamage = hitboxComponent.ShieldDamage;
                DealDamageWithModifiers(ref hullDamage, ref durabilityDamage, ref shieldDamage, 0);
            }
            EmitSignal(SignalName.HitByHitbox, hitboxComponent);
        }
    }

    public partial class ProjectileHitContext : RefCounted
	{
        public Projectile Projectile;
		public double ModifiedHullDamage;
        public double ModifiedDurabilityDamage;
        public double ModifiedShieldDamage;
	}

}
