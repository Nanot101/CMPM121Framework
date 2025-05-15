using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

public abstract class Spell
{

    protected string name;
    protected string description;
    protected int icon;
    public float last_cast;
    public float lifetime;
    public SpellCaster owner;
    public Hittable.Team team;
    protected SpellData attributes;
    int angle;
    public Spell(SpellCaster owner)
    {
        this.owner = owner;
        attributes = new SpellData();
    }
    public Spell(SpellData values, SpellCaster owner)
    {
        this.owner = owner;
        this.attributes = values;
    }
    public void SetSpellValues(SpellData spellData)
    {
        this.attributes = spellData;
    }
    protected abstract void InitializeSpellData();



    // protected int N;
    // protected float delay;
    // protected int angle;
    // protected string projectileTrajectory;
    // protected int secondaryDamage;
    // protected string secondaryProjectile;
    // protected float spray;

    public virtual string GetName() => attributes.name ?? "Default: Bolt";
    public virtual string GetDescription() => attributes.description ?? "Default: A basic bolt of energy.";

    public virtual Damage.Type GetDamageType()
    {
        return attributes.damage.type;
    }
    public virtual int GetManaCost() => 10;
    public virtual int GetDamage()
    {
        int dmg = attributes.GetBaseDamage();
        return dmg > 0 ? dmg : 0;
    }
    public virtual float GetCooldown() {
        float cooldown = attributes.GetBaseCooldown();
        return cooldown > 0 ? cooldown : 0.75f;
    }
    public virtual int GetIcon() {
        int iconn = attributes.icon;
        return iconn > 0 ? iconn : 0;
    }
    public virtual float GetSpeed() {
        float speed = attributes.GetBaseSpeed();
        return speed > 0 ? speed : 0;
    }
    public int getAngle()
    {
        return angle;
    }
    public virtual string GetTrajectory() 
    {
        return attributes.trajectory;
    }
    public virtual float GetSize()
    {
        //return attributes.GetFinalSize();
        return attributes.size;
    }
    public float getSpeed()
    {
        return attributes.getBaseSpeed();
    }
    public float getCooldown()
    {
        return attributes.GetBaseCooldown();
    }
    public SpellData GetAttributes()
    {
        return attributes;
    }
    public void addToDamage(int damageAdded)
    {
        Debug.Log($"Calling innerspell.addToDamage. damageAdded should still be {damageAdded}");
        //this is causing errors
        attributes.addToDamage(damageAdded);
    }
    public void addToMana(int manaAdded)
    {
        attributes.addToDamage(manaAdded);
    }
    public void addToSpeed(float speedAdded)
    {
        attributes.addToSpeed(speedAdded);
    }
    public void addToAngle(int angleAdded)
    {
        angle += angleAdded;
    }
    public void addToCooldown(float cooldownAdded)
    {
        attributes.addToCooldown(cooldownAdded);
    }
    public virtual IEnumerator Cast(Vector3 where, Vector3 target, Hittable.Team team)
    {
        Debug.Log("Spell's Cast called");
        last_cast = Time.time;
        this.team = team;
         GameManager.Instance.projectileManager.CreateProjectile(
            attributes.projectileSprite, 
            GetTrajectory(), 
            where, 
            target - where, 
            GetSpeed(), 
            OnHit,
            lifetime,
            GetSize());
        yield return new WaitForEndOfFrame();
    }


    protected virtual void OnHit(Hittable other, Vector3 impact)
    {
        if (other.team != team)
        {
            int damage = GetDamage();
            Damage.Type damageType = attributes.GetDamageType();
            other.Damage(new Damage(damage, damageType));
        }
    }


    public virtual bool IsReady()
    {

        return last_cast + GetCooldown() < Time.time;
    }
}
