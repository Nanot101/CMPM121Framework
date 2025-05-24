using UnityEngine;

public class NewWaveTrigger : IRelicTrigger
{
    private Relic relic;

    public void Initialize(Relic relic)
    {
        this.relic = relic;
        EventBus.Instance.OnNewWave += HandleNewWave;
    }

    public void Uninitialize()
    {
        EventBus.Instance.OnNewWave -= HandleNewWave;
    }

    private void HandleNewWave()
    {
        Debug.Log("New wave; activating effect");
        relic.ActivateEffect();
    }
}
