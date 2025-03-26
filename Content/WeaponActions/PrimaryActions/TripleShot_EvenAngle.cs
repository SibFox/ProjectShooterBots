using Godot;

[GlobalClass]
public partial class TripleShot_EvenAngle : WeaponAction
{
    public override bool Action(Weapon weapon, double delta = -1)
    {
        for (double i = 0; i < 3; i++)
        {
            Vector2 spawnPosition = weapon.MuzzlePosition;
            Vector2 spawnVelocity = (Vector2.FromAngle(weapon.Holder.Rotation) * weapon.Stats.ProjectileSpeed)
            .Rotated((float)Mathf.LerpAngle(-weapon.CalculatedDeviation / 2.0, weapon.CalculatedDeviation / 2.0, i/3));

            EmitSignal(GameEvents.SignalName.NewProjectileFromWeapon, weapon, weapon.Stats.ProjectileToShoot, weapon.Holder, spawnPosition, spawnVelocity);
        }

        return true;
    }
}