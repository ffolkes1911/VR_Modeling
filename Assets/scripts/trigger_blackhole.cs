using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trigger_blackhole : MonoBehaviour
{
    private List<GameObject> objects = new List<GameObject>();
    public float pullForce = 1000;
    private float sphere_radius;

    // Start is called before the first frame update
    void Start()
    {
        sphere_radius = gameObject.GetComponent<SphereCollider>().radius;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        foreach (GameObject col in objects)
        {
            // calculate direction from target to me
            Vector3 forceDirection = transform.position - col.transform.position;
            var distance = Mathf.Pow(Mathf.Max(
                sphere_radius - Vector3.Distance(transform.position, col.transform.position), 0
                ), 2);

            // apply force on target towards me
            var rbody = col.GetComponent<Rigidbody>();
            if (rbody != null)
            {
                rbody.AddForce(forceDirection * pullForce * Time.fixedDeltaTime);
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!objects.Contains(other.gameObject))
        {
            objects.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        objects.Remove(other.gameObject);
    }

}
