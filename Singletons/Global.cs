using Godot;

[GlobalClass]
public partial class Global : Node
{
	#region Enums
	public enum Stats
    {
        AllDamage,
		HullDamage,
		DurabilityDamage,
		ShieldDamage,
		MaxHull,
		MaxDurability,
		MaxShield
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
