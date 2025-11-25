using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

public partial class ModifiableStat(double value = 0, Func<double> modifierCalculationFunction = null) : Resource
{
    public double Value { get; set; } = value;
    private double _storedCalcValue = value;
    private double _calculatedValue;
    public double CalculatedValue { get => CalculationFunctionDelegate.Invoke(); }

    private Func<double> _calcFunc = modifierCalculationFunction;
    public Func<double> CalculationFunctionDelegate 
    { 
        get
        {
            if (_calcFunc == null)
                return CalculateModifiers;
            return _calcFunc;    
        } 
        set => _calcFunc = value; 
    }

    private bool modifiersHasChanged = true;
    // private int previousModifiersCount = -1;
    LinkedList<Modifier> Modifiers = new();

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

    double CalculateModifiers()
    {
        if (modifiersHasChanged)
        {
            modifiersHasChanged = false;
            // previousModifiersCount = Modifiers.Count;
            if (Modifiers.Count > 0)
            {
                IOrderedEnumerable<Modifier> ordered = Modifiers.OrderByDescending(p => p.Priority);

                double addMod = 0;
                double multMod = 0;
                double finalAddMod = 0;
                double finalMultMod = 0;
                
                for (int i = 0; i < ordered.Count(); i++)
                {
                    Modifier modifier = ordered.ElementAt(i);
                    
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

                double addAndMult = (Value + addMod) + (Value * multMod);
                _storedCalcValue = (addAndMult + finalAddMod) + (addAndMult * finalMultMod);
                return _storedCalcValue;
            }
            _storedCalcValue = Value;
            return _storedCalcValue;
        }
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