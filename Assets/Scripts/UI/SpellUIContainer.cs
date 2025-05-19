using UnityEngine;

public class SpellUIContainer : MonoBehaviour
{
    public SpellUI[] spellUIs;
    public PlayerController player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // we only have one spell (right now)
        // spellUIs[0].SetActive(true);


        //Problem is not here
        Debug.Log($"Setting spells. Spellui length: {spellUIs.Length}");
        Debug.Log($"Spellui 1: {spellUIs[1]}");
        for (int i = 0; i < spellUIs.Length; ++i)
        {
            spellUIs[i].SetActive(true);
            Debug.Log($"Spell {i}: {spellUIs}");
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
}
