using UnityEngine;
public class TakeDamageTrigger : IRelicTrigger
{
    private Relic relic;

    public void Initialize(Relic relic)
    {
        this.relic = relic;
        EventBus.Instance.OnDamage += OnDamage;
    }
    public void Uninitialize()
    {
        EventBus.Instance.OnDamage -= OnDamage;
    }

    private void OnDamage(Vector3 pos, Damage dmg, Hittable target)
    {
        if (target.team == Hittable.Team.PLAYER)
        {
            relic.ActivateEffect();
        }
    }
}
