using System.Collections;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.Audio;

// Wraps and modifies another spell, uses bundle
public class ModifierSpell : Spell
{
    // The base spell being wrapped and modified
    protected Spell innerSpell;

    // Modifier properties
    protected float damageMultiplier;
    int damageAdder;
    float manaMultiplier;
    int manaAdder;
    float speedMultiplier;
    int speedAdder;
    int angle;
    string projectile_trajectory;
    float cooldownMultiplier;
    float delay;

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
        cooldownMultiplier = 1;
        description = string.Empty;
        icon = innerSpell.GetIcon();
    }
    public ModifierSpell(Spell innerSpell, SpellCaster owner, float dmgMlt, int dmgAdd, float manaMlt, int manaAdd, float speedMlt, int speedAdd, int angl, string trajectory, float timeBetweenShots, float cooldownMlt, string descrp) : base(owner)
    {
        this.innerSpell = innerSpell;
        damageMultiplier = dmgMlt;
        damageAdder = dmgAdd;
        manaMultiplier = manaMlt;
        manaAdder = manaAdd;
        speedMultiplier = speedMlt;
        speedAdder = speedAdd;
        angle = angl;
        projectile_trajectory = trajectory;
        delay = timeBetweenShots;
        cooldownMultiplier = cooldownMlt;
        description = descrp;
        if (innerSpell != null)
        {
            icon = innerSpell.GetIcon();
        }
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
    public void setInnerSpell(Spell spell)
    {
        innerSpell = spell;
    }
    

    public override float GetSpeed()
    {
        float baseSpeed = innerSpell.GetSpeed();
        float modifiedSpeed = (baseSpeed + speedAdder) * speedMultiplier;
        return Mathf.RoundToInt(modifiedSpeed);
    }

    // Applies additional modifiers to a spell, can be customized by subclasses
    public virtual void ApplyModifiers()
    {
        //set damage 
        float damage = innerSpell.GetDamage() + damageAdder;
        damage *= damageMultiplier;
        int damageIncrease = (int)damage - innerSpell.GetDamage();
        innerSpell.addToDamage(damageIncrease);
        
        //set mana cost
        float manaCost = innerSpell.GetManaCost() + manaAdder;
        manaCost *= manaMultiplier;
        int manaIncrease = (int)manaCost - innerSpell.GetManaCost();
        innerSpell.addToMana(manaIncrease);

        //set speed
        float speed = innerSpell.getSpeed() + speedAdder;
        speed *= speedMultiplier;
        float speedIncrease = speed - innerSpell.GetSpeed();
        innerSpell.addToSpeed(speedIncrease);
        
        innerSpell.addToAngle(innerSpell.getAngle());
        float cooldown = GetCooldown();
        Debug.Log($"Base cooldown: {cooldown}");
        cooldown *= cooldownMultiplier;
        Debug.Log($"Cooldown with {cooldownMultiplier} multiplier: {cooldown}");
        float cooldownIncrease = cooldown - innerSpell.getCooldown();
        Debug.Log($"Trying to add cooldown: {cooldownIncrease}");
        innerSpell.addToCooldown(cooldownIncrease);
        // left blank, to be overwritten
        
    }

    public override IEnumerator Cast(Vector3 where, Vector3 target, Hittable.Team team)
    {
        Debug.Log("Modifier Spell Cast called");
        this.team = team;
        // Apply before casting
        ApplyModifiers();
        // Adjust trajectory or angle if specified
        Vector3 direction = (target - where).normalized;
        // Modify the angle if applicable
        int angle = innerSpell.getAngle();
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
            yield return innerSpell.Cast(where, where + direction * GetSpeed(), team);
        }
    }

    protected override void InitializeSpellData()
    {
        throw new System.NotImplementedException();
    }
}
