using System.Collections;
using UnityEngine;

// Wraps and modifies another spell, uses bundle
public class ModifierSpell : Spell
{
    // The base spell being wrapped and modified
    protected Spell innerSpell;

    // Modifier properties
    float damageMultiplier;
    int damageAdder;
    float manaMultiplier;
    int manaAdder;
    float speedMultiplier;
    int speedAdder;
    int angle;
    string projectile_trajectory;
    float delay;
    string description;

    // Constructor that wraps a given spell and assigns the owning caster
    public ModifierSpell(Spell innerSpell, SpellCaster owner) : base(owner)
    {
        this.innerSpell = innerSpell;
        damageMultiplier = 1;
        damageAdder = 0;
        manaMultiplier = 1;
        manaAdder = 0;
        speedMultiplier = 1;
        speedAdder = 0;
        angle = 0;
        projectile_trajectory = string.Empty;
        delay = 0;
        description = string.Empty;
        icon = innerSpell.GetIcon();
    }

    public override string GetName()
    {
        return innerSpell.GetName() + " (Modified)";
    }

    public override string GetDescription()
    {
        return description != string.Empty ? description : innerSpell.GetDescription();
    }

    public override int GetIcon()
    {
        return icon != 0 ? icon : innerSpell.GetIcon();
    }

    public Spell GetSpellBase()
    {
        return innerSpell;
    }

    // Applying modifiers to the base spell
    public override int GetDamage()
    {
        int baseDamage = innerSpell.GetDamage();
        float modifiedDamage = (baseDamage + damageAdder) * damageMultiplier;
        return Mathf.RoundToInt(modifiedDamage);
    }

    public override int GetManaCost()
    {
        int baseMana = innerSpell.GetManaCost();
        float modifiedMana = (baseMana + manaAdder) * manaMultiplier;
        return Mathf.RoundToInt(modifiedMana);
    }

    public override float GetCooldown()
    {
        return innerSpell.GetCooldown();
    }

    

    public override float GetSpeed()
    {
        float baseSpeed = innerSpell.GetSpeed();
        float modifiedSpeed = (baseSpeed + speedAdder) * speedMultiplier;
        return Mathf.RoundToInt(modifiedSpeed);
    }

    // Applies additional modifiers to a spell, can be customized by subclasses
    public virtual void ApplyModifiers(Spell spell)
    {
        damageMultiplier = Mathf.Max(1, damageMultiplier);
        manaMultiplier = Mathf.Max(1, manaMultiplier);
        speedMultiplier = Mathf.Max(1, speedMultiplier);
    }

    public override IEnumerator Cast(Vector3 where, Vector3 target, Hittable.Team team)
    {
        this.team = team;
        // Apply before casting
        ApplyModifiers(innerSpell);
        // Adjust trajectory or angle if specified
        Vector3 direction = (target - where).normalized;
        // Modify the angle if applicable
        if (angle != 0)
        {
            float radians = angle * Mathf.Deg2Rad;
            direction = new Vector3(
                direction.x * Mathf.Cos(radians) - direction.z * Mathf.Sin(radians),
                direction.y,
                direction.x * Mathf.Sin(radians) + direction.z * Mathf.Cos(radians)
            );
        }
        yield return innerSpell.Cast(where, where + direction * GetSpeed(), team);

        if (delay > 0)
        {
            yield return new WaitForSeconds(delay);
        }
    }

    protected override void InitializeSpellData()
    {
        throw new System.NotImplementedException();
    }
}
