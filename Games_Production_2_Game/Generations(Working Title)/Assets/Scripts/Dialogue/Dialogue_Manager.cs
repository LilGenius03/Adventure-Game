using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;
using UnityEngine.EventSystems;

public class Dialogue_Manager : MonoBehaviour
{
    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;

    private Story currentStory;
    public bool dialogueIsPlaying {  get; private set; }

    [Header("PlayerRef")]
    private PlayerMovement_Script playerMovement_Script;
    [SerializeField] GameObject Player;

    [Header("Choices")]
    [SerializeField] private GameObject[] choices;
    private TextMeshProUGUI[] choicesText;

    private static Dialogue_Manager instance;

    private void Awake()
    {
       if (instance != null)
        {
            Debug.LogWarning("Found more than one Dialogue Manager");
        }
       instance = this;

       playerMovement_Script = Player.GetComponent<PlayerMovement_Script>();
    }

    public static Dialogue_Manager GetInstance()
    {
        return instance;
    }

    private void Start()
    {
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);

        // get all of the choices text
        choicesText = new TextMeshProUGUI[choices.Length];
        int index = 0;
        foreach(GameObject choice in choices)
        {
            choicesText[index] = choice.GetComponentInChildren<TextMeshProUGUI>();
            index++;
        }
    }

    private void Update()
    {
        //return right away if dialogue isn't playing
        if (!dialogueIsPlaying)
        {
            return;
        }

        if(currentStory.currentChoices.Count == 0 && playerMovement_Script.SubmitKeyPressed)
        {
            ContinueStory();
        }
    }

    public void EnterDialogueMode(TextAsset inkJSON)
    {
        currentStory = new Story(inkJSON.text);
        dialogueIsPlaying = true;
        dialoguePanel.SetActive(true);

        ContinueStory();
    }

    private IEnumerator ExitDialogueMode()
    {
        yield return new WaitForSeconds(0.2f);

        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        dialogueText.text = "";
    }

    private void ContinueStory()
    {
        if (currentStory.canContinue)
        {
            //set text for the current dialogue line
            dialogueText.text = currentStory.Continue();
            // display choices, if any, for this dialogue line
            DisplayChoices();
        }
        else
        {
            StartCoroutine(ExitDialogueMode());
        }
    }

    private void DisplayChoices()
    {
        List<Choice> currentChoices = currentStory.currentChoices;

        //defensive check to make sure our UI can support the number of choices coming in
        if(currentChoices.Count > choices.Length)
        {
            Debug.LogError("More choices were given than the UI can handle. Number of choices given" + currentChoices.Count);
        }

        int index = 0;
        // enable and initialise the choices up to the amount of choices for this line of dialogue
        foreach(Choice choice in currentChoices)
        {
            choices[index].gameObject.SetActive(true);
            choicesText[index].text = choice.text;
            index++;
        }
        // go through the remaining choices the UI supports and make sure they're hidden
        for (int i = index; i < choices.Length; i++)
        {
            choices[i].gameObject.SetActive(false);
        }

        StartCoroutine(SelectFirstChoice());
    }

    private IEnumerator SelectFirstChoice()
    {
        // Event System requires we clear it first, then wait
        // for at least oen frame before we set the current selected object.
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(choices[0].gameObject);
    }

    public void MakeChoice(int choiceIndex)
    {
        //Execute choice made
        currentStory.ChooseChoiceIndex(choiceIndex);
        ContinueStory();
    }
}