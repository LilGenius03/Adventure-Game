using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;
using UnityEngine.EventSystems;
using Ink.UnityIntegration;

public class Dialogue_Manager : MonoBehaviour
{
    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private TextMeshProUGUI displayNameText;
    [SerializeField] private Animator portraitAnimator;
    [SerializeField] private GameObject continueIcon;

    [Header("Settings")]
    [SerializeField] private float typingSpeed = 0.04f;

    [Header("Globals Ink File")]
    [SerializeField] private InkFile globalsInkFile;

    [Header("Boolean Options")]
    private bool canFollow;

    [Header("Audio")]
    private AudioSource audioSource;
    [SerializeField] private Dialogue_Audio_Info_SO defaultAudioInfo;
    [SerializeField] private Dialogue_Audio_Info_SO[] audioInfos;
    private Dictionary<string, Dialogue_Audio_Info_SO> audioInfoDictionary;
    private Dialogue_Audio_Info_SO currentAudioInfo;
    [SerializeField] private bool makePredictable;

    private Story currentStory;
    public bool dialogueIsPlaying {  get; private set; }

    private bool canContinueToNextLine = false;

    [Header("PlayerRef")]
    private PlayerMovement_Script playerMovement_Script;
    [SerializeField] GameObject Player;
    //private Follow_AI follow;

    [Header("Choices")]
    [SerializeField] private GameObject[] choices;
    private TextMeshProUGUI[] choicesText;
    private Coroutine displayLineCoroutine;

    private static Dialogue_Manager instance;
    private const string SPEAKER_TAG = "speaker";
    private const string PORTRAIT_TAG = "portrait";
    private const string AUDIO_TAG = "audio";
    private const string PARTY_TAG = "PartyMember";
    private Dialogue_Variables dialogueVariables;
    private void Awake()
    {
       if (instance != null)
        {
            Debug.LogWarning("Found more than one Dialogue Manager");
        }
       instance = this;

       playerMovement_Script = Player.GetComponent<PlayerMovement_Script>();
       audioSource = this.gameObject.AddComponent<AudioSource>();
       currentAudioInfo = defaultAudioInfo;
       //follow.GetComponent<Follow_AI>();

        dialogueVariables = new Dialogue_Variables(globalsInkFile.filePath);
    }

    public static Dialogue_Manager GetInstance()
    {
        return instance;
    }

    private void Start()
    {
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        // get the layout animator


        // get all of the choices text
        choicesText = new TextMeshProUGUI[choices.Length];
        int index = 0;
        foreach(GameObject choice in choices)
        {
            choicesText[index] = choice.GetComponentInChildren<TextMeshProUGUI>();
            index++;
        }

        InitializeAudioInfoDictionary();
    }

    private void InitializeAudioInfoDictionary()
    {
        audioInfoDictionary = new Dictionary<string, Dialogue_Audio_Info_SO>();
        audioInfoDictionary.Add(defaultAudioInfo.ID, defaultAudioInfo);
        foreach(Dialogue_Audio_Info_SO audioInfo in audioInfos)
        {
            audioInfoDictionary.Add(audioInfo.ID, audioInfo);
        }
    }

    private void SetCurrentAudioInfo(string id)
    {
        Dialogue_Audio_Info_SO audioInfo = null;
        audioInfoDictionary.TryGetValue(id, out audioInfo);
        if (audioInfo != null)
        {
          this.currentAudioInfo = audioInfo;  
        }
        else
        {
            Debug.LogWarning("Failed to find audio info for id: " + id);
        }
    }

    private void Update()
    {
        //return right away if dialogue isn't playing
        if (!dialogueIsPlaying)
        {
            return;
        }

        if(canContinueToNextLine == true && currentStory.currentChoices.Count == 0 && playerMovement_Script.SubmitKeyPressed)
        {
            ContinueStory();
        }
    }

    private IEnumerator DisplayLine(string line)
    {
        //set the text to the full line, but set the visible characters to 0
        dialogueText.text = line;
        dialogueText.maxVisibleCharacters = 0;
        // hide items while text is typing
        continueIcon.SetActive(false);
        HideChoices();

        canContinueToNextLine = false;

        foreach(char letter in line.ToCharArray())
        {
            PlayDialogueSound(dialogueText.maxVisibleCharacters, dialogueText.text[dialogueText.maxVisibleCharacters]);
            dialogueText.maxVisibleCharacters++;
            yield return new WaitForSeconds(typingSpeed);
        }

        //actions to take after the entire line has finished displaying
        continueIcon.SetActive(true);
        canContinueToNextLine = true;
    }

    private void PlayDialogueSound(int currentDisplayedCharacterCount, char currentCharacter)
    {
        AudioClip[] dialogueTypingSoundClips = currentAudioInfo.dialogueTypingSoundClips;
        int frequencyLevel = currentAudioInfo.frequencyLevel;
        float minPitch = currentAudioInfo.minPitch;
        float maxPitch = currentAudioInfo.maxPitch;
        bool stopAudioSource = currentAudioInfo.stopAudioSource;


        if(currentDisplayedCharacterCount % frequencyLevel == 0)
        {
            if (stopAudioSource)
            {
                audioSource.Stop();
            }
             AudioClip soundClip = null;
            if(makePredictable)
            {
                int hashCode = currentCharacter.GetHashCode();
                int predictableIndex = hashCode % dialogueTypingSoundClips.Length;
                soundClip = dialogueTypingSoundClips[predictableIndex];

                int minPitchInt = (int)(minPitch * 100);
                int maxPitchInt = (int)(maxPitch * 100);
                int pitchRangeInt = maxPitchInt - minPitchInt;
                //cannot divide by 0, so if there is no range then skip the selection
                if(pitchRangeInt != 0) 
                {
                    int predictablePitchInt = (hashCode % pitchRangeInt) + minPitchInt;
                    float predictablePitch = predictablePitchInt / 100f;
                    audioSource.pitch = predictablePitch;
                }
            }

            else
            {
                int randomIndex = Random.Range(0, dialogueTypingSoundClips.Length);
                soundClip = dialogueTypingSoundClips[randomIndex];

                audioSource.pitch = Random.Range(minPitch, maxPitch);
            }

 
            audioSource.PlayOneShot(soundClip);
        }
    }

    private void HideChoices()
    {
        foreach(GameObject choiceButton in choices)
        {
            choiceButton.SetActive(false);
        }
    }

    public void EnterDialogueMode(TextAsset inkJSON)
    {
        currentStory = new Story(inkJSON.text);
        dialogueIsPlaying = true;
        dialoguePanel.SetActive(true);

        dialogueVariables.StartListening(currentStory);
        // reset portrait, layout and speaker
        displayNameText.text = "???";
        portraitAnimator.Play("default");

        ContinueStory();
    }

    private IEnumerator ExitDialogueMode()
    {
        yield return new WaitForSeconds(0.2f);

        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        dialogueText.text = "";
        dialogueVariables.StopListening(currentStory);

        //go back to default audio
        SetCurrentAudioInfo(defaultAudioInfo.ID);
    }

    private void ContinueStory()
    {
        if (currentStory.canContinue)
        {
            //set text for the current dialogue line
            if(displayLineCoroutine != null )
            {
                StopCoroutine(displayLineCoroutine);
            }
            string nextLine = currentStory.Continue();

            //handles tags
            HandleTags(currentStory.currentTags);
            displayLineCoroutine = StartCoroutine(DisplayLine(nextLine));
            // display choices, if any, for this dialogue line
            DisplayChoices();          
        }
        else
        {
            StartCoroutine(ExitDialogueMode());
        }
    }

    private void HandleTags(List<string> currentTags)
    {
        //Loop through each tag and handle it accordingly
        foreach (string tag in currentTags)
        {
            // parse the tag
            string[] splitTag = tag.Split(':');

            if (splitTag.Length != 2)
            {
                Debug.LogError("Tag could not be appropiately parsed: " + tag);
            }

            string tagKey = splitTag[0].Trim();
            string tagValue = splitTag[1].Trim();

            //handle the tag

            switch (tagKey)
            {
                case SPEAKER_TAG:
                    displayNameText.text = tagValue;
                    break;
                case PORTRAIT_TAG:
                    portraitAnimator.Play(tagValue);
                    break;
                case AUDIO_TAG:
                    SetCurrentAudioInfo(tagValue);
                    break;
                default:
                //case PARTY_TAG:
                    //follow.FollowPlayer();
                    //break;
                    Debug.LogWarning("Tag came in but is not currently being handled: " + tag);
                    break;
            }
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
        if(canContinueToNextLine)
        {
            //Execute choice made
            currentStory.ChooseChoiceIndex(choiceIndex);

            ContinueStory();
        }
    }

    public Ink.Runtime.Object GetVariableState(string variableName)
    {
        Ink.Runtime.Object variableValue = null;
        dialogueVariables.variables.TryGetValue(variableName, out variableValue);

        if(variableValue = null)
        {
            Debug.LogWarning("Ink Variable was found to be null: " + variableName);

        }

        return variableValue;
    }

       
}
