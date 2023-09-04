using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinAudioController : MonoBehaviour
{
    public GameObject WinAudio;
    public GameOverManager gameOverManager;

    private void Start()
    {
        gameOverManager.OnGameEnd += HandleGameEnd;
    }

    private void HandleGameEnd(AudioClip clipaudio)
    {
        if (gameOverManager.win == true)
        {
            Debug.Log("gg");
            //WinAudio.Play();
            WinAudio.SetActive(true);
        }
    }
}
