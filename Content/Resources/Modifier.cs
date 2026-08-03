using Godot;

[GlobalClass]
public partial class Modifier() : Resource
{
    public enum Operations
    {
        Add,
        AddFinal,
        Multiply,
        MultiplyFinal,
        Set
    }
    
    [Export]
    public Global.CharacterStats TargetetStat;
    [Export]
    public double Value { get; set; }
    [Export]
    public Operations Operation;

    public string Name { get; set; }

    private int _priority;
    [Export]
    public int Priority { get => _priority; set => _priority = Mathf.Clamp(value, 0, 100); }

    public Buff CausedBy;
    
    
}