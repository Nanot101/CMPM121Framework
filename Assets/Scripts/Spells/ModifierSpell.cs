using System.Collections;
using UnityEngine;


// Wraps and modifies another spell, uses bundle
public class ModifierSpell : SpellBase
{
    // the base spell being wrapped and modified
    protected SpellBase innerSpell;

    // Constructor that wraps a given spell and assigns the owning caster
    public ModifierSpell(SpellBase innerSpell, SpellCaster owner) : base(owner)
    {
        this.innerSpell = innerSpell;
    }


    // Use this if owner is assigned later
    // public ModifierSpell(SpellBase innerSpell)
    // {
    //     this.innerSpell = innerSpell;
    // }


    public override string GetName()
    {
        return innerSpell.GetName();
    }

    // Each subclass will add modifiers here
    public virtual void ApplyModifiers(SpellModBundle bundle)
    {
        // left blank, to be overwritten
    }

    public override int GetDamage()
    {
        var bundle = new SpellModBundle();
        ApplyModifiers(bundle); // calls the overidden version
        return Mathf.RoundToInt(ValueModifier.ApplyModifiers(innerSpell.GetDamage(), bundle.DamageModifiers));
    }

    public override int GetManaCost()
    {
        var bundle = new SpellModBundle();
        ApplyModifiers(bundle);
        return Mathf.RoundToInt(ValueModifier.ApplyModifiers(innerSpell.GetManaCost(), bundle.ManaCostModifiers));
    }

    public override float GetCooldown()
    {
        var bundle = new SpellModBundle();
        ApplyModifiers(bundle);
        return ValueModifier.ApplyModifiers(innerSpell.GetCooldown(), bundle.CooldownModifiers);
    }

    public override int GetIcon()
    {
        var bundle = new SpellModBundle();
        ApplyModifiers(bundle);
        return Mathf.RoundToInt(ValueModifier.ApplyModifiers(innerSpell.GetIcon(), bundle.IconModifiers));
    }

    public override IEnumerator Cast(Vector3 where, Vector3 target, Hittable.Team team)
    {
        lastCastTime = Time.time;

        var bundle = new SpellModBundle();
        ApplyModifiers(bundle);

        // Optionally pass the bundle to inner spell if it needs it
        return innerSpell.Cast(where, target, team);
    }
}
