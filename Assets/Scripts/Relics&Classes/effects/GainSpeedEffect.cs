using UnityEngine;
using System.Collections.Generic;

public class GainSpeedEffect : IRelicEffect
{
    private readonly string amountExpr;
    private PlayerController player;
    private int speedAmount;

    public GainSpeedEffect(string amount)
    {
        this.amountExpr = amount;
    }

    public void Apply()
    {
        player = GameManager.Instance.player.GetComponent<PlayerController>();
        if (player == null)
        {
            Debug.LogWarning("[GainSpeedEffect] No player found");
            return;
        }

        Dictionary<string, float> vars = new()
        {
            { "wave", GameManager.Instance.CurrentWave },
            { "base", player.Speed }
        };

        RpnEvaluator rpn = new RpnEvaluator();
        speedAmount = (int)rpn.EvaluateRPN(amountExpr, vars);

        player.Speed += speedAmount;
        Debug.Log($"[GainSpeedEffect] Applied speed boost: +{speedAmount}");
        player.StartCoroutine(RemoveAfterDelay(3f));

    }

    public void Cleanup()
    {
        if (player != null)
        {
            player.Speed -= speedAmount;
            Debug.Log($"[GainSpeedEffect] Removed speed boost: -{speedAmount}");
        }

        EventBus.Instance.OnMove -= RemoveOnMove;
        EventBus.Instance.OnSpellCast -= RemoveOnCast;
    }

    private void RemoveOnMove()
    {
        Cleanup();
    }

    private void RemoveOnCast()
    {
        Cleanup();
    }

    private System.Collections.IEnumerator RemoveAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Cleanup();
    }
}
