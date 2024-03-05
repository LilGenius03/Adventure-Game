using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterData : MonoBehaviour
{
    public string characterName;

    public List<int[]> stats;

    //Offence = 0, Defence = 1, Accuracy = 2, Evasion = 3, Luck = 4, Speed = 5, Energy Offence = 6, EnergyDefence = 7, Support = 8, Stamina = 9, Vigour = 10, Vitality = 11

    private int baseOffence = 5, baseDefence = 5, baseAccuracy = 5, baseEvasion = 5, baseLuck = 5, baseSpeed = 5, baseEnergyOffence = 5, baseEnergyDefence = 5, baseSupport = 5, baseStamina = 5, baseVigour = 5, baseVitality = 5;
    private int offence, defence, accuracy, evasion, luck, speed, energyOffence, energyDefence, support, stamina, vigour, vitality;
    private int offenceMod, defenceMod, accuracyMod, evasionMod, luckMod, speedMod, energyOffenceMod, energyDefenceMod, supportMod, staminaMod, vigourMod, vitalityMod;
    private int equipmentOffence, equipmentDefence, equipmentAccuracy, equipmentEvasion, equipmentLuck, equipmentSpeed, equipmentEnergyOffence, equipmentEnergyDefence, equipmentSupport, equipmentStamina, equipmentVigour, equipmentVitality;
    private int augmentedOffence, augmentedDefence, augmentedAccuracy, augmentedEvasion, augmentedLuck, augmentedSpeed, augmentedEnergyOffence, augmentedEnergyDefence, augmentedSupport, augmentedStamina, augmentedVigour, augmentedVitality;
    
    public int[] baseStats = new int[12] /*{baseOffence, baseDefence, baseAccuracy, baseEvasion, baseLuck, baseSpeed, baseEnergyOffence, baseEnergyDefence, baseSupport, baseStamina, baseVigour, baseVitality}*/;
    public int[] currentStats = new int[12] /*{offence, defence, accuracy, evasion, luck, speed, energyOffence, energyDefence, support, stamina, vigour, vitality}*/;
    public int[] statMods = new int[12] /*{offenceMod, defenceMod, accuracyMod, evasionMod, luckMod, speedMod, energyOffenceMod, energyDefenceMod, supportMod, staminaMod, vigourMod, vitalityMod}*/;
    public int[] equipmentStats = new int[12] /*{equipmentOffence, equipmentDefence, equipmentAccuracy, equipmentEvasion, equipmentLuck, equipmentSpeed, equipmentEnergyOffence, equipmentEnergyDefence, equipmentSupport, equipmentStamina, equipmentVigour, equipmentVitality}*/;
    public int[] augmentedStats = new int[12] /*{augmentedOffence, augmentedDefence, augmentedAccuracy, augmentedEvasion, augmentedLuck, augmentedSpeed, augmentedEnergyOffence, augmentedEnergyDefence, augmentedSupport, augmentedStamina, augmentedVigour, augmentedVitality}*/;

    public int level, maxBP, maxEP, maxHP, maxXP, BP, EP, HP, XP;
    public JobData primaryJob, secondaryJob;
    public EquipmentData headSlot, bodySlot, feetSlot, leftHandSlot, rightHandSlot, accessorySlot;
    
    public CharacterData(JobData job)
    {
        primaryJob = job;

        characterName = job.characterName;

        stats.Add(baseStats);
        stats.Add(currentStats);
        stats.Add(statMods);
        stats.Add(equipmentStats);
        stats.Add(augmentedStats);

        level = 1;

        for (int i = 0; i < baseStats.Length; i++)
        {
            baseStats[i] = (5 + job.stats[i]) * level;
        }
        baseStats[9] = baseStats[9] * 5;
        baseStats[10] = baseStats[10] * 2;
        baseStats[11] = baseStats[11] * 10;

        for (int i = 0; i < currentStats.Length; i++)
        {
            currentStats[i] = baseStats[i];
        }
        maxBP = baseStats[9];
        maxEP = baseStats[10];
        maxHP = baseStats[11];
        maxXP = (level*level) * 100;
    }

    public void LevelUp(int EXP)
    {
        XP += EXP;
        if(XP >= maxXP)
        {
            XP -= maxXP;
            level++;

            for (int i = 0; i < baseStats.Length; i++)
            {
                if(secondaryJob == null)
                {
                    baseStats[i] = (5 + primaryJob.stats[i]) * level;
                }
                else
                {
                    baseStats[i] = (5 + primaryJob.stats[i] + secondaryJob.stats[i]) * level;
                }
            }
            baseStats[9] = baseStats[9] * 5;
            baseStats[10] = baseStats[10] * 2;
            baseStats[11] = baseStats[11] * 10;

            for (int i = 0; i < currentStats.Length; i++)
            {
                currentStats[i] = baseStats[i];
                currentStats[i] += statMods[i];
                currentStats[i] += equipmentStats[i];
            }
            maxBP = baseStats[9];
            maxEP = baseStats[10];
            maxHP = baseStats[11];
            maxXP = (level*level) * 100;
        }
    }

    public void ModifyStat(int stat, int amount)
    {
        statMods[stat] += amount;

        currentStats[stat] += amount;
    }

    public void EquipItem(EquipmentData equip)
    {
        if (equip.equipped == true)
        {
            switch (equip.slot)
            {
                case "Head":
                {
                    if(headSlot == null)
                    {
                        equip.equipped = true;
                        headSlot = equip;

                        for(int i = 0; i < headSlot.stats.Length; i++)
                        {
                            equipmentStats[i] += headSlot.stats[i];
                        }
                    }
                    else
                    {
                        for(int i = 0; i < headSlot.stats.Length; i++)
                        {
                            equipmentStats[i] -= headSlot.stats[i];
                        }
                        //Return old headSlot equip to inventory
                        headSlot.equipped = false;
                        
                        equip.equipped = true;
                        headSlot = equip;

                        for(int i = 0; i < headSlot.stats.Length; i++)
                        {
                            equipmentStats[i] += headSlot.stats[i];
                        }
                    }
                    break;
                }

                default:
                {
                    break;
                }
            }
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
