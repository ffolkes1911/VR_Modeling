using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class trigger_light_switch : MonoBehaviour
{
    private Light[] lights;
    private ui_controller ui_Controller;
    private float t_next_action = 0f;
    private float cooldown = 1.0f;
    private bool switch_on = false;

    // Start is called before the first frame update
    void Start()
    {
        lights = GameObject.FindObjectsOfType<Light>();
        ui_Controller = GameObject.FindObjectOfType<ui_controller>();
    }


    private void OnCollisionEnter(Collision other)
    {
        var t_now = Time.time;
        if (t_now > t_next_action)
        {
            ui_Controller.ChangeScore(50, 0.1f);
            t_next_action = t_now + cooldown;
            foreach (Light light in lights)
            {
                light.enabled = switch_on;
            }
            switch_on = !switch_on;
        }
    }
}
