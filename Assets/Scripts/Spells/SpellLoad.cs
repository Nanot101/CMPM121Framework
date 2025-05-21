using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using Unity.Collections;
using Newtonsoft.Json.Linq;

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


    var rawData = JsonConvert.DeserializeObject<Dictionary<string, JObject>>(jsonFile.text);

        foreach (var kvp in rawData)
        {
            string key = kvp.Key;
            JObject value = kvp.Value;

            if (value.ContainsKey("damage") || value.ContainsKey("projectile"))
            {
                // It's a base spell
                SpellData spell = value.ToObject<SpellData>();
                if (spell.damage == null)
                {
                    Debug.LogError($"Failed to load damage for spell: {spell.name}");
                }
                else
                {
                    Debug.Log($"Loaded spell: {spell.name} with damage: {spell.damage.amount} and type: {spell.damage.type}");
                }
                Spells[key] = spell;
            }
            else
            {
                // It's a modifier
                ModifierData modifier = value.ToObject<ModifierData>();
                Modifiers[key] = modifier;
            }
        }
        Debug.Log("-------------");
        Debug.Log($"Loaded {Spells.Count} base spells and {Modifiers.Count} modifiers.");
    }


    public Dictionary<string, SpellData> getSpellDict()
    {
        return Spells;
    }
    public Dictionary<string, ModifierData>  getModifierDict() { 
        return Modifiers; 
    }

}
