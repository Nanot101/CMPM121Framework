using System.Collections;
using UnityEngine;

public class DamageAmpModifier : ModifierSpell
{
    private readonly ModifierData data;
    private Damage modifiedDamageData;
    public DamageAmpModifier(Spell innerSpell, SpellCaster owner) : base(innerSpell, owner)
    {
        data = SpellLoader.Modifiers["damage_amp"];

        if (!string.IsNullOrEmpty(data.mana_multiplier))
            manaMultiplier = float.TryParse(data.mana_multiplier, out float manaMult) ? manaMult : 1f;

        if (!string.IsNullOrEmpty(data.cooldown_multiplier))
            cooldownMultiplier = float.TryParse(data.cooldown_multiplier, out float cooldownMult) ? cooldownMult : 1f;

        if (!string.IsNullOrEmpty(data.damage_multiplier))
            damageMultiplier = float.TryParse(data.damage_multiplier, out float damageMult) ? damageMult : 1f;
        else
            damageMultiplier = 1f;

        Debug.Log($"[DamageAmpModifier] Initialized with Damage Multiplier: {damageMultiplier}, Mana Multiplier: {manaMultiplier}, Cooldown Multiplier: {cooldownMultiplier}");
    }

    protected override void InitializeSpellData()
    {
        // No re-initialization needed, handled in the constructor
    }

    public override float GetCooldown()
    {
        return innerSpell.GetCooldown() * cooldownMultiplier;
    }

    // public override void ApplyModifiers(Spell spell)
    // {
    //     attributes.damage.amount = GetDamage().ToString();
    // }

    public override int GetManaCost()
    {
        return Mathf.RoundToInt(innerSpell.GetManaCost() * manaMultiplier);
    }

    public override int GetDamage()
    {
        int baseDamage = innerSpell.GetDamage();
        int modifiedDamage = Mathf.RoundToInt(baseDamage * damageMultiplier);
        Damage.Type damageType = innerSpell.GetDamageType();
        Debug.Log($"[DamageAmpModifier] Base Damage: {baseDamage}, Multiplier: {damageMultiplier}, Modified Damage: {modifiedDamage}, Damage Type: {damageType}");

        
        modifiedDamageData = new Damage(modifiedDamage, damageType);

        

        return modifiedDamage;
    }

    public Damage GetDamageData()
    {
        if (modifiedDamageData == null)
        {
            GetDamage();
        }
        return modifiedDamageData;
    }

    public override IEnumerator Cast(Vector3 where, Vector3 target, Hittable.Team team)
{
    this.team = team;
    
    GetDamage(); 

    // Apply the damage override just before casting
    innerSpell.SetDamageOverride(modifiedDamageData);

    Debug.Log($"[DamageAmpModifier] Applying Modified Damage: {modifiedDamageData.amount} of type {modifiedDamageData.type}");

    // Cast the inner spell
    yield return innerSpell.Cast(where, target, team);
}

}
