public class KillTrigger : IRelicTrigger
{
    private Relic relic;

    public void Initialize(Relic relic)
    {
        this.relic = relic;
        EventBus.Instance.OnKill += OnKill;
    }
    public void Uninitialize()
    {
        EventBus.Instance.OnKill -= OnKill;
    }

    private void OnKill(Hittable killer, Hittable killed)
    {
        if (killer.team == Hittable.Team.PLAYER)
        {
            relic.ActivateEffect();
        }
    }
}
