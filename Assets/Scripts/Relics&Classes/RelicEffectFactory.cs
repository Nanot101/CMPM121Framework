using UnityEngine;
public static class RelicEffectFactory
{
    public static IRelicEffect Create(RelicDef.Effect def)
    {
        Debug.Log($"[RelicEffectFactory] Creating effect of type {def.type}");
        return def.type switch
        {
            "gain-mana" => new GainManaEffect(def.amount),
            "gain-spellpower" => new GainSpellpowerEffect(def.amount, def.until),
            _ => null,
        };
    }
}
