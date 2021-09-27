using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisasembleProp : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            Vector3 dir = (transform.position - other.transform.position).normalized;
            transform.GetComponent<BoxCollider>().isTrigger = false;
            transform.GetComponent<Rigidbody>().isKinematic = false;

            // Push Debris
            foreach (Transform child in transform)
            {
                if (child.GetComponent<Rigidbody>() != null)
                {
                    Rigidbody rb = child.GetComponent<Rigidbody>();
                    rb.AddForce(dir, ForceMode.Impulse);
                    rb.isKinematic = false;
                }
            }

        }
    }
}
