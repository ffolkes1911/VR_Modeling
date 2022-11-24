using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trigger_destroy : MonoBehaviour
{
    private ui_controller ui_Controller;
    // Start is called before the first frame update
    void Start()
    {
        ui_Controller = GameObject.FindObjectOfType<ui_controller>();
    }

    private void OnTriggerEnter(Collider other)
    {
        ui_Controller.ChangeScore(100, 0.25f);
        Destroy(other.gameObject);
    }
}
