using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleMenu : MonoBehaviour
{
    public List<Button> targetButton;
    public List<TMP_Text> targetText;
    public List<Button> techButton;
    public List<TMP_Text> techText;
    public List<Button> inventoryButton;
    public List<TMP_Text> inventoryText;
    public List<GameObject> characterUI;
    public GameObject battleUI, targetMenu, techMenu, inventoryMenu, tacticMenu, boostMenu;
    public Slider boostSlider;
    public bool newTurn;
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
        currentChar = party.members[charNo];

        for(int i = 0; i < currentChar.techs.Count; i++)
        {
            techText[i].text = currentChar.techs[i].title;
            techButton[i].gameObject.SetActive(true);
            techButton[i].onClick.RemoveAllListeners();
            techButton[i].onClick.AddListener(() => SelectTech(currentChar.techs[i]));
        }

        battleUI.gameObject.SetActive(true);
    }

    public void Attack()
    {
        for(int i = 0; i < encounter.enemies.Count; i++)
        {
            if(targetButton[i].gameObject.tag == "Enemy" && encounter.enemies[i].HP > 0)
            {
                targetText[i].text = encounter.enemies[i].enemyName;
                targetButton[i].gameObject.SetActive(true);
                targetButton[i].onClick.RemoveAllListeners();
                targetButton[i].onClick.AddListener(() => encounter.enemies[i].PhysicalAttack(currentChar.Offence(), currentChar.Accuracy(), currentChar.Luck(), currentChar.weaponSlot.type));
                targetButton[i].onClick.AddListener(() => targetMenu.SetActive(false));
            }
            else
            {
                targetButton[i].gameObject.SetActive(false);
            }
        }
        targetMenu.SetActive(true);
    }

    public void SelectTech(TechData tech)
    {
        selectedTech = tech;
        switch(tech.target)
        {
            case "Self":
            {
                Tech(tech, currentChar.combattantScript);
                break;
            }
            
            case "Enemy":
            {
                for(int i = 0; i < encounter.enemies.Count; i++)
                {
                    if(targetButton[i].gameObject.tag == "Enemy" && encounter.enemies[i].HP > 0)
                    {
                        targetText[i].text = encounter.enemies[i].enemyName;
                        targetButton[i].gameObject.SetActive(true);
                        targetButton[i].onClick.RemoveAllListeners();
                        targetButton[i].onClick.AddListener(() => Tech(tech, encounter.enemies[i].combattantScript));
                        targetButton[i].onClick.AddListener(() => targetMenu.SetActive(false));
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
                Tech(tech, currentChar.combattantScript);
                break;
            }

            case "Ally":
            {
                for(int i = 0; i < party.members.Count; i++)
                {
                    if(targetButton[i].gameObject.tag == "Ally" && party.members[i].HP > 0)
                    {
                        targetText[i].text = party.members[i].characterName;
                        targetButton[i].gameObject.SetActive(true);
                        targetButton[i].onClick.RemoveAllListeners();
                        targetButton[i].onClick.AddListener(() => Tech(tech, party.members[i].combattantScript));
                        targetButton[i].onClick.AddListener(() => targetMenu.SetActive(false));
                    }
                    else
                    {
                        targetButton[i].gameObject.SetActive(false);
                    }
                }
                targetMenu.SetActive(true);

                break;
            }

            case "Allies":
            {
                Tech(tech, currentChar.combattantScript);
                break;
            }

            case "Dead Ally":
            {
                for(int i = 0; i < party.members.Count; i++)
                {
                    if(targetButton[i].gameObject.tag == "Ally" && party.members[i].HP <= 0)
                    {
                        targetText[i].text = party.members[i].characterName;
                        targetButton[i].gameObject.SetActive(true);
                        targetButton[i].onClick.RemoveAllListeners();
                        targetButton[i].onClick.AddListener(() => Tech(tech, party.members[i].combattantScript));
                        targetButton[i].onClick.AddListener(() => targetMenu.SetActive(false));
                    }
                    else
                    {
                        targetButton[i].gameObject.SetActive(false);
                    }
                }
                targetMenu.SetActive(true);
                break;
            }

            case "Dead Allies":
            {
                Tech(tech, currentChar.combattantScript);
                break;
            }
        }
    }

    public void EndTurn()
    {
        currentChar.BP -= currentChar.boost;

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

    public void Tech(TechData tech, CombattantScript combattant)
    {
        if(tech.health < currentChar.HP && tech.energy < currentChar.EP && tech.boosts < currentChar.BP)
        {
            SpendPoints(tech, currentChar);

            switch(tech.target)
            {
                case "Self":
                {
                    selectedCharacter = currentChar;

                    DefensiveTech(tech, selectedCharacter);

                    break;
                }
                
                case "Enemy":
                {
                    EnemyData targetEnemy = combattant.gameObject.GetComponent<EnemyData>();

                    selectedEnemy = targetEnemy;

                    OffensiveTech(tech, targetEnemy);

                    break;
                }

                case "Enemies":
                {
                    for(int i = 0; i < encounter.enemies.Count; i++)
                    {
                        if(encounter.enemies[i].HP > 0)
                        {
                            selectedEnemy = encounter.enemies[i];
                            
                            OffensiveTech(tech, encounter.enemies[i]);
                        }
                    }

                    break;
                }

                case "Ally":
                {
                    CharacterData targetAlly = combattant.gameObject.GetComponent<CharacterData>();

                    selectedCharacter = targetAlly;

                    DefensiveTech(tech, targetAlly);

                    break;
                }

                case "Allies":
                {
                    for(int i = 0; i < party.members.Count; i++)
                    {
                        if(party.members[i].HP > 0)
                        {
                            selectedCharacter = party.members[i];

                            DefensiveTech(tech, party.members[i]);
                        }
                    }

                    break;
                }

                case "Dead Ally":
                {
                    CharacterData targetAlly = combattant.gameObject.GetComponent<CharacterData>();

                    selectedCharacter = targetAlly;
                    
                    DefensiveTech(tech, targetAlly);

                    break;
                }

                case "Dead Allies":
                {
                    for(int i = 0; i < party.members.Count; i++)
                    {
                        if(party.members[i].HP < 0)
                        {
                            selectedCharacter = party.members[i];
                            
                            DefensiveTech(tech, party.members[i]);
                        }
                    }

                    break;
                }

                case "Summon":
                {
                    DefensiveTech(tech, currentChar);

                    break;
                }

                default:
                {
                    break;
                }
            }
        }
    }

    public void OffensiveTech(TechData tech, EnemyData enemy)
    {
        switch (tech.effect)
        {
            case "Debuff":
            {
                switch (tech.type)
                {
                    case "Offence":
                    {
                        Subtech(tech, enemy.AugmentOffence(-currentChar.Support() - tech.energy - currentChar.boost));

                        break;
                    }

                    case "Defence":
                    {
                        Subtech(tech, enemy.AugmentDefence(-currentChar.Support() - tech.energy - currentChar.boost));

                        break;
                    }

                    case "Accuracy":
                    {
                        Subtech(tech, enemy.AugmentAccuracy(-currentChar.Support() - tech.energy - currentChar.boost));

                        break;
                    }

                    case "Evasion":
                    {
                        Subtech(tech, enemy.AugmentEvasion(-currentChar.Support() - tech.energy - currentChar.boost));

                        break;
                    }

                    case "Luck":
                    {
                        Subtech(tech, enemy.AugmentLuck(-currentChar.Support() - tech.energy - currentChar.boost));

                        break;
                    }

                    case "Speed":
                    {
                        Subtech(tech, enemy.AugmentSpeed(-currentChar.Support() - tech.energy - currentChar.boost));

                        break;
                    }

                    case "Energy Offence":
                    {
                        Subtech(tech, enemy.AugmentEnergyOffence(-currentChar.Support() - tech.energy - currentChar.boost));

                        break;
                    }

                    case "Energy Defence":
                    {
                        Subtech(tech, enemy.AugmentEnergyDefence(-currentChar.Support() - tech.energy - currentChar.boost));

                        break;
                    }

                    case "Support":
                    {
                        Subtech(tech, enemy.AugmentSupport(-currentChar.Support() - tech.energy - currentChar.boost));

                        break;
                    }

                    case "Stamina":
                    {
                        Subtech(tech, enemy.AugmentStamina(-currentChar.Support() - tech.energy - currentChar.boost));

                        break;
                    }

                    case "Vigour":
                    {
                        Subtech(tech, enemy.AugmentVigour(-currentChar.Support() - tech.energy - currentChar.boost));

                        break;
                    }

                    case "Vitality":
                    {
                        Subtech(tech, enemy.AugmentVitality(-currentChar.Support() - tech.energy - currentChar.boost));

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
                Subtech(tech, enemy.PhysicalAttack((currentChar.Offence() + tech.energy + currentChar.boost), (currentChar.Accuracy() + tech.energy + currentChar.boost), (currentChar.Luck() + tech.energy + currentChar.boost), tech.type));

                break;
            }

            case "Energy Attack":
            {
                Subtech(tech, enemy.EnergyAttack((currentChar.EnergyOffence() + tech.energy + currentChar.boost), tech.type));

                break;
            }

            case "Harm":
            {
                switch(tech.type)
                {
                    case "Health":
                    {
                        Subtech(tech, enemy.HarmHP(currentChar.Support() + tech.health + currentChar.boost));

                        break;
                    }

                    case "Energy":
                    {
                        Subtech(tech, enemy.HarmEP(currentChar.Support() + tech.energy + currentChar.boost));

                        break;
                    }

                    case "Boost":
                    {
                        Subtech(tech, enemy.HarmBP(currentChar.Support() + tech.health + tech.energy + currentChar.boost));

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

    public void DefensiveTech(TechData tech, CharacterData ally)
    {
        switch (tech.effect)
        {
            case "Buff":
            {
                switch (tech.type)
                {
                    case "Offence":
                    {
                        Subtech(tech, ally.AugmentOffence(currentChar.Support() + tech.energy + currentChar.boost));

                        break;
                    }

                    case "Defence":
                    {
                        Subtech(tech, ally.AugmentDefence(currentChar.Support() + tech.energy + currentChar.boost));

                        break;
                    }

                    case "Accuracy":
                    {
                        Subtech(tech, ally.AugmentAccuracy(currentChar.Support() + tech.energy + currentChar.boost));

                        break;
                    }

                    case "Evasion":
                    {
                        Subtech(tech, ally.AugmentEvasion(currentChar.Support() + tech.energy + currentChar.boost));

                        break;
                    }

                    case "Luck":
                    {
                        Subtech(tech, ally.AugmentLuck(currentChar.Support() + tech.energy + currentChar.boost));

                        break;
                    }

                    case "Speed":
                    {
                        Subtech(tech, ally.AugmentSpeed(currentChar.Support() + tech.energy + currentChar.boost));

                        break;
                    }

                    case "Energy Offence":
                    {
                        Subtech(tech, ally.AugmentEnergyOffence(currentChar.Support() + tech.energy + currentChar.boost));

                        break;
                    }

                    case "Energy Defence":
                    {
                        Subtech(tech, ally.AugmentEnergyDefence(currentChar.Support() + tech.energy + currentChar.boost));

                        break;
                    }

                    case "Support":
                    {
                        Subtech(tech, ally.AugmentSupport(currentChar.Support() + tech.energy + currentChar.boost));

                        break;
                    }

                    case "Stamina":
                    {
                        Subtech(tech, ally.AugmentStamina(currentChar.Support() + tech.energy + currentChar.boost));

                        break;
                    }

                    case "Vigour":
                    {
                        Subtech(tech, ally.AugmentVigour(currentChar.Support() + tech.energy + currentChar.boost));

                        break;
                    }

                    case "Vitality":
                    {
                        Subtech(tech, ally.AugmentVitality(currentChar.Support() + tech.energy + currentChar.boost));

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
                        Subtech(tech, ally.HealHP(currentChar.Support() + tech.energy + currentChar.boost));

                        break;
                    }

                    case "Energy":
                    {
                        Subtech(tech, ally.HealEP(currentChar.Support() + tech.health + currentChar.boost));

                        break;
                    }

                    case "Boost":
                    {
                        Subtech(tech, ally.HealBP(currentChar.Support() + tech.health + tech.energy + currentChar.boost));

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
                    selectedCharacter = currentChar;

                    DefensiveSubtech(tech, selectedCharacter, sub);

                    break;
                }
                
                case "Enemy":
                {
                    OffensiveSubtech(tech, selectedEnemy, sub);

                    break;
                }

                case "Enemies":
                {
                    for(int i = 0; i < encounter.enemies.Count; i++)
                    {
                        if(encounter.enemies[i].HP > 0)
                        {
                            OffensiveSubtech(tech, encounter.enemies[i], sub);
                        }
                    }

                    break;
                }

                case "Ally":
                {
                    DefensiveSubtech(tech, selectedCharacter, sub);

                    break;
                }

                case "Allies":
                {
                    for(int i = 0; i < party.members.Count; i++)
                    {
                        if(party.members[i].HP > 0)
                        {
                            DefensiveSubtech(tech, party.members[i], sub);
                        }
                    }

                    break;
                }

                case "Dead Ally":
                {
                    DefensiveSubtech(tech, selectedCharacter, sub);

                    break;
                }

                case "Dead Allies":
                {
                    for(int i = 0; i < party.members.Count; i++)
                    {
                        if(party.members[i].HP < 0)
                        {
                            DefensiveSubtech(tech, party.members[i], sub);
                        }
                    }

                    break;
                }

                case "Summon":
                {
                    DefensiveSubtech(tech, currentChar);

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

    public void OffensiveSubtech(TechData tech, EnemyData enemy, int sub)
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

                        enemy.AugmentOffence(-sub);

                        break;
                    }

                    case "Defence":
                    {
                        sub = sub / 10;

                        enemy.AugmentDefence(-sub);

                        break;
                    }

                    case "Accuracy":
                    {
                        sub = sub / 10;
                        
                        enemy.AugmentAccuracy(-sub);

                        break;
                    }

                    case "Evasion":
                    {
                        sub = sub / 10;
                        
                        enemy.AugmentEvasion(-sub);

                        break;
                    }

                    case "Luck":
                    {
                        sub = sub / 10;
                        
                        enemy.AugmentLuck(-sub);

                        break;
                    }

                    case "Speed":
                    {
                        sub = sub / 10;
                        
                        enemy.AugmentSpeed(-sub);

                        break;
                    }

                    case "Energy Offence":
                    {
                        sub = sub / 10;
                        
                        enemy.AugmentEnergyOffence(-sub);

                        break;
                    }

                    case "Energy Defence":
                    {
                        sub = sub / 10;
                        
                        enemy.AugmentEnergyDefence(-sub);

                        break;
                    }

                    case "Support":
                    {
                        sub = sub / 10;
                        
                        enemy.AugmentSupport(-sub);

                        break;
                    }

                    case "Stamina":
                    {
                        sub = sub / 10;
                        
                        enemy.AugmentStamina(-sub);

                        break;
                    }

                    case "Vigour":
                    {
                        sub = sub / 10;
                        
                        enemy.AugmentVigour(-sub);

                        break;
                    }

                    case "Vitality":
                    {
                        sub = sub / 10;
                        
                        enemy.AugmentVitality(-sub);

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

                break;
            }

            case "Energy Attack":
            {
                enemy.EnergyAttack((currentChar.EnergyOffence() + tech.energy + currentChar.boost), tech.type);

                break;
            }

            case "Harm":
            {
                switch(tech.subtype)
                {
                    case "Health":
                    {
                        sub = sub / 100;
                        
                        enemy.HarmHP(sub);

                        break;
                    }

                    case "Energy":
                    {
                        sub = sub / 100;

                        enemy.HarmEP(sub);

                        break;
                    }

                    case "Boost":
                    {
                        sub = sub / 100;

                        enemy.HarmBP(sub);

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

    public void DefensiveSubtech(TechData tech, CharacterData ally, int sub)
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

        // change the boost slider
    }
}
