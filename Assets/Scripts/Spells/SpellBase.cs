using System.Collections;
using UnityEngine;


// Abstract base class for all spell types, defining shared behavior like casting, cooldowns, and mana usage.
public abstract class SpellBase
{
    public SpellCaster owner;
    // used for targeting logic
    public Hittable.Team team;
    // used for cooldown tracking
    protected float lastCastTime;
    public virtual float LastCast => lastCastTime;

    // Constructor to assign the spell's owner
    public SpellBase(SpellCaster owner)
    {
        this.owner = owner;
    }

    public abstract string GetName();
    public abstract int GetManaCost();
    public abstract int GetDamage();
    public abstract float GetCooldown();
    public abstract int GetIcon();

    // returns true if ready to be cast
    public bool IsReady()
    {
        return Time.time >= lastCastTime + GetCooldown();
    }

    // Attempts to cast the spell at the target location. 
    // If successful, deducts mana, sets cooldown, and starts the casting coroutine.
    public IEnumerator TryCast(Vector3 where, Vector3 target, Hittable.Team team)
    {
        // don't do it if low on mana or on cooldown
        if (!IsReady() || owner.mana < GetManaCost())
            yield break;

        lastCastTime = Time.time;
        owner.mana -= GetManaCost();
        this.team = team;

        yield return Cast(where, target, team);
    }

    // Abstract method to define the specific spell effect; must be implemented in derived classes
    public abstract IEnumerator Cast(Vector3 where, Vector3 target, Hittable.Team team);
}
