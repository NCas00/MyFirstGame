using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class GameOverManager : MonoBehaviour
{
    [SerializeField] private GameObject lossUI;
    [SerializeField] private GameObject winUI;
    //[SerializeField] private float gameShutdownDelay = 3f;
    [SerializeField] private float timeToWin = 120f;

    public bool win;
    public bool loss;

    private AudioClip audioClip;
    public Action<AudioClip> OnGameEnd;

    private void Update()
    {
        timeToWin -= Time.deltaTime;

        if (timeToWin <= 0 && !loss)
        {
            Win();
        }
    }

    public void Win()
    {
        win = true;

            OnGameEnd?.Invoke(audioClip);

        Time.timeScale = 0f;

        winUI.SetActive(true);
    }

    public void Loss()
    {
        loss = true;

        OnGameEnd?.Invoke(audioClip);

        Time.timeScale = 0f;
        
        lossUI.SetActive(true);
    }
}



