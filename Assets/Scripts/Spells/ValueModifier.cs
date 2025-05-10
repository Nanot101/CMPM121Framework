using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum Type { Add, Multiply }
// Holds individual modifications using additive and multiplicative mods. Used to scale
public class ValueModifier
{
    public Type ModifierType;
    public float Value;

    public ValueModifier(Type type, float value)
    {
        this.ModifierType = type;
        this.Value = value;
    }

    public float Apply(float baseValue)
    {
        switch (ModifierType)
        {
            case Type.Add:
                return baseValue + Value;
            case Type.Multiply:
                return baseValue * Value;
            default:
                return baseValue;
        }
    }
    
    public static float ApplyModifiers(float baseValue, List<ValueModifier> modifiers)
    {
        if (modifiers == null || modifiers.Count == 0)
        {
            return baseValue;
        }

        float result = baseValue;

        foreach (var mod in modifiers)
        {
            result = mod.Apply(result);
        }

        return result;
    }

    public static int ApplyModifiers(int baseValue, List<ValueModifier> modifiers)
    {
        float result = ApplyModifiers((float)baseValue, modifiers);
        return Mathf.RoundToInt(result);
    }
}
