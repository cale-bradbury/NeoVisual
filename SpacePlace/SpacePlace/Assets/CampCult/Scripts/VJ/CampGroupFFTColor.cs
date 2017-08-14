using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CampCreateGroup))]
public class CampGroupFFTColor : MonoBehaviour {

    public CampReflectColor output;
    public int minFFT;
    public int maxFFT;
    public Gradient gradient;
    CampCreateGroup group;
    System.Type type;

    // Use this for initialization
    void OnEnable()
    {
        group = GetComponent<CampCreateGroup>();
        type = output.obj.GetType();
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < group.all.Count; i++)
        {
            output.obj = group.all[i].GetComponent(type);
            output.SetValue(gradient.Evaluate(CampAudioController.FFT[(int)Mathf.Lerp(minFFT, maxFFT, (float)i / group.all.Count)]));
        }
    }
}
