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
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        characterName.text = GameDataManager.instance.party.members[characterNo].characterName;
        HPText.text = GameDataManager.instance.party.tempCharacters[characterNo].GetComponent<CharacterData>().HP.ToString() + " / " + GameDataManager.instance.party.tempCharacters[characterNo].GetComponent<CharacterData>().maxHP.ToString();
        EPText.text = GameDataManager.instance.party.tempCharacters[characterNo].GetComponent<CharacterData>().EP.ToString() + " / " + GameDataManager.instance.party.tempCharacters[characterNo].GetComponent<CharacterData>().maxEP.ToString();
        BPText.text = GameDataManager.instance.party.tempCharacters[characterNo].GetComponent<CharacterData>().BP.ToString() + " / " + GameDataManager.instance.party.tempCharacters[characterNo].GetComponent<CharacterData>().maxBP.ToString();
    }
}
