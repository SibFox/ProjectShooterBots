using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class BuffsContainer : Node
{
    [Export(PropertyHint.NodeType, "Node2D")]
    Character Holder;

    void OnChildEnteredTree(Node node)
    {
        Buff buff = node as Buff;
        if (buff != null && buff.Holder != null && buff.Modifiers.Count > 0)
        {
            buff.Owner = this;
            buff.AddChild(buff.DurationTimer);
            buff.DurationTimer.Owner = buff;

            foreach (Buff existingBuff in GetChildren().Cast<Buff>())
            {
                if (existingBuff.Title == buff.Title && existingBuff != buff)
                {
                    existingBuff.Reapply();
                    if (buff.Duration > existingBuff.TimeLeft && !existingBuff.DurationTimer.IsStopped())
                    {
                        existingBuff.DurationTimer.Start(buff.Duration);
                        buff.QueueFree();
                        return;
                    }
                    // else if (existingBuff.Infinite)
                    // {
                    //     buff.QueueFree();
                    // }
                    buff.QueueFree();
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
                ModifierPaths.AddModifier(modifier.TargetetStat, modifier, Holder);
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
                ModifierPaths.RemoveModifier(modifier.TargetetStat, modifier, Holder);
            }
            node.QueueFree();
        }
    }


    #region Modifier Appliances

    static class ModifierPaths
    {
        #region Paths Dictionary
        public enum Action
        {
            Add,
            Remove
        }

        public static Dictionary<Global.CharacterStats, Action<Modifier, Character, Action>> CharacterStats = new()
        {
            // ~~~~~~ Max Hull/Dur/Shield ~~~~~~
            { Global.CharacterStats.MaxHull, MaxHull },
            { Global.CharacterStats.MaxDurability, MaxDurability},
            { Global.CharacterStats.MaxShield, MaxShield },

            // ~~~~~~ Hull Recovery ~~~~~~
            { Global.CharacterStats.HullRecoveryFlat, HullRecoveryFlat },
            { Global.CharacterStats.HullRecoveryPercent, HullRecoveryPercent },
            { Global.CharacterStats.HullRecoveryCD, HullRecoveryCD },

            // ~~~~~~ Durability Recovery ~~~~~~
            { Global.CharacterStats.DurabilityRecoveryFlat, DurabilityRecoveryFlat },
            { Global.CharacterStats.DurabilityRecoveryPercent, DurabilityRecoveryPercent },
            { Global.CharacterStats.DurabilityRecoveryCD, DurabilityRecoveryCD },

            // ~~~~~~ Shield Recovery ~~~~~~
            { Global.CharacterStats.ShieldRecoveryFlat, ShieldRecoveryFlat },
            { Global.CharacterStats.ShieldRecoveryPercent, ShieldRecoveryPercent },
            { Global.CharacterStats.ShieldRecoveryCD, ShieldRecoveryCD },
        };

        public static Dictionary<Global.WeaponStats, Action<Modifier, Character, Action>> WeaponStats = new()
        {

        };

        static public void AddModifier(Global.CharacterStats targetetStat, Modifier modifier, Character holder)
        {
            CharacterStats[targetetStat](modifier, holder, Action.Add);
        }

        static public void RemoveModifier(Global.CharacterStats targetetStat, Modifier modifier, Character holder)
        {
            CharacterStats[targetetStat](modifier, holder, Action.Remove);
        }

        static public void AddModifier(Global.WeaponStats targetetStat, Modifier modifier, Character holder)
        {
            WeaponStats[targetetStat](modifier, holder, Action.Add);
        }

        static public void RemoveModifier(Global.WeaponStats targetetStat, Modifier modifier, Character holder)
        {
            WeaponStats[targetetStat](modifier, holder, Action.Remove);
        }
        #endregion

        #region Max Hull/Dur/Shield
        // ~~~~~~ Max Hull/Dur/Shield ~~~~~~
        static void MaxHull(Modifier modifier, Character holder, Action action)
        {
            switch (action)
            {
                case Action.Add:
                    holder?.healthComponent?.AddHullModifier(modifier);
                    return;
                case Action.Remove:
                    holder?.healthComponent?.RemoveHullModifier(modifier);
                    return;
            }
        }

        static void MaxDurability(Modifier modifier, Character holder, Action action)
        {
            switch (action)
            {
                case Action.Add:
                    holder?.healthComponent?.AddDurabilityModifier(modifier);
                    return;
                case Action.Remove:
                    holder?.healthComponent?.RemoveDurabilityModifier(modifier);
                    return;
            }
        }

        static void MaxShield(Modifier modifier, Character holder, Action action)
        {
            switch (action)
            {
                case Action.Add:
                    holder?.healthComponent?.AddShieldModifier(modifier);
                    return;
                case Action.Remove:
                    holder?.healthComponent?.RemoveShieldModifier(modifier);
                    return;
            }
        }
        #endregion

        #region Character Recovery
        #region Hull Recovery
        // ~~~~~~ Hull Recovery ~~~~~~
        static void HullRecoveryFlat(Modifier modifier, Character holder, Action action)
        {
            switch (action)
            {
                case Action.Add:
                    holder?.CharacterStats?.AddHullRecoveryFlatModifier(modifier);
                    return;
                case Action.Remove:
                    holder?.CharacterStats?.RemoveHullRecoveryFlatModifier(modifier);
                    return;
            }
        }

        static void HullRecoveryPercent(Modifier modifier, Character holder, Action action)
        {
            switch (action)
            {
                case Action.Add:
                    holder?.CharacterStats?.AddHullRecoveryPercentModifier(modifier);
                    return;
                case Action.Remove:
                    holder?.CharacterStats?.RemoveHullRecoveryPercentModifier(modifier);
                    return;
            }
        }

        static void HullRecoveryCD(Modifier modifier, Character holder, Action action)
        {
            switch (action)
            {
                case Action.Add:
                    holder?.CharacterStats?.AddHullRecoveryCDModifier(modifier);
                    return;
                case Action.Remove:
                    holder?.CharacterStats?.RemoveHullRecoveryCDModifier(modifier);
                    return;
            }
        }
        #endregion

        #region Durability Recovery
        // ~~~~~~ Durability Recovery ~~~~~~
        static void DurabilityRecoveryFlat(Modifier modifier, Character holder, Action action)
        {
            switch (action)
            {
                case Action.Add:
                    holder?.CharacterStats?.AddDurabilityRecoveryFlatModifier(modifier);
                    return;
                case Action.Remove:
                    holder?.CharacterStats?.RemoveDurabilityRecoveryFlatModifier(modifier);
                    return;
            }
        }

        static void DurabilityRecoveryPercent(Modifier modifier, Character holder, Action action)
        {
            switch (action)
            {
                case Action.Add:
                    holder?.CharacterStats?.AddDurabilityRecoveryPercentModifier(modifier);
                    return;
                case Action.Remove:
                    holder?.CharacterStats?.RemoveDurabilityRecoveryPercentModifier(modifier);
                    return;
            }
        }

        static void DurabilityRecoveryCD(Modifier modifier, Character holder, Action action)
        {
            switch (action)
            {
                case Action.Add:
                    holder?.CharacterStats?.AddDurabilityRecoveryCDModifier(modifier);
                    return;
                case Action.Remove:
                    holder?.CharacterStats?.RemoveDurabilityRecoveryCDModifier(modifier);
                    return;
            }
        }
        #endregion

        #region Shield Recovery
        // ~~~~~~ Shield Recovery ~~~~~~
        static void ShieldRecoveryFlat(Modifier modifier, Character holder, Action action)
        {
            switch (action)
            {
                case Action.Add:
                    holder?.CharacterStats?.AddShieldRecoveryFlatModifier(modifier);
                    return;
                case Action.Remove:
                    holder?.CharacterStats?.RemoveShieldRecoveryFlatModifier(modifier);
                    return;
            }
        }

        static void ShieldRecoveryPercent(Modifier modifier, Character holder, Action action)
        {
            switch (action)
            {
                case Action.Add:
                    holder?.CharacterStats?.AddShieldRecoveryPercentModifier(modifier);
                    return;
                case Action.Remove:
                    holder?.CharacterStats?.RemoveShieldRecoveryPercentModifier(modifier);
                    return;
            }
        }

        static void ShieldRecoveryCD(Modifier modifier, Character holder, Action action)
        {
            switch (action)
            {
                case Action.Add:
                    holder?.CharacterStats?.AddShieldRecoveryCDModifier(modifier);
                    return;
                case Action.Remove:
                    holder?.CharacterStats?.RemoveShieldRecoveryCDModifier(modifier);
                    return;
            }
        }
        #endregion
        #endregion

        /*
            # Placeholder
        static void (Modifier modifier, Character holder, Action action)
        {
            switch (action)
            {
                case Action.Add:
                    
                    return;
                case Action.Remove:
                    
                    return;
            }
        }
        */
    }
    #endregion
}
