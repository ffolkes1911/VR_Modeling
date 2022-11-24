using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trigger_change_color : MonoBehaviour
{
    private int col_count = 0;
    private Renderer cubeRenderer;
    private Color initial_color;
    private ui_controller ui_Controller;

    // Start is called before the first frame update
    void Start()
    {
        cubeRenderer = gameObject.GetComponent<Renderer>();
        initial_color = cubeRenderer.material.color;
        ui_Controller = GameObject.FindObjectOfType<ui_controller>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        col_count += 1;
        if (col_count == 1)
        {
            cubeRenderer.material.SetColor("_Color", Color.red);
            ui_Controller.ChangeScore(10, 0.05f);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        col_count -= 1;
        if (col_count <= 0)
        {
            cubeRenderer.material.SetColor("_Color", initial_color);
        }
    }
}
