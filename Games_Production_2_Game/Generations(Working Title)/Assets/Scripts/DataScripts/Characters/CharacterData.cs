using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterData : MonoBehaviour
{
    public string characterName;

    public List<int[]> stats;

    //Offence = 0, Defence = 1, Accuracy = 2, Evasion = 3, Luck = 4, Speed = 5, Energy Offence = 6, EnergyDefence = 7, Support = 8, Stamina = 9, Vigour = 10, Vitality = 11
    
    private int[] baseStats = new int[12] /*{baseOffence, baseDefence, baseAccuracy, baseEvasion, baseLuck, baseSpeed, baseEnergyOffence, baseEnergyDefence, baseSupport, baseStamina, baseVigour, baseVitality}*/;
    private int[] statMods = new int[12] /*{offenceMod, defenceMod, accuracyMod, evasionMod, luckMod, speedMod, energyOffenceMod, energyDefenceMod, supportMod, staminaMod, vigourMod, vitalityMod}*/;
    private int[] equipmentStats = new int[12] /*{equipmentOffence, equipmentDefence, equipmentAccuracy, equipmentEvasion, equipmentLuck, equipmentSpeed, equipmentEnergyOffence, equipmentEnergyDefence, equipmentSupport, equipmentStamina, equipmentVigour, equipmentVitality}*/;
    private int[] totalStats = new int[12] /*{offence, defence, accuracy, evasion, luck, speed, energyOffence, energyDefence, support, stamina, vigour, vitality}*/;
    private int[] augmentedStats = new int[12] /*{augmentedOffence, augmentedDefence, augmentedAccuracy, augmentedEvasion, augmentedLuck, augmentedSpeed, augmentedEnergyOffence, augmentedEnergyDefence, augmentedSupport, augmentedStamina, augmentedVigour, augmentedVitality}*/;

    public int level, maxBP, maxEP, maxHP, maxXP, BP, EP, HP, XP, boost;
    public JobData primaryJob, secondaryJob;
    public WeaponData weaponSlot;
    public ArmourData shieldSlot, headSlot, bodySlot, feetSlot, accessorySlot;
    public GameObject summon;
    public Transform summonPos;
    //public EquipmentData[] slots = new EquipmentData[6];

    public List<string> proficiency;
    public List<string> resistance;
    public List<string> vulnerability;
    public List<TechData> techs;
    public CombattantScript combattantScript;

    public int Offence()
    {
        return augmentedStats[0];
    }

    public int Defence()
    {
        return augmentedStats[1];
    }

    public int Accuracy()
    {
        return augmentedStats[2];
    }

    public int Evasion()
    {
        return augmentedStats[3];
    }

    public int Luck()
    {
        return augmentedStats[4];
    }

    public int Speed()
    {
        return augmentedStats[5];
    }

    public int EnergyOffence()
    {
        return augmentedStats[6];
    }

    public int EnergyDefence()
    {
        return augmentedStats[7];
    }

    public int Support()
    {
        return augmentedStats[8];
    }

    public int Stamina()
    {
        return augmentedStats[9];
    }

    public int Vigour()
    {
        return augmentedStats[10];
    }

    public int Vitality()
    {
        return augmentedStats[11];
    }
    
    public CharacterData(JobData job, int lv)
    {
        characterName = job.characterName;
        primaryJob = job;
        level = lv;

        stats.Add(baseStats);
        stats.Add(statMods);
        stats.Add(equipmentStats);
        stats.Add(totalStats);
        stats.Add(augmentedStats);

        foreach(string item in job.proficiency)
        {
            proficiency.Add(item);
        }

        foreach(string item in job.resistance)
        {
            resistance.Add(item);
        }

        foreach(string item in job.vulnerability)
        {
            vulnerability.Add(item);
        }

        foreach(TechData item in job.techs)
        {
            if (level >= item.level)
            {
                techs.Add(item);
            }
        }

        for (int i = 0; i < baseStats.Length; i++)
        {
            baseStats[i] = (5 + job.stats[i]) * level;
        }
        baseStats[9] = baseStats[9] * 5;
        baseStats[10] = baseStats[10] * 2;
        baseStats[11] = baseStats[11] * 10;

        for (int i = 0; i < totalStats.Length; i++)
        {
            totalStats[i] = baseStats[i];
            augmentedStats[i] = totalStats[i];
        }
        maxBP = totalStats[9];
        maxEP = totalStats[10];
        maxHP = totalStats[11];
        maxXP = (level*level) * 100;

        combattantScript = new CombattantScript();
    }

    public void LevelUp(int EXP)
    {
        XP += EXP;

        for (int i = 0; i < totalStats.Length; i++)
        {
            ResetStat(i);
        }

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

            for (int i = 0; i < totalStats.Length; i++)
            {
                totalStats[i] = baseStats[i];
                totalStats[i] += statMods[i];
                totalStats[i] += equipmentStats[i];
                augmentedStats[i] = totalStats[i];
            }
            maxBP = totalStats[9];
            maxEP = totalStats[10];
            maxHP = totalStats[11];
            maxXP = (level*level) * 10;

            foreach(TechData item in primaryJob.techs)
            {
                bool learnt = false;

                foreach (TechData tech in techs)
                {
                    if(item == tech)
                    {
                        learnt = true;
                    }
                }

                if (level >= item.level && !learnt)
                {
                    techs.Add(item);
                }
            }

            foreach(TechData item in secondaryJob.techs)
            {
                bool learnt = false;

                foreach (TechData tech in techs)
                {
                    if(item == tech)
                    {
                        learnt = true;
                    }
                }

                if (level >= item.level && !learnt)
                {
                    techs.Add(item);
                }
            }

            if(XP >= maxXP)
            {
                LevelUp(0);
            }
        }
    }

    public void NewJob(JobData newJob)
    {
        RemoveJob();

        for (int i = 0; i < baseStats.Length; i++)
        {
            baseStats[i] = (5 + primaryJob.stats[i] + secondaryJob.stats[i]) * level;
        }
        baseStats[9] = baseStats[9] * 5;
        baseStats[10] = baseStats[10] * 2;
        baseStats[11] = baseStats[11] * 10;

        for (int i = 0; i < totalStats.Length; i++)
        {
            totalStats[i] = baseStats[i];
            totalStats[i] += statMods[i];
            totalStats[i] += equipmentStats[i];
            augmentedStats[i] = totalStats[i];
        }
        maxBP = totalStats[9];
        maxEP = totalStats[10];
        maxHP = totalStats[11];

        foreach(string item in newJob.proficiency)
        {
            bool proficient = false;

            foreach(string prof in proficiency)
            {
                if(prof == item)
                {
                    proficient = true;
                }
            }

            if(proficient != true)
            {
                proficiency.Add(item);
            }
        }

        foreach(string item in newJob.resistance)
        {
            bool resistant = false;

            foreach(string res in resistance)
            {
                if(res == item)
                {
                    resistant = true;
                }
            }

            if(resistant != true)
            {
                resistance.Add(item);
            }
        }

        foreach(string item in newJob.vulnerability)
        {
            bool vulnerable = false;

            foreach(string vul in vulnerability)
            {
                if(vul == item)
                {
                    vulnerable = true;
                }
            }

            if(vulnerable != true)
            {
                vulnerability.Add(item);
            }
        }

        foreach(string res in resistance)
        {
            bool cancel = false;

            foreach(string vul in vulnerability)
            {
                if (res == vul)
                {
                    cancel = true;
                }
            }

            if(cancel)
            {
                resistance.Remove(res);
                vulnerability.Remove(res);
            }
        }

        foreach(TechData item in newJob.techs)
        {
            bool learnt = false;

            foreach (TechData tech in techs)
            {
                if(item == tech)
                {
                    learnt = true;
                }
            }

            if (level >= item.level && !learnt)
            {
                techs.Add(item);
            }
        }
    }

    public void RemoveJob()
    {
        if(secondaryJob != null)
        {
            for (int i = 0; i < baseStats.Length; i++)
            {
                baseStats[i] = (5 + primaryJob.stats[i]) * level;
            }
            baseStats[9] = baseStats[9] * 5;
            baseStats[10] = baseStats[10] * 2;
            baseStats[11] = baseStats[11] * 10;

            for (int i = 0; i < totalStats.Length; i++)
            {
                totalStats[i] = baseStats[i];
                totalStats[i] += statMods[i];
                totalStats[i] += equipmentStats[i];
                augmentedStats[i] = totalStats[i];
            }
            maxBP = totalStats[9];
            maxEP = totalStats[10];
            maxHP = totalStats[11];

            foreach(string item in secondaryJob.proficiency)
            {
                bool proficient = false;

                foreach(string prof in primaryJob.proficiency)
                {
                    if(prof == item)
                    {
                        proficient = true;
                    }
                }

                if(proficient != true)
                {
                    proficiency.Remove(item);
                }
            }

            foreach(string item in secondaryJob.resistance)
            {
                bool resistant = false;

                foreach(string res in primaryJob.resistance)
                {
                    if(res == item)
                    {
                        resistant = true;
                    }
                }

                if(resistant != true)
                {
                    resistance.Remove(item);
                }
            }

            foreach(string item in secondaryJob.vulnerability)
            {
                bool vulnerable = false;

                foreach(string vul in primaryJob.vulnerability)
                {
                    if(vul == item)
                    {
                        vulnerable = true;
                    }
                }

                if(vulnerable != true)
                {
                    vulnerability.Remove(item);
                }
            }

            foreach(string item in primaryJob.resistance)
            {
                bool confirm = true;

                foreach(string res in resistance)
                {
                    if (item == res)
                    {
                        confirm = false;
                    }
                }

                if(confirm)
                {
                    resistance.Add(item);
                }
            }

            foreach(string item in primaryJob.vulnerability)
            {
                bool confirm = true;

                foreach(string vul in vulnerability)
                {
                    if (item == vul)
                    {
                        confirm = false;
                    }
                }

                if(confirm)
                {
                    vulnerability.Add(item);
                }
            }

            foreach(TechData item in secondaryJob.techs)
            {
                bool forget = false;

                foreach (TechData tech in techs)
                {
                    if(item == tech)
                    {
                        forget = true;
                    }
                }

                foreach (TechData tech in primaryJob.techs)
                {
                    if(item == tech)
                    {
                        forget = false;
                    }
                }

                if (forget)
                {
                    techs.Remove(item);
                }
            }

            secondaryJob = null;
        }
    }

    public void ModifyStat(int amount, int stat)
    {
        if(stat == 9)
        {
            amount = amount * 5;
            statMods[stat] += amount;
            maxBP += amount;
            totalStats[stat] += amount;
            augmentedStats[stat] = totalStats[stat];
        }
        if(stat == 10)
        {
            amount = amount * 2;
            statMods[stat] += amount;
            maxEP += amount;
            totalStats[stat] += amount;
            augmentedStats[stat] = totalStats[stat];
        }
        if(stat == 11)
        {
            amount = amount * 10;
            statMods[stat] += amount;
            maxHP += amount;
            totalStats[stat] += amount;
            augmentedStats[stat] = totalStats[stat];
        }
        else
        {
            statMods[stat] += amount;
            totalStats[stat] += amount;
            augmentedStats[stat] = totalStats[stat];
        }
    }

    public void EquipWeapon(WeaponData weapon)
    {
        bool equipable = false;

        foreach (string item in proficiency)
        {
            if(weapon.type == item)
            {
                equipable = true;
            }
        }

        if(equipable)
        {
            if (weaponSlot.equipped == true)
            {
                for(int i = 0; i < equipmentStats.Length; i++)
                {
                    equipmentStats[i] -= weaponSlot.data.stats[i];
                    totalStats[i] -= weaponSlot.data.stats[i];
                    augmentedStats[i] -= weaponSlot.data.stats[i];
                }

                weaponSlot.equipped = false;
                weaponSlot = weapon;
                weaponSlot.equipped = true;

                for(int i = 0; i < equipmentStats.Length; i++)
                {
                    equipmentStats[i] += weaponSlot.data.stats[i];
                    totalStats[i] += weaponSlot.data.stats[i];
                    augmentedStats[i] += weaponSlot.data.stats[i];
                }
            }
            else
            {
                weaponSlot = weapon;
                weaponSlot.equipped = true;

                for(int i = 0; i < equipmentStats.Length; i++)
                {
                    equipmentStats[i] += weaponSlot.data.stats[i];
                    totalStats[i] += weaponSlot.data.stats[i];
                    augmentedStats[i] += weaponSlot.data.stats[i];
                }
            }
        }
    }

    public void EquipArmour(ArmourData armour)
    {
        switch (armour.type)
        {
            case "Shield":
            {
                if(shieldSlot.equipped == true)
                {
                    for(int i = 0; i < equipmentStats.Length; i++)
                    {
                        equipmentStats[i] -= shieldSlot.data.stats[i];
                        totalStats[i] -= shieldSlot.data.stats[i];
                        augmentedStats[i] -= shieldSlot.data.stats[i];
                    }

                    shieldSlot.equipped = false;
                    shieldSlot = armour;
                    shieldSlot.equipped = true;

                    for(int i = 0; i < equipmentStats.Length; i++)
                    {
                        equipmentStats[i] += shieldSlot.data.stats[i];
                        totalStats[i] += shieldSlot.data.stats[i];
                        augmentedStats[i] += shieldSlot.data.stats[i];
                    }
                }
                else
                {
                    shieldSlot = armour;
                    shieldSlot.equipped = true;

                    for(int i = 0; i < equipmentStats.Length; i++)
                    {
                        equipmentStats[i] += shieldSlot.data.stats[i];
                        totalStats[i] += shieldSlot.data.stats[i];
                        augmentedStats[i] += shieldSlot.data.stats[i];
                    }
                }
                break;
            }
            
            case "Head":
            {
                if(headSlot.equipped == true)
                {
                    for(int i = 0; i < equipmentStats.Length; i++)
                    {
                        equipmentStats[i] -= headSlot.data.stats[i];
                        totalStats[i] -= headSlot.data.stats[i];
                        augmentedStats[i] -= headSlot.data.stats[i];
                    }

                    headSlot.equipped = false;
                    headSlot = armour;
                    headSlot.equipped = true;

                    for(int i = 0; i < equipmentStats.Length; i++)
                    {
                        equipmentStats[i] += headSlot.data.stats[i];
                        totalStats[i] += headSlot.data.stats[i];
                        augmentedStats[i] += headSlot.data.stats[i];
                    }
                }
                else
                {
                    headSlot = armour;
                    headSlot.equipped = true;

                    for(int i = 0; i < equipmentStats.Length; i++)
                    {
                        equipmentStats[i] += headSlot.data.stats[i];
                        totalStats[i] += headSlot.data.stats[i];
                        augmentedStats[i] += headSlot.data.stats[i];
                    }
                }
                break;
            }

            case "Body":
            {
                if(bodySlot.equipped == true)
                {
                    for(int i = 0; i < equipmentStats.Length; i++)
                    {
                        equipmentStats[i] -= bodySlot.data.stats[i];
                        totalStats[i] -= bodySlot.data.stats[i];
                        augmentedStats[i] -= bodySlot.data.stats[i];
                    }

                    bodySlot.equipped = false;
                    bodySlot = armour;
                    bodySlot.equipped = true;

                    for(int i = 0; i < equipmentStats.Length; i++)
                    {
                        equipmentStats[i] += bodySlot.data.stats[i];
                        totalStats[i] += bodySlot.data.stats[i];
                        augmentedStats[i] += bodySlot.data.stats[i];
                    }
                }
                else
                {
                    bodySlot = armour;
                    bodySlot.equipped = true;

                    for(int i = 0; i < equipmentStats.Length; i++)
                    {
                        equipmentStats[i] += bodySlot.data.stats[i];
                        totalStats[i] += bodySlot.data.stats[i];
                        augmentedStats[i] += bodySlot.data.stats[i];
                    }
                }
                break;
            }

            case "Feet":
            {
                if(feetSlot.equipped == true)
                {
                    for(int i = 0; i < equipmentStats.Length; i++)
                    {
                        equipmentStats[i] -= feetSlot.data.stats[i];
                        totalStats[i] -= feetSlot.data.stats[i];
                        augmentedStats[i] -= feetSlot.data.stats[i];
                    }

                    feetSlot.equipped = false;
                    feetSlot = armour;
                    feetSlot.equipped = true;

                    for(int i = 0; i < equipmentStats.Length; i++)
                    {
                        equipmentStats[i] += feetSlot.data.stats[i];
                        totalStats[i] += feetSlot.data.stats[i];
                        augmentedStats[i] += feetSlot.data.stats[i];
                    }
                }
                else
                {
                    feetSlot = armour;
                    feetSlot.equipped = true;

                    for(int i = 0; i < equipmentStats.Length; i++)
                    {
                        equipmentStats[i] += feetSlot.data.stats[i];
                        totalStats[i] += feetSlot.data.stats[i];
                        augmentedStats[i] += feetSlot.data.stats[i];
                    }
                }
                break;
            }

            case "Accessory":
            {
                if(accessorySlot.equipped == true)
                {
                    for(int i = 0; i < equipmentStats.Length; i++)
                    {
                        equipmentStats[i] -= accessorySlot.data.stats[i];
                        totalStats[i] -= accessorySlot.data.stats[i];
                        augmentedStats[i] -= accessorySlot.data.stats[i];
                    }

                    accessorySlot.equipped = false;
                    accessorySlot = armour;
                    accessorySlot.equipped = true;

                    for(int i = 0; i < equipmentStats.Length; i++)
                    {
                        equipmentStats[i] += accessorySlot.data.stats[i];
                        totalStats[i] += accessorySlot.data.stats[i];
                        augmentedStats[i] += accessorySlot.data.stats[i];
                    }
                }
                else
                {
                    accessorySlot = armour;
                    accessorySlot.equipped = true;

                    for(int i = 0; i < equipmentStats.Length; i++)
                    {
                        equipmentStats[i] += accessorySlot.data.stats[i];
                        totalStats[i] += accessorySlot.data.stats[i];
                        augmentedStats[i] += accessorySlot.data.stats[i];
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

    public void RollInitiative()
    {
        combattantScript.speedStat = totalStats[6];
    }

    public void Boost(int bp)
    {
        if(bp < 0 && (boost + bp) >= 0)
        {
            boost += bp;
        }
        else if(bp > 0 && (boost + bp) <= BP)
        {
            boost += bp;
        }
    }

    public int HealHP(int healing)
    {
        HP += healing;

        if(HP > maxHP)
        {
            HP = maxHP;
        }
        
        return (healing / 10);
    }

    public int HealEP(int healing)
    {
        EP += healing;

        if(EP > maxEP)
        {
            EP = maxEP;
        }
        
        return (healing / 2);
    }

    public int HealBP(int healing)
    {
        BP += healing;

        if(BP > maxBP)
        {
            BP = maxBP;
        }
        
        return (healing / 5);
    }

    public int HarmHP(int harming)
    {
        HP -= (harming / 10);

        if(HP < 0)
        {
            HP = 0;
        }
        
        return (harming);
    }

    public int HarmEP(int harming)
    {
        EP -= (harming / 2);

        if(EP < 0)
        {
            EP = 0;
        }
        
        return (harming);
    }

    public int HarmBP(int harming)
    {
        BP -= (harming / 5);

        if(BP < 0)
        {
            BP = 0;
        }
        
        return (harming);
    }

    public int PhysicalAttack(int enemyOffence, int enemyAccuracy, int enemyLuck, string damageType)
    {
        int damage;

        float hitChance = enemyAccuracy + Random.Range(0, enemyAccuracy);
        float missChance = Evasion() + Random.Range(0, Evasion());

        if(missChance < hitChance)
        {
            float critChance = enemyLuck + Random.Range(0, enemyLuck);
            float evadeChance = Evasion() + Random.Range(0, Evasion());

            if (evadeChance < critChance)
            {
                enemyOffence += enemyLuck;
            }
        }
        else
        {
            return 0;
        }
        
        foreach(string type in resistance)
        {
            if(type == damageType)
            {
                damage = (enemyOffence * (enemyOffence/Defence())) / 2;

                HP -= damage;

                return damage;
            }
        }

        foreach(string type in vulnerability)
        {
            if(type == damageType)
            {
                damage = (enemyOffence * (enemyOffence/Defence())) * 2;
                
                HP -= damage;

                return damage;
            }
        }

        damage = (enemyOffence * (enemyOffence/Defence()));

        HP -= damage;

        return damage;
    }

    public int EnergyAttack(int enemyOffence, string damageType)
    {
        int damage;
        
        foreach(string type in resistance)
        {
            if(type == damageType)
            {
                damage = (enemyOffence * (enemyOffence/EnergyDefence())) / 2;

                HP -= damage;

                return damage;
            }
        }

        foreach(string type in vulnerability)
        {
            if(type == damageType)
            {
                damage = (enemyOffence * (enemyOffence/EnergyDefence())) * 2;
                
                HP -= damage;

                return damage;
            }
        }

        damage = (enemyOffence * (enemyOffence/EnergyDefence()));

        HP -= damage;

        return damage;
    }

    public int Damage(int amount, string damageType)
    {
        int damage;
        
        foreach(string type in resistance)
        {
            if(type == damageType)
            {
                damage = (amount * (amount/Defence())) / 2;

                HP -= damage;

                return damage;
            }
        }

        foreach(string type in vulnerability)
        {
            if(type == damageType)
            {
                damage = (amount * (amount/Defence())) * 2;
                
                HP -= damage;

                return damage;
            }
        }

        damage = (amount * (amount/Defence()));

        HP -= damage;

        return damage;
    }

    public GameObject Summon(TechData summonTech)
    {
        summon = Instantiate(summonTech.summon, summonPos);

        return summon;
    }

    private void AugmentStat(int augment, int stat)
    {
        if (augment < 0)
        {
            augmentedStats[stat] = augmentedStats[stat] / (1 + (-augment / totalStats[stat]));
        }
        else if(augment > 0)
        {
            augmentedStats[stat] = augmentedStats[stat] + augment;
        }
    }

    public void ResetStat(int stat)
    {
        augmentedStats[stat] = totalStats[stat];
    }

    public int AugmentOffence(int augment)
    {
        int stat = 0;

        ResetStat(stat);
        AugmentStat(augment, stat);

        return (augmentedStats[stat] - totalStats[stat]);
    }

    public int AugmentDefence(int augment)
    {
        int stat = 1;

        ResetStat(stat);
        AugmentStat(augment, stat);

        return (augmentedStats[stat] - totalStats[stat]);
    }

    public int AugmentAccuracy(int augment)
    {
        int stat = 2;

        ResetStat(stat);
        AugmentStat(augment, stat);

        return (augmentedStats[stat] - totalStats[stat]);
    }

    public int AugmentEvasion(int augment)
    {
        int stat = 3;

        ResetStat(stat);
        AugmentStat(augment, stat);

        return (augmentedStats[stat] - totalStats[stat]);
    }

    public int AugmentLuck(int augment)
    {
        int stat = 4;

        ResetStat(stat);
        AugmentStat(augment, stat);

        return (augmentedStats[stat] - totalStats[stat]);
    }

    public int AugmentSpeed(int augment)
    {
        int stat = 5;

        ResetStat(stat);
        AugmentStat(augment, stat);

        return (augmentedStats[stat] - totalStats[stat]);
    }

    public int AugmentEnergyOffence(int augment)
    {
        int stat = 6;

        ResetStat(stat);
        AugmentStat(augment, stat);

        return (augmentedStats[stat] - totalStats[stat]);
    }

    public int AugmentEnergyDefence(int augment)
    {
        int stat = 7;

        ResetStat(stat);
        AugmentStat(augment, stat);
        
        return (augmentedStats[stat] - totalStats[stat]);
    }

    public int AugmentSupport(int augment)
    {
        int stat = 8;

        ResetStat(stat);
        AugmentStat(augment, stat);

        return (augmentedStats[stat] - totalStats[stat]);
    }

    public int AugmentStamina(int augment)
    {
        int stat = 9;

        ResetStat(stat);
        AugmentStat(augment, stat);

        return (augmentedStats[stat] - totalStats[stat]);
    }

    public int AugmentVigour(int augment)
    {
        int stat = 10;

        ResetStat(stat);
        AugmentStat(augment, stat);

        return (augmentedStats[stat] - totalStats[stat]);
    }

    public int AugmentVitality(int augment)
    {
        int stat = 11;

        ResetStat(stat);
        AugmentStat(augment, stat);

        return (augmentedStats[stat] - totalStats[stat]);
    }
}
