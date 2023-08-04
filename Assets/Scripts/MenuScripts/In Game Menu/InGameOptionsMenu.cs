using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameOptionsMenu : MonoBehaviour
{
    public GameObject optionsPanel;
    public float previousTimeScale;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OpenOptionsPanel();
        }
    }

    public void OpenOptionsPanel()
    {
        //previousTimeScale = Time.timeScale;
        Time.timeScale = 0f;
        optionsPanel.SetActive(true);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}
