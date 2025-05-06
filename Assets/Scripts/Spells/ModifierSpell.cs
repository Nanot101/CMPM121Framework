using System.Collections;
using UnityEngine;


// Wraps and modifies another spell, uses bundle
public class ModifierSpell : SpellBase
{
    // the base spell being wrapped and modified
    protected SpellBase innerSpell;
    float damageMultiplier;
    int damageAdder;
    float manaMultiplier;
    int manaAdder;
    float speedMultiplier;
    int speedAdder;
    int angle;
    string projectile_trajectory;
    float delay;
    // Constructor that wraps a given spell and assigns the owning caster
    public ModifierSpell(SpellBase innerSpell, SpellCaster owner) : base(owner)
    {
        this.innerSpell = innerSpell;
        this.damageMultiplier = 1;
        this.damageAdder = 0;
        this.manaMultiplier = 1;
        this.manaAdder = 0;
        this.speedMultiplier = 1;
        this.speedAdder = 0;
        this.angle = 0;
        this.projectile_trajectory = string.Empty;
        this.delay = 0;
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
    public SpellBase GetSpellBase()
    {
        return innerSpell;
    }
    // Each subclass will add modifiers here
    public virtual void ApplyModifiers(SpellBase spell)
    {
        float damage = spell.GetDamage() + damageAdder;
        damage *= damageMultiplier;
        //set damage 
        float manaCost = spell.GetManaCost() + manaAdder;
        manaCost *= manaMultiplier;
        manaCost = (int)manaCost;
        //set mana cost
        float speed = spell.getSpeed() + speedAdder;// get speed needs to be implemented
        speed *= speedMultiplier;
        //set speed
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
