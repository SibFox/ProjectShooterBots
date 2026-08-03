using Godot;

[GlobalClass]
public partial class Global : Node
{
	#region Enums
	public enum CharacterStats
	{
		AllDamage,
		HullDamage,
		DurabilityDamage,
		ShieldDamage,
		CritChance,
		CritDamage,
		ArmorPierce,
		MaxHull,            		//+
		MaxDurability,      		//+
		MaxShield,          		//+
		HullRecoveryFlat,			//?
		HullRecoveryPercent,		//?
		HullRecoveryCD,				//?
		DurabilityRecoveryFlat,		//?
		DurabilityRecoveryPercent,	//?
		DurabilityRecoveryCD,		//?
		ShieldRecoveryFlat,			//?
		ShieldRecoveryPercent,		//?
		ShieldRecoveryCD,			//?
		Toughness,					//---
		MaxSpeed,
	}

	public enum WeaponStats
    {
        AllDamage,
		HullDamage,
		DurabilityDamage,
		ShieldDamage,
		CritChance,
		CritDamage,
		ArmorPierce,
    }
	#endregion


	public static Main Main 
	{
		get => mainScene;
		set 
		{
			if (value is Main)
			{
				if (mainScene != null)
					return;
				mainScene = value;
			}            
		}
	}

	public static GameEvents Events => Main.GetNode<GameEvents>("//root/GameEvents");

	private static Main mainScene = null;

	public static double GetCalculatedTimeFactor(double localFactor) => Main.GlobalTimeFactor * localFactor;
}
