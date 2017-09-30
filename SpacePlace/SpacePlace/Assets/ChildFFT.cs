using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildFFT : MonoBehaviour {
    
    public CampReflectFloat output;
    public int minFFT;
    public int maxFFT;
    public float minValue;
    public float maxValue;
    System.Type type;

    // Use this for initialization
    void OnEnable()
    {
        type = output.obj.GetType();
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            output.obj = transform.GetChild(i).GetComponent(type);
            output.SetValue(Utils.Lerp(minValue, maxValue, CampAudioController.FFT[(int)Mathf.Lerp(minFFT, maxFFT, (float)i / transform.childCount)]));
        }
    }
}
