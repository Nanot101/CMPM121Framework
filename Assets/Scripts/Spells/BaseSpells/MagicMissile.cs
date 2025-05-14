using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicMissile : Spell
{
    private readonly SpellData data;
    private readonly RpnEvaluator rpn = new();

    public MagicMissile(SpellCaster owner) : base(owner)
    {
        data = SpellLoader.Spells["magic_missile"];
    }

    // --- Attribute Getters ---
    // => data.name ?? "(Default) Magic Missile";
    public override string GetName() { // read and working
        // Debug.Log($"[Magic Missile] Name Read: {data.name ?? "(Default) Magic Missile"} | What I want: {data.name}");
        return data.name ?? "(Default) Magic Missile";
    }

    public override int GetManaCost()
    {
        return rpn.SafeEvaluateInt(data.mana_cost, BuildVars(), 20);
    }

    public override int GetDamage()
    {
        return rpn.SafeEvaluateInt(data.damage.amount, BuildVars(), 10);
    }

    public override float GetCooldown() // read and applied
    {
        float evaluatedCooldown = rpn.SafeEvaluateFloat(data.cooldown, BuildVars(), 3f);
        //Debug.Log($"[Magic Missile] Cooldown Read: {evaluatedCooldown} | What I want: {data.cooldown}");
        return evaluatedCooldown;
    }

    //=> data.icon >= 0 ? data.icon : 0;
    public override int GetIcon() { // read and applied
        // Debug.Log($"[Magic Missile] Icon Read: {(data.icon >= 0 ? data.icon : 0)} | What I want: {data.icon}");
        return data.icon >= 0 ? data.icon : 0;
    }
    

    public override float GetSpeed()
    {
        return rpn.SafeEvaluateFloat(data.projectile.speed, BuildVars(), 10f); // not being read, defaults to speed which is intended but incorrect.
    }

    // => data.projectile.trajectory ?? "homing"; // not being read, defaults to homing which is intended but incorrect.
    public override string GetTrajectory()  // read and working
    {
        // Debug.Log($"[Magic Missile] Trajectory Read: {data.projectile.trajectory ?? "homing"} | What I want: {data.projectile.trajectory}");
        return data.projectile.trajectory ?? "homing";
    }

    public override float GetSize()
    {
        return Mathf.Max(0.1f, data.size > 0 ? data.size : 0.7f);
    }

    // --- Casting Logic ---
    public override IEnumerator Cast(Vector3 where, Vector3 target, Hittable.Team team)
    {
        Debug.Log("Magic Missile's Cast called");
        string name = GetName();
        float cooldown = GetCooldown();
        last_cast = Time.time;
        Dictionary<string, float> vars = BuildVars();
        int damage = GetDamage();
        int spriteIndex = data.projectile.sprite;
        string trajectory = GetTrajectory();
        float speed = GetSpeed();
        float lifetime = rpn.SafeEvaluateFloat(data.projectile.lifetime, vars, 2f);

        Vector3 direction = (target - where).normalized;

        GameManager.Instance.projectileManager.CreateProjectile(
            spriteIndex,
            trajectory,
            where,
            direction,
            speed,
            (hitTarget, hitPoint) =>
            {
                Damage.Type type = data.damage.type;
                Damage dmg = new Damage(damage, type);
                hitTarget.Damage(dmg);
            },
            lifetime
        );

        yield return null;
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
            { "power", owner.Power },
            { "wave", GameManager.Instance.CurrentWave },
            { "base", 1 }
        };
    }

    protected override void InitializeSpellData()
    {
        throw new System.NotImplementedException();
    }
}
