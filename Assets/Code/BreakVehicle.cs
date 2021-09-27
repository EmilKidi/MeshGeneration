using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakVehicle : MonoBehaviour
{

    public GameObject BrokenPropPrefab;
    public Material BrokenPropMaterial;
    public ParticleSystem[] particles;
    public float BreakingForce;

    private bool isSpawned = false;

    private void Start()
    {
        // Play particles 
        foreach (ParticleSystem ps in particles)
        {
            if (ps.isPlaying)
            {
                ps.Stop();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            gameObject.SetActive(false);
            //Vector3 dir = (transform.position - other.transform.position).normalized;
            GameObject BrokenPropPrefabCopy = Instantiate(BrokenPropPrefab, transform.position, transform.rotation, transform.parent);
            BrokenPropPrefabCopy.GetComponent<Renderer>().material = BrokenPropMaterial;
            isSpawned = true;

            // Play particles 
            foreach (ParticleSystem ps in particles)
            {
                ps.Play(true);
                ps.transform.SetParent(null, true);
            }
        }
    }

}
