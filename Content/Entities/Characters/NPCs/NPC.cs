using Godot;
using System;

public partial class NPC : Character
{
	public override bool _Setup()
	{
		Inventory.AddWeapon(WeaponDatabase.GetWeapon("Strais"));
		Inventory.SwitchWeaponNode(0);
		return true;
	}
}
