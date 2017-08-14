using UnityEngine;
using System.Collections;

public class FFTScale : MonoBehaviour {

    public int fft = 3;
    public float min = 0;
    public float max = 1;

    // Update is called once per frame
    void Update()
    {
        transform.localScale = Vector3.one* Mathf.Lerp(min, max, CampAudioController.FFT[fft]);
    }
}
