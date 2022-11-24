using UnityEngine;

public class score_trigger : MonoBehaviour
{
    private float score = 100;
    private bool triggered = false;
    // private main_controller mc = ... // find main controller

    //Upon collision add score
    private void OnTriggerEnter(Collider other)
    {
        if (triggered)
        {
            Debug.Log("Object already used");
        }
        else
        {
            triggered = true;
            // add score to main controller
            Debug.Log("Adding score " + score);
        }
    }
}

