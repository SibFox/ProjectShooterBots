using Godot;

public partial class HealthBuff : MapObject
{
    [Export]
    private PackedScene buff;

    protected override void OnInteractAreaEnter(Node2D body)
    {
        if (body is Character character)
        {
            character.AddBuff(buff.InstantiateOrNull<Buff>());
        }
    }

}
