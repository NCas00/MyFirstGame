using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LossAudioController : MonoBehaviour
{
    public GameObject LossAudio;
    public GameOverManager gameOverManager;

    private void Start()
    {
        gameOverManager.OnGameEnd += HandleGameEnd;
    }

    private void HandleGameEnd(AudioClip clipaudio)
    {
        if (gameOverManager.loss == true)
        {
            Debug.Log("sei un bufu");
            //LossAudio.Play();
            LossAudio.SetActive(true);
        }
    }
}
