using System;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;


public class BRInputDebug : MonoBehaviour
{
    public InputActionAsset Actions;

    [SerializeField] private TMP_Text playerText;
    [SerializeField] private TMP_Text uiText;

    private void Update()
    {
        playerText.text = Actions.FindActionMap("Player").enabled ? "Player" : "";
        uiText.text = Actions.FindActionMap("UI").enabled ? "UI" : "";
    }
}
