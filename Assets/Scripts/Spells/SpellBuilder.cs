using UnityEngine;
using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;


public class SpellBuilder 
{

    public Spell Build(SpellCaster owner)
    {
        // uncomment these return statements to see the spells in action
        // return new ArcaneSpray(owner);
        // return new ArcaneBolt(owner);
        // return new MagicMissile(owner);
        return new ArcaneBlast(owner); // fixed
    }

   
    public SpellBuilder()
    {        
    }

}
