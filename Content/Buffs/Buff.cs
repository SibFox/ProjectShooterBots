using Godot;
using Godot.Collections;

[GlobalClass]
public partial class Buff : Node
{
	[ExportCategory("View")]
	[Export]
	public Texture2D Icon;
	[Export]
	public string Title { get; private set; }
	[Export(PropertyHint.MultilineText)]
	public string Description { get; private set; }

	[ExportCategory("Stat")]
	[Export(PropertyHint.Range, "0, 60, 0.01, or_greater")]
	public double Duration { get; private set; }
	[Export]
	public bool Infinite { get; private set; }
	[Export(PropertyHint.ResourceType, "Modifier")]
	public Array<Modifier> Modifiers;
	
	public Timer DurationTimer => GetNode<Timer>("%DurationTimer");

	private Character _holder;
	public Character Holder
    {
        get => _holder;
		set
        {
            if (value is not null)
				_holder = value;
        }
    }
}
