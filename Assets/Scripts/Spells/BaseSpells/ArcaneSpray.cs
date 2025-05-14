using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcaneSpray : Spell
{
    private readonly SpellData data;
    private readonly RpnEvaluator rpn = new();

    public ArcaneSpray(SpellCaster owner) : base(owner)
    {
        data = SpellLoader.Spells["arcane_spray"];
    }

    // --- Attribute Getters ---
    public override string GetName() { // read and working
        // Debug.Log($"[Arcane Spray] Name Read: {data.name ?? "(Default) Arcane Spray"} | What I want: {data.name}");
        return data.name ?? "(Default) Arcane Spray";
    }

    public override string GetDescription() // read and working
    {
        // Debug.Log($"[Arcane Spray] Description Read: {data.description ?? "(Default) Arcane Spray Description"} | What I want: {data.description}");
        return data.description ?? "(Default) Arcane Spray Description";
    }

    public override int GetManaCost() // read and applied
    {
        int manaCost = rpn.SafeEvaluateInt(data.mana_cost, BuildVars(), 10);
        // Debug.Log($"[Arcane Spray] Mana Read & Calculated: {manaCost} | What I want: {data.mana_cost}");
        return manaCost;
    }


    public override int GetDamage() // read and applied
    {
        int evalDmg = rpn.SafeEvaluateInt(data.damage.amount, BuildVars(), 1);
        // Debug.Log($"[Arcane Spray] Damage Read & Calculated: {evalDmg} | What I want: {data.damage.amount}");
        return evalDmg;
    }

    public override float GetCooldown() // read and applied
    {
        float evaluatedCooldown = rpn.SafeEvaluateFloat(data.cooldown, BuildVars(), 1.0f);
        // Debug.Log($"[Arcane Spray] Cooldown Read: {evaluatedCooldown} | What I want: {data.cooldown}");
        return evaluatedCooldown;
    }

    public override int GetIcon() { // read and applied
        // Debug.Log($"[Arcane Spray] Icon Read: {(data.icon >= 0 ? data.icon : 0)} | What I want: {data.icon}");
        return data.icon >= 0 ? data.icon : 0;
    }

    public override float GetSpeed() // read and applied
    {
        float evalSpeed = rpn.SafeEvaluateFloat(data.projectile.speed, BuildVars(), 0.1f);
        // Debug.Log($"[Arcane Spray] Speed Read: {evalSpeed} | What I want: {data.projectile.speed}");
        return evalSpeed;
    }

    public override string GetTrajectory()  // read and applied
    {
        // Debug.Log($"[Arcane Spray] Trajectory Read: {data.projectile.trajectory ?? "straight"} | What I want: {data.projectile.trajectory}");
        return data.projectile.trajectory ?? "straight";
    }

    public override float GetSize() // NOT read bc there's no size in the .json file, applied fallback
    {
        float sizee = Mathf.Max(0.1f, data.size > 0 ? data.size : 0.7f);
        // Debug.Log($"[Arcane Spray] Size Read: {(data.size > 0 ? data.size : 0)} | What I want: {data.size}");
        return sizee;
    }

    // --- Casting Logic ---
    public override IEnumerator Cast(Vector3 where, Vector3 target, Hittable.Team team)
    {
        Debug.Log("Arcane Spray's Cast called");
        Dictionary<string, float> vars = BuildVars();
        int damage = GetDamage();
        
        // These are so i can debug it on cast
        // string name = GetName();
        // string description = GetDescription();
        // int manaCost = GetManaCost();
        // float size = GetSize();
        // float cooldown = GetCooldown();

        last_cast = Time.time;
        int spriteIndex = data.projectile.sprite;
        string trajectory = GetTrajectory();
        float speed = GetSpeed();
        float lifetime = rpn.SafeEvaluateFloat(data.projectile.lifetime, vars, 1f);
        int numProjectiles = rpn.SafeEvaluateInt(data.N, vars, 1);
        float sprayAngle = rpn.SafeParseFloat(data.spray, 0.1f);

        Vector3 baseDir = (target - where).normalized;
        float baseAngle = Mathf.Atan2(baseDir.y, baseDir.x);
        float angleOffset = numProjectiles > 1 ? -sprayAngle * (numProjectiles - 1) / 2f : 0f;

        for (int i = 0; i < numProjectiles; i++)
        {
            float angle = baseAngle + angleOffset + i * sprayAngle;
            Vector3 dir = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0);

            GameManager.Instance.projectileManager.CreateProjectile(
                spriteIndex,
                trajectory,
                where,
                dir,
                speed,
                (hitTarget, hitPoint) =>
                {
                    Damage.Type type = data.damage.type;
                    Damage dmg = new Damage(damage, type);
                    hitTarget.Damage(dmg);
                },
                lifetime + 2 // hard-coded to extend lifetime cause i like it like that
            );
        }

        yield return new WaitForEndOfFrame();
    }

    public override bool IsReady()
    {
        float cooldown = GetCooldown();
        bool isReady = Time.time >= last_cast + cooldown;
        // Debug.Log($"[Arcane Spray] IsReady Check: {isReady} | Current Time: {Time.time} | Next Available: {last_cast + cooldown}");
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
