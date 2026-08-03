using Godot;

public partial class Character : CharacterBody2D
{
	[Signal]
	public delegate void CharacterShootEventHandler(Weapon weapon, MouseButton type);
	[Signal]
	public delegate void CharacterReloadEventHandler(Weapon weapon);
	

	private protected bool active = false;
	private protected double timeFactor = 1f;
	private protected double previousTimeFactor;


	[Export(PropertyHint.ResourceType, "CharacterStatsData")]
	public CharacterStatsData CharacterStats;
	[Export(PropertyHint.NodeType, "HealthComponent")]
	public HealthComponent healthComponent;
	[Export(PropertyHint.NodeType, "HurtboxComponent")]
	private HurtboxComponent hurtboxComponent;
	[Export(PropertyHint.NodeType, "VelocityComponent")]
	private VelocityComponent velocityComponent;


	public Timer HullRegenWaitTime => GetNode<Timer>("Timers/HullRegenWait");
	public Timer DurabilityRegenWaitTime => GetNode<Timer>("Timers/DurabilityRegenWait");
	public Timer ShieldRegenWaitTime => GetNode<Timer>("Timers/ShieldRegenWait");
	public Inventory Inventory => GetNode<Inventory>("Inventory");
	public BuffsContainer BuffsContainer => GetNode<BuffsContainer>("BuffsContainer");

	public double CalculatedDelta { get; private set; }
	public virtual Vector2 Direction => Vector2.Zero;

	public bool IsMoving => Velocity.Length() > 0;


	public override void _Ready()
	{
		active = _Setup();
	}

	public virtual bool _Setup() => false;

	public override void _PhysicsProcess(double delta)
	{
		CalculatedDelta = delta * Global.GetCalculatedTimeFactor(timeFactor);
		MandatoryLogic(CalculatedDelta);
		UpdateVelocity(CalculatedDelta);
		CustomUpdate(CalculatedDelta);
	}

	protected virtual void UpdateVelocity(double delta)
	{
		velocityComponent.MaximizeVelocity(Direction);
		velocityComponent.Move(this);
	}

	protected void MandatoryLogic(double delta)
    {
        if (HullRegenWaitTime.IsStopped() && !healthComponent.IsHullMax)
        {
			healthComponent.Heal((CharacterStats.HullRecoveryFlat * delta) +
				(healthComponent.MaxHullPoints * CharacterStats.HullRecoveryPercent * delta), 0, 0, true);
        }
        if (DurabilityRegenWaitTime.IsStopped() && !healthComponent.IsDurabilityMax)
        {
			healthComponent.Heal(0, (CharacterStats.DurabilityRecoveryFlat * delta) +
				healthComponent.MaxDurability * CharacterStats.DurabilityRecoveryPercent * delta, 0, true);
        }
		if (ShieldRegenWaitTime.IsStopped() && !healthComponent.IsShieldMax)
        {
            healthComponent.Heal(0, 0, (CharacterStats.ShieldRecoveryFlat * delta) +
				healthComponent.MaxShield * CharacterStats.ShieldRecoveryPercent * delta, true);
        }
    }

	protected virtual void CustomUpdate(double delta)
	{
		
    }

	void ManageShot(Weapon weapon, MouseButton type)
	{
		switch (type)
		{
			case MouseButton.Left: weapon?.ActionPrimary(); break;
			case MouseButton.Right: weapon?.ActionSecondary(); break;
		}
	}

	void ManageReload(Weapon weapon)
	{
		weapon?.ActionReload();
	}

	public void AddBuff(Buff buff)
    {
		if (buff is not null)
        {
            buff.Holder = this;
        	BuffsContainer.AddChild(buff);
        }
    }

	protected virtual void OnHullChanged(HealthComponent.HullUpdate hullUpdate)
    {
        if (!hullUpdate.IsHeal)
        	HullRegenWaitTime.Start(CharacterStats.HullRecoveryCD);
    }

	protected virtual void OnDurabilityChanged(HealthComponent.DurabilityUpdate durabilityUpdate)
    {
		if (!durabilityUpdate.IsHeal)
        	DurabilityRegenWaitTime.Start(CharacterStats.DurabilityRecoveryCD);
    }

	protected virtual void OnShieldChanged(HealthComponent.ShieldUpdate shieldUpdate)
    {
		if (!shieldUpdate.IsHeal)
        	ShieldRegenWaitTime.Start(CharacterStats.ShieldRecoveryCD);
    }

	protected virtual void OnHullExposed() {}

	protected virtual void OnShieldDestroyed() {}

	protected virtual void OnHullRegenTimerTimeout() {}

	protected virtual void OnDurabilityRegenTimerTimeout() { }
	
	protected virtual void OnShieldRegenTimerTimeout() {}

	protected virtual void OnDeath()
	{
		QueueFree();
	}
	
}
