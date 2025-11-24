using System.Collections.Generic;
using System.Linq;
using Godot;

public partial class ModifiableStat() : Resource
{
    // public delegate void ModifierFunction();

    public double Value { get; set; }
    public double CalculatedValue { get; private set; }

    LinkedList<Modifier> Modifiers;

    public double CalculateModifiers()
    {
        IOrderedEnumerable<Modifier> ordered = Modifiers.OrderByDescending(p => p.Priority); // OrderBy(p => p.Priority, );
        CalculatedValue = Value;

        double addMod = 0;
        double multMod = 1;
        double finalAddMod = 1;
        double finalMultMod = 1;
        
        for (int i = 0; i < ordered.Count(); i++)
        {
            Modifier modifier = ordered.ElementAt(i);
            
            switch (modifier.Operation)
            {
                case Modifier.Operations.Set:
                    return modifier.Value;
                case Modifier.Operations.Add:
                    addMod += modifier.Value;
                    break;
                case Modifier.Operations.AddFinal:
                    finalAddMod += modifier.Value;
                    break;
                case Modifier.Operations.Multiply:
                    multMod += modifier.Value;
                    break;
                case Modifier.Operations.MultiplyFinal:
                    finalMultMod += modifier.Value;
                    break;
            }
        }

        CalculatedValue = ((Value + addMod) + (Value * multMod) + finalAddMod) * finalMultMod;
        

        return CalculatedValue;
    }









    public static ModifiableStat operator +(ModifiableStat modStat, Modifier modifier)
    {
        modStat.Modifiers.AddLast(modifier);
        return modStat;
    }
}