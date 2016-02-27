using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ccCreateGroup))]
public class ccGroupFFT : MonoBehaviour {

	public CCReflectFloat output;
	public int minFFT;
	public int maxFFT;
	public float minValue;
	public float maxValue;
	ccCreateGroup group;
	System.Type type;

	// Use this for initialization
	void OnEnable () {
		group = GetComponent<ccCreateGroup> ();
		type = output.obj.GetType ();
	}
	
	// Update is called once per frame
	void Update () {
		for(int i = 0; i<group.all.Count;i++){
			output.obj = group.all[i].GetComponent(type);
			output.SetValue(Utils.Lerp (minValue,maxValue,ccAudioController.FFT[ (int)Mathf.Lerp(minFFT,maxFFT, (float)i/group.all.Count)]));
		}
	}
}
