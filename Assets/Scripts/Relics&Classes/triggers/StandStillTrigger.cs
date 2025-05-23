using UnityEngine;

public class StandStillTrigger : IRelicTrigger
{
    private Relic relic;
    private float requiredTime;
    private float stillTime = 0f;
    private bool wasMoving = true;
    public StandStillTrigger(string amount)
    {
        float.TryParse(amount, out requiredTime);
    }

    public void Initialize(Relic relic)
    {
        this.relic = relic;
        // EventBus.Instance.OnMove += OnMove;
        // EventBus.Instance.OnStandStill += OnStandStill;
    }

    private void HandleStandStill(Vector3 pos)
    {
        relic.ActivateEffect();
    }

    public void UpdateTime(float dt)
    {
        if (wasMoving)
        {
            stillTime = 0f;
            wasMoving = false;
        }

        stillTime += dt;
        // Debug.Log($"[StandStillTrigger] stillTime = {stillTime}");

        if (stillTime >= requiredTime)
        {
            // Debug.Log("Activating effect");
            relic.ActivateEffect();
        }
    }

    public void Reset()
    {
        stillTime = 0f;
        wasMoving = true;
    }

}
