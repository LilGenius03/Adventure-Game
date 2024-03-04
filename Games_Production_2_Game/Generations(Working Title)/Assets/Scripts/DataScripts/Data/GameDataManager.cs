using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class GameDataManager : MonoBehaviour
{
    [SerializeField] private string fileName;

    private DataFileHandler dataFileHandler;

    public PartyManager party;
    public InventoryManager inventory;
    public ProgressManager progress;
    
    public GameData gameData;
    public static GameDataManager instance;
    
    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        dataFileHandler = new DataFileHandler(Application.persistentDataPath, fileName);
    }

    public void NewGame(int slot)
    {
        gameData = new GameData();

        party = gameData.party;
        inventory = gameData.inventory;
        progress = gameData.progress;

        SceneManager.LoadScene(progress.currentScene);

        Debug.Log("New Game");

        SaveGame(slot);
    }

    public void SaveGame(int slot)
    {
        gameData.SaveData();
        dataFileHandler.Save(gameData, slot);
    }

    public void LoadGame(int slot)
    {
        gameData = dataFileHandler.Load(slot);

        if(this.gameData == null)
        {
            NewGame(slot);
        }
        else
        {
            gameData.LoadData();

            Debug.Log("Load Game File " + slot);
            Debug.Log("Loading Scene " + progress.currentScene);
        }

        SceneManager.LoadScene(progress.currentScene);
    }

    public void ExitGame()
    {
        gameData = null;
    }

    private void OnApplicationQuit()
    {

    }
}
