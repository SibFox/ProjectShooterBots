using Godot;
using System.Linq;

public partial class BuffsContainer : Node
{
    public override void _PhysicsProcess(double delta)
    {
        foreach (Buff buff in GetChildren().Cast<Buff>())
        {
            if (buff.DurationTimer.IsStopped() && !buff.Infinite && !buff.IsQueuedForDeletion())
            {
                GD.Print($"{System.DateTime.Now:HH:mm:ss:fff} [BuffsContainer] " + buff.Title);
                buff.QueueFree();
            }
        }
    }

    void OnChildEnteredTree(Node node)
    {
        if (node is Buff buff)
        {
            foreach (Buff existingBuff in GetChildren().Cast<Buff>())
            {
                if (existingBuff.Name == buff.Name)
                {
                    if (buff.Duration > existingBuff.DurationTimer.WaitTime)
                        existingBuff.DurationTimer.Start(buff.Duration);
                    buff.QueueFree();
                    return;
                }
            }
            if (!buff.Infinite)
            {
                buff.DurationTimer.Start(buff.Duration);
                buff.DurationTimer.Timeout += A; // NOTE: Добавить таймер, который возвращает родительский нод? (Или даже присоединённый Export'ом)
            }
        }
    }

    void A()
    {}
}
