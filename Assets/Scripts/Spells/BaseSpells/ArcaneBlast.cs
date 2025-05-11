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
    }

    // --- Attribute Getters ---
    public override string GetName() => data.name ?? "(Default) Arcane Blast";

    public override int GetManaCost()
    {
        return rpn.SafeEvaluateInt(data.mana_cost, BuildVars(), 15);
    }

    public override int GetDamage()
    {
        return rpn.SafeEvaluateInt(data.damage.amount, BuildVars(), 10);
    }

    public override float GetCooldown()
    {
        return rpn.SafeEvaluateFloat(data.cooldown, BuildVars(), 1.5f);
    }

    public override int GetIcon() => data.icon >= 0 ? data.icon : 0;

    public override float GetSpeed()
    {
        return rpn.SafeEvaluateFloat(data.projectile.speed, BuildVars(), 12f);
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
        int damage = GetDamage();
        int spriteIndex = data.projectile.sprite;
        string trajectory = GetTrajectory();
        float speed = GetSpeed();
        float lifetime = rpn.SafeEvaluateFloat(data.projectile.lifetime, vars, 2f);

        // Cast the main projectile (Arcane Blast) - only ONE slow-moving projectile
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
    private int GetFinalDamage()
    {
        return data.GetFinalDamage(10); // Assuming a default power value of 10
    }

    private int GetSecondaryDamage()
    {
        return data.GetFinalSecondaryDamage();
    }

    private float GetProjectileSpeed()
    {
        return data.GetFinalSpeed();
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
    private Dictionary<string, float> BuildVars()
    {
        //Debug.Log($"Power: {owner.Power}, Wave: {GameManager.Instance.CurrentWave}");
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
