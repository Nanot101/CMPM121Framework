using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Internal;
using UnityEngine;

public class ArcaneSpray : SpellBase
{
    private SpellData data;
    private RpnEvaluator rpn = new();

    public ArcaneSpray(SpellCaster owner) : base(owner)
    {
        data = SpellLoader.Spells["arcane_spray"];
    }

    public override string GetName() => data.name;
    public override int GetManaCost() => data.mana_cost;
    public override int GetDamage() => rpn.EvaluateRPN(data.damage.amount, BuildVars());
    public override float GetCooldown() => data.cooldown;
    public override int GetIcon() => data.icon;

    private Dictionary<string, int> BuildVars()
    {
        return new Dictionary<string, int>
        {
            { "power", owner.Power },
            { "wave", GameManager.Instance.CurrentWave},
            { "base", 1 }
        };
    }

    public override IEnumerator Cast(Vector3 where, Vector3 target, Hittable.Team team)
    {
        Dictionary<string, int> vars = BuildVars();

        int numProjectiles = rpn.EvaluateRPN(data.N, vars);
        float sprayAngle = float.Parse(data.spray); // in radians
        int damage = rpn.EvaluateRPN(data.damage.amount, vars);
        int spriteIndex = data.projectile.sprite;
        string trajectory = data.projectile.trajectory;
        float speed = rpn.EvaluateRPN(data.projectile.speed, vars);
        float lifetime = 1f;

        if (!string.IsNullOrEmpty(data.projectile.lifetime))
        {
            lifetime = rpn.EvaluateRPN(data.projectile.lifetime, vars);
        }

        // Calculate direction from 'where' to 'target'
        Vector3 baseDir = (target - where).normalized;
        float baseAngle = Mathf.Atan2(baseDir.y, baseDir.x);
        float angleOffset = -sprayAngle * (numProjectiles - 1) / 2f;

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
                //FIXME: missing argument
                
                ,lifetime
            );
        }

        yield return null;
    }
}
