using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

[System.Serializable]
public class DamageData
{
    public string amount;
    public Damage.Type type;
}

[System.Serializable]
public class ProjectileData
{
    public string trajectory;
    public string speed;
    public int sprite;
    public string lifetime;
}

[System.Serializable]
public class SpellData
{
    public string name;
    public string description;
    public int icon;
    public string mana_cost;
    public string cooldown;
    public string N;
    public string spray;
    public DamageData damage;
    public string speed;
    public string trajectory = "straight";
    public float size = 0.7f;
    public int projectileSprite = 0;
    public string secondary_damage;
    public ProjectileData projectile;
    public ProjectileData secondary_projectile;


    // Modifiers for various properties
    public List<ValueModifier> damageModifiers = new List<ValueModifier>();
    public List<ValueModifier> manaCostModifiers = new List<ValueModifier>();
    public List<ValueModifier> cooldownModifiers = new List<ValueModifier>();
    public List<ValueModifier> speedModifiers = new List<ValueModifier>();
    public List<ValueModifier> sizeModifiers = new List<ValueModifier>();
    public List<ValueModifier> secondaryDamageModifiers = new List<ValueModifier>();
    public List<ValueModifier> numProjectilesModifiers = new List<ValueModifier>();

    // Variable context for RPN evaluation
    // private Dictionary<string, int> variableContext = new Dictionary<string, int>();

    // public void SetVariable(string key, int value)
    // {
    //     variableContext[key] = value;
    // }

    // private int Evaluate(string expression)
    // {
    //     return new RpnEvaluator().EvaluateRPN(expression, variableContext);
    // }

    // this is needed for arcane blast bc of numprojectiles
    public void SetContext(Dictionary<string, float> context)
    {
        foreach (var kvp in context)
        {
            variableContext[kvp.Key] = kvp.Value;
        }
    }
    private Dictionary<string, float> variableContext = new Dictionary<string, float>();

    // public void SetVariable(string key, int value)
    // {
    //     variableContext[key] = (float)value;
    // }

    // public void SetVariable(string key, float value)
    // {
    //     variableContext[key] = value;
    // }
    private int Evaluate(string expression)
    {
        if (string.IsNullOrEmpty(expression)){
            Debug.Log("In Evaluate: expression is null or empty, returning 1");
            return 1;
        }
        float result = new RpnEvaluator().EvaluateRPN(expression, variableContext);
        Debug.Log($"Expression: {expression} | Result: " + result);
        return Mathf.RoundToInt(result);
    }

    // private float EvaluateFloat(string expression)
    // {
    //     return new RpnEvaluator().EvaluateRPN(expression, variableContext);
    // }

    // public int GetFinalDamage(int power)
    // {
    //     if (damage == null || string.IsNullOrEmpty(damage.amount))
    //     {
    //         return 0;
    //     }
        
    //     int baseDamage = Evaluate(damage.amount) * power / 10;
    //     return ValueModifier.ApplyModifiers(baseDamage, damageModifiers);
    // }

    // public int GetFinalManaCost()
    // {
    //     int baseManaCost = Evaluate(mana_cost);
    //     return ValueModifier.ApplyModifiers(baseManaCost, manaCostModifiers);
    // }

    // public float GetFinalCooldown()
    // {
    //     float baseCooldown = Evaluate(cooldown);
    //     return ValueModifier.ApplyModifiers(baseCooldown, cooldownModifiers);
    // }

    //get this removed
    public float GetFinalSpeed()
    {
        float baseSpeed = Evaluate(speed);
        return ValueModifier.ApplyModifiers(baseSpeed, speedModifiers);
    }

    // public float GetFinalSize()
    // {
    //     return ValueModifier.ApplyModifiers(size, sizeModifiers);
    // }

    public int GetFinalSecondaryDamage()
    {
        if (string.IsNullOrEmpty(secondary_damage)) return 0;
        
        int baseSecondaryDamage = Evaluate(secondary_damage);
        return ValueModifier.ApplyModifiers(baseSecondaryDamage, secondaryDamageModifiers);
    }

    public int GetFinalNumProjectiles()
    {
        if (string.IsNullOrEmpty(N)) return 1;
        
        int baseNum = Evaluate(N);
        return ValueModifier.ApplyModifiers(baseNum, numProjectilesModifiers);
    }
    
    public Damage.Type GetDamageType()
    {
        // Default to physical
        return damage != null ? damage.type : Damage.Type.PHYSICAL;
    }

    public float getBaseSpeed()
    {
        float baseSpeed = Evaluate(speed);
        return baseSpeed;
    }

    public static implicit operator SpellData(Spell v)
    {
        throw new NotImplementedException();
    }
    public int GetBaseDamage()
    {
        if (damage == null || string.IsNullOrEmpty(damage.amount))
        {
            return 0;
        }

        int baseDamage = Evaluate(damage.amount);
        return baseDamage;
    }

    public int GetBaseManaCost()
    {
        int baseManaCost = Evaluate(mana_cost);
        return baseManaCost;
    }

    public float GetBaseCooldown()
    {
        float baseCooldown = Evaluate(cooldown);
        return baseCooldown;
    }

    public float GetBaseSpeed()
    {
        float baseSpeed = Evaluate(speed);
        return baseSpeed;
    }
    public void addToDamage(int damageAdded)
    {
        damage.amount += damageAdded + " + ";
    }
    public void addToMana(int manaAdded)
    {
        mana_cost += manaAdded + " + ";
    }
    public void addToSpeed(float speedAdded)
    {
        speed += speedAdded + " + ";
    }
    public void addToCooldown(float cooldownAdded)
    {
        cooldown += cooldownAdded + " + ";
        Debug.Log($"In spellDef->addToCooldown: cooldown is {cooldown}");
    }

}
    

[System.Serializable]
public class ModifierData
{
    public string name;
    public string description;
    public string damage_multiplier;
    public string mana_multiplier;
    public string speed_multiplier;
    public string cooldown_multiplier;
    public string mana_adder;
    public string delay;
    public string angle;
    public string projectile_trajectory;
}