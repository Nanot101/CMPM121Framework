using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicMissile : Spell
{
    // private readonly SpellData attributes;
    private readonly RpnEvaluator rpn = new();

    public MagicMissile(SpellCaster owner) : base(owner)
    {
        attributes = SpellLoader.Spells["magic_missile"];
    }

    // --- Attribute Getters ---
    public override string GetName() { // read and working
        // Debug.Log($"[Magic Missile] Name Read: {attributes.name ?? "(Default) Magic Missile"} | What I want: {attributes.name}");
        return attributes.name ?? "(Default) Magic Missile";
    }

    public override string GetDescription() // read and working
    {
        // Debug.Log($"[Magic Missile] Description Read: {attributes.description ?? "(Default) Magic Missile Description"} | What I want: {attributes.description}");
        return attributes.description ?? "(Default) Magic Missile Description";
    }

    public override int GetManaCost() // read and applied
    {
        int manaCost = rpn.SafeEvaluateInt(attributes.mana_cost, BuildVars(), 10);
        // Debug.Log($"[Magic Missile] Mana Read & Calculated: {manaCost} | What I want: {attributes.mana_cost}");
        return manaCost;
    }

    public override int GetDamage() // read and applied
    {
        int evalDmg = rpn.SafeEvaluateInt(attributes.damage.amount, BuildVars(), 1);
        // Debug.Log($"[Magic Missile] Damage Read & Calculated: {evalDmg} | What I want: {attributes.damage.amount}");
        return evalDmg;
    }

    public override float GetCooldown() // read and applied
    {
        float evaluatedCooldown = rpn.SafeEvaluateFloat(attributes.cooldown, BuildVars(), 1.0f);
        //Debug.Log($"[Magic Missile] Cooldown Read: {evaluatedCooldown} | What I want: {attributes.cooldown}");
        return evaluatedCooldown;
    }

    public override int GetIcon() { // read and applied
        // Debug.Log($"[Magic Missile] Icon Read: {(attributes.icon >= 0 ? attributes.icon : 0)} | What I want: {attributes.icon}");
        return attributes.icon >= 0 ? attributes.icon : 0;
    }
    
    public override float GetSpeed() // read and applied
    {
        float evalSpeed = rpn.SafeEvaluateFloat(attributes.projectile.speed, BuildVars(), 0.1f);
        // Debug.Log($"[Magic Missile] Speed Read: {evalSpeed} | What I want: {attributes.projectile.speed}");
        return evalSpeed;
    }

    public override string GetTrajectory()  // read and working
    {
        // Debug.Log($"[Magic Missile] Trajectory Read: {attributes.projectile.trajectory ?? "homing"} | What I want: {attributes.projectile.trajectory}");
        return attributes.projectile.trajectory ?? "homing";
    }

    public override float GetSize() // NOT read bc there's no size in the .json file, applied fallback
    {
        float sizee = Mathf.Max(0.1f, attributes.size > 0 ? attributes.size : 0.7f);
        // Debug.Log($"[Magic Missile] Size Read: {(attributes.size > 0 ? attributes.size : 0)} | What I want: {attributes.size}");
        return sizee;
    }

    // --- Casting Logic ---
    public override IEnumerator Cast(Vector3 where, Vector3 target, Hittable.Team team)
    {
        Debug.Log("Magic Missile's Cast called");
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
        float lifetime = rpn.SafeEvaluateFloat(attributes.projectile.lifetime, vars, 2f);

        Vector3 direction = (target - where).normalized;

        GameManager.Instance.projectileManager.CreateProjectile(
            spriteIndex,
            trajectory,
            where,
            direction,
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
        // Debug.Log($"[Magic Missile] IsReady Check: {isReady} | Current Time: {Time.time} | Next Available: {last_cast + cooldown}");
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
