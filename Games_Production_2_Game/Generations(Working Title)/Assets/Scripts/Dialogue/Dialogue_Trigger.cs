using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogue_Trigger : MonoBehaviour
{
    [Header("Visual Cue")]
    [SerializeField] private GameObject VisualCue;

    [Header("Ink JSON")]
    [SerializeField] private TextAsset inkJSON;

    [Header("PlayerMovement")]
    private PlayerMovement_Script playerMovement_Script;
    [SerializeField] GameObject Player;

    private bool playerInRange;

    private void Awake()
    {
        VisualCue.SetActive(false);
        playerInRange = false;
        playerMovement_Script = Player.GetComponent<PlayerMovement_Script>();
    }

    private void Update()
    {
        if(playerInRange && !Dialogue_Manager.GetInstance().dialogueIsPlaying)
        {
            VisualCue.SetActive(true);
            
            if(playerMovement_Script.keyPressed)
            {
                Dialogue_Manager.GetInstance().EnterDialogueMode(inkJSON);
            }
            
        }
        else
        {
            VisualCue.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerInRange = false;
        }
    }
}
