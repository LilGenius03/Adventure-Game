using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public EncounterManager encounter;
    public PartyManager party;
    public InventoryManager inventory;
    public List<GameObject> initiative;

    BattleManager(EncounterManager battle)
    {
        encounter = battle;
        party = GameDataManager.instance.party;
        inventory = GameDataManager.instance.inventory;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /*if(encounter.enemies.count =< 0)
        {
            EndEncounter();
        }*/
    }

    /*void EndEncounter()
    {
        foreach (CharacterScript character in party.members)
        {
            if (character.alive)
            {
                character.exp += totalExp;
            }
        }
    }*/
}
