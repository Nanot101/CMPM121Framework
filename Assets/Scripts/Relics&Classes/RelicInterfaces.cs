public interface IRelicTrigger
{
    void Initialize(Relic relic);
}

public interface IRelicEffect
{
    void Apply();
    void Cleanup();
}
