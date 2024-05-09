using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{
    public PartyManager party;
    public EncounterManager encounter;
    public InventoryManager inventory;
    public List<CombattantScript> combattants;
    public List<CombattantScript> initiative;
    public EnemyHandler enemyHandler;
    public Transform[] characterPos;
    public Transform[] enemyPos;
    public GameObject textBox, infoBox;
    public TMP_Text text, info;

    //public static BattleManager battle;
    public BattleMenu battleMenu;

    public int currentTurn = 0;
    public bool nextTurn;

    /*public BattleManager()
    {
        party = GameDataManager.instance.party;
        inventory = GameDataManager.instance.inventory;
        encounter = GameDataManager.instance.encounter;
    }*/
    
    // Start is called before the first frame update
    void Start()
    {
        party = GameDataManager.instance.party;
        inventory = GameDataManager.instance.inventory;
        encounter = GameDataManager.instance.encounter;

        for(int i = 0; i < party.members.Count; i++)
        {
            party.tempCharacters.Add(Instantiate(party.characters[i], characterPos[i]));
            party.tempMembers.Add(party.tempCharacters[i].GetComponent<CharacterData>());
            party.tempMembers[i].StartBattle();
            //party.members[i].BattlePos(characterPos[i]);
        }
        
        for(int i = 0; i < encounter.enemies.Count; i++)
        {
            encounter.tempMonsters.Add(Instantiate(encounter.monsters[i], enemyPos[i]));
            encounter.tempEnemies.Add(encounter.tempMonsters[i].GetComponent<EnemyData>());
            encounter.tempMonsters[i].GetComponent<EnemyData>().StartBattle();
            //encounter.enemies[i].BattlePos(enemyPos[i]);
        }

        RollInitiative();
        
    }

    // Update is called once per frame
    void Update()
    {
        if(nextTurn)
        {
            bool lost = true;

            foreach(CharacterData character in party.tempMembers)
            {
                if(character.HP > 0)
                {
                    lost = false;
                    //Debug.Log(won);
                }
            }

            if(lost)
            {
                LoseEncounter();
            }
            
            bool won = true;

            foreach(EnemyData enemy in encounter.tempEnemies)
            {
                if(enemy.HP > 0)
                {
                    won = false;
                    //Debug.Log(won);
                }
            }

            if(won)
            {
                WinEncounter();
            }


            nextTurn = false;

            if(currentTurn < initiative.Count)
            {
                if(initiative[currentTurn].gameObject.GetComponent<CharacterData>())
                {
                    CharacterTurn(initiative[currentTurn].gameObject.GetComponent<CharacterData>());
                }
                else if(initiative[currentTurn].gameObject.GetComponent<EnemyData>())
                {
                    EnemyTurn(initiative[currentTurn].gameObject.GetComponent<EnemyData>());
                }

                currentTurn++;
            }
            else
            {
                StartRound();
            }
        }
    }

    public void RollInitiative()
    {
        initiative.Clear();
        combattants.Clear();
        
        foreach(CharacterData character in party.tempMembers)
        {
            character.RollInitiative();
            combattants.Add(character.combattantScript);
            Debug.Log(combattants.Count);
        }

        foreach(EnemyData enemy in encounter.tempEnemies)
        {
            enemy.RollInitiative();
            combattants.Add(enemy.combattantScript);
        }

        foreach(CombattantScript combattant in combattants)
        {
            combattant.RollInitiative();
        }

        do
        {
            CombattantScript currentFastest = combattants[0];

            for(int i = 0; i < combattants.Count; i++)
            {
                if(combattants[i].initiative > currentFastest.initiative)
                {
                    currentFastest = combattants[i];
                }
            }

            initiative.Add(currentFastest);
            combattants.Remove(currentFastest);
        } while(combattants.Count > 0);

        currentTurn = 0;
        nextTurn = true;
    }

    public void StartRound()
    {
        RollInitiative();
        currentTurn = 0;
        nextTurn = true;
    }

    public void CharacterTurn(CharacterData character)
    {
        if(character.HP > 0)
        {
            bool matched = false;
            int charNo = 0;
            do
            {
                if(character == party.tempMembers[charNo])
                {
                    
                    battleMenu.CharacterTurn(charNo);
                    info.text = party.tempMembers[charNo].characterName + "'s Turn";
                    matched = true;
                }
                else if(charNo >= party.tempMembers.Count)
                {
                    charNo = 0;
                    nextTurn = true;
                    matched = true;
                    return;
                }
                else
                {
                    charNo++;
                }

            } while(!matched);
        }
        else
        {
            nextTurn = true;
        }
    }

    public void EnemyTurn(EnemyData enemy)
    {
        if(enemy.HP > 0)
        {
            Debug.Log(enemy.enemyName);

            bool matched = false;
            int charNo = 0;
            do
            {
                if(enemy == encounter.tempEnemies[charNo])
                {
                    enemyHandler.EnemyTurn(charNo);
                    info.text = encounter.tempEnemies[charNo].enemyName + "'s Turn";
                    matched = true;
                }
                else if(charNo >= encounter.tempEnemies.Count)
                {
                    charNo = 0;
                    nextTurn = true;
                    return;
                }
                else
                {
                    charNo++;
                }

            } while(!matched);
        }
        else
        {
            nextTurn = true;
        }

        //nextTurn = true;
    }

    void LoseEncounter()
    {
        SceneManager.LoadScene(2);
    }

    void WinEncounter()
    {
        SceneManager.LoadScene(3);

        foreach(EnemyData enemy in encounter.enemies)
        {
            int drop = 0;
            foreach(ItemData item in enemy.drops)
            {
                float chance = Random.Range(0, 100);
                if (chance <= enemy.rarity[drop])
                {
                    inventory.items.Add(item);
                }
                drop++;
            }

            int droppedCoins = Random.Range(0, enemy.coins);
            inventory.coins += droppedCoins;
        }
        
        foreach (CharacterData character in party.tempMembers)
        {
            
            if (character.HP > 0)
            {
                int exp = 0;

                foreach(EnemyData enemy in encounter.enemies)
                {
                    exp += (enemy.level*enemy.level)*(10*(enemy.level / character.level));
                }
                character.LevelUp(exp);
            }

            //SceneManager.LoadScene(GameDataManager.instance.progress.currentScene);
        }
    }
}
