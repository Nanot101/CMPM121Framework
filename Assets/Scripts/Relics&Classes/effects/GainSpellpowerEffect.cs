using System.Collections.Generic;
using UnityEngine;

public class GainSpellpowerEffect : IRelicEffect
{
    private readonly string amount;
    private readonly string until;
    private bool applied = false;
    private readonly RpnEvaluator evaluator = new RpnEvaluator();

    public GainSpellpowerEffect(string amount, string until)
    {
        this.amount = amount;
        this.until = until;
    }

    public void Apply()
    {
        if (applied) return;

        int power = evaluator.SafeEvaluateInt(amount, new Dictionary<string, float>
        {
            { "wave", GameManager.Instance.CurrentWave }
        }, fallback: 0);

        var playerGO = GameManager.Instance.player;
        if (playerGO == null)
        {
            Debug.LogWarning("[GainSpellpowerEffect] Player GameObject is null.");
            return;
        }

        var playerController = playerGO.GetComponent<PlayerController>();
        if (playerController == null)
        {
            Debug.LogWarning("[GainSpellpowerEffect] PlayerController component missing on player GameObject.");
            return;
        }

        var spellCaster = playerController.spellcaster;
        if (spellCaster == null)
        {
            Debug.LogWarning("[GainSpellpowerEffect] SpellCaster not found on PlayerController.");
            return;
        }

        spellCaster.AddTemporarySpellpower(power);
        applied = true;
        Debug.Log($"[GainSpellpowerEffect] Applied temporary spellpower: {power}");

        if (until == "move")
            EventBus.Instance.OnMove += Cleanup;
        else if (until == "cast-spell")
            EventBus.Instance.OnSpellCast += Cleanup;
    }

    public void Cleanup()
    {
        if (!applied) return;

        var playerGO = GameManager.Instance.player;
        if (playerGO == null)
        {
            Debug.LogWarning("[GainSpellpowerEffect] Player GameObject is null during Cleanup.");
            return;
        }

        var playerController = playerGO.GetComponent<PlayerController>();
        if (playerController == null)
        {
            Debug.LogWarning("[GainSpellpowerEffect] PlayerController missing during Cleanup.");
            return;
        }

        var spellCaster = playerController.spellcaster;
        if (spellCaster == null)
        {
            Debug.LogWarning("[GainSpellpowerEffect] SpellCaster missing during Cleanup.");
            return;
        }

        spellCaster.RemoveTemporarySpellpower();
        applied = false;
        Debug.Log("[GainSpellpowerEffect] Removed temporary spellpower on Cleanup.");

        if (until == "move")
            EventBus.Instance.OnMove -= Cleanup;
        else if (until == "cast-spell")
            EventBus.Instance.OnSpellCast -= Cleanup;
    }
}
