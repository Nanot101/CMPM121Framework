public static class RelicTriggerFactory
{
    public static IRelicTrigger Create(RelicDef.Trigger def)
    {
        return def.type switch
        {
            "take-damage" => new TakeDamageTrigger(),
            "on-kill" => new KillTrigger(),
            "stand-still" => new StandStillTrigger(def.amount),
            "new-wave" => new NewWaveTrigger(),
            _ => null,
        };
    }
}
