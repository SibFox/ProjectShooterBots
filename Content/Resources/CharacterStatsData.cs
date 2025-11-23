using Godot;

[GlobalClass]
public partial class CharacterStatsData : Resource
{
	[ExportCategory("Characteristics")]

	[Export(PropertyHint.Range, "0, 300, 1, or_greater")]
	public int Toughness = 50;
	[Export(PropertyHint.Range, "0, 100, 1, or_greater")]
	public float HullRecovery = 0;

	[Export(PropertyHint.Range, "0, 1, 0.01, or_greater")]
	public float DurabilityRecovery = 0.2f;
	/// <summary> In Seconds </summary>
	[Export(PropertyHint.Range, "0, 1, 0.01, or_greater")]
	public float DurabilityRecoveryCD = 3f;
	
	[Export(PropertyHint.Range, "0, 1, 0.01, or_greater")]
	public float ShieldRecovery = 0.05f;
	/// <summary> In Seconds </summary>
	[Export(PropertyHint.Range, "0, 1, 0.01, or_greater")]
	public float ShieldRecoveryCD = 3f;
}
