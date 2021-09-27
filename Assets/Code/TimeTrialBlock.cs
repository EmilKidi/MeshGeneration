using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeTrialBlock : MonoBehaviour
{
    public GameObject isStart;
    public GameObject isStop;
    public Rigidbody rb;
    public Vector3 size;
    bool counting = false;
    bool finished = false;

    public Text timerText;
    public Text counterText;
    private float miliCount;
    private float secondsCount;
    private int minuteCount;
    private string miliCountString = "";

    public void UpdateTimerUI()
    {
        secondsCount += Time.deltaTime;
        miliCount = Time.deltaTime * 1000;
        counterText.text = minuteCount + "m:" + (int)secondsCount + "s:" + miliCountString + "ms";

        if (secondsCount >= 60)
        {
            minuteCount++;
            secondsCount = 0;
        }
        else if (miliCount >= 1000)
        {
            miliCount = 0;
        }

        if (miliCount.ToString().Replace(".", "").Length > 3)
        {
            miliCountString = miliCount.ToString().Replace(".","").Substring(0, 3);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!counting)
        {
            Collider[] hitCollidersStart = Physics.OverlapBox(isStart.transform.position, size);
            foreach (var hitCollider in hitCollidersStart)
            {
                if (hitCollider.name == rb.name)
                {
                    counting = true;
                    counterText.gameObject.SetActive(true);
                }
            }
        }

        if (counting && !finished)
        {
            Collider[] hitCollidersStop = Physics.OverlapBox(isStop.transform.position, size);
            foreach (var hitCollider in hitCollidersStop)
            {
                if (hitCollider.name == rb.name)
                {
                    counting = false;
                    finished = true;
                    counterText.color = new Color(156, 238, 104);
                }
            }
        }

        if (counting && !finished)
        {
            UpdateTimerUI();
        }
    }
}
