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
        int baseCost = SafeEvaluateInt(data.mana_cost);
        return baseCost > 0 ? baseCost : 10;
    }

    public override int GetDamage()
    {
        int baseDamage = SafeEvaluateInt(data.damage.amount);
        return baseDamage > 0 ? baseDamage : 5;
    }

    public override float GetCooldown()
    {
        float baseCooldown = SafeEvaluateFloat(data.cooldown);
        return baseCooldown > 0 ? baseCooldown : 0.75f;
    }

    public override int GetIcon() => data.icon >= 0 ? data.icon : 0;

    public override float GetSpeed()
    {
        float baseSpeed = SafeEvaluateFloat(data.projectile.speed);
        return Mathf.Max(0.1f, baseSpeed);
    }

    public override string GetTrajectory() => data.projectile.trajectory ?? "straight";

    public override float GetSize()
    {
        float size = data.size > 0 ? data.size : 0.7f;
        return Mathf.Max(0.1f, size);
    }

    // --- Casting Logic ---
    public override IEnumerator Cast(Vector3 where, Vector3 target, Hittable.Team team)
    {
        Dictionary<string, int> vars = BuildVars();

        int numProjectiles = SafeEvaluateInt(data.N, 1); // Default to 1 if not specified
        float sprayAngle = SafeParseFloat(data.spray, 0.1f);
        int damage = GetDamage();
        int spriteIndex = data.projectile.sprite;
        string trajectory = GetTrajectory();
        float speed = GetSpeed();
        float lifetime = SafeEvaluateFloat(data.projectile.lifetime, 1f); // This now works!

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
                lifetime
            );
        }

        yield return null;
    }

    // --- Variable Context Builder ---
    private Dictionary<string, int> BuildVars()
    {
        return new Dictionary<string, int>
        {
            { "power", owner.Power },
            { "wave", GameManager.Instance.CurrentWave },
            { "base", 1 }
        };
    }

    // --- Safe Evaluators ---
    private int SafeEvaluateInt(string expression, int fallback = 0)
    {
        if (string.IsNullOrEmpty(expression)) return fallback;
        try
        {
            return rpn.EvaluateRPN(expression, BuildVars());
        }
        catch
        {
            Debug.LogWarning($"Failed to evaluate RPN expression (int): {expression}");
            return fallback;
        }
    }

    private float SafeEvaluateFloat(string expression, float fallback = 0f)
    {
        if (string.IsNullOrEmpty(expression)) return fallback;

        // Check if the expression is a simple numeric value
        if (float.TryParse(expression, out float result))
        {
            return result;
        }

        try
        {
            return rpn.EvaluateRPN(expression, BuildVars());
        }
        catch
        {
            Debug.LogWarning($"Failed to evaluate RPN expression (float): {expression}");
            return fallback;
        }
    }


    private float SafeParseFloat(string value, float fallback = 0f)
    {
        if (string.IsNullOrEmpty(value)) return fallback;
        if (float.TryParse(value, out float result))
        {
            return result;
        }
        Debug.LogWarning($"Failed to parse float: {value}");
        return fallback;
    }

    protected override void InitializeSpellData()
    {
        throw new System.NotImplementedException();
    }
}
