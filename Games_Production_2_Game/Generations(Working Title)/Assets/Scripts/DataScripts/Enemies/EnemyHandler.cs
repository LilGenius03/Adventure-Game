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
    private float time = 3.0f;

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

    public void EnemyTurn(int enemyNo)
    {
        currentEnemy = encounter.tempEnemies[enemyNo];
        Debug.Log(currentEnemy.enemyName + "'s Turn");
        int actionOptions = 0;
        int boostChoice = 0;
        boost = 0;

        boostChoice = Random.Range(0, currentEnemy.level);

        if((boostChoice*5) <= currentEnemy.BP)
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
            Debug.Log("Attack!");

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
                            StartCoroutine(AttackText(party.tempMembers[i].sub, currentEnemy.proficiency[weaponChoice], party.tempMembers[i].characterName));
                        }

                        characterNo++;
                    }
                }
            }

            Debug.Log("Attack Finished!");
        }
        else
        {
            Debug.Log("Tech!");

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
                            SelectTech(currentEnemy.techs[i]);
                        }

                        techNo++;
                    }
                }
            }

            Debug.Log("Tech Finished");
        }

        Debug.Log("Turn Finished");
    }

    public void EndTurn()
    {
        battle.text.text = "";
        battle.textBox.SetActive(false);
        currentEnemy.BP -= currentEnemy.boost;
        currentEnemy.boost = 0;
        currentEnemy = null;
        battle.nextTurn = true;
    }

    IEnumerator AttackText(int damage, string damageType, string characterName)
    {
        battle.textBox.SetActive(true);

        if(damage <= 0)
        {
            battle.text.text = currentEnemy.enemyName + " Misses!";
        }
        else
        {
            battle.text.text = currentEnemy.enemyName + " dealt " + damage.ToString() + " " + damageType + " damage to " + characterName;
        }
        
        yield return new WaitForSeconds(time);

        //battle.text.text = "";

        //battle.textBox.SetActive(false);

        EndTurn();
    }

    IEnumerator TechAttackText(int damage, string damageType, string characterName)
    {
        battle.textBox.SetActive(true);
        
        if(damage <= 0)
        {
            battle.text.text = currentEnemy.enemyName + " Misses!";
        }
        else
        {
            battle.text.text = currentEnemy.enemyName + " dealt " + damage.ToString() + " " + damageType + " damage to " + characterName;
        }
        
        yield return new WaitForSeconds(time);

        //battle.text.text = "";

        //battle.textBox.SetActive(false);
    }

    IEnumerator AugmentText(int augment, string stat, string targetName)
    {
        battle.textBox.SetActive(true);
        
        if(augment <= 0)
        {
            battle.text.text = currentEnemy.enemyName + " decreases " + targetName + "'s " + stat + " by " + augment.ToString();
        }
        else
        {
            battle.text.text = currentEnemy.enemyName + " increases " + targetName + "'s " + stat + " by " + augment.ToString();
        }
        
        yield return new WaitForSeconds(time);

        //battle.text.text = "";

        //battle.textBox.SetActive(false);
    }

    IEnumerator HealthText(int augment, string targetName)
    {
        battle.textBox.SetActive(true);
        
        if(augment <= 0)
        {
            battle.text.text = currentEnemy.enemyName + " harms " + targetName + "'s HP by " + augment.ToString();
        }
        else
        {
            battle.text.text = currentEnemy.enemyName + " heals " + targetName + "'s HP by " + augment.ToString();
        }
        
        yield return new WaitForSeconds(time);

        //battle.text.text = "";

        //battle.textBox.SetActive(false);
    }

    IEnumerator EnergyText(int augment, string targetName)
    {
        battle.textBox.SetActive(true);
        
        if(augment <= 0)
        {
            battle.text.text = currentEnemy.enemyName + " harms " + targetName + "'s EP by " + augment.ToString();
        }
        else
        {
            battle.text.text = currentEnemy.enemyName + " heals " + targetName + "'s EP by " + augment.ToString();
        }
        
        yield return new WaitForSeconds(time);

        //battle.text.text = "";

        //battle.textBox.SetActive(false);
    }

    IEnumerator BoostText(int augment, string targetName)
    {
        battle.textBox.SetActive(true);
        
        if(augment <= 0)
        {
            battle.text.text = currentEnemy.enemyName + " harms " + targetName + "'s BP by " + augment.ToString();
        }
        else
        {
            battle.text.text = currentEnemy.enemyName + " heals " + targetName + "'s BP by " + augment.ToString();
        }
        
        yield return new WaitForSeconds(time);

        //battle.text.text = "";

        //battle.textBox.SetActive(false);
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
                StartCoroutine(Tech(tech, currentEnemy.combattantScript));
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
                                StartCoroutine(Tech(tech, party.tempMembers[i].combattantScript));
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
                StartCoroutine(Tech(tech, currentEnemy.combattantScript));
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
                                StartCoroutine(Tech(tech, encounter.tempEnemies[i].combattantScript));
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
                StartCoroutine(Tech(tech, currentEnemy.combattantScript));
                break;
            }

            case "Summon":
            {
                StartCoroutine(Tech(tech, currentEnemy.combattantScript));
                break;
            }

            default:
            {
                break;
            }
        }
    }

    public IEnumerator Tech(TechData tech, CombattantScript combattant)
    {
        float totalTime = time;
        
        if (tech.subtarget != "" && tech.subtarget != null)
        {
            totalTime = time * 2;
        }
        else
        {
            totalTime = time;
        }
        
        if(tech.health < currentEnemy.HP && tech.energy < currentEnemy.EP && tech.boosts < currentEnemy.BP)
        {
            SpendPoints(tech, currentEnemy);

            switch(tech.target)
            {
                case "Self":
                {
                    selectedEnemy = currentEnemy;

                    StartCoroutine(DefensiveTech(tech, selectedEnemy));

                    yield return new WaitForSeconds(totalTime);

                    break;
                }

                case "Enemy":
                {
                    CharacterData targetCharacter = combattant.gameObject.GetComponent<CharacterData>();

                    selectedCharacter = targetCharacter;

                    StartCoroutine(OffensiveTech(tech, targetCharacter));

                    yield return new WaitForSeconds(totalTime);

                    break;
                }

                case "Enemies":
                {
                    for(int i = 0; i < party.tempMembers.Count; i++)
                    {
                        if(party.tempMembers[i].HP > 0)
                        {
                            selectedCharacter = party.tempMembers[i];
                            
                            StartCoroutine(OffensiveTech(tech, party.tempMembers[i]));

                            yield return new WaitForSeconds(totalTime);
                        }
                    }

                    break;
                }

                case "Ally":
                {
                    EnemyData targetEnemy = combattant.gameObject.GetComponent<EnemyData>();

                    selectedEnemy = targetEnemy;

                    StartCoroutine(DefensiveTech(tech, targetEnemy));

                    yield return new WaitForSeconds(totalTime);

                    break;
                }

                case "Allies":
                {
                    for(int i = 0; i < encounter.tempEnemies.Count; i++)
                    {
                        if(encounter.tempEnemies[i].HP > 0)
                        {
                            selectedEnemy = encounter.tempEnemies[i];

                            StartCoroutine(DefensiveTech(tech, encounter.tempEnemies[i]));

                            yield return new WaitForSeconds(totalTime);
                        }
                    }

                    break;
                }

                case "Summon":
                {
                    StartCoroutine(DefensiveTech(tech, currentEnemy));

                    yield return new WaitForSeconds(totalTime);

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

    public IEnumerator OffensiveTech(TechData tech, CharacterData character)
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

                        StartCoroutine(AugmentText(character.sub, tech.type, character.characterName));

                        yield return new WaitForSeconds(time);

                        StartCoroutine(Subtech(tech, character.sub));

                        yield return new WaitForSeconds(time);

                        break;
                    }

                    case "Defence":
                    {
                        character.AugmentDefence(-currentEnemy.Support() - tech.energy - currentEnemy.boost);

                        StartCoroutine(AugmentText(character.sub, tech.type, character.characterName));

                        yield return new WaitForSeconds(time);

                        StartCoroutine(Subtech(tech, character.sub));

                        yield return new WaitForSeconds(time);

                        break;
                    }

                    case "Accuracy":
                    {
                        character.AugmentAccuracy(-currentEnemy.Support() - tech.energy - currentEnemy.boost);

                        StartCoroutine(AugmentText(character.sub, tech.type, character.characterName));

                        yield return new WaitForSeconds(time);

                        StartCoroutine(Subtech(tech, character.sub));

                        yield return new WaitForSeconds(time);

                        break;
                    }

                    case "Evasion":
                    {
                        character.AugmentEvasion(-currentEnemy.Support() - tech.energy - currentEnemy.boost);

                        StartCoroutine(AugmentText(character.sub, tech.type, character.characterName));

                        yield return new WaitForSeconds(time);

                        StartCoroutine(Subtech(tech, character.sub));

                        yield return new WaitForSeconds(time);

                        break;
                    }

                    case "Luck":
                    {
                        character.AugmentLuck(-currentEnemy.Support() - tech.energy - currentEnemy.boost);

                        StartCoroutine(AugmentText(character.sub, tech.type, character.characterName));

                        yield return new WaitForSeconds(time);

                        StartCoroutine(Subtech(tech, character.sub));

                        yield return new WaitForSeconds(time);

                        break;
                    }

                    case "Speed":
                    {
                        character.AugmentSpeed(-currentEnemy.Support() - tech.energy - currentEnemy.boost);

                        StartCoroutine(AugmentText(character.sub, tech.type, character.characterName));

                        yield return new WaitForSeconds(time);

                        StartCoroutine(Subtech(tech, character.sub));

                        yield return new WaitForSeconds(time);

                        break;
                    }

                    case "Energy Offence":
                    {
                        character.AugmentEnergyOffence(-currentEnemy.Support() - tech.energy - currentEnemy.boost);

                        StartCoroutine(AugmentText(character.sub, tech.type, character.characterName));

                        yield return new WaitForSeconds(time);

                        StartCoroutine(Subtech(tech, character.sub));

                        yield return new WaitForSeconds(time);

                        break;
                    }

                    case "Energy Defence":
                    {
                        character.AugmentEnergyDefence(-currentEnemy.Support() - tech.energy - currentEnemy.boost);

                        StartCoroutine(AugmentText(character.sub, tech.type, character.characterName));

                        yield return new WaitForSeconds(time);

                        StartCoroutine(Subtech(tech, character.sub));

                        yield return new WaitForSeconds(time);

                        break;
                    }

                    case "Support":
                    {
                        character.AugmentSupport(-currentEnemy.Support() - tech.energy - currentEnemy.boost);

                        StartCoroutine(AugmentText(character.sub, tech.type, character.characterName));

                        yield return new WaitForSeconds(time);

                        StartCoroutine(Subtech(tech, character.sub));

                        yield return new WaitForSeconds(time);

                        break;
                    }

                    case "Stamina":
                    {
                        character.AugmentStamina(-currentEnemy.Support() - tech.energy - currentEnemy.boost);

                        StartCoroutine(AugmentText(character.sub, tech.type, character.characterName));

                        yield return new WaitForSeconds(time);

                        StartCoroutine(Subtech(tech, character.sub));

                        yield return new WaitForSeconds(time);

                        break;
                    }

                    case "Vigour":
                    {
                        character.AugmentVigour(-currentEnemy.Support() - tech.energy - currentEnemy.boost);

                        StartCoroutine(AugmentText(character.sub, tech.type, character.characterName));

                        yield return new WaitForSeconds(time);

                        StartCoroutine(Subtech(tech, character.sub));

                        yield return new WaitForSeconds(time);

                        break;
                    }

                    case "Vitality":
                    {
                        character.AugmentVitality(-currentEnemy.Support() - tech.energy - currentEnemy.boost);

                        StartCoroutine(AugmentText(character.sub, tech.type, character.characterName));

                        yield return new WaitForSeconds(time);

                        StartCoroutine(Subtech(tech, character.sub));

                        yield return new WaitForSeconds(time);

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

                StartCoroutine(TechAttackText(character.sub, tech.type, character.characterName));

                yield return new WaitForSeconds(time);

                StartCoroutine(Subtech(tech, character.sub));

                yield return new WaitForSeconds(time);

                break;
            }

            case "Energy Attack":
            {
                character.EnergyAttack((currentEnemy.EnergyOffence() + tech.energy + currentEnemy.boost), tech.type);

                StartCoroutine(TechAttackText(character.sub, tech.type, character.characterName));

                yield return new WaitForSeconds(time);

                StartCoroutine(Subtech(tech, character.sub));

                yield return new WaitForSeconds(time);

                break;
            }

            case "Harm":
            {
                switch(tech.type)
                {
                    case "Health":
                    {
                        character.HarmHP(currentEnemy.Support() + tech.health + currentEnemy.boost);

                        StartCoroutine(HealthText(character.sub,character.characterName));

                        yield return new WaitForSeconds(time);

                        StartCoroutine(Subtech(tech, character.sub));

                        yield return new WaitForSeconds(time);

                        break;
                    }

                    case "Energy":
                    {
                        character.HarmEP(currentEnemy.Support() + tech.energy + currentEnemy.boost);

                        StartCoroutine(EnergyText(character.sub,character.characterName));

                        yield return new WaitForSeconds(time);

                        StartCoroutine(Subtech(tech, character.sub));

                        yield return new WaitForSeconds(time);

                        break;
                    }

                    case "Boost":
                    {
                        character.HarmBP(currentEnemy.Support() + tech.health + tech.energy + currentEnemy.boost);

                        StartCoroutine(BoostText(character.sub,character.characterName));

                        yield return new WaitForSeconds(time);

                        StartCoroutine(Subtech(tech, character.sub));

                        yield return new WaitForSeconds(time);

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

    public IEnumerator DefensiveTech(TechData tech, EnemyData ally)
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

                        StartCoroutine(AugmentText(ally.sub, tech.type, ally.enemyName));

                        yield return new WaitForSeconds(time);

                        StartCoroutine(Subtech(tech, ally.sub));

                        yield return new WaitForSeconds(time);

                        break;
                    }

                    case "Defence":
                    {
                        ally.AugmentDefence(currentEnemy.Support() + tech.energy + currentEnemy.boost);

                        StartCoroutine(AugmentText(ally.sub, tech.type, ally.enemyName));

                        yield return new WaitForSeconds(time);

                        StartCoroutine(Subtech(tech, ally.sub));

                        yield return new WaitForSeconds(time);

                        break;
                    }

                    case "Accuracy":
                    {
                        ally.AugmentAccuracy(currentEnemy.Support() + tech.energy + currentEnemy.boost);

                        StartCoroutine(AugmentText(ally.sub, tech.type, ally.enemyName));

                        yield return new WaitForSeconds(time);

                        StartCoroutine(Subtech(tech, ally.sub));

                        yield return new WaitForSeconds(time);

                        break;
                    }

                    case "Evasion":
                    {
                        ally.AugmentEvasion(currentEnemy.Support() + tech.energy + currentEnemy.boost);

                        StartCoroutine(AugmentText(ally.sub, tech.type, ally.enemyName));

                        yield return new WaitForSeconds(time);

                        StartCoroutine(Subtech(tech, ally.sub));

                        yield return new WaitForSeconds(time);

                        break;
                    }

                    case "Luck":
                    {
                        ally.AugmentLuck(currentEnemy.Support() + tech.energy + currentEnemy.boost);

                        StartCoroutine(AugmentText(ally.sub, tech.type, ally.enemyName));

                        yield return new WaitForSeconds(time);

                        StartCoroutine(Subtech(tech, ally.sub));

                        yield return new WaitForSeconds(time);

                        break;
                    }

                    case "Speed":
                    {
                        ally.AugmentSpeed(currentEnemy.Support() + tech.energy + currentEnemy.boost);

                        StartCoroutine(AugmentText(ally.sub, tech.type, ally.enemyName));

                        yield return new WaitForSeconds(time);

                        StartCoroutine(Subtech(tech, ally.sub));

                        yield return new WaitForSeconds(time);

                        break;
                    }

                    case "Energy Offence":
                    {
                        ally.AugmentEnergyOffence(currentEnemy.Support() + tech.energy + currentEnemy.boost);

                        StartCoroutine(AugmentText(ally.sub, tech.type, ally.enemyName));

                        yield return new WaitForSeconds(time);

                        StartCoroutine(Subtech(tech, ally.sub));

                        yield return new WaitForSeconds(time);

                        break;
                    }

                    case "Energy Defence":
                    {
                        ally.AugmentEnergyDefence(currentEnemy.Support() + tech.energy + currentEnemy.boost);

                        StartCoroutine(AugmentText(ally.sub, tech.type, ally.enemyName));

                        yield return new WaitForSeconds(time);

                        StartCoroutine(Subtech(tech, ally.sub));

                        yield return new WaitForSeconds(time);

                        break;
                    }

                    case "Support":
                    {
                        ally.AugmentSupport(currentEnemy.Support() + tech.energy + currentEnemy.boost);

                        StartCoroutine(AugmentText(ally.sub, tech.type, ally.enemyName));

                        yield return new WaitForSeconds(time);

                        StartCoroutine(Subtech(tech, ally.sub));

                        yield return new WaitForSeconds(time);

                        break;
                    }

                    case "Stamina":
                    {
                        ally.AugmentStamina(currentEnemy.Support() + tech.energy + currentEnemy.boost);

                        StartCoroutine(AugmentText(ally.sub, tech.type, ally.enemyName));

                        yield return new WaitForSeconds(time);

                        StartCoroutine(Subtech(tech, ally.sub));

                        yield return new WaitForSeconds(time);

                        break;
                    }

                    case "Vigour":
                    {
                        ally.AugmentVigour(currentEnemy.Support() + tech.energy + currentEnemy.boost);

                        StartCoroutine(AugmentText(ally.sub, tech.type, ally.enemyName));

                        yield return new WaitForSeconds(time);

                        StartCoroutine(Subtech(tech, ally.sub));

                        yield return new WaitForSeconds(time);

                        break;
                    }

                    case "Vitality":
                    {
                        ally.AugmentVitality(currentEnemy.Support() + tech.energy + currentEnemy.boost);

                        StartCoroutine(AugmentText(ally.sub, tech.type, ally.enemyName));

                        yield return new WaitForSeconds(time);

                        StartCoroutine(Subtech(tech, ally.sub));

                        yield return new WaitForSeconds(time);

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

                        StartCoroutine(HealthText(ally.sub, ally.enemyName));

                        yield return new WaitForSeconds(time);

                        StartCoroutine(Subtech(tech, ally.sub));

                        yield return new WaitForSeconds(time);

                        break;
                    }

                    case "Energy":
                    {
                        ally.HealEP(currentEnemy.Support() + tech.health + currentEnemy.boost);

                        StartCoroutine(HealthText(ally.sub, ally.enemyName));

                        yield return new WaitForSeconds(time);

                        StartCoroutine(Subtech(tech, ally.sub));

                        yield return new WaitForSeconds(time);

                        break;
                    }

                    case "Boost":
                    {
                        ally.HealBP(currentEnemy.Support() + tech.health + tech.energy + currentEnemy.boost);

                        StartCoroutine(HealthText(ally.sub, ally.enemyName));

                        yield return new WaitForSeconds(time);

                        StartCoroutine(Subtech(tech, ally.sub));

                        yield return new WaitForSeconds(time);

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
                StartCoroutine(Subtech(tech, 0));

                break;
            }
        }
    }

    public IEnumerator Subtech(TechData tech, int sub)
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

                    StartCoroutine(DefensiveSubtech(tech, selectedEnemy, sub));

                    yield return new WaitForSeconds(time);

                    break;
                }
                
                case "Enemy":
                {
                    StartCoroutine(OffensiveSubtech(tech, selectedCharacter, sub));

                    yield return new WaitForSeconds(time);

                    break;
                }

                case "Enemies":
                {
                    for(int i = 0; i < party.tempMembers.Count; i++)
                    {
                        if(party.tempMembers[i].HP > 0)
                        {
                            StartCoroutine(OffensiveSubtech(tech, party.tempMembers[i], sub));

                            yield return new WaitForSeconds(time);
                        }
                    }

                    break;
                }

                case "Ally":
                {
                    StartCoroutine(DefensiveSubtech(tech, selectedEnemy, sub));

                    yield return new WaitForSeconds(time);

                    break;
                }

                case "Allies":
                {
                    for(int i = 0; i < encounter.tempEnemies.Count; i++)
                    {
                        if(encounter.tempEnemies[i].HP > 0)
                        {
                            StartCoroutine(DefensiveSubtech(tech, encounter.tempEnemies[i], sub));

                            yield return new WaitForSeconds(time);
                        }
                    }

                    break;
                }

                case "Summon":
                {
                    StartCoroutine(DefensiveSubtech(tech, currentEnemy, 0));

                    yield return new WaitForSeconds(time);

                    break;
                }

                default:
                {
                    break;
                }
            }
        }
    }

    public IEnumerator OffensiveSubtech(TechData tech, CharacterData character, int sub)
    {
        switch (tech.subeffect)
        {
            case "Debuff":
            {
                switch (tech.subtype)
                {
                    case "Offence":
                    {
                        //sub = sub / 10;

                        character.AugmentOffence(-sub);

                        StartCoroutine(AugmentText(-sub, tech.subtype, character.characterName));

                        yield return new WaitForSeconds(time);

                        break;
                    }

                    case "Defence":
                    {
                        //sub = sub / 10;

                        character.AugmentDefence(-sub);

                        StartCoroutine(AugmentText(-sub, tech.subtype, character.characterName));

                        yield return new WaitForSeconds(time);

                        break;
                    }

                    case "Accuracy":
                    {
                        //sub = sub / 10;
                        
                        character.AugmentAccuracy(-sub);

                        StartCoroutine(AugmentText(-sub, tech.subtype, character.characterName));

                        yield return new WaitForSeconds(time);

                        break;
                    }

                    case "Evasion":
                    {
                        //sub = sub / 10;
                        
                        character.AugmentEvasion(-sub);

                        StartCoroutine(AugmentText(-sub, tech.subtype, character.characterName));

                        yield return new WaitForSeconds(time);

                        break;
                    }

                    case "Luck":
                    {
                        //sub = sub / 10;
                        
                        character.AugmentLuck(-sub);

                        StartCoroutine(AugmentText(-sub, tech.subtype, character.characterName));

                        yield return new WaitForSeconds(time);

                        break;
                    }

                    case "Speed":
                    {
                        //sub = sub / 10;
                        
                        character.AugmentSpeed(-sub);

                        StartCoroutine(AugmentText(-sub, tech.subtype, character.characterName));

                        yield return new WaitForSeconds(time);

                        break;
                    }

                    case "Energy Offence":
                    {
                        //sub = sub / 10;
                        
                        character.AugmentEnergyOffence(-sub);

                        StartCoroutine(AugmentText(-sub, tech.subtype, character.characterName));

                        yield return new WaitForSeconds(time);

                        break;
                    }

                    case "Energy Defence":
                    {
                        //sub = sub / 10;
                        
                        character.AugmentEnergyDefence(-sub);

                        StartCoroutine(AugmentText(-sub, tech.subtype, character.characterName));

                        yield return new WaitForSeconds(time);

                        break;
                    }

                    case "Support":
                    {
                        //sub = sub / 10;
                        
                        character.AugmentSupport(-sub);

                        StartCoroutine(AugmentText(-sub, tech.subtype, character.characterName));

                        yield return new WaitForSeconds(time);

                        break;
                    }

                    case "Stamina":
                    {
                        //sub = sub / 10;
                        
                        character.AugmentStamina(-sub);

                        StartCoroutine(AugmentText(-sub, tech.subtype, character.characterName));

                        yield return new WaitForSeconds(time);

                        break;
                    }

                    case "Vigour":
                    {
                        //sub = sub / 10;
                        
                        character.AugmentVigour(-sub);

                        StartCoroutine(AugmentText(-sub, tech.subtype, character.characterName));

                        yield return new WaitForSeconds(time);

                        break;
                    }

                    case "Vitality":
                    {
                        //sub = sub / 10;
                        
                        character.AugmentVitality(-sub);

                        StartCoroutine(AugmentText(-sub, tech.subtype, character.characterName));

                        yield return new WaitForSeconds(time);

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

                StartCoroutine(TechAttackText(sub, tech.subtype, character.characterName));

                yield return new WaitForSeconds(time);

                break;
            }

            case "Energy Attack":
            {
                character.EnergyAttack((currentEnemy.EnergyOffence() + tech.energy + currentEnemy.boost), tech.type);

                StartCoroutine(TechAttackText(sub, tech.subtype, character.characterName));

                yield return new WaitForSeconds(time);

                break;
            }

            case "Harm":
            {
                switch(tech.subtype)
                {
                    case "Health":
                    {
                        //sub = sub / 100;
                        
                        character.HarmHP(sub);

                        StartCoroutine(HealthText(-sub, character.characterName));

                        yield return new WaitForSeconds(time);

                        break;
                    }

                    case "Energy":
                    {
                        //sub = sub / 100;

                        character.HarmEP(sub);

                        StartCoroutine(EnergyText(-sub, character.characterName));

                        yield return new WaitForSeconds(time);

                        break;
                    }

                    case "Boost":
                    {
                        //sub = sub / 100;

                        character.HarmBP(sub);

                        StartCoroutine(BoostText(-sub, character.characterName));

                        yield return new WaitForSeconds(time);

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

    public IEnumerator DefensiveSubtech(TechData tech, EnemyData ally, int sub)
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

                        StartCoroutine(AugmentText(sub, tech.subtype, ally.enemyName));

                        yield return new WaitForSeconds(time);

                        break;
                    }

                    case "Defence":
                    {
                        ally.AugmentDefence(sub);

                        StartCoroutine(AugmentText(sub, tech.subtype, ally.enemyName));

                        yield return new WaitForSeconds(time);

                        break;
                    }

                    case "Accuracy":
                    {
                        ally.AugmentAccuracy(sub);

                        StartCoroutine(AugmentText(sub, tech.subtype, ally.enemyName));

                        yield return new WaitForSeconds(time);

                        break;
                    }

                    case "Evasion":
                    {
                        ally.AugmentEvasion(sub);

                        StartCoroutine(AugmentText(sub, tech.subtype, ally.enemyName));

                        yield return new WaitForSeconds(time);

                        break;
                    }

                    case "Luck":
                    {
                        ally.AugmentLuck(sub);

                        StartCoroutine(AugmentText(sub, tech.subtype, ally.enemyName));

                        yield return new WaitForSeconds(time);

                        break;
                    }

                    case "Speed":
                    {
                        ally.AugmentSpeed(sub);

                        StartCoroutine(AugmentText(sub, tech.subtype, ally.enemyName));

                        yield return new WaitForSeconds(time);

                        break;
                    }

                    case "Energy Offence":
                    {
                        ally.AugmentEnergyOffence(sub);

                        StartCoroutine(AugmentText(sub, tech.subtype, ally.enemyName));

                        yield return new WaitForSeconds(time);

                        break;
                    }

                    case "Energy Defence":
                    {
                        ally.AugmentEnergyDefence(sub);

                        StartCoroutine(AugmentText(sub, tech.subtype, ally.enemyName));

                        yield return new WaitForSeconds(time);

                        break;
                    }

                    case "Support":
                    {
                        ally.AugmentSupport(sub);

                        StartCoroutine(AugmentText(sub, tech.subtype, ally.enemyName));

                        yield return new WaitForSeconds(time);

                        break;
                    }

                    case "Stamina":
                    {
                        ally.AugmentStamina(sub);

                        StartCoroutine(AugmentText(sub, tech.subtype, ally.enemyName));

                        yield return new WaitForSeconds(time);

                        break;
                    }

                    case "Vigour":
                    {
                        ally.AugmentVigour(sub);

                        StartCoroutine(AugmentText(sub, tech.subtype, ally.enemyName));

                        yield return new WaitForSeconds(time);

                        break;
                    }

                    case "Vitality":
                    {
                        ally.AugmentVitality(sub);

                        StartCoroutine(AugmentText(sub, tech.subtype, ally.enemyName));

                        yield return new WaitForSeconds(time);

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

                        StartCoroutine(HealthText(sub, ally.enemyName));

                        yield return new WaitForSeconds(time);

                        break;
                    }

                    case "Energy":
                    {
                        ally.HealEP(sub);

                        StartCoroutine(EnergyText(sub, ally.enemyName));

                        yield return new WaitForSeconds(time);

                        break;
                    }

                    case "Boost":
                    {
                        ally.HealBP(sub);

                        StartCoroutine(BoostText(sub, ally.enemyName));

                        yield return new WaitForSeconds(time);

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
