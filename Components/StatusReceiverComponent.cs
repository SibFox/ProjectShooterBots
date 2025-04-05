using Godot;
using Godot.Collections;
using System.Linq;

[Tool]
[GlobalClass]
public partial class StatusReceiverComponent : Node2D
{
	private Dictionary<string, double> statusesTimers = [];


	private Dictionary<string, double> hullDamagePercentModifiers = [];
	private Dictionary<string, double> durabilityDamagePercentModifiers = [];
	private Dictionary<string, double> shieldDamagePercentModifiers = [];

	public double HullDamagePercentModifier => hullDamagePercentModifiers.Values.Sum();
	public double DurabilityDamagePercentModifier => durabilityDamagePercentModifiers.Values.Sum();
	public double ShieldDamagePercentModifier => shieldDamagePercentModifiers.Values.Sum();


	public void AddHullDamagePercentModifier(string name, double change)
	{
		hullDamagePercentModifiers.TryGetValue(name, out var currentValue);
		currentValue += change;
		hullDamagePercentModifiers[name] = currentValue;
	}

	public void SetHullDamagePercentModifier(string name, double val)
	{
		hullDamagePercentModifiers[name] = val;
	}

	public double GetHullDamagePercentModifier(string name)
	{
		hullDamagePercentModifiers.TryGetValue(name, out var currentValue);
		return currentValue;
	}

	public bool RemoveHullDamagePercentModifier(string name)
	{
		return hullDamagePercentModifiers.Remove(name);
	}

	public void AddDurabilityDamagePercentModifier(string name, double change)
	{
		durabilityDamagePercentModifiers.TryGetValue(name, out var currentValue);
		currentValue += change;
		durabilityDamagePercentModifiers[name] = currentValue;
	}

	public void SetDurabilityDamagePercentModifier(string name, double val)
	{
		durabilityDamagePercentModifiers[name] = val;
	}

	public double GetDurabilityDamagePercentModifier(string name)
	{
		durabilityDamagePercentModifiers.TryGetValue(name, out var currentValue);
		return currentValue;
	}

	public bool RemoveDurabilityDamagePercentModifier(string name)
	{
		return durabilityDamagePercentModifiers.Remove(name);
	}



	public void ApplyDamageModifiers(ref double hullDamage, ref double durabilityDamage, ref double shieldDamage, double critMultiplier = 0)
	{
		if (critMultiplier > 0)
		{
			hullDamage *= critMultiplier;
			durabilityDamage *= critMultiplier;
			shieldDamage *= critMultiplier;
		}
		hullDamage *= 1f + HullDamagePercentModifier;
		durabilityDamage *= 1f + DurabilityDamagePercentModifier;
		shieldDamage *= 1f + ShieldDamagePercentModifier;
	}
}
