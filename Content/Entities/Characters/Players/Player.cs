using Godot;


public partial class Player : Character
{
	public override Vector2 Direction => Input.GetVector("move_left", "move_right", "move_up", "move_down");

	public override bool _Setup()
	{
		Inventory.AddWeapon(WeaponDatabase.GetWeapon("Esta 8"));
		Inventory.AddWeapon(WeaponDatabase.GetWeapon("Initiate 77"));
		Inventory.AddWeapon(WeaponDatabase.GetWeapon("Strais"));
		return true;
	}

	protected override void CustomUpdate(double delta)
	{
		LookAt(GetGlobalMousePosition());
		HandleInputs();
	}

	void HandleInputs()
	{
		Weapon weapon = Inventory.currentWeapon;
		if (weapon != null)
		{
			if (((weapon.Stats.ShootType == WeaponDatabase.ShootType.Single & Input.IsActionJustPressed("action_primary")) |
				(weapon.Stats.ShootType == WeaponDatabase.ShootType.Auto & Input.IsActionPressed("action_primary"))) & weapon.BurstShotsDone == 0)
			{
				EmitSignal(SignalName.CharacterShoot, weapon, 0);
			}

			if ((!weapon.Ejected & Input.IsActionJustPressed("action_eject")) | (weapon.Ejected & Input.IsActionJustPressed("action_reload")) |
				(weapon.Stats.ReloadType == WeaponDatabase.ReloadType.Pump & (Input.IsActionJustPressed("action_eject") | Input.IsActionJustPressed("action_reload"))))
			{
				EmitSignal(SignalName.CharacterReload, weapon);
			}
		}
	}

	public override void _UnhandledInput(InputEvent @event)
    {
		if (@event.IsActionPressed("equip_weapon_previous"))
		{
            (Inventory.previousIndex, Inventory.equipedIndex.Val) = (Inventory.equipedIndex.Val, Inventory.previousIndex);
			Inventory.SwitchWeaponNode(Inventory.equipedIndex.Val);
			GetViewport().SetInputAsHandled();
			return;
        }
        
		Inventory.previousIndex = Inventory.equipedIndex.Val;
        if (@event.IsActionPressed("equip_weapon_1"))
		{
			Inventory.SwitchWeaponNode(0);
			GetViewport().SetInputAsHandled();
		}
		if (@event.IsActionPressed("equip_weapon_2"))
		{
			Inventory.SwitchWeaponNode(1);
			GetViewport().SetInputAsHandled();
		}
		if (@event.IsActionPressed("equip_weapon_3"))
		{
			Inventory.SwitchWeaponNode(2);
			GetViewport().SetInputAsHandled();
		}
		if (@event.IsActionPressed("equip_weapon_scroll_up"))
		{
			Inventory.SwitchWeaponNode(Inventory.equipedIndex.ScrollUp());
			GetViewport().SetInputAsHandled();
		}
		if (@event.IsActionPressed("equip_weapon_scroll_down"))
		{
			Inventory.SwitchWeaponNode(Inventory.equipedIndex.ScrollDown());
			GetViewport().SetInputAsHandled();
		}

    }

}
