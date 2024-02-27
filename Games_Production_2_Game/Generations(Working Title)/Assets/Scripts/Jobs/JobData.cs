using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Job")]
public class JobData : ScriptableObject
{
    public string jobName;
    public int offence, defence, accuracy, evasion, luck, speed, energyOffence, energyDefence, support, stamina, vigour, vitality;
    public WeaponData[] weapon;
    public TechData[] tech;
}
