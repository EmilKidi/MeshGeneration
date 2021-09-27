using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TrafficLight : MonoBehaviour
{
    public bool KeepGreen;
    public float InitialDelay = 2.0f;
    public Color Red;
    public Color Green;
    public Color Yellow;

    // 0 = green, 2 = yellow, 3 = red
    private int state;
    private int LastState;
    private Material[] Materials;

    void Start()
    {
        Materials = GetComponent<Renderer>().materials.Skip(2).ToArray();
       
        if (KeepGreen)
        {
            TurnOnBulb(0, Green, 0);
        }
        else
        {
            TurnOnBulb(2, Red, 1);
            InvokeRepeating("TrafficLightSwitch", InitialDelay, 5.0f);
        }
    }

    void TurnOnBulb(int MaterialIndex, Color Color, int nextState)
    {
        for (int i = 0; i < Materials.Length; i++)
        {
            if (MaterialIndex == i)
            {
                if (MaterialIndex == 1)
                {
                    Materials[LastState].color = Color.green;
                    Materials[LastState].EnableKeyword("_EMISSION");
                }
                Materials[i].color = Color;
                Materials[i].EnableKeyword("_EMISSION");
            }
            else
            {
                Materials[i].color = Color.black;
                Materials[i].DisableKeyword("_EMISSION");
            }
        }

        LastState = MaterialIndex;
        state = nextState;
    }

    void TrafficLightSwitch()
    {
        if (state == 0)
        {
            TurnOnBulb(state, Green, 1);
            return;
        }

        if (state == 1)
        {
            if (LastState == 2)
            {
                TurnOnBulb(state, Yellow, 0);
            }
            else
            {
                TurnOnBulb(state, Yellow, 2);
            }
            return;
        }

        if (state == 2)
        {
            TurnOnBulb(state, Red, 1);
            return;
        }
    }
}
