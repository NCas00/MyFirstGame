using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameOverManager : MonoBehaviour
{
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private GameObject deathAudio;
    [SerializeField] private float gameShutdownDelay = 3f; 

    public void GameOver()
    {
        Debug.Log("Hai perso");
        gameOverUI.SetActive(true);
        deathAudio.SetActive(true);
       //QuitGame();
        StartCoroutine(QuitGame());
        //Invoke("QuitGame()", gameShutdownDelay);
        //Invoke(nameof(QuitGame), gameShutdownDelay);
    }
    
    private IEnumerator QuitGame()
    {
        yield return new WaitForSeconds(gameShutdownDelay);
        Debug.Log("Scarso");
        UnityEditor.EditorApplication.isPlaying = false;
        //Application.Quit();
    }
    /*
    private void QuitGame()
    {
        Debug.Log("Scarso");
        Application.Quit();
        //UnityEditor.EditorApplication.isPlaying = false;
    }*/
}

