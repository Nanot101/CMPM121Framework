using UnityEngine;
using System;

public class EventBus
{
    private static EventBus theInstance;
    public static EventBus Instance
    {
        get
        {
            if (theInstance == null)
                theInstance = new EventBus();
            return theInstance;
        }
    }

    public event Action<Vector3, Damage, Hittable> OnDamage;
    // public event Action<Hittable> OnKill;
    public event Action<Hittable, Hittable> OnKill;
    public event Action OnMove;
    public event Action<Vector3> OnStandStill;
    public event Action OnSpellCast;
    public void DoMove() => OnMove?.Invoke();
    public void DoStandStill(Vector3 pos) => OnStandStill?.Invoke(pos);
    public void DoSpellCast() => OnSpellCast?.Invoke();


    public void DoDamage(Vector3 where, Damage dmg, Hittable target)
    {
        OnDamage?.Invoke(where, dmg, target);
    }
    // public void DoKill(Hittable killer)
    // {
    //     OnKill?.Invoke(killer);
    // }

    public void DoKill(Hittable killer, Hittable killed) => OnKill?.Invoke(killer, killed);

}
