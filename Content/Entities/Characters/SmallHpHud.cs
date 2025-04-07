using System;
using Godot;

public partial class SmallHpHud : Panel
{ 
    private Character Holder => Owner as Character;

    private ProgressBar HullBar => GetNode<ProgressBar>("HullBar");
    private ProgressBar DurabilityBar => GetNode<ProgressBar>("DurabilityBar");
    private ProgressBar ShieldBar => GetNode<ProgressBar>("ShieldBar");

    public override void _PhysicsProcess(double delta)
    {
        GlobalPosition = Holder.GlobalPosition + new Vector2(-30, -50);

        SetVisibles();
        SetMaxes();
        SetCurrent();
        FancieExpose();
    }

    private void SetVisibles()
    {
        DurabilityBar.Visible = Holder.healthComponent.HasDurability;
        ShieldBar.Visible = Holder.healthComponent.HasShield;
    }

    void SetMaxes()
    {
        HullBar.MaxValue = Holder.healthComponent.MaxHullPoints;
        DurabilityBar.MaxValue = Holder.healthComponent.MaxDurability;
        ShieldBar.MaxValue = Holder.healthComponent.MaxShield;
    }

    private void SetCurrent()
    {
        HullBar.Value = Holder.healthComponent.CurrentHullPoints;
        DurabilityBar.Value = Holder.healthComponent.CurrentDurability;
        ShieldBar.Value = Holder.healthComponent.CurrentShield;
    }

    private void FancieExpose()
    {
        StyleBoxFlat hullFill = HullBar.GetThemeStylebox("fill") as StyleBoxFlat;//.Duplicate() as StyleBoxFlat;
		StyleBoxFlat durFill = DurabilityBar.GetThemeStylebox("fill") as StyleBoxFlat;//.Duplicate() as StyleBoxFlat;

        if (Holder.healthComponent.IsExposed)
        {
            hullFill.BgColor = new Color(.92f, .12f, 0);
            durFill.BorderColor = new Color(.75f, .65f, .18f, .68f);
        }
        else
        {
            hullFill.BgColor = Colors.DarkRed;
			durFill.BorderColor = new Color(1f, .96f, .34f, .67f);			
        }

        // HullBar.AddThemeStyleboxOverride("fill", hullFill);
		// DurabilityBar.AddThemeStyleboxOverride("fill", durFill);
    }

}
