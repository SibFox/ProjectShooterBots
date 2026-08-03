using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

public partial class ModifiableStat(double value = 0,
                        ModifiableStat.CalculationFunctionDelegate modifierCalculationFunction = null) : Resource
{
    public delegate double CalculationFunctionDelegate(double value, ref double storedCalcValue, IOrderedEnumerable<Modifier> modifiers);

    private bool modifiersHasChanged = true;
    LinkedList<Modifier> Modifiers = new();

    public double Value { get; set; } = value;
    private double _storedCalcValue = value;
    public double CalculatedValue
    {
        get
        {
            if (modifiersHasChanged)
            {
                modifiersHasChanged = false;
                if (Modifiers.Count != 0)
                    return CalculationFunctionMethod.Invoke(Value, ref _storedCalcValue, Modifiers.OrderByDescending(p => p.Priority));
                _storedCalcValue = Value;
                return _storedCalcValue;
            }
            return _storedCalcValue;
        }
    }

    // private CalculationFunctionDelegate _calcFunc = modifierCalculationFunction;
    private CalculationFunctionDelegate CalculationFunctionMethod 
    { 
        get
        {
            if (modifierCalculationFunction == null)
                return CalculateModifiers;
            return modifierCalculationFunction;    
        }
    }

    public void AddModifier(Modifier modifier)
    {
        if (!Modifiers.Contains(modifier))
        {
            Modifiers.AddLast(modifier);
            modifiersHasChanged = true;
        }
    }

    public void RemoveModifier(Modifier modifier)
    {
        if (Modifiers.Remove(modifier))
        {
            modifiersHasChanged = true;
        }
    }

    double CalculateModifiers(double Value, ref double _storedCalcValue, IOrderedEnumerable<Modifier> Modifiers)
    {
        double addMod = 0;
        double multMod = 0;
        double finalAddMod = 0;
        double finalMultMod = 0;
        
        for (int i = 0; i < Modifiers.Count(); i++)
        {
            Modifier modifier = Modifiers.ElementAt(i);
            
            switch (modifier.Operation)
            {
                case Modifier.Operations.Set:
                    _storedCalcValue = Mathf.Snapped(modifier.Value, 0.00000);
                    return _storedCalcValue;
                case Modifier.Operations.Add:
                    addMod += Mathf.Snapped(modifier.Value, 0.00000);
                    break;
                case Modifier.Operations.AddFinal:
                    finalAddMod += Mathf.Snapped(modifier.Value, 0.00000);
                    break;
                case Modifier.Operations.Multiply:
                    multMod += Mathf.Snapped(modifier.Value, 0.00000);
                    break;
                case Modifier.Operations.MultiplyFinal:
                    finalMultMod += Mathf.Snapped(modifier.Value, 0.00000);
                    break;
            }
        }

        double finalValue = (Value + addMod) + (Value * multMod);
        _storedCalcValue = (finalValue + finalAddMod) + (finalValue * finalMultMod);
        return _storedCalcValue;        
    }

    #region Operator overides

    public static ModifiableStat operator +(ModifiableStat modStat, Modifier modifier)
    {
        if (!modStat.Modifiers.Contains(modifier))
        {
            modStat.Modifiers.AddLast(modifier);
            modStat.modifiersHasChanged = true;
        }
        return modStat;
    }

    public static ModifiableStat operator -(ModifiableStat modStat, Modifier modifier)
    {
        if (modStat.Modifiers.Remove(modifier))
        {
            modStat.modifiersHasChanged = true;
        }
        return modStat;
    }

    // Int
    public static bool operator >(ModifiableStat modStat, int val)
    {
        return modStat.Value > val;
    }

    public static bool operator <(ModifiableStat modStat, int val)
    {
        return modStat.Value < val;
    }

    public static bool operator >(int val, ModifiableStat modStat)
    {
        return val > modStat.Value;
    }

    public static bool operator <(int val, ModifiableStat modStat)
    {
        return val < modStat.Value;
    }

    // Double
    public static bool operator >(ModifiableStat modStat, double val)
    {
        return modStat.Value > val;
    }

    public static bool operator <(ModifiableStat modStat, double val)
    {
        return modStat.Value < val;
    }

    public static bool operator >(double val, ModifiableStat modStat)
    {
        return val > modStat.Value;
    }

    public static bool operator <(double val, ModifiableStat modStat)
    {
        return  val < modStat.Value;
    }

    #endregion
}