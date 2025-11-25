using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class BuffsContainer : Node
{
    void OnChildEnteredTree(Node node)
    {
        Buff buff = node as Buff;
        var arr = GetChildren();
        if (buff != null && buff.Holder != null && buff.Modifiers.Count > 0)
        {
            buff.Owner = this;
            buff.AddChild(buff.DurationTimer);
            buff.DurationTimer.Owner = buff;

            foreach (Buff existingBuff in GetChildren().Cast<Buff>())
            {
                if (existingBuff.Title == buff.Title && existingBuff != buff)
                {
                    if (buff.Duration > existingBuff.TimeLeft && !existingBuff.DurationTimer.IsStopped())
                    {
                        existingBuff.DurationTimer.Start();
                        buff.QueueFree();
                        return;
                    }
                    else
                    {
                        existingBuff.QueueFree();
                    }
                    return;
                }
            }
            if (!buff.Infinite)
            {
                buff.DurationTimer.Start();
            }

            buff.BuffEnded += OnBuffEnd; // (Пока сделал по другому)NOTE: Добавить таймер(наследованый класс), 
                                                // который возвращает родительский нод? (Или даже присоединённый Export'ом)

            foreach (Modifier modifier in buff.Modifiers)
            {
                ModifierPaths.AddModifier.To[modifier.TargetetStat](modifier, buff.Holder);
            }
        }
    }

    void OnBuffEnd(Node node)
    {
        Buff buff = node as Buff;
        if (buff != null)
        {
            GD.Print($"{DateTime.Now:HH:mm:ss:fff} [BuffsContainer] " + buff.Title);
            foreach (Modifier modifier in buff.Modifiers)
            {
                ModifierPaths.RemoveModifier.From[modifier.TargetetStat](modifier, buff.Holder);
            }
            node.QueueFree();
        }
    }


    #region Modifier Appliances

    class ModifierPaths
    {
        public static class AddModifier
        {
            public static Dictionary<Global.Stats, Action<Modifier, Character>> To = new()
            {
                { Global.Stats.MaxHull, MaxHull },
                { Global.Stats.MaxDurability, MaxDurability },
                { Global.Stats.MaxShield, MaxShield }
            };

            static void MaxHull(Modifier modifier, Character holder)
            {
                holder.healthComponent.AddHullModifier(modifier);
            }

            static void MaxDurability(Modifier modifier, Character holder)
            {
                holder.healthComponent.AddDurabilityModifier(modifier);
            }

            static void MaxShield(Modifier modifier, Character holder)
            {
                holder.healthComponent.AddShieldModifier(modifier);
            }
        }



        public static class RemoveModifier
        {
            public static Dictionary<Global.Stats, Action<Modifier, Character>> From = new()
            {
                { Global.Stats.MaxHull, MaxHull },
                { Global.Stats.MaxDurability, MaxDurability },
                { Global.Stats.MaxShield, MaxShield }
            };

            static void MaxHull(Modifier modifier, Character holder)
            {
                holder.healthComponent.RemoveHullModifier(modifier);
            }

            static void MaxDurability(Modifier modifier, Character holder)
            {
                holder.healthComponent.RemoveDurabilityModifier(modifier);
            }

            static void MaxShield(Modifier modifier, Character holder)
            {
                holder.healthComponent.RemoveShieldModifier(modifier);
            }
        }
    }
        #endregion
}
