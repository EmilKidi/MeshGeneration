using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{

    public Text speedText;
    public Rigidbody vehicleRigidbody;

    void Update()
    {
        speedText.text = UnityEngine.Mathf.Round(vehicleRigidbody.velocity.magnitude * 2).ToString();
    }
}
