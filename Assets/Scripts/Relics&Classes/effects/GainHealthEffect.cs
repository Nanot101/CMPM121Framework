using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GainHealthEffect : IRelicEffect
{
    private string amount;
    private string until;
    private bool applied;
    private Coroutine healCoroutine;
    private readonly RpnEvaluator evaluator = new RpnEvaluator();

    public GainHealthEffect(string amountStr, string until)
    {
        this.amount = amountStr;
        this.until = until;
    }

    public void Apply()
    {
        if (applied) return;

        if (CoroutineMonoHelper.Instance == null)
        {
            Debug.LogError("[GainHealthEffect] CoroutineMonoHelper.Instance is null.");
            return;
        }

        healCoroutine = CoroutineMonoHelper.Instance.StartCoroutine(HealLoop());
        if (until == "move")
        {
            EventBus.Instance.OnMove += Cleanup;
        }

        applied = true;
    }

    private IEnumerator HealLoop()
    {
        var playerGO = GameManager.Instance.player;
        if (playerGO == null)
        {
            Debug.LogWarning("[GainHealthEffect] Player GameObject is null.");
            yield break;
        }

        var playerController = playerGO.GetComponent<PlayerController>();
        if (playerController == null)
        {
            Debug.LogWarning("[GainHealthEffect] PlayerController component missing.");
            yield break;
        }

        while (true)
        {
            int healAmount = evaluator.SafeEvaluateInt(amount, new Dictionary<string, float>
            {
                { "wave", GameManager.Instance.CurrentWave }
            }, fallback: 0);

            playerController.Heal(healAmount);
            yield return new WaitForSeconds(3f);
        }
    }

    public void Cleanup()
    {
        if (!applied) return;

        if (healCoroutine != null && CoroutineMonoHelper.Instance != null)
        {
            CoroutineMonoHelper.Instance.StopCoroutine(healCoroutine);
            healCoroutine = null;
        }

        if (until == "move")
        {
            EventBus.Instance.OnMove -= Cleanup;
        }

        applied = false;
    }
}
