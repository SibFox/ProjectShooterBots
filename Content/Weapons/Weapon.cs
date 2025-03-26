using System.Linq;
using Godot;
using Godot.Collections;

public partial class Weapon : Node2D
{
	[Export]
	public string WeaponName;
	[Export(PropertyHint.MultilineText)]
	public string WeaponDescription;
	[Export(PropertyHint.ResourceType, "WeaponStatsData")]
	public WeaponStatsData Stats;

	public Character Holder;

	private AnimatedSprite2D Sprite => GetNode<AnimatedSprite2D>("Sprite");

	public Timer RateOfFire => GetNode<Timer>("Timers/RateOfFire");
	public Timer ReloadTime => GetNode<Timer>("Timers/ReloadTime");
	public Timer EjectTime => GetNode<Timer>("Timers/EjectTime");
	// public Timer RecoilResetTime => GetNode<Timer>("Timers/RecoilReset");
	public Timer HeatResetTime => GetNode<Timer>("Timers/HeatReset");

	private AudioStreamPlayer2D SoundShoot => GetNode<AudioStreamPlayer2D>("Sounds/Shoot");
	private AudioStreamPlayer2D SoundEject => GetNode<AudioStreamPlayer2D>("Sounds/Eject");
	private AudioStreamPlayer2D SoundReload => GetNode<AudioStreamPlayer2D>("Sounds/Reload");
	private AudioStreamPlayer2D SoundJam => GetNode<AudioStreamPlayer2D>("Sounds/Jam");
	private AudioStreamPlayer2D SoundEquip => GetNode<AudioStreamPlayer2D>("Sounds/Equip");

	public Vector2 MuzzlePosition  => GetNode<Marker2D>("Muzzle").GlobalPosition;
	public Vector2 MuzzlePosition2 => GetNode<Marker2D>("Muzzle2").GlobalPosition;

	public int ClipAmmoCurrent 	{ get; private set; }
	public int AmmoStored 		{ get; private set; }
	public int BurstShotsDone 	{ get; private set; }

	public double RecoilApplied { get; private set; }
	public double MaxHeat 		{ get => 100; }
	public double Heat 			{ get; private set; }
	public double ChargeTimeNow { get; private set; }

	public bool InHands 		{ get; set; }
	public bool IsBurst			{ get; private set; }
	public bool Ejected 		{ get; private set; }
	public bool CanBeReloaded 	{ get; private set; }
	public bool Jammed 			{ get; private set; }
	public bool Charged 		{ get; private set; }
	
	// private delegate bool PrimaryActionsToPerform(Weapon weapon, double delta = -1);
	// private delegate bool SecondaryActionsToPerform(Weapon weapon, double delta = -1);
	// private delegate bool UpdateActions(Weapon weapon, double delta);
	// private delegate bool UpdateActionsInHands(Weapon weapon, double delta);
	
	// private PrimaryActionsToPerform primaryActionsToPerform;
	// private SecondaryActionsToPerform secondaryActionsToPerform;
	// private UpdateActions updateActionsToPerform;
	// private UpdateActionsInHands updateInHandsActionsToPerfrom;

	// [Export(PropertyHint.ResourceType, "WeaponAction")]
	// private Array<WeaponAction> primaryActions;
	// [Export(PropertyHint.ResourceType, "CSharpScript")]
	// private Array<CSharpScript> secondaryActions;
	// [Export(PropertyHint.ResourceType, "CSharpScript")]
	// private Array<CSharpScript> updateActions;
	// [Export(PropertyHint.ResourceType, "CSharpScript")]
	// private Array<CSharpScript> updateInHandsActions;

	public override void _Ready()
	{
		RateOfFire.WaitTime = Stats.RateOfFire;
		EjectTime.WaitTime = Stats.EjectTime;
		ReloadTime.WaitTime = Stats.ReloadTime;

		ClipAmmoCurrent = Stats.ClipSize;
		AmmoStored = Stats.ClipsMax * Stats.ClipSize / 2;

		IsBurst = Stats.BurstAmount > 1;		
	}
	
	// private bool ActionsSetup()
	// {
	// 	GD.Print($"Set: {primaryActions}");
	// 	if (primaryActions.Count > 0)
	// 		foreach (WeaponAction action in primaryActions)
	// 			primaryActionsToPerform += action.Action;
	// 	if (secondaryActions.Count > 0)
	// 		foreach (CSharpScript action in secondaryActions)
	// 			secondaryActionsToPerform += action.Action;
	// 	if (updateActions.Count > 0)
	// 		foreach (CSharpScript action in updateActions)
	// 			updateActionsToPerform += action.Action;
	// 	if (updateInHandsActions.Count > 0)
	// 		foreach (CSharpScript action in updateInHandsActions)
	// 			updateInHandsActionsToPerfrom += action.Action;
		
		
	// 	return true;
	// }
	

	public override void _PhysicsProcess(double delta)
	{
		double calculatedDelta = Holder.CalculatedDelta;
		Visible = InHands;
		if (Holder != null)
		{
			_Update(calculatedDelta);

			if (InHands)
			{
				UpdateInHands(calculatedDelta);
				//updateInHandsActionsToPerfrom?.Invoke(this, calculatedDelta);
			}
			else _StopAllTimers();

			Update(calculatedDelta);
			//updateActionsToPerform?.Invoke(this, calculatedDelta);
		}
	}
	
	public void _StopAllTimers()
	{
		RateOfFire.Stop();
		ReloadTime.Stop();
	}

	private void _Update(double delta)
	{
		// if (RecoilResetTime.IsStopped())
		RecoilApplied -= Stats.RecoilResetStrength * delta;

		IsBurst = Stats.BurstAmount > 1;
		CanBeReloaded = Ejected;

		if (CalculatedDeviation > Stats.DeviationMax || (CalculatedDeviation > Stats.DeviationMax * Stats.MovementInaccuracy & Holder.IsMoving))
		{
			RecoilApplied -= Stats.RecoilResetStrength / 3 * delta;
		}

		HandleSprite();

		ClipAmmoCurrent = Mathf.Clamp(ClipAmmoCurrent, 0, Stats.ClipSize);
		AmmoStored = Mathf.Clamp(AmmoStored, 0, Stats.ClipsMax * Stats.ClipSize);
		RecoilApplied = Mathf.Clamp(RecoilApplied, 0, Mathf.DegToRad(90)); //Holder.IsMoving ? Stats.DeviationMax * Stats.MovementInaccuracy : Stats.DeviationMax
	}

	public virtual void Update(double delta) {}

	public virtual void UpdateInHands(double delta) {}

	private void HandleSprite()
	{
		Sprite.Animation = "hold";
		if ((Ejected || !EjectTime.IsStopped()) & Stats.ReloadType != WeaponDatabase.ReloadType.Pump)
		{
			Sprite.Animation = "eject";
		}
		if (!ReloadTime.IsStopped())
		{
			Sprite.Animation = "reload";
		}
	}

	private protected virtual void _on_RateOfFire_Timeout()
	{
		if (IsBurst & BurstShotsDone > 0)
			ActionPrimary();
	}

	private protected virtual void _on_EjectTime_Timeout()
	{
		Ejected = true;
	}

	private protected virtual void _on_ReloadTime_Timeout()
	{
		if (AmmoStored > 0)
		{
			BurstShotsDone = 0;
			switch (Stats.ReloadType)
			{
				case WeaponDatabase.ReloadType.Full:
				{
					ClipAmmoCurrent = Mathf.Clamp(Stats.ClipSize, 0, AmmoStored);
					AmmoStored -= ClipAmmoCurrent;
					Ejected = false;
				} break;
				case WeaponDatabase.ReloadType.Pump:
				{
					int reloadAmount = Mathf.Clamp(Stats.ReloadAmount, 0, AmmoStored);
					ClipAmmoCurrent += reloadAmount;
					AmmoStored -= reloadAmount;
					SoundReload.Play();
					if (ClipAmmoCurrent != Stats.ClipSize & AmmoStored > 0)
					{
						ReloadTime.Start(Stats.ReloadTime);
					}
				} break;
			}
		}
	}

	private protected virtual void _on_RecoilReset_Timeout() {}

	private protected virtual void _on_HeatReset_Timeout() {}

	public virtual bool Shoot()
	{
		for (int i = 0; i < Stats.Multishot; i++)
		{
			Vector2 spawnPosition = MuzzlePosition;
			Vector2 spawnVelocity = (Vector2.FromAngle(Holder.Rotation) * Stats.ProjectileSpeed).Spread(CalculatedDeviation);
			
			Projectile.NewProjectileFromWeapon(this, Stats.ProjectileToShoot, Holder, spawnPosition, spawnVelocity);
		}

		return true;
	}

	public void ActionPrimary()
	{
		if (Stats.ReloadType == WeaponDatabase.ReloadType.Pump & !ReloadTime.IsStopped())
		{
			ReloadTime.Stop();
			return;
		}

		if (RateOfFire.IsStopped() & ClipAmmoCurrent > 0)
		{
			if (Shoot() /*primaryActionsToPerform != null*/)
			{
				// GD.Print($"[Weapon/ActionPrimary] entered fire");
				// if (primaryActionsToPerform(this))
				// {
				// }
				SoundShoot.Play();

				ClipAmmoCurrent -= Stats.AmmoConsumption;
				RecoilApplied += RecoilModified;

				if (IsBurst & BurstShotsDone < Stats.BurstAmount - 1)
				{
					++BurstShotsDone;
					RateOfFire.Start(Stats.BurstDelay);
				}
				else
				{
					BurstShotsDone = 0;
					RateOfFire.Start(Stats.RateOfFire);
				}

				// RecoilResetTime.Start(Stats.RecoilResetTime);
			}
		}
	}

	public void ActionSecondary()
	{

	}

	public void ActionReload()
	{
		if (AmmoStored > 0 & ClipAmmoCurrent < Stats.ClipSize)
		{
			if (!Ejected & EjectTime.IsStopped() & Stats.ReloadType != WeaponDatabase.ReloadType.Pump)
			{
				ClipAmmoCurrent = 0;
				EjectTime.Start(Stats.EjectTime);
				SoundEject.Play();
				return;
			}
			if (ReloadTime.IsStopped() & EjectTime.IsStopped())
			{
				ReloadTime.Start((Stats.ReloadType != WeaponDatabase.ReloadType.Pump) ? Stats.ReloadTime : Stats.ReloadTime + Stats.EjectTime);
				if (Stats.ReloadType != WeaponDatabase.ReloadType.Pump)
				{
					SoundReload.Play();
				}
			}	
		}
	}

	public void AmmoPickup(double multiplier)
	{
		AmmoStored += Mathf.Clamp((int)(Stats.ClipSize * (Stats.ClipGain * multiplier)), 0, Stats.ClipsMax * Stats.ClipSize);
	}



	public double RecoilModified
	{
		get
		{
			double modified = Stats.Recoil;

			return modified;
		}		
	}
	
	public double CalculatedDeviation
	{
		get 
		{
			double modifiedMax = Stats.DeviationMax;
			double modified = Stats.Deviation + RecoilApplied;

			if (Holder.IsMoving)
			{
				modifiedMax *= Stats.MovementInaccuracy;
				modified *= Stats.MovementInaccuracy;
			}
			return Mathf.Clamp(modified, 0, modifiedMax);
		}
		
	}
}
