using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UiInput : MonoBehaviour
{
    public GameObject panel1;
    public GameObject panel2;
    public GameObject panel3;

    private UiInputManager uiActions;

    private void Awake()
    {
        uiActions = new UiInputManager();
    }

    private void OnEnable()
    {
        uiActions.Enable();
        uiActions.UI.Click.performed += Click_performed;
    }

    private void OnDisable()
    {
        uiActions.Disable();
        uiActions.UI.Click.performed -= Click_performed;
    }

    private void Click_performed(InputAction.CallbackContext context)
    {
        
    }

    public void ActivatePanel1(GameObject panel)
    {
        
        panel1.SetActive(true);
        panel2.SetActive(false);
        panel3.SetActive(false);
    }

    public void ActivatePanel2(GameObject panel)
    {

        panel1.SetActive(false);
        panel2.SetActive(true);
        panel3.SetActive(false);
    }

    public void ActivatePanel3(GameObject panel)
    {

        panel1.SetActive(false);
        panel2.SetActive(false);
        panel3.SetActive(true);
    }
}
