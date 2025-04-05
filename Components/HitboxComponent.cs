using Godot;
using Godot.Collections;
using System.Linq;

[Tool]
[GlobalClass]
public partial class HitboxComponent : Area2D
{
	[Export(PropertyHint.Range, "0, 1000, 1, or_greater")]
	private double hullDamage;
	[Export(PropertyHint.Range, "0, 300, 1, or_greater")]
	private double durabilityDamage;
	[Export(PropertyHint.Range, "0, 1000, 1, or_greater")]
	private double shieldDamage;
	[Export(PropertyHint.ResourceType, "DamageType")]
	private DamageType damageType;

	public double HullDamage
	{
		set => hullDamage = value;
		get => hullDamage;
	}

	public double DurabilityDamage
	{
		set => durabilityDamage = value;
		get => durabilityDamage;
	}

	public double ShieldDamage
	{
		set => shieldDamage = value;
		get => shieldDamage;
	}	
}
