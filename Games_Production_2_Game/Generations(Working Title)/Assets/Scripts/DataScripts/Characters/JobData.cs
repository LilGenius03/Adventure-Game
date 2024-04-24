using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Job")]
[System.Serializable]
public class JobData : ScriptableObject
{
    public string characterName, jobName;
    //public int offence, defence, accuracy, evasion, luck, speed, energyOffence, energyDefence, support, stamina, vigour, vitality;
    public int[] stats = new int[12];
    public List<string> proficiency;
    public List<string> resistance;
    public List<string> vulnerability;

    public List<TechData> techs;
}
