using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterData : MonoBehaviour
{
    public string characterName;

    public List<int[]> stats;

    //Offence = 0, Defence = 1, Accuracy = 2, Evasion = 3, Luck = 4, Speed = 5, Energy Offence = 6, EnergyDefence = 7, Support = 8, Stamina = 9, Vigour = 10, Vitality = 11
    
    public int[] baseStats = new int[12] /*{baseOffence, baseDefence, baseAccuracy, baseEvasion, baseLuck, baseSpeed, baseEnergyOffence, baseEnergyDefence, baseSupport, baseStamina, baseVigour, baseVitality}*/;
    public int[] statMods = new int[12] /*{offenceMod, defenceMod, accuracyMod, evasionMod, luckMod, speedMod, energyOffenceMod, energyDefenceMod, supportMod, staminaMod, vigourMod, vitalityMod}*/;
    public int[] equipmentStats = new int[12] /*{equipmentOffence, equipmentDefence, equipmentAccuracy, equipmentEvasion, equipmentLuck, equipmentSpeed, equipmentEnergyOffence, equipmentEnergyDefence, equipmentSupport, equipmentStamina, equipmentVigour, equipmentVitality}*/;
    public int[] totalStats = new int[12] /*{offence, defence, accuracy, evasion, luck, speed, energyOffence, energyDefence, support, stamina, vigour, vitality}*/;
    public int[] augmentedStats = new int[12] /*{augmentedOffence, augmentedDefence, augmentedAccuracy, augmentedEvasion, augmentedLuck, augmentedSpeed, augmentedEnergyOffence, augmentedEnergyDefence, augmentedSupport, augmentedStamina, augmentedVigour, augmentedVitality}*/;

    public int level, maxBP, maxEP, maxHP, maxXP, BP, EP, HP, XP, boost, sub;
    public JobData primaryJob, secondaryJob;
    public WeaponData weaponSlot;
    public ArmourData shieldSlot, headSlot, bodySlot, feetSlot, accessorySlot;
    public GameObject summon;
    public Transform summonPos;
    public Sprite overworldSprite, battleSprite;
    public SpriteRenderer spriteRenderer;
    public bool battle;
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

    public void StartBattle()
    {
        Debug.Log("Battle Started");
        
        //spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        
        /*if(battle)
        {
            spriteRenderer.sprite = battleSprite;
        }
        else
        {
            spriteRenderer.sprite = overworldSprite;
        }*/

        characterName = primaryJob.characterName;

        /*stats.Add(baseStats);
        stats.Add(statMods);
        stats.Add(equipmentStats);
        stats.Add(totalStats);
        stats.Add(augmentedStats);*/

        foreach(string item in primaryJob.proficiency)
        {
            proficiency.Add(item);
        }

        foreach(string item in primaryJob.resistance)
        {
            resistance.Add(item);
        }

        foreach(string item in primaryJob.vulnerability)
        {
            vulnerability.Add(item);
        }

        foreach(TechData item in primaryJob.techs)
        {
            if (level >= item.level)
            {
                techs.Add(item);
            }
        }

        for(int i = 0; i < 12; i++)
        {
            baseStats[i] = (5 + primaryJob.stats[i]) * level;
            Debug.Log(baseStats[i]);
        }
        baseStats[9] = baseStats[9] * 5;
        baseStats[10] = baseStats[10] * 2;
        baseStats[11] = baseStats[11] * 10;

        for (int i = 0; i < 12; i++)
        {
            totalStats[i] = baseStats[i];
            augmentedStats[i] = totalStats[i];
            Debug.Log(baseStats[i]);
            Debug.Log(totalStats[i]);
            Debug.Log(augmentedStats[i]);
        }
        maxBP = totalStats[9];
        maxEP = totalStats[10];
        maxHP = totalStats[11];
        maxXP = (level*level) * 100;

        HP = maxHP;
        EP = maxEP;
        BP = maxBP;

        //Debug.Log("What?");
    }

    public void BattlePos(Transform battlePos)
    {
        //gameObject.transform.position = battlePos.position;
    }

    public void LoadPos()
    {
        gameObject.transform.position = GameDataManager.instance.progress.characterPos;
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
        battle = false;
        
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
        combattantScript.speedStat = totalStats[5];
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

    public void HealHP(int healing)
    {
        HP += (healing / 2);

        if(HP > maxHP)
        {
            HP = maxHP;
        }

        Debug.Log(characterName + " Heals " + healing + " HP");

        Debug.Log(characterName);
        
        sub = (healing / 2);
    }

    public void HealEP(int healing)
    {
        EP += (healing / 10);

        if(EP > maxEP)
        {
            EP = maxEP;
        }

        Debug.Log(characterName + " Heals " + healing + " EP");

        Debug.Log(characterName);
        
        sub = (healing / 10);


    }

    public void HealBP(int healing)
    {
        BP += (healing / 5);

        if(BP > maxBP)
        {
            BP = maxBP;
        }

        Debug.Log(characterName + " Heals " + healing + " BP");

        Debug.Log(characterName);
        
        sub = (healing / 5);
    }

    public void HarmHP(int harming)
    {
        HP -= (harming / 2);

        if(HP < 0)
        {
            HP = 0;
        }

        Debug.Log("Harm " + harming + " HP");
        
        sub = (harming / 2);
    }

    public void HarmEP(int harming)
    {
        EP -= (harming / 10);

        if(EP < 0)
        {
            EP = 0;
        }

        Debug.Log("Harm " + harming + " EP");
        
        sub = (harming / 10);
    }

    public void HarmBP(int harming)
    {
        BP -= (harming / 5);

        if(BP < 0)
        {
            BP = 0;
        }

        Debug.Log("Harm " + harming + " BP");
        
        sub = (harming / 5);
    }

    public void Attack(int enemyOffence, int enemyAccuracy, int enemyLuck, string damageType)
    {
        int damage = 0;

        float hitChance = enemyAccuracy + Random.Range(0, enemyAccuracy);
        float missChance = Evasion() + Random.Range(0, Evasion());

        if(missChance < hitChance)
        {
            float critChance = enemyLuck + Random.Range(0, enemyLuck);
            float evadeChance = Evasion() + Random.Range(0, Evasion());
            Debug.Log("Hit!");

            if (evadeChance < critChance)
            {
                enemyOffence += enemyLuck;
                Debug.Log("Crit!");
            }

            Debug.Log(augmentedStats[1]);

            damage = (enemyOffence * (enemyOffence/Defence()));
        }
        else
        {
            Debug.Log("Miss!");

            return;
        }
        
        foreach(string type in resistance)
        {
            if(type == damageType && damage > 0)
            {
                damage = damage / 2;

                Debug.Log("Uneffective!");

                return;
            }
        }

        foreach(string type in vulnerability)
        {
            if(type == damageType && damage > 0)
            {
                damage = damage * 2;

                Debug.Log("Effective!");

                return;
            }
        }

        HP -= damage;

        return;
    }

    public void PhysicalAttack(int enemyOffence, int enemyAccuracy, int enemyLuck, string damageType)
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

            damage = (enemyOffence * (enemyOffence/Defence()));
        }
        else
        {
            return;
        }
        
        foreach(string type in resistance)
        {
            if(type == damageType)
            {
                damage = damage / 2;

                sub = damage;

                return;
            }
        }

        foreach(string type in vulnerability)
        {
            if(type == damageType)
            {
                damage = damage * 2;

                sub = damage;

                return;
            }
        }

        HP -= damage;

        sub = damage;

        return;
    }

    public void EnergyAttack(int enemyOffence, string damageType)
    {
        int damage;

        damage = (enemyOffence * (enemyOffence/EnergyDefence()));
        
        foreach(string type in resistance)
        {
            if(type == damageType)
            {
                damage = damage / 2;

                sub = damage;

                return;
            }
        }

        foreach(string type in vulnerability)
        {
            if(type == damageType)
            {
                damage = damage * 2;

                sub = damage;

                return;
            }
        }

        HP -= damage;

        sub = damage;

        return;
    }

    public void Damage(int amount, string damageType)
    {
        int damage;
        
        foreach(string type in resistance)
        {
            if(type == damageType)
            {
                damage = (amount * (amount/Defence())) / 2;

                HP -= damage;

                sub = damage;

                return;
            }
        }

        foreach(string type in vulnerability)
        {
            if(type == damageType)
            {
                damage = (amount * (amount/Defence())) * 2;
                
                HP -= damage;

                sub = damage;

                return;
            }
        }

        damage = (amount * (amount/Defence()));

        HP -= damage;

        sub = damage;

        return;
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

    public void AugmentOffence(int augment)
    {
        int stat = 0;

        ResetStat(stat);
        AugmentStat(augment, stat);

        sub = (augmentedStats[stat] - totalStats[stat]);
    }

    public void AugmentDefence(int augment)
    {
        int stat = 1;

        ResetStat(stat);
        AugmentStat(augment, stat);

        sub = (augmentedStats[stat] - totalStats[stat]);
    }

    public void AugmentAccuracy(int augment)
    {
        int stat = 2;

        ResetStat(stat);
        AugmentStat(augment, stat);

        sub = (augmentedStats[stat] - totalStats[stat]);
    }

    public void AugmentEvasion(int augment)
    {
        int stat = 3;

        ResetStat(stat);
        AugmentStat(augment, stat);

        sub = (augmentedStats[stat] - totalStats[stat]);
    }

    public void AugmentLuck(int augment)
    {
        int stat = 4;

        ResetStat(stat);
        AugmentStat(augment, stat);

        sub = (augmentedStats[stat] - totalStats[stat]);
    }

    public void AugmentSpeed(int augment)
    {
        int stat = 5;

        ResetStat(stat);
        AugmentStat(augment, stat);

        sub = (augmentedStats[stat] - totalStats[stat]);
    }

    public void AugmentEnergyOffence(int augment)
    {
        int stat = 6;

        ResetStat(stat);
        AugmentStat(augment, stat);

        sub = (augmentedStats[stat] - totalStats[stat]);
    }

    public void AugmentEnergyDefence(int augment)
    {
        int stat = 7;

        ResetStat(stat);
        AugmentStat(augment, stat);
        
        sub =  (augmentedStats[stat] - totalStats[stat]);
    }

    public void AugmentSupport(int augment)
    {
        int stat = 8;

        ResetStat(stat);
        AugmentStat(augment, stat);

        sub = (augmentedStats[stat] - totalStats[stat]);
    }

    public void AugmentStamina(int augment)
    {
        int stat = 9;

        ResetStat(stat);
        AugmentStat(augment, stat);

        sub = (augmentedStats[stat] - totalStats[stat]);
    }

    public void AugmentVigour(int augment)
    {
        int stat = 10;

        ResetStat(stat);
        AugmentStat(augment, stat);

        sub = (augmentedStats[stat] - totalStats[stat]);
    }

    public void AugmentVitality(int augment)
    {
        int stat = 11;

        ResetStat(stat);
        AugmentStat(augment, stat);

        sub = (augmentedStats[stat] - totalStats[stat]);
    }
}
