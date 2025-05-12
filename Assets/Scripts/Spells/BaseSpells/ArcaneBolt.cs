using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcaneBolt : Spell
{
    private readonly SpellData data;
    private readonly RpnEvaluator rpn = new();

    public ArcaneBolt(SpellCaster owner) : base(owner)
    {
        if (owner == null)
        {
            Debug.LogError("ArcaneBolt owner is null");
        }
        else
        {
            Debug.Log("ArcaneBolt initialized with owner: " + owner.team);
        }
        data = SpellLoader.Spells["arcane_bolt"];
    }

    // --- Attribute Getters ---
    public override string GetName() => data.name ?? "(Default) Arcane Bolt";

    public override int GetManaCost()
    {
        return rpn.SafeEvaluateInt(data.mana_cost, BuildVars(), 10);
    }

    public override int GetDamage()
    {
        return rpn.SafeEvaluateInt(data.damage.amount, BuildVars(), 5);
    }

    public override float GetCooldown()
    {
        return rpn.SafeEvaluateFloat(data.cooldown, BuildVars(), 0.75f);
    }

    public override int GetIcon() => data.icon >= 0 ? data.icon : 0;

    public override float GetSpeed()
    {
        return rpn.SafeEvaluateFloat(data.projectile.speed, BuildVars(), 0.1f);
    }

    public override string GetTrajectory() => data.projectile.trajectory ?? "straight";

    public override float GetSize()
    {
        return Mathf.Max(0.1f, data.size > 0 ? data.size : 0.7f);
    }
    public override Damage.Type GetDamageType()
    {
        return Damage.Type.ARCANE;
    }

    // --- Casting Logic ---
    // public override IEnumerator Cast(Vector3 where, Vector3 target, Hittable.Team team)
    // {
    //     Dictionary<string, float> vars = BuildVars();

    //     // int numProjectiles = 1;
    //     int damage = GetDamage();
    //     int spriteIndex = data.projectile.sprite;
    //     string trajectory = GetTrajectory();
    //     float speed = GetSpeed();
    //     float lifetime = rpn.SafeEvaluateFloat(data.projectile.lifetime, vars, 5f);

    //     Vector3 dir = (target - where).normalized;

    //     GameManager.Instance.projectileManager.CreateProjectile(
    //         spriteIndex,
    //         trajectory,
    //         where,
    //         dir,
    //         speed,
    //         (hitTarget, hitPoint) =>
    //         {
    //             Damage.Type type = data.damage.type;
    //             Damage dmg = new Damage(damage, type);
    //             hitTarget.Damage(dmg);
    //         },
    //         lifetime
    //     );

    //     yield return null;
    // }

    public override IEnumerator Cast(Vector3 where, Vector3 target, Hittable.Team team)
    {
        Dictionary<string, float> vars = BuildVars();

        int damage = GetDamage(); // Base damage calculation
        Damage.Type type = data.damage.type;

        // Check if a modifier has overridden the damage
        Damage overridden = GetOverriddenDamage();
        if (overridden != null)
        {
            damage = overridden.amount;
            type = overridden.type;
        }

        int spriteIndex = data.projectile.sprite;
        string trajectory = GetTrajectory();
        float speed = GetSpeed();
        float lifetime = rpn.SafeEvaluateFloat(data.projectile.lifetime, vars, 5f);

        Vector3 dir = (target - where).normalized;

        GameManager.Instance.projectileManager.CreateProjectile(
            spriteIndex,
            trajectory,
            where,
            dir,
            speed,
            (hitTarget, hitPoint) =>
            {
                Damage dmg = new Damage(damage, type);
                hitTarget.Damage(dmg);
            },
            lifetime
        );

        yield return null;
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
