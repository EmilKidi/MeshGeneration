using UnityEngine;
using System.Collections;

public class CameraPlayer : MonoBehaviour
{

    public GameObject player;
    public float rotationTime;
    private Vector3 offset;

    void Start()
    {
        offset = new Vector3(-0.2f, 2.6f, -8.6f);
    }

    void FixedUpdate()
    {
        Vector3 desiredPosition = player.transform.position + -player.transform.forward * 8f;
        desiredPosition.x -= 0.3f;
        desiredPosition.y += 2f;
        transform.rotation = Quaternion.Lerp(transform.rotation, player.transform.rotation, Time.deltaTime * rotationTime);
        transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * rotationTime);
    }
}