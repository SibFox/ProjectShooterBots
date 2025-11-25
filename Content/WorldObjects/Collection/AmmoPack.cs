using Godot;
using System;
using System.Linq;

public partial class AmmoPack : MapObject
{
	enum AmmoRichness
	{
		Small,
		Normal,
		Big
	}

	[Export]
	private AmmoRichness Richness = AmmoRichness.Normal;	

	protected override void OnInteractAreaEnter(Node2D body)
	{
		if (body is Player player)
		{
			foreach (Weapon weapon in player.Inventory.GetChildren().Cast<Weapon>())
			{
				weapon.AmmoPickup(Richness switch
				{
					AmmoRichness.Small => .5,
					AmmoRichness.Normal => 1,
					AmmoRichness.Big => 1.5,
					_ => throw new Exception("[AmmoPack] why you there")
				}); //TODO: стоит перенести формулу сюда, но как это сделать красиво
			}

			QueueFree();
		}		
	}
}
