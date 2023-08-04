using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinAudioController : MonoBehaviour
{
    public AudioSource winAudio;
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
            winAudio.Play();
        }
    }
}
