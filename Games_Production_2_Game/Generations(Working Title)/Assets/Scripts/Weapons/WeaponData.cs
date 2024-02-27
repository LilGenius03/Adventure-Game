using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponData : MonoBehaviour
{
    [CreateAssetMenu(menuName = "Weapon")]
    public class JobData : ScriptableObject
    {
        public string weaponName, weaponType;
        public int offence, defence, accuracy, evasion, luck, speed, energyOffence, energyDefence, support, stamina, vigour, vitality;
    }
}
