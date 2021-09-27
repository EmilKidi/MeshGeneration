using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drive : MonoBehaviour
{

    public WheelCollider[] WCs;
    public GameObject[] wheels;
    public float torque = 200;
    public float maxSteerAngle = 30;

    // Update is called once per frame
    void Update()
    {
        /*float a = -Input.GetAxis("Vertical");
        float s = Input.GetAxis("Horizontal");
        Go(a, s);*/
    }

    public void Go(float accel, float steer)
    {
        accel = Mathf.Clamp(accel, -1, 1);
        steer = Mathf.Clamp(steer, -1, 1) * maxSteerAngle;
        float thrustTorque = accel * torque;

        for(int i = 0; i < 4; i++)
        {
            if (i < 2)
            {
                WCs[i].motorTorque = thrustTorque;
                WCs[i].steerAngle = steer;
            }

            Quaternion quat;
            Vector3 position;
            WCs[i].GetWorldPose(out position, out quat);
            wheels[i].transform.position = position;
            wheels[i].transform.rotation = quat;
        }
    }
}
