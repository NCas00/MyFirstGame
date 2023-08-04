using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameOptions : MonoBehaviour
{
    InGameOptionsMenu inGameOptionsMenu;

    public GameObject optionsInGamePanel; 
    public GameObject audioPanel; 
    public GameObject weatherPanel; 

    public void OpenAudioPanel()
    {
        audioPanel.SetActive(true);

        optionsInGamePanel.SetActive(false);
    }

    public void OpenWeatherPanel()
    {
        weatherPanel.SetActive(true);

        optionsInGamePanel.SetActive(false);
    }

    public void ResumeGame()
    {
        //Time.timeScale = inGameOptionsMenu.previousTimeScale;

        Time.timeScale = 1f;

        optionsInGamePanel.SetActive(false);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }


    public void ExitGame()
    {

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }
}
