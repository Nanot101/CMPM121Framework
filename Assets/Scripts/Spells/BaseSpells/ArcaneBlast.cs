using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcaneBlast : Spell
{
    private readonly SpellData data;
    private readonly RpnEvaluator rpn = new();

    public ArcaneBlast(SpellCaster owner) : base(owner)
    {
        data = SpellLoader.Spells["arcane_blast"];
        // Debug.Log($"[ArcaneBlast] Owner is {(owner == null ? "null" : "not null")}, Power is {owner?.Power}");
    }

    // --- Attribute Getters ---
    public override string GetName() { // read and working
        // Debug.Log($"[Arcane Blast] Name Read: {data.name ?? "(Default) Arcane Blast"} | What I want: {data.name}");
        return data.name ?? "(Default) Arcane Blast";
    }

    public override string GetDescription() // read and working
    {
        // Debug.Log($"[Arcane Blast] Description Read: {data.description ?? "(Default) Arcane Blast Description"} | What I want: {data.description}");
        return data.description ?? "(Default) Arcane Blast Description";
    }

    public override int GetManaCost() // read and applied
    {
        int manaCost = rpn.SafeEvaluateInt(data.mana_cost, BuildVars(), 10);
        // Debug.Log($"[Arcane Blast] Mana Read & Calculated: {manaCost} | What I want: {data.mana_cost}");
        return manaCost;
    }

    public override int GetDamage() // read and applied
    {
        int evalDmg = rpn.SafeEvaluateInt(data.damage.amount, BuildVars(), 1);
        // Debug.Log($"[Arcane Blast] Damage Read & Calculated: {evalDmg} | What I want: {data.damage.amount}");
        return evalDmg;
    }

    public override float GetCooldown() // read and applied
    {
        float evaluatedCooldown = rpn.SafeEvaluateFloat(data.cooldown, BuildVars(), 1.0f);
        // Debug.Log($"[Arcane Blast] Cooldown Read: {evaluatedCooldown} | What I want: {data.cooldown}");
        return evaluatedCooldown;
    }

    public override int GetIcon() { // read and applied
        // Debug.Log($"[Arcane Blast] Icon Read: {(data.icon >= 0 ? data.icon : 0)} | What I want: {data.icon}");
        return data.icon >= 0 ? data.icon : 0;
    }

    public override float GetSpeed() // read and applied
    {
        float evalSpeed = rpn.SafeEvaluateFloat(data.projectile.speed, BuildVars(), 0.1f);
        // Debug.Log($"[Arcane Blast] Speed Read: {evalSpeed} | What I want: {data.projectile.speed}");
        return evalSpeed;
    }

    public override string GetTrajectory()  // read and applied
    {
        // Debug.Log($"[Arcane Blast] Trajectory Read: {data.projectile.trajectory ?? "straight"} | What I want: {data.projectile.trajectory}");
        return data.projectile.trajectory ?? "straight";
    }

    public override float GetSize() // NOT read bc there's no size in the .json file, applied fallback
    {
        float sizee = Mathf.Max(0.1f, data.size > 0 ? data.size : 0.7f);
        // Debug.Log($"[Arcane Blast] Size Read: {(data.size > 0 ? data.size : 0)} | What I want: {data.size}");
        return sizee;
    }

    // --- Casting Logic ---
    public override IEnumerator Cast(Vector3 where, Vector3 target, Hittable.Team team)
    {
        Debug.Log("Arcane Blast's Cast called");
        Dictionary<string, float> vars = BuildVars();
        data.SetContext(vars);
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
        float lifetime = rpn.SafeEvaluateFloat(data.projectile.lifetime, vars, 2f);

        // Cast only ONE slow-moving projectile
        Vector3 direction = (target - where).normalized;
        GameManager.Instance.projectileManager.CreateProjectile(
            spriteIndex,
            trajectory,
            where,
            direction,
            speed,
            OnFirstHit,
            lifetime
        );

        yield return new WaitForEndOfFrame();
    }

    public override bool IsReady()
    {
        float cooldown = GetCooldown();
        bool isReady = Time.time >= last_cast + cooldown;
        // Debug.Log($"[Arcane Blast] IsReady Check: {isReady} | Current Time: {Time.time} | Next Available: {last_cast + cooldown}");
        return isReady;
    }

    // --- Handling Impact ---
    private void OnFirstHit(Hittable other, Vector3 hitPosition)
    {
        if (other.team != team)
        {
            // Deal the primary damage
            other.Damage(new Damage(GetDamage(), data.damage.type));
        }

        // Explode into secondary projectiles
        int numProjectiles = GetFinalNumProjectiles();
        float degreeGap = 360f / numProjectiles;

        for (int i = 0; i < numProjectiles; i++)
        {
            Vector3 direction = new Vector3(Mathf.Sin(degreeGap * i * Mathf.Deg2Rad), Mathf.Cos(degreeGap * i * Mathf.Deg2Rad), 0);
            GameManager.Instance.projectileManager.CreateProjectile(
                data.secondary_projectile.sprite,
                data.secondary_projectile.trajectory,
                hitPosition,
                direction,
                GetSecondaryProjectileSpeed(),
                OnSecondaryHit,
                data.secondary_projectile != null ? rpn.SafeEvaluateFloat(data.secondary_projectile.lifetime, BuildVars(), 2f) : 2f
            );
        }
    }

    private void OnSecondaryHit(Hittable other, Vector3 hitPosition)
    {
        if (other.team != team)
        {
            other.Damage(new Damage(GetSecondaryDamage(), data.damage.type));
        }
    }

    // --- Helper Methods ---

    private int GetSecondaryDamage()
    {
        return data.GetFinalSecondaryDamage();
    }

    private float GetProjectileSpeed()
    {
        return data.GetFinalSpeed(); // we dont need this
    }

    private float GetSecondaryProjectileSpeed()
    {
        if (data.secondary_projectile != null)
        {
            return rpn.SafeEvaluateFloat(data.secondary_projectile.speed, BuildVars(), 12f);
        }
        return GetProjectileSpeed();
    }

    private int GetFinalNumProjectiles()
    {
        return data.GetFinalNumProjectiles();
    }

    // --- Variable Context Builder ---
    // private Dictionary<string, float> BuildVars()
    // {
    //     //Debug.Log($"Power: {owner.Power}, Wave: {GameManager.Instance.CurrentWave}");
    //     return new Dictionary<string, float>
    //     {
    //         { "power", owner.Power },
    //         { "wave", GameManager.Instance.CurrentWave },
    //         { "base", 1 }
    //     };
    // }
    private Dictionary<string, float> BuildVars()
    {
        var vars = new Dictionary<string, float>
        {
            { "power", owner.Power },
            { "wave", GameManager.Instance.CurrentWave },
            { "base", 1 }
        };

        // Debug the values before returning
        // Debug.Log("[ArcaneBlast] BuildVars Contents:");
        // foreach (var entry in vars)
        // {
        //     Debug.Log($"{entry.Key}: {entry.Value}");
        // }

        return vars;
    }


    protected override void InitializeSpellData()
    {
        throw new System.NotImplementedException();
    }
}
