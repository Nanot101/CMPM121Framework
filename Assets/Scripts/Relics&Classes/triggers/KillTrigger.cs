public class KillTrigger : IRelicTrigger
{
    private Relic relic;

    public void Initialize(Relic relic)
    {
        this.relic = relic;
        EventBus.Instance.OnKill += relic.ActivateEffect;
    }
}
