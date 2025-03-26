using Godot;

public partial class WeaponAction : RefCounted
{
    public virtual bool Action( Weapon weapon, double delta = -1) => false;
}