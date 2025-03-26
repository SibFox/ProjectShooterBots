using System.Linq;
using Godot;

public partial class PlayerHud : Control
{
	[Export]
	private Player player;
	private HealthComponent component => player.healthComponent;

	private Weapon weapon;
	
	private Panel OverallPanel => GetNode<Panel>("OverallPanel");
	private ProgressBar HullBar => GetNode<ProgressBar>("%HullBar");
	private ProgressBar DurabilityBar => GetNode<ProgressBar>("%DurabilityBar");
	private ProgressBar ShieldBar => GetNode<ProgressBar>("%ShieldBar");
	private ProgressBar ReloadBar => GetNode<ProgressBar>("%ReloadBar");
	private Label WeaponName => GetNode<Label>("%WeaponName");
	private Label HPDurValue => GetNode<Label>("%HPDurValue");
	private Label ShieldValue => GetNode<Label>("%ShieldValue");
	private Label BulletCount => GetNode<Label>("%BulletCount");
	private Label BulletStoredCount => GetNode<Label>("%BulletStoredCount");
	private Label ReloadSign => GetNode<Label>("%ReloadSign");
	private Label EmptySign => GetNode<Label>("%EmptySign");
	private Label DebugRecoil => GetNode<Label>("%DebugRecoil");
	private GridContainer ClipSize => GetNode<GridContainer>("%ClipSize");

	private StyleBox _durabilityBarFillTheme;
	private Color _durabilityBarStoredColor;
	private Color _durabilityExposedColor = Colors.Brown;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Global.Events.CharacterSwitchedWeapon += _on_PlayerSwitchWeapon;

		_durabilityBarFillTheme = DurabilityBar.GetThemeStylebox("fill");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
			// Connect(GameEvents.SignalName.CharacterSwitchedWeapon, new Callable(this, "_on_PlayerSwitchWeapon"));
		

		Size = GetViewport().GetVisibleRect().Size;
		OverallPanel.SetSize(new(1280/2, OverallPanel.Size.Y));
		OverallPanel.SetPosition(new(Size.X / 2 - OverallPanel.Size.X / 2, GetViewport().GetVisibleRect().Size.Y - OverallPanel.Size.Y));

		if (player != null)
		{
			weapon = player.Inventory.currentWeapon;

			VisibilityReset();
			HealthBars();
			if (weapon != null)
				WeaponInHands();
		}
	}

	private void VisibilityReset()
	{
		WeaponName.Visible = weapon != null;
		BulletCount.Visible = weapon != null;
		BulletStoredCount.Visible = weapon != null;
		ReloadBar.Visible = weapon != null;
		ReloadSign.Visible = weapon != null;
		EmptySign.Visible = weapon != null;
		DebugRecoil.Visible = weapon != null;

		HullBar.Visible = component.HasHull;
		DurabilityBar.Visible = component.HasDurability;
		ShieldBar.Visible = component.HasShield;

		ShieldValue.Visible = component.HasShieldRemaining;
		HPDurValue.Visible = !ShieldValue.Visible;
	}

	private void HealthBars()
	{
		HullBar.MaxValue = component.MaxHullPoints;
		DurabilityBar.MaxValue = component.MaxDurability;
		ShieldBar.MaxValue = component.MaxShield;

		HullBar.Value = component.CurrentHullPoints;
		DurabilityBar.Value = component.CurrentDurability;
		ShieldBar.Value = component.CurrentShield;

		HPDurValue.Text = $"{Mathf.Round(component.CurrentHullPoints)}hull | dur{Mathf.Round(component.CurrentDurability)}";
		ShieldValue.Text = $"{Mathf.Round(component.CurrentShield)}shld";

	}

	private void WeaponInHands()
	{
		ReloadBarProgress();

		WeaponName.Text = weapon.WeaponName;
		BulletCount.Text = weapon.ClipAmmoCurrent.ToString();
		BulletStoredCount.Text = weapon.AmmoStored.ToString();
		ReloadSign.Visible = Mathf.IsZeroApprox(weapon.ClipAmmoCurrent) & weapon.AmmoStored > 0;
		EmptySign.Visible = Mathf.IsZeroApprox(weapon.ClipAmmoCurrent) & Mathf.IsZeroApprox(weapon.AmmoStored);

		DebugRecoil.Text = $"{Mathf.RadToDeg(weapon.CalculatedDeviation):N2}\t|\t{weapon.CalculatedDeviation:N2}";

		foreach (ColorRect rect in ClipSize.GetChildren().Cast<ColorRect>())
		{
			rect.Color = Colors.Black;
		}

		if (ClipSize.GetChildCount() > 0)
			for (int i = 0; i < weapon.ClipAmmoCurrent; i++)
			{
				ClipSize.GetChild<ColorRect>(i).Color = Colors.White;
			}
	}

	private void ReloadBarProgress()
	{
		ReloadBar.Visible = !weapon.ReloadTime.IsStopped() || !weapon.EjectTime.IsStopped();

		if (ReloadBar.Visible)
		{
			if (!weapon.EjectTime.IsStopped())
				ReloadBar.Value = weapon.EjectTime.TimeLeft / weapon.EjectTime.WaitTime;
			if (!weapon.ReloadTime.IsStopped())
			{
				ReloadBar.Value = 1 - (weapon.ReloadTime.TimeLeft / weapon.ReloadTime.WaitTime);
			}
		}
	}

	private void _on_PlayerSwitchWeapon(Weapon currentWeapon, Weapon previousWeapon, Character character)
	{
		if (character is Player)
		{
			foreach (Node child in ClipSize.GetChildren())
				ClipSize.RemoveChild(child);

			if (currentWeapon != null)	
			{
				ClipSize.Columns = Mathf.Clamp(currentWeapon.Stats.ClipSize / 3, 1, 20);
				for (int i = 0; i < currentWeapon.Stats.ClipSize; i++)
				{
					ColorRect rect = new()
					{ 
						Color = Colors.Black,
						SizeFlagsHorizontal = SizeFlags.ExpandFill,
						SizeFlagsVertical = SizeFlags.ExpandFill
					};
					ClipSize.AddChild(rect);
				}
			}
		}
	}
}
