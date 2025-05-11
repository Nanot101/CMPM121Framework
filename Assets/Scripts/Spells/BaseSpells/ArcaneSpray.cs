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
    public override string GetName() => data.name ?? "(Default) Arcane Spray";

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

    // --- Casting Logic ---
    public override IEnumerator Cast(Vector3 where, Vector3 target, Hittable.Team team)
    {
        Dictionary<string, float> vars = BuildVars();

        int numProjectiles = rpn.SafeEvaluateInt(data.N, vars, 1);
        float sprayAngle = rpn.SafeParseFloat(data.spray, 0.1f);
        int damage = GetDamage();
        int spriteIndex = data.projectile.sprite;
        string trajectory = GetTrajectory();
        float speed = GetSpeed();
        float lifetime = rpn.SafeEvaluateFloat(data.projectile.lifetime, vars, 1f);

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
