using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyUI : MonoBehaviour
{
    public TMP_Text enemyName;
    public int enemyNo;
    public EncounterManager encounter;
    public EnemyData enemy;

    // Start is called before the first frame update
    void Start()
    {
        encounter = GameDataManager.instance.encounter;
        //enemy = encounter.tempEnemies[enemyNo];
        Debug.Log("Started");
    }

    // Update is called once per frame
    void Update()
    {
        if(!enemy)
        {
            enemy = encounter.tempEnemies[enemyNo];
        }
        else
        {
            if(enemy.HP > ((enemy.maxHP / 4) * 3))
            {
                enemyName.color = Color.blue;
            }
            else if(enemy.HP > (enemy.maxHP / 2))
            {
                enemyName.color = Color.green;
            }
            else if(enemy.HP > (enemy.maxHP / 4))
            {
                enemyName.color = Color.yellow;
            }
            else if(enemy.HP > 0)
            {
                enemyName.color = Color.red;
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
    }
}
