using UnityEngine;
using System.Collections;

public class FFTFloat : MonoBehaviour {

	public int fft = 3;
	public float min = 0;
	public float max = 1;
	public CampReflectFloat field;

	// Update is called once per frame
	void Update () {
		field.SetValue (min+(max-min)*CampAudioController.FFT [fft]);
	}
}
