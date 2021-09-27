using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakProp : MonoBehaviour
{
    public GameObject BrokenPropPrefab;
    public float BreakingForce;

    private bool isSpawned = false;
    private ParticleSystem[] particles;

    private void Start()
    {
        particles = BrokenPropPrefab.GetComponentsInChildren<ParticleSystem>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player" && isSpawned == false)
        {
            gameObject.SetActive(false);
            Vector3 dir = (transform.position - other.transform.position).normalized;
            GameObject BrokenPropPrefabCopy = Instantiate(BrokenPropPrefab, transform.position, transform.rotation, transform.parent);

            // Push Debris
            foreach (Transform child in BrokenPropPrefabCopy.transform)
            {
                if (child.GetComponent<Rigidbody>() != null)
                {
                    child.GetComponent<Rigidbody>().AddForce(dir, ForceMode.Impulse);
                }

                if (child.GetComponent<ParticleSystem>() != null)
                {
                    child.transform.rotation = Quaternion.Euler(dir);
                }
            }

            // Play particles 
            foreach (ParticleSystem ps in particles)
            {
                ps.Play(true);
            }

            isSpawned = true;
        }
    }
}
