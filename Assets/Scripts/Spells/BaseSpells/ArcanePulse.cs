using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcanePulse : Spell
{
    // private readonly Spellattributes attributes;
    private readonly RpnEvaluator rpn = new();
    private float interval;

    public ArcanePulse(SpellCaster owner, float interval) : base(owner)
    {
        attributes = SpellLoader.Spells["arcane_pulse"];
        this.interval = interval;
    }

    // --- Attribute Getters ---
    public override string GetName() => attributes.name ?? "(Default) Arcane Pulse";

    public override int GetManaCost() => rpn.SafeEvaluateInt(attributes.mana_cost, BuildVars(), 10);

    public override int GetDamage() => rpn.SafeEvaluateInt(attributes.damage.amount, BuildVars(), 5);

    public override float GetCooldown() => rpn.SafeEvaluateFloat(attributes.cooldown, BuildVars(), 0.75f);

    public override int GetIcon() => attributes.icon >= 0 ? attributes.icon : 0;

    public override float GetSpeed() => rpn.SafeEvaluateFloat(attributes.projectile.speed, BuildVars(), 0.1f);

    public override string GetTrajectory() => attributes.projectile.trajectory ?? "straight";

    public override float GetSize() => Mathf.Max(0.1f, attributes.size > 0 ? attributes.size : 0.7f);

    public override Damage.Type GetDamageType()
    {
        return Damage.Type.ARCANE;
    }


    // --- Casting Logic ---
    public override IEnumerator Cast(Vector3 where, Vector3 target, Hittable.Team team)
    {
        Dictionary<string, float> vars = BuildVars();

        int damage = GetDamage();
        Damage.Type type = attributes.damage.type;

            //Damage overridden = GetOverriddenDamage();
            //if (overridden != null)
            //{
            //    damage = overridden.amount;
            //    type = overridden.type;
            //}
        last_cast = Time.time;
        float speed = GetSpeed();
        float lifetime = rpn.SafeEvaluateFloat(attributes.projectile.lifetime, vars, 3f);
        int spriteIndex = attributes.projectile.sprite;
        string trajectory = GetTrajectory();
        float coneAngle = 30f; // Spread angle for the secondary projectiles
        int numProjectiles = rpn.SafeEvaluateInt(attributes.N, vars, 2); // Number of secondary projectiles

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
                    attributes.secondary_projectile.sprite,
                    attributes.secondary_projectile.trajectory,
                    mainPosition,
                    newDir,
                    rpn.SafeEvaluateFloat(attributes.secondary_projectile.speed, vars, 8f),
                    (targetHit, hitPoint) =>
                    {
                        Damage.Type type = attributes.damage.type;
                        Damage dmg = new Damage(damage, type);
                        targetHit.Damage(dmg);
                    },
                    rpn.SafeEvaluateFloat(attributes.secondary_projectile.lifetime, vars, 1f)
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
    public override bool IsReady()
    {
        float cooldown = GetCooldown();
        bool isReady = Time.time >= last_cast + cooldown;
        // Debug.Log($"[Arcane Pulse] IsReady Check: {isReady} | Current Time: {Time.time} | Next Available: {last_cast + cooldown}");
        return isReady;
    }

    protected override void InitializeSpellData()
    {
        throw new System.NotImplementedException();
    }
}
