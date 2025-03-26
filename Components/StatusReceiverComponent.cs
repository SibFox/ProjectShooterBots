using Godot;
using System;

[Tool]
[GlobalClass]
public partial class StatusReceiverComponent : Node2D
{
	public double[] ApplyDamageModifiers(ref double hullDamage, ref double durabilityDamage, bool crit, bool hasShieldRemaining)
	{
		double modifiedHullDamage = hullDamage;
		double modifiedDurabiltiyDamage = durabilityDamage;
		return new[] { modifiedHullDamage, modifiedDurabiltiyDamage };
	}
}
