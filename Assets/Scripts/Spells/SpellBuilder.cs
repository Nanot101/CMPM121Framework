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
    List<ModifierData> modifiers;
    Spell baseSpell;
    public Spell Build(SpellCaster owner)
    {
        // uncomment these return statements to see the spells in action
        modifiers = new List<ModifierData>();
        while (true)
        {
            float spellOrModifier = Random.value * 2;
            if (spellOrModifier > 1)
            {
                //Spell base
                //int numSpells = 4;
                int rand = (int)(Random.value * spellsAndModifiers.getSpellDict().Count);
                string[] keys = spellsAndModifiers.getSpellDict().Keys.ToArray();
                string spellName = keys[rand];
                //baseSpell = spellsAndModifiers.getSpellDict()[spellName];
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
                        baseSpell = new ArcanePulse(owner);
                    break;
                }
                return baseSpell;
            }
            else
            {
                int rand = (int)(Random.value * spellsAndModifiers.getModifierDict().Count);
                string[] keys = spellsAndModifiers.getModifierDict().Keys.ToArray();
                string modifierName = keys[rand];
                modifiers.Add(spellsAndModifiers.getModifierDict()[modifierName]);

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
