using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcanePulse : Spell
{
    private readonly SpellData data;
    private readonly RpnEvaluator rpn = new();
    private float interval;

    public ArcanePulse(SpellCaster owner) : base(owner)
    {
        data = SpellLoader.Spells["arcane_pulse"];
        interval = rpn.SafeEvaluateFloat(data.interval, BuildVars(), 5f);
    }

    // --- Attribute Getters ---
    public override string GetName() => data.name ?? "(Default) Arcane Pulse";

    public override int GetManaCost() => rpn.SafeEvaluateInt(data.mana_cost, BuildVars(), 10);

    public override int GetDamage() => rpn.SafeEvaluateInt(data.damage.amount, BuildVars(), 5);

    public override float GetCooldown() => rpn.SafeEvaluateFloat(data.cooldown, BuildVars(), 0.75f);

    public override int GetIcon() => data.icon >= 0 ? data.icon : 0;

    public override float GetSpeed() => rpn.SafeEvaluateFloat(data.projectile.speed, BuildVars(), 0.1f);

    public override string GetTrajectory() => data.projectile.trajectory ?? "straight";

    public override float GetSize() => Mathf.Max(0.1f, data.size > 0 ? data.size : 0.7f);

    // --- Casting Logic ---
    public override IEnumerator Cast(Vector3 where, Vector3 target, Hittable.Team team)
    {
        Dictionary<string, float> vars = BuildVars();

        int damage = GetDamage();
        float speed = GetSpeed();
        float lifetime = rpn.SafeEvaluateFloat(data.projectile.lifetime, vars, 3f);
        int spriteIndex = data.projectile.sprite;
        string trajectory = GetTrajectory();
        float coneAngle = 30f; // Spread angle for the secondary projectiles
        int numProjectiles = rpn.SafeEvaluateInt(data.N, vars, 2); // Number of secondary projectiles

        // Main projectile
        Vector3 baseDir = (target - where).normalized;
        var mainProjectile = GameManager.Instance.projectileManager.CreateProjectileWithReference(
            spriteIndex,
            trajectory,
            where,
            baseDir,
            speed,
            (targetHit, hitPoint) =>
            {
                Damage.Type type = data.damage.type;
                Damage dmg = new Damage(damage, type);
                targetHit.Damage(dmg);
            },
            lifetime
        );

        // --- Secondary Projectile Logic ---
        float elapsed = 0f;
        while (elapsed < lifetime)
        {
            elapsed += interval;
            yield return new WaitForSeconds(interval);

            if (mainProjectile == null) break;

            // Fire secondary projectiles in a cone
            Vector3 mainPosition = mainProjectile.transform.position;
            Vector3 mainDirection = mainProjectile.GetComponent<ProjectileController>().movement.GetDirection();
            // Debug.Log($"Main Direction: {mainDirection}");

            for (int i = 0; i < numProjectiles; i++)
            {
                float angleOffset = (-coneAngle / 2) + (i * (coneAngle / Mathf.Max(1, numProjectiles - 1)));
                Vector3 newDir = Quaternion.AngleAxis(angleOffset, Vector3.forward) * mainDirection;
                GameManager.Instance.projectileManager.CreateProjectile(
                    data.secondary_projectile.sprite,
                    data.secondary_projectile.trajectory,
                    mainPosition,
                    newDir,
                    rpn.SafeEvaluateFloat(data.secondary_projectile.speed, vars, 8f),
                    (targetHit, hitPoint) =>
                    {
                        Damage.Type type = data.damage.type;
                        Damage dmg = new Damage(damage, type);
                        targetHit.Damage(dmg);
                    },
                    rpn.SafeEvaluateFloat(data.secondary_projectile.lifetime, vars, 1f)
                );
            }
        }
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
