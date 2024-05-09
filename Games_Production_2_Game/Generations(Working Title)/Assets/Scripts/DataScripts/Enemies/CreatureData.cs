using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Creature")]
[System.Serializable]
public class CreatureData : ScriptableObject
{
    public string typeName;
    //public int offence, defence, accuracy, evasion, luck, speed, energyOffence, energyDefence, support, stamina, vigour, vitality;
    public int[] stats = new int[12];
    public List<string> resistance;
    public List<string> vulnerability;
    public List<Sprite> resistanceIcon;
    public List<Sprite> vulnerabilityIcon;
    public List<TechData> techs;
}
