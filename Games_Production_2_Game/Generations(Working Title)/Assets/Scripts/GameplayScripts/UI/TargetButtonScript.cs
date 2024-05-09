using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class TargetButtonScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public bool ally;
    public int targetNo;
    public CharacterData character;
    public EnemyData enemy;
    public GameObject targetInfo;
    public string targetName;
    public TMP_Text targetText;
    public List<string> resistance, vulnerability;
    public List<TMP_Text> resistanceText, vulnerabilityText;
    public List<Sprite> resistanceIcon, vulnerabilityIcon;
    public List<Image> resistanceImage, vulnerabilityImage;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(ally && !character)
        {
            character = GameDataManager.instance.party.tempMembers[targetNo];
            targetName = character.characterName;
            for(int i = 0; i < character.vulnerability.Count; i++)
            {
                vulnerability.Add(character.vulnerability[i]);
                vulnerabilityIcon.Add(character.vulnerabilityIcon[i]);
            }
            for(int i = 0; i < character.resistance.Count; i++)
            {
                resistance.Add(character.resistance[i]);
                resistanceIcon.Add(character.resistanceIcon[i]);
            }
        }
        else if(!ally && !enemy)
        {
            enemy = GameDataManager.instance.encounter.tempEnemies[targetNo];
            targetName = enemy.enemyName;
            for(int i = 0; i < enemy.vulnerability.Count; i++)
            {
                vulnerability.Add(enemy.vulnerability[i]);
                vulnerabilityIcon.Add(enemy.vulnerabilityIcon[i]);
            }
            for(int i = 0; i < enemy.resistance.Count; i++)
            {
                resistance.Add(enemy.resistance[i]);
                resistanceIcon.Add(enemy.resistanceIcon[i]);
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Enter");

        if(character || enemy)
        {
            targetInfo.SetActive(true);
            targetText.gameObject.SetActive(true);
            targetText.text = targetName;
            for(int i = 0; i < vulnerability.Count; i++)
            {
                vulnerabilityText[i].gameObject.SetActive(true);
                vulnerabilityText[i].text = vulnerability[i];
                vulnerabilityImage[i].gameObject.SetActive(true);
                vulnerabilityImage[i].sprite = vulnerabilityIcon[i];
            }
            for(int i = 0; i < resistance.Count; i++)
            {
                resistanceText[i].gameObject.SetActive(true);
                resistanceText[i].text = resistance[i];
                resistanceImage[i].gameObject.SetActive(true);
                resistanceImage[i].sprite = resistanceIcon[i];
            }
        }
        else
        {
            targetInfo.SetActive(false);
            targetText.gameObject.SetActive(false);
            targetText.text = "";
            for(int i = 0; i < vulnerabilityText.Count; i++)
            {
                vulnerabilityText[i].gameObject.SetActive(false);
                vulnerabilityText[i].text = "";
                vulnerabilityImage[i].gameObject.SetActive(false);
                vulnerabilityImage[i].sprite = null;
            }
            for(int i = 0; i < resistanceText.Count; i++)
            {
                resistanceText[i].gameObject.SetActive(false);
                resistanceText[i].text = "";
                resistanceImage[i].gameObject.SetActive(false);
                resistanceImage[i].sprite = null;
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("Exit");
        targetInfo.SetActive(false);
        targetText.gameObject.SetActive(false);
        targetText.text = "";
        for(int i = 0; i < vulnerabilityText.Count; i++)
        {
            vulnerabilityText[i].gameObject.SetActive(false);
            vulnerabilityText[i].text = "";
            vulnerabilityImage[i].gameObject.SetActive(false);
            vulnerabilityImage[i].sprite = null;
        }
        for(int i = 0; i < resistanceText.Count; i++)
        {
            resistanceText[i].gameObject.SetActive(false);
            resistanceText[i].text = "";
            resistanceImage[i].gameObject.SetActive(false);
            resistanceImage[i].sprite = null;
        }
    }
}
