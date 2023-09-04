using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class BWMainMenuScript : MonoBehaviour
{
    //menu panel
    public GameObject mainMenuPanel;
    public GameObject optionsPanel;
    public GameObject audioPanel;

    //audio references
    public Slider themeSlider, sfxSlider;
    private float sfxValue, themeValue;

    public AudioSource themeAudio;
    public AudioSource[] sfxAudio;

    public InputManagerSingleton inputManagerSingleton;

    private void Awake()
    {
        inputManagerSingleton = InputManagerSingleton.Instance;
    }

    private void Start()
    {

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


    //MainMenu Methods

    public void PlayGame()
    {
        SceneManager.LoadScene("BWMainScene");
    }

    public void ToggleOptionsPanel()
    {
        optionsPanel.SetActive(!optionsPanel.activeSelf);
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }


    //OptionsMenu Methods

    public void OpenAudioSettings()
    {
        audioPanel.SetActive(true);
    }

    public void BackToMainMenu()
    {
        mainMenuPanel.SetActive(true);
        optionsPanel.SetActive(false);
        audioPanel.SetActive(false);
    }


    //AudioMenu Methods

    public void UpdateSound()
    {
        themeAudio.volume = themeSlider.value;

        for (int i = 0; i < sfxAudio.Length; i++)
        {
            sfxAudio[i].volume = sfxSlider.value;
        }
    }

    public void BackToOptionsPanel()
    {
        audioPanel.SetActive(false);
        optionsPanel.SetActive(true);
    }
}
