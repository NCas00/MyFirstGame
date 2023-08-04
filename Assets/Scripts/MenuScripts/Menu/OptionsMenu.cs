using UnityEngine;

public class OptionsMenu: MonoBehaviour
{
    public GameObject mainMenuPanel;   
    public GameObject optionsPanel;    
    public GameObject audioPanel;      
 

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
}
