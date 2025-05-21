using UnityEngine;

public class StandStillTrigger : IRelicTrigger
{
    private Relic relic;
    private float requiredTime;
    private bool active = false;

    public StandStillTrigger(string amount)
    {
        float.TryParse(amount, out requiredTime);
    }

    public void Initialize(Relic relic)
    {
        this.relic = relic;
        EventBus.Instance.OnStandStill += HandleStandStill;
    }

    private void HandleStandStill(Vector3 pos)
    {
        relic.ActivateEffect();
    }
}
