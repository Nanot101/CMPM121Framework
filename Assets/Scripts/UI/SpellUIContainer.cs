using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class SpellUIContainer : MonoBehaviour
{
    public GameObject[] spellUIs;
    public PlayerController player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // we only have one spell (right now)
        spellUIs[0].SetActive(true);
        for(int i = 1; i< spellUIs.Length; ++i)
        {
            spellUIs[i].SetActive(false);
        }
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
    // Update is called once per frame
    void Update()
    {
        
    }

}
