using System.Collections.Generic;
using Godot;

public partial class WeaponDatabase : Node
{
	public enum WeaponType {
		Pistol,
		Smg,
		Rifle,
		AssaultRifle,
		SniperRifle,
		Shotgun,
		MachineGun,
		RocketLauncher,
		GrenadeLauncher,
		Flamethrower,
		Melee
	}

	public enum ReloadType {
		Full,
		Pump
	}

	public enum ShootType {
		Single,
		Auto
	}

	public enum HoldType {
		Solo,
		Pair
	}



	public static Weapon GetWeapon(string name)
	{
		if (Scenes.TryGetValue(name, out var path))
			return GD.Load<PackedScene>(path).Instantiate<Weapon>();
		return null;
	}	

	static Dictionary<string, string> Scenes = new();

	public override void _Ready()
	{
		Scenes.Add("Esta 8", "res://Content/Weapons/Pistols/Esta 8.tscn");
		Scenes.Add("Initiate 77", "res://Content/Weapons/Assault Rifles/Initiate 77.tscn");
		Scenes.Add("Strais", "res://Content/Weapons/Shotguns/Strais.tscn");
	}
}
