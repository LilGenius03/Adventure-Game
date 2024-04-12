using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyData : MonoBehaviour
{
    public string enemyName;
    public CreatureData creatureData;

    public int[] overrideStats = new int[12];
    private int[] baseStats = new int[12];
    private int[] totalStats = new int[12];
    private int[] augmentedStats = new int[12];

    public int level, maxBP, maxEP, maxHP, BP, EP, HP;
    public List<string> proficiency;
    public List<string> resistance;
    public List<string> vulnerability;

    public List<ItemData> drops;
    public List<float> rarity;
    public int coins;
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
    
    public EnemyData(CreatureData creature, int lv)
    {
        creatureData = creature;
        level = lv;

        foreach(string item in creatureData.proficiency)
        {
            proficiency.Add(item);
        }

        foreach(string item in creatureData.resistance)
        {
            resistance.Add(item);
        }

        foreach(string item in creatureData.vulnerability)
        {
            vulnerability.Add(item);
        }

        for (int i = 0; i < baseStats.Length; i++)
        {
            baseStats[i] = (5 + creature.stats[i]) * level;
        }
        baseStats[9] = baseStats[9] * 5;
        baseStats[10] = baseStats[10] * 2;
        baseStats[11] = baseStats[11] * 10;

        /*for (int i = 0; i < overrideStats.Length; i++)
        {
            overrideStats[i] = overrideStats[i] * level;
        }
        overrideStats[9] = overrideStats[9] * 5;
        overrideStats[10] = overrideStats[10] * 2;
        overrideStats[11] = overrideStats[11] * 10;*/

        for (int i = 0; i < totalStats.Length; i++)
        {
            /*if(overrideStats[i] != 0)
            {
                totalStats[i] = overrideStats[i];
            }
            else
            {*/
                totalStats[i] = baseStats[i];
                augmentedStats[i] = totalStats[i];
            //}
        }
        maxBP = totalStats[9];
        maxEP = totalStats[10];
        maxHP = totalStats[11];

        combattantScript = new CombattantScript();
    }

    public void RollInitiative()
    {
        combattantScript.speedStat = totalStats[6];
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
