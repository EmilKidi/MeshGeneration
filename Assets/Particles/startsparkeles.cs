using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class startsparkeles : MonoBehaviour
{
    private ParticleSystem ps;

    // Start is called before the first frame update
    void Start()
    {
        ps = GetComponent<ParticleSystem>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            GetComponent<ParticleSystem>().Play();
            Debug.Log("hit playing");
        }
            Debug.Log("hit playing");
    }
}
