using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CampCreateGroup))]
public class CampGroupFFT : MonoBehaviour {

	public CampReflectFloat output;
	public int minFFT;
	public int maxFFT;
	public float minValue;
	public float maxValue;
	CampCreateGroup group;
	System.Type type;

	// Use this for initialization
	void OnEnable () {
		group = GetComponent<CampCreateGroup> ();
		type = output.obj.GetType ();
	}
	
	// Update is called once per frame
	void Update () {
		for(int i = 0; i<group.all.Count;i++){
			output.obj = group.all[i].GetComponent(type);
			output.SetValue(Utils.Lerp (minValue,maxValue,CampAudioController.FFT[ (int)Mathf.Lerp(minFFT,maxFFT, (float)i/group.all.Count)]));
		}
	}
}
