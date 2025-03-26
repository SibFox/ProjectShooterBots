using Godot;
using Godot.Collections;

[GlobalClass]
public partial class WeaponStatsData : Resource
{
	[ExportGroup("Base")]
	[Export]
	public WeaponDatabase.WeaponType WeaponType;
	[Export(PropertyHint.Range, "0, 1000, 1, or_greater")]
	public double HullDamage = 100;
	[Export(PropertyHint.Range, "0, 250, 1, or_greater")]
	public double DurabilityDamage = 5;
	[Export(PropertyHint.Range, "0.25, 1, 0.01")]
	public double DamageVariatyLowest = 0.75;
	[Export(PropertyHint.Range, "1, 1.5, 0.01")]
	public double DamageVariatyHighest = 1.15;
	[Export(PropertyHint.Range, "0, 100, 0.01")]
	public double CritChance { get => critChance; set => critChance = value; }
	[Export(PropertyHint.Range, "1, 3, 0.01")]
	public double CritDamage = 1.5;
	[Export(PropertyHint.Range, "0, 100, 1")]
	public int ArmorPiering;
	[Export(PropertyHint.Range, "0.01, 3, 0.01")]
	public double RateOfFire = 0.2;

	// [ExportGroup("Actions")]
	// [Export]
	// public Array<CSharpScript> PrimaryActions;
	// [Export]
	// public Array<CSharpScript> SecondaryActions;

	[ExportGroup("Ammunition")]
	[Export]
	public int ClipSize = 10;
	[Export(PropertyHint.Range, "0, 10, 0.1, or_greater")]
	public double ClipGain = 2;
	[Export]
	public int ClipsMax = 5;

	[ExportGroup("Shooting Behaviour")]
	[Export]
	public WeaponDatabase.ShootType ShootType;
	[Export(PropertyHint.Range, "1, 10, 1, or_greater")]
	public int BurstAmount = 1;
	[Export(PropertyHint.Range, "0.01, 1, 0.01, or_greater")]
	public double BurstDelay = 0.2;
	[Export(PropertyHint.Range, "1, 10, 1, or_greater")]
	public int Multishot = 1;
	[Export(PropertyHint.Range, "1, 10, 1, or_greater")]
	public int AmmoConsumption = 1;

	[ExportGroup("Reload Behaviour")]
	[Export]
	public WeaponDatabase.ReloadType ReloadType;
	[Export(PropertyHint.Range, "0, 5, 0.01, or_greater")]
	public double ReloadTime = 2;
	[Export(PropertyHint.Range, "0, 5, 0.01, or_greater")]
	public double EjectTime = 0.5;
	[Export(PropertyHint.Range, "1, 10, 1, or_greater")]
	public int ReloadAmount = 1;

	[ExportGroup("Weapon Behaviour")]
	[ExportSubgroup("Accuracy")]
	[Export(PropertyHint.Range, "0, 90, 0.1, radians_as_degrees")]
	public double Deviation;
	[Export(PropertyHint.Range, "0, 90, 0.1, radians_as_degrees")]
	public double DeviationMax;
	[Export(PropertyHint.Range, "0, 90, 0.1, radians_as_degrees")]
	public double Recoil;
	[Export(PropertyHint.Range, "0, 90, 0.1, radians_as_degrees")]
	public double RecoilResetStrength;
	// [Export(PropertyHint.Range, "0, 3, 0.01")]
	// public double RecoilResetTime = 0.5;
	[Export(PropertyHint.Range, "1, 3, 0.01")]
	public double MovementInaccuracy = 1.4;

	[ExportSubgroup("Heat")]
	[Export(PropertyHint.Range, "0, 100, 0.1")]
	public double HeatGain = 5;
	[Export(PropertyHint.Range, "0, 100, 0.1")]
	public double CoolStrength = 20;
	[Export(PropertyHint.Range, "0, 3, 0.01")]
	public double HeatResetTime = 1;

	[ExportSubgroup("Charge Shot")]
	[Export(PropertyHint.Range, "0, 3, 0.01, or_greater")]
	public double ChargeTime = 0;
	[Export]
	public bool ChargeForEveryShot;
	[Export]
	public bool ShootImmediatlyWhenCharged;

	[ExportSubgroup("Additional")]
	[Export(PropertyHint.Range, "0, 1, 0.01")]
	public double SlowingOnFire = 0.35;
	[Export(PropertyHint.Range, "0, 3, 0.01")]
	public double SlowingTime = 0.3;
	[Export(PropertyHint.Range, "0, 100, 0.01")]
	public double JamChance { get => jamChance; set => jamChance = value; }
	[Export]
	public WeaponDatabase.HoldType HoldType;
	
	[ExportGroup("Projectile")]
	[Export]
	public PackedScene ProjectileToShoot;
	[Export(PropertyHint.Range, "0, 10000, 1, or_greater")]
	public float ProjectileSpeed = 3000;
	[Export(PropertyHint.Range, "0, 10000, 1, or_greater")]
	public double MaxDistance = 1500;
	[Export(PropertyHint.Range, "0, 10000, 1, or_greater")]
	public double EffectiveDistance = 1000;



	private double critChance = 0;
	private double jamChance = 5;
}
