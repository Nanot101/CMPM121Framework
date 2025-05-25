using System.Runtime.CompilerServices;

public interface IRelicTrigger
{
    void Initialize(Relic relic);
    void Uninitialize();
}

public interface IRelicEffect
{
    void Apply();
    void Cleanup();
}
