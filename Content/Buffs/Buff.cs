using Godot;
using Godot.Collections;

[GlobalClass]
public partial class Buff : Node
{
	[Signal]
	public delegate void BuffEndedEventHandler(Node buffNode);

	[ExportCategory("View")]
	[Export]
	public Texture2D Icon;
	[Export]
	public string Title { get; private set; }
	[Export(PropertyHint.MultilineText)]
	public string Description { get; private set; }

	[ExportCategory("Stat")]
	[Export(PropertyHint.Range, "0, 60, 0.01, or_greater")]
	public double Duration
	{
		get => DurationTimer.WaitTime; 
		private set
		{
			DurationTimer.WaitTime = value;
			Infinite = DurationTimer.WaitTime == 0;
		}
	}
	public double TimeLeft { get => DurationTimer.TimeLeft; }
	[Export]
	public bool Infinite { get; private set; }
	[Export(PropertyHint.ResourceType, "Modifier")]
	public Array<Modifier> Modifiers;
	
	private Timer _durTimer = new();
	public Timer DurationTimer { get => _durTimer; }

	private Character _holder;
	public Character Holder
	{
		get => _holder;
		set
		{
			if (value is not null)
			{
				_holder = value;
				DurationTimer.Timeout += OnDurationTimerTimeout;
			}
		}
	}
	
	public virtual void Reapply() {}

	public void SetSelfForModifiers()
	{
		foreach (Modifier modifier in Modifiers)
			modifier.CausedBy = this;
	}

	void OnDurationTimerTimeout()
	{
		GD.Print($"{System.DateTime.Now:HH:mm:ss:fff} [Buff] Expired");
		EmitSignalBuffEnded(this);
	}

	
}
