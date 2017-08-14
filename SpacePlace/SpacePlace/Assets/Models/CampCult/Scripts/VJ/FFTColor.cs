using UnityEngine;
using System.Collections;


public class FFTColor : MonoBehaviour {

	public int fft = 60;
	public float mul = 1;
	public Color min = Color.black;
	public Color max = Color.white;
	public CampReflectColor field;
	
	// Update is called once per frame
	void Update () {
		field.SetValue (Color.Lerp(min,max,CampAudioController.FFT [fft]*mul));
	}
}
