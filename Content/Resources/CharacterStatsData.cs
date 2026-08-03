using Godot;

[GlobalClass]
public partial class CharacterStatsData : Resource
{
	[ExportCategory("Characteristics")]

	private ModifiableStat _toughness = new(50);
	[Export(PropertyHint.Range, "0, 300, 1, or_greater")]
	public double Toughness
    {
        get
		{
			if (Engine.IsEditorHint())
				return _toughness.Value;
			return _toughness.CalculatedValue;
		}
		private set
		{
			_toughness.Value = value;
		}
    }

	private ModifiableStat _hullRecoveryFlat = new(0);
	private ModifiableStat _hullRecoveryPercent = new(0);
	private ModifiableStat _hullRecoveryCD = new(3);
	[Export(PropertyHint.Range, "0, 100, 1, or_greater")]
	public double HullRecoveryFlat
	{
		get
		{
			if (Engine.IsEditorHint())
				return _hullRecoveryFlat.Value;
			return _hullRecoveryFlat.CalculatedValue;
		}
		private set
		{
			_hullRecoveryFlat.Value = value;
		}
	}

	[Export(PropertyHint.Range, "0, 1, 0.01, or_greater")]
	public double HullRecoveryPercent
	{
		get
		{
			if (Engine.IsEditorHint())
				return _hullRecoveryPercent.Value;
			return _hullRecoveryPercent.CalculatedValue;
		}
		private set
		{
			_hullRecoveryPercent.Value = value;
		}
	}

	[Export(PropertyHint.Range, "0, 1, 0.01, or_greater")]
	public double HullRecoveryCD
	{
		get
		{
			if (Engine.IsEditorHint())
				return _hullRecoveryCD.Value;
			return _hullRecoveryCD.CalculatedValue;
		}
		private set
		{
			_hullRecoveryCD.Value = value;
		}
	}

	private ModifiableStat _durRecoveryFlat = new(0);
	private ModifiableStat _durRecoveryPercent = new(0.2);
	private ModifiableStat _durRecoveryCD = new(3);
	[Export(PropertyHint.Range, "0, 100, 1, or_greater")]
	public double DurabilityRecoveryFlat
	{
		get
		{
			if (Engine.IsEditorHint())
				return _durRecoveryFlat.Value;
			return _durRecoveryFlat.CalculatedValue;
		}
		private set
		{
			_durRecoveryFlat.Value = value;
		}
	}

	[Export(PropertyHint.Range, "0, 1, 0.01, or_greater")]
	public double DurabilityRecoveryPercent
	{
		get
		{
			if (Engine.IsEditorHint())
				return _durRecoveryPercent.Value;
			return _durRecoveryPercent.CalculatedValue;
		}
		private set
		{
			_durRecoveryPercent.Value = value;
		}
	}
	
	/// <summary> In Seconds </summary>
	[Export(PropertyHint.Range, "0, 1, 0.01, or_greater")]
	public double DurabilityRecoveryCD
	{
		get
		{
			if (Engine.IsEditorHint())
				return _durRecoveryCD.Value;
			return _durRecoveryCD.CalculatedValue;
		}
		private set
		{
			_durRecoveryCD.Value = value;
		}
	}
	


	private ModifiableStat _shlRecoveryFlat = new(80);
	private ModifiableStat _shlRecoveryPercent = new(0);
	private ModifiableStat _shlRecoveryCD = new(3);
	[Export(PropertyHint.Range, "0, 100, 1, or_greater")]
	public double ShieldRecoveryFlat
	{
		get
		{
			if (Engine.IsEditorHint())
				return _shlRecoveryFlat.Value;
			return _shlRecoveryFlat.CalculatedValue;
		}
		private set
		{
			_shlRecoveryFlat.Value = value;
		}
	}

	[Export(PropertyHint.Range, "0, 1, 0.01, or_greater")]
	public double ShieldRecoveryPercent
	{
		get
		{
			if (Engine.IsEditorHint())
				return _shlRecoveryPercent.Value;
			return _shlRecoveryPercent.CalculatedValue;
		}
		private set
		{
			_shlRecoveryPercent.Value = value;
		}
	}

	/// <summary> In Seconds </summary>
	[Export(PropertyHint.Range, "0, 1, 0.01, or_greater")]
	public double ShieldRecoveryCD
	{
		get
		{
			if (Engine.IsEditorHint())
				return _shlRecoveryCD.Value;
			return _shlRecoveryCD.CalculatedValue;
		}
		private set
		{
			_shlRecoveryCD.Value = value;
		}
	}

	#region Modifer appliences
	// ~~~~~ Tougness
	// ~~~ Add
	public void AddToughnessModifier(Modifier modifier)
	{
		_toughness += modifier;
	}

	// ~~~ Remove
	public void RemoveToughnessModifier(Modifier modifier)
	{
		_toughness -= modifier;
	}

	// ~~~~~ Hull 
	// ~~~ Add
	public void AddHullRecoveryFlatModifier(Modifier modifier)
	{
		_hullRecoveryFlat += modifier;
	}

	public void AddHullRecoveryPercentModifier(Modifier modifier)
	{
		_hullRecoveryPercent += modifier;
	}

	public void AddHullRecoveryCDModifier(Modifier modifier)
	{
		_hullRecoveryCD += modifier;
	}

	// ~~~ Remove
	public void RemoveHullRecoveryFlatModifier(Modifier modifier)
	{
		_hullRecoveryFlat -= modifier;
	}

	public void RemoveHullRecoveryPercentModifier(Modifier modifier)
	{
		_hullRecoveryPercent -= modifier;
	}
	
	public void RemoveHullRecoveryCDModifier(Modifier modifier)
	{
		_hullRecoveryCD -= modifier;
	}

	// ~~~~~ Durability
	// ~~~ Add
	public void AddDurabilityRecoveryFlatModifier(Modifier modifier)
	{
		_durRecoveryFlat += modifier;
	}

	public void AddDurabilityRecoveryPercentModifier(Modifier modifier)
	{
		_durRecoveryPercent += modifier;
	}
	
	public void AddDurabilityRecoveryCDModifier(Modifier modifier)
	{
		_durRecoveryCD += modifier;
	}

	// ~~~ Remove
	public void RemoveDurabilityRecoveryFlatModifier(Modifier modifier)
	{
		_durRecoveryFlat -= modifier;
	}

	public void RemoveDurabilityRecoveryPercentModifier(Modifier modifier)
	{
		_durRecoveryPercent -= modifier;
	}
	
	public void RemoveDurabilityRecoveryCDModifier(Modifier modifier)
	{
		_durRecoveryCD -= modifier;
	}

	// ~~~~~ Shield
	// ~~~ Add
	public void AddShieldRecoveryFlatModifier(Modifier modifier)
	{
		_shlRecoveryFlat += modifier;
	}

	public void AddShieldRecoveryPercentModifier(Modifier modifier)
	{
		_shlRecoveryPercent += modifier;
	}

	public void AddShieldRecoveryCDModifier(Modifier modifier)
	{
		_shlRecoveryCD += modifier;
	}

	// ~~~ Remove
	public void RemoveShieldRecoveryFlatModifier(Modifier modifier)
	{
		_shlRecoveryFlat -= modifier;
	}

	public void RemoveShieldRecoveryPercentModifier(Modifier modifier)
	{
		_shlRecoveryPercent -= modifier;
	}

	public void RemoveShieldRecoveryCDModifier(Modifier modifier)
	{
		_shlRecoveryCD -= modifier;
	}
	#endregion
}
