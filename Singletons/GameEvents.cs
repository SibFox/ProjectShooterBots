using Godot;


public partial class GameEvents : Node
{
	[Signal]
	public delegate void NewProjectileFromWeaponEventHandler(Weapon weapon, Projectile projectile, Character character, Vector2 spawnPosition, Vector2 spawnVelocity);
	[Signal]
	public delegate void CharacterSwitchedWeaponEventHandler(Weapon currentWeapon, Weapon previousWeapon, Character character);
	[Signal]
	public delegate void EntityCollisionEventHandler(Node2D entity, Weapon weapon, Projectile projectile, SceneTree tree);


	public static Error EmitNewProjectileFromWeapon(Weapon weapon, Projectile projectile, Character character, Vector2 spawnPosition, Vector2 spawnVelocity)
	{
		return Global.Events.EmitSignal(SignalName.NewProjectileFromWeapon, weapon, projectile, character, spawnPosition, spawnVelocity);
	}

	public static Error EmitCharacterSwitchedWeapon(Weapon currentWeapon, Weapon previousWeapon, Character character)
	{
		return Global.Events.EmitSignal(SignalName.CharacterSwitchedWeapon, currentWeapon, previousWeapon, character);
	}

	public static Error EmitEntityCollision(EntityCollisionEvent entityCollision)
	{
		return Global.Events.EmitSignal(SignalName.EntityCollision, entityCollision.Entity, entityCollision.WeaponStats, entityCollision.Projectile, entityCollision.Tree);
	}


	public class EntityCollisionEvent
	{
		public Node2D Entity;
		public Weapon WeaponStats;
		public Projectile Projectile;
		public SceneTree Tree;
	}
}
