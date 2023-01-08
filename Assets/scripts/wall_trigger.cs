using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wall_trigger : MonoBehaviour
{
    private wall_controller wall_Controller;
    private bool triggered = false;
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
    }

    private void OnTriggerEnter(Collider other)
    {
        if (triggered)
            return;

        var player_box = other.GetComponent<PlayerBox>();
        Debug.Log(other);
        if (player_box == null)
            return;

        triggered = true;
        wall_Controller.hit_fail();
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
        triggered = false;
        //StartCoroutine(FadeObject(true));
    }
}
