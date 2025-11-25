using Godot;

public partial class BuffObject : MapObject
{
    [Export]
    private PackedScene buff;

    protected override void OnInteractAreaEnter(Node2D body)
    {
        if (body is Character character)
        {
            character.AddBuff(buff.InstantiateOrNull<Buff>());
            character.healthComponent.Heal(50, 0, 0);
        }

        QueueFree();
    }

}
