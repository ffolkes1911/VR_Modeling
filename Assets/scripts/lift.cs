using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

public class lift : MonoBehaviour
{
    public float max_height = 7;
    public float speed = 0.8f;
    private bool lift_enabled = false;
    private bool lift_reset = false;
    private float initial_height ;
    private int col_count = 0;

    void Start() {
        initial_height = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (lift_enabled){
            var new_pos = gameObject.transform.position + new Vector3(0, speed, 0) * Time.deltaTime;
            Debug.Log("current pos" + new_pos);
            if (new_pos[1] > max_height)
            {
                new_pos[1] = max_height;
                lift_reset = true;
            }
            gameObject.transform.position = new_pos;
        }
        else if (lift_reset)
        {
            var new_pos = gameObject.transform.position + new Vector3(0, -speed, 0) * Time.deltaTime;
            if (new_pos[1] < initial_height)
            {
                new_pos[1] = initial_height;
                lift_reset = false;
            }
            gameObject.transform.position = new_pos;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        lift_enabled = true;
        col_count += 1;
    }

    private void OnTriggerExit(Collider other)
    {
        col_count -= 1;
        if (col_count <= 0)
        {
            lift_enabled = false;
            lift_reset = true;
        }
    }
}
