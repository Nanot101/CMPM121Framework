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
    public SpellUI spellui;

    public int speed;

    public Unit unit;
    public GameEndText endText;
    public SpellBuilder spellBuilder;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        unit = GetComponent<Unit>();
        GameManager.Instance.player = gameObject;
    }

    public void StartLevel()
    {
        RpnEvaluator rpn = new RpnEvaluator();
        Dictionary<string, float> vars = new Dictionary<string, float>
            {
                { "wave", (float)GameManager.Instance.CurrentWave } };
        int hpNum = (int)rpn.EvaluateRPN("95 wave 5 * +", vars);
        int mana = (int)rpn.EvaluateRPN("90 wave 10 * +", vars);
        int manaRegen = (int)rpn.EvaluateRPN("10 wave +", vars);
        int spellPower = (int)rpn.EvaluateRPN("wave 10 *", vars);
        spellcaster = new SpellCaster(mana, manaRegen, Hittable.Team.PLAYER, spellBuilder);
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
        spellui.SetSpell(spellcaster.spell);
    }

    void updateHP()
    {
        //only do next line if new maxHP increases their curent HP
        //int missingHP = hp.max_hp - hp.hp;
        int oldMaxHP = hp.max_hp;

        RpnEvaluator rpn = new RpnEvaluator();
        Dictionary<string, float> vars = new Dictionary<string, float>
            {
                { "wave", (float)(GameManager.Instance.CurrentWave + 1) } };
        int hpNum = (int)rpn.EvaluateRPN("95 wave 5 * +", vars);
        int mana = (int)rpn.EvaluateRPN("90 wave 10 * +", vars);
        int manaRegen = (int)rpn.EvaluateRPN("10 wave +", vars);
        int spellPower = (int)rpn.EvaluateRPN("wave 10 *", vars);
        hp.SetMaxHP(hpNum);
        spellcaster = new SpellCaster(mana, manaRegen, Hittable.Team.PLAYER, spellBuilder);
        StartCoroutine(spellcaster.ManaRegeneration());
        manaui.SetSpellCaster(spellcaster);


        //only do next line if MaxHP increases their current HP
        hp.hp += (hpNum - oldMaxHP);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.state == GameManager.GameState.WAVEEND)
        {
            updateHP();
        }
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
        unit.movement = value.Get<Vector2>()*speed;
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
}
