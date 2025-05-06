using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

public class Spell : SpellBase
{
    public float last_cast;
    // public SpellCaster owner;
    // public Hittable.Team team;

    public Spell(SpellCaster owner) : base(owner) { }

    public override string GetName() => "Bolt";
    public override int GetManaCost() => 10;
    public override int GetDamage() => 100;
    public override float GetCooldown() => 0.75f;
    public override int GetIcon() => 0;

    public override IEnumerator Cast(Vector3 where, Vector3 target, Hittable.Team team)
    {
        GameManager.Instance.projectileManager.CreateProjectile(0, "straight", where, target - where, 15f, OnHit);
        yield return null;
    }

    private void OnHit(Hittable other, Vector3 impact)
    {
        if (other.team != team)
            other.Damage(new Damage(GetDamage(), Damage.Type.ARCANE));
    }

    // public bool IsReady()
    // {
    //     return (last_cast + GetCooldown() < Time.time);
    // }

    // public virtual IEnumerator Cast(Vector3 where, Vector3 target, Hittable.Team team)
    // {
    //     this.team = team;
    //     GameManager.Instance.projectileManager.CreateProjectile(0, "straight", where, target - where, 15f, OnHit);
    //     yield return new WaitForEndOfFrame();
    // }

    // void OnHit(Hittable other, Vector3 impact)
    // {
    //     if (other.team != team)
    //     {
    //         other.Damage(new Damage(GetDamage(), Damage.Type.ARCANE));
    //     }

    // }

}
