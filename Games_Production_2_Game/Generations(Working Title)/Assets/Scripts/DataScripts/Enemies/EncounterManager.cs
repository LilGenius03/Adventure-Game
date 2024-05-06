using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EncounterManager
{
    public GameObject background;
    public List<EnemyData> enemies;
    public List<EnemyData> tempEnemies;
    public List<GameObject> monsters;
    public List<GameObject> tempMonsters;
    public List<ItemData> drops;
    public int coins;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
