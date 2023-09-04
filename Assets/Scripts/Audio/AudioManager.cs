using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    private static readonly string FirstPlay = "FirstPlay";
    private static readonly string ThemePref = "ThemePref";
    private static readonly string SFXPref = "SFXPref";

    private int firstPlayInt;
    public Slider sfxSlider, themeSlider;
    private float sfxValue, themeValue;
    public Button musicOnButton, musicOffButton, sfxOnButton, sfxOffButton;

    public AudioSource themeAudio;
    public AudioSource[] sfxAudio;

    void Start()
    {
        firstPlayInt = PlayerPrefs.GetInt(FirstPlay);

        if(firstPlayInt == 0)
        {
            themeValue = .25f;
            sfxValue = .75f;

            themeSlider.value = themeValue;
            sfxSlider.value = sfxValue;

            PlayerPrefs.SetFloat(ThemePref, themeValue);
            PlayerPrefs.SetFloat(SFXPref, sfxValue);

            PlayerPrefs.SetInt(FirstPlay, -1);
        }
        else
        {
            PlayerPrefs.GetFloat(ThemePref);
            themeSlider.value = themeValue;

            PlayerPrefs.GetFloat(SFXPref);
            sfxSlider.value = sfxValue;
        }
    }

    public void SaveSoundSettings()
    {
        PlayerPrefs.SetFloat(ThemePref, themeSlider.value);
        PlayerPrefs.SetFloat(SFXPref, sfxSlider.value);
    }

    private void OnApplicationFocus(bool focus)
    {
        if (!focus)
        {
            SaveSoundSettings();
        }
    }

    public void UpdateSound()
    {
        themeAudio.volume = themeSlider.value;

        for(int i = 0; i < sfxAudio.Length; i++)
        {
            sfxAudio[i].volume = sfxSlider.value;
        }

    }
}
