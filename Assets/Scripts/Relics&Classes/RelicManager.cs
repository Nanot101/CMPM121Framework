using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
public class RelicManager : MonoBehaviour
{
    public static RelicManager Instance;
    [SerializeField] RelicUI relicUI1; 

    private List<Relic> activeRelics = new();

    private void Awake()
    {
        Instance = this;
    }
    public void AddRelic(RelicDef def)
    {
        if (activeRelics.Any(r => r.def.name == def.name))
        {
            Debug.Log("Relic already acquired: " + def.name);
            return;
        }

        var relic = new Relic(def);
        activeRelics.Add(relic);

        if (relic.GetTrigger() is StandStillTrigger standStillTrigger)
        {
            var player = FindAnyObjectByType<PlayerController>();
            if (player != null)
            {
                player.standStillTrigger = standStillTrigger;
            }
        }

    }
    public List<Relic> GetActiveRelics() => activeRelics;

    // private void Start()
    // {
    //     // Wait for RelicLoad to finish loading relics before this runs
    //     var testRelic = RelicLoad.GetRelicByName("Power Burst");
    //     if (testRelic != null)
    //     {
    //         AddRelic(testRelic);
    //         Debug.Log($"{testRelic.name} relic added to player for testing.");
    //     }
    //     else
    //     {
    //         Debug.LogWarning($"{testRelic.name} relic not found in loaded relics.");
    //     }
    // }

    public void Reset()
    {

        foreach (var relic in activeRelics)
        {
            relic.Uninitialize();
        }
        var player = FindAnyObjectByType<PlayerController>();
        if (player != null)
        {
            player.standStillTrigger = null;
            
        }
        activeRelics.Clear();
        Debug.Log("All relics cleared on game restart.");
        relicUI1.DebugLogCurrentRelics();
        
    }
}
