using UnityEngine;

using System.Collections.Generic;


// Stores all value modifiers grouped by type
// Used to dynamically modify attributes
public class SpellModBundle
{
    public List<ModifierSpell> modifiers = new();
    //public List<ValueModifier> ManaCostModifiers = new();
    //public List<ValueModifier> CooldownModifiers = new();
    //public List<ValueModifier> SpeedModifiers = new();
    //public List<ValueModifier> IconModifiers = new();

    public ModifierSpell applyModifiers()
    {
        SpellBase spell = modifiers[0].GetSpellBase();
        foreach(ModifierSpell SpellMod in modifiers)
        {
            SpellMod.ApplyModifiers();
        }

    }
}
