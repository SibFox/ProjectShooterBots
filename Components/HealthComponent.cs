using Godot;

[Tool]
[GlobalClass]
public partial class HealthComponent : Node2D
{
	[Signal]
	public delegate void HullChangedEventHandler(HullUpdate hullUpdate);
	[Signal]
	public delegate void DurabilityChangedEventHandler(DurabilityUpdate durUpdate);
	[Signal]
	public delegate void ShieldChangedEventHandler(ShieldUpdate shieldUpdate);
	[Signal]
	public delegate void HullExposedEventHandler();
	[Signal]
	public delegate void ShieldDestroyedEventHandler();
	[Signal]
	public delegate void DiedEventHandler();
	



	[Export(PropertyHint.Range, "0, 10000, 1, or_greater")]
	public double MaxHullPoints 
	{
		get => maxHullPoints;
		private set
		{
			maxHullPoints = value;
			if (CurrentHullPoints > maxHullPoints)
			{
				CurrentHullPoints = maxHullPoints;
			}
		}
	}

	[Export(PropertyHint.Range, "0, 1000, 1, or_greater")]
	public double MaxDurability
	{
		get => maxDurability;
		private set
		{
			maxDurability = value;
			if (CurrentDurability > maxDurability)
			{
				CurrentDurability = maxDurability;
			}
		}
	}

	[Export(PropertyHint.Range, "0, 10000, 1, or_greater")]
	public double MaxShield
	{
		get => maxShield;
		private set
		{
			maxShield = value;
			if (CurrentShield > maxShield)
			{
				CurrentShield = maxShield;
			}
		}
	}

	[Export]
	private bool suppressDamageFloat;

	public bool HasHull => MaxHullPoints > 0;
	public bool HasDurability => MaxDurability > 0;
	public bool HasShield => MaxShield > 0;
	public bool HasHullRemaining => !Mathf.IsEqualApprox(CurrentHullPoints, 0f);
	public bool HasDurabilityRemaining => !Mathf.IsEqualApprox(CurrentDurability, 0f);
	public bool HasShieldRemaining => !Mathf.IsEqualApprox(CurrentShield, 0f);
	public double CurrentHullPointsPercent => MaxHullPoints > 0 ? currentHullPoints / MaxHullPoints : 0f;
	public double CurrentDurabilityPercent => MaxDurability > 0 ? currentDurability / MaxDurability : 0f;
	public double CurrentShieldPercent => MaxShield > 0 ? currentShield / MaxShield : 0f;


	public double CurrentHullPoints
	{
		get => currentHullPoints;
		private set 
		{
			var previousHull = currentHullPoints;
			currentHullPoints = Mathf.Clamp(value, 0, MaxHullPoints);
			var hullUpdate = new HullUpdate 
			{
				PreviousHull = previousHull,
				CurrentHull = currentHullPoints,
				MaxHullPoints = MaxHullPoints,
				HullPointsPercent = CurrentHullPointsPercent,
				IsHeal = previousHull <= currentHullPoints
			};
			EmitSignal(SignalName.HullChanged, hullUpdate);
			if (HasHull & !HasHullRemaining && !hasDied)
			{
				hasDied = true;
				EmitSignal(SignalName.Died);
			}
		}
	}

	public double CurrentDurability
	{
		get => currentDurability;
		private set 
		{
			var previousDur = currentDurability;
			currentDurability = Mathf.Clamp(value, 0, MaxDurability);
			var durUpdate = new DurabilityUpdate 
			{
				PreviousDurability = previousDur,
				CurrentDurability = currentDurability,
				MaxDurability = MaxDurability,
				DurabilityPercent = CurrentDurabilityPercent,
				IsHeal = previousDur <= currentDurability
			};
			EmitSignal(SignalName.DurabilityChanged, durUpdate);
			if (HasDurability & !HasDurabilityRemaining && !isExposed)
			{
				isExposed = true;
				EmitSignal(SignalName.HullExposed);
			}
		}
	}

	public double CurrentShield
	{
		get => currentShield;
		private set 
		{
			var previousShield = currentShield;
			currentShield = Mathf.Clamp(value, 0, MaxShield);
			var shieldUpdate = new ShieldUpdate 
			{
				PreviousShield = previousShield,
				CurrentShield = currentShield,
				MaxShield = MaxShield,
				ShieldPercent = CurrentShieldPercent,
				IsHeal = previousShield <= currentShield
			};
			EmitSignal(SignalName.ShieldChanged, shieldUpdate);
			if (HasShield & !HasShieldRemaining)
			{
				EmitSignal(SignalName.ShieldDestroyed);
			}
		}
	}

	public bool IsDamaged => CurrentHullPoints < MaxHullPoints;

	public bool isExposed;
	private bool hasDied;
	private double maxHullPoints = 2000;
	private double maxDurability = 300;
	private double maxShield = 0;
	private double currentHullPoints;
	private double currentDurability;
	private double currentShield;

	public override void _Ready()
	{
		InitializeHealth();
	}
	// public override void _Notification(int what)
	// {
	//     if (what == NotificationSceneInstantiated)
	//     {
	//         this.WireNodes();
	//     }
	// }

	public void Damage(double hullDamage, double durDamage, bool forceHideDamage = false)
	{
		if (HasShield & HasShieldRemaining)
		{
			CurrentShield -= hullDamage;
		}
		else
		{
			if (!isExposed)
			{
				CurrentDurability -= durDamage;
				CurrentHullPoints -= hullDamage * 0.3;
			}
			else
			{
				CurrentHullPoints -= hullDamage + durDamage * 0.5;
			}
		}
	}

	public void Heal(double hull, double durability)
	{
		Damage(-hull, -durability);
	}

	private void InitializeHealth()
	{
		CurrentHullPoints = MaxHullPoints;
		CurrentDurability = MaxDurability;
		CurrentShield = MaxShield;
	}

	// public void ApplyScalling(Curve curve, float progress)
	// {
	//	   Вызывает метод в конце фрейма
	//     CallDeferred(nameof(ApplyScallingInternal), curve, progress);
	// }

	// private void ApplyScallingInternal(Curve curve, float progress)
	// {
	//     var curveValue = curve.Sample(progress);
	//     MaxHullPoints *= 1f + curveValue;
	//     CurrentHullPoints = MaxHullPoints;
	// }

	public partial class HullUpdate : RefCounted
	{
		public double PreviousHull;
		public double CurrentHull;
		public double MaxHullPoints;
		public double HullPointsPercent;
		public bool IsHeal;
	}

	public partial class DurabilityUpdate : RefCounted
	{
		public double PreviousDurability;
		public double CurrentDurability;
		public double MaxDurability;
		public double DurabilityPercent;
		public bool IsHeal;
	}

	public partial class ShieldUpdate : RefCounted
	{
		public double PreviousShield;
		public double CurrentShield;
		public double MaxShield;
		public double ShieldPercent;
		public bool IsHeal;
	}

}
