using System;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;


public class BWInputDebug : MonoBehaviour
{
    public InputManagerSingleton inputManagerSingleton;

    [SerializeField] private TMP_Text playerText;
    [SerializeField] private TMP_Text uiText;

    private void Awake()
    {
        inputManagerSingleton = InputManagerSingleton.Instance;
    }

    private void Update()
    {
        playerText.text = inputManagerSingleton.Actions.Player.enabled ? "Player" : "";
        uiText.text = inputManagerSingleton.Actions.UI.enabled ? "UI" : "";
    }
}
