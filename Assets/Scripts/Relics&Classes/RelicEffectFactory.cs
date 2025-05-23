using UnityEngine;
public static class RelicEffectFactory
{
    public static IRelicEffect Create(RelicDef.Effect def, string relicName)
    {
        Debug.Log($"[RelicEffectFactory] Creating effect of type {def.type}");
        return def.type switch
        {
            "gain-mana" => new GainManaEffect(def.amount),
            "gain-spellpower" => new GainSpellpowerEffect(def.amount, def.until, relicName),
            _ => null,
        };
    }
}
