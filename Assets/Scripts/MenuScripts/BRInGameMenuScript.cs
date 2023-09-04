using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class BRInGameMenuScript : MonoBehaviour
{
    [SerializeField] private GameObject igOptionsPanel;
    [SerializeField] private GameObject igAudioPanel;
    [SerializeField] private GameObject igWeatherPanel;

    //AudioReferences

    private static readonly string FirstPlay = "BRFirstPlay";
    private static readonly string ThemePref = "BRThemePref";
    private static readonly string SFXPref = "BRSFXPref";

    private int firstPlayInt;
    public Slider themeSlider, sfxSlider;
    private float sfxValue, themeValue;

    public AudioSource themeAudio;
    public AudioSource[] sfxAudio;


    //WeatherReferences

    public Material daySkybox;
    public Material sunsetSkybox;
    public Material nightSkybox;

    public InputActionAsset Actions;

    private void Awake()
    {
        Actions.FindActionMap("UI").FindAction("Click").performed += Click_performed;
    }

    private void Start()
    {
        /*
        firstPlayInt = PlayerPrefs.GetInt(FirstPlay);

        if (firstPlayInt == 0)
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
        }*/
    }

    private void Update()
    {
        
    }
    
    private void OnEnable()
    {
        Actions.FindActionMap("Player").Disable();
        Actions.FindActionMap("UI").Enable();
    }

    private void OnDisable()
    {
        Actions.FindActionMap("UI").Disable();
        Actions.FindActionMap("Player").Enable();
    }

    private void Click_performed(InputAction.CallbackContext context)
    {

    }


    public void OpenAudioPanel()
    {
        igAudioPanel.SetActive(true);

        igOptionsPanel.SetActive(false);
    }

    public void OpenWeatherPanel()
    {
        igWeatherPanel.SetActive(true);

        igOptionsPanel.SetActive(false);
    }


    //AudioMethods

        /*
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
    }*/

    public void UpdateSound()
    {
        themeAudio.volume = themeSlider.value;

        for (int i = 0; i < sfxAudio.Length; i++)
        {
            sfxAudio[i].volume = sfxSlider.value;
        }
    }


    //Weathermethods

    public void SetDaySkybox()
    {
        RenderSettings.skybox = daySkybox;
    }

    public void SetSunsetSkybox()
    {
        RenderSettings.skybox = sunsetSkybox;
    }

    public void SetNightSkybox()
    {
        RenderSettings.skybox = nightSkybox;
    }


    //OtherMethods

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        igOptionsPanel.SetActive(false);

		Actions.FindActionMap("UI").Disable();
		Actions.FindActionMap("Player").Enable();

		Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void BackToOptionsPanel()
    {
        igWeatherPanel.SetActive(false);
        igAudioPanel.SetActive(false);
        igOptionsPanel.SetActive(true);
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("BRMainMenu");
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
