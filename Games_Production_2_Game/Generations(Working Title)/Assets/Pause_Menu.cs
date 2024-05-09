using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class Pause_Menu : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject optionsMenu;
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private GameObject DialogueCanvas;

    private void Start()
    {
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(false);
    }
    public void Pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        DialogueCanvas.SetActive(false);
    }

   public void Play()
   {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        DialogueCanvas.SetActive(true);
    }

    public void Quit()
    {
        SceneManager.LoadScene("TitleScreen");
        Time.timeScale = 1f;
    }

    public void Options()
    {
        optionsMenu.SetActive(true);
        pauseMenu.SetActive(false);
        DialogueCanvas.SetActive(false);
        Time.timeScale = 0f;
    }

    public void SetVolume(float volume)
    {
        Debug.Log(volume);
        audioMixer.SetFloat("Volume", volume);
    }

    public void Return()
    {
        optionsMenu.SetActive(false);
        pauseMenu.SetActive(false);
        DialogueCanvas.SetActive(true);
        Time.timeScale = 1f;
    }

}
