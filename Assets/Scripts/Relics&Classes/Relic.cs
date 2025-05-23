using UnityEngine;
public class Relic
{
    public RelicDef def;
    private IRelicTrigger trigger;
    private IRelicEffect effect;

    public Relic(RelicDef def)
    {
        this.def = def;
        effect = RelicEffectFactory.Create(def.effect);
        trigger = RelicTriggerFactory.Create(def.trigger);

        trigger?.Initialize(this);
    }

    public void ActivateEffect()
    {
        Debug.Log($"[Relic] Activating effect for {def.name}");
        effect?.Apply();
    }

    public void ClearEffect()
    {
        effect?.Cleanup();
    }

    public IRelicTrigger GetTrigger()
    {
        return trigger;
    }


    public string GetName() => def.name;
    public int GetSpriteIndex() => def.sprite;
}
