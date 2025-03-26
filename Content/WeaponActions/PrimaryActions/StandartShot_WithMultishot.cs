using Godot;

[GlobalClass]
public partial class StandartShot_WithMultishot : WeaponAction
{
    public override bool Action(Weapon weapon, double delta = -1)
    {
        for (int i = 0; i < weapon.Stats.Multishot; i++)
        {
            Vector2 spawnPosition = weapon.MuzzlePosition;
            Vector2 spawnVelocity = (Vector2.FromAngle(weapon.Holder.Rotation) * weapon.Stats.ProjectileSpeed).Spread(weapon.CalculatedDeviation);

            EmitSignal(GameEvents.SignalName.NewProjectileFromWeapon, weapon, weapon.Stats.ProjectileToShoot, weapon.Holder, spawnPosition, spawnVelocity);
        }

        return true;
    }
}