using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxChanger : MonoBehaviour
{
    public GameObject optionsInGamePanel;
    public GameObject weatherPanel;

    public Material daySkybox;
	public Material sunsetSkybox;
	public Material nightSkybox;

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

    public void BackToOptionsPanel()
    {
        weatherPanel.SetActive(false);
        optionsInGamePanel.SetActive(true);
    }
}
