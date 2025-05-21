using UnityEngine;

public class SpellUIContainer : MonoBehaviour
{
    public GameObject[] spellUIs;
    public PlayerController player;
    private SpellCaster spellCaster;

    void Start()
    {

        // Debug.Log($"Setting spells. Spellui length: {spellUIs.Length}");
        // Debug.Log($"Spellui 1: {spellUIs[1]}");
        for (int i = 0; i < spellUIs.Length; ++i)
        {
            spellUIs[i].SetActive(true);
            // Debug.Log($"Spell {i}: {spellUIs}");
        }

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void removeFirstSpell()
    {
        spellUIs[0].SetActive(false);
    }
    public void removeSecondSpell()
    {
        spellUIs[1].SetActive(false);
    }
    public void removeThirdSpell()
    {
        spellUIs[2].SetActive(false);
    }
    public void removeFourthSpell()
    {
        spellUIs[3].SetActive(false);
    }
    
    public void RemoveSpell(int index)
    {
        if (index >= 0 && index < spellUIs.Length)
        {
            SpellUI spellUI = spellUIs[index].GetComponent<SpellUI>();
            if (spellUI != null)
            {
                spellUI.SetActive(false, index); // calls SpellUI's SetActive
            }
            else
            {
                Debug.LogWarning($"No SpellUI component found on GameObject at index {index}");
            }
        }
        else
        {
            Debug.LogWarning($"Invalid spell index: {index}");
        }
    }
}
