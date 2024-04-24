using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyData : MonoBehaviour
{
    public string enemyName;
    public CreatureData creatureData;

    public int[] overrideStats = new int[12];
    public int[] baseStats = new int[12];
    public int[] totalStats = new int[12];
    public int[] augmentedStats = new int[12];

    public int level, maxBP, maxEP, maxHP, BP, EP, HP, boost, sub;
    public List<string> proficiency;
    public List<string> resistance;
    public List<string> vulnerability;
    public List<TechData> techs;

    public List<ItemData> drops;
    public List<float> rarity;
    public int coins;
    public CombattantScript combattantScript;

    public Sprite battleSprite;
    public SpriteRenderer spriteRenderer;
    public GameObject summon;
    public Transform summonPos;

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

        /*stats.Add(baseStats);
        stats.Add(statMods);
        stats.Add(equipmentStats);
        stats.Add(totalStats);
        stats.Add(augmentedStats);*/

        foreach(string item in creatureData.resistance)
        {
            resistance.Add(item);
        }

        foreach(string item in creatureData.vulnerability)
        {
            vulnerability.Add(item);
        }

        foreach(TechData item in creatureData.techs)
        {
            if (level >= item.level)
            {
                techs.Add(item);
            }
        }

        for(int i = 0; i < 12; i++)
        {
            baseStats[i] = (5 + creatureData.stats[i]) * level;
        }
        baseStats[9] = baseStats[9] * 25;
        baseStats[10] = baseStats[10] * 10;
        baseStats[11] = baseStats[11] * 50;

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

        HP = maxHP;
        EP = maxEP;
        BP = maxBP;
    }

    public void BattlePos(Transform battlePos)
    {
        //gameObject.transform.position = battlePos.position;
    }

    void Start()
    {
        /*spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        spriteRenderer.sprite = battleSprite;*/
    }
    
    public EnemyData(CreatureData creature, int lv)
    {
        creatureData = creature;
        level = lv;

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
        HP += healing;

        if(HP > maxHP)
        {
            HP = maxHP;
        }
        
        sub = (healing / 10);
    }

    public void HealEP(int healing)
    {
        EP += healing;

        if(EP > maxEP)
        {
            EP = maxEP;
        }
        
        sub = (healing / 2);
    }

    public void HealBP(int healing)
    {
        BP += healing;

        if(BP > maxBP)
        {
            BP = maxBP;
        }
        
        sub = (healing / 5);
    }

    public void HarmHP(int harming)
    {
        HP -= (harming / 10);

        if(HP < 0)
        {
            HP = 0;
        }
        
        sub = (harming);
    }

    public void HarmEP(int harming)
    {
        EP -= (harming / 2);

        if(EP < 0)
        {
            EP = 0;
        }
        
        sub = (harming);
    }

    public void HarmBP(int harming)
    {
        BP -= (harming / 5);

        if(BP < 0)
        {
            BP = 0;
        }
        
        sub = (harming);
    }

    public void Attack(int enemyOffence, int enemyAccuracy, int enemyLuck, string damageType)
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

                return;
            }
        }

        foreach(string type in vulnerability)
        {
            if(type == damageType)
            {
                damage = damage * 2;

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
            sub = 0;

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
                damage = (enemyOffence * (enemyOffence/EnergyDefence())) / 2;

                sub = damage;

                return;
            }
        }

        foreach(string type in vulnerability)
        {
            if(type == damageType)
            {
                damage = (enemyOffence * (enemyOffence/EnergyDefence())) * 2;

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
        
        sub = (augmentedStats[stat] - totalStats[stat]);
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
