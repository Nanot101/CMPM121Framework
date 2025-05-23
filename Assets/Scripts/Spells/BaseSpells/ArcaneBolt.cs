using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcaneBolt : Spell
{
    //private readonly SpellData data;
    private readonly RpnEvaluator rpn = new();

    public ArcaneBolt(SpellCaster owner) : base(owner)
    {
        attributes = SpellLoader.Spells["arcane_bolt"];
    }

    // --- Attribute Getters ---
    public override string GetName() { // read and working
        // Debug.Log($"[Arcane Bolt] Name Read: {data.name ?? "(Default) Arcane Bolt"} | What I want: {data.name}");
        return attributes.name ?? "(Default) Arcane Bolt";
    }

    public override string GetDescription() // read and working
    {
        // Debug.Log($"[Arcane Bolt] Description Read: {data.description ?? "(Default) Arcane Bolt Description"} | What I want: {data.description}");
        return attributes.description ?? "(Default) Arcane Bolt Description";
    }

    public override int GetManaCost() // read and applied
    {
        int manaCost = rpn.SafeEvaluateInt(attributes.mana_cost, BuildVars(), 10);
        // Debug.Log($"[Arcane Bolt] Mana Read & Calculated: {manaCost} | What I want: {data.mana_cost}");
        return manaCost;
    }

    public override int GetDamage() // read and applied
    {
        int evalDmg = rpn.SafeEvaluateInt(attributes.damage.amount, BuildVars(), 1);
        // Debug.Log($"[Arcane Bolt] Damage Read & Calculated: {evalDmg} | What I want: {data.damage.amount}");
        return evalDmg;
    }

    public override float GetCooldown() // read and applied
    {
        float evaluatedCooldown = rpn.SafeEvaluateFloat(attributes.cooldown, BuildVars(), 1.0f);
        // Debug.Log($"[Arcane Bolt] Cooldown Read: {evaluatedCooldown} | What I want: {data.cooldown}");
        return evaluatedCooldown;
    }

    public override int GetIcon() { // read and applied
        // Debug.Log($"[Arcane Bolt] Icon Read: {(data.icon >= 0 ? data.icon : 0)} | What I want: {data.icon}");
        return attributes.icon >= 0 ? attributes.icon : 0;
    }
    public override float GetSpeed() // read and applied
    {
        float evalSpeed = rpn.SafeEvaluateFloat(attributes.projectile.speed, BuildVars(), 0.1f);
        // Debug.Log($"[Arcane Bolt] Speed Read: {evalSpeed} | What I want: {data.projectile.speed}");
        return evalSpeed;
    }

    public override string GetTrajectory()  // read and applied
    {
        // Debug.Log($"[Arcane Bolt] Trajectory Read: {data.projectile.trajectory ?? "straight"} | What I want: {data.projectile.trajectory}");
        return attributes.projectile.trajectory ?? "straight";
    }
    public override float GetSize() // NOT read bc there's no size in the .json file, applied fallback
    {
        float sizee = Mathf.Max(0.1f, attributes.size > 0 ? attributes.size : 0.7f);
        // Debug.Log($"[Arcane Bolt] Size Read: {(data.size > 0 ? data.size : 0)} | What I want: {data.size}");
        return sizee;
    }

    // --- Casting Logic ---
    public override IEnumerator Cast(Vector3 where, Vector3 target, Hittable.Team team)
    {
        Debug.Log("Arcane Bolt's Cast called");
        Dictionary<string, float> vars = BuildVars();
        int damage = GetDamage();

        // These are so i can debug it on cast
        // string name = GetName();
        // string description = GetDescription();
        // int manaCost = GetManaCost();
        // float size = GetSize();
        // float cooldown = GetCooldown();

        last_cast = Time.time;
        int spriteIndex = attributes.projectile.sprite;
        string trajectory = GetTrajectory();
        float speed = GetSpeed();
        float lifetime = rpn.SafeEvaluateFloat(attributes.projectile.lifetime, vars, 5f);

        Vector3 dir = (target - where).normalized;

        GameManager.Instance.projectileManager.CreateProjectile(
            spriteIndex,
            trajectory,
            where,
            dir,
            speed,
            (hitTarget, hitPoint) =>
            {
                Damage.Type type = attributes.damage.type;
                Damage dmg = new Damage(damage, type);
                hitTarget.Damage(dmg);
            },
            lifetime
        );

        yield return new WaitForEndOfFrame();
    }

    public override bool IsReady()
    {
        float cooldown = GetCooldown();
        bool isReady = Time.time >= last_cast + cooldown;
        // Debug.Log($"[Arcane Bolt] IsReady Check: {isReady} | Current Time: {Time.time} | Next Available: {last_cast + cooldown}");
        return isReady;
    }

    // --- Variable Context Builder ---
    private Dictionary<string, float> BuildVars()
    {
        return new Dictionary<string, float>
        {
            { "power", owner.CurrentSpellpower },
            { "wave", GameManager.Instance.CurrentWave },
            { "base", 1 }
        };
    }

    protected override void InitializeSpellData()
    {
        throw new System.NotImplementedException();
    }
}
