using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class BWInGameMenuScript : MonoBehaviour
{
    [SerializeField] private GameObject igOptionsPanel;
    [SerializeField] private GameObject igAudioPanel;
    [SerializeField] private GameObject igWeatherPanel;

    //AudioReferences

    private static readonly string FirstPlay = "BWFirstPlay";
    private static readonly string ThemePref = "BWThemePref";
    private static readonly string SFXPref = "BWSFXPref";

    private int firstPlayInt;
    public Slider themeSlider, sfxSlider;
    private float sfxValue, themeValue;

	public AudioSource themeAudio;
    public AudioSource[] sfxAudio;


    //WeatherReferences

    public Material daySkybox;
    public Material sunsetSkybox;
    public Material nightSkybox;


    public InputManagerSingleton inputManagerSingleton;

    private void Awake()
    {
        inputManagerSingleton = InputManagerSingleton.Instance;
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
        inputManagerSingleton.Actions.Player.Disable();
        inputManagerSingleton.Actions.UI.Enable();
        inputManagerSingleton.Actions.UI.Click.performed += Click_performed;
    }

    private void OnDisable()
    {
        inputManagerSingleton.Actions.UI.Disable();
        inputManagerSingleton.Actions.Player.Enable();
        inputManagerSingleton.Actions.UI.Click.performed -= Click_performed;
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

        inputManagerSingleton.Actions.Player.Enable();
        inputManagerSingleton.Actions.UI.Disable();

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
        SceneManager.LoadScene("BWMainMenu");
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
