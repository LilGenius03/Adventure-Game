using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueAudioInfo", menuName = "ScriptableObjects/DialogueAudioInfoSO", order = 1)]
public class Dialogue_Audio_Info_SO : ScriptableObject
{
    public string ID;

     public AudioClip[] dialogueTypingSoundClips;
    [Range(0, 5)] public int frequencyLevel = 2;
    [Range(-2, 2)] public float minPitch = 0.5f;
    [Range(-2, 2)] public float maxPitch = 3f;
     public bool stopAudioSource;
}
