using Godot;
using System;

public partial class ExplodingBarrel : MapObject
{
	[Export]
	private PackedScene ExplosionProjectileScene;
	[Export(PropertyHint.Range, "0, 5000, 1, or_greater")]
	public double HullDamage = 1500;
	[Export(PropertyHint.Range, "0, 1000, 1, or_greater")]
	public double DurabilityDamage = 350;

	private CollisionShape2D hitboxComponentShape => GetNode<HitboxComponent>("HitboxComponent").GetChild<CollisionShape2D>(0);


	public override void _Draw()
	{
		// DrawCircle(Vector2.Zero, 100f, Colors.MediumVioletRed);
		DrawCircle(Vector2.Zero, 30f, Colors.DarkRed);
	}

	void HullChanged(HealthComponent.HullUpdate hullUpdate)
	{
		GD.Print($"{DateTime.Now:HH:mm:ss:fff} [Barrel] {Math.Round(hullUpdate.CurrentHull, 2)}/{hullUpdate.MaxHullPoints}");
	}

	void HitByProjectile(HurtboxComponent.ProjectileHitContext projectileHitContext)
	{
		// GD.Print($"{DateTime.Now:HH:mm:ss:fff} [Barrel] {Math.Round(projectileHitContext.ModifiedHullDamage, 2)} hull damage");
	}

	void HitByHitbox(HitboxComponent hitboxComponent)
	{
		// GD.Print($"{DateTime.Now:HH:mm:ss:fff} [Barrel] {Math.Round(hitboxComponent.HullDamage, 2)} hull damage");
	}

	void Explode()
	{
		Global.Main.GetNode<AudioStreamPlayer2D>("ExplosionSound").Play();
		Projectile.NewProjectileFromDamageData(new DamageData() {
			HullDamage = HullDamage,
			DurabilityDamage = DurabilityDamage
		}, ExplosionProjectileScene, this, GlobalPosition, Vector2.Zero, 1);
		GD.Print($"{DateTime.Now:HH:mm:ss:fff} Explode");
		QueueFree();
	}


}
