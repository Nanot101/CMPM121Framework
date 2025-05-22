using UnityEngine;
using System.Collections.Generic;
using Newtonsoft.Json;

public class ClassLoad : MonoBehaviour
{
    public static Dictionary<string, ClassDef> classes;
    private void Awake()
    {
        LoadClasses();
    }
    private static void LoadClasses()
    {
        TextAsset json = Resources.Load<TextAsset>("classes");
        if (json == null)
        {
            Debug.LogError("Failed to load classes.json from Resources.");
            return;
        }

        try
        {
            classes = JsonConvert.DeserializeObject<Dictionary<string, ClassDef>>(json.text);

            if (classes == null || classes.Count == 0)
            {
                Debug.LogWarning("classes.json loaded but no classes found.");
            }
            else
            {
                Debug.Log($"Loaded {classes.Count} classes:");
                foreach (var kvp in classes)
                {
                    var c = kvp.Value;
                    Debug.Log($"{kvp.Key}: Sprite {c.sprite}, Health: {c.health}, Mana: {c.mana}, Speed: {c.speed}, Mana-Regen {c.mana_regeneration}, Spellpower: {c.spellpower}");
                }
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Error parsing classes.json:\n" + ex.Message);
        }
    }

    public static ClassDef GetClass(string id)
    {
        if (classes == null)
        {
            Debug.LogError("ClassLoad.classes not initialized.");
            return null;
        }

        if (classes.TryGetValue(id, out ClassDef classDef))
        {
            return classDef;
        }

        Debug.LogWarning($"Class ID '{id}' not found.");
        return null;
    }

}
