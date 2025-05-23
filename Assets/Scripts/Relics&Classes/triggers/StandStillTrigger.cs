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
        EventBus.Instance.OnStandStill += OnMove;
        EventBus.Instance.OnStandStill += OnStandStill;
    }

    private void HandleStandStill(Vector3 pos)
    {
        relic.ActivateEffect();
    }

    private void OnMove(Vector3 pos)
    {
        // Reset timer if player moved
        stillTime = 0f;
        wasMoving = true;
    }

    private void OnStandStill(Vector3 pos)
    {
        if (wasMoving)
        {
            stillTime = 0f; // start tracking only when we newly stop moving
            wasMoving = false;
        }

        stillTime += Time.deltaTime;
        Debug.Log($"[StandStillTrigger] stillTime = {stillTime}");
        if (stillTime >= requiredTime)
        {
            relic.ActivateEffect();
        }
    }
}
