using Godot;

[GlobalClass]
public partial class AbsoluteStraightShot : WeaponAction
{
    public override bool Action(Weapon weapon, double delta = -1)
    {
        Vector2 spawnPosition = weapon.MuzzlePosition;
        Vector2 spawnVelocity = Vector2.FromAngle(weapon.Holder.Rotation) * weapon.Stats.ProjectileSpeed;

        EmitSignal(GameEvents.SignalName.NewProjectileFromWeapon, weapon, weapon.Stats.ProjectileToShoot, weapon.Holder, spawnPosition, spawnVelocity);

        return true;
    }
}