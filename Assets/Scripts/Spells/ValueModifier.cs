using UnityEngine;
using System.Collections;
using System.Collections.Generic;


// Holds individual modifications using additive and multiplicative mods. Used to scale
public class ValueModifier
{
    public enum Type { Add, Multiply }

    public Type ModifierType;
    public float Value;

    public ValueModifier(Type type, float value)
    {
        ModifierType = type;
        Value = value;
    }


    
    public static float ApplyModifiers(float baseValue, List<ValueModifier> modifiers)
    {
        float result = baseValue;

        // Apply additive first
        foreach (var mod in modifiers)
        {
            if (mod.ModifierType == Type.Add)
            {
                result += mod.Value;
            }
        }

        // Apply multiplicative second
        foreach (var mod in modifiers)
        {
            if (mod.ModifierType == Type.Multiply)
            {
                result *= mod.Value;
            }
        }

        return result;
    }
}
