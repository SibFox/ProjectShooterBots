using Godot;
using System;

[GlobalClass]
public partial class DamageData : Resource
{
	[Export(PropertyHint.Range, "0, 1000, 1, or_greater")]
	public double HullDamage = 100;
	[Export(PropertyHint.Range, "0, 250, 1, or_greater")]
	public double DurabilityDamage = 5;
	[Export(PropertyHint.Range, "0.25, 1, 0.01")]
	public double DamageVariatyLowest = 0.75;
	[Export(PropertyHint.Range, "1, 1.5, 0.01")]
	public double DamageVariatyHighest = 1.15;
	[Export(PropertyHint.Range, "0, 100, 0.01")]
	public double CritChance;
	[Export(PropertyHint.Range, "1, 3, 0.01")]
	public double CritDamage = 1.5;
	[Export(PropertyHint.Range, "0, 100, 1")]
	public int ArmorPierce;
}
