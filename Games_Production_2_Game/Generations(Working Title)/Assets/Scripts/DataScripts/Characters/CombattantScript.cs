using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombattantScript : MonoBehaviour
{
    public int speedStat;
    public int initiative;

    public void RollInitiative()
    {
        initiative = Random.Range(speedStat/2, speedStat);
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
