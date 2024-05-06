using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public PartyManager party;
    public InventoryManager inventory;
    public ProgressManager progress;
    public EncounterManager encounter;

    public GameData()
    {
        party = new PartyManager();
        inventory = new InventoryManager();
        progress = new ProgressManager();
        encounter = new EncounterManager();
    }

    public void SaveData()
    {
        progress = GameDataManager.instance.progress;
        party = GameDataManager.instance.party;
        inventory = GameDataManager.instance.inventory;
        encounter = GameDataManager.instance.encounter;
    }

    public void LoadData()
    {
        GameDataManager.instance.progress = progress;
        GameDataManager.instance.party = party;
        GameDataManager.instance.inventory = inventory;
        GameDataManager.instance.encounter = encounter;
    }
}
