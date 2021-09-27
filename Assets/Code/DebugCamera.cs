using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugCamera : MonoBehaviour
{

    public GameObject[] cams;
    int currentCam = 0;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) {
            cams[currentCam].SetActive(false);
            currentCam = currentCam + 1;
            currentCam = (currentCam == cams.Length) ? 0 : currentCam;
            cams[currentCam].SetActive(true);
        }
        
    }
}
