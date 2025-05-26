using Godot;
using Godot.Collections;

public partial class ProjectileDatabase : Node
{
    public enum ProjectileType
    {
        Balistic,
        Hitscan,
        Explosion
    }




    public Projectile GetProjectile(string name)
	{
		if (Scenes.TryGetValue(name, out var path))
		    return GD.Load<PackedScene>(path).Instantiate<Projectile>();
        return null;
	}	
    
    Dictionary<string, string> Scenes = [];

    public override void _Ready()
    {
        Scenes.Add("Pulse Bullet", "res://Content/Projectiles/PulseBullet.tscn");
        Scenes.Add("Physic Explosion", "res://Content/Projectiles/PhysicExplosion.tscn");
    }
}