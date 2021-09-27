using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drive2w : MonoBehaviour
{
    public GameObject Body;
    public GameObject mesh;
    public GameObject Joystick;
    public float torque = 5000;
    public float maxSteerAngle = 15;
    public float breakingForceFactor = 0.99f;
    public SpeedCurve SpeedCurveSettings;
    public WheelCollider[] WCs;
    public GameObject[] wheels;
    private Rigidbody rb;
    private float a, s;
    private Vector3 joystickInitPos;
    private RectTransform joystickInitTransform;

    [System.Serializable]
    public class SpeedCurve
    {
        public AnimationCurve speedCurve;
        public AnimationCurve breakCurve;
        public float speedCurveLenght = 40;
    }

    private void Start()
    {
        rb = Body.GetComponent<Rigidbody>();
        joystickInitTransform = Joystick.GetComponent<RectTransform>();
        joystickInitPos = joystickInitTransform.localPosition;
    }

    void Update()
    {
        a = Input.GetAxis("Vertical");
        s = Input.GetAxis("Horizontal");

        // tak carl
        //a = 2 * (joystickInitTransform.localPosition.y - (joystickInitPos.y - 500)) / ((joystickInitPos.y + 500) - (joystickInitPos.y - 500)) - 1;
        //s = 2 * (joystickInitTransform.localPosition.x - (joystickInitPos.x - 500)) / ((joystickInitPos.x + 500) - (joystickInitPos.x - 500)) - 1;

        //Go(a, s);
        BrakeLight(a);
        ResetBike();
    }

    private void FixedUpdate()
    {
        Go(a, s);
    }

    public void Go(float accel, float steer)
    {
        accel = Mathf.Clamp(accel, -1, 1);
        steer = Mathf.Clamp(steer, -1, 1) * maxSteerAngle;
        float thrustTorque = Mathf.Clamp(accel * torque, -1000, accel * torque);

        // Rotate bike when Steering.
        Vector3 desiredRotation = Body.transform.rotation.eulerAngles;
        desiredRotation.z = -steer / 2;
        Body.transform.rotation = Quaternion.RotateTowards(Body.transform.rotation, Quaternion.Euler(desiredRotation), 60 * Time.deltaTime);

        // Calculate slipping when breaking.
        WheelFrictionCurve fFrictionF = WCs[0].forwardFriction;
        WheelFrictionCurve sFrictionF = WCs[0].sidewaysFriction;
        WheelFrictionCurve fFrictionR = WCs[1].forwardFriction;
        WheelFrictionCurve sFrictionR = WCs[1].sidewaysFriction;
        float extrenumSlipFactor;
        float stiffnessFactor;

        if (accel <= 0 && rb.velocity.magnitude > 10)
        {
            extrenumSlipFactor  = Mathf.Abs(accel) * 15;
            stiffnessFactor     = Mathf.Abs(accel) * 4;
        }
        else
        {
            extrenumSlipFactor  = sFrictionF.extremumSlip - accel / 100;
            stiffnessFactor     = sFrictionF.stiffness - (accel / 100);
        }

        if (accel > 0)
        {
            thrustTorque = thrustTorque + (SpeedCurveSettings.speedCurve.Evaluate(rb.velocity.magnitude / SpeedCurveSettings.speedCurveLenght) * (torque / 2));
        }
        else
        {
            steer = steer * 1.2f;
            rb.velocity = rb.velocity * (SpeedCurveSettings.breakCurve.Evaluate(rb.velocity.magnitude / SpeedCurveSettings.speedCurveLenght));
        }

        sFrictionF.stiffness    = Mathf.Clamp(stiffnessFactor, 2f, 4f);
        sFrictionF.extremumSlip = Mathf.Clamp(extrenumSlipFactor, 4f, 15f);

        for (int i = 0; i < 2; i++)
        {
            if (i == 0)
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

        WCs[0].forwardFriction = fFrictionF;
        WCs[0].sidewaysFriction = sFrictionF;
        WCs[1].forwardFriction = fFrictionR;
        WCs[1].sidewaysFriction = sFrictionR;
    }

    private void ResetBike()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Body.transform.localPosition = new Vector3(-0.5f, 0.79f, -1.42f);
            Body.transform.localRotation = Quaternion.Euler(0, 180, 0);
            Body.transform.rotation = Quaternion.Euler(0, 0, 0);
            rb.velocity = Vector3.zero;
            for (int i = 0; i < 2; i++)
            {
                WCs[i].motorTorque = 0;
            }
        }
    }

    private void BrakeLight(float accel)
    {
        float intensity = (accel < 0) ? 0.015f : 0.003f;
        Color color = new Color(215 * intensity, 215 * intensity, 215 * intensity);
        mesh.GetComponent<Renderer>().materials[1].SetColor("_EmissionColor", color);
    }
}
