using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ProgressManager
{
    public int currentScene = 2, battleScene = 1;

    public List<bool> chest;
    public List<bool> quest;
    public List<bool> choice;

    public Vector3 characterPos;

    public ProgressManager()
    {

    }

    public void SavePos()
    {
        characterPos = GameDataManager.instance.party.members[0].transform.position;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
