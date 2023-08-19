using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class GameInput : MonoBehaviour
{
    [SerializeField] private PlayerController player;

    [SerializeField] private Joystick joystick;

    [SerializeField] private Button cut_Button;
    [SerializeField] private Button spawn_Button;

    [SerializeField] private Button info_Button;
    private bool info_UI_Display;
    [SerializeField] private GameObject info_UI;

    //public event EventHandler OnInteractAction;

    private PlayerInputActions inputActions;
    private void Awake()
    {
        inputActions = new PlayerInputActions();
        inputActions.Player.Enable();

        inputActions.Player.Interact.performed += Interact_performed;
        inputActions.Player.Cutting.performed += Cutting_performed;
    }


    private void Start()
    {
        /*if(SystemInfo.deviceType == DeviceType.Handheld)   // to check if which device we are using
        {
            joystick.gameObject.SetActive(true);
        }*/

        cut_Button.onClick.AddListener(Cut);
        spawn_Button.onClick.AddListener(Spawn);

        info_UI_Display = false;
        info_Button.onClick.AddListener(Info);
    }

    private void Cutting_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        Debug.Log("C pressed");

        player.SelectedCounterClass()?.Interact();
    }

    private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        Debug.Log("E pressed");
        //OnInteractAction?.Invoke(this, EventArgs.Empty);  // Alternate method by using C# events
        player.SelectedCounterClass()?.Interact();
    }

    private void Cut()
    {
        player.SelectedCounterClass()?.Interact();
    }

    private void Spawn()
    {
        player.SelectedCounterClass()?.Interact();
    }

    private void Info()
    {
        if(info_UI_Display == false)
        {
            info_UI.SetActive(true);
            info_UI_Display = true;
        }
        else
        {
            info_UI.SetActive(false);
            info_UI_Display = false;
        }
    }

    public Vector2 GetMovementVectorNormalised()
    {
        //Vector2 inputVector = inputActions.Player.Move.ReadValue<Vector2>();   // using the input system(keys)

        float x = joystick.Horizontal; 
        float y = joystick.Vertical;

        Vector2 inputVector = new Vector2(x, y);  // using the touch input(joystick)

        inputVector = inputVector.normalized;

        return inputVector;
    }   
}
