using UnityEngine;

public class ThemeAudioController : MonoBehaviour
{
	public AudioSource themeAudio;

	private void Start()
	{
		themeAudio.Play();

        GameOverManager gameOverManager = FindObjectOfType<GameOverManager>();
        gameOverManager.OnGameEnd += HandleGameEnd;
	}

    private void HandleGameEnd(AudioClip clipaudio)
    {
        themeAudio.Stop();
    }
}
