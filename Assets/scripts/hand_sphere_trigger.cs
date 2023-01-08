using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class hand_sphere_trigger : MonoBehaviour
{
    private wall_controller wall_Controller;
    private bool triggered = false;
    public Type box_type;
    public Renderer rend;
    public Color default_color;
    public Collider col;

    // Start is called before the first frame update
    void Start()
    {
        wall_Controller = gameObject.GetComponentInParent<wall_controller>();
        rend = gameObject.GetComponent<Renderer>();
        default_color =
            new Color(rend.material.color.r, rend.material.color.g, rend.material.color.b, rend.material.color.a);
        col = gameObject.GetComponent<Collider>();
        rend.material.color =
            new Color(default_color.r, default_color.g, default_color.b, 0.0f);
        col.enabled = false;
        rend.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Hand Sphere HIT");
        if (triggered)
            return;

        var player_box = other.gameObject.GetComponent<PlayerBox>();
        if (player_box == null)
            return;

        triggered = true;
        if (player_box.box_type == box_type)
        {
            wall_Controller.hit_success();
        }
        else
        {
            wall_Controller.hit_neutral();
        }
    }

    public void Spawn()
    {
        triggered = false;
        col.enabled = true;
        rend.enabled = true;
    }

    public void Despawn()
    {
        triggered = true;
        col.enabled = false;
        rend.enabled = false;
    }


    public void Reset()
    {
    }
}
