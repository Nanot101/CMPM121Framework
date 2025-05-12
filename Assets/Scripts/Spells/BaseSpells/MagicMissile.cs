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
    public override string GetName() => data.name ?? "(Default) Magic Missile";

    public override int GetManaCost()
    {
        return rpn.SafeEvaluateInt(data.mana_cost, BuildVars(), 20);
    }

    public override int GetDamage()
    {
        return rpn.SafeEvaluateInt(data.damage.amount, BuildVars(), 10);
    }

    public override float GetCooldown()
    {
        return rpn.SafeEvaluateFloat(data.cooldown, BuildVars(), 3f);
    }

    public override int GetIcon() => data.icon >= 0 ? data.icon : 0;

    public override float GetSpeed()
    {
        return rpn.SafeEvaluateFloat(data.projectile.speed, BuildVars(), 10f);
    }

    public override string GetTrajectory() => data.projectile.trajectory ?? "homing";

    public override float GetSize()
    {
        return Mathf.Max(0.1f, data.size > 0 ? data.size : 0.7f);
    }
        public override Damage.Type GetDamageType()
    {
        return Damage.Type.ARCANE;
    }

    // --- Casting Logic ---
    public override IEnumerator Cast(Vector3 where, Vector3 target, Hittable.Team team)
    {
        Dictionary<string, float> vars = BuildVars();
        int damage = GetDamage();
        Damage.Type type = data.damage.type;

        Damage overridden = GetOverriddenDamage();
        if (overridden != null)
        {
            damage = overridden.amount;
            type = overridden.type;
        }

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
