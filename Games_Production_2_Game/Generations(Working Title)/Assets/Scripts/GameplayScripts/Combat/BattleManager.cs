using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public PartyManager party;
    public EncounterManager encounter;
    public InventoryManager inventory;
    public List<CombattantScript> combattants;
    public List<CombattantScript> initiative;

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

        foreach(CharacterData character in party.members)
        {
            character.RollInitiative();
            combattants.Add(character.combattantScript);
        }
        foreach(EnemyData enemy in encounter.enemies)
        {
            enemy.RollInitiative();
            combattants.Add(enemy.combattantScript);
        }

        RollInitiative();
        
    }

    // Update is called once per frame
    void Update()
    {
        if(nextTurn)
        {
            nextTurn = false;

            if(currentTurn < initiative.Count)
            {
                currentTurn++;

                if(initiative[currentTurn].gameObject.GetComponent<CharacterData>())
                {
                    CharacterTurn(initiative[currentTurn].gameObject.GetComponent<CharacterData>());
                }
                else if(initiative[currentTurn].gameObject.GetComponent<EnemyData>())
                {
                    EnemyTurn(initiative[currentTurn].gameObject.GetComponent<EnemyData>());
                }
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

        combattants = initiative;
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
                if(character == party.members[charNo])
                {
                    battleMenu.CharacterTurn(charNo);
                    matched = true;
                }
                else if(charNo >= party.members.Count)
                {
                    charNo = 0;
                    currentTurn++;
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
            currentTurn++;
            nextTurn = true;
        }
    }

    public void EnemyTurn(EnemyData enemy)
    {
        if(enemy.HP > 0)
        {

        }

        currentTurn++;
        nextTurn = true;
    }

    void WinEncounter()
    {
        foreach (EnemyData enemy in encounter.enemies)
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
        
        foreach (CharacterData character in party.members)
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
        }
    }
}
