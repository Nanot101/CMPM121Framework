using System.Collections.Generic;

[System.Serializable]
public class DamageData
{
    public string amount;
    public Damage.Type type;
}

[System.Serializable]
public class ProjectileData
{
    public string trajectory;
    public string speed;
    public int sprite;
    public string lifetime;
}

[System.Serializable]
public class SpellData
{
    public string name;
    public string description;
    public int icon;
    public int mana_cost;
    public float cooldown;
    public string N;
    public string spray;
    public DamageData damage;
    public string secondary_damage;
    public ProjectileData projectile;
    public ProjectileData secondary_projectile;
}


[System.Serializable]
public class ModifierData
{
    public string name;
    public string description;
    public string damage_multiplier;
    public string mana_multiplier;
    public string speed_multiplier;
    public string cooldown_multiplier;
    public string mana_adder;
    public string delay;
    public string angle;
    public string projectile_trajectory;
}
