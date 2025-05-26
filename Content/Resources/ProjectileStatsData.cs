using Godot;
using System;

[GlobalClass]
public partial class ProjectileStatsData : Resource
{
	[ExportGroup("Multipliers")]
	[Export(PropertyHint.Range, "0.25, 2, 0.05, or_greater")]
	public double DamageMultiplier = 1;
	[Export(PropertyHint.Range, "0.25, 2, 0.05, or_greater")]
	public double DamageHullMultiplier = 1;
	[Export(PropertyHint.Range, "0.25, 2, 0.05, or_greater")]
	public double DamageDurabilityMultiplier = 1;
	[Export(PropertyHint.Range, "0.25, 2, 0.05, or_greater")]
	public double DamageShieldMultiplier = 1;
	[Export(PropertyHint.Range, "0.25, 2, 0.05, or_greater")]
	public double CritChanceMultiplier = 1;
	[Export(PropertyHint.Range, "0.25, 2, 0.05, or_greater")]
	public double CritDamageMultiplier = 1;
	[Export(PropertyHint.Range, "0.25, 2, 0.05, or_greater")]
	public double WeakpointDamageMultiplier = 1;
	[Export(PropertyHint.Range, "0.25, 2, 0.05, or_greater")]
	public double ArmorPierncingMultiplier = 1;

	[ExportGroup("Base")]
	[Export(PropertyHint.Enum, "ProjectileDatabase.ProjectileType")]
	public ProjectileDatabase.ProjectileType ProjectileType;
	[Export(PropertyHint.ResourceType, "DamageType")]
	public DamageType DamageType;
	[Export(PropertyHint.Range, "0, 5, 1, or_greater")]
	public int RicochetAmount = 1;
	[Export(PropertyHint.Range, "0, 100, 0.01")]
	public double RicochetChance { get => ricochetChance; set => ricochetChance = value; }
	[Export(PropertyHint.Range, "0, 300, 1, or_greater")]
	public int PiercingStrength = 30;
	[Export]
	public bool CanHitOwner = true;

	

	private double ricochetChance = 30;
}
