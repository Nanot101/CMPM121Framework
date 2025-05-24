using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.Experimental.AI;
using System;

public class SpellCaster
{
    public int mana;
    public int max_mana;
    public int mana_reg;
    public Hittable.Team team;
    public Spell[] spells;
    public int Power;
    public int activeSpell;
    // private int temporarySpellpower = 0;
    // public int CurrentSpellpower => Power + temporarySpellpower;
    private int temporarySpellpowerFromGoldenMask = 0;
    private int temporarySpellpowerFromJadeElephant = 0;

    public int CurrentSpellpower => Power + temporarySpellpowerFromGoldenMask + temporarySpellpowerFromJadeElephant;
    public event Action<int> OnSpellpowerChanged;
    

    public IEnumerator ManaRegeneration()
    {
        while (true)
        {
            mana += mana_reg;
            mana = Mathf.Min(mana, max_mana);
            yield return new WaitForSeconds(1);
        }
    }

    public SpellCaster(int mana, int mana_reg, Hittable.Team team, SpellBuilder spellBuilder)
    {
        this.mana = mana;
        this.max_mana = mana;
        this.mana_reg = mana_reg;
        this.team = team;
        this.Power = 1;
        this.activeSpell = 0;
        //string spellDescription = "";
        this.spells = new Spell[4];
        this.spells[0] = new ArcaneBolt(this);
        this.spells[1] = new ArcaneBlast(this);
        this.spells[2] = new ArcaneSpray(this);
        this.spells[3] = new ArcanePulse(this, 0.5f);
        // spellBuilder.Build(this);

    }

        public void AddMana(int amount)
    {
        mana = Mathf.Min(mana + amount, max_mana);
        Debug.Log($"[SpellCaster] Mana increased by {amount}. Current mana: {mana}");
    }


    // public void AddTemporarySpellpower(int amount)
    // {
    //     temporarySpellpower += amount;
    //     OnSpellpowerChanged?.Invoke(CurrentSpellpower);
    //     Debug.Log($"[SpellCaster] Temporary spellpower added: {amount}. Total power now: {GetTotalPower()}");
    // }
    public void AddTemporarySpellpower(int amount, string source)
    {
        if (source == "Golden Mask" || source == "Power Burst")
            temporarySpellpowerFromGoldenMask += amount;
        else if (source == "Jade Elephant")
            temporarySpellpowerFromJadeElephant += amount;

        OnSpellpowerChanged?.Invoke(CurrentSpellpower);
        Debug.Log($"[SpellCaster] Temporary spellpower added: {amount} from {source}. Total power now: {GetTotalPower()}");
    }

    // public void RemoveTemporarySpellpower()
    // {
    //     temporarySpellpower = 0;
    //     OnSpellpowerChanged?.Invoke(CurrentSpellpower);
    //     Debug.Log($"[SpellCaster] Temporary spellpower removed. Total power now: {GetTotalPower()}");
    // }

    public void RemoveTemporarySpellpower(string source)
    {
        if (source == "Golden Mask" || source == "Power Burst")
            temporarySpellpowerFromGoldenMask = 0;
        else if (source == "Jade Elephant")
            temporarySpellpowerFromJadeElephant = 0;

        OnSpellpowerChanged?.Invoke(CurrentSpellpower);
        Debug.Log($"[SpellCaster] Temporary spellpower from {source} removed. Total power now: {GetTotalPower()}");
    }


    public int GetTotalPower()
    {
        return CurrentSpellpower;
    }

    // public IEnumerator Cast(Vector3 where, Vector3 target)
    // {        
    //     if (mana >= spell.GetManaCost() && spell.IsReady())
    //     {
    //         mana -= spell.GetManaCost();
    //         yield return spell.Cast(where, target, team);
    //     }
    //     yield break;
    // }
    public IEnumerator Cast(Vector3 where, Vector3 target)
    {
        if (spells[activeSpell] == null)
        {
            Debug.Log("Active spell is set to null.");
            yield break;
        }
        if (mana < spells[activeSpell].GetManaCost())
        {
            Debug.Log("[SpellCaster] Not enough mana to cast the spell.");
            yield break;
        }

        if (!spells[activeSpell].IsReady())
        {
            Debug.Log($"[SpellCaster] {spells[activeSpell].GetName()} is still on cooldown.");
            yield break;
        }

        mana -= spells[activeSpell].GetManaCost();
        Debug.Log($"[SpellCaster] Casting {spells[activeSpell].GetName()} at target. Remaining Mana: {mana}");
        yield return spells[activeSpell].Cast(where, target, team);

        EventBus.Instance.DoSpellCast();
        RemoveTemporarySpellpower("Golden Mask");
        RemoveTemporarySpellpower("Power Burst");
    }
    public void setSpell(Spell spell)
    {
        for (int i = 0; i < spells.Length; i++)
        {
            if (spells[i] == null)
            {
                spells[i] = spell;
                Debug.Log("New spell was set at index " + i);
                break;
            }
        }
    }
    public void setActiveSpell(int spellNum)
    {
        if (spells[activeSpell] != null || spells[spellNum] != null )
            activeSpell = spellNum;
        else
            Debug.Log("Spell number invalid: No spell at that index");
    }

    public Spell getSpellAtIndex(int index)
    {
        // Debug.Log($"Spellui at index {index}: {spells[index]}");
        return spells[index];
    }
    public Spell getActiveSpell()
    {
        return spells[activeSpell];
    }
    public void RemoveSpell(int index)
    {
        if (index >= 0 && index < spells.Length)
        {
            Debug.Log($"[SpellCaster] Removing spell at index {index}: {spells[index]?.GetName()}");
            spells[index] = null;
        }
        else
        {
            Debug.LogWarning($"[SpellCaster] Invalid index: {index}");
        }
    }
}
