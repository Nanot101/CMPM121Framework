using UnityEngine;
using UnityEngine.InputSystem;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.IO;
using System.Collections.Generic;
using JetBrains.Annotations;

public class PlayerController : MonoBehaviour
{
    //public GameEndText GameEndText;
    public Hittable hp;
    public HealthBar healthui;
    public ManaBar manaui;

    public SpellCaster spellcaster;
    public SpellUI[] spellUIs;
    public ClassLoad ClassLoad;
    public ClassDef chosenClass;

    public Unit unit;
    public GameEndText endText;
    public SpellBuilder spellBuilder;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        unit = GetComponent<Unit>();
        GameManager.Instance.player = gameObject;
        chosenClass = ClassLoad.GetClass("mage");
        if (chosenClass != null)
        {
            // Example usage
            Debug.Log($"Using class mage: Health={chosenClass.health}, Mana={chosenClass.mana}");
            // Initialize player stats here, e.g.:
            // this.health = playerClass.health;
            // this.mana = playerClass.mana;
        }
        else
        {
            Debug.LogError("Failed to load player class.");
        }
    }

    public void ChooseClass(ClassDef chosenClass)
    {
        this.chosenClass = chosenClass;
    }

    public void StartLevel()
    {
        RpnEvaluator rpn = new RpnEvaluator();
        Dictionary<string, float> vars = new Dictionary<string, float>
            {
                { "wave", (float)GameManager.Instance.CurrentWave }
            };
        //FIXME: how to call this chooseclass method
        ChooseClass(playerClass);
        int hpNum = (int)rpn.EvaluateRPN(chosenClass.health, vars);
        int mana = (int)rpn.EvaluateRPN(chosenClass.mana, vars);
        int manaRegen = (int)rpn.EvaluateRPN(chosenClass.mana_regeneration, vars);
        int spellPower = (int)rpn.EvaluateRPN(chosenClass.spellpower, vars);
        int speed = (int)rpn.EvaluateRPN(chosenClass.speed, vars);
        spellcaster = new SpellCaster(mana, manaRegen, Hittable.Team.PLAYER, spellBuilder);
        Spell spell = new ArcaneBolt(spellcaster);
        spellcaster.setSpell(spell);
        StartCoroutine(spellcaster.ManaRegeneration());
       
        hp = new Hittable(hpNum, Hittable.Team.PLAYER, gameObject);
        //hp.OnDeath += Die;
        // hp.team = Hittable.Team.PLAYER;

        // // tell UI elements what to show
        // healthui.SetHealth(hp);
        // manaui.SetSpellCaster(spellcaster);
        // spellui.SetSpell(spellcaster.spell);
        //spellcaster = new SpellCaster(125, 8, Hittable.Team.PLAYER);
        //StartCoroutine(spellcaster.ManaRegeneration());
        
        //hp = new Hittable(100, Hittable.Team.PLAYER, gameObject);
        hp.OnDeath += Die;
        hp.team = Hittable.Team.PLAYER;

        // tell UI elements what to show
        healthui.SetHealth(hp);
        manaui.SetSpellCaster(spellcaster);
        //spellUIs = new SpellUI[4];
        for(int i = 0; i < 4; i++)
        {
            // Debug.Log("playercont: in for loop");
            if (spellcaster.getSpellAtIndex(i) != null)
            {
                // Debug.Log("playercont: in if statement");
                spellUIs[i].SetSpell(spellcaster.getSpellAtIndex(i));
            }
        }
    }

    public void updateHP()
    {
        //only do next line if new maxHP increases their curent HP
        //int missingHP = hp.max_hp - hp.hp;
        int oldMaxHP = hp.max_hp;
        Debug.Log("updateHP called");
        RpnEvaluator rpn = new RpnEvaluator();
        Dictionary<string, float> vars = new Dictionary<string, float>
            {
                { "wave", (float)(GameManager.Instance.CurrentWave + 1) } };
        int hpNum = (int)rpn.EvaluateRPN(chosenClass.health, vars);
        int mana = (int)rpn.EvaluateRPN(chosenClass.mana, vars);
        int manaRegen = (int)rpn.EvaluateRPN(chosenClass.mana_regeneration, vars);
        int spellPower = (int)rpn.EvaluateRPN(chosenClass.spellpower, vars);
        int speed = (int)rpn.EvaluateRPN(chosenClass.speed, vars);
        spellcaster = new SpellCaster(mana, manaRegen, Hittable.Team.PLAYER, spellBuilder);
        //int hpNum = (int)rpn.EvaluateRPN("95 wave 5 * +", vars);
        //int mana = (int)rpn.EvaluateRPN("90 wave 10 * +", vars);
        //int manaRegen = (int)rpn.EvaluateRPN("10 wave +", vars);
        //int spellPower = (int)rpn.EvaluateRPN("wave 10 *", vars);
        //hp.SetMaxHP(hpNum);
        //spellcaster = new SpellCaster(mana, manaRegen, Hittable.Team.PLAYER, spellBuilder);
        StartCoroutine(spellcaster.ManaRegeneration());
        manaui.SetSpellCaster(spellcaster);


        //only do next line if MaxHP increases their current HP
        hp.hp += (hpNum - oldMaxHP);
    }

    void OnAttack(InputValue value)
    {
        if (GameManager.Instance.state == GameManager.GameState.PREGAME || GameManager.Instance.state == GameManager.GameState.GAMEOVER) return;
        Vector2 mouseScreen = Mouse.current.position.value;
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(mouseScreen);
        mouseWorld.z = 0;
        StartCoroutine(spellcaster.Cast(transform.position, mouseWorld));
    }

    void OnMove(InputValue value)
    {
        if (GameManager.Instance.state == GameManager.GameState.PREGAME || GameManager.Instance.state == GameManager.GameState.GAMEOVER) return;
        // int currSpeed = GetSpeed();
        unit.movement = value.Get<Vector2>()*GetSpeed();
    }

    void Die()
    {
        Debug.Log("You Lost");
        endText.onDie();
    }
    public int GetCurrentHp()
    {
        return hp.hp;
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1)) {
            spellcaster.setActiveSpell(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            spellcaster.setActiveSpell(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            spellcaster.setActiveSpell(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            spellcaster.setActiveSpell(3);
        }
    }
    public int GetSpeed()
    {
        RpnEvaluator rpn = new RpnEvaluator();
        Dictionary<string, float> vars = new Dictionary<string, float>
            {
                { "wave", (float)(GameManager.Instance.CurrentWave + 1) } };
        int currSpeed = (int)rpn.EvaluateRPN(chosenClass.speed, vars);
        return currSpeed;
    }
    public int GetSpellpower()
    {
        RpnEvaluator rpn = new RpnEvaluator();
        Dictionary<string, float> vars = new Dictionary<string, float>
            {
                { "wave", (float)(GameManager.Instance.CurrentWave + 1) } };
        int currSpellpower = (int)rpn.EvaluateRPN(chosenClass.spellpower, vars);
        return currSpellpower;
    }
    public void AddToSpellpower(int numAdded)
    {
        chosenClass.spellpower += " " + numAdded + " +";
    }
}