using UnityEngine;

using System.Collections.Generic;


// Stores all value modifiers grouped by type
// Used to dynamically modify attributes
public class SpellModBundle
{
    public List<ValueModifier> DamageModifiers = new();
    public List<ValueModifier> ManaCostModifiers = new();
    public List<ValueModifier> CooldownModifiers = new();
    public List<ValueModifier> SpeedModifiers = new();
    public List<ValueModifier> IconModifiers = new();
}
