using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

public class Spell
{

    protected string name;
    protected string description;
    protected int icon;
    public float last_cast;
    public SpellCaster owner;
    public Hittable.Team team;

    public Spell(SpellCaster owner)
    {
        this.owner = owner;
    }

    protected int N;
    protected float delay;
    protected int angle;
    protected string projectileTrajectory;
    protected int secondaryDamage;
    protected string secondaryProjectile;
    protected float spray;

    public virtual string GetName() => name ?? "Bolt";
    public virtual string GetDescription() => description ?? "A basic bolt of energy.";

    public virtual int GetManaCost() => 10;
    public virtual int GetDamage() => 100;
    public virtual float GetCooldown() => 0.75f;
    public virtual int GetIcon() => 0;
    public virtual int GetSpeed() => 0;

    public virtual IEnumerator Cast(Vector3 where, Vector3 target, Hittable.Team team)
    {
        last_cast = Time.time;
        GameManager.Instance.projectileManager.CreateProjectile(0, "straight", where, target - where, 15f, OnHit);
        yield return new WaitForEndOfFrame();
    }


    protected virtual void OnHit(Hittable other, Vector3 impact)
    {
        if (other.team != team)
            other.Damage(new Damage(GetDamage(), Damage.Type.ARCANE));
    }


    public bool IsReady()
    {
        return (last_cast + GetCooldown() < Time.time);
    }
}
