using Godot;
using Godot.Collections;
using System.Linq;

[Tool]
[GlobalClass]
public partial class HitboxComponent : Area2D
{
	[Export(PropertyHint.Range, "0, 1000, 1, or_greater")]
	private float hullDamage;
	[Export(PropertyHint.Range, "0, 300, 1, or_greater")]
	private float durabilityDamage;
	[Export(PropertyHint.ResourceType, "DamageType")]
	private DamageType damageType;
	

	public float HullDamagePercentModifier => hullDamagePercentModifiers.Values.Sum();
	public float DurabilityDamagePercentModifier => hullDamagePercentModifiers.Values.Sum();

	public float HullDamage
	{
		set => hullDamage = value;
		get => hullDamage * (1f + HullDamagePercentModifier);
	}

	public float DurabilityDamage
	{
		set => durabilityDamage = value;
		get => durabilityDamage * (1f + DurabilityDamagePercentModifier);
	}

	

	private Dictionary<string, float> hullDamagePercentModifiers = new();
	private Dictionary<string, float> durabilityDamagePercentModifiers = new();




	public void AddHullDamagePercentModifier(string name, float change)
	{
		hullDamagePercentModifiers.TryGetValue(name, out var currentValue);
		currentValue += change;
		hullDamagePercentModifiers[name] = currentValue;
	}

	public void SetHullDamageModifier(string name, float val)
	{
		hullDamagePercentModifiers[name] = val;
	}

	public float GetHullDamageModifier(string name)
	{
		hullDamagePercentModifiers.TryGetValue(name, out var currentValue);
		return currentValue;
	}

	public bool DeleteHullDamageModifier(string name)
	{
		return hullDamagePercentModifiers.Remove(name);
	}

	public void AddDurabilityDamagePercentModifier(string name, float change)
	{
		durabilityDamagePercentModifiers.TryGetValue(name, out var currentValue);
		currentValue += change;
		durabilityDamagePercentModifiers[name] = currentValue;
	}

	public void SetDurabilityDamageModifier(string name, float val)
	{
		durabilityDamagePercentModifiers[name] = val;
	}

	public float GetDurabilityDamageModifier(string name)
	{
		durabilityDamagePercentModifiers.TryGetValue(name, out var currentValue);
		return currentValue;
	}

	public bool DeleteDurabilityDamageModifier(string name)
	{
		return durabilityDamagePercentModifiers.Remove(name);
	}
}
