using UnityEngine;
using UnityEngine.UI;

public class AudioMenu : MonoBehaviour
{
    public Slider musicVolumeSlider;
    public Slider sfxVolumeSlider;
    public Button musicOnButton;
    public Button musicOffButton;
    public Button sfxOnButton;
    public Button sfxOffButton;
    public Button backButton;
    public GameObject optionsPanel;
    public GameObject audioPanel;

    public AudioSource musicAudioSource;
    public AudioSource sfxAudioSource;


    private void Start()
    {
        musicVolumeSlider.value = musicAudioSource.volume;
        sfxVolumeSlider.value = sfxAudioSource.volume;
        UpdateMusicButtons();
        UpdateSFXButtons();
    }

    public void SetMusicVolume(float volume)
    {
        musicAudioSource.volume = volume;

        UpdateMusicButtons();
    }

    public void SetSFXVolume(float volume)
    {
        sfxAudioSource.volume = volume;

        UpdateSFXButtons();
    }

    public void TurnMusicOn()
    {
        SetMusicVolume(1.0f);
    }

    public void TurnMusicOff()
    {
        SetMusicVolume(0.0f);
    }

    public void TurnSFXOn()
    {
        SetSFXVolume(1.0f);
    }

    public void TurnSFXOff()
    {
        SetSFXVolume(0.0f);
    }

    private void UpdateMusicButtons()
    {
        float musicVolume = musicVolumeSlider.value;
        musicOnButton.gameObject.SetActive(musicVolume > 0.0f);
        musicOffButton.gameObject.SetActive(musicVolume <= 0.0f);
    }

    private void UpdateSFXButtons()
    {
        float sfxVolume = sfxVolumeSlider.value;
        sfxOnButton.gameObject.SetActive(sfxVolume > 0.0f);
        sfxOffButton.gameObject.SetActive(sfxVolume <= 0.0f);
    }


    public void BackToOptionsPanel()
    {
        audioPanel.SetActive(false);
        optionsPanel.SetActive(true);
    }

}
