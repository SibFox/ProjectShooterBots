using Godot;
using Godot.Collections;


public partial class Inventory : Node2D
{
	private Character Holder => Owner as Character;

	//Consists of maximum 3.
	[Export]	
	private Array<Weapon> Loadout = [];


	public Weapon currentWeapon;
	public Weapon previousWeapon;
	public IntScrollable equipedIndex = new(0, 2);
	public int previousIndex;



	public void SwitchWeaponNode(int index)
	{
		equipedIndex.Val = index;
		if (currentWeapon != null)
		{
			previousWeapon = currentWeapon;
			previousWeapon.InHands = false;
			previousWeapon._StopAllTimers();
		}

		currentWeapon = GetChild<Weapon>(equipedIndex.Val);

		if (currentWeapon != null)
		{
			if (currentWeapon != previousWeapon)
			{
				currentWeapon.InHands = true;
				currentWeapon.Holder = Holder;
			}
			else
			{
				currentWeapon = null;
				previousWeapon = null;
			}
			// Global.Events.EmitSignal(GameEvents.SignalName.CharacterSwitchedWeapon, currentWeapon, previousIndex, Holder);
			GameEvents.EmitCharacterSwitchedWeapon(currentWeapon, previousWeapon, Holder);
		}
	}

    public override void _Ready()
	{
	}

	public override void _Process(double delta)
	{
	}

	public void AddWeapon(Weapon weapon)
	{
		weapon.Holder = Holder;
		AddChild(weapon);
		//GD.Print($"[Inventory] added {weapon}");
	}

	void on_ChildEnteredTree(Node node)
	{
		if (node is not Weapon)
		{
			RemoveChild(node);
			//GD.Print($"[Inventory] removed {node}");
		}
	}
}
