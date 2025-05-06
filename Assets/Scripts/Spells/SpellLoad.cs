using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;

public class SpellLoader : MonoBehaviour
{
    public static Dictionary<string, SpellData> Spells { get; private set; } = new();
    public static Dictionary<string, ModifierData> Modifiers { get; private set; } = new();

    private void Awake()
    {
        LoadSpellsFromJson();
    }

    private void LoadSpellsFromJson()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("spells");
        if (jsonFile == null)
        {
            Debug.LogError("spells.json not found in Resources.");
            return;
        }

        Dictionary<string, Dictionary<string, object>> rawData =
            JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, object>>>(jsonFile.text);

        foreach (var kvp in rawData)
        {
            string key = kvp.Key;
            var value = kvp.Value;

            if (value.ContainsKey("damage") || value.ContainsKey("projectile"))
            {
                // It's a base spell
                string serialized = JsonConvert.SerializeObject(value);
                SpellData spell = JsonConvert.DeserializeObject<SpellData>(serialized);
                Spells[key] = spell;
            }
            else
            {
                // It's a modifier
                string serialized = JsonConvert.SerializeObject(value);
                ModifierData modifier = JsonConvert.DeserializeObject<ModifierData>(serialized);
                Modifiers[key] = modifier;
            }
        }

        Debug.Log($"Loaded {Spells.Count} base spells and {Modifiers.Count} modifiers.");
    }
}
