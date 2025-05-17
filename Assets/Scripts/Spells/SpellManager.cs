// using System.Collections.Generic;
// using UnityEngine;

// public class SpellManager : MonoBehaviour
// {
//     public static SpellManager Instance { get; private set; }

//     public SpellBuilder spellBuilder;

//     private void Awake()
//     {
//         if (Instance != null && Instance != this)
//         {
//             Destroy(this.gameObject);
//         }
//         else
//         {
//             Instance = this;
//         }
//     }

//     public Spell GenerateNewSpell(SpellCaster owner)
//     {
//         return spellBuilder.Build(owner);
//     }
// }
