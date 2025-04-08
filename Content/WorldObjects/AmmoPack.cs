using Godot;
using System;
using System.Linq;

public partial class AmmoPack : MapObject
{
	protected override void OnInteractAreaEnter(Node2D body)
	{
		if (body is Player player)
		{
			foreach (Weapon weapon in player.Inventory.GetChildren().Cast<Weapon>())
			{
				weapon.AmmoPickup(.5); //TODO: стоит перенести формулу сюда, но как это сделать красиво
			}

            QueueFree();
		}

		
	}

}
