using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Linq;

public class RelicUI : MonoBehaviour
{
    public GameObject relicPanel;
    public GameObject relic1, relic2, relic3;
    public PlayerController player;

    private List<GameObject> relicSlots;

    void Awake()
    {
        relicSlots = new List<GameObject> { relic1, relic2, relic3 };
    }

    public void ShowRelicChoices()
    {
        Debug.Log("Showing relics");
        // Get relics player does NOT have
        var ownedNames = RelicManager.Instance.GetActiveRelics()
            .Select(r => r.def.name)
            .ToHashSet();
        var available = RelicLoad.relics
            .Where(r => !ownedNames.Contains(r.name))
            .OrderBy(x => Random.value)
            .Take(3)
            .ToList();

        // if no new relics, hide panel
        if (available.Count == 0)
        {
            relicPanel.SetActive(false);
            return;
        }

        relicPanel.SetActive(true);

        for (int i = 0; i < relicSlots.Count; i++)
        {
            var slot = relicSlots[i];

            if (i < available.Count)
            {
                // get each slot and their components and populate them
                var relicDef = available[i];
                var thisRelic = relicDef;
                Debug.Log($"Slot {i} = {slot.name}, Active: {slot.activeSelf}");
                slot.SetActive(true);

                var icon = slot.transform.Find("icon").GetComponent<Image>();
                var desc = slot.transform.Find("RelicDesc").GetComponent<TextMeshProUGUI>();
                var button = slot.transform.Find("Button").GetComponent<Button>();
                var highlight = slot.transform.Find("highlight").gameObject;
                Debug.Log("Available relics: " + string.Join(", ", available.Select(r => r.name)));

                GameManager.Instance.relicIconManager.PlaceSprite(thisRelic.sprite, icon);
                desc.text = thisRelic.name + ": " + thisRelic.trigger.description + ", " + thisRelic.effect.description;
                highlight.SetActive(false);

                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(() =>
                {
                    RelicManager.Instance.AddRelic(thisRelic);
                    DebugLogCurrentRelics();
                    relicPanel.SetActive(false);
                });
            }
            else
            {
                slot.SetActive(false);
            }
        }
    }

    public void DebugLogCurrentRelics()
    {
        var relics = RelicManager.Instance.GetActiveRelics();
        if (relics.Count == 0)
        {
            Debug.Log("Player currently has no relics.");
        }
        else
        {
            Debug.Log("Player's current relics:");
            foreach (var relic in relics)
            {
                Debug.Log($"- {relic.def.name}: {relic.def.trigger.description}, {relic.def.effect.description}");
            }
        }
    }
}
