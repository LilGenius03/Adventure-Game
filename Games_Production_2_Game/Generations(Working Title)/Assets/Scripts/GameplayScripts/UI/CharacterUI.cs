using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterUI : MonoBehaviour
{
    public Slider HPSlider, EPSlider, BPSlider;
    public TMP_Text characterName, HPText, EPText, BPText;
    public int characterNo;
    public CharacterData character;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {   
        if(!character)
        {
            character = GameDataManager.instance.party.tempMembers[characterNo];
            characterName.text = GameDataManager.instance.party.members[characterNo].characterName;
        }
        else
        {
            HPSlider.maxValue = character.maxHP;
            HPSlider.value = character.HP;
            EPSlider.maxValue = character.maxEP;
            EPSlider.value = character.EP;
            BPSlider.maxValue = character.maxBP;
            BPSlider.value = character.BP;

            HPText.text = character.HP.ToString() + " / " + character.maxHP.ToString();
            EPText.text = character.EP.ToString() + " / " + character.maxEP.ToString();
            BPText.text = character.BP.ToString() + " / " + character.maxBP.ToString();
        }
    }
}
