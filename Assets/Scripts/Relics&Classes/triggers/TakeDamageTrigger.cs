using UnityEngine;
public class TakeDamageTrigger : IRelicTrigger
{
    private Relic relic;

    public void Initialize(Relic relic)
    {
        this.relic = relic;
        EventBus.Instance.OnDamage += OnDamage;
    }

    private void OnDamage(Vector3 pos, Damage dmg, Hittable target)
    {
        relic.ActivateEffect();
    }
}
