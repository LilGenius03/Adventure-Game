using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public PartyManager party;
    public InventoryManager inventory;
    public ProgressManager progress;

    public GameData()
    {
        party = new PartyManager();
        inventory = new InventoryManager();
        progress = new ProgressManager();
    }

    public void SaveData()
    {
        progress = GameDataManager.instance.progress;
        party = GameDataManager.instance.party;
        inventory = GameDataManager.instance.inventory;
    }

    public void LoadData()
    {
        GameDataManager.instance.progress = progress;
        GameDataManager.instance.party = party;
        GameDataManager.instance.inventory = inventory;
    }
}
