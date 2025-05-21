using System.Collections.Generic;
using UnityEngine;

public class GainManaEffect : IRelicEffect
{
    private readonly string amount;
    private readonly RpnEvaluator evaluator = new RpnEvaluator();

    public GainManaEffect(string amount)
    {
        this.amount = amount;
    }

    public void Apply()
    {
        int mana = evaluator.SafeEvaluateInt(amount, new Dictionary<string, float>
        {
            { "wave", GameManager.Instance.CurrentWave }
        }, fallback: 0);

        var playerGO = GameManager.Instance.player;
        if (playerGO != null)
        {
            var playerController = playerGO.GetComponent<PlayerController>();
            if (playerController != null && playerController.spellcaster != null)
            {
                playerController.spellcaster.AddMana(mana);
            }
            else
            {
                Debug.LogWarning("PlayerController or SpellCaster not found on player GameObject.");
            }
        }
        else
        {
            Debug.LogWarning("GameManager.Instance.player is null - cannot add mana.");
        }
    }

    public void Cleanup()
    {
        // no cleanup needed here
    }
}
