using System.Collections;
using UnityEngine;

public class joystick_reactor : MonoBehaviour
{
    public joystick_press_watcher watcher;
    public bool IsPressed = false; // used to display button state in the Unity Inspector window
    public ui_controller ui_Controller;

    void Start()
    {
        watcher.primaryButtonPress.AddListener(onPrimaryButtonEvent);
    }

    public void onPrimaryButtonEvent(bool pressed)
    {
        IsPressed = pressed;
        if (pressed)
        {
            if (ui_Controller.GamePaused)
            {
                ui_Controller.ResumeGame();
            }
            else
            {
                ui_Controller.PauseGame();
            }
        }
    }
}