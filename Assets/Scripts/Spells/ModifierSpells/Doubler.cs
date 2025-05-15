using System.Collections;
using UnityEngine;

public class DoublerModifier : ModifierSpell
{
    private readonly ModifierData data;

    public DoublerModifier(Spell innerSpell, SpellCaster owner) : base(innerSpell, owner)
    {
        data = SpellLoader.Modifiers["doubler"];
        
        SpellData innerAttributes = innerSpell.GetAttributes();

        if (!string.IsNullOrEmpty(data.mana_multiplier))
            manaMultiplier = innerAttributes.EvaluateFloat(data.mana_multiplier);

        if (!string.IsNullOrEmpty(data.cooldown_multiplier))
            cooldownMultiplier = innerAttributes.EvaluateFloat(data.cooldown_multiplier);

        if (!string.IsNullOrEmpty(data.delay))
            delay = innerAttributes.EvaluateFloat(data.delay);

        Debug.Log($"DoublerModifier initialized with Delay: {delay}, Mana Multiplier: {manaMultiplier}, Cooldown Multiplier: {cooldownMultiplier}");
    }

    protected override void InitializeSpellData()
    {
        // handled in the constructor, no need to re-initialize
    }

    public override float GetCooldown()
    {
        return innerSpell.GetCooldown() * cooldownMultiplier;
    }

    public override int GetManaCost()
    {
        return Mathf.RoundToInt(innerSpell.GetManaCost() * manaMultiplier);
    }

    public override IEnumerator Cast(Vector3 where, Vector3 target, Hittable.Team team)
    {
        this.team = team;
        ApplyModifiers(innerSpell);

        // First cast
        yield return innerSpell.Cast(where, target, team);
        // Wait
        if (delay > 0)
        {
            yield return new WaitForSeconds(delay);
        }
        // Second cast
        yield return innerSpell.Cast(where, target, team);
    }
}
