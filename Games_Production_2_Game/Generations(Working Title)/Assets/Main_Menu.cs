using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class Main_Menu : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject optionsMenu;
    [SerializeField] private GameObject MainMenu;
    [SerializeField] private AudioSource pauseMusic;
    [SerializeField] private AudioMixer audioMixer;


    public void Play()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Quit()
    {
        Application.Quit();
        pauseMusic.Stop();
    }

    public void Options()
    {
        optionsMenu.SetActive(true);
        MainMenu.SetActive(false);
    }

    public void SetVolume(float volume)
    {
        Debug.Log(volume);
        audioMixer.SetFloat("Volume", volume);
    }

    public void Return()
    {
        optionsMenu.SetActive(false);
        MainMenu.SetActive(true);
        pauseMusic.Stop();
    }
}
