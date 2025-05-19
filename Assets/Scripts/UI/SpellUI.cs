using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SpellUI : MonoBehaviour
{
    public GameObject icon;
    public RectTransform cooldown;
    public TextMeshProUGUI manacost;
    public TextMeshProUGUI damage;
    public GameObject highlight;
    // public AbstractSpell spell;
    public Spell spell;
    float last_text_update;
    const float UPDATE_DELAY = 1;
    public GameObject dropbutton;
    public int spellIndex;
    public PlayerController playerController;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        last_text_update = 0;
        if (playerController == null)
        {
            playerController = FindAnyObjectByType<PlayerController>();
        }
    }

    public void SetSpell(Spell spell)
    {
        this.spell = spell;
        GameManager.Instance.spellIconManager.PlaceSprite(spell.GetIcon(), icon.GetComponent<Image>());
    }

    // Update is called once per frame
    void Update()
    {

        if (spell == null) return;
        if (Time.time > last_text_update + UPDATE_DELAY)
        {
            manacost.text = spell.GetManaCost().ToString();
            damage.text = spell.GetDamage().ToString();
            last_text_update = Time.time;
        }
    
        float since_last = Time.time - spell.last_cast;
        float perc;
        if (since_last > spell.GetCooldown())
        {
            perc = 0;
        }
        else
        {
            perc = 1-since_last / spell.GetCooldown();
        }
        cooldown.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 48 * perc);
    }
    public void SetActive(bool active, int index)
    {
        if (!active)
        {
            icon.SetActive(false);
            manacost.text = "";
            damage.text = "";
            playerController.spellcaster.RemoveSpell(index);
        }
        else
        {
            icon.SetActive(true);

        }
    }
}
