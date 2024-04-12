using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Creature")]
public class CreatureData : ScriptableObject
{
    public string typeName;
    //public int offence, defence, accuracy, evasion, luck, speed, energyOffence, energyDefence, support, stamina, vigour, vitality;
    public int[] stats = new int[12];
    public List<string> proficiency;
    public List<string> resistance;
    public List<string> vulnerability;

    public TechData tech1, tech2, tech3, tech4, tech5, tech6, tech7, tech8;

}
