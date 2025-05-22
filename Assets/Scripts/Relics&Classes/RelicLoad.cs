using UnityEngine;
using System.Collections.Generic;
using Newtonsoft.Json;
using System;

public class RelicLoad : MonoBehaviour
{
    public static List<RelicDef> relics;

    private void Awake()
    {
        LoadRelics();
    }
    private static void LoadRelics()
    {
        TextAsset json = Resources.Load<TextAsset>("relics");
        if (json == null)
        {
            Debug.LogError("Failed to load relics.json from Resources.");
            return;
        }

        try
        {
            relics = JsonConvert.DeserializeObject<List<RelicDef>>(json.text);

            if (relics == null || relics.Count == 0)
            {
                Debug.LogWarning("relics.json loaded but no relics found.");
            }
            else
            {
                Debug.Log("------------------");
                Debug.Log($"Loaded {relics.Count} relics:");
                foreach (var relic in relics)
                {
                    Debug.Log($"Relic: {relic.name} | Relic sprite: {relic.sprite}");
                    Debug.Log($"Trigger type: {relic.trigger?.type} | Trigger description: {relic.trigger?.description} | Trigger amount: {relic.trigger?.amount} ");
                    Debug.Log($"Effect type: {relic.effect?.type} | Effect description: {relic.effect?.description} | Effect amount: {relic.effect?.amount} | Effect until: {relic.effect?.until} ");
                }
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Error parsing relics.json:\n" + ex.Message);
        }
    }
}
