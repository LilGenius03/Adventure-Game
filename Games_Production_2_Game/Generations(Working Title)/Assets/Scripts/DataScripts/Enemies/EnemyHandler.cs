using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHandler : MonoBehaviour
{
    public EncounterManager encounter;
    public PartyManager party;
    public EnemyData currentEnemy, selectedEnemy;
    public CharacterData selectedCharacter;
    public TechData selectedTech;
    public BattleManager battle;

    public int boost = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        party = GameDataManager.instance.party;
        encounter = GameDataManager.instance.encounter;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EnemyTurn(int charNo)
    {
        bool acted = true;
        do
        {
            currentEnemy = encounter.tempEnemies[charNo];
            Debug.Log(currentEnemy.enemyName);
            int actionOptions = 0;
            int boostChoice = 0;
            boost = 0;

            boostChoice = Random.Range(0, currentEnemy.level);

            if((boostChoice*5) >= currentEnemy.BP)
            {
                boost = (boostChoice*5);
            }
            else if(currentEnemy.BP > 0)
            {
                boost = currentEnemy.BP;
            }

            currentEnemy.Boost(boost);

            int actionChoice = Random.Range(0, 2);

            if(actionChoice == 0)
            {
                int attackChoice;
                
                for(int i = 0; i < party.tempMembers.Count; i++)
                {
                    if(party.tempMembers[i].HP > 0)
                    {
                        actionOptions++;
                    }
                }

                if(actionOptions > 0)
                {
                    attackChoice = Random.Range(0, actionOptions);

                    int characterNo = 0;

                    for(int i = 0; i < party.tempMembers.Count; i++)
                    {
                        if(party.tempMembers[i].HP > 0)
                        {
                            if(characterNo == attackChoice)
                            {
                                int weaponChoice = Random.Range(0, currentEnemy.proficiency.Count);
                                party.tempMembers[i].Attack((currentEnemy.Offence() + boost), (currentEnemy.Accuracy() + boost), (currentEnemy.Luck() + boost), currentEnemy.proficiency[weaponChoice]);
                            }
                            else
                            {
                                characterNo++;
                            }
                        }
                    }
                }
            }
            else
            {
                int techChoice;
                
                for(int i = 0; i < currentEnemy.techs.Count; i++)
                {
                    if(currentEnemy.techs[i].energy < currentEnemy.EP)
                    {
                        actionOptions++;
                    }
                }

                if(actionOptions > 0)
                {
                    techChoice = Random.Range(0, actionOptions);

                    int techNo = 0;

                    for(int i = 0; i < currentEnemy.techs.Count; i++)
                    {
                        if(currentEnemy.techs[i].energy < currentEnemy.EP)
                        {
                            if(techNo == techChoice)
                            {
                                if((currentEnemy.techs[i].target != "Allies") && encounter.tempEnemies.Count > 1)
                                {
                                    SelectTech(currentEnemy.techs[i]);
                                }
                                else
                                {
                                    acted = false;
                                }
                            }
                            else
                            {
                                techNo++;
                            }
                        }
                    }
                }
            }
        } while(!acted);

        Debug.Log("Attack Finished");

        EndTurn();
    }

    public void EndTurn()
    {
        currentEnemy.BP -= currentEnemy.boost;
        currentEnemy.boost = 0;
        currentEnemy = null;
        battle.nextTurn = true;
    }

    public void SpendPoints(TechData tech, EnemyData enemy)
    {
        currentEnemy.HarmHP(tech.health);
        currentEnemy.HarmEP(tech.energy);
        currentEnemy.HarmBP(tech.boosts);
    }

    public void SelectTech(TechData tech)
    {
        selectedTech = tech;
        switch(tech.target)
        {
            case "Self":
            {
                Tech(tech, currentEnemy.combattantScript);
                break;
            }
            
            case "Enemy":
            {
                int actionOptions = 0;
                
                int characterChoice;
                
                for(int i = 0; i < party.tempMembers.Count; i++)
                {
                    if(party.tempMembers[i].HP > 0)
                    {
                        actionOptions++;
                    }
                }

                if(actionOptions > 0)
                {
                    characterChoice = Random.Range(0, actionOptions);

                    int characterNo = 0;

                    for(int i = 0; i < party.tempMembers.Count; i++)
                    {
                        if(party.tempMembers[i].HP > 0)
                        {
                            if(characterNo == characterChoice)
                            {
                                Tech(tech, party.tempMembers[i].combattantScript);
                            }
                            else
                            {
                                characterNo++;
                            }
                        }
                    }
                }

                break;
            }

            case "Enemies":
            {
                Tech(tech, currentEnemy.combattantScript);
                break;
            }

            case "Ally":
            {
                int actionOptions = 0;
                
                int characterChoice;
                
                for(int i = 0; i < encounter.tempEnemies.Count; i++)
                {
                    if(encounter.tempEnemies[i].HP > 0)
                    {
                        actionOptions++;
                    }
                }

                if(actionOptions > 0)
                {
                    characterChoice = Random.Range(0, actionOptions);

                    int characterNo = 0;

                    for(int i = 0; i < encounter.tempEnemies.Count; i++)
                    {
                        if(encounter.tempEnemies[i].HP > 0)
                        {
                            if(characterNo == characterChoice)
                            {
                                Tech(tech, encounter.tempEnemies[i].combattantScript);
                            }
                            else
                            {
                                characterNo++;
                            }
                        }
                    }
                }

                break;
            }

            case "Allies":
            {
                Tech(tech, currentEnemy.combattantScript);
                break;
            }

            case "Summon":
            {
                Tech(tech, currentEnemy.combattantScript);
                break;
            }

            default:
            {
                break;
            }
        }
    }

    public void Tech(TechData tech, CombattantScript combattant)
    {
        if(tech.health < currentEnemy.HP && tech.energy < currentEnemy.EP && tech.boosts < currentEnemy.BP)
        {
            SpendPoints(tech, currentEnemy);

            switch(tech.target)
            {
                case "Self":
                {
                    selectedEnemy = currentEnemy;

                    DefensiveTech(tech, selectedEnemy);

                    break;
                }

                case "Enemy":
                {
                    CharacterData targetCharacter = combattant.gameObject.GetComponent<CharacterData>();

                    selectedCharacter = targetCharacter;

                    OffensiveTech(tech, targetCharacter);

                    break;
                }

                case "Enemies":
                {
                    for(int i = 0; i < party.tempMembers.Count; i++)
                    {
                        if(party.tempMembers[i].HP > 0)
                        {
                            selectedCharacter = party.tempMembers[i];
                            
                            OffensiveTech(tech, party.tempMembers[i]);
                        }
                    }

                    break;
                }

                case "Ally":
                {
                    EnemyData targetEnemy = combattant.gameObject.GetComponent<EnemyData>();

                    selectedEnemy = targetEnemy;

                    DefensiveTech(tech, targetEnemy);

                    break;
                }

                case "Allies":
                {
                    for(int i = 0; i < encounter.tempEnemies.Count; i++)
                    {
                        if(encounter.tempEnemies[i].HP > 0)
                        {
                            selectedEnemy = encounter.tempEnemies[i];

                            DefensiveTech(tech, encounter.tempEnemies[i]);
                        }
                    }

                    break;
                }

                case "Summon":
                {
                    DefensiveTech(tech, currentEnemy);

                    break;
                }

                default:
                {
                    break;
                }
            }
        }
    }

    public void OffensiveTech(TechData tech, CharacterData character)
    {
        switch (tech.effect)
        {
            case "Debuff":
            {
                switch (tech.type)
                {
                    case "Offence":
                    {
                        character.AugmentOffence(-currentEnemy.Support() - tech.energy - currentEnemy.boost);

                        Subtech(tech, character.sub);

                        break;
                    }

                    case "Defence":
                    {
                        character.AugmentDefence(-currentEnemy.Support() - tech.energy - currentEnemy.boost);

                        Subtech(tech, character.sub);

                        break;
                    }

                    case "Accuracy":
                    {
                        character.AugmentAccuracy(-currentEnemy.Support() - tech.energy - currentEnemy.boost);

                        Subtech(tech, character.sub);

                        break;
                    }

                    case "Evasion":
                    {
                        character.AugmentEvasion(-currentEnemy.Support() - tech.energy - currentEnemy.boost);

                        Subtech(tech, character.sub);

                        break;
                    }

                    case "Luck":
                    {
                        character.AugmentLuck(-currentEnemy.Support() - tech.energy - currentEnemy.boost);

                        Subtech(tech, character.sub);

                        break;
                    }

                    case "Speed":
                    {
                        character.AugmentSpeed(-currentEnemy.Support() - tech.energy - currentEnemy.boost);

                        Subtech(tech, character.sub);

                        break;
                    }

                    case "Energy Offence":
                    {
                        character.AugmentEnergyOffence(-currentEnemy.Support() - tech.energy - currentEnemy.boost);

                        Subtech(tech, character.sub);

                        break;
                    }

                    case "Energy Defence":
                    {
                        character.AugmentEnergyDefence(-currentEnemy.Support() - tech.energy - currentEnemy.boost);

                        Subtech(tech, character.sub);

                        break;
                    }

                    case "Support":
                    {
                        character.AugmentSupport(-currentEnemy.Support() - tech.energy - currentEnemy.boost);

                        Subtech(tech, character.sub);

                        break;
                    }

                    case "Stamina":
                    {
                        character.AugmentStamina(-currentEnemy.Support() - tech.energy - currentEnemy.boost);

                        Subtech(tech, character.sub);

                        break;
                    }

                    case "Vigour":
                    {
                        character.AugmentVigour(-currentEnemy.Support() - tech.energy - currentEnemy.boost);

                        Subtech(tech, character.sub);

                        break;
                    }

                    case "Vitality":
                    {
                        character.AugmentVitality(-currentEnemy.Support() - tech.energy - currentEnemy.boost);

                        Subtech(tech, character.sub);

                        break;
                    }

                    default:
                    {
                        break;
                    }
                }

                break;
            }

            case "Physical Attack":
            {
                character.PhysicalAttack((currentEnemy.Offence() + tech.energy + currentEnemy.boost), (currentEnemy.Accuracy() + tech.energy + currentEnemy.boost), (currentEnemy.Luck() + tech.energy + currentEnemy.boost), tech.type);

                Subtech(tech, character.sub);

                break;
            }

            case "Energy Attack":
            {
                character.EnergyAttack((currentEnemy.EnergyOffence() + tech.energy + currentEnemy.boost), tech.type);

                Subtech(tech, character.sub);

                break;
            }

            case "Harm":
            {
                switch(tech.type)
                {
                    case "Health":
                    {
                        character.HarmHP(currentEnemy.Support() + tech.health + currentEnemy.boost);

                        Subtech(tech, character.sub);

                        break;
                    }

                    case "Energy":
                    {
                        character.HarmEP(currentEnemy.Support() + tech.energy + currentEnemy.boost);

                        Subtech(tech, character.sub);

                        break;
                    }

                    case "Boost":
                    {
                        character.HarmBP(currentEnemy.Support() + tech.health + tech.energy + currentEnemy.boost);

                        Subtech(tech, character.sub);

                        break;
                    }

                    default:
                    {
                        break;
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

    public void DefensiveTech(TechData tech, EnemyData ally)
    {
        switch (tech.effect)
        {
            case "Buff":
            {
                switch (tech.type)
                {
                    case "Offence":
                    {
                        ally.AugmentOffence(currentEnemy.Support() + tech.energy + currentEnemy.boost);

                        Subtech(tech, ally.sub);

                        break;
                    }

                    case "Defence":
                    {
                        ally.AugmentDefence(currentEnemy.Support() + tech.energy + currentEnemy.boost);

                        Subtech(tech, ally.sub);

                        break;
                    }

                    case "Accuracy":
                    {
                        ally.AugmentAccuracy(currentEnemy.Support() + tech.energy + currentEnemy.boost);

                        Subtech(tech, ally.sub);

                        break;
                    }

                    case "Evasion":
                    {
                        ally.AugmentEvasion(currentEnemy.Support() + tech.energy + currentEnemy.boost);

                        Subtech(tech, ally.sub);

                        break;
                    }

                    case "Luck":
                    {
                        ally.AugmentLuck(currentEnemy.Support() + tech.energy + currentEnemy.boost);

                        Subtech(tech, ally.sub);

                        break;
                    }

                    case "Speed":
                    {
                        ally.AugmentSpeed(currentEnemy.Support() + tech.energy + currentEnemy.boost);

                        Subtech(tech, ally.sub);

                        break;
                    }

                    case "Energy Offence":
                    {
                        ally.AugmentEnergyOffence(currentEnemy.Support() + tech.energy + currentEnemy.boost);

                        Subtech(tech, ally.sub);

                        break;
                    }

                    case "Energy Defence":
                    {
                        ally.AugmentEnergyDefence(currentEnemy.Support() + tech.energy + currentEnemy.boost);

                        Subtech(tech, ally.sub);

                        break;
                    }

                    case "Support":
                    {
                        ally.AugmentSupport(currentEnemy.Support() + tech.energy + currentEnemy.boost);

                        Subtech(tech, ally.sub);

                        break;
                    }

                    case "Stamina":
                    {
                        ally.AugmentStamina(currentEnemy.Support() + tech.energy + currentEnemy.boost);

                        Subtech(tech, ally.sub);

                        break;
                    }

                    case "Vigour":
                    {
                        ally.AugmentVigour(currentEnemy.Support() + tech.energy + currentEnemy.boost);

                        Subtech(tech, ally.sub);

                        break;
                    }

                    case "Vitality":
                    {
                        ally.AugmentVitality(currentEnemy.Support() + tech.energy + currentEnemy.boost);

                        Subtech(tech, ally.sub);

                        break;
                    }

                    default:
                    {
                        break;
                    }
                }

                break;
            }

            case "Heal":
            {
                switch (tech.type)
                {
                    case "Health":
                    {
                        ally.HealHP(currentEnemy.Support() + tech.energy + currentEnemy.boost);

                        Subtech(tech, ally.sub);

                        break;
                    }

                    case "Energy":
                    {
                        ally.HealEP(currentEnemy.Support() + tech.health + currentEnemy.boost);

                        Subtech(tech, ally.sub);

                        break;
                    }

                    case "Boost":
                    {
                        ally.HealBP(currentEnemy.Support() + tech.health + tech.energy + currentEnemy.boost);

                        Subtech(tech, ally.sub);

                        break;
                    }

                    default:
                    {
                        break;
                    }
                }

                break;
            }

            case "Summon":
            {
                Subtech(tech, 0);

                break;
            }
        }
    }

    public void Subtech(TechData tech, int sub)
    {
        if (tech.subtarget != "" && tech.subtarget != null)
        {
            if(sub < 0)
            {
                sub = -sub;
            }

            switch(tech.subtarget)
            {
                case "Self":
                {
                    selectedEnemy = currentEnemy;

                    DefensiveSubtech(tech, selectedEnemy, sub);

                    break;
                }
                
                case "Enemy":
                {
                    OffensiveSubtech(tech, selectedCharacter, sub);

                    break;
                }

                case "Enemies":
                {
                    for(int i = 0; i < party.tempMembers.Count; i++)
                    {
                        if(party.tempMembers[i].HP > 0)
                        {
                            OffensiveSubtech(tech, party.tempMembers[i], sub);
                        }
                    }

                    break;
                }

                case "Ally":
                {
                    DefensiveSubtech(tech, selectedEnemy, sub);

                    break;
                }

                case "Allies":
                {
                    for(int i = 0; i < encounter.tempEnemies.Count; i++)
                    {
                        if(encounter.tempEnemies[i].HP > 0)
                        {
                            DefensiveSubtech(tech, encounter.tempEnemies[i], sub);
                        }
                    }

                    break;
                }

                case "Summon":
                {
                    DefensiveSubtech(tech, currentEnemy, 0);

                    break;
                }

                default:
                {
                    break;
                }
            }
        }

        EndTurn();
    }

    public void OffensiveSubtech(TechData tech, CharacterData character, int sub)
    {
        switch (tech.subeffect)
        {
            case "Debuff":
            {
                switch (tech.subtype)
                {
                    case "Offence":
                    {
                        sub = sub / 10;

                        character.AugmentOffence(-sub);

                        break;
                    }

                    case "Defence":
                    {
                        sub = sub / 10;

                        character.AugmentDefence(-sub);

                        break;
                    }

                    case "Accuracy":
                    {
                        sub = sub / 10;
                        
                        character.AugmentAccuracy(-sub);

                        break;
                    }

                    case "Evasion":
                    {
                        sub = sub / 10;
                        
                        character.AugmentEvasion(-sub);

                        break;
                    }

                    case "Luck":
                    {
                        sub = sub / 10;
                        
                        character.AugmentLuck(-sub);

                        break;
                    }

                    case "Speed":
                    {
                        sub = sub / 10;
                        
                        character.AugmentSpeed(-sub);

                        break;
                    }

                    case "Energy Offence":
                    {
                        sub = sub / 10;
                        
                        character.AugmentEnergyOffence(-sub);

                        break;
                    }

                    case "Energy Defence":
                    {
                        sub = sub / 10;
                        
                        character.AugmentEnergyDefence(-sub);

                        break;
                    }

                    case "Support":
                    {
                        sub = sub / 10;
                        
                        character.AugmentSupport(-sub);

                        break;
                    }

                    case "Stamina":
                    {
                        sub = sub / 10;
                        
                        character.AugmentStamina(-sub);

                        break;
                    }

                    case "Vigour":
                    {
                        sub = sub / 10;
                        
                        character.AugmentVigour(-sub);

                        break;
                    }

                    case "Vitality":
                    {
                        sub = sub / 10;
                        
                        character.AugmentVitality(-sub);

                        break;
                    }

                    default:
                    {
                        break;
                    }
                }

                break;
            }

            case "Physical Attack":
            {
                character.PhysicalAttack((currentEnemy.Offence() + tech.energy + currentEnemy.boost), (currentEnemy.Accuracy() + tech.energy + currentEnemy.boost), (currentEnemy.Luck() + tech.energy + currentEnemy.boost), tech.type);

                break;
            }

            case "Energy Attack":
            {
                character.EnergyAttack((currentEnemy.EnergyOffence() + tech.energy + currentEnemy.boost), tech.type);

                break;
            }

            case "Harm":
            {
                switch(tech.subtype)
                {
                    case "Health":
                    {
                        sub = sub / 100;
                        
                        character.HarmHP(sub);

                        break;
                    }

                    case "Energy":
                    {
                        sub = sub / 100;

                        character.HarmEP(sub);

                        break;
                    }

                    case "Boost":
                    {
                        sub = sub / 100;

                        character.HarmBP(sub);

                        break;
                    }

                    default:
                    {
                        break;
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

    public void DefensiveSubtech(TechData tech, EnemyData ally, int sub)
    {
        switch (tech.subeffect)
        {
            case "Buff":
            {
                switch (tech.subtype)
                {
                    case "Offence":
                    {
                        ally.AugmentOffence(sub);

                        break;
                    }

                    case "Defence":
                    {
                        ally.AugmentDefence(sub);

                        break;
                    }

                    case "Accuracy":
                    {
                        ally.AugmentAccuracy(sub);

                        break;
                    }

                    case "Evasion":
                    {
                        ally.AugmentEvasion(sub);

                        break;
                    }

                    case "Luck":
                    {
                        ally.AugmentLuck(sub);

                        break;
                    }

                    case "Speed":
                    {
                        ally.AugmentSpeed(sub);

                        break;
                    }

                    case "Energy Offence":
                    {
                        ally.AugmentEnergyOffence(sub);

                        break;
                    }

                    case "Energy Defence":
                    {
                        ally.AugmentEnergyDefence(sub);

                        break;
                    }

                    case "Support":
                    {
                        ally.AugmentSupport(sub);

                        break;
                    }

                    case "Stamina":
                    {
                        ally.AugmentStamina(sub);

                        break;
                    }

                    case "Vigour":
                    {
                        ally.AugmentVigour(sub);

                        break;
                    }

                    case "Vitality":
                    {
                        ally.AugmentVitality(sub);

                        break;
                    }

                    default:
                    {
                        break;
                    }
                }

                break;
            }

            case "Heal":
            {
                switch (tech.subtype)
                {
                    case "Health":
                    {
                        ally.HealHP(sub);

                        break;
                    }

                    case "Energy":
                    {
                        ally.HealEP(sub);

                        break;
                    }

                    case "Boost":
                    {
                        ally.HealBP(sub);

                        break;
                    }

                    default:
                    {
                        break;
                    }
                }

                break;
            }

            case "Summon":
            {
                ally.Summon(tech);

                break;
            }
        }
    }
}
