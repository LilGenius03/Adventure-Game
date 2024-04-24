using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Tech")]
[System.Serializable]
public class TechData : ScriptableObject
{
    public string title, description, target, effect, type, subtarget, subeffect, subtype;
    public int level, health, energy, boosts;
    public GameObject summon;

    // Name = 0, description = 1, target = 2, effect = 3, type = 4, sub-target = 5, sub-effect = 6, sub-type = 7

    // targets {enemy, enemies, ally, allies, self, dead}
    // effects {physical Hit, energy Hit, buff, debuff, Heal, Hurt}
    // types: Physical Hit {Fist, Club, Knife, Staff, Sword, Spear, Axe, Hammer, Scythe, Flail, Bow, Throw}
    // types: Energy Hit {Air, Earth, Fire, Water, Ice, Electricity, Psychic, Poison, Radiance, Shadow, Sound, Force}
    // types: Buff {Offence, Defence, Accuracy, Evasion, Luck, Speed, EnergyOffence, EnergyDefence, Support, Stamina, Vigour, Vitality}
    // types: Debuff {Offence, Defence, Accuracy, Evasion, Luck, Speed, EnergyOffence, EnergyDefence, Support, Stamina, Vigour, Vitality}
    // types: Heal {BP, EP, HP}
    // types: Hurt {BP, EP, HP}
}
