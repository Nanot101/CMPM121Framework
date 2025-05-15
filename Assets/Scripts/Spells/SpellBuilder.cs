// using UnityEngine;
// using System.IO;
// using Newtonsoft.Json.Linq;
// using Newtonsoft.Json;
// using System.Collections.Generic;
// using System.Linq;
// using System.Collections;

// public class SpellBuilder : MonoBehaviour
// {
//     public SpellLoader spellsAndModifiers;
//     List<ModifierSpell> modifiers;
//     Spell baseSpell;
//     public Spell Build(SpellCaster owner)
//     {
//         // uncomment these return statements to see the spells in action
//         while (true)
//         {
//             //ModifierSpell lastModifier = null;
//             float spellOrModifier = Random.value * 2;
//             modifiers = new List<ModifierSpell>();
//             if (spellOrModifier > 1)
//             {
//                 //Spell base
//                 //int numSpells = 4;
//                 int rand = (int)(Random.value * spellsAndModifiers.getSpellDict().Count);
//                 string[] keys = spellsAndModifiers.getSpellDict().Keys.ToArray();
//                 string spellName = keys[rand];
//                 // string spellName = "magic_missile";
//                 //baseSpell = spellsAndModifiers.getSpellDict()[spellName];
//                 switch (spellName)
//                 {
//                     case "arcane_bolt":
//                         baseSpell = new ArcaneBolt(owner);
//                     break;
//                     case "magic_missile":
//                         baseSpell = new MagicMissile(owner);
//                     break;
//                     case "arcane_blast":
//                         baseSpell = new ArcaneBlast(owner);
//                     break;
//                     case "arcane_spray":
//                         baseSpell = new ArcaneSpray(owner);
//                     break;
//                 }
//                 Spell recentSpell = baseSpell;
                
//                 if (modifiers != null)
//                 {
//                     string descriptionWithModifiers = "";
//                     foreach (ModifierSpell modifier in modifiers)
//                     {
//                         modifier.setInnerSpell(recentSpell);
//                         modifier.ApplyModifiers();
//                         descriptionWithModifiers += modifier.GetDescription();
//                         recentSpell = modifier.GetSpellBase();
//                     }
//                 }
//                 return recentSpell;
//             }
//             else
//             {
//                 int rand = (int)(Random.value * spellsAndModifiers.getModifierDict().Count);
//                 string[] keys = spellsAndModifiers.getModifierDict().Keys.ToArray();
//                 string modifierName = keys[rand];

//                 //modifiers.Add(SpellLoader.Modifiers[modifierName]);
//                 //        public ModifierSpell(Spell innerSpell, SpellCaster owner, float dmgMlt, int dmgAdd, float manaMlt, int manaAdd, float speedMlt, int speedAdd, int angl, string trajectory, float timeBetweenShots, float cooldownMlt, string descrp) : base(owner)

//                 Debug.Log($"Applying {modifierName}");
//                 switch (modifierName)
//                 {
//                     case "damage_amp":
//                         ModifierSpell dAmp = new ModifierSpell(null, owner, 1.5f, 0, 1.5f, 0, 1, 0, 0, "straight", 0, 1, "Increased damage and increased mana cost.");
//                         modifiers.Add(dAmp);
//                         Debug.Log($"Applying {dAmp}");
//                         break;
//                     case "speed_amp":
//                         ModifierSpell speedAmp = new ModifierSpell(null, owner, 1, 0, 1, 0, 1.75f, 0, 0, "straight", 0, 1, "Faster projectile speed.");
//                         modifiers.Add(speedAmp);
//                         Debug.Log($"Applying {speedAmp}");
//                         break;
//                     case "doubler":
//                         ModifierSpell doubler = new ModifierSpell(null, owner, 1, 0, 1, 0, 1, 0, 0, "straight", 0, 1.5f, "Spell is cast a second time after a small delay; increased mana cost and cooldown.");
//                         modifiers.Add(doubler);
//                         Debug.Log($"Applying {doubler}");
//                         break;
//                     case "splitter":
//                         ModifierSpell splitter = new ModifierSpell(null, owner, 1, 0, 1.5f, 0, 1, 0, 10, "straight", 0, 1.5f, "Spell is cast twice in slightly different directions; increased mana cost.");
//                         modifiers.Add(splitter);
//                         Debug.Log($"Applying {splitter}");
//                         break;
//                     case "chaos":
//                         ModifierSpell chaos = new ModifierSpell(null, owner, 6.5f, 0, 1, 0, 1, 0, 0, "spiraling", 0, 1.5f, "Significantly increased damage, but projectile is spiraling.");
//                         modifiers.Add(chaos);
//                         Debug.Log($"Applying {chaos}");
//                         break;
//                     case "homing":
//                         ModifierSpell homing = new ModifierSpell(null, owner, 0.75f, 0, 1, 10, 1, 0, 0, "homing", 0, 1.5f, "Homing projectile, with decreased damage and increased mana cost.");
//                         modifiers.Add(homing);
//                         Debug.Log($"Applying {homing}");
//                         break;
//                 }
                
//             }
//         }
//         //return new ArcaneSpray(owner);
//         // return new ArcaneBolt(owner);
//         // return new MagicMissile(owner);
//         // return new ArcaneBlast(owner); // Buggy, dont uncomment this
//     }

   
//     public SpellBuilder()
//     {
//     }

// }



using UnityEngine;
using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

public class SpellBuilder : MonoBehaviour
{
    public SpellLoader spellsAndModifiers;
    List<ModifierSpell> modifiers;
    Spell baseSpell;
    public Spell Build(SpellCaster owner)
    {
        // uncomment these return statements to see the spells in action
        modifiers = new List<ModifierSpell>();
        // Debug.Log(modifiers);
        while (true)
        {
            //ModifierSpell lastModifier = null;
            float spellOrModifier = Random.value * 2;
            
            if (spellOrModifier > 1)
            {
                //Spell base
                //int numSpells = 4;
                int rand = (int)(Random.value * spellsAndModifiers.getSpellDict().Count);
                string[] keys = spellsAndModifiers.getSpellDict().Keys.ToArray();
                string spellName = keys[rand];
                // string spellName = "magic_missile";
                //baseSpell = spellsAndModifiers.getSpellDict()[spellName];
                spellName = "arcane_bolt";
                switch (spellName)
                {
                    case "arcane_bolt":
                        baseSpell = new ArcaneBolt(owner);
                    break;
                    case "magic_missile":
                        baseSpell = new MagicMissile(owner);
                    break;
                    case "arcane_blast":
                        baseSpell = new ArcaneBlast(owner);
                    break;
                    case "arcane_spray":
                        baseSpell = new ArcaneSpray(owner);
                    break;
                    case "arcane_pulse":
                        baseSpell = new ArcanePulse(owner, 0.5f);
                        break;
                }
                Spell recentSpell = baseSpell;
                int modifierLength = modifiers.Count;
                if (modifierLength != 0)
                {
                    string descriptionWithModifiers = "";
                    Debug.Log("Inside if (!modifiers.Any())...");
                    // foreach (ModifierSpell modifier in modifiers)
                    for (int i = 0; i < modifiers.Count - 1; i++)
                    {
                        Debug.Log("Inside for loop");
                        ModifierSpell modifier = modifiers[i];
                        modifier.setInnerSpell(recentSpell);
                        Debug.Log("Calling apply modifiers");
                        modifier.ApplyModifiers();
                        Debug.Log("apply modifiers finished");
                        descriptionWithModifiers += modifier.GetDescription();
                        recentSpell = modifier.GetSpellBase();
                    }

                    ModifierSpell LastModifier = modifiers[modifierLength - 1];
                    LastModifier.setInnerSpell(recentSpell);
                    Debug.Log(LastModifier);
                    return LastModifier;
                }
                else {
                    Debug.LogError("Modifier list is not found");
                }
                return recentSpell;
            }
            else
            {
                int rand = (int)(Random.value * spellsAndModifiers.getModifierDict().Count);
                string[] keys = spellsAndModifiers.getModifierDict().Keys.ToArray();
                // string modifierName = keys[rand];
                string modifierName = "damage_amp";

                //modifiers.Add(SpellLoader.Modifiers[modifierName]);
                //        public ModifierSpell(Spell innerSpell, SpellCaster owner, float dmgMlt, int dmgAdd, float manaMlt, int manaAdd, float speedMlt, int speedAdd, int angl, string trajectory, float timeBetweenShots, float cooldownMlt, string descrp) : base(owner)

                //Debug.Log($"Applying {modifierName}");
                switch (modifierName)
                {
                    case "damage_amp":
                        ModifierSpell dAmp = new ModifierSpell(null, owner, 1.5f, 0, 1.5f, 0, 1, 0, 0, "straight", 0, 1, "Increased damage and increased mana cost.");
                        modifiers.Add(dAmp);
                        Debug.Log($"Applied damage_amp");
                        break;
                    case "speed_amp":
                        ModifierSpell speedAmp = new ModifierSpell(null, owner, 1, 0, 1, 0, 1.75f, 0, 0, "straight", 0, 1, "Faster projectile speed.");
                        modifiers.Add(speedAmp);
                        Debug.Log($"Applied speed_amp");
                        break;
                    case "doubler":
                        ModifierSpell doubler = new ModifierSpell(null, owner, 1, 0, 1, 0, 1, 0, 0, "straight", 0, 1.5f, "Spell is cast a second time after a small delay; increased mana cost and cooldown.");
                        modifiers.Add(doubler);
                        Debug.Log($"Applied doubler");
                        break;
                    case "splitter":
                        ModifierSpell splitter = new ModifierSpell(null, owner, 1, 0, 1.5f, 0, 1, 0, 10, "straight", 0, 1.5f, "Spell is cast twice in slightly different directions; increased mana cost.");
                        modifiers.Add(splitter);
                        Debug.Log($"Applied splitter");
                        break;
                    case "chaos":
                        ModifierSpell chaos = new ModifierSpell(null, owner, 6.5f, 0, 1, 0, 1, 0, 0, "spiraling", 0, 1.5f, "Significantly increased damage, but projectile is spiraling.");
                        modifiers.Add(chaos);
                        Debug.Log($"Applied chaos");
                        break;
                    case "homing":
                        ModifierSpell homing = new ModifierSpell(null, owner, 0.75f, 0, 1, 10, 1, 0, 0, "homing", 0, 1.5f, "Homing projectile, with decreased damage and increased mana cost.");
                        modifiers.Add(homing);
                        Debug.Log($"Applied homing");
                        break;
                }
                
            }
        }
        //return new ArcaneSpray(owner);
        // return new ArcaneBolt(owner);
        // return new MagicMissile(owner);
        // return new ArcaneBlast(owner); // Buggy, dont uncomment this
    }

   
    public SpellBuilder()
    {
    }

}