using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PartyManager
{
    //public CharacterData talos;
    public List<CharacterData> members;
    public List<CharacterData> tempMembers;
    public List<GameObject> characters;
    public List<GameObject> tempCharacters;

    public PartyManager()
    {
        
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    /*void Update()
    {
        if(members.Count > characters.Count)
        {
            for(int i = 0; i < members.Count; i++)
            {
                bool match = false;

                if(characters.Count > 0)
                {
                    for(int j = 0; j < characters.Count; j++)
                    {
                        if(members[i].gameObject == characters[j])
                        {
                            match = true;
                        }
                    }
                }

                if (!match)
                {
                    characters.Add(members[i].gameObject);
                }
            }
        }

        if(members.Count < characters.Count)
        {
            for(int i = 0; i < characters.Count; i++)
            {
                bool match = false;

                if(members.Count > 0)
                {
                    for(int j = 0; j < members.Count; j++)
                    {
                        if(members[j].gameObject == characters[i])
                        {
                            match = true;
                        }
                    }
                }

                if (!match)
                {
                    characters.Remove(members[i].gameObject);
                }
            }
        }
    }*/
}
