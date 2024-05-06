using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleMenu : MonoBehaviour
{
    public List<Button> targetButton;
    public List<TMP_Text> targetText;
    public List<Button> allyButton;
    public List<TMP_Text> allyText;
    public List<Button> techButton;
    public List<TMP_Text> techText;
    public List<Button> inventoryButton;
    public List<TMP_Text> inventoryText;
    public List<GameObject> characterUI;
    public GameObject battleUI, targetMenu, techMenu, inventoryMenu, tacticMenu, boostMenu;
    public TMP_Text boostText;
    public Slider boostSlider;
    public bool newTurn;
    public int targetNo = 0, techNo = 0, subNo = 0;
    private float time = 3.0f;
    public CharacterData currentChar, selectedCharacter;
    public EnemyData selectedEnemy;
    public TechData selectedTech;
    public ItemData selectedItem;

    public PartyManager party;
    public EncounterManager encounter;
    public InventoryManager inventory;
    public BattleManager battle;
    
    // Start is called before the first frame update
    void Start()
    {
        party = GameDataManager.instance.party;
        inventory = GameDataManager.instance.inventory;
        encounter = GameDataManager.instance.encounter;
    }

    public void CharacterTurn(int charNo)
    {
        currentChar = party.tempMembers[charNo];
        Debug.Log(currentChar.characterName);

        for(int i = 0; i < currentChar.techs.Count; i++)
        {
            techText[i].text = currentChar.techs[i].title;
            techButton[i].gameObject.SetActive(true);
            techButton[i].onClick.RemoveAllListeners();
            techButton[i].onClick.AddListener(() => SelectTech(currentChar.techs[techNo]));
            techButton[i].onClick.AddListener(() => techMenu.SetActive(false));
        }
        battleUI.gameObject.SetActive(true);
    }

    public void TargetInt(int targetNumber)
    {
        targetNo = targetNumber;
    }

    public void TechInt(int techNumber)
    {
        techNo = techNumber;
    }

    public void Attack()
    {
        for(int i = 0; i < encounter.tempEnemies.Count; i++)
        {
            if(targetButton[i].gameObject.tag == "Enemy" && encounter.tempEnemies[i].HP > 0)
            {
                targetText[i].text = encounter.tempEnemies[i].enemyName;
                targetButton[i].gameObject.SetActive(true);
                targetButton[i].onClick.RemoveAllListeners();
                targetButton[i].onClick.AddListener(() => encounter.tempEnemies[targetNo].Attack((currentChar.Offence() + currentChar.boost), (currentChar.Accuracy() + currentChar.boost), (currentChar.Luck() + currentChar.boost), currentChar.weaponSlot.type));
                targetButton[i].onClick.AddListener(() => targetMenu.SetActive(false));
                targetButton[i].onClick.AddListener(() => StartCoroutine(AttackText(encounter.tempEnemies[targetNo].sub, encounter.tempEnemies[targetNo].enemyName)));
            }
            else
            {
                targetButton[i].gameObject.SetActive(false);
            }
        }
        targetMenu.SetActive(true);
    }

    IEnumerator AttackText(int damage, string enemyName)
    {
        battle.textBox.SetActive(true);

        if(damage <= 0)
        {
            battle.text.text = currentChar.characterName + " Misses!";
        }
        else
        {
            battle.text.text = currentChar.characterName + " dealt " + damage.ToString() + " " + currentChar.weaponSlot.type + " damage to " + enemyName;
        }
        
        yield return new WaitForSeconds(time);

        //battle.text.text = "";

        //battle.textBox.SetActive(false);

        EndTurn();
    }

    IEnumerator TechAttackText(int damage, string damageType, string enemyName)
    {
        battle.textBox.SetActive(true);
        
        if(damage <= 0)
        {
            battle.text.text = currentChar.characterName + " Misses!";
        }
        else
        {
            battle.text.text = currentChar.characterName + " dealt " + damage.ToString() + " " + damageType + " damage to " + enemyName;
        }
        
        yield return new WaitForSeconds(time);

        //battle.text.text = "";

        //battle.textBox.SetActive(false);

        //EndTurn();
    }

    IEnumerator AugmentText(int augment, string stat, string targetName)
    {
        battle.textBox.SetActive(true);
        
        if(augment <= 0)
        {
            battle.text.text = currentChar.characterName + " decreases " + targetName + "'s " + stat + " by " + augment.ToString();
        }
        else
        {
            battle.text.text = currentChar.characterName + " increases " + targetName + "'s " + stat + " by " + augment.ToString();
        }
        
        yield return new WaitForSeconds(time);

        //battle.text.text = "";

        //battle.textBox.SetActive(false);

        //EndTurn();
    }

    IEnumerator HealthText(int augment, string targetName)
    {
        battle.textBox.SetActive(true);
        
        if(augment <= 0)
        {
            battle.text.text = currentChar.characterName + " harms " + targetName + "'s HP by " + augment.ToString();
        }
        else
        {
            battle.text.text = currentChar.characterName + " heals " + targetName + "'s HP by " + augment.ToString();
        }
        
        yield return new WaitForSeconds(time);

        //battle.text.text = "";

        //battle.textBox.SetActive(false);

        //EndTurn();
    }

    IEnumerator EnergyText(int augment, string targetName)
    {
        battle.textBox.SetActive(true);
        
        if(augment <= 0)
        {
            battle.text.text = currentChar.characterName + " harms " + targetName + "'s EP by " + augment.ToString();
        }
        else
        {
            battle.text.text = currentChar.characterName + " heals " + targetName + "'s EP by " + augment.ToString();
        }
        
        yield return new WaitForSeconds(time);

        //battle.text.text = "";

        //battle.textBox.SetActive(false);

        //EndTurn();
    }

    IEnumerator BoostText(int augment, string targetName)
    {
        battle.textBox.SetActive(true);
        
        if(augment <= 0)
        {
            battle.text.text = currentChar.characterName + " harms " + targetName + "'s BP by " + augment.ToString();
        }
        else
        {
            battle.text.text = currentChar.characterName + " heals " + targetName + "'s BP by " + augment.ToString();
        }
        
        yield return new WaitForSeconds(time);

        //battle.text.text = "";

        //battle.textBox.SetActive(false);

        //EndTurn();
    }

    public void SelectTech(TechData tech)
    {
        selectedTech = tech;
        switch(tech.target)
        {
            case "Self":
            {
                StartCoroutine(Tech(tech, currentChar.combattantScript));
                //EndTurn();
                break;
            }
            
            case "Enemy":
            {
                for(int i = 0; i < encounter.tempEnemies.Count; i++)
                {
                    if(targetButton[i].gameObject.tag == "Enemy" && encounter.tempEnemies[i].HP > 0)
                    {
                        targetText[i].text = encounter.tempEnemies[i].enemyName;
                        targetButton[i].gameObject.SetActive(true);
                        targetButton[i].onClick.RemoveAllListeners();
                        targetButton[i].onClick.AddListener(() => StartCoroutine(Tech(tech, encounter.tempEnemies[targetNo].combattantScript)));
                        targetButton[i].onClick.AddListener(() => targetMenu.SetActive(false));
                        //targetButton[i].onClick.AddListener(() => EndTurn());
                    }
                    else
                    {
                        targetButton[i].gameObject.SetActive(false);
                    }
                }
                targetMenu.SetActive(true);

                break;
            }

            case "Enemies":
            {
                StartCoroutine(Tech(tech, currentChar.combattantScript));
                //EndTurn();
                break;
            }

            case "Ally":
            {   
                for(int i = 0; i < party.tempMembers.Count; i++)
                {
                    if(allyButton[i].gameObject.tag == "Ally" && party.tempMembers[i].HP > 0)
                    {
                        //Debug.Log("target ally");

                        allyText[i].text = party.tempMembers[i].characterName;
                        allyButton[i].gameObject.SetActive(true);
                        allyButton[i].onClick.RemoveAllListeners();
                        allyButton[i].onClick.AddListener(() => StartCoroutine(Tech(tech, party.tempMembers[targetNo].combattantScript)));
                        allyButton[i].onClick.AddListener(() => targetMenu.SetActive(false));
                        //allyButton[i].onClick.AddListener(() => EndTurn());
                    }
                    else
                    {
                        allyButton[i].gameObject.SetActive(false);
                    }
                }
                targetMenu.SetActive(true);

                break;
            }

            case "Allies":
            {
                StartCoroutine(Tech(tech, currentChar.combattantScript));
                //EndTurn();
                break;
            }

            case "Dead Ally":
            {
                for(int i = 0; i < party.tempMembers.Count; i++)
                {
                    if(allyButton[i].gameObject.tag == "Ally" && party.tempMembers[i].HP <= 0)
                    {
                        allyText[i].text = party.tempMembers[i].characterName;
                        allyButton[i].gameObject.SetActive(true);
                        allyButton[i].onClick.RemoveAllListeners();
                        allyButton[i].onClick.AddListener(() => StartCoroutine(Tech(tech, party.tempMembers[targetNo].combattantScript)));
                        allyButton[i].onClick.AddListener(() => targetMenu.SetActive(false));
                        //allyButton[i].onClick.AddListener(() => EndTurn());
                    }
                    else
                    {
                        allyButton[i].gameObject.SetActive(false);
                    }
                }
                targetMenu.SetActive(true);
                break;
            }

            case "Dead Allies":
            {
                StartCoroutine(Tech(tech, currentChar.combattantScript));
                //EndTurn();
                break;
            }

            case "Summon":
            {
                StartCoroutine(Tech(tech, currentChar.combattantScript));
                //EndTurn();
                break;
            }
        }
    }

    public void EndTurn()
    {
        battle.text.text = "";
        battle.textBox.SetActive(false);
        currentChar.BP -= currentChar.boost;
        currentChar.boost = 0;

        for(int i = 0; i < currentChar.techs.Count; i++)
        {
            techText[i].text = "";
            techButton[i].gameObject.SetActive(false);
            techButton[i].onClick.RemoveAllListeners();
        }

        currentChar = null;
        battle.nextTurn = true;
    }

    public void SpendPoints(TechData tech, CharacterData character)
    {
        currentChar.HarmHP(tech.health);
        currentChar.HarmEP(tech.energy);
        currentChar.HarmBP(tech.boosts);
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

        if(tech.health < currentChar.HP && tech.energy < currentChar.EP && tech.boosts < currentChar.BP)
        {
            SpendPoints(tech, currentChar);

            switch(tech.target)
            {
                case "Self":
                {
                    Debug.Log("target self");
                    
                    selectedCharacter = currentChar;

                    StartCoroutine(DefensiveTech(tech, selectedCharacter));

                    yield return new WaitForSeconds(totalTime);

                    break;
                }

                case "Enemy":
                {
                    Debug.Log("target enemy");
                    
                    EnemyData targetEnemy = combattant.gameObject.GetComponent<EnemyData>();

                    selectedEnemy = targetEnemy;

                    StartCoroutine(OffensiveTech(tech, targetEnemy));

                    yield return new WaitForSeconds(totalTime);

                    break;
                }

                case "Enemies":
                {
                    for(int i = 0; i < encounter.tempEnemies.Count; i++)
                    {
                        if(encounter.tempEnemies[i].HP > 0)
                        {
                            Debug.Log("target enemy");
                            
                            selectedEnemy = encounter.tempEnemies[i];
                            
                            StartCoroutine(OffensiveTech(tech, encounter.tempEnemies[i]));

                            yield return new WaitForSeconds(totalTime);
                        }
                    }

                    break;
                }

                case "Ally":
                {
                    Debug.Log("target ally");

                    CharacterData targetAlly = combattant.gameObject.GetComponent<CharacterData>();

                    selectedCharacter = targetAlly;

                    StartCoroutine(DefensiveTech(tech, targetAlly));

                    yield return new WaitForSeconds(totalTime);

                    break;
                }

                case "Allies":
                {
                    for(int i = 0; i < party.tempMembers.Count; i++)
                    {
                        if(party.tempMembers[i].HP > 0)
                        {
                            Debug.Log("target ally");

                            selectedCharacter = party.tempMembers[i];

                            StartCoroutine(DefensiveTech(tech, party.tempMembers[i]));

                            yield return new WaitForSeconds(totalTime);
                        }
                    }

                    break;
                }

                case "Dead Ally":
                {
                    Debug.Log("target ally");
                    
                    CharacterData targetAlly = combattant.gameObject.GetComponent<CharacterData>();

                    selectedCharacter = targetAlly;
                    
                    StartCoroutine(DefensiveTech(tech, targetAlly));

                    yield return new WaitForSeconds(totalTime);

                    break;
                }

                case "Dead Allies":
                {
                    for(int i = 0; i < party.tempMembers.Count; i++)
                    {
                        if(party.tempMembers[i].HP < 0)
                        {
                            Debug.Log("target ally");
                            
                            selectedCharacter = party.tempMembers[i];
                            
                            StartCoroutine(DefensiveTech(tech, party.tempMembers[i]));

                            yield return new WaitForSeconds(totalTime);
                        }
                    }

                    break;
                }

                case "Summon":
                {
                    StartCoroutine(DefensiveTech(tech, currentChar));

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

    public IEnumerator OffensiveTech(TechData tech, EnemyData enemy)
    {
        switch (tech.effect)
        {
            case "Debuff":
            {
                switch (tech.type)
                {
                    case "Offence":
                    {
                        enemy.AugmentOffence(-currentChar.Support() - tech.energy - currentChar.boost);

                        StartCoroutine(AugmentText(enemy.sub, tech.type, enemy.enemyName));

                        yield return new WaitForSeconds(time);
                        
                        StartCoroutine(Subtech(tech, enemy.sub));

                        yield return new WaitForSeconds(time);

                        break;
                    }

                    case "Defence":
                    {
                        enemy.AugmentDefence(-currentChar.Support() - tech.energy - currentChar.boost);

                        StartCoroutine(AugmentText(enemy.sub, tech.type, enemy.enemyName));

                        yield return new WaitForSeconds(time);

                        StartCoroutine(Subtech(tech, enemy.sub));

                        yield return new WaitForSeconds(time);

                        break;
                    }

                    case "Accuracy":
                    {
                        enemy.AugmentAccuracy(-currentChar.Support() - tech.energy - currentChar.boost);

                        StartCoroutine(AugmentText(enemy.sub, tech.type, enemy.enemyName));

                        yield return new WaitForSeconds(time);

                        StartCoroutine(Subtech(tech, enemy.sub));

                        yield return new WaitForSeconds(time);

                        break;
                    }

                    case "Evasion":
                    {
                        enemy.AugmentEvasion(-currentChar.Support() - tech.energy - currentChar.boost);

                        StartCoroutine(AugmentText(enemy.sub, tech.type, enemy.enemyName));

                        yield return new WaitForSeconds(time);

                        StartCoroutine(Subtech(tech, enemy.sub));

                        yield return new WaitForSeconds(time);

                        break;
                    }

                    case "Luck":
                    {
                        enemy.AugmentLuck(-currentChar.Support() - tech.energy - currentChar.boost);

                        StartCoroutine(AugmentText(enemy.sub, tech.type, enemy.enemyName));

                        yield return new WaitForSeconds(time);

                        StartCoroutine(Subtech(tech, enemy.sub));

                        yield return new WaitForSeconds(time);

                        break;
                    }

                    case "Speed":
                    {
                        enemy.AugmentSpeed(-currentChar.Support() - tech.energy - currentChar.boost);

                        StartCoroutine(AugmentText(enemy.sub, tech.type, enemy.enemyName));

                        yield return new WaitForSeconds(time);

                        StartCoroutine(Subtech(tech, enemy.sub));

                        yield return new WaitForSeconds(time);

                        break;
                    }

                    case "Energy Offence":
                    {
                        enemy.AugmentEnergyOffence(-currentChar.Support() - tech.energy - currentChar.boost);

                        StartCoroutine(AugmentText(enemy.sub, tech.type, enemy.enemyName));

                        yield return new WaitForSeconds(time);

                        StartCoroutine(Subtech(tech, enemy.sub));

                        yield return new WaitForSeconds(time);

                        break;
                    }

                    case "Energy Defence":
                    {
                        enemy.AugmentEnergyDefence(-currentChar.Support() - tech.energy - currentChar.boost);

                        StartCoroutine(AugmentText(enemy.sub, tech.type, enemy.enemyName));

                        yield return new WaitForSeconds(time);

                        StartCoroutine(Subtech(tech, enemy.sub));

                        yield return new WaitForSeconds(time);

                        break;
                    }

                    case "Support":
                    {
                        enemy.AugmentSupport(-currentChar.Support() - tech.energy - currentChar.boost);

                        StartCoroutine(AugmentText(enemy.sub, tech.type, enemy.enemyName));

                        yield return new WaitForSeconds(time);

                        StartCoroutine(Subtech(tech, enemy.sub));

                        yield return new WaitForSeconds(time);

                        break;
                    }

                    case "Stamina":
                    {
                        enemy.AugmentStamina(-currentChar.Support() - tech.energy - currentChar.boost);

                        StartCoroutine(AugmentText(enemy.sub, tech.type, enemy.enemyName));

                        yield return new WaitForSeconds(time);

                        StartCoroutine(Subtech(tech, enemy.sub));

                        yield return new WaitForSeconds(time);

                        break;
                    }

                    case "Vigour":
                    {
                        enemy.AugmentVigour(-currentChar.Support() - tech.energy - currentChar.boost);

                        StartCoroutine(AugmentText(enemy.sub, tech.type, enemy.enemyName));

                        yield return new WaitForSeconds(time);

                        StartCoroutine(Subtech(tech, enemy.sub));

                        yield return new WaitForSeconds(time);

                        break;
                    }

                    case "Vitality":
                    {
                        enemy.AugmentVitality(-currentChar.Support() - tech.energy - currentChar.boost);

                        StartCoroutine(AugmentText(enemy.sub, tech.type, enemy.enemyName));

                        yield return new WaitForSeconds(time);

                        StartCoroutine(Subtech(tech, enemy.sub));

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
                Debug.Log("Attack Enemy");

                enemy.PhysicalAttack((currentChar.Offence() + tech.energy + currentChar.boost), (currentChar.Accuracy() + tech.energy + currentChar.boost), (currentChar.Luck() + tech.energy + currentChar.boost), tech.type);

                StartCoroutine(TechAttackText(enemy.sub, tech.type, enemy.enemyName));

                yield return new WaitForSeconds(time);

                StartCoroutine(Subtech(tech, enemy.sub));

                yield return new WaitForSeconds(time);

                break;
            }

            case "Energy Attack":
            {
                Debug.Log("Attack Enemy");

                enemy.EnergyAttack((currentChar.EnergyOffence() + tech.energy + currentChar.boost), tech.type);

                StartCoroutine(TechAttackText(enemy.sub, tech.type, enemy.enemyName));

                yield return new WaitForSeconds(time);

                StartCoroutine(Subtech(tech, enemy.sub));

                yield return new WaitForSeconds(time);

                break;
            }

            case "Harm":
            {
                switch(tech.type)
                {
                    case "Health":
                    {
                        Debug.Log("Harm Enemy");

                        enemy.HarmHP(currentChar.Support() + tech.health + currentChar.boost);

                        StartCoroutine(HealthText(enemy.sub, enemy.enemyName));

                        yield return new WaitForSeconds(time);

                        StartCoroutine(Subtech(tech, enemy.sub));

                        yield return new WaitForSeconds(time);

                        break;
                    }

                    case "Energy":
                    {
                        Debug.Log("Harm Enemy");

                        enemy.HarmEP(currentChar.Support() + tech.energy + currentChar.boost);

                        StartCoroutine(EnergyText(enemy.sub, enemy.enemyName));

                        yield return new WaitForSeconds(time);

                        StartCoroutine(Subtech(tech, enemy.sub));

                        yield return new WaitForSeconds(time);

                        break;
                    }

                    case "Boost":
                    {
                        Debug.Log("Harm Enemy");

                        enemy.HarmBP(currentChar.Support() + tech.health + tech.energy + currentChar.boost);

                        StartCoroutine(BoostText(enemy.sub, enemy.enemyName));

                        yield return new WaitForSeconds(time);

                        StartCoroutine(Subtech(tech, enemy.sub));

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

    public IEnumerator DefensiveTech(TechData tech, CharacterData ally)
    {
        switch (tech.effect)
        {
            case "Buff":
            {
                switch (tech.type)
                {
                    case "Offence":
                    {
                        ally.AugmentOffence(currentChar.Support() + tech.energy + currentChar.boost);

                        StartCoroutine(AugmentText(ally.sub, tech.type, ally.characterName));

                        yield return new WaitForSeconds(time);

                        StartCoroutine(Subtech(tech, ally.sub));

                        yield return new WaitForSeconds(time);

                        break;
                    }

                    case "Defence":
                    {
                        ally.AugmentDefence(currentChar.Support() + tech.energy + currentChar.boost);

                        StartCoroutine(AugmentText(ally.sub, tech.type, ally.characterName));

                        yield return new WaitForSeconds(time);

                        StartCoroutine(Subtech(tech, ally.sub));

                        yield return new WaitForSeconds(time);

                        break;
                    }

                    case "Accuracy":
                    {
                        ally.AugmentAccuracy(currentChar.Support() + tech.energy + currentChar.boost);

                        StartCoroutine(AugmentText(ally.sub, tech.type, ally.characterName));

                        yield return new WaitForSeconds(time);

                        StartCoroutine(Subtech(tech, ally.sub));

                        yield return new WaitForSeconds(time);

                        break;
                    }

                    case "Evasion":
                    {
                        ally.AugmentEvasion(currentChar.Support() + tech.energy + currentChar.boost);

                        StartCoroutine(AugmentText(ally.sub, tech.type, ally.characterName));

                        yield return new WaitForSeconds(time);

                        StartCoroutine(Subtech(tech, ally.sub));

                        yield return new WaitForSeconds(time);

                        break;
                    }

                    case "Luck":
                    {
                        ally.AugmentLuck(currentChar.Support() + tech.energy + currentChar.boost);

                        StartCoroutine(AugmentText(ally.sub, tech.type, ally.characterName));

                        yield return new WaitForSeconds(time);

                        StartCoroutine(Subtech(tech, ally.sub));

                        yield return new WaitForSeconds(time);

                        break;
                    }

                    case "Speed":
                    {
                        ally.AugmentSpeed(currentChar.Support() + tech.energy + currentChar.boost);

                        StartCoroutine(AugmentText(ally.sub, tech.type, ally.characterName));

                        yield return new WaitForSeconds(time);

                        StartCoroutine(Subtech(tech, ally.sub));

                        yield return new WaitForSeconds(time);

                        break;
                    }

                    case "Energy Offence":
                    {
                        ally.AugmentEnergyOffence(currentChar.Support() + tech.energy + currentChar.boost);

                        StartCoroutine(AugmentText(ally.sub, tech.type, ally.characterName));

                        yield return new WaitForSeconds(time);

                        StartCoroutine(Subtech(tech, ally.sub));

                        yield return new WaitForSeconds(time);

                        break;
                    }

                    case "Energy Defence":
                    {
                        ally.AugmentEnergyDefence(currentChar.Support() + tech.energy + currentChar.boost);

                        StartCoroutine(AugmentText(ally.sub, tech.type, ally.characterName));

                        yield return new WaitForSeconds(time);

                        StartCoroutine(Subtech(tech, ally.sub));

                        yield return new WaitForSeconds(time);

                        break;
                    }

                    case "Support":
                    {
                        ally.AugmentSupport(currentChar.Support() + tech.energy + currentChar.boost);

                        StartCoroutine(AugmentText(ally.sub, tech.type, ally.characterName));

                        yield return new WaitForSeconds(time);

                        StartCoroutine(Subtech(tech, ally.sub));

                        yield return new WaitForSeconds(time);

                        break;
                    }

                    case "Stamina":
                    {
                        ally.AugmentStamina(currentChar.Support() + tech.energy + currentChar.boost);

                        StartCoroutine(AugmentText(ally.sub, tech.type, ally.characterName));

                        yield return new WaitForSeconds(time);

                        StartCoroutine(Subtech(tech, ally.sub));

                        yield return new WaitForSeconds(time);

                        break;
                    }

                    case "Vigour":
                    {
                        ally.AugmentVigour(currentChar.Support() + tech.energy + currentChar.boost);

                        StartCoroutine(AugmentText(ally.sub, tech.type, ally.characterName));

                        yield return new WaitForSeconds(time);

                        StartCoroutine(Subtech(tech, ally.sub));

                        yield return new WaitForSeconds(time);

                        break;
                    }

                    case "Vitality":
                    {
                        ally.AugmentVitality(currentChar.Support() + tech.energy + currentChar.boost);

                        StartCoroutine(AugmentText(ally.sub, tech.type, ally.characterName));

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
                        Debug.Log("Heal Ally");

                        ally.HealHP(currentChar.Support() + tech.energy + currentChar.boost);

                        StartCoroutine(HealthText(ally.sub, ally.characterName));

                        yield return new WaitForSeconds(time);

                        StartCoroutine(Subtech(tech, ally.sub));

                        yield return new WaitForSeconds(time);

                        break;
                    }

                    case "Energy":
                    {
                        Debug.Log("Heal Ally");

                        ally.HealEP(currentChar.Support() + tech.health + currentChar.boost);

                        StartCoroutine(HealthText(ally.sub, ally.characterName));

                        yield return new WaitForSeconds(time);

                        StartCoroutine(Subtech(tech, ally.sub));

                        yield return new WaitForSeconds(time);

                        break;
                    }

                    case "Boost":
                    {
                        Debug.Log("Heal Ally");

                        ally.HealBP(currentChar.Support() + tech.health + tech.energy + currentChar.boost);

                        StartCoroutine(HealthText(ally.sub, ally.characterName));

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
                    selectedCharacter = currentChar;

                    StartCoroutine(DefensiveSubtech(tech, selectedCharacter, sub));

                    yield return new WaitForSeconds(time);

                    break;
                }
                
                case "Enemy":
                {
                    StartCoroutine(OffensiveSubtech(tech, selectedEnemy, sub));

                    yield return new WaitForSeconds(time);

                    break;
                }

                case "Enemies":
                {
                    for(int i = 0; i < encounter.tempEnemies.Count; i++)
                    {
                        if(encounter.tempEnemies[i].HP > 0)
                        {
                            StartCoroutine(OffensiveSubtech(tech, encounter.tempEnemies[i], sub));

                            yield return new WaitForSeconds(time);
                        }
                    }

                    break;
                }

                case "Ally":
                {
                    StartCoroutine(DefensiveSubtech(tech, selectedCharacter, sub));

                    yield return new WaitForSeconds(time);

                    break;
                }

                case "Allies":
                {
                    for(int i = 0; i < party.tempMembers.Count; i++)
                    {
                        if(party.tempMembers[i].HP > 0)
                        {
                            StartCoroutine(DefensiveSubtech(tech, party.tempMembers[i], sub));

                            yield return new WaitForSeconds(time);
                        }
                    }

                    break;
                }

                case "Dead Ally":
                {
                    StartCoroutine(DefensiveSubtech(tech, selectedCharacter, sub));

                    yield return new WaitForSeconds(time);

                    break;
                }

                case "Dead Allies":
                {
                    for(int i = 0; i < party.tempMembers.Count; i++)
                    {
                        if(party.tempMembers[i].HP < 0)
                        {
                            StartCoroutine(DefensiveSubtech(tech, party.tempMembers[i], sub));

                            yield return new WaitForSeconds(time);
                        }
                    }

                    break;
                }

                case "Summon":
                {
                    StartCoroutine(DefensiveSubtech(tech, currentChar, 0));

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

    public IEnumerator OffensiveSubtech(TechData tech, EnemyData enemy, int sub)
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

                        enemy.AugmentOffence(-sub);

                        StartCoroutine(AugmentText(-sub, tech.subtype, enemy.enemyName));
                        
                        yield return new WaitForSeconds(time);

                        break;
                    }

                    case "Defence":
                    {
                        //sub = sub / 10;

                        enemy.AugmentDefence(-sub);

                        StartCoroutine(AugmentText(-sub, tech.subtype, enemy.enemyName));

                        yield return new WaitForSeconds(time);

                        break;
                    }

                    case "Accuracy":
                    {
                        //sub = sub / 10;
                        
                        enemy.AugmentAccuracy(-sub);

                        StartCoroutine(AugmentText(-sub, tech.subtype, enemy.enemyName));

                        yield return new WaitForSeconds(time);

                        break;
                    }

                    case "Evasion":
                    {
                        //sub = sub / 10;
                        
                        enemy.AugmentEvasion(-sub);

                        StartCoroutine(AugmentText(-sub, tech.subtype, enemy.enemyName));

                        yield return new WaitForSeconds(time);

                        break;
                    }

                    case "Luck":
                    {
                        //sub = sub / 10;
                        
                        enemy.AugmentLuck(-sub);

                        StartCoroutine(AugmentText(-sub, tech.subtype, enemy.enemyName));

                        yield return new WaitForSeconds(time);

                        break;
                    }

                    case "Speed":
                    {
                        //sub = sub / 10;
                        
                        enemy.AugmentSpeed(-sub);

                        StartCoroutine(AugmentText(-sub, tech.subtype, enemy.enemyName));

                        yield return new WaitForSeconds(time);

                        break;
                    }

                    case "Energy Offence":
                    {
                        //sub = sub / 10;
                        
                        enemy.AugmentEnergyOffence(-sub);

                        StartCoroutine(AugmentText(-sub, tech.subtype, enemy.enemyName));

                        yield return new WaitForSeconds(time);

                        break;
                    }

                    case "Energy Defence":
                    {
                        //sub = sub / 10;
                        
                        enemy.AugmentEnergyDefence(-sub);

                        StartCoroutine(AugmentText(-sub, tech.subtype, enemy.enemyName));

                        yield return new WaitForSeconds(time);

                        break;
                    }

                    case "Support":
                    {
                        //sub = sub / 10;
                        
                        enemy.AugmentSupport(-sub);

                        StartCoroutine(AugmentText(-sub, tech.subtype, enemy.enemyName));

                        yield return new WaitForSeconds(time);

                        break;
                    }

                    case "Stamina":
                    {
                        //sub = sub / 10;

                        enemy.AugmentStamina(-sub);
                        
                        StartCoroutine(AugmentText(-sub, tech.subtype, enemy.enemyName));

                        yield return new WaitForSeconds(time);

                        break;
                    }

                    case "Vigour":
                    {
                        //sub = sub / 10;
                        
                        enemy.AugmentVigour(-sub);

                        StartCoroutine(AugmentText(-sub, tech.subtype, enemy.enemyName));

                        yield return new WaitForSeconds(time);

                        break;
                    }

                    case "Vitality":
                    {
                        //sub = sub / 10;
                        
                        enemy.AugmentVitality(-sub);

                        StartCoroutine(AugmentText(-sub, tech.subtype, enemy.enemyName));

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
                enemy.PhysicalAttack((currentChar.Offence() + tech.energy + currentChar.boost), (currentChar.Accuracy() + tech.energy + currentChar.boost), (currentChar.Luck() + tech.energy + currentChar.boost), tech.type);

                StartCoroutine(TechAttackText(sub, tech.subtype, enemy.enemyName));

                yield return new WaitForSeconds(time);

                break;
            }

            case "Energy Attack":
            {
                enemy.EnergyAttack((currentChar.EnergyOffence() + tech.energy + currentChar.boost), tech.type);

                StartCoroutine(TechAttackText(sub, tech.subtype, enemy.enemyName));

                yield return new WaitForSeconds(time);

                break;
            }

            case "Harm":
            {
                switch(tech.subtype)
                {
                    case "Health":
                    {   
                        enemy.HarmHP(sub);

                        StartCoroutine(HealthText(-sub, enemy.enemyName));

                        yield return new WaitForSeconds(time);

                        break;
                    }

                    case "Energy":
                    {
                        enemy.HarmEP(sub);

                        StartCoroutine(EnergyText(-sub, enemy.enemyName));

                        yield return new WaitForSeconds(time);

                        break;
                    }

                    case "Boost":
                    {
                        enemy.HarmBP(sub);

                        StartCoroutine(BoostText(-sub, enemy.enemyName));

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

        //EndTurn();
    }

    public IEnumerator DefensiveSubtech(TechData tech, CharacterData ally, int sub)
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

                        StartCoroutine(AugmentText(sub, tech.subtype, ally.characterName));

                        yield return new WaitForSeconds(time);

                        break;
                    }

                    case "Defence":
                    {
                        ally.AugmentDefence(sub);

                        StartCoroutine(AugmentText(sub, tech.subtype, ally.characterName));

                        yield return new WaitForSeconds(time);

                        break;
                    }

                    case "Accuracy":
                    {
                        ally.AugmentAccuracy(sub);

                        StartCoroutine(AugmentText(sub, tech.subtype, ally.characterName));

                        yield return new WaitForSeconds(time);

                        break;
                    }

                    case "Evasion":
                    {
                        ally.AugmentEvasion(sub);

                        StartCoroutine(AugmentText(sub, tech.subtype, ally.characterName));

                        yield return new WaitForSeconds(time);

                        break;
                    }

                    case "Luck":
                    {
                        ally.AugmentLuck(sub);
                        
                        StartCoroutine(AugmentText(sub, tech.subtype, ally.characterName));

                        yield return new WaitForSeconds(time);

                        break;
                    }

                    case "Speed":
                    {
                        ally.AugmentSpeed(sub);

                        StartCoroutine(AugmentText(sub, tech.subtype, ally.characterName));

                        yield return new WaitForSeconds(time);

                        break;
                    }

                    case "Energy Offence":
                    {
                        ally.AugmentEnergyOffence(sub);

                        StartCoroutine(AugmentText(sub, tech.subtype, ally.characterName));

                        yield return new WaitForSeconds(time);

                        break;
                    }

                    case "Energy Defence":
                    {
                        ally.AugmentEnergyDefence(sub);

                        StartCoroutine(AugmentText(sub, tech.subtype, ally.characterName));

                        yield return new WaitForSeconds(time);

                        break;
                    }

                    case "Support":
                    {
                        ally.AugmentSupport(sub);

                        StartCoroutine(AugmentText(sub, tech.subtype, ally.characterName));

                        yield return new WaitForSeconds(time);

                        break;
                    }

                    case "Stamina":
                    {
                        ally.AugmentStamina(sub);

                        StartCoroutine(AugmentText(sub, tech.subtype, ally.characterName));

                        yield return new WaitForSeconds(time);

                        break;
                    }

                    case "Vigour":
                    {
                        ally.AugmentVigour(sub);

                        StartCoroutine(AugmentText(sub, tech.subtype, ally.characterName));

                        yield return new WaitForSeconds(time);

                        break;
                    }

                    case "Vitality":
                    {
                        ally.AugmentVitality(sub);

                        StartCoroutine(AugmentText(sub, tech.subtype, ally.characterName));

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

                        StartCoroutine(HealthText(sub, ally.characterName));

                        yield return new WaitForSeconds(time);

                        break;
                    }

                    case "Energy":
                    {
                        ally.HealEP(sub);

                        StartCoroutine(HealthText(sub, ally.characterName));

                        yield return new WaitForSeconds(time);

                        break;
                    }

                    case "Boost":
                    {
                        ally.HealBP(sub);

                        StartCoroutine(HealthText(sub, ally.characterName));

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

        //EndTurn();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetAxis("Mouse ScrollWheel") > 0.1)
        {
            if(currentChar)
            {
                currentChar.Boost(5);
            }
        }
        else if(Input.GetAxis("Mouse ScrollWheel") < -0.1)
        {
            if(currentChar)
            {
                currentChar.Boost(-5);
            }
        }

        if(currentChar)
        {
            boostMenu.SetActive(true);

            if(currentChar.BP < currentChar.level * 5)
            {
                boostText.text = currentChar.boost.ToString() + " / " + currentChar.maxBP;
            }
            else
            {
                int boostNo = currentChar.level * 5;
                boostText.text = currentChar.boost.ToString() + " / " + boostNo.ToString();
            }
        }
        else
        {
            boostMenu.SetActive(false);
        }

        // change the boost slider
    }
}
