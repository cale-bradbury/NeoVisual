using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XInputController : MonoBehaviour {

    public string[] buttons = new string[] { "A", "B", "X", "Y" };
    public string[] axis = new string[] { "XAxis", "YAxis", "RTrigger", "LTrigger" };

    public string eventPrefix = "k";

    float[] values;
    float[] lastValue;
    float[] lastValue2;

    // Use this for initialization
    void OnEnable ()
    {
        values = new float[axis.Length];
        lastValue = new float[axis.Length];
        for (int i = 0; i < axis.Length; i++)
        {
            values[i] = lastValue[i] = 0;
        }
    }
	
	// Update is called once per frame
	void Update () {
        for (int i = 0; i < axis.Length; i++)
        {
            values[i] = Input.GetAxis(axis[i]);
        }

        for (int i = 0; i < buttons.Length; i++)
        {
            if (Input.GetButton(buttons[i]))
            {
                for (int j = 0; j < axis.Length; j++)
                {
                    if (values[j] != lastValue[j])
                    {
                        Messenger.Broadcast<float>(eventPrefix + i + "-" + j, j < 2 ? values[j] * .5f + .5f : values[j]);
                    }
                }
            }
        }
        for (int i = 0; i < axis.Length; i++)
        {
            lastValue[i] = values[i];
        }

    }
}
